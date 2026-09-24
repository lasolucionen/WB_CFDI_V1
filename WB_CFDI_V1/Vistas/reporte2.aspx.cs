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
    public partial class reporte2 : System.Web.UI.Page
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

                using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                {
                    try
                    {

                        /*
                                                 SELECT
                                                  --xml.ID,
                                                  --xml.USER_ID,
                                                  xml.CONTRA_RECIBO_ID,
                                                  xml.UUID,
                                                  --xml.XML,
                                                  xml.RAZON_SOCIAL_EMISOR,
                                                  xml.RFC_EMISOR,
                                                  xml.TOTAL,
                                                  xml.SUBTOTAL,
                                                  xml.IMPUESTOS,
                                                  xml.FECHA_RECEPCION,
                                                  xml.FECHA_FACTURA,
                                                  xml.SERIE,
                                                  xml.FOLIO,
                                                  --xml.TIENDA,
                                                  --xml.NO_COMPRA,
                                                  --xml.FECHA_COMPRA,
                                                  --xml.REVISADO,
                                                  --xml.NOMBRE_ARCHIVO,
                                                  --xml.ESTATUS,
                                                  es.ESTATUS AS ESTATUS2,
                                                  xml.TIPO_COMPROBANTE
                                                FROM dbo.CFDI_XML_PSI xml
                                                INNER JOIN dbo.CFDI_ESTATUS es ON xml.ESTATUS = es.ID
                                                WHERE FECHA_FACTURA
                                                BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                                                AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)
                                                AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                                                AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                                                OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                                                ORDER BY xml.CONTRA_RECIBO_ID DESC
                         */



                        /*
                        SELECT
                          xml.CONTRA_RECIBO_ID
                         ,xml.UUID
                         ,xml.RAZON_SOCIAL_EMISOR
                         ,xml.RFC_EMISOR
                         ,xml.TOTAL
                         ,xml.SUBTOTAL
                         ,xml.IMPUESTOS
                         ,xml.FECHA_RECEPCION
                         ,xml.FECHA_FACTURA
                         ,xml.SERIE
                         ,xml.FOLIO
                         ,es.ESTATUS AS ESTATUS2
                         ,xml.TIPO_COMPROBANTE
                         --,IT_RELFACTURAS_COMPRAS.NUMSERIE
                         ,IT_RELFACTURAS_COMPRAS.NUMALBARAN
                         ,IT_RELFACTURAS_COMPRAS.NUMSERIEFAC
                         ,IT_RELFACTURAS_COMPRAS.NUMFAC
                         ,IT_RELFACTURAS_COMPRAS.FECHA_PROCESO
                        FROM CFDI_XML_PSI xml
                        INNER JOIN CFDI_ESTATUS es
                          ON xml.ESTATUS = es.ID
                        LEFT OUTER JOIN " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS
                          ON xml.UUID = IT_RELFACTURAS_COMPRAS.FOLIOFISCAL
                        WHERE FECHA_FACTURA
                        BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                        AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)
                        AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                        AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                        ORDER BY xml.CONTRA_RECIBO_ID DESC
                         */

                        var emp = Extensions.cEmpresa();

                        string sql =
                                @"
                            SELECT
                              xml.CONTRA_RECIBO_ID
                             ,xml.UUID
                             ,xml.RAZON_SOCIAL_EMISOR
                             ,xml.RFC_EMISOR
                             ,xml.TOTAL
                             ,xml.SUBTOTAL
                             ,xml.IMPUESTOS
                             ,xml.FECHA_RECEPCION
                             ,xml.FECHA_FACTURA
                             ,xml.SERIE
                             ,xml.FOLIO

                             ,CASE
                                WHEN (b.CHECK_ID IS NOT NULL) THEN 'Oracle/Pagado'
                                ELSE es.ESTATUS
                              END ESTATUS2


                             ,xml.TIPO_COMPROBANTE

                             ,ic.FECHA_PROCESO
                             ,b.CHECK_ID
                             ,b.CHECK_NUMBER
                             ,b.CHECK_DATE
                             ,
                             
                             (SELECT Stuff(
                               (SELECT N', ' + b.NUMSERIE + '-' + CAST(b.NUMALBARAN as varchar(10)) FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS b WITH (NOLOCK) WHERE b.FOLIOFISCAL = xml.UUID
                                  FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,2,N'') ) [NUMSERIE_NUMALBARAN]


                            FROM CFDI_XML_PSI xml WITH (NOLOCK)
                            INNER JOIN CFDI_ESTATUS es WITH (NOLOCK)
                              ON xml.ESTATUS = es.ID


                            LEFT OUTER JOIN CFDI_GENERAL.dbo.XXROD_AP_PAYMENTS_BY_INVOICE b WITH (NOLOCK) ON xml.UUID = b.UUID



                        LEFT OUTER JOIN
                                 (SELECT DISTINCT 
                                    MAX(FECHA_PROCESO) FECHA_PROCESO,FOLIOFISCAL
                                    FROM " + emp.ICG_BD + @".dbo.IT_RELFACTURAS_COMPRAS WITH (NOLOCK) group by FOLIOFISCAL) ic
                                        ON xml.UUID = ic.FOLIOFISCAL

                        WHERE FECHA_FACTURA
                        BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                        AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)
                        --AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                        AND (@ESTATUS = 0 OR (@ESTATUS = 7 AND b.CHECK_ID IS NOT NULL) OR xml.ESTATUS = @ESTATUS)


                        AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                        ORDER BY xml.CONTRA_RECIBO_ID DESC
                        ";

                        if (req.ESTATUS == 4)
                        {
                            //req.CONTR = "";
                            req.A       = "1";
                            req.ESTATUS = 1;
                        }

                        if (req.ESTATUS == 5)
                        {
                            req.CONTR   = "";
                            req.A       = "2";
                            req.ESTATUS = 1;
                        }


                        DataTable rs = con.Query<CFDI_XML>(sql, req,commandTimeout:6000)
                                .Select(
                                        s => new {
                                                     s.RFC_EMISOR,
                                                     s.RAZON_SOCIAL_EMISOR,
                                                     s.CONTRA_RECIBO_ID,
                                                     s.UUID,
                                                     s.SERIE,
                                                     s.FOLIO,
                                                     s.SUBTOTAL,
                                                     s.IMPUESTOS,
                                                     s.TOTAL,
                                                     s.TIPO_COMPROBANTE,
                                                     s.ESTATUS2,
                                                     s.FECHA_RECEPCION,
                                                     s.FECHA_FACTURA,
                                                     s.FECHA_PROCESO,
                                                     s.CHECK_ID,
                                                     s.CHECK_NUMBER,
                                                     s.CHECK_DATE,
                                                     s.NUMSERIE_NUMALBARAN,

                                                     //s.NUMSERIE,
                                                     //s.NUMALBARAN,
                                                     //s.NUMSERIEFAC,
                                                     //s.NUMFAC,
                                                     //s.FECHA_PROCESO,
                                                 })

                                .ToDataTable(true);

                        foreach (DataRow it in rs.Rows)
                        {
                            var data = it["NUMSERIE - NUMALBARAN"].ToString();

                            int count = 0;
                            string filas = "";
                            foreach (var t in data.Split(","))
                            {
                                if (count == 15)
                                {
                                    filas += Environment.NewLine;
                                    count = 0;
                                }


                                filas += t + ",";
                                count++;

                            }

                            it["NUMSERIE - NUMALBARAN"] = filas.TrimEnd(',');

                            //if (req.ESTATUS == 7)
                            //{
                            //    it["ESTATUS2"] = "Oracle/Pagado";
                            //}
                        }

                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("Reporte Facturas");

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

                                    ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("ESTATUS2").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    //ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;
                                    //ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).DataType = XLCellValues.DateTime;

                                    ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy hh:mm AM/PM";
                                    ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                                    //ran.Column(tbl.Field("FECHA_PROCESO").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";

                                    ran.Column(tbl.Field("NUMSERIE - NUMALBARAN").Index + 1).Style.Alignment.SetWrapText(true);
                                    ran.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                                ran.AdjustToContents();
                                //ws.Column(15).Width = 150;
                            }

                        }

                        

                        //*************************************************************************************************/

                        Response.Clear();
                        Response.ContentType =
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        Response.AddHeader(
                                "content-disposition",
                                "attachment;filename=\"Reporte Facturas.xlsx\"");

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
            public DateTime FECHA1     { get; set; }
            public DateTime FECHA2     { get; set; }
            public int      ESTATUS    { get; set; }
            public string   RFC_EMISOR { get; set; }
            public string   CONTR      { get; set; }
            public string   A          { get; set; }

        }
    }
}