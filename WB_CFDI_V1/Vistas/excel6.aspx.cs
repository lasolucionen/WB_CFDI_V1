using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using ClosedXML.Excel;
using Dapper;
using EO.Pdf;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;

using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class excel6 : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-2));
            Response.Cache.SetNoStore();
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ExpiresAbsolute = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            Response.Expires         = 0;
            Response.CacheControl    = "no-cache";
            Response.AppendHeader("Pragma", "no-cache");

            EO.Pdf.Runtime.AddLicense(
                "U/Ge6sUF6bua4/AAIr112Oj1y/Oy5+nOzcSIpdT1EaFZ7ekDHuio5cGz3Lhnp6ax2r11puX9F+6wtcAAHeOe6c3/Ee5Z2+UFELxbqbPD265rp7XKy7BrsbTB5a9pl8XezZ+v3PYEFO6ntKbCzZ+f4+X4Hrxbp6ax2r116u34GeCt7Pb26diRqNfH1vea3P3EA+2O4u319dmKydXO6Lto6u34GeCt7Pb26bto4+30EO2s3MJ14+30EO2s3MLNF+ic3PIEEMidtc2+AuCr3P6x3a9qsMDAF+ic3PIEEMidtcD2I++i6ekE7PN3qLbA3rBoqbTK5J9qqb7B27lpp6TS+Lto3PwBFA==");


            Req req = (Req)HttpContext.Current.Session["data"];


            if (req != null)
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    try
                    {
                        var emp = Extensions.cEmpresa();

                        string filtr = "";
                        string ftr = "Todas";

                        if (req.ESTATUS == 1)
                        {
                            ftr = "Con Complemento";

                            filtr = @"
                            INNER JOIN 
                              (
                              
                                SELECT
                                  MIN(d.ID_DOCUMENTO) ID_DOCUMENTO, MIN(d.NUM_PARCIALIDAD) NUM_PARCIALIDAD,d.COMPLEMENTO_ID 
                                FROM dbo.CFDI_XML_COMPLEMENTO_DETALLE d WITH (NOLOCK)
                                INNER JOIN dbo.CFDI_XML_COMPLEMENTO_PAGO c WITH (NOLOCK)
                                  ON d.COMPLEMENTO_ID = c.ID
                                  WHERE c.RFC_EMISOR=@RFC_EMISOR
                                 GROUP BY d.ID_DOCUMENTO, d.COMPLEMENTO_ID
                              
                              ) t

                             ON  t.ID_DOCUMENTO = xml.UUID
                            ";
                        }

                        if (req.ESTATUS == 2)
                        {
                            ftr = "Sin Complemento";

                            filtr = @"
                            INNER JOIN 
                              (
                              
                                SELECT
                                  MIN(d.ID_DOCUMENTO) ID_DOCUMENTO, MIN(d.NUM_PARCIALIDAD) NUM_PARCIALIDAD,d.COMPLEMENTO_ID 
                                FROM dbo.CFDI_XML_COMPLEMENTO_DETALLE d WITH (NOLOCK)
                                INNER JOIN dbo.CFDI_XML_COMPLEMENTO_PAGO c WITH (NOLOCK)
                                  ON d.COMPLEMENTO_ID = c.ID
                                  WHERE c.RFC_EMISOR=@RFC_EMISOR
                                 GROUP BY d.ID_DOCUMENTO, d.COMPLEMENTO_ID
                              
                              ) t

                             ON  NOT t.ID_DOCUMENTO = xml.UUID
                            ";
                        }

                        string sql =
                            @"
                        SELECT
                          xml.ID,
                          xml.USER_ID,
                          xml.CONTRA_RECIBO_ID,
                          xml.XML,
                          xml.RAZON_SOCIAL_EMISOR,
                          xml.RFC_EMISOR,
                          xml.TOTAL,
                          xml.SUBTOTAL,
                          xml.IMPUESTOS,
                          xml.VERSION,
                          xml.FECHA_RECEPCION,
                          xml.FECHA_FACTURA,
                          xml.SERIE,
                          xml.FOLIO,
                          xml.TIENDA,
                          xml.ARCHIVO,
                          xml.NOMBRE_ARCHIVO,
                          es.ESTATUS AS ESTATUS2,
                          xml.NUMEFECTO,
                          xml.FECHA_SALDADO,
                          xml.TIPO_COMPROBANTE,
                          ic.FECHA_PROCESO,
                          xml.UUID
                        FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                        INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID

                        " + filtr + @"

                        LEFT OUTER JOIN
                                 (SELECT DISTINCT 
                                    MAX(FECHA_PROCESO) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK) group by FOLIOFISCAL) ic
                                        ON xml.UUID = ic.FOLIOFISCAL
                        WHERE (FECHA_RECEPCION
                        BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME))
                        --AND xml.USER_ID = @USER_ID
                        AND xml.RFC_EMISOR = @RFC_EMISOR

                        ORDER BY xml.CONTRA_RECIBO_ID DESC";



                        var rs = con.Query<CFDI_XML>(sql, req).ToList();

                        foreach (var xml in rs)
                        {
                            if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                            {
                                if (xml.TOTAL != null) xml.TOTAL         = xml.TOTAL * -1;
                                if (xml.SUBTOTAL != null) xml.SUBTOTAL   = xml.SUBTOTAL * -1;
                                if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                            }


                            try
                            {
                                XML3.Comprobante c33 = new XML3.Comprobante();

                                c33 = c33.DeserializeStr(xml.XML);


                                if (c33.Version != null)
                                {

                                    xml.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                        .Sum(s => s.Importe.Dbl());

                                    xml.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                        .Sum(s => s.Importe.Dbl());

                                    if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                    {
                                        xml.IVA = xml.IVA * -1;
                                        xml.IEPS = xml.IEPS * -1;
                                    }


                                }


                            }
                            catch (Exception ex)
                            {

                            }

                        }


                        var lst = rs.Select(
                            s => new
                            {
                                s.CONTRA_RECIBO_ID,
                                s.RFC_EMISOR,
                                s.RAZON_SOCIAL_EMISOR,
                                s.SERIE,
                                s.FOLIO,
                                s.TIENDA,
                                s.TOTAL,
                                s.SUBTOTAL,
                                s.IMPUESTOS,
                                s.IVA,
                                s.IEPS,
                                FECHA_FACTURA = s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                FECHA_RECEPCION = s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                FECHA_PROCESO = s.FECHA_PROCESO == null ? "" : s.FECHA_PROCESO.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                ESTATUS = s.ESTATUS2,
                                s.NUMEFECTO,
                                s.FECHA_SALDADO,
                                s.UUID,
                                //NOMBRE_ARCHIVO = s.NOMBRE_ARCHIVO + ".xml"
                            }).ToDataTable(true);


                            var wb = new XLWorkbook();
                            var ws = wb.Worksheets.Add("Reporte");

                            int row = 2;
                            int column = 2;

                            for (int j = 1; j < column; j++)
                            {
                                ws.Column(j).Width = 2.5;
                            }

                            IXLCell cell = ws.Cell(row, column);
                            IXLRange ran = null;

                            
                            var tx=cell.RichText.AddText("Filtro: ");
                            tx.Bold = true;


                            tx = cell.RichText.AddText(ftr);
                            tx.Bold = false;

                            var h = ws.Range("B" + row + ":C" + row).Merge();

                            cell = cell.CellBelow(2);

                            var tbl = cell.InsertTable(lst, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                tbl.FirstCell().CellBelow(),
                                tbl.LastCell());


                            if (ran != null)
                            {
                                //ran.Columns("I").Style.NumberFormat.Format = "$#,##0.00";
                                //ran.Columns("K").Style.Alignment.Horizontal=XLAlignmentHorizontalValues.Center;
                                //ran.Columns("L").Style.Alignment.Horizontal=XLAlignmentHorizontalValues.Center;
                                
                                //ran.Columns("J").Style.NumberFormat.Format = "$#,##0.00";
                                //ran.Columns("K").Style.NumberFormat.Format = "$#,##0.00";

                                ran.Columns("G").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("H").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("I").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("J").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("K").Style.NumberFormat.Format = "$#,##0.00";


                                ran.AdjustToContents();
                            }



                            Response.Clear();
                            Response.ContentType =
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"Reporte_"
                                + Extensions.GetAlias()
                                + ".xlsx\"");

                            using (var memoryStream = new MemoryStream())
                            {

                                wb.SaveAs(memoryStream);
                                memoryStream.WriteTo(Response.OutputStream);
                            }

                            Response.End();

                    }
                    catch(Exception ex)
                    {
                        //Log.Write("Err " + ex.Message);
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
            HttpContext.Current.Session["data"] = req;
        }


        public class Req
        {
            public DateTime? FECHA_BLOQ { get; set; }
            public DateTime  FECHA1     { get; set; }
            public DateTime  FECHA2     { get; set; }
            public int       ESTATUS    { get; set; }
            public int       USER_ID    { get; set; }
            public string    RFC_EMISOR { get; set; }
            public string    UUID       { get; set; }
            public string    FOLIO      { get; set; }
            public string    SERIE      { get; set; }
            public string    CONTR      { get; set; }
            public string    A          { get; set; }
            public int       OPT        { get; set; }
        }
    }
}