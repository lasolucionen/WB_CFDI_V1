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
using XML3;

namespace WB_CFDI_V1.Vistas
{
    public partial class contrareciboPDF : System.Web.UI.Page
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
            List<CFDI_XML> pdf = null;

            if (pdf_id != null)
            {
                EO.Pdf.Runtime.AddLicense(
                    "U/Ge6sUF6bua4/AAIr112Oj1y/Oy5+nOzcSIpdT1EaFZ7ekDHuio5cGz3Lhnp6ax2r11puX9F+6wtcAAHeOe6c3/Ee5Z2+UFELxbqbPD265rp7XKy7BrsbTB5a9pl8XezZ+v3PYEFO6ntKbCzZ+f4+X4Hrxbp6ax2r116u34GeCt7Pb26diRqNfH1vea3P3EA+2O4u319dmKydXO6Lto6u34GeCt7Pb26bto4+30EO2s3MJ14+30EO2s3MLNF+ic3PIEEMidtc2+AuCr3P6x3a9qsMDAF+ic3PIEEMidtcD2I++i6ekE7PN3qLbA3rBoqbTK5J9qqb7B27lpp6TS+Lto3PwBFA==");


                try
                {

                    using (IDbConnection con = new SqlConnection(Extensions.GetBD_CFDI()))
                    {
                        string sql =
                            @"
                            SELECT
                                XML,
                                RAZON_SOCIAL_EMISOR,
                                RFC_EMISOR,
                                SERIE,
                                FOLIO,
                                TOTAL,
                                SUBTOTAL,
                                FECHA_FACTURA,
                                TIPO_COMPROBANTE
                            FROM dbo.CFDI_XML_PSI WITH (NOLOCK) WHERE CONTRA_RECIBO_ID = @ID";

                        pdf = con.Query<CFDI_XML>(sql, new { ID = pdf_id.Value }).ToList();

                        foreach (var xml in pdf)
                        {
                            try
                            {
                                Comprobante c33 = new Comprobante();
                                c33 = c33.DeserializeStr(xml.XML);

                                if (c33.Impuestos != null && c33.Impuestos.Traslados != null && c33.Impuestos.Traslados.Traslado != null)
                                {
                                    xml.IVA = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "002")
                                        .Sum(s => s.Importe.Dbl());

                                    xml.IEPS = c33.Impuestos.Traslados.Traslado.Where(w => w.Impuesto == "003")
                                        .Sum(s => s.Importe.Dbl());

                                }

                            }
                            catch (Exception exception)
                            {

                            }


                            if (xml.TIPO_COMPROBANTE != null && (xml.TIPO_COMPROBANTE.ToLower() == "e" || xml.TIPO_COMPROBANTE.ToLower() == "egreso"))
                            {
                                if (xml.TOTAL != null) xml.TOTAL = xml.TOTAL * -1;
                                if (xml.SUBTOTAL != null) xml.SUBTOTAL = xml.SUBTOTAL * -1;
                                if (xml.IMPUESTOS != null) xml.IMPUESTOS = xml.IMPUESTOS * -1;

                                if (xml.IVA != null) xml.IVA = xml.IVA * -1;
                                if (xml.IEPS != null) xml.IEPS = xml.IEPS * -1;
                            }


                        }

                        var contr = con.Query<CONTRA_RECIBOS>("SELECT * FROM CONTRA_RECIBOS_PSI WITH (NOLOCK) WHERE ID = @ID", new { ID = pdf_id.Value }).FirstOrDefault();

                        SetPDF(pdf, pdf_id.Value, contr);


                    }


                }
                catch(Exception ex)
                {

                     if (!ex.Message.Contains("Subproceso anulado."))
                     {
                         Log.Write("contrareciboPDF: " + ex.Message + " ID: " + pdf_id);
                     }
                }
                finally
                {
                    HttpContext.Current.Session["pdf_id"] = null;
                }
            }
        }

        private void SetPDF(List<CFDI_XML> rs, int id, CONTRA_RECIBOS contr)
        {
            try
            {
                HttpContext context = HttpContext.Current;


                string file      = context.Server.MapPath("~/contrarecibo.html");
                string plantilla = File.ReadAllText(file);

                var parser   = new HtmlParser(Configuration.Default.WithCss());
                var document = parser.Parse(plantilla);

                PdfDocument doc = new PdfDocument();

/*            rs = new List<CFDI_XML>();

            for (int i = 1; i <= 12; i++)
            {
                rs.Add(new CFDI_XML(){SERIE = "" + i,FECHA_FACTURA = DateTime.Now,TOTAL = 128.36});
            }*/

                int rw   = 0;
                int page = 0;

                document.QuerySelector("#contrarecibo").InnerHtml   = Extensions.GetSerie() + "-" + id.Str();
                document.QuerySelector("#razonSocial1").InnerHtml   = Extensions.GetRazon_Social() + " (" + Extensions.GetReceptorRFC() + ")";
                document.QuerySelector("#razonSocial2").InnerHtml   = rs[0].RAZON_SOCIAL_EMISOR;
                document.QuerySelector("#rfcEmisor").InnerHtml      = "(" + rs[0].RFC_EMISOR + ")";

                document.QuerySelector("#fechaRecepcion").InnerHtml = contr.FECHA.ToString("dd/MM/yyyy");
                document.QuerySelector("#fechaPago").InnerHtml      = contr.FECHA_PAGO.ToString("dd/MM/yyyy");
                document.QuerySelector("#nombre").InnerHtml         = "";
                document.QuerySelector("#observacion").InnerHtml    = Convert.ToString(contr.OBSERVACION);

                var dom = (IHtmlDocument)document.Clone();

                dom.QuerySelectorAll(".head")[0].ClassList.Remove("NoShow");
                dom.QuerySelectorAll(".head")[1].ClassList.Remove("NoShow");


                bool   ok       = false;
                double total    = 0;
                double iva      = 0;
                double ieps     = 0;
                double subTotal = 0;


                foreach (CFDI_XML xml in rs)
                {
                    ok = true;

                    var row = dom.QuerySelectorAll(".row")[rw];
                    row.ClassList.Remove("NoShow");

                    var td = row.QuerySelectorAll("TD");
                    td[0].InnerHtml =  xml.SERIE + xml.FOLIO;
                    td[1].InnerHtml =  xml.FECHA_FACTURA.Value.ToString("dd/MM/yyyy");
                    td[2].InnerHtml =  xml.TOTAL.Value.ToString("C2");
                    total           += xml.TOTAL.Value;

                    if (xml.IVA != null)
                    {
                        iva += xml.IVA.Value;
                    }

                    if (xml.IEPS != null)
                    {
                        ieps += xml.IEPS.Value;
                    }

                    if (xml.SUBTOTAL != null)
                    {
                        subTotal += xml.SUBTOTAL.Value;
                    }

                    rw++;
                    if ((page == 0 && rw == 13) || (page > 0 && rw == 16))
                    {
                        SetDoc(dom, doc, page);
                        dom = (IHtmlDocument)document.Clone();
                        page++;
                        rw = 0;
                    }
                }

                if (ok)
                {
                    dom.QuerySelectorAll(".subTotal")[0].ClassList.Remove("NoShow");
                    dom.QuerySelector("#subTotal").InnerHtml = subTotal.ToString("c2");

                    rw++;

                    if ((page == 0 && rw == 13) || (page > 0 && rw == 16))
                    {
                        SetDoc(dom, doc, page);
                        dom = (IHtmlDocument)document.Clone();
                        page++;
                        rw = 0;
                    }

                    dom.QuerySelectorAll(".iva")[0].ClassList.Remove("NoShow");
                    dom.QuerySelector("#iva").InnerHtml = iva.ToString("c2");

                    rw++;

                    if ((page == 0 && rw == 13) || (page > 0 && rw == 16))
                    {
                        SetDoc(dom, doc, page);
                        dom = (IHtmlDocument)document.Clone();
                        page++;
                        rw = 0;
                    }

                    dom.QuerySelectorAll(".ieps")[0].ClassList.Remove("NoShow");
                    dom.QuerySelector("#ieps").InnerHtml = ieps.ToString("c2");

                    rw++;

                    if ((page == 0 && rw == 13) || (page > 0 && rw == 16))
                    {
                        SetDoc(dom, doc, page);
                        dom = (IHtmlDocument)document.Clone();
                        page++;
                        rw = 0;
                    }

                    dom.QuerySelectorAll(".total")[0].ClassList.Remove("NoShow");
                    dom.QuerySelector("#total").InnerHtml = total.ToString("c2");
                }

                SetDoc(dom, doc, page);

                HttpResponse resp = HttpContext.Current.Response;
                resp.Clear();
                resp.ClearHeaders();
                resp.AddHeader("content-disposition", "attachment; filename=\"Contra Recibo_" + id + ".pdf\"");
                resp.ContentType = "application/pdf";
                doc.Save(resp.OutputStream);

                Response.End();
            }
            catch (Exception e)
            {
                Log.Write("Err: " + e.Message);
            }

        }

        private static void SetDoc(IHtmlDocument dom, PdfDocument doc,int page)
        {
            SizeF pageSize = new SizeF(3.54331f, 4.72441f); //PdfPageSizes.FromName("Letter");
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

            HtmlToPdf.Options.StartPageIndex = page;
            HtmlToPdf.Options.StartPosition = 0;

            HtmlToPdf.ConvertHtml(dom.DocumentElement.OuterHtml, doc);
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json), WebMethod(EnableSession = true)]
        public static void SetID(int id)
        {
            HttpContext.Current.Session["pdf_id"] = id;
        }
    }
}