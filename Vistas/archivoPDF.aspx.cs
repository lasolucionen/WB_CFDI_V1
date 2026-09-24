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
using XML2;
using XML3;
using Concepto = XML3.Concepto;


namespace WB_CFDI_V1.Vistas
{
    public partial class archivoPDF : System.Web.UI.Page
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
            CFDI_XML pdf = null;

            if (pdf_id != null)
            {
                EO.Pdf.Runtime.AddLicense(
                    "U/Ge6sUF6bua4/AAIr112Oj1y/Oy5+nOzcSIpdT1EaFZ7ekDHuio5cGz3Lhnp6ax2r11puX9F+6wtcAAHeOe6c3/Ee5Z2+UFELxbqbPD265rp7XKy7BrsbTB5a9pl8XezZ+v3PYEFO6ntKbCzZ+f4+X4Hrxbp6ax2r116u34GeCt7Pb26diRqNfH1vea3P3EA+2O4u319dmKydXO6Lto6u34GeCt7Pb26bto4+30EO2s3MJ14+30EO2s3MLNF+ic3PIEEMidtc2+AuCr3P6x3a9qsMDAF+ic3PIEEMidtcD2I++i6ekE7PN3qLbA3rBoqbTK5J9qqb7B27lpp6TS+Lto3PwBFA==");

                XML3.Comprobante c33 = new XML3.Comprobante();
                XML2.Comprobante c32 = new XML2.Comprobante();



                try
                {

                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        string sql =
                            @"
                    SELECT XML,NOMBRE_ARCHIVO FROM dbo.CFDI_XML_PSI WHERE ID = @ID";

                        pdf = con.Query<CFDI_XML>(sql, new { ID = pdf_id.Value }).SingleOrDefault();

                        c33 = c33.DeserializeStr(pdf.XML);
                        c32 = c32.DeserializeStr(pdf.XML);
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


                        SetPDF(
                            c33.Emisor.Nombre,
                            c33.Emisor.Rfc,
                            "", //c33.Emisor.DomicilioFiscal.Calle,
                            "", //c33.Emisor.DomicilioFiscal.NoExterior,
                            "", //c33.Emisor.DomicilioFiscal.Colonia,
                            "", //c33.Emisor.DomicilioFiscal.Localidad,
                            "", //c33.Emisor.DomicilioFiscal.Pais,
                            "", //c33.Emisor.DomicilioFiscal.Estado,
                            "", //c33.Emisor.DomicilioFiscal.CodigoPostal,
                            c33.Emisor.RegimenFiscal,
                            c33.Folio,
                            c33.Complemento.TimbreFiscalDigital.UUID,
                            c33.NoCertificado,
                            c33.Fecha,
                            c33.Receptor.Nombre,
                            c33.Receptor.Rfc,
                            "", //c33.Receptor.Domicilio.Calle,
                            "", //c33.Receptor.Domicilio.NoExterior,
                            "", //c33.Receptor.Domicilio.Colonia,
                            "", //c33.Receptor.Domicilio.Pais,
                            "", //c33.Receptor.Domicilio.Estado,
                            "", //c33.Receptor.Domicilio.CodigoPostal,
                            c33.FormaPago,
                            c33.MetodoPago,
                            "", //c33.NumCtaPago ?? "",
                            c33.SubTotal,
                            c33.Impuestos.Traslados.Traslado.Where(w=>w.Impuesto=="002").Sum(s=>s.TasaOCuota.Dbl()).Str() /*.TasaOCuota*/,
                            c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002").Sum(s => s.Importe.Dbl()).Str(),
                            c33.Impuestos.Traslados.Traslado.Where(w=>w.Impuesto=="003").Sum(s=>s.TasaOCuota.Dbl()).Str() /*.TasaOCuota*/,
                            c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003").Sum(s => s.Importe.Dbl()).Str(),
                            c33.Total,
                            c33.LugarExpedicion,
                            conceptos,
                            c33.Complemento.TimbreFiscalDigital.SelloCFD,
                            c33.Complemento.TimbreFiscalDigital.SelloSAT,
                            c33.Complemento.TimbreFiscalDigital.Version,
                            c33.Complemento.TimbreFiscalDigital.NoCertificadoSAT,
                            c33.Complemento.TimbreFiscalDigital.FechaTimbrado,
                            pdf,
                            c33.Moneda,
                            c33.Receptor.UsoCFDI,
                            c33.Serie,
                            c33.Complemento.TimbreFiscalDigital.RfcProvCertif
                        );
                    }

                    if (c32.Version != null)
                    {
                        var conceptos = c32.Conceptos.Concepto.Select(
                            s => new XML3.Concepto
                            {
                                Cantidad      = s.Cantidad,
                                Descripcion   = s.Descripcion,
                                ValorUnitario = s.ValorUnitario,
                                Importe       = s.Importe,
                                ClaveUnidad   = s.Unidad,
                                ClaveProdServ = ""
                            }).ToList();

                        SetPDF(
                            c32.Emisor.Nombre,
                            c32.Emisor.Rfc,
                            c32.Emisor.DomicilioFiscal.Calle,
                            c32.Emisor.DomicilioFiscal.NoExterior,
                            c32.Emisor.DomicilioFiscal.Colonia,
                            c32.Emisor.DomicilioFiscal.Localidad ?? c32.Emisor.DomicilioFiscal.Municipio,
                            c32.Emisor.DomicilioFiscal.Pais,
                            c32.Emisor.DomicilioFiscal.Estado,
                            c32.Emisor.DomicilioFiscal.CodigoPostal,
                            c32.Emisor.RegimenFiscal.Regimen,
                            c32.Folio,
                            c32.Complemento.TimbreFiscalDigital.UUID,
                            c32.NoCertificado,
                            c32.Fecha,
                            c32.Receptor.Nombre,
                            c32.Receptor.Rfc,
                            c32.Receptor.Domicilio.Calle,
                            c32.Receptor.Domicilio.NoExterior,
                            c32.Receptor.Domicilio.Colonia,
                            c32.Receptor.Domicilio.Pais,
                            c32.Receptor.Domicilio.Estado,
                            c32.Receptor.Domicilio.CodigoPostal,
                            c32.FormaDePago,
                            c32.MetodoDePago,
                            c32.NumCtaPago ?? "",
                            c32.SubTotal,
                            c32.Impuestos.Traslados.Traslado.Where(w=>w.Impuesto=="002").Sum(s=>s.Tasa.Dbl()).Str(),
                            c32.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002").Sum(s => s.Importe.Dbl()).Str(),
                            c32.Impuestos.Traslados.Traslado.Where(w=>w.Impuesto=="003").Sum(s=>s.Tasa.Dbl()).Str(),
                            c32.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003").Sum(s => s.Importe.Dbl()).Str(),
                            c32.Total,
                            c32.LugarExpedicion,
                            conceptos,
                            c32.Complemento.TimbreFiscalDigital.SelloCFD,
                            c32.Complemento.TimbreFiscalDigital.SelloSAT,
                            c32.Complemento.TimbreFiscalDigital.Version,
                            c32.Complemento.TimbreFiscalDigital.NoCertificadoSAT,
                            c32.Complemento.TimbreFiscalDigital.FechaTimbrado,
                            pdf,
                            "",
                            "",
                            c32.Serie,
                            ""

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

        private void SetPDF(string emisorNombre, string emisorRFC, string emisorCalle, string emisorNoExterior, string emisorColonia, string emisorLocalidad, string emisorPais, string emisorEstado, string emisorCodigoPostal, string emisorRegimen, string folio, string complementoUUID, string noCertificado, string fecha, string receptorNombre, string receptorRFC, string receptorCalle, string receptorNoExterior, string receptorColonia, string receptorPais, string receptorEstado, string receptorCodigoPostal, string formaDePago, string metodoDePago, string numCtaPago, string subTotal, string ivaTasa, string ivaImporte, string iepsTasa, string iepsImporte, string total, string lugarExpedicion, List<Concepto> conceptos, string complementoSelloCFD, string complementoSelloSAT, string complementoVersion, string complementoNoCertificadoSAT, string complementoFechaTimbrado, CFDI_XML pdf, string moneda, string receptorUsoCFDI, string serie, string complementoRfcProvCertif)
        {
            HttpContext context = HttpContext.Current;


            string file = context.Server.MapPath("~/plantilla1.html");
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
                //{"#emisorCalle",                 emisorCalle},
                //{"#emisorNoExterior",            emisorNoExterior},
                //{"#emisorColonia",               emisorColonia},
                //{"#emisorLocalidad",             emisorLocalidad},
                //{"#emisorPais",                  emisorPais},
                //{"#emisorEstado",                emisorEstado},
                //{"#emisorCodigoPostal",          emisorCodigoPostal},
                {"#emisorRegimen",               emisorRegimen.Descr()},
                {"#lugarExpedicion",             lugarExpedicion},
                {"#folio",                       folio},
                {"#serie",                       serie},
                {"#complementoUUID",             complementoUUID},
                {"#noCertificado",               noCertificado},
                {"#fecha",                       fecha},
                {"#receptorNombre",              receptorNombre},
                {"#receptorRFC",                 receptorRFC},
                //{"#receptorCalle",               receptorCalle},
                //{"#receptorNoExterior",          receptorNoExterior},
                //{"#receptorColonia",             receptorColonia},
                //{"#receptorPais",                receptorPais},
                //{"#receptorEstado",              receptorEstado},
                //{"#receptorCodigoPostal",        receptorCodigoPostal},
                {"#formaDePago",                 formaDePago.Descr()},
                {"#metodoDePago",                metodoDePago.Descr()},
                {"#moneda",                      moneda},
                //{"#numCtaPago",                  numCtaPago},
                {"#receptorUsoCFDI",             receptorUsoCFDI.Descr()},
                {"#subTotal",                    subTotal.Dbl().ToString("c2")},
                //{"#ivaTasa",                     (ivaTasa.Dbl() * 100).ToString()},
                {"#ivaImporte",                  ivaImporte.Dbl().ToString("c2")},               
                //{"#iepsTasa",                    (iepsTasa.Dbl() * 100).ToString()},
                {"#iepsImporte",                 iepsImporte.Dbl().ToString("c2")},
                {"#total",                       total.Dbl().ToString("c2")},
                {"#complementoSelloCFD",         complementoSelloCFD.WordWrap(108)},
                {"#complementoSelloSAT",         complementoSelloSAT.WordWrap(108)},
                {"#complementoCadena",           cadena.WordWrap(140)},
                {"#complementoNoCertificadoSAT", complementoNoCertificadoSAT},
                {"#complementoFechaTimbrado",    complementoFechaTimbrado},
            };


            PdfDocument doc = new PdfDocument();



            var rows = document.QuerySelectorAll(".dataRow1");


            int pages = Math.Ceiling(conceptos.Count / rows.Length.Dbl()).Int32();
            int k = 0;
            for (int i = 1; i <= pages; i++)
            {

                var dom = parser.Parse(document.DocumentElement.OuterHtml);

                foreach (var row in dom.QuerySelectorAll(".dataRow1"))
                {
                    var dt = row.QuerySelectorAll("TD");


                    dt[0].InnerHtml = conceptos[k].Cantidad.Dbl().ToString();
                    dt[1].InnerHtml = conceptos[k].ClaveUnidad;
                    dt[2].InnerHtml = conceptos[k].ClaveProdServ;
                    dt[3].InnerHtml = conceptos[k].Descripcion.Replace(" ", "  ").WordWrap(120);
                    dt[4].InnerHtml = conceptos[k].ValorUnitario.Dbl().ToString("N2");
                    dt[5].InnerHtml = conceptos[k].Importe.Dbl().ToString("N2");

                    k++;
                    if (conceptos.Count == k)
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