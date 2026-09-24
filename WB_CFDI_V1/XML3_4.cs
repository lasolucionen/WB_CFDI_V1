using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XML3_4
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
                TextReader reader = new StringReader(xml);
                Comprobante ob = (Comprobante)deserializer.Deserialize(reader);
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

        public static Comprobante Deserialize(this Comprobante obj, string path)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Comprobante));
                TextReader reader = new StreamReader(path);
                Comprobante ob = (Comprobante)deserializer.Deserialize(reader);
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
                Indent = true
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Comprobante));

            obj.Xsi = null;
            obj.Cfdi = null;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("cfdi", "http://www.sat.gob.mx/cfd/4");
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




    [XmlRoot(ElementName = "Emisor", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Emisor
    {
        [XmlAttribute(AttributeName = "Rfc")]
        public string Rfc { get; set; }

        [XmlAttribute(AttributeName = "Nombre")]
        public string Nombre { get; set; }

        [XmlAttribute(AttributeName = "RegimenFiscal")]
        public string RegimenFiscal { get; set; }
    }

    [XmlRoot(ElementName = "Receptor", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Receptor
    {
        [XmlAttribute(AttributeName = "Rfc")]
        public string Rfc { get; set; }

        [XmlAttribute(AttributeName = "Nombre")]
        public string Nombre { get; set; }

        [XmlAttribute(AttributeName = "UsoCFDI")]
        public string UsoCFDI { get; set; }
    }

    [XmlRoot(ElementName = "Traslado", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Traslado
    {
        [XmlAttribute(AttributeName = "Base")]
        public string Base { get; set; }

        [XmlAttribute(AttributeName = "Impuesto")]
        public string Impuesto { get; set; }

        [XmlAttribute(AttributeName = "TipoFactor")]
        public string TipoFactor { get; set; }

        [XmlAttribute(AttributeName = "TasaOCuota")]
        public string TasaOCuota { get; set; }

        [XmlAttribute(AttributeName = "Importe")]
        public string Importe { get; set; }
    }

    [XmlRoot(ElementName = "Traslados", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Traslados
    {
        [XmlElement(ElementName = "Traslado", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public List<Traslado> Traslado { get; set; }
    }

    [XmlRoot(ElementName = "Retencion", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Retencion
    {
        [XmlAttribute(AttributeName = "Base")]
        public string Base { get; set; }
        [XmlAttribute(AttributeName = "Impuesto")]
        public string Impuesto { get; set; }
        [XmlAttribute(AttributeName = "TipoFactor")]
        public string TipoFactor { get; set; }
        [XmlAttribute(AttributeName = "TasaOCuota")]
        public string TasaOCuota { get; set; }
        [XmlAttribute(AttributeName = "Importe")]
        public string Importe { get; set; }
    }

    [XmlRoot(ElementName = "Retenciones", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Retenciones
    {
        [XmlElement(ElementName = "Retencion", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public List<Retencion> Retencion { get; set; }
    }

    [XmlRoot(ElementName = "Impuestos", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Impuestos
    {
        [XmlElement(ElementName = "Traslados", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Traslados Traslados { get; set; }

        [XmlElement(ElementName = "Retenciones", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Retenciones Retenciones { get; set; }

        [XmlAttribute(AttributeName = "TotalImpuestosRetenidos")]
        public string TotalImpuestosRetenidos { get; set; }

        [XmlAttribute(AttributeName = "TotalImpuestosTrasladados")]
        public string TotalImpuestosTrasladados { get; set; }
    }

    [XmlRoot(ElementName = "Concepto", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Concepto
    {
        [XmlElement(ElementName = "Impuestos", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Impuestos Impuestos { get; set; }

        [XmlElement(ElementName = "InformacionAduanera", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public InformacionAduanera InformacionAduanera { get; set; }

        [XmlAttribute(AttributeName = "ClaveProdServ")]
        public string ClaveProdServ { get; set; }

        [XmlAttribute(AttributeName = "NoIdentificacion")]
        public string NoIdentificacion { get; set; }

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

        [XmlAttribute(AttributeName = "Descuento")]
        public string Descuento { get; set; }
    }

    [XmlRoot(ElementName = "InformacionAduanera", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class InformacionAduanera
    {
        [XmlAttribute(AttributeName = "NumeroPedimento")]
        public string NumeroPedimento { get; set; }
    }

    [XmlRoot(ElementName = "Conceptos", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Conceptos
    {
        [XmlElement(ElementName = "Concepto", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public List<Concepto> Concepto { get; set; }
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

        [XmlAttribute(AttributeName = "RfcProvCertif")]
        public string RfcProvCertif { get; set; }

        [XmlAttribute(AttributeName = "SelloCFD")]
        public string SelloCFD { get; set; }

        [XmlAttribute(AttributeName = "NoCertificadoSAT")]
        public string NoCertificadoSAT { get; set; }

        [XmlAttribute(AttributeName = "SelloSAT")]
        public string SelloSAT { get; set; }
    }

    [XmlRoot(ElementName = "TrasladosLocales", Namespace = "http://www.sat.gob.mx/implocal")]
    public class TrasladosLocales
    {
        [XmlAttribute(AttributeName = "ImpLocTrasladado")]
        public string ImpLocTrasladado { get; set; }
        [XmlAttribute(AttributeName = "Importe")]
        public string Importe { get; set; }
        [XmlAttribute(AttributeName = "TasadeTraslado")]
        public string TasadeTraslado { get; set; }
    }

    [XmlRoot(ElementName = "ImpuestosLocales", Namespace = "http://www.sat.gob.mx/implocal")]
    public class ImpuestosLocales
    {
        [XmlElement(ElementName = "TrasladosLocales", Namespace = "http://www.sat.gob.mx/implocal")]
        public TrasladosLocales TrasladosLocales { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "TotaldeRetenciones")]
        public string TotaldeRetenciones { get; set; }
        [XmlAttribute(AttributeName = "TotaldeTraslados")]
        public string TotaldeTraslados { get; set; }
    }

    [XmlRoot(ElementName = "Complemento", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Complemento
    {
        [XmlElement(ElementName = "TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }

        [XmlElement(ElementName = "ImpuestosLocales", Namespace = "http://www.sat.gob.mx/implocal")]
        public ImpuestosLocales ImpuestosLocales { get; set; }
    }


    [XmlRoot(ElementName = "CfdiRelacionado", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class CfdiRelacionado
    {
        [XmlAttribute(AttributeName = "UUID")]
        public string UUID { get; set; }
    }

    [XmlRoot(ElementName = "CfdiRelacionados", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class CfdiRelacionados
    {
        [XmlElement(ElementName = "CfdiRelacionado", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public List<CfdiRelacionado> CfdiRelacionado { get; set; }
        [XmlAttribute(AttributeName = "TipoRelacion")]
        public string TipoRelacion { get; set; }
    }

    [XmlRoot(ElementName = "Comprobante", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Comprobante
    {
        public Comprobante()
        {
            SchemaLocation = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";
        }

        [XmlElement(ElementName = "CfdiRelacionados", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public CfdiRelacionados CfdiRelacionados { get; set; }

        [XmlElement(ElementName = "Emisor", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Emisor Emisor { get; set; }

        [XmlElement(ElementName = "Receptor", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Receptor Receptor { get; set; }

        [XmlElement(ElementName = "Conceptos", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Conceptos Conceptos { get; set; }

        [XmlElement(ElementName = "Impuestos", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Impuestos Impuestos { get; set; }

        [XmlElement(ElementName = "Complemento", Namespace = "http://www.sat.gob.mx/cfd/4")]
        public Complemento Complemento { get; set; }

        [XmlAttribute(AttributeName = "cfdi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Cfdi { get; set; }

        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }

        [XmlAttribute(AttributeName = "Sello")]
        public string Sello { get; set; }

        [XmlAttribute(AttributeName = "Certificado")]
        public string Certificado { get; set; }

        [XmlAttribute(AttributeName = "NoCertificado")]
        public string NoCertificado { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "Serie")]
        public string Serie { get; set; }

        [XmlAttribute(AttributeName = "Folio")]
        public string Folio { get; set; }

        [XmlAttribute(AttributeName = "Fecha")]
        public string Fecha { get; set; }

        [XmlAttribute(AttributeName = "FormaPago")]
        public string FormaPago { get; set; }

        [XmlAttribute(AttributeName = "SubTotal")]
        public string SubTotal { get; set; }

        [XmlAttribute(AttributeName = "Descuento")]
        public string Descuento { get; set; }

        [XmlAttribute(AttributeName = "Moneda")]
        public string Moneda { get; set; }

        [XmlAttribute(AttributeName = "Total")]
        public string Total { get; set; }

        [XmlAttribute(AttributeName = "TipoDeComprobante")]
        public string TipoDeComprobante { get; set; }

        [XmlAttribute(AttributeName = "MetodoPago")]
        public string MetodoPago { get; set; }

        [XmlAttribute(AttributeName = "LugarExpedicion")]
        public string LugarExpedicion { get; set; }

    }

}
