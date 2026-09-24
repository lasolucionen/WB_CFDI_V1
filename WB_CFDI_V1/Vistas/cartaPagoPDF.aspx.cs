using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Dapper;
using EO.Pdf;
using Microsoft.VisualBasic;

namespace WB_CFDI_V1.Vistas
{
    public partial class cartaPagoPDF : System.Web.UI.Page
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

            string pdf_id = (string)HttpContext.Current.Session["pdf_id"];
            List<TESORERIA> pdf = null;

            if (pdf_id != null)
            {
                EO.Pdf.Runtime.AddLicense(
                    "U/Ge6sUF6bua4/AAIr112Oj1y/Oy5+nOzcSIpdT1EaFZ7ekDHuio5cGz3Lhnp6ax2r11puX9F+6wtcAAHeOe6c3/Ee5Z2+UFELxbqbPD265rp7XKy7BrsbTB5a9pl8XezZ+v3PYEFO6ntKbCzZ+f4+X4Hrxbp6ax2r116u34GeCt7Pb26diRqNfH1vea3P3EA+2O4u319dmKydXO6Lto6u34GeCt7Pb26bto4+30EO2s3MJ14+30EO2s3MLNF+ic3PIEEMidtc2+AuCr3P6x3a9qsMDAF+ic3PIEEMidtcD2I++i6ekE7PN3qLbA3rBoqbTK5J9qqb7B27lpp6TS+Lto3PwBFA==");


                try
                {

                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_ICG()))
                    {

                        /*
                        string sql = @"
                        SELECT
                          SERIE
                         ,NUMERO
                         ,POSICION
                         ,FP.DESCRIPCION
                         ,FECHADOCUMENTO
                         ,SUDOCUMENTO
                         ,FECHAVENCIMIENTO
                         ,CASE
                            WHEN TOTALNETO = 0 THEN TOTALNETO
                            ELSE IMPORTE
                          END IMPORTE
                        FROM TESORERIA T WITH (NOLOCK)
                        LEFT JOIN FACTURASCOMPRA FC WITH (NOLOCK)
                          ON T.SERIE = FC.NUMSERIE
                            AND T.NUMERO = FC.NUMFACTURA
                            AND T.N = FC.N
                        INNER JOIN FORMASPAGO FP WITH (NOLOCK)
                          ON T.CODFORMAPAGO = FP.CODFORMAPAGO
                        WHERE ORIGEN = 'P'
                        AND TIPODOCUMENTO IN ('F', 'L')
                        AND NUMEFECTO = @NUMEFECTO
                        ";*/

                        /*
                        string sql = @"
                        SELECT * 
                        FROM
                        (
	                        SELECT
		                        T.SERIE,
		                        NUMERO,
		                        POSICION,
		                        DESCRIPCION,    --'FORMA DE PAGO'
		                        FECHADOCUMENTO,
		                        SUDOCUMENTO,
		                        FECHAVENCIMIENTO,
		                        IMPORTE, 
		                        CASE WHEN FF.TIENDA IS NULL THEN 0 WHEN FF.TIENDA = 'DIF' AND NO_COMPRA LIKE 'PREC%' THEN 1 ELSE 0 END 'AJUSTE'
	                        FROM TESORERIA T WITH(NOLOCK)
	                        INNER JOIN FORMASPAGO FP WITH(NOLOCK) ON T.CODFORMAPAGO = FP.CODFORMAPAGO
	                        LEFT JOIN FACTURASCOMPRA FC WITH(NOLOCK) ON T.SERIE = FC.NUMSERIE AND T.NUMERO = FC.NUMFACTURA AND T.N = FC.N
	                        LEFT JOIN CFDI_BD.DBO.CFDI_XML_PSI FF WITH(NOLOCK) ON FC.SUFACTURA	COLLATE Modern_Spanish_CI_AS = FF.SERIE + CONVERT(NVARCHAR(MAX),FOLIO)
	                        WHERE
	                        ORIGEN = 'P'
	                        AND TIPODOCUMENTO IN ('F','L')
	                        AND T.NUMEFECTO = @NUMEFECTO --No de contrarecibo (Seire-Numero)
                        ) A
                        ORDER BY AJUSTE, SUDOCUMENTO
                        ";*/

                        string sql = @"
                        SELECT *
                        FROM
                        (
                            SELECT
                        T.SERIE,
                        NUMERO,
                        POSICION,
                        DESCRIPCION,    --'FORMA DE PAGO'
                        FECHADOCUMENTO,
                        SUDOCUMENTO,
                        FECHAVENCIMIENTO,
                        IMPORTE,
                        CASE WHEN FF.TIENDA IS NULL THEN 0 WHEN FF.TIENDA = 'DIF' AND NO_COMPRA LIKE 'PREC%' THEN 1 ELSE 0 END 'AJUSTE'
                            FROM TESORERIA T WITH(NOLOCK)
                            INNER JOIN FORMASPAGO FP WITH(NOLOCK) ON T.CODFORMAPAGO = FP.CODFORMAPAGO
                        INNER JOIN PROVEEDORES P WITH(NOLOCK) ON T.CODIGOINTERNO = P.CODPROVEEDOR
                            LEFT JOIN FACTURASCOMPRA FC WITH(NOLOCK) ON T.SERIE = FC.NUMSERIE AND T.NUMERO = FC.NUMFACTURA AND T.N = FC.N
                            LEFT JOIN CFDI_BD.DBO.CFDI_XML_PSI FF WITH(NOLOCK) ON FC.SUFACTURA COLLATE Modern_Spanish_CI_AS = FF.SERIE + CONVERT(NVARCHAR(MAX),FOLIO)
                        AND (FF.RFC_EMISOR COLLATE Modern_Spanish_CI_AS = P.NIF20 )
                            WHERE
                            ORIGEN = 'P'
                            AND TIPODOCUMENTO IN ('F','L')
                            AND T.NUMEFECTO = @NUMEFECTO --No de contrarecibo (Seire-Numero)
                        ) A
                        ORDER BY AJUSTE, SUDOCUMENTO

                        ";





                        pdf = con.Query<TESORERIA>(sql, new { NUMEFECTO = Extensions.GetSerie() + "-" + pdf_id }).ToList();
                        //pdf = con.Query<TESORERIA>(sql, new { NUMEFECTO = "T278806" }).ToList();


                        /*sql =
                            @"
                        SELECT DISTINCT 
                            CODPROVEEDOR,
                            NOMPROVEEDOR,
                            NOMCOMERCIAL,
                            DIRECCION1,
                            CODPOSTAL,
                            PROVINCIA,
                            POBLACION  
                        FROM PROVEEDORES P WITH(NOLOCK)
                        INNER JOIN TESORERIA T WITH(NOLOCK) ON T.CODIGOINTERNO = P.CODPROVEEDOR
                        WHERE 
                            T.NUMEFECTO = @NUMEFECTO --No de Contrarecibo (Serie-Número)";*/


                        sql =
                            @"
                       SELECT DISTINCT 
                            CODPROVEEDOR,
                            NOMPROVEEDOR,
                            NOMCOMERCIAL,
                            DIRECCION1,
                            CODPOSTAL,
                            PROVINCIA,
                            POBLACION  
                        FROM PROVEEDORES P WITH(NOLOCK)
                        WHERE 
                            P.NIF20 = @RFC";


                        /*sql =
                            @"
                        SELECT DISTINCT 
                            CODPROVEEDOR,
                            NOMPROVEEDOR,
                            NOMCOMERCIAL,
                            DIRECCION1,
                            CODPOSTAL,
                            PROVINCIA,
                            POBLACION  
                        FROM A_PROVEEDORES
                        WHERE 
                            NUMEFECTO = @NUMEFECTO --No de contrarecibo (Seire-Numero)";*/

                        //PROVEEDORES pv = con.Query<PROVEEDORES>(sql, new { NUMEFECTO = Extensions.GetSerie() + "-" + pdf_id }).SingleOrDefault();

                        using (IDbConnection con2 = new SqlConnection(Extensions.GetBD_CFDI()))
                        {
                            CFDI_XML xm = con2.Query<CFDI_XML>(
                                "SELECT TOP 1 RFC_EMISOR FROM CFDI_XML_PSI WITH (NOLOCK) WHERE CONTRA_RECIBO_ID = @CONTRA_RECIBO_ID",
                                new {CONTRA_RECIBO_ID = pdf_id}).SingleOrDefault();


                            PROVEEDORES pv = con.Query<PROVEEDORES>(sql, new { RFC = xm.RFC_EMISOR }).FirstOrDefault();

                            //--PROVEEDORES pv = con.Query<PROVEEDORES>(sql, new { NUMEFECTO = "97" }).SingleOrDefault();

                            /*
                            for (int i = 0; i < 100; i++)
                            {
                                DateTime cc=DateTime.Now;
                                pdf.Add(new TESORERIA { DESCRIPCION = "a1", IMPORTE = 10, NUMEFECTO = "a1", SERIE = "a2", NUMERO = 10, FECHADOCUMENTO = cc, FECHAVENCIMIENTO = cc });
                            }*/


                            sql = @"SELECT * FROM dbo.CONTRA_RECIBOS_PSI WITH (NOLOCK) WHERE ID = @ID";
                            var cntr = con2.Query<CONTRA_RECIBOS>(sql, new { ID = pdf_id }).FirstOrDefault();


                            SetPDF(pdf, pdf_id, pv, cntr);
                        }
                        

                    }


                }
                catch(Exception ex)
                {
                    //Log.Write("Carta Pago: " + ex.Message);
                }
                finally
                {
                    HttpContext.Current.Session["pdf_id"] = null;
                }
            }
        }


        private void SetPDF(List<TESORERIA> rs, string id, PROVEEDORES pv, CONTRA_RECIBOS cntr)
        {
            HttpContext context = HttpContext.Current;


            string file = context.Server.MapPath("~/cartaPago.html");
            string plantilla = File.ReadAllText(file);

            var parser = new HtmlParser(Configuration.Default.WithCss());
            var document = parser.Parse(plantilla);

            PdfDocument doc = new PdfDocument();

            /*            rs = new List<CFDI_XML>();

                        for (int i = 1; i <= 12; i++)
                        {
                            rs.Add(new CFDI_XML(){SERIE = "" + i,FECHA_FACTURA = DateTime.Now,TOTAL = 128.36});
                        }*/

            int rw = 0;
            int page = 0;

            document.QuerySelector("#razonSocial1").InnerHtml = Extensions.GetRazon_Social();
            document.QuerySelector("#receptorRFC").InnerHtml = Extensions.GetReceptorRFC();

            document.QuerySelector("#CODPROVEEDOR").InnerHtml = pv.CODPROVEEDOR.Value.Str();
            document.QuerySelector("#NOMPROVEEDOR").InnerHtml = pv.NOMPROVEEDOR;
            document.QuerySelector("#NOMCOMERCIAL").InnerHtml = pv.NOMCOMERCIAL;
            document.QuerySelector("#DIRECCION1").InnerHtml = pv.DIRECCION1;
            document.QuerySelector("#CODPOSTAL").InnerHtml = pv.CODPOSTAL;
            document.QuerySelector("#POBLACION").InnerHtml = pv.POBLACION;
            document.QuerySelector("#PROVINCIA").InnerHtml = pv.PROVINCIA;

            document.QuerySelector("#cDIRECCION1").InnerHtml = Extensions.GetDireccion1();
            document.QuerySelector("#cCODPOSTAL").InnerHtml = Extensions.GetCodPostal();
            document.QuerySelector("#cPOBLACION").InnerHtml = Extensions.GetPoblacion();
            document.QuerySelector("#cPROVINCIA").InnerHtml = Extensions.GetProvincia();
            document.QuerySelector("#CONTR").InnerHtml = Extensions.GetSerie() + "-" + id;


            var dom = (IHtmlDocument)document.Clone();

            //dom.QuerySelectorAll(".head")[0].ClassList.Remove("NoShow");
            //dom.QuerySelectorAll(".head")[1].ClassList.Remove("NoShow");


            bool ok = false;
            double total = 0;
            foreach (TESORERIA it in rs)
            {
                ok = true;

                var row = dom.QuerySelectorAll(".row")[rw];
                row.ClassList.Remove("NoShow");


                if (it.AJUSTE==1)
                {
                    row.Style.BackgroundColor = "silver";
                }
                

                var td = row.QuerySelectorAll("TD");
                td[0].InnerHtml = it.SERIE;
                td[1].InnerHtml = it.NUMERO.Value.Str();
                td[2].InnerHtml = it.DESCRIPCION;
                td[3].InnerHtml = it.FECHADOCUMENTO.Value.ToString("dd/MM/yyyy");
                td[4].InnerHtml = it.SUDOCUMENTO;
                td[5].InnerHtml = it.FECHAVENCIMIENTO.Value.ToString("dd/MM/yyyy");
                td[6].InnerHtml = it.IMPORTE.Value.ToString("C2");

                total += it.IMPORTE.Value;

                rw++;
                if (page >= 0 && rw == 42) //(page == 0 && rw == 13) || 
                {
                    SetDoc(dom, doc, page, cntr);
                    dom = (IHtmlDocument)document.Clone();
                    page++;
                    rw = 0;
                }
            }

            if (ok)
            {
                dom.QuerySelectorAll(".total")[0].ClassList.Remove("NoShow");
                dom.QuerySelector("#total").InnerHtml = total.ToString("c2");
            }

            SetDoc(dom, doc, page, cntr);

            HttpResponse resp = HttpContext.Current.Response;
            resp.Clear();
            resp.ClearHeaders();
            resp.AddHeader("content-disposition", "attachment; filename=\"CartaDePago_" + Extensions.GetSerie() + "-" + id + ".pdf\"");
            resp.ContentType = "application/pdf";
            doc.Save(resp.OutputStream);

            Response.End();

        }

        private static void SetDoc(IHtmlDocument dom, PdfDocument doc, int page, CONTRA_RECIBOS cntr)
        {
            SizeF pageSize = new SizeF(8.26772f, 11.6929f); //PdfPageSizes.FromName("Letter");
            float marginLeft = 0f;
            float marginTop = 0f;
            float marginRight = 0f;
            float marginBottom = 0f;
            bool autoFitWidth = true;

            //Set page layout arguments
            HtmlToPdf.Options.ZoomLevel = 0.7f;
            HtmlToPdf.Options.PageSize = pageSize;
            HtmlToPdf.Options.OutputArea = new RectangleF(
                marginLeft,
                marginTop,
                pageSize.Width - marginLeft - marginRight,
                pageSize.Height - marginTop - marginBottom);

            if (autoFitWidth)
            {
                HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.ShrinkToFit;
            }
            else
            {
                HtmlToPdf.Options.AutoFitX = HtmlToPdfAutoFitMode.None;
            }

            string msg = "";

            if (cntr.FECHA_CIERRE != null && cntr.HORA_CIERRE != null && cntr.ORACLE)
            {
                msg = "Carta pago cerrada en la ciudad de " + Extensions.GetPoblacion().Capitalize() + " " + Extensions.GetProvincia().Capitalize() + " a " +
                      cntr.FECHA_CIERRE.Value.ToString("dd/MM/yyyy") + " " + 
                      DateTime.Parse(cntr.HORA_CIERRE.Value.ToString()).ToString("hh:mm:ss tt");
            }

            if (!cntr.ORACLE)
            {
                msg = "Carta Pago pendiente de Cierre";

            }

            
            

            HtmlToPdf.Options.FooterHtmlFormat = "<div style='text-align:center'>" + msg + "</div>";
            HtmlToPdf.Options.StartPageIndex = page;
            HtmlToPdf.Options.StartPosition = 0;

            HtmlToPdf.ConvertHtml(dom.DocumentElement.OuterHtml, doc);
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static string SetID(string id)
        {


            using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
            {


                try
                {

                    var sql = @"SELECT ORACLE FROM CONTRA_RECIBOS_PSI WITH (NOLOCK) WHERE ID = @ID";
                    var rs  = con.Query<CONTRA_RECIBOS>(sql, new { id }).FirstOrDefault();

                    if (!rs.ORACLE)
                    {
                        return "No se puede generar el archivo por que la carta pago no esta cerrada.";
                    }

                }
                catch (Exception ex)
                {
                    
                }

            }


            HttpContext.Current.Session["pdf_id"] = id;

            return "";

        }
    }
}