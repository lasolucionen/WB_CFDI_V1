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
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class excel4 : System.Web.UI.Page
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

            string id = (string)HttpContext.Current.Session["id"];


            if (id != null)
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
                      es.ESTATUS AS ESTATUS2,
                      xml.NUMEFECTO,
                      xml.FECHA_SALDADO,
                      xml.TIPO_COMPROBANTE
                    FROM dbo.CFDI_XML_PSI xml WITH (NOLOCK)
                    INNER JOIN dbo.CFDI_ESTATUS es WITH (NOLOCK) ON xml.ESTATUS = es.ID
                    WHERE xml.ID = @ID";

                        List<CFDI_XML> ls = new List<CFDI_XML>();

                            var rs = con.Query<CFDI_XML>(sql, new {ID = id}).SingleOrDefault();


                            if( rs.TIPO_COMPROBANTE != null
                             && (rs.TIPO_COMPROBANTE.ToLower() == "e" || rs.TIPO_COMPROBANTE.ToLower() == "egreso") )
                            {
                                if( rs.TOTAL     != null ) rs.TOTAL     = rs.TOTAL     * -1;
                                if( rs.SUBTOTAL  != null ) rs.SUBTOTAL  = rs.SUBTOTAL  * -1;
                                if( rs.IMPUESTOS != null ) rs.IMPUESTOS = rs.IMPUESTOS * -1;
                            }


                            try
                            {
                                XML3.Comprobante c33 = new XML3.Comprobante();

                                c33 = c33.DeserializeStr(rs.XML);

                                if( c33.Version != null )
                                {

                                    rs.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                                .Sum(s => s.Importe.Dbl());

                                    rs.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                                 .Sum(s => s.Importe.Dbl());

                                    if( rs.TIPO_COMPROBANTE != null
                                     && (rs.TIPO_COMPROBANTE.ToLower() == "e"
                                      || rs.TIPO_COMPROBANTE.ToLower() == "egreso") )
                                    {
                                        rs.IVA  = rs.IVA  * -1;
                                        rs.IEPS = rs.IEPS * -1;
                                    }


                                }


                            }
                            catch(Exception ex)
                            {

                            }



                            if( rs != null )
                            {

                                ls.Add(rs);
                            }

                        

                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("Reporte");

                        int row    = 2;
                        int column = 2;

                        for( int j = 1; j < column; j++ )
                        {
                            ws.Column(j).Width = 2.5;
                        }

                        if( ls.Count != 0 )
                        {
                            //ls.Add(new CFDI_XML());


                            var lst = ls.Select(
                                    s => new {
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
                                                 FECHA_FACTURA =
                                                         s.FECHA_FACTURA.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                                 FECHA_RECEPCION =
                                                         s.FECHA_RECEPCION.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                                 ESTATUS = s.ESTATUS2,
                                                 //s.NUMEFECTO,
                                                 //s.FECHA_SALDADO,
                                                 //NOMBRE_ARCHIVO = s.NOMBRE_ARCHIVO + ".xml"
                                             }).ToDataTable(true);


                            IXLCell  cell = ws.Cell(row, column);
                            IXLRange ran  = null;

                            var tbl = cell.InsertTable(lst, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                    tbl.FirstCell().CellBelow(),
                                    tbl.LastCell());


                            /*
                            var total = ran.LastRow().RowBelow();
                            total.Cell("F").Value           = "TOTAL:";
                            total.Cell("F").Style.Font.Bold = true;

                            total.Cell("G").Value                     = ls.Sum(s => s.TOTAL);
                            total.Cell("G").Style.NumberFormat.Format = "$#,##0.00";*/

                            if( ran != null )
                            {
                                ran.Columns("G").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("H").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("I").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("J").Style.NumberFormat.Format = "$#,##0.00";
                                ran.Columns("K").Style.NumberFormat.Format = "$#,##0.00";

                                ran.AdjustToContents();
                            }

                            var it = con.Query<CFDI_COMPRAS>(
                                      @"SELECT SERIE, FOLIO FROM CFDI_COMPRAS WITH (NOLOCK) WHERE ID_XML = @ID_XML",
                                      new { ID_XML = id }
                                      ).Select(c => new{
                                                            c.SERIE,
                                                            c.FOLIO
                                                       }).ToDataTable(true);


                            row = ran.LastRow().RowBelow().RowBelow().RowNumber();
                            cell = ws.Cell(row, column);

                            tbl       = cell.InsertTable(it, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                      tbl.FirstCell().CellBelow(),
                                      tbl.LastCell());

                            ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            //row = total.RowBelow().RowBelow().RowNumber();
                            //ran.LastRow().RowBelow();
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


        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static void SetID(string id)
        {
             HttpContext.Current.Session["id"] = id;

        }
    }
}