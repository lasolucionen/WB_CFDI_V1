using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Dapper;
using EO.Pdf;
using Newtonsoft.Json;
using WB_CFDI_V1.Properties;
using XML4;
using Concepto = XML3.Concepto;


namespace WB_CFDI_V1.Vistas
{
    public partial class archivoPDF2 : System.Web.UI.Page
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

            int? pdf_id = (int?)HttpContext.Current.Session["pdf_id"];
            CFDI_XML_COMPLEMENTO_PAGO pdf = null;

            if (pdf_id != null)
            {
                EO.Pdf.Runtime.AddLicense(
                    "U/Ge6sUF6bua4/AAIr112Oj1y/Oy5+nOzcSIpdT1EaFZ7ekDHuio5cGz3Lhnp6ax2r11puX9F+6wtcAAHeOe6c3/Ee5Z2+UFELxbqbPD265rp7XKy7BrsbTB5a9pl8XezZ+v3PYEFO6ntKbCzZ+f4+X4Hrxbp6ax2r116u34GeCt7Pb26diRqNfH1vea3P3EA+2O4u319dmKydXO6Lto6u34GeCt7Pb26bto4+30EO2s3MJ14+30EO2s3MLNF+ic3PIEEMidtc2+AuCr3P6x3a9qsMDAF+ic3PIEEMidtcD2I++i6ekE7PN3qLbA3rBoqbTK5J9qqb7B27lpp6TS+Lto3PwBFA==");

                XML4.Comprobante c33 = new XML4.Comprobante();




                try
                {

                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        string sql =
                            @"
                    SELECT XML,NOMBRE_ARCHIVO FROM dbo.CFDI_XML_COMPLEMENTO_PAGO WITH (NOLOCK) WHERE ID = @ID";

                        pdf = con.Query<CFDI_XML_COMPLEMENTO_PAGO>(sql, new { ID = pdf_id.Value }).SingleOrDefault();

                        c33 = c33.DeserializeStr(pdf.XML);
                    }

                    if (c33.Version != null)
                    {
                        var conceptos = c33.Conceptos.Concepto.Select(
                            s => new XML3.Concepto
                            {
                                Cantidad      = s.Cantidad,
                                Descripcion   = s.Descripcion,
                                ValorUnitario = s.ValorUnitario,
                                Importe       = s.Importe,
                                ClaveUnidad   = s.ClaveUnidad,
                                ClaveProdServ = s.ClaveProdServ
                            }).ToList();

                        string ivaTasa = "";
                        string ivaImporte = "";

                        //if (c33.Impuestos != null)
                        {
                            //ivaTasa = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002").Sum(s => s.TasaOCuota.Dbl()).Str();
                            //ivaImporte = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002").Sum(s => s.Importe.Dbl()).Str();
                        }

                        

                        SetPDF(
                            c33.Emisor.Nombre,
                            c33.Emisor.Rfc, //c33.Emisor.DomicilioFiscal.CodigoPostal,
                            c33.Emisor.RegimenFiscal,
                            c33.Folio,
                            c33.Complemento.TimbreFiscalDigital.UUID,
                            c33.NoCertificado,
                            c33.Fecha,
                            c33.Receptor.Nombre,
                            c33.Receptor.Rfc,
                            c33.LugarExpedicion,
                            c33.Complemento.TimbreFiscalDigital.SelloCFD,
                            c33.Complemento.TimbreFiscalDigital.SelloSAT,
                            c33.Complemento.TimbreFiscalDigital.Version,
                            c33.Complemento.TimbreFiscalDigital.NoCertificadoSAT,
                            c33.Complemento.TimbreFiscalDigital.FechaTimbrado,
                            pdf,
                            c33.Receptor.UsoCFDI,
                            c33.Serie,
                            c33.Complemento.TimbreFiscalDigital.RfcProvCertif,
                            c33.Complemento.Pagos.Pago.DoctoRelacionado
                        );
                    }





                }
                catch
                {

                }
                finally
                {
                    HttpContext.Current.Session["pdf_id"] = null;
                }
            }
        }


        class SetP : IEnumerable
        {
            public IHtmlDocument document;

            public SetP(IHtmlDocument document)
            {
                this.document = document;
            }

            public void Add(string selectors, string html)
            {
                document.QuerySelector(selectors).InnerHtml = html;
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        private void SetPDF(string emisorNombre, string emisorRFC, string emisorRegimen, string folio,
            string complementoUUID, string noCertificado,
            string fecha, string receptorNombre, string receptorRFC,
            string lugarExpedicion,
            string complementoSelloCFD, string complementoSelloSAT, string complementoVersion,
            string complementoNoCertificadoSAT, string complementoFechaTimbrado, CFDI_XML_COMPLEMENTO_PAGO pdf,
            string receptorUsoCFDI, string serie, string complementoRfcProvCertif,
            List<DoctoRelacionado> pagoDoctoRelacionado)
        {
            HttpContext context = HttpContext.Current;


            string file = context.Server.MapPath("~/plantilla3.html");
            string plantilla = File.ReadAllText(file);

            var parser   = new HtmlParser();
            var document = parser.Parse(plantilla);

            var cadena = string.Format(
                "||{0}|{1}|{2}|{3}|{4}|{5}||",
                complementoVersion,
                complementoUUID,
                complementoFechaTimbrado,
                complementoRfcProvCertif,
                complementoSelloCFD,
                complementoNoCertificadoSAT
            );

            new SetP(document)
            {
                {"#emisorNombre",                emisorNombre},
                {"#emisorRFC",                   emisorRFC},
                {"#emisorRegimen",               emisorRegimen.Descr()},
                {"#lugarExpedicion",             lugarExpedicion},
                {"#folio",                       folio},
                {"#serie",                       serie},
                {"#complementoUUID",             complementoUUID},
                {"#noCertificado",               noCertificado},
                {"#fecha",                       fecha},
                {"#receptorNombre",              receptorNombre},
                {"#receptorRFC",                 receptorRFC},
                {"#receptorUsoCFDI",             receptorUsoCFDI.Descr()},
                {"#complementoSelloCFD",         complementoSelloCFD.WordWrap(108)},
                {"#complementoSelloSAT",         complementoSelloSAT.WordWrap(108)},
                {"#complementoCadena",           cadena.WordWrap(140)},
                {"#complementoNoCertificadoSAT", complementoNoCertificadoSAT},
                {"#complementoFechaTimbrado",    complementoFechaTimbrado},
            };


            PdfDocument doc = new PdfDocument();



            var rows = document.QuerySelectorAll(".dataRow1");

            var gp = pagoDoctoRelacionado.GroupBy(g => g.IdDocumento).ToDictionary(ky => ky.Key, v => v.FirstOrDefault());
            
            var d=new List<DoctoRelacionado>();

            foreach (var pair in gp)
            {
                d.Add(pair.Value);
            }


            int pages = Math.Ceiling(d.Count / rows.Length.Dbl()).Int32();
            int k = 0;
            for (int i = 1; i <= pages; i++)
            {

                var dom = parser.Parse(document.DocumentElement.OuterHtml);

                foreach (var row in dom.QuerySelectorAll(".dataRow1"))
                {
                    var dt = row.QuerySelectorAll("TD");


                    dt[0].InnerHtml = d[k].Serie;
                    dt[1].InnerHtml = d[k].Folio;
                    dt[2].InnerHtml = d[k].ImpSaldoAnt;
                    dt[3].InnerHtml = d[k].ImpPagado;
                    dt[4].InnerHtml = d[k].ImpSaldoInsoluto;
                    dt[5].InnerHtml = d[k].MonedaDR;
                    dt[6].InnerHtml = d[k].MetodoDePagoDR;
                    dt[7].InnerHtml = d[k].IdDocumento;

                    k++;
                    if (d.Count == k)
                    {
                        break;
                    }

                }

                SizeF pageSize = PdfPageSizes.FromName("Letter");
                float marginLeft = 0.3f;
                float marginTop = 0.3f;
                float marginRight = 0.3f;
                float marginBottom = 0.3f;
                bool  autoFitWidth = true;

                //Set page layout arguments
                HtmlToPdf.Options.ZoomLevel = 0.75f;
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

                HtmlToPdf.Options.StartPageIndex = i - 1;
                HtmlToPdf.Options.StartPosition = 0;
                HtmlToPdf.Options.FooterHtmlFormat = string.Format(@"
                <hr />
                <table width='100%'>
                    <tr>
                        <td style='font-weight: bold;'>Este documento es una representación impresa de un CFDI</td>
                        <td style='text-align:right'>
                            Página {0}/{1}
                        </td>
                    </tr>
                </table>", i, pages);

                HtmlToPdf.ConvertHtml(dom.DocumentElement.OuterHtml, doc);
            }




            HttpResponse resp = HttpContext.Current.Response;
            resp.Clear();
            resp.ClearHeaders();
            resp.AddHeader("content-disposition", "attachment; filename=\"" + pdf.NOMBRE_ARCHIVO + ".pdf\"");
            resp.ContentType = "application/pdf";
            doc.Save(resp.OutputStream);

            Response.End();
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static void SetID(int id)
        {
            HttpContext.Current.Session["pdf_id"] = id;
        }
    }
}