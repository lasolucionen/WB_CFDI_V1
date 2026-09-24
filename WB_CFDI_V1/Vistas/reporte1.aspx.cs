using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Dapper;
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class reporte1 : System.Web.UI.Page
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

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                {
                    try
                    {

                        string sql =
                                @"
                        -- RESUMEN DE COMPRAS POR PERIODO
                        --DECLARE @CODPROVEEDOR INT = 0
                        --DECLARE @FECHAINICIAL DATETIME = '01/12/2010'
                        --DECLARE @FECHAFINAL DATETIME = '31/12/2019'

                        --SET @FECHAINICIAL = @FECHAINICIAL + '00:00:00'
                        --SET @FECHAFINAL = @FECHAFINAL + '23:59:59'

                        SELECT 
                        FC.CODPROVEEDOR, P.NOMPROVEEDOR, P.NIF20 RFC,
                        SUM(TOTALBRUTO) 'SUBTOTAL', 
                        SUM(FC.TOTALIMPUESTOS) 'IMPUESTOS',
                        SUM(TOTALNETO) TOTAL
                        FROM
                        (
                        SELECT NUMSERIEFAC, NUMFAC, FECHA_PROCESO
                        FROM IT_RELFACTURAS_COMPRAS WITH(NOLOCK)
                        WHERE FECHA_PROCESO BETWEEN @FECHAINICIAL + ' 00:00:00' AND @FECHAFINAL + ' 23:59:59'
                        GROUP BY NUMSERIEFAC, NUMFAC, FECHA_PROCESO
                        ) F
                        INNER JOIN FACTURASCOMPRA FC WITH(NOLOCK) ON F.NUMSERIEFAC COLLATE Latin1_General_CS_AI = FC.NUMSERIE AND F.NUMFAC = FC.NUMFACTURA
                        INNER JOIN PROVEEDORES P WITH(NOLOCK) ON FC.CODPROVEEDOR = P.CODPROVEEDOR
                        WHERE FC.CODPROVEEDOR = @CODPROVEEDOR OR @CODPROVEEDOR = 0
                        GROUP BY FC.CODPROVEEDOR, P.NOMPROVEEDOR, NIF20
                        ORDER BY FC.CODPROVEEDOR
                        ";


                        if (Environment.MachineName == "CTL-PC")
                        {
                            sql = "SELECT * FROM RESUMEN_COMPRAS WITH (NOLOCK)";
                        }

                        var rs = con.Query<RESUMEN_COMPRAS>(sql, req).ToDataTable(true);



                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("Resumen Compras");

                        int row = 2;
                        int column = 2;

                        for (int j = 1; j < column; j++)
                        {
                            ws.Column(j).Width = 2.5;
                        }

                        if (rs.Rows.Count>0)
                        {

                            IXLCell cell = ws.Cell(row, column);
                            IXLRange ran = null;

                            var tbl = cell.InsertTable(rs, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                    tbl.FirstCell().CellBelow(),
                                    tbl.LastCell());



                            if (ran != null)
                            {

                                    ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    ran.Column(tbl.Field("IMPUESTOS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                    //ran.Column(tbl.Field("IVA").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                    //ran.Column(tbl.Field("IEPS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                    //ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    //ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    //ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    //ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    //ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    //ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                    //ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;
                                    //ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).DataType = XLCellValues.DateTime;

                                    //ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                    //ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                    //ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";


                                ran.AdjustToContents();
                            }

                        }

                        //*************************************************************************************************/

                         sql =
                                @"
                        -- DETALLE DE COMPRAS POR PERIODO POR PROVEEDOR
                        --DECLARE @CODPROVEEDORD INT = 0
                        --DECLARE @FECHAINICIALD DATETIME = '01/12/2019'
                        --DECLARE @FECHAFINALD DATETIME = '31/12/2019'

                        --SET @FECHAINICIALD = @FECHAINICIALD + '00:00:00'
                        --SET @FECHAFINALD = @FECHAFINALD + '23:59:59'

                        SELECT 
                        AC.CODPROVEEDOR, P.NOMPROVEEDOR, P.NIF20 RFC, F.FECHA_PROCESO, AC.TOTALBRUTO SUBTOTAL, AC.TOTALIMPUESTOS IMPUESTOS, AC.TOTALNETO TOTAL,
                        AC.NUMSERIEFAC, AC.NUMFAC, A.FECHA 'FECHA FACTURA',
                        AC.NUMSERIE, AC.NUMALBARAN, AC.FECHAALBARAN 'FECHA ALBARAN'
                        FROM
                        (
                        SELECT NUMSERIE, NUMALBARAN, FECHA_PROCESO
                        FROM IT_RELFACTURAS_COMPRAS WITH(NOLOCK)
                        WHERE FECHA_PROCESO BETWEEN @FECHAINICIALD AND @FECHAFINALD
                        GROUP BY NUMSERIE, NUMALBARAN, FECHA_PROCESO
                        ) F
                        INNER JOIN ALBCOMPRACAB AC WITH(NOLOCK) ON F.NUMSERIE COLLATE Latin1_General_CS_AI = AC.NUMSERIE AND F.NUMALBARAN = AC.NUMALBARAN
                        INNER JOIN PROVEEDORES P WITH(NOLOCK) ON AC.CODPROVEEDOR = P.CODPROVEEDOR
                        INNER JOIN FACTURASCOMPRA A WITH(NOLOCK) ON AC.NUMSERIEFAC = A.NUMSERIE AND AC.NUMFAC = A.NUMFACTURA AND AC.NFAC = A.N
                        WHERE AC.CODPROVEEDOR = @CODPROVEEDORD OR @CODPROVEEDORD = 0
                        ORDER BY AC.CODPROVEEDOR
                        ";


                        if (Environment.MachineName == "CTL-PC")
                        {
                            sql = "SELECT * FROM DETALLE_COMPRAS WITH (NOLOCK)";
                        }

                        Extensions.AddMap<DETALLE_COMPRAS>();

                        rs = con.Query<DETALLE_COMPRAS>(sql, req).ToDataTable(true);

                        ws = wb.Worksheets.Add("Detalle Compras");

                        row    = 2;
                        column = 2;

                        for (int j = 1; j < column; j++)
                        {
                            ws.Column(j).Width = 2.5;
                        }


                        if (rs.Rows.Count > 0)
                        {

                            IXLCell cell = ws.Cell(row, column);
                            IXLRange ran = null;

                            var tbl = cell.InsertTable(rs, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            ran = ws.Range(
                                    tbl.FirstCell().CellBelow(),
                                    tbl.LastCell());



                            if (ran != null)
                            {

                                ran.Column(tbl.Field("TOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                ran.Column(tbl.Field("SUBTOTAL").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                ran.Column(tbl.Field("IMPUESTOS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                //ran.Column(tbl.Field("IVA").Index + 1).Style.NumberFormat.Format = "$#,##0.00";
                                //ran.Column(tbl.Field("IEPS").Index + 1).Style.NumberFormat.Format = "$#,##0.00";

                                //ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                //ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                //ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                //ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;
                                //ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).DataType = XLCellValues.DateTime;

                                //ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                //ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                //ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";


                                ran.AdjustToContents();
                            }

                        }

                        //*************************************************************************************************/

                        Response.Clear();
                        Response.ContentType =
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"Reporte1.xlsx\"");

                        using (var memoryStream = new MemoryStream())
                        {

                            wb.SaveAs(memoryStream);
                            memoryStream.WriteTo(Response.OutputStream);
                        }

                        Response.End();


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
            public DateTime FECHAINICIAL { get; set; }
            public DateTime FECHAFINAL   { get; set; }
            public string   CODPROVEEDOR { get; set; }

            public DateTime FECHAINICIALD { get; set; }
            public DateTime FECHAFINALD   { get; set; }
            public string   CODPROVEEDORD { get; set; }
        }
    }
}