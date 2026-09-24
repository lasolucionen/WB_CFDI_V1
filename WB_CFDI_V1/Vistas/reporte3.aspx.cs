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
    public partial class reporte3 : System.Web.UI.Page
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

                        var emp = Extensions.cEmpresa();

                        string sql =
                                @"
                            SELECT
                              xml.CONTRA_RECIBO_ID
                             ,xml.UUID
                             ,xml.XML
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

                            FROM CFDI_XML_PSI xml WITH (NOLOCK)
                            INNER JOIN CFDI_ESTATUS es WITH (NOLOCK)
                              ON xml.ESTATUS = es.ID

                        WHERE FECHA_FACTURA
                        BETWEEN CAST((@fecha1 + ' 00:00:00') AS DATETIME) AND CAST((@fecha2 + ' 23:59:59') AS DATETIME)
                        AND (@RFC_EMISOR IS NULL OR xml.RFC_EMISOR = @RFC_EMISOR)
                        AND (@ESTATUS = 0 OR xml.ESTATUS = @ESTATUS)
                        AND (@A IS NULL OR @A = '1' AND NOT xml.CONTRA_RECIBO_ID IS NULL
                                        OR @A = '2' AND xml.CONTRA_RECIBO_ID IS NULL)
                        ORDER BY xml.CONTRA_RECIBO_ID DESC
                        ";

                        if (req.ESTATUS == 4)
                        {
                            //req.CONTR = "";
                            req.A = "1";
                            req.ESTATUS = 1;
                        }

                        if (req.ESTATUS == 5)
                        {
                            req.CONTR = "";
                            req.A = "2";
                            req.ESTATUS = 1;
                        }






                        DataTable rs = con.Query<CFDI_XML>(sql, req)
                                .Select(
                                        s => new
                                        {
                                            s.RFC_EMISOR,
                                            s.RAZON_SOCIAL_EMISOR,
                                            s.CONTRA_RECIBO_ID,
                                            s.UUID,
                                            s.XML,
                                            s.SERIE,
                                            s.FOLIO,
                                            s.SUBTOTAL,
                                            s.IMPUESTOS,
                                            s.TOTAL,
                                            s.TIPO_COMPROBANTE,
                                            s.ESTATUS2,
                                            s.FECHA_RECEPCION,
                                            s.FECHA_FACTURA,

                                            //s.NUMSERIE,
                                            //s.NUMALBARAN,
                                            //s.NUMSERIEFAC,
                                            //s.NUMFAC,
                                            //s.FECHA_PROCESO,
                                        })

                                .ToDataTable(true);



                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("Reporte Facturas");

                        int row = 2;
                        int column = 2;

                        for (int j = 1; j < column; j++)
                        {
                            ws.Column(j).Width = 2.5;
                        }

                        if (rs.Rows.Count > 0)
                        {

                            IXLCell cell = ws.Cell(row, column);
                            IXLRange ran = null;




                            DataTable tbl2;
                            List<DETALLE> ds = new List<DETALLE>();

                            foreach (DataRow x1 in rs.Rows)
                            {
                                tbl2 = rs.Clone();
                                tbl2.ImportRow(x1);


                                XML3.Comprobante c33 = new XML3.Comprobante();
                                c33 = c33.DeserializeStr(x1["XML"].ToString());



                                tbl2.Columns.Remove("XML");



                                /*
                                var tbl = cell.InsertTable(tbl2, true);
                                tbl.Theme = XLTableTheme.TableStyleMedium3;

                                ran = ws.Range(
                                        tbl.FirstCell().CellBelow(),
                                        tbl.LastCell());
                                */

                                /*
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


                                ran.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                                ran.AdjustToContents();
                                //ws.Column(15).Width = 150;


                                cell = ws.Cell(ran.LastRowUsed().RowNumber() + 3, column);

                                */




                                if (c33.Version != null)
                                {

                                    var sql1 = @"
                                    SELECT
                                      a.CODARTICULO
                                     ,a.REFPROVEEDOR
                                     ,c.DESCRIPCION AS MARCA
                                     ,d.DESCRIPCION AS SUBFAMILIA
                                    FROM dbo.IT_REL_ARTPROV b
                                    INNER JOIN dbo.ARTICULOS a
                                      ON b.CODARTICULO = a.CODARTICULO
                                        AND b.REFPROVEEDOR COLLATE Latin1_General_CS_AI = a.REFPROVEEDOR
                                    INNER JOIN dbo.MARCA c
                                      ON c.CODMARCA = a.MARCA
                                    INNER JOIN dbo.SUBFAMILIAS d
                                      ON a.DPTO = d.NUMDPTO
                                        AND a.SECCION = d.NUMSECCION
                                        AND a.FAMILIA = d.NUMFAMILIA
                                        AND a.SUBFAMILIA = d.NUMSUBFAMILIA

                                            WHERE b.[CODARTICULO PROV] = @CODARTICULO_PROV";



                                    var art = new Dictionary<string, IT_REL_ARTPROV>();

                                    foreach (var cp in c33.Conceptos.Concepto)
                                    {

                                        if (cp.NoIdentificacion != null && !art.ContainsKey(cp.NoIdentificacion))
                                        {
                                            using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_ICG()))
                                            {
                                                IT_REL_ARTPROV rs2 = con2.Query<IT_REL_ARTPROV>(sql1,
                                                    new { CODARTICULO_PROV = cp.NoIdentificacion }).FirstOrDefault();

                                                if (rs2 != null)
                                                {
                                                    art.Add(cp.NoIdentificacion, rs2);
                                                }
                                            }
                                        }

                                        IT_REL_ARTPROV rel;
                                        var CODARTICULO = "";
                                        var REFERENCIA = "";
                                        var MARCA = "";
                                        var SUBFAMILIA = "";

                                        if (cp.NoIdentificacion != null && art.TryGetValue(cp.NoIdentificacion, out rel))
                                        {
                                            CODARTICULO = rel.CODARTICULO;
                                            REFERENCIA = rel.REFPROVEEDOR;
                                            MARCA = rel.MARCA;
                                            SUBFAMILIA = rel.SUBFAMILIA;
                                        }

                                        ds.Add(
                                            new DETALLE
                                            {
                                                CODARTICULO      = CODARTICULO,
                                                REFERENCIA       = REFERENCIA,
                                                CANTIDAD         = cp.Cantidad.Dbl().ToString(),
                                                UNIDAD           = cp.ClaveUnidad,
                                                CODARTICULO_PROV = cp.NoIdentificacion,
                                                DESCRIPCION      = cp.Descripcion,
                                                MARCA            = MARCA,
                                                SUBFAMILIA       = SUBFAMILIA,
                                                PU               = cp.ValorUnitario.Dbl().ToString("N2"),
                                                IMPORTE          = cp.Importe.Dbl().ToString("N2"),
                                                DESCUENTO        = cp.Descuento.Dbl().ToString("N2"),
                                                SERIE            = x1["SERIE"].ToString(),
                                                FOLIO            = x1["FOLIO"].ToString(),
                                                TIPO_COMPROBANTE = x1["TIPO_COMPROBANTE"].ToString(),
                                                ESTATUS2         = x1["ESTATUS2"].ToString(),
                                                FECHA_RECEPCION  = x1["FECHA_RECEPCION"].ToString(),
                                                FECHA_FACTURA    = x1["FECHA_FACTURA"].ToString(),
                                                UUID             = x1["UUID"].ToString()
                                            });
                                    }


                                }

                            }

                            var tbl = cell.InsertTable(ds, true);
                            tbl.Theme = XLTableTheme.TableStyleMedium2;

                            var head = true;
                            var oldClave = "";
                            var htmlColor = "";

                            foreach (var rw in tbl.Rows())
                            {
                                if (!head)
                                {
                                    var it = rw.Cell(tbl.Field("UUID").Index + 1).Value.ToString();

                                    if (oldClave != it)
                                    {
                                        oldClave = it;


                                        if (htmlColor == "#DCE6F1")
                                        {
                                            htmlColor = "#FFF";
                                        }
                                        else
                                        {
                                            htmlColor = "#DCE6F1";
                                        }
                                    }

                                    foreach (var cll in rw.Cells())
                                    {
                                        cll.Style.Fill.BackgroundColor = XLColor.FromHtml(htmlColor);
                                    }
                                }

                                head = false;

                                //rw.Cell(1).Style.Fill.BackgroundColor = XLColor.FromHtml("#DCE6F1");
                            }

                            ran = ws.Range(
                                tbl.FirstCell().CellBelow(),
                                tbl.LastCell());


                            ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("MARCA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("SUBFAMILIA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ran.Column(tbl.Field("CANTIDAD").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).DataType = XLCellValues.DateTime;
                            ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).DataType = XLCellValues.DateTime;

                            ran.Column(tbl.Field("FECHA_FACTURA").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";
                            ran.Column(tbl.Field("FECHA_RECEPCION").Index + 1).Style.DateFormat.Format = "dd/mm/yyyy";

                            ran.AdjustToContents();



                            //cell = ws.Cell(ran.LastRowUsed().RowNumber() + 3, column);

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
                        var err = ex.Message;

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

        public class DETALLE
        {
            public string CODARTICULO      { get; set; }
            public string REFERENCIA       { get; set; }
            public string DESCRIPCION      { get; set; }
            public string MARCA            { get; set; }
            public string SUBFAMILIA       { get; set; }
            public string CANTIDAD         { get; set; }
            public string UNIDAD           { get; set; }
            public string CODARTICULO_PROV { get; set; }
            public string PU               { get; set; }
            public string IMPORTE          { get; set; }
            public string DESCUENTO        { get; set; }
            public string SERIE            { get; set; }
            public string FOLIO            { get; set; }
            public string TIPO_COMPROBANTE { get; set; }
            public string ESTATUS2         { get; set; }
            public string FECHA_RECEPCION  { get; set; }
            public string FECHA_FACTURA    { get; set; }
            public string UUID             { get; set; }
        }
    }
}