using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Dapper;
using WB_CFDI_V1.Properties;
using XML2;
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class excel2 : System.Web.UI.Page
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

            List<int> data = (List<int>)HttpContext.Current.Session["data"];
            bool? chkXML   = (bool?)HttpContext.Current.Session["chkXML"];
            bool? chkPDF   = (bool?)HttpContext.Current.Session["chkPDF"];

            if (data != null && chkPDF != null && chkPDF != null)
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    try
                    {
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
                      xml.TIPO_COMPROBANTE
                    FROM dbo.CFDI_XML_PSI xml
                    INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                    WHERE xml.ID = @ID";

                        MemoryStream st = new MemoryStream();
                        using (ZipStorer zip = ZipStorer.Create(st, ""))
                        {


                            List<CFDI_XML> ls = new List<CFDI_XML>();
                            foreach (var it in data)
                            {
                                var rs = con.Query<CFDI_XML>(sql, new {ID = it}).SingleOrDefault();


                                if( rs.TIPO_COMPROBANTE != null && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso") )
                                {
                                    if( rs.TOTAL     != null ) rs.TOTAL     = rs.TOTAL     * -1;
                                    if( rs.SUBTOTAL  != null ) rs.SUBTOTAL  = rs.SUBTOTAL  * -1;
                                    if( rs.IMPUESTOS != null ) rs.IMPUESTOS = rs.IMPUESTOS * -1;
                                }


                                    try
                                    {
                                        XML3.Comprobante c33 = new XML3.Comprobante();
                                        XML2.Comprobante c32 = new XML2.Comprobante();


                                        c33 = c33.DeserializeStr(rs.XML);
                                        c32 = c32.DeserializeStr(rs.XML);

                                        if (c33.Version != null)
                                        {

                                            rs.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                                .Sum(s => s.Importe.Dbl());

                                            rs.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                                .Sum(s => s.Importe.Dbl());

                                            if (rs.TIPO_COMPROBANTE != null && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                            {
                                                rs.IVA = rs.IVA * -1;
                                                rs.IEPS = rs.IEPS * -1;
                                            }


                                        }

                                        if (c32.Version != null)
                                        {
                                            rs.IVA = c32.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                                .Sum(s => s.Importe.Dbl());

                                            rs.IEPS = c32.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                                .Sum(s => s.Importe.Dbl());

                                            if (rs.TIPO_COMPROBANTE != null && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso"))
                                            {
                                                rs.IVA = rs.IVA * -1;
                                                rs.IEPS = rs.IEPS * -1;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                


                                if (rs != null)
                                {
                                    if (chkPDF.Value && rs.ARCHIVO != null)
                                    {
                                        zip.AddStream(
                                            ZipStorer.Compression.Deflate,
                                            rs.NOMBRE_ARCHIVO + ".pdf",
                                            new MemoryStream(rs.ARCHIVO),
                                            DateTime.Now,
                                            "");
                                    }

                                    if (chkXML.Value)
                                    {
                                        zip.AddStream(
                                            ZipStorer.Compression.Deflate,
                                            rs.NOMBRE_ARCHIVO + ".xml",
                                            rs.XML.ToStream(),
                                            DateTime.Now,
                                            "");
                                    }

                                    rs.ARCHIVO = null;

                                    ls.Add(rs);
                                }
                                
                            }


                            if (ls.Count == 0)
                            {
                                ls.Add(new CFDI_XML());
                            }

                            var lst=ls.Select(
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
                                    FECHA_FACTURA   = s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt")
                                }).ToDataTable(true);

                            var wb = new XLWorkbook();
                            var ws = wb.Worksheets.Add("Reporte CFDI");

                            int row = 2;
                            int column = 2;

                            for (int j = 1; j < column; j++)
                            {
                                ws.Column(j).Width = 2.5;
                            }

                            IXLCell cell = ws.Cell(row, column);
                            IXLRange ran = null;

                            var tbl = cell.InsertTable(lst, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                tbl.FirstCell().CellBelow(),
                                tbl.LastCell());

                            var total = ran.LastRow().RowBelow();
                            total.Cell("F").Value = "TOTAL:";
                            total.Cell("F").Style.Font.Bold = true;

                            total.Cell("G").Value = ls.Sum(s => s.TOTAL);
                            total.Cell("G").Style.NumberFormat.Format = "$#,##0.00";

                            if (ran != null)
                            {
                                ran.Columns("G").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("H").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("I").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("J").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("K").Style.NumberFormat.Format = "$#,##0.00";

                                //ran.Columns("E").Style.NumberFormat.Format = "@";
                                //ran.Columns("E").SetDataType(XLCellValues.Text);
                                /*foreach (var rw in ran.Rows())
                                {
                                    rw.Cell("E").Style.NumberFormat.Format = "@";
                                    rw.Cell("E").Value = "'" + rw.Cell("E").Value;
                                    //rw.Cell("F").Style.NumberFormat.Format = "@";
                                    //rw.Cell("E").Style.NumberFormat.Format = "$#,##0.00";
                                }*/
                                ran.AdjustToContents();
                            }


                            MemoryStream rep = new MemoryStream();
                            wb.SaveAs(rep);
                            rep.Position = 0;
                            zip.AddStream(
                                ZipStorer.Compression.Deflate,
                                "CFDI_" + Extensions.GetAlias()  + ".xlsx",
                                rep,
                                DateTime.Now,
                                "");

                        }

                        

                        Response.Expires = 0;
                        Response.Buffer = true;
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment; filename=\"CFDI_" + Extensions.GetAlias() + ".zip\"");
                        Response.ContentType = "application/x-compressed";
                        Response.BinaryWrite(st.ToArray());
                        Response.End();
                    }
                    catch
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




            return;
            /*var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Reporte");
            List<Data> data = (List<Data>)HttpContext.Current.Session["data"];
            if (data != null)
            {


                int row = 2;
                int column = 2;

                for (int j = 1; j < column; j++)
                {
                    ws.Column(j).Width = 2.5;
                }


                //*****************************************************************                  
                if (data.Count == 0)
                {
                    data.Add(new Data());
                }

                IXLCell cell = ws.Cell(row, column);
                IXLRange ran = null;

                var tbl = cell.InsertTable(data, true);
                tbl.Theme = XLTableTheme.TableStyleMedium2;

                for (int i = 3; i <= 6; i++)
                {
                    tbl.FirstRow().Cell(i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }


                ran = ws.Range(
                    tbl.FirstCell().CellBelow(),
                    tbl.LastCell());

                ran.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ran.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ran.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                if (ran != null)
                {
                    ran.AdjustToContents();
                }


                for (int i = 3; i <= 6; i++)
                {
                    var co = ran.Column(i).ColumnLetter();
                    ws.Column(co).Width = ws.Column(co).Width + 3;
                    
                }
            }
            HttpContext.Current.Session["data"] = null;

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader(
                "content-disposition",
                "attachment;filename=\"Reporte_" + ".xlsx\"");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
            }
            Response.End();*/
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static void SetID(bool chkXML, bool chkPDF, List<int> data)
        {
            HttpContext.Current.Session["data"]   = data;
            HttpContext.Current.Session["chkXML"] = chkXML;
            HttpContext.Current.Session["chkPDF"] = chkPDF;
        }
    }
}