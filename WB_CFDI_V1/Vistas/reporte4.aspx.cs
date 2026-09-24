using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngleSharp;
using AngleSharp.Css.Values;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ClosedXML.Excel;
using Dapper;
using EO.Pdf;
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class reporte4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-2));
            Response.Cache.SetNoStore();
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Pragma", "no-cache");

            Req req = (Req)HttpContext.Current.Session["req"];

            HttpContext.Current.Session["req"] = null;

            if (req != null)
            {

                EO.Pdf.Runtime.AddLicense(
                    "U/Ge6sUF6bua4/AAIr112Oj1y/Oy5+nOzcSIpdT1EaFZ7ekDHuio5cGz3Lhnp6ax2r11puX9F+6wtcAAHeOe6c3/Ee5Z2+UFELxbqbPD265rp7XKy7BrsbTB5a9pl8XezZ+v3PYEFO6ntKbCzZ+f4+X4Hrxbp6ax2r116u34GeCt7Pb26diRqNfH1vea3P3EA+2O4u319dmKydXO6Lto6u34GeCt7Pb26bto4+30EO2s3MJ14+30EO2s3MLNF+ic3PIEEMidtc2+AuCr3P6x3a9qsMDAF+ic3PIEEMidtcD2I++i6ekE7PN3qLbA3rBoqbTK5J9qqb7B27lpp6TS+Lto3PwBFA==");


                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    try
                    {


                        var emp = Extensions.cEmpresa();

                        string sql1 =
                                @"
SELECT
  c.FECHA AS FECHA_CONTRA_RECIBO
 ,a.ID
 ,a.USER_ID
 ,a.UUID
 --,a.XML
 ,a.RAZON_SOCIAL_EMISOR
 ,a.RFC_EMISOR
 ,a.TOTAL
 ,a.SUBTOTAL
 ,a.IMPUESTOS
 ,a.CONTRA_RECIBO_ID
 ,a.FECHA_RECEPCION
 ,a.FECHA_FACTURA
 ,a.SERIE
 ,a.FOLIO
 ,a.ESTATUS
 ,a.TIPO_COMPROBANTE
 ,a.NUMEFECTO
 ,a.FECHA_SALDADO
 ,a.TIENDA
 ,a.REVISADO
 ,a.NO_COMPRA
 ,a.FECHA_COMPRA
 ,a.OBSERVACION1
 ,a.FORMA_PAGO
 ,a.METODO_PAGO
 ,a.TOTAL_IMPUESTOS_RETENIDOS
 ,CFDI_ESTATUS.ESTATUS AS ESTATUS2
 ,c.FECHA_CIERRE
 ,c.HORA_CIERRE
 ,d.VALIDADORA
FROM dbo.CONTRA_RECIBOS_PSI_TMP b
INNER JOIN dbo.CFDI_XML_PSI a
  ON b.ID = a.CONTRA_RECIBO_ID
LEFT OUTER JOIN dbo.CONTRA_RECIBOS_PSI c
  ON a.CONTRA_RECIBO_ID = c.ID
INNER JOIN dbo.CFDI_ESTATUS
  ON a.ESTATUS = CFDI_ESTATUS.ID
LEFT OUTER JOIN dbo.CTR_USUARIO d
  ON d.RFC_PROVEEDOR = a.RFC_EMISOR
WHERE b.USUARIO = @USUARIO
                        ";



                        string sql2 =
                            @"

INSERT INTO CONTRA_RECIBOS_PSI_TMP
SELECT ID, @USUARIO FROM dbo.CONTRA_RECIBOS_PSI
WHERE FECHA
BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                        ";

                        //var _f1 = new DateTime(DateTime.Now.Year-1, 1, 1);
                        //var _f2 = DateTime.Now;
                        

                        //req.FECHA1 = _f1;
                        //req.FECHA2 = _f2;

                        //_f3 = req.FECHA2.AddDays(-1);


                        req.USUARIO = Extensions.GetUserName();
                        con.Execute("DELETE FROM CONTRA_RECIBOS_PSI_TMP WHERE USUARIO = @USUARIO", req);
                        con.Execute(sql2, req);

                        
                        var wb = new XLWorkbook();
                        var rs1 = con.Query<CFDI_XML>(sql1, req, commandTimeout: 6000).ToList();
                        rs1.RemoveAll(r => string.IsNullOrEmpty(r.VALIDADORA));

                        //********************
                        //_f3 = new DateTime(2023, 9, 13);
                        //********************

                        var _f1 = req.FECHA1;
                        var _f2 = req.FECHA2;
                        var _f3 = _f2.AddDays(-1);

                        var f1 = _f1.ToString("dd-") + _f1.ToString("MMM").Capitalize().Replace(".","")  + _f1.ToString("-yyyy");
                        var f2 = _f2.ToString("dd-") + _f2.ToString("MMM").Capitalize().Replace(".", "") + _f2.ToString("-yyyy");
                        var f3 = _f3.ToString("dd-") + _f3.ToString("MMM").Capitalize().Replace(".", "") + _f3.ToString("-yyyy");

                        //*************************************************************************************************/

                        var ic   = new Dictionary<string, ST>();
                        var ls1  = new List<REP>();
                        var uuid = new Hashtable();
                        List<Acumulado> acc = new List<Acumulado>();

                        var Validadores = rs1.GroupBy(g => g.VALIDADORA).OrderBy(o=>o.Key).ToDictionary(k => k.Key, l => l.ToList());

                        foreach (var kv1 in Validadores)
                        {
                            //var rfc = kv1.Value.GroupBy(g => g.RFC_EMISOR).ToDictionary(k => k.Key, l => l.ToList());

                            var nDay = _f3.Date.AddDays(-1).Date;

                            var lstPeriodo  =kv1.Value.Where(w => nDay >= w.FECHA_CONTRA_RECIBO.Value.Date).ToList();
                            var lstPeriodo2 = kv1.Value.Where(w => w.FECHA_CIERRE == null).ToList();


                            var lstAyer1 = kv1.Value.Where(w => w.FECHA_CONTRA_RECIBO.Value.Date == _f3.Date).ToList();
                            var lstAyer2 = kv1.Value.Where(w => w.FECHA_CIERRE != null &&  w.FECHA_CIERRE.Value.Date == _f3.Date).ToList();

                            var nuevos = lstAyer1.GroupBy(g => g.CONTRA_RECIBO_ID).Count();
                            
                            var ctrPeriodo = lstPeriodo.GroupBy(g=>g.CONTRA_RECIBO_ID).ToDictionary(k=>k.Key,l=>l.ToList());
                            var ctrAyer1= lstAyer1.GroupBy(g=>g.CONTRA_RECIBO_ID).ToDictionary(k=>k.Key,l=>l.ToList());
                            var ctrAyer2= lstAyer2.GroupBy(g=>g.CONTRA_RECIBO_ID).ToDictionary(k=>k.Key,l=>l.ToList());


                            int    ignoradosPeriodo   = 0;
                            int    incompletosPeriodo = 0;
                            int    validados          = ctrAyer2.Count;

                            double total              = 0;
                            int    xmlPorValidar      = 0;
                            
                            double totalValidado      = 0;
                            int    xmlValidado        = 0;


                            double xmltotalFinalDia = 0;
                            int    xmlFinalDia = 0;

                            //**************************************************************************************************


                            foreach (var kv3 in ctrAyer2)
                            {


                                foreach (var xml in kv3.Value)
                                {
                                    if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                    {
                                        if (xml.TOTAL != null) xml.TOTAL         = xml.TOTAL * -1;
                                        if (xml.SUBTOTAL != null) xml.SUBTOTAL   = xml.SUBTOTAL * -1;
                                        if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                                    }


                                    if (!uuid.ContainsKey(xml.UUID))
                                    {
                                        uuid.Add(xml.UUID, "");
                                        totalValidado += xml.TOTAL.Dbl();
                                        xmlValidado++;
                                    }
                                    else
                                    {

                                    }

                                }

                            }



                            //**************************************************************************************************


                            foreach (var xml in lstPeriodo2)
                            {



                                    if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                    {
                                        if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                                        if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                                        if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                                    }


                                    if (!uuid.ContainsKey(xml.UUID))
                                    {
                                        uuid.Add(xml.UUID, "");
                                        xmltotalFinalDia += xml.TOTAL.Dbl();
                                        xmlFinalDia++;
                                    }
                                    else
                                    {

                                    }

                            }



                            //**************************************************************************************************


                            int ignoradosAyer = 0;
                            int incompletosAyer = 0;
                            int validadosAyer = 0;

                            foreach (var kv3 in ctrAyer1)
                            {
                                var v1 = kv3.Value.Count;
                                var v2 = kv3.Value.Where(w => w.ESTATUS == 1).Count();
                                //var v3 = kv3.Value.Where(w => w.ESTATUS == 2 && w.FECHA_CIERRE == null).Count();

                                var v4 = kv3.Value.Where(w => w.FECHA_CIERRE != null && w.FECHA_CIERRE.Value.Date == _f3.Date && w.FECHA_CONTRA_RECIBO.Value.Date == _f3.Date).Count();

                                if (v2 == v1)
                                {
                                    ignoradosAyer++;


                                    //foreach (var xml in kv3.Value)
                                    //{
                                    //    if (!uuid.ContainsKey(xml.UUID))
                                    //    {
                                    //        xmlPorValidar++;
                                    //    }
                                    //    else
                                    //    {

                                    //    }
                                    //}
                                }
                                else
                                {

                                    if (v4 > 0)
                                    {
                                        validadosAyer++;
                                    }
                                    else
                                    {
                                        //if (v3 > 0)
                                        {
                                            incompletosAyer++;

                                            //foreach (var xml in kv3.Value)
                                            //{
                                            //    if (!uuid.ContainsKey(xml.UUID))
                                            //    {
                                            //        xmlPorValidar++;
                                            //    }
                                            //    else
                                            //    {

                                            //    }
                                            //}
                                        }
                                    }
                                }

                            }



                            foreach (var kv3 in ctrPeriodo)
                            {
                                var v1 = kv3.Value.Count;
                                var v2 = kv3.Value.Where(w => w.ESTATUS == 1).Count();
                                var v3 = kv3.Value.Where(w => w.ESTATUS == 2 && w.FECHA_CIERRE == null).Count();
                                //var v4 = kv3.Value.Where(w => w.FECHA_CIERRE == null).ToList();

                                //foreach (var xml in v4)
                                //{


                                //        if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                //        {
                                //            if (xml.TOTAL != null) xml.TOTAL         = xml.TOTAL * -1;
                                //            if (xml.SUBTOTAL != null) xml.SUBTOTAL   = xml.SUBTOTAL * -1;
                                //            if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                                //        }


                                //        if (!uuid.ContainsKey(xml.UUID))
                                //        {
                                //            uuid.Add(xml.UUID, "");
                                //            xmltotalFinalDia += xml.TOTAL.Dbl();
                                //            xmlFinalDia++;
                                //        }
                                //        else
                                //        {

                                //        }

                                //}

                                //var v4 = kv3.Value.Where(w => w.FECHA_CIERRE != null && w.FECHA_CIERRE.Value.Date == nDay.Date).Count();

                                if (v2 == v1)
                                {
                                    ignoradosPeriodo++;


                                    //foreach (var xml in kv3.Value)
                                    //{
                                    //    if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                    //    {
                                    //        if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                                    //        if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                                    //        if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                                    //    }


                                    //    if (!uuid.ContainsKey(xml.UUID))
                                    //    {
                                    //        uuid.Add(xml.UUID, "");
                                    //        total += xml.TOTAL.Dbl();
                                    //        xmlPorValidar++;
                                    //    }
                                    //    else
                                    //    {

                                    //    }

                                    //}
                                }
                                else
                                {


                                    if (v3 > 0)
                                    {
                                        incompletosPeriodo++;




                                        //foreach (var xml in kv3.Value)
                                        //{
                                        //    if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                        //    {
                                        //        if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                                        //        if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                                        //        if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                                        //    }


                                        //    if (!uuid.ContainsKey(xml.UUID))
                                        //    {
                                        //        uuid.Add(xml.UUID, "");
                                        //        total += xml.TOTAL.Dbl();
                                        //        xmlPorValidar++;
                                        //    }
                                        //    else
                                        //    {

                                        //    }

                                        //}
                                    }


                                }



                            }



                            //**************************************************************************************************
                            //var rzn = kv1.Value.FirstOrDefault(f => f.RFC_EMISOR == kv1.Key).RAZON_SOCIAL_EMISOR;
                            ls1.Add(new REP
                            {
                                //RFC_EMISOR = kv1.Key,
                                //RAZON_SOCIAL_EMISOR = rzn,
                                NUEVOS = nuevos,
                                SIN_PROCESAR = ignoradosPeriodo,
                                INCOMPLETOS = incompletosPeriodo,
                                //POR_VALIDAR = ignoradosPeriodo + incompletosPeriodo + nuevos,

                                //XML_POR_VALIDAR = xmlPorValidar,
                                //dXML_POR_VALIDAR = total,

                                VALIDADORA = kv1.Key,
                                VALIDADOS = validados,

                                dXML_VALIDADO = totalValidado,
                                XML_VALIDADO = xmlValidado,

                                dXML_FINAL_DIA = xmltotalFinalDia,
                                XML_FINAL_DIA  = xmlFinalDia,

                                VALIDADOS_AYER = validadosAyer,
                                SIN_PROCESAR_AYER = ignoradosAyer,
                                INCOMPLETOS_AYER = incompletosAyer,

                            });

                        }




                        //****************************************************************************************************
                        wb.Worksheets.Add("Dia " + f3);
                        var ws = wb.Worksheets.Worksheet(1);

                        int row = 2;
                        int column = 2;

                        for (int j = 1; j < column; j++)
                        {
                            ws.Column(j).Width = 2.5;
                        }

                        //IXLCell cell = ws.Cell(row, column + 1);
                        IXLCell cell = ws.Cell(row, column);
                        IXLRange ran = null;


                        //cell.Value = "Periodo " + f1 + " a " + f2;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontSize = 13;

                        cell = ws.Cell(row, column);
                        //cell = ws.Cell(row + 1, column);

                        //tb.DefaultView.Sort = "NUEVOS DESC";
                        //tb                  = tb.DefaultView.ToTable();


                        var tb = ls1.ToDataTable();

                        var tbl = cell.InsertTable(tb, true);
                        tbl.Theme = XLTableTheme.TableStyleMedium2;
                        tbl.ShowTotalsRow = true;

                        tbl.Field("RAZON_SOCIAL_EMISOR").TotalsRowLabel   = "TOTAL:";
                        tbl.Field("NUEVOS").TotalsRowFunction             = XLTotalsRowFunction.Sum;
                        tbl.Field("VALIDADOS").TotalsRowFunction          = XLTotalsRowFunction.Sum;
                        tbl.Field("SIN PROCESAR").TotalsRowFunction       = XLTotalsRowFunction.Sum;
                        tbl.Field("INCOMPLETOS").TotalsRowFunction        = XLTotalsRowFunction.Sum;
                        //tbl.Field("POR VALIDAR").TotalsRowFunction        = XLTotalsRowFunction.Sum;
                        tbl.Field("No. XML POR VALIDAR").TotalsRowFunction = XLTotalsRowFunction.Sum;
                        //tbl.Field("$ VALIDADO").TotalsRowFunction = XLTotalsRowFunction.Sum;
                        tbl.Field("No. XML VALIDADO").TotalsRowFunction = XLTotalsRowFunction.Sum;
                        //tbl.Field("$ POR VALIDAR").TotalsRowFunction      = XLTotalsRowFunction.Sum;
                        tbl.Field("SIN PROCESAR AYER").TotalsRowFunction   = XLTotalsRowFunction.Sum;
                        tbl.Field("INCOMPLETOS AYER").TotalsRowFunction = XLTotalsRowFunction.Sum;
                        tbl.Field("VALIDADOS AYER").TotalsRowFunction   = XLTotalsRowFunction.Sum;

                        ran = ws.Range(
                                tbl.FirstCell().CellBelow(),
                                tbl.LastCell());



                        if (ran != null)
                        {

                            //ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ////ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ////ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            //ran.Column(tbl.Field("ESTATUS2").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            //ran.Column(tbl.Field("POR VALIDAR").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("NUEVOS").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("VALIDADOS").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            //ran.Column(tbl.Field("$ VALIDADO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("No. XML VALIDADO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("SIN PROCESAR").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("No. XML POR VALIDAR").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("INCOMPLETOS").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("SIN PROCESAR AYER").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("VALIDADOS AYER").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("INCOMPLETOS AYER").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            //ran.Column(tbl.Field("$ POR VALIDAR").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                            //ran.Column(tbl.Field("$ VALIDADO").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                            


                            //tbl.Cell(1, tbl.Field("IGNORADOS").Index + 1).Style.Fill.BackgroundColor                          = XLColor.Red;

                            ran.AdjustToContents();

                            

                        }




                        //*************************************************************************************************/
                        int    sNUEVOS            = 0;
                        int    sINCOMPLETOS       = 0;
                        int    sSIN_PROCESAR      = 0;
                        int    sVALIDADOS         = 0;
                        double sVALIDADO          = 0;
                        int    s_XML_VALIDADO     = 0;
                        int    sPOR_VALIDAR       = 0;
                        int    s_XML_POR_VALIDAR  = 0;
                        double s_POR_VALIDAR      = 0;
                        int    sINCOMPLETOS_AYER  = 0;
                        int    sSIN_PROCESAR_AYER = 0;
                        int    sVALIDADOS_AYER    = 0;

                        if (ls1.Count > 0)
                        {


                            foreach (var rows in ls1)
                            {

                                var vl = acc.FirstOrDefault(f => f.VALIDADORA.Str() == rows.VALIDADORA.Str());

                                if (vl == null)
                                {
                                    acc.Add(new Acumulado
                                    {
                                        VALIDADORA        = rows.VALIDADORA.Str(),
                                        INCOMPLETOS       = rows.INCOMPLETOS.Int32(),
                                        SIN_PROCESAR      = rows.SIN_PROCESAR.Int32(),
                                        NUEVOS            = rows.NUEVOS.Int32(),
                                        VALIDADOS         = rows.VALIDADOS.Int32(),
                                        
                                        dXML_VALIDADO     = rows.dXML_VALIDADO.Dbl(),
                                        XML_VALIDADO      = rows.XML_VALIDADO.Int32(),

                                        dXML_POR_VALIDAR  = rows.dXML_POR_VALIDAR.Dbl(),
                                        XML_POR_VALIDAR   = rows.XML_POR_VALIDAR.Int32(),

                                        dXML_FINAL_DIA = rows.dXML_FINAL_DIA.Dbl(),
                                        XML_FINAL_DIA  = rows.XML_FINAL_DIA.Int32(),

                                        INCOMPLETOS_AYER  = rows.INCOMPLETOS_AYER.Int32(),
                                        SIN_PROCESAR_AYER = rows.SIN_PROCESAR_AYER.Int32(),
                                        VALIDADOS_AYER    = rows.VALIDADOS_AYER.Int32(),
                                    });

                                }
                                else
                                {
                                    vl.INCOMPLETOS       += rows.INCOMPLETOS.Int32();
                                    vl.SIN_PROCESAR      += rows.SIN_PROCESAR.Int32();
                                    vl.NUEVOS            += rows.NUEVOS.Int32();
                                    vl.VALIDADOS         += rows.VALIDADOS.Int32();

                                    vl.dXML_VALIDADO     += rows.dXML_VALIDADO.Dbl();
                                    vl.XML_VALIDADO      += rows.XML_VALIDADO.Int32();

                                    vl.dXML_POR_VALIDAR  += rows.dXML_POR_VALIDAR.Dbl();
                                    vl.XML_POR_VALIDAR   += rows.XML_POR_VALIDAR.Int32();
                                    
                                    vl.INCOMPLETOS_AYER  += rows.INCOMPLETOS_AYER.Int32();
                                    vl.SIN_PROCESAR_AYER += rows.SIN_PROCESAR_AYER.Int32();
                                    vl.VALIDADOS_AYER    += rows.VALIDADOS_AYER.Int32();
                                }

                                sINCOMPLETOS       += rows.INCOMPLETOS.Int32();
                                sSIN_PROCESAR      += rows.SIN_PROCESAR.Int32();
                                sNUEVOS            += rows.NUEVOS.Int32();
                                sVALIDADOS         += rows.VALIDADOS.Int32();
                                sVALIDADO          += rows.dXML_VALIDADO.Dbl();
                                s_XML_VALIDADO     += rows.XML_VALIDADO.Int32();

                                s_XML_POR_VALIDAR  += rows.XML_POR_VALIDAR.Int32();
                                s_POR_VALIDAR      += rows.dXML_POR_VALIDAR.Dbl();

                                sINCOMPLETOS_AYER  += rows.INCOMPLETOS_AYER.Int32();
                                sSIN_PROCESAR_AYER += rows.SIN_PROCESAR_AYER.Int32();
                                sVALIDADOS_AYER    += rows.VALIDADOS_AYER.Int32();
                            }

                            /*

                            var dtb = new DataTable();

                            dtb.Columns.Add(" ", typeof(String));
                            foreach (var ac in acc)
                            {
                                dtb.Columns.Add(ac.VALIDADORA, typeof(object));

                            }


                            dtb.Rows.Add("NUEVOS");
                            dtb.Rows.Add("SIN PROCESAR");
                            dtb.Rows.Add("INCOMPLETOS");
                            dtb.Rows.Add("");
                            dtb.Rows.Add("POR VALIDAR");
                            dtb.Rows.Add("$ POR VALIDAR");
                            dtb.Rows.Add("VALIDADOS");
                            dtb.Rows.Add("");
                            dtb.Rows.Add("SIN PROCESAR AYER");
                            dtb.Rows.Add("INCOMPLETOS AYER");
                            dtb.Rows.Add("VALIDADOS AYER");

                            foreach (var ac in acc)
                            {
                                var inx=dtb.Columns[ac.VALIDADORA].Ordinal;

                                var _row = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "NUEVOS").FirstOrDefault();
                                _row[inx] = ac.NUEVOS;


                                _row = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "SIN PROCESAR").FirstOrDefault();
                                _row[inx] = ac.SIN_PROCESAR;

                                _row      = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "INCOMPLETOS").FirstOrDefault();
                                _row[inx] = ac.INCOMPLETOS;

                                _row = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "POR VALIDAR").FirstOrDefault();
                                _row[inx] = ac.POR_VALIDAR;

                                _row = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "$ POR VALIDAR").FirstOrDefault();
                                _row[inx] = ac._POR_VALIDAR;

                                _row = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "VALIDADOS").FirstOrDefault();
                                _row[inx] = ac.VALIDADOS;

                                _row      = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "SIN PROCESAR AYER").FirstOrDefault();
                                _row[inx] = ac.SIN_PROCESAR_AYER;

                                _row      = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "INCOMPLETOS AYER").FirstOrDefault();
                                _row[inx] = ac.INCOMPLETOS_AYER;

                                _row      = dtb.Rows.Cast<DataRow>().Where(dr => dr[0] == "VALIDADOS AYER").FirstOrDefault();
                                _row[inx] = ac.VALIDADOS_AYER;
                            }

                            */

                            //****************************************************************************************************


                            wb.Worksheets.Add("Acumulado");
                            ws = wb.Worksheets.Worksheet(2);


                            //----------------------------------------------------------------------------------------------------
                            row = 2;
                            column = 2;

                            for (int j = 1; j < column; j++)
                            {
                                ws.Column(j).Width = 2.5;
                            }

                            cell = ws.Cell(row, column);
                            ran = null;


                            //cell.Value = "Periodo " + f1 + " a " + f2;
                            //cell.Style.Font.Bold = true;
                            //cell.Style.Font.FontSize = 13;

                            cell = ws.Cell(row, column);

                            //tb.DefaultView.Sort = "NUEVOS DESC";
                            //tb = tb.DefaultView.ToTable();


                            tb = acc.Select(s =>
                                new
                                {
                                    s.VALIDADORA,
                                    s.NUEVOS,
                                    s.SIN_PROCESAR,
                                    s.INCOMPLETOS,

                                    s.dXML_POR_VALIDAR,
                                    s.XML_POR_VALIDAR,

                                    s.VALIDADOS,

                                    s.dXML_VALIDADO,
                                    s.XML_VALIDADO,

                                    s.SIN_PROCESAR_AYER,
                                    s.INCOMPLETOS_AYER,
                                    s.VALIDADOS_AYER

                                }).ToDataTable();


                            tbl = cell.InsertTable(tb, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;
                            tbl.ShowTotalsRow = true;

                            tbl.Field("VALIDADORA").TotalsRowLabel             = "TOTAL:";
                            tbl.Field("NUEVOS").TotalsRowFunction              = XLTotalsRowFunction.Sum;
                            tbl.Field("VALIDADOS").TotalsRowFunction           = XLTotalsRowFunction.Sum;
                            tbl.Field("SIN PROCESAR").TotalsRowFunction        = XLTotalsRowFunction.Sum;
                            tbl.Field("INCOMPLETOS").TotalsRowFunction         = XLTotalsRowFunction.Sum;
                            tbl.Field("No. XML POR VALIDAR").TotalsRowFunction = XLTotalsRowFunction.Sum;
                            //tbl.Field("POR VALIDAR").TotalsRowFunction         = XLTotalsRowFunction.Sum;
                            //tbl.Field("$ POR VALIDAR").TotalsRowFunction       = XLTotalsRowFunction.Sum;
                            //tbl.Field("$ VALIDADO").TotalsRowFunction          = XLTotalsRowFunction.Sum;
                            tbl.Field("No. XML VALIDADO").TotalsRowFunction    = XLTotalsRowFunction.Sum;
                            tbl.Field("SIN PROCESAR AYER").TotalsRowFunction   = XLTotalsRowFunction.Sum;
                            tbl.Field("INCOMPLETOS AYER").TotalsRowFunction    = XLTotalsRowFunction.Sum;
                            tbl.Field("VALIDADOS AYER").TotalsRowFunction      = XLTotalsRowFunction.Sum;

                            ran = ws.Range(
                                    tbl.FirstCell().CellBelow(),
                                    tbl.LastCell());



                            if (ran != null)
                            {
                                ran.Column(tbl.Field("NUEVOS").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("VALIDADOS").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("SIN PROCESAR").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("INCOMPLETOS").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("No. XML POR VALIDAR").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("$ VALIDADO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("No. XML VALIDADO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("POR VALIDAR").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("SIN PROCESAR AYER").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("VALIDADOS AYER").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ran.Column(tbl.Field("INCOMPLETOS AYER").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("$ POR VALIDAR").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                //ran.Column(tbl.Field("$ VALIDADO").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                ran.AdjustToContents();

                            }


                            //****************************************************************************************************





                        }

                        //*************************************************************************************************/
                        HttpContext context   = HttpContext.Current;
                        string      file      = context.Server.MapPath("~/ReporteContrarecibo.html");
                        string      plantilla = File.ReadAllText(file);

                        var parser   = new HtmlParser(Configuration.Default.WithCss());
                        var document = parser.Parse(plantilla);

                        PdfDocument doc = new PdfDocument();

                        
                        //var tbTR    = document.QuerySelector("#tbTR");
                        //var tbTotal = (IHtmlElement)document.QuerySelector("#tbTotal").Clone();
                        //              document.QuerySelector("#tbTotal").Remove();


                        //document.QuerySelector("#tbTD").Style.Display = "none";


                        document.QuerySelector("#fecha").InnerHtml = f1 + " AL " + f2;

                        /*var col = 0;
                        foreach (var ac in acc)
                        {
                            
                            var dtb1 = (IHtmlElement)document.QuerySelector("#tbTD").Clone();
                            dtb1.Style.Display = "";
                            dtb1.Id            = "";

                            col++;
                            if (col % 2 != 0)
                            {
                                dtb1.Style.BackgroundColor = "#DCE6F1";
                            }

                            dtb1.QuerySelectorAll(".validadora")[0].InnerHtml      = ac.VALIDADORA;
                            dtb1.QuerySelectorAll(".nuevos")[0].InnerHtml          = ac.NUEVOS.Str();
                            dtb1.QuerySelectorAll(".sinProcesar")[0].InnerHtml     = ac.SIN_PROCESAR.Str();
                            dtb1.QuerySelectorAll(".incompletos")[0].InnerHtml     = ac.INCOMPLETOS.Str();
                            dtb1.QuerySelectorAll(".porValidar")[0].InnerHtml      = ac.POR_VALIDAR.Str();
                            dtb1.QuerySelectorAll(".nXmlPorValidar")[0].InnerHtml  = ac._XML_POR_VALIDAR.Str();
                            dtb1.QuerySelectorAll("._PorValidar")[0].InnerHtml     = ac._POR_VALIDAR.ToString("C");
                            dtb1.QuerySelectorAll(".validados")[0].InnerHtml       = ac.VALIDADOS.Str();
                            dtb1.QuerySelectorAll("._Validado")[0].InnerHtml       = ac.VALIDADO.ToString("C");
                            dtb1.QuerySelectorAll(".nXmlValidado")[0].InnerHtml    = ac._XML_VALIDADO.Str();
                            dtb1.QuerySelectorAll(".sinProcesarAyer")[0].InnerHtml = ac.SIN_PROCESAR_AYER.Str();
                            dtb1.QuerySelectorAll(".incompletoAyer")[0].InnerHtml  = ac.INCOMPLETOS_AYER.Str();
                            dtb1.QuerySelectorAll(".validadosAyer")[0].InnerHtml   = ac.VALIDADOS_AYER.Str();

                            tbTR.Append(dtb1);
                        }


                        col++;
                        if (col % 2 != 0)
                        {
                            tbTotal.Style.BackgroundColor = "#DCE6F1";
                        }


                        tbTotal.QuerySelectorAll(".nuevos1")[0].InnerHtml      = sNUEVOS.Str();
                        tbTotal.QuerySelectorAll(".sinProcesar1")[0].InnerHtml     = sSIN_PROCESAR.Str();
                        tbTotal.QuerySelectorAll(".incompletos1")[0].InnerHtml     = sINCOMPLETOS.Str();
                        tbTotal.QuerySelectorAll(".porValidar1")[0].InnerHtml      = sPOR_VALIDAR.Str();
                        tbTotal.QuerySelectorAll(".nXmlPorValidar1")[0].InnerHtml  = s_XML_POR_VALIDAR.Str();
                        tbTotal.QuerySelectorAll("._PorValidar1")[0].InnerHtml     = s_POR_VALIDAR.ToString("C");
                        tbTotal.QuerySelectorAll(".validados1")[0].InnerHtml       = sVALIDADOS.Str();
                        tbTotal.QuerySelectorAll("._Validado1")[0].InnerHtml       = sVALIDADO.ToString("C");
                        tbTotal.QuerySelectorAll(".nXmlValidado1")[0].InnerHtml    = s_XML_VALIDADO.Str();
                        tbTotal.QuerySelectorAll(".sinProcesarAyer1")[0].InnerHtml = sSIN_PROCESAR_AYER.Str();
                        tbTotal.QuerySelectorAll(".incompletoAyer1")[0].InnerHtml  = sINCOMPLETOS_AYER.Str();
                        tbTotal.QuerySelectorAll(".validadosAyer1")[0].InnerHtml   = sVALIDADOS_AYER.Str();

                        tbTR.Append(tbTotal);
                        */


                        double scm0 = 0;
                        double scm1 = 0;
                        double scm2 = 0;
                        double scm3 = 0;
                        double scm4 = 0;
                        double scm5 = 0;
                        double scm6 = 0;
                        double scm7 = 0;
                        double scm8 = 0;
                        double scm9 = 0;
                        double scm10 = 0;
                        double scm11 = 0;
                        double scm12 = 0;
                        double scm13 = 0;
                        double scm14 = 0;

                        //***************************************************************************************
                        var cm0 = acc.FirstOrDefault(f => f.VALIDADORA == "NANCY GARCIA");
                        document.QuerySelectorAll(".nuevos")[0].InnerHtml = cm0.NUEVOS.Str();
                        document.QuerySelectorAll(".sinProcesar")[0].InnerHtml = cm0.SIN_PROCESAR.Str();
                        document.QuerySelectorAll(".incompletos")[0].InnerHtml = cm0.INCOMPLETOS.Str();
                        document.QuerySelectorAll(".validadosAyer")[0].InnerHtml = cm0.VALIDADOS_AYER.Str();

                        document.QuerySelectorAll(".xmlValidado")[0].InnerHtml = cm0.XML_VALIDADO.Str();
                        document.QuerySelectorAll(".dXmlValidado")[0].InnerHtml = cm0.dXML_VALIDADO.ToString("c");

                        document.QuerySelectorAll(".xmlFinalDia")[0].InnerHtml  = cm0.XML_FINAL_DIA.Str();
                        document.QuerySelectorAll(".dXmlFinalDia")[0].InnerHtml = cm0.dXML_FINAL_DIA.ToString("c");


                        document.QuerySelectorAll(".xmlPorValidar")[0].InnerHtml  = (cm0.XML_VALIDADO+ cm0.XML_FINAL_DIA).Str();
                        document.QuerySelectorAll(".dXmlPorValidar")[0].InnerHtml = (cm0.dXML_VALIDADO+ cm0.dXML_FINAL_DIA).ToString("c");


                        document.QuerySelectorAll(".cPreviosProcesados")[0].InnerHtml = (cm0.VALIDADOS- cm0.VALIDADOS_AYER).Str();
                        document.QuerySelectorAll(".iSinProcesar")[0].InnerHtml = (cm0.NUEVOS- cm0.VALIDADOS_AYER).Str();

                        document.QuerySelectorAll(".tSinProcesar")[0].InnerHtml = ((cm0.NUEVOS - cm0.VALIDADOS_AYER)+(cm0.SIN_PROCESAR)+(cm0.INCOMPLETOS)).Str();
                        document.QuerySelectorAll(".cFinalDia")[0].InnerHtml = ((cm0.NUEVOS - cm0.VALIDADOS_AYER)+(cm0.SIN_PROCESAR)+(cm0.INCOMPLETOS)).Str();
                        document.QuerySelectorAll(".cInicioDia")[0].InnerHtml = (cm0.VALIDADOS+ (cm0.NUEVOS - cm0.VALIDADOS_AYER) + (cm0.SIN_PROCESAR) + (cm0.INCOMPLETOS)).Str();



                        //document.QuerySelectorAll(".porValidar")[0].InnerHtml = cm0.POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".nXmlPorValidar")[0].InnerHtml = cm0._XML_POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".dPorValidar")[0].InnerHtml = cm0._POR_VALIDAR.ToString("c");

                        document.QuerySelectorAll(".validados")[0].InnerHtml     = cm0.VALIDADOS.Str();
                        //document.QuerySelectorAll(".nXmlValidado")[0].InnerHtml = cm0._XML_VALIDADO.Str();
                        //document.QuerySelectorAll(".dValidado")[0].InnerHtml    = cm0.VALIDADO.ToString("c");


                        //document.QuerySelectorAll(".pendiente")[0].InnerHtml    = (cm0.POR_VALIDAR - cm0.VALIDADOS).Str();
                        //document.QuerySelectorAll(".xmlPendiente")[0].InnerHtml = (cm0._XML_POR_VALIDAR - cm0._XML_VALIDADO).Str();
                        //document.QuerySelectorAll(".dPendiente")[0].InnerHtml   = (cm0._POR_VALIDAR - cm0.VALIDADO).ToString("c");

                        //scm0 = (cm0.POR_VALIDAR - cm0.VALIDADOS);
                        //scm1 = (cm0._XML_POR_VALIDAR - cm0._XML_VALIDADO);
                        //scm2 = (cm0._POR_VALIDAR - cm0.VALIDADO);

                        scm0 += cm0.NUEVOS;
                        scm1 += (cm0.VALIDADOS + (cm0.NUEVOS - cm0.VALIDADOS_AYER) + (cm0.SIN_PROCESAR) + (cm0.INCOMPLETOS));
                        scm2 += cm0.VALIDADOS_AYER;
                        scm3 += (cm0.VALIDADOS - cm0.VALIDADOS_AYER);
                        scm4 += cm0.VALIDADOS;
                        scm5 += (cm0.NUEVOS - cm0.VALIDADOS_AYER);
                        scm6 += cm0.SIN_PROCESAR;
                        scm7 += cm0.INCOMPLETOS;
                        scm8 += ((cm0.NUEVOS - cm0.VALIDADOS_AYER) + (cm0.SIN_PROCESAR) + (cm0.INCOMPLETOS));
                        scm9 += cm0.XML_VALIDADO;
                        scm10 += cm0.dXML_VALIDADO;
                        scm11 += cm0.XML_FINAL_DIA;
                        scm12 += cm0.dXML_FINAL_DIA;
                        scm13 += (cm0.XML_VALIDADO + cm0.XML_FINAL_DIA);
                        scm14 += (cm0.dXML_VALIDADO + cm0.dXML_FINAL_DIA);
                        //***************************************************************************************

                        var cm1 = acc.FirstOrDefault(f => f.VALIDADORA == "GLORIA ARREVILLAGA");
                        document.QuerySelectorAll(".nuevos")[1].InnerHtml        = cm1.NUEVOS.Str();
                        document.QuerySelectorAll(".sinProcesar")[1].InnerHtml   = cm1.SIN_PROCESAR.Str();
                        document.QuerySelectorAll(".incompletos")[1].InnerHtml   = cm1.INCOMPLETOS.Str();
                        document.QuerySelectorAll(".validadosAyer")[1].InnerHtml = cm1.VALIDADOS_AYER.Str();

                        document.QuerySelectorAll(".xmlValidado")[1].InnerHtml   = cm1.XML_VALIDADO.Str();
                        document.QuerySelectorAll(".dXmlValidado")[1].InnerHtml  = cm1.dXML_VALIDADO.ToString("c");

                        document.QuerySelectorAll(".xmlFinalDia")[1].InnerHtml  = cm1.XML_FINAL_DIA.Str();
                        document.QuerySelectorAll(".dXmlFinalDia")[1].InnerHtml = cm1.dXML_FINAL_DIA.ToString("c");

                        document.QuerySelectorAll(".xmlPorValidar")[1].InnerHtml = (cm1.XML_VALIDADO + cm1.XML_FINAL_DIA).Str();
                        document.QuerySelectorAll(".dXmlPorValidar")[1].InnerHtml = (cm1.dXML_VALIDADO + cm1.dXML_FINAL_DIA).ToString("c");

                        document.QuerySelectorAll(".cPreviosProcesados")[1].InnerHtml = (cm1.VALIDADOS - cm1.VALIDADOS_AYER).Str();
                        document.QuerySelectorAll(".iSinProcesar")[1].InnerHtml = (cm1.NUEVOS - cm1.VALIDADOS_AYER).Str();

                        document.QuerySelectorAll(".tSinProcesar")[1].InnerHtml = ((cm1.NUEVOS - cm1.VALIDADOS_AYER) + (cm1.SIN_PROCESAR) + (cm1.INCOMPLETOS)).Str();
                        document.QuerySelectorAll(".cFinalDia")[1].InnerHtml = ((cm1.NUEVOS - cm1.VALIDADOS_AYER) + (cm1.SIN_PROCESAR) + (cm1.INCOMPLETOS)).Str();
                        document.QuerySelectorAll(".cInicioDia")[1].InnerHtml = (cm1.VALIDADOS + (cm1.NUEVOS - cm1.VALIDADOS_AYER) + (cm1.SIN_PROCESAR) + (cm1.INCOMPLETOS)).Str();

                        //document.QuerySelectorAll(".porValidar")[1].InnerHtml     = cm1.POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".nXmlPorValidar")[1].InnerHtml = cm1._XML_POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".dPorValidar")[1].InnerHtml    = cm1._POR_VALIDAR.ToString("c");

                        document.QuerySelectorAll(".validados")[1].InnerHtml    = cm1.VALIDADOS.Str();
                        //document.QuerySelectorAll(".nXmlValidado")[1].InnerHtml = cm1._XML_VALIDADO.Str();
                        //document.QuerySelectorAll(".dValidado")[1].InnerHtml    = cm1.VALIDADO.ToString("c");


                        //document.QuerySelectorAll(".pendiente")[1].InnerHtml = (cm1.POR_VALIDAR - cm1.VALIDADOS).Str();
                        //document.QuerySelectorAll(".xmlPendiente")[1].InnerHtml = (cm1._XML_POR_VALIDAR - cm1._XML_VALIDADO).Str();
                        //document.QuerySelectorAll(".dPendiente")[1].InnerHtml = (cm1._POR_VALIDAR - cm1.VALIDADO).ToString("c");

                        //scm0 += (cm1.POR_VALIDAR - cm1.VALIDADOS);
                        //scm1 += (cm1._XML_POR_VALIDAR - cm1._XML_VALIDADO);
                        //scm2 += (cm1._POR_VALIDAR - cm1.VALIDADO);

                        scm0 += cm1.NUEVOS;
                        scm1 += (cm1.VALIDADOS + (cm1.NUEVOS - cm1.VALIDADOS_AYER) + (cm1.SIN_PROCESAR) + (cm1.INCOMPLETOS));
                        scm2 += cm1.VALIDADOS_AYER;
                        scm3 += (cm1.VALIDADOS - cm1.VALIDADOS_AYER);
                        scm4 += cm1.VALIDADOS;
                        scm5 += (cm1.NUEVOS - cm1.VALIDADOS_AYER);
                        scm6 += cm1.SIN_PROCESAR;
                        scm7 += cm1.INCOMPLETOS;
                        scm8 += ((cm1.NUEVOS - cm1.VALIDADOS_AYER) + (cm1.SIN_PROCESAR) + (cm1.INCOMPLETOS));
                        scm9 += cm1.XML_VALIDADO;
                        scm10 += cm1.dXML_VALIDADO;
                        scm11 += cm1.XML_FINAL_DIA;
                        scm12 += cm1.dXML_FINAL_DIA;
                        scm13 += (cm1.XML_VALIDADO + cm1.XML_FINAL_DIA);
                        scm14 += (cm1.dXML_VALIDADO + cm1.dXML_FINAL_DIA);

                        //***************************************************************************************

                        var cm2 = acc.FirstOrDefault(f => f.VALIDADORA == "SANDRA CHANG");
                        document.QuerySelectorAll(".nuevos")[2].InnerHtml      = cm2.NUEVOS.Str();
                        document.QuerySelectorAll(".sinProcesar")[2].InnerHtml   = cm2.SIN_PROCESAR.Str();
                        document.QuerySelectorAll(".incompletos")[2].InnerHtml   = cm2.INCOMPLETOS.Str();
                        document.QuerySelectorAll(".validadosAyer")[2].InnerHtml = cm2.VALIDADOS_AYER.Str();

                        document.QuerySelectorAll(".cPreviosProcesados")[2].InnerHtml = (cm2.VALIDADOS - cm2.VALIDADOS_AYER).Str();
                        document.QuerySelectorAll(".iSinProcesar")[2].InnerHtml = (cm2.NUEVOS - cm2.VALIDADOS_AYER).Str();

                        document.QuerySelectorAll(".xmlValidado")[2].InnerHtml = cm2.XML_VALIDADO.Str();
                        document.QuerySelectorAll(".dXmlValidado")[2].InnerHtml = cm2.dXML_VALIDADO.ToString("c");

                        document.QuerySelectorAll(".xmlFinalDia")[2].InnerHtml  = cm2.XML_FINAL_DIA.Str();
                        document.QuerySelectorAll(".dXmlFinalDia")[2].InnerHtml = cm2.dXML_FINAL_DIA.ToString("c");

                        document.QuerySelectorAll(".xmlPorValidar")[2].InnerHtml = (cm2.XML_VALIDADO + cm2.XML_FINAL_DIA).Str();
                        document.QuerySelectorAll(".dXmlPorValidar")[2].InnerHtml = (cm2.dXML_VALIDADO + cm2.dXML_FINAL_DIA).ToString("c");

                        document.QuerySelectorAll(".tSinProcesar")[2].InnerHtml = ((cm2.NUEVOS - cm2.VALIDADOS_AYER) + (cm2.SIN_PROCESAR) + (cm2.INCOMPLETOS)).Str();
                        document.QuerySelectorAll(".cFinalDia")[2].InnerHtml = ((cm2.NUEVOS - cm2.VALIDADOS_AYER) + (cm2.SIN_PROCESAR) + (cm2.INCOMPLETOS)).Str();
                        document.QuerySelectorAll(".cInicioDia")[2].InnerHtml = (cm2.VALIDADOS + (cm2.NUEVOS - cm2.VALIDADOS_AYER) + (cm2.SIN_PROCESAR) + (cm2.INCOMPLETOS)).Str();

                        //document.QuerySelectorAll(".porValidar")[2].InnerHtml     = cm2.POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".nXmlPorValidar")[2].InnerHtml = cm2._XML_POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".dPorValidar")[2].InnerHtml    = cm2._POR_VALIDAR.ToString("c");

                        document.QuerySelectorAll(".validados")[2].InnerHtml    = cm2.VALIDADOS.Str();
                        //document.QuerySelectorAll(".nXmlValidado")[2].InnerHtml = cm2._XML_VALIDADO.Str();
                        //document.QuerySelectorAll(".dValidado")[2].InnerHtml    = cm2.VALIDADO.ToString("c");


                        //document.QuerySelectorAll(".pendiente")[2].InnerHtml = (cm2.POR_VALIDAR - cm2.VALIDADOS).Str();
                        //document.QuerySelectorAll(".xmlPendiente")[2].InnerHtml = (cm2._XML_POR_VALIDAR - cm2._XML_VALIDADO).Str();
                        //document.QuerySelectorAll(".dPendiente")[2].InnerHtml = (cm2._POR_VALIDAR - cm2.VALIDADO).ToString("c");

                        //scm0 += (cm2.POR_VALIDAR - cm2.VALIDADOS);
                        //scm1 += (cm2._XML_POR_VALIDAR - cm2._XML_VALIDADO);
                        //scm2 += (cm2._POR_VALIDAR - cm2.VALIDADO);

                        scm0 += cm2.NUEVOS;
                        scm1 += (cm2.VALIDADOS + (cm2.NUEVOS - cm2.VALIDADOS_AYER) + (cm2.SIN_PROCESAR) + (cm2.INCOMPLETOS));
                        scm2 += cm2.VALIDADOS_AYER;
                        scm3 += (cm2.VALIDADOS - cm2.VALIDADOS_AYER);
                        scm4 += cm2.VALIDADOS;
                        scm5 += (cm2.NUEVOS - cm2.VALIDADOS_AYER);
                        scm6 += cm2.SIN_PROCESAR;
                        scm7 += cm2.INCOMPLETOS;
                        scm8 += ((cm2.NUEVOS - cm2.VALIDADOS_AYER) + (cm2.SIN_PROCESAR) + (cm2.INCOMPLETOS));
                        scm9 += cm2.XML_VALIDADO;
                        scm10 += cm2.dXML_VALIDADO;
                        scm11 += cm2.XML_FINAL_DIA;
                        scm12 += cm2.dXML_FINAL_DIA;
                        scm13 += (cm2.XML_VALIDADO + cm2.XML_FINAL_DIA);
                        scm14 += (cm2.dXML_VALIDADO + cm2.dXML_FINAL_DIA);
                        //***************************************************************************************
                        document.QuerySelectorAll(".nuevos")[3].InnerHtml             = scm0.Str();
                        document.QuerySelectorAll(".cInicioDia")[3].InnerHtml         = scm1.Str();
                        document.QuerySelectorAll(".validadosAyer")[3].InnerHtml      = scm2.Str();
                        document.QuerySelectorAll(".cPreviosProcesados")[3].InnerHtml = scm3.Str();
                        document.QuerySelectorAll(".validados")[3].InnerHtml          = scm4.Str();
                        document.QuerySelectorAll(".iSinProcesar")[3].InnerHtml       = scm5.Str();
                        document.QuerySelectorAll(".sinProcesar")[3].InnerHtml        = scm6.Str();
                        document.QuerySelectorAll(".incompletos")[3].InnerHtml        = scm7.Str();
                        document.QuerySelectorAll(".tSinProcesar")[3].InnerHtml       = scm8.Str();
                        document.QuerySelectorAll(".cFinalDia")[3].InnerHtml          = scm8.Str();
                        document.QuerySelectorAll(".xmlValidado")[3].InnerHtml      = scm9.Str();
                        document.QuerySelectorAll(".dXmlValidado")[3].InnerHtml      = scm10.ToString("c");

                        document.QuerySelectorAll(".xmlFinalDia")[3].InnerHtml  = scm11.Str();
                        document.QuerySelectorAll(".dXmlFinalDia")[3].InnerHtml = scm12.ToString("c");

                        document.QuerySelectorAll(".xmlPorValidar")[3].InnerHtml  = scm13.Str();
                        document.QuerySelectorAll(".dXmlPorValidar")[3].InnerHtml = scm14.ToString("c");

                        //document.QuerySelectorAll(".sinProcesar")[3].InnerHtml = sSIN_PROCESAR.Str();
                        //document.QuerySelectorAll(".incompletos")[3].InnerHtml = sINCOMPLETOS.Str();

                        //document.QuerySelectorAll(".porValidar")[3].InnerHtml     = sPOR_VALIDAR.Str();
                        //document.QuerySelectorAll(".nXmlPorValidar")[3].InnerHtml = s_XML_POR_VALIDAR.Str();
                        //document.QuerySelectorAll(".dPorValidar")[3].InnerHtml    = s_POR_VALIDAR.ToString("C");

                        //document.QuerySelectorAll(".validados")[3].InnerHtml    = sVALIDADOS.Str();
                        //document.QuerySelectorAll(".nXmlValidado")[3].InnerHtml = s_XML_VALIDADO.Str();
                        //document.QuerySelectorAll(".dValidado")[3].InnerHtml    = sVALIDADO.ToString("C");


                        //document.QuerySelectorAll(".pendiente")[3].InnerHtml    = scm0.Str();
                        //document.QuerySelectorAll(".xmlPendiente")[3].InnerHtml = scm1.Str();
                        //document.QuerySelectorAll(".dPendiente")[3].InnerHtml   = scm2.ToString("c");

                        //***************************************************************************************


                        var i = 1;

                        SizeF pageSize     = PdfPageSizes.FromName("Letter");
                        float marginLeft   = 0.3f;
                        float marginTop    = 0.3f;
                        float marginRight  = 0.3f;
                        float marginBottom = 0.3f;
                        bool  autoFitWidth = true;

                        //Set page layout arguments
                        HtmlToPdf.Options.ZoomLevel = 0.75f;
                        HtmlToPdf.Options.PageSize  = pageSize;
                        HtmlToPdf.Options.OutputArea = new RectangleF(
                            marginLeft,
                            marginTop,
                            pageSize.Width - marginLeft - marginRight,
                            pageSize.Height - marginTop - marginBottom);


                        HtmlToPdf.Options.BaseUrl = "file:///" + context.Server.MapPath("~/");

                        if (autoFitWidth)
                        {
                            HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ShrinkToFit;
                        }
                        else
                        {
                            HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.None;
                        }

                        HtmlToPdf.Options.StartPageIndex = i - 1;
                        HtmlToPdf.Options.StartPosition  = 0;
                        HtmlToPdf.ConvertHtml(document.DocumentElement.OuterHtml, doc);



                        HttpResponse resp = HttpContext.Current.Response;
                        resp.Clear();
                        resp.ClearHeaders();
                        resp.AddHeader("content-disposition", "attachment; filename=\"Reporte Contrarecibos(" + f1 + " AL " + f2 + ").pdf\"");
                        resp.ContentType = "application/pdf";
                        doc.Save(resp.OutputStream);

                        Response.End();


                        return;

                        //*************************************************************************************************/
                        Response.Clear();
                        Response.ContentType =
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        Response.AddHeader(
                            "content-disposition",
                            "attachment;filename=\"Reporte Contrarecibos.xlsx\"");

                        using (var memoryStream = new MemoryStream())
                        {

                            wb.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                        }

                        Response.End();


                        //*****************************

                        //--------------------------------------------------------------




                    }
                    catch (Exception ex)
                    {
                        
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                        }
                    }
                }

            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static void SetID(Req req)
        {
            HttpContext.Current.Session["req"] = req;
        }

        public class Req
        {
            public DateTime FECHA1     { get; set; }
            public DateTime FECHA2     { get; set; }
            public int      ESTATUS    { get; set; }
            public string   RFC_EMISOR { get; set; }
            public string   CONTR      { get; set; }
            public string   A          { get; set; }
            public string   USUARIO          { get; set; }

        }
    }

    public class ST
    {
        public int NUEVOS { get; set; }
        public int VALIDADOS { get; set; }
        public double TOTAL { get; set; }
        public int IGNORADOS_AYER { get; set; }
        public int INCOMPLETOS_AYER { get; set; }
        public int VALIDADOS_AYER { get; set; }
    }

    public class Acumulado
    {
        public string VALIDADORA { get; set; }
        public int NUEVOS { get; set; }
        public int VALIDADOS { get; set; }


        [DisplayName("MONTO TOTAL")]
        public double dXML_VALIDADO { get; set; }

        [DisplayName("No. XML VALIDADO")]
        public int XML_VALIDADO { get; set; }


        [DisplayName("SIN PROCESAR")]
        public int SIN_PROCESAR { get; set; }
        public int INCOMPLETOS { get; set; }


        [DisplayName("MONTO TOTAL")]
        public double dXML_POR_VALIDAR { get; set; }

        [DisplayName("No. XML POR VALIDAR")]
        public int XML_POR_VALIDAR { get; set; }



        [DisplayName("SIN PROCESAR AYER")] 
        public int SIN_PROCESAR_AYER { get; set; }

        [DisplayName("INCOMPLETOS AYER")]
        public int INCOMPLETOS_AYER { get; set; }

        [DisplayName("VALIDADOS AYER")]
        public int VALIDADOS_AYER { get; set; }


        public double dXML_FINAL_DIA { get; set; }
        public int XML_FINAL_DIA { get; set; }
    }
}