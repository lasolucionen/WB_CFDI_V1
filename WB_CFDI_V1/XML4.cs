using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Mapster;

namespace XML4
{
    static class XML_GEN
    {
        public class UpperCaseUTF8Encoding : UTF8Encoding
        {
            // Code from a blog http://www.distribucon.com/blog/CategoryView,category,XML.aspx
            //
            // Dan Miser - Thoughts from Dan Miser
            // Tuesday, January 29, 2008 
            // He used the Reflector to understand the heirarchy of the encoding class
            //
            //      Back to Reflector, and I notice that the Encoding.WebName is the property used to
            //      write out the encoding string. I now create a descendant class of UTF8Encoding.
            //      The class is listed below. Now I just call XmlTextWriter, passing in
            //      UpperCaseUTF8Encoding.UpperCaseUTF8 for the Encoding type, and everything works
            //      perfectly. - Dan Miser

            public override string WebName
            {
                get { return base.WebName.ToUpper(); }
            }

            public static UpperCaseUTF8Encoding UpperCaseUTF8
            {
                get
                {
                    if (upperCaseUtf8Encoding == null)
                    {
                        upperCaseUtf8Encoding = new UpperCaseUTF8Encoding();
                    }

                    return upperCaseUtf8Encoding;
                }
            }

            private static UpperCaseUTF8Encoding upperCaseUtf8Encoding = null;

        }


        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return new UpperCaseUTF8Encoding(); }
            }
        }


        public static Comprobante DeserializeStr(this Comprobante obj, string xml)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Comprobante));
                TextReader    reader       = new StringReader(xml);
                Comprobante   ob           = (Comprobante) deserializer.Deserialize(reader);
                reader.Close();
                return ob;
            }
            catch (Exception ex)
            {
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(XML4_4.Comprobante));
                    TextReader reader = new StringReader(xml);
                    XML4_4.Comprobante ob = (XML4_4.Comprobante)deserializer.Deserialize(reader);
                    reader.Close();


                    /*
                    TypeAdapterConfig<XML3_4.Comprobante, XML3.Comprobante>.NewConfig()
                        .Map(dest => dest.Prueba1, src => src.Prueba2)
                        .Ignore("Version");
                    */


                    var com = ob.Adapt<XML4.Comprobante>();

                    return com;
                }
                catch (Exception ex2)
                {
                    //MessageBox.Show(ex.Message);
                    //return null;
                    throw;
                }
            }
        }

        public static Comprobante Deserialize(this Comprobante obj, string path)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Comprobante));
                TextReader    reader       = new StreamReader(path);
                Comprobante   ob           = (Comprobante) deserializer.Deserialize(reader);
                reader.Close();
                return ob;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //return null;
                throw;
            }

        }

        public static string SerializeStr(this Comprobante obj)
        {

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                //settings.OmitXmlDeclaration = true;
                IndentChars = "\t",
                Indent      = true
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Comprobante));

            obj.Xsi  = null;
            obj.Cfdi = null;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            using (var strWriter = new Utf8StringWriter())
            using (XmlWriter writer = XmlWriter.Create(strWriter, settings))
            {
                serializer.Serialize(writer, obj, ns);
                //serializer.Serialize(writer, obj);
                return strWriter.ToString();
            }
        }
    }







    [XmlRoot(ElementName = "Emisor", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Emisor
    {
        [XmlAttribute(AttributeName = "Rfc")]
        public string Rfc { get; set; }

        [XmlAttribute(AttributeName = "Nombre")]
        public string Nombre { get; set; }

        [XmlAttribute(AttributeName = "RegimenFiscal")]
        public string RegimenFiscal { get; set; }
    }

    [XmlRoot(ElementName = "Receptor", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Receptor
    {
        [XmlAttribute(AttributeName = "Rfc")]
        public string Rfc { get; set; }

        [XmlAttribute(AttributeName = "Nombre")]
        public string Nombre { get; set; }

        [XmlAttribute(AttributeName = "UsoCFDI")]
        public string UsoCFDI { get; set; }
    }

    [XmlRoot(ElementName = "Concepto", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Concepto
    {
        [XmlAttribute(AttributeName = "ClaveProdServ")]
        public string ClaveProdServ { get; set; }

        [XmlAttribute(AttributeName = "Cantidad")]
        public string Cantidad { get; set; }

        [XmlAttribute(AttributeName = "ClaveUnidad")]
        public string ClaveUnidad { get; set; }

        [XmlAttribute(AttributeName = "Descripcion")]
        public string Descripcion { get; set; }

        [XmlAttribute(AttributeName = "ValorUnitario")]
        public string ValorUnitario { get; set; }

        [XmlAttribute(AttributeName = "Importe")]
        public string Importe { get; set; }
    }

    [XmlRoot(ElementName = "Conceptos", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Conceptos
    {
        [XmlElement(ElementName = "Concepto", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public List<Concepto> Concepto { get; set; }
    }

    [XmlRoot(ElementName = "DoctoRelacionado", Namespace = "http://www.sat.gob.mx/Pagos")]
    public class DoctoRelacionado
    {
        [XmlAttribute(AttributeName = "IdDocumento")]
        public string IdDocumento { get; set; }

        [XmlAttribute(AttributeName = "Serie")]
        public string Serie { get; set; }

        [XmlAttribute(AttributeName = "Folio")]
        public string Folio { get; set; }

        [XmlAttribute(AttributeName = "MonedaDR")]
        public string MonedaDR { get; set; }

        [XmlAttribute(AttributeName = "MetodoDePagoDR")]
        public string MetodoDePagoDR { get; set; }

        [XmlAttribute(AttributeName = "NumParcialidad")]
        public string NumParcialidad { get; set; }

        [XmlAttribute(AttributeName = "ImpSaldoAnt")]
        public string ImpSaldoAnt { get; set; }

        [XmlAttribute(AttributeName = "ImpPagado")]
        public string ImpPagado { get; set; }

        [XmlAttribute(AttributeName = "ImpSaldoInsoluto")]
        public string ImpSaldoInsoluto { get; set; }
    }

    [XmlRoot(ElementName = "Pago", Namespace = "http://www.sat.gob.mx/Pagos")]
    public class Pago
    {
        [XmlElement(ElementName = "DoctoRelacionado", Namespace = "http://www.sat.gob.mx/Pagos")]
        public List<DoctoRelacionado> DoctoRelacionado { get; set; }

        [XmlAttribute(AttributeName = "FechaPago")]
        public string FechaPago { get; set; }

        [XmlAttribute(AttributeName = "FormaDePagoP")]
        public string FormaDePagoP { get; set; }

        [XmlAttribute(AttributeName = "MonedaP")]
        public string MonedaP { get; set; }

        [XmlAttribute(AttributeName = "Monto")]
        public string Monto { get; set; }

        [XmlAttribute(AttributeName = "NumOperacion")]
        public string NumOperacion { get; set; }

        [XmlAttribute(AttributeName = "RfcEmisorCtaBen")]
        public string RfcEmisorCtaBen { get; set; }

        [XmlAttribute(AttributeName = "RfcEmisorCtaOrd")]
        public string RfcEmisorCtaOrd { get; set; }

        [XmlAttribute(AttributeName = "CtaBeneficiario")]
        public string CtaBeneficiario { get; set; }

        [XmlAttribute(AttributeName = "NomBancoOrdExt")]
        public string NomBancoOrdExt { get; set; }

        [XmlAttribute(AttributeName = "CtaOrdenante")]
        public string CtaOrdenante { get; set; }
    }

    [XmlRoot(ElementName = "Pagos", Namespace = "http://www.sat.gob.mx/Pagos")]
    public class Pagos
    {
        [XmlElement(ElementName = "Pago", Namespace = "http://www.sat.gob.mx/Pagos")]
        public Pago Pago { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
    public class TimbreFiscalDigital
    {

        public TimbreFiscalDigital()
        {
            SchemaLocation =
                "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/sitio_internet/cfd/TimbreFiscalDigital/TimbreFiscalDigitalv11.xsd";

            xmlns.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        }

        [XmlNamespaceDeclarations] 
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        [XmlAttribute(AttributeName = "tfd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Tfd { get; set; }

        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "UUID")]
        public string UUID { get; set; }

        [XmlAttribute(AttributeName = "FechaTimbrado")]
        public string FechaTimbrado { get; set; }

        [XmlAttribute(AttributeName = "SelloCFD")]
        public string SelloCFD { get; set; }

        [XmlAttribute(AttributeName = "NoCertificadoSAT")]
        public string NoCertificadoSAT { get; set; }

        [XmlAttribute(AttributeName = "SelloSAT")]
        public string SelloSAT { get; set; }

        [XmlAttribute(AttributeName = "RfcProvCertif")]
        public string RfcProvCertif { get; set; }
    }

    [XmlRoot(ElementName = "Complemento", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Complemento
    {
        [XmlElement(ElementName = "Pagos", Namespace = "http://www.sat.gob.mx/Pagos")]
        public Pagos Pagos { get; set; }

        [XmlElement(ElementName = "TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }
    }



    [XmlRoot(ElementName = "Comprobante", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Comprobante
    {
        public Comprobante()
        {
            SchemaLocation = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd";
        }

        [XmlElement(ElementName = "Emisor", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Emisor Emisor { get; set; }

        [XmlElement(ElementName = "Receptor", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Receptor Receptor { get; set; }

        [XmlElement(ElementName = "Conceptos", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Conceptos Conceptos { get; set; }

        [XmlElement(ElementName = "Complemento", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Complemento Complemento { get; set; }

        [XmlAttribute(AttributeName = "cfdi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Cfdi { get; set; }

        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "pago10", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Pago10 { get; set; }

        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "Serie")]
        public string Serie { get; set; }

        [XmlAttribute(AttributeName = "Folio")]
        public string Folio { get; set; }

        [XmlAttribute(AttributeName = "Fecha")]
        public string Fecha { get; set; }

        [XmlAttribute(AttributeName = "SubTotal")]
        public string SubTotal { get; set; }

        [XmlAttribute(AttributeName = "Moneda")]
        public string Moneda { get; set; }

        [XmlAttribute(AttributeName = "Total")]
        public string Total { get; set; }

        [XmlAttribute(AttributeName = "TipoDeComprobante")]
        public string TipoDeComprobante { get; set; }

        [XmlAttribute(AttributeName = "LugarExpedicion")]
        public string LugarExpedicion { get; set; }

        [XmlAttribute(AttributeName = "NoCertificado")]
        public string NoCertificado { get; set; }

        [XmlAttribute(AttributeName = "Certificado")]
        public string Certificado { get; set; }

        [XmlAttribute(AttributeName = "Sello")]
        public string Sello { get; set; }

    }

}
