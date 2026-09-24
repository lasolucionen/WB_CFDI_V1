using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Dapper;

namespace WB_CFDI_V1.Vistas
{
    public partial class excelContr : System.Web.UI.Page
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

            Req data = (Req)HttpContext.Current.Session["data"];
            HttpContext.Current.Session["data"] = null;

            if (data != null)
            {

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    try
                    {
                        string sql = @"

                            SELECT
                              a.ID,
                              a.FECHA,
                              a.RFC_EMISOR,
                              a.FECHA_RECEPCION,
                              a.FECHA_PAGO,
                              SUM(b.TOTAL) AS IMPORTE
                            FROM dbo.CFDI_XML_PSI b
                            INNER JOIN dbo.CONTRA_RECIBOS_PSI a ON b.CONTRA_RECIBO_ID = a.ID
                            WHERE

                              (@OPT = 0 AND a.FECHA_RECEPCION BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                               OR
                               @OPT = 1 AND a.FECHA_PAGO BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME))

                            GROUP BY a.ID,
                                     a.FECHA,
                                     a.RFC_EMISOR,
                                     a.FECHA_RECEPCION,
                                     a.FECHA_PAGO

                            ORDER BY a.RFC_EMISOR,
                                     a.FECHA_PAGO";


                        var ls = con.Query<CONTRA_RECIBOS>(sql, data).ToList();


                        var lst = ls.Select(
                                s => new {
                                             s.RFC_EMISOR,
                                             ID = Extensions.GetSerie() + "-" + s.ID,
                                             s.FECHA_RECEPCION,
                                             s.FECHA_PAGO,
                                             s.IMPORTE

                                         }).ToDataTable(true);

                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("Contrarecibos");

                        int row    = 2;
                        int column = 2;

                        for( int j = 1; j < column; j++ )
                        {
                            ws.Column(j).Width = 2.5;
                        }

                        IXLCell  cell = ws.Cell(row, column);
                        IXLRange ran  = null;

                        var tbl = cell.InsertTable(lst, true);
                        tbl.Theme = XLTableTheme.TableStyleMedium2;

                        ran = ws.Range(
                                tbl.FirstCell().CellBelow(),
                                tbl.LastCell());

                        ran.Column("B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ran.Column("E").Style.NumberFormat.Format = "$#,##0.00";
                        //tbl.FirstRow().Cell("A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        if( ran != null )
                        {
                            ran.AdjustToContents();
                        }


                        Response.Clear();
                        Response.ContentType =
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"Contrarecibos_"
                              + Extensions.GetAlias()
                              + ".xlsx\"");

                        using( var memoryStream = new MemoryStream() )
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
                        if( con != null )
                        {
                            con.Close();
                        }
                    }
                }

            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod]
        public static void SetFiltr(Req req)
        {
            HttpContext.Current.Session["data"] = req;
        }

        public class Req
        {
            public DateTime FECHA1  { get; set; }
            public DateTime FECHA2  { get; set; }
            public int      OPT     { get; set; }
        }
    }
}
