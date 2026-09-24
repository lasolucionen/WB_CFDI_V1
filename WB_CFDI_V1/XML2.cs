using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace _XML2
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
                get
                {
                    return base.WebName.ToUpper();
                }
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

    [XmlRoot(ElementName = "DomicilioFiscal", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class DomicilioFiscal
    {
        [XmlAttribute(AttributeName = "calle")]
        public string Calle { get; set; }
        [XmlAttribute(AttributeName = "noExterior")]
        public string NoExterior { get; set; }
        [XmlAttribute(AttributeName = "colonia")]
        public string Colonia { get; set; }
        [XmlAttribute(AttributeName = "localidad")]
        public string Localidad { get; set; }
        [XmlAttribute(AttributeName = "referencia")]
        public string Referencia { get; set; }
        [XmlAttribute(AttributeName = "municipio")]
        public string Municipio { get; set; }
        [XmlAttribute(AttributeName = "estado")]
        public string Estado { get; set; }
        [XmlAttribute(AttributeName = "pais")]
        public string Pais { get; set; }
        [XmlAttribute(AttributeName = "codigoPostal")]
        public string CodigoPostal { get; set; }
    }

    [XmlRoot(ElementName = "RegimenFiscal", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class RegimenFiscal
    {
        [XmlAttribute(AttributeName = "Regimen")]
        public string Regimen { get; set; }
    }

    [XmlRoot(ElementName = "Emisor", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Emisor
    {
        [XmlElement(ElementName = "DomicilioFiscal", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public DomicilioFiscal DomicilioFiscal { get; set; }
        [XmlElement(ElementName = "RegimenFiscal", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public RegimenFiscal RegimenFiscal { get; set; }
        [XmlAttribute(AttributeName = "rfc")]
        public string Rfc { get; set; }
        [XmlAttribute(AttributeName = "nombre")]
        public string Nombre { get; set; }
    }

    [XmlRoot(ElementName = "Domicilio", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Domicilio
    {
        [XmlAttribute(AttributeName = "calle")]
        public string Calle { get; set; }
        [XmlAttribute(AttributeName = "noExterior")]
        public string NoExterior { get; set; }
        [XmlAttribute(AttributeName = "noInterior")]
        public string NoInterior { get; set; }
        [XmlAttribute(AttributeName = "colonia")]
        public string Colonia { get; set; }
        [XmlAttribute(AttributeName = "municipio")]
        public string Municipio { get; set; }
        [XmlAttribute(AttributeName = "estado")]
        public string Estado { get; set; }
        [XmlAttribute(AttributeName = "pais")]
        public string Pais { get; set; }
        [XmlAttribute(AttributeName = "codigoPostal")]
        public string CodigoPostal { get; set; }
    }

    [XmlRoot(ElementName = "Receptor", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Receptor
    {
        [XmlElement(ElementName = "Domicilio", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Domicilio Domicilio { get; set; }
        [XmlAttribute(AttributeName = "rfc")]
        public string Rfc { get; set; }
        [XmlAttribute(AttributeName = "nombre")]
        public string Nombre { get; set; }
    }

    [XmlRoot(ElementName = "Concepto", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Concepto
    {
        [XmlAttribute(AttributeName = "cantidad")]
        public string Cantidad { get; set; }
        [XmlAttribute(AttributeName = "unidad")]
        public string Unidad { get; set; }
        [XmlAttribute(AttributeName = "noIdentificacion")]
        public string NoIdentificacion { get; set; }
        [XmlAttribute(AttributeName = "descripcion")]
        public string Descripcion { get; set; }
        [XmlAttribute(AttributeName = "valorUnitario")]
        public string ValorUnitario { get; set; }
        [XmlAttribute(AttributeName = "importe")]
        public string Importe { get; set; }
    }

    [XmlRoot(ElementName = "Conceptos", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Conceptos
    {
        [XmlElement(ElementName = "Concepto", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public List<Concepto> Concepto { get; set; }
    }

    [XmlRoot(ElementName = "Traslado", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Traslado
    {
        [XmlAttribute(AttributeName = "impuesto")]
        public string Impuesto { get; set; }
        [XmlAttribute(AttributeName = "tasa")]
        public string Tasa { get; set; }
        [XmlAttribute(AttributeName = "importe")]
        public string Importe { get; set; }
    }

    [XmlRoot(ElementName = "Traslados", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Traslados
    {
        [XmlElement(ElementName = "Traslado", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public List<Traslado> Traslado { get; set; }
    }

    [XmlRoot(ElementName = "Impuestos", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Impuestos
    {
        [XmlElement(ElementName = "Traslados", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Traslados Traslados { get; set; }
        [XmlAttribute(AttributeName = "totalImpuestosTrasladados")]
        public string TotalImpuestosTrasladados { get; set; }
    }

    [XmlRoot(ElementName = "TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
    public class TimbreFiscalDigital
    {
        public TimbreFiscalDigital()
        {
            SchemaLocation =
                "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd";

            xmlns.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "selloCFD")]
        public string SelloCFD { get; set; }
        [XmlAttribute(AttributeName = "FechaTimbrado")]
        public string FechaTimbrado { get; set; }
        [XmlAttribute(AttributeName = "UUID")]
        public string UUID { get; set; }
        [XmlAttribute(AttributeName = "noCertificadoSAT")]
        public string NoCertificadoSAT { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "selloSAT")]
        public string SelloSAT { get; set; }
        [XmlAttribute(AttributeName = "tfd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Tfd { get; set; }
    }

    [XmlRoot(ElementName = "Complemento", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Complemento
    {
        [XmlElement(ElementName = "TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }
    }

    [XmlRoot(ElementName = "Comprobante", Namespace = "http://www.sat.gob.mx/cfd/3")]
    public class Comprobante
    {
        public Comprobante()
        {
            SchemaLocation = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
        }

        [XmlElement(ElementName = "Emisor", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Emisor Emisor { get; set; }
        [XmlElement(ElementName = "Receptor", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Receptor Receptor { get; set; }
        [XmlElement(ElementName = "Conceptos", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Conceptos Conceptos { get; set; }
        [XmlElement(ElementName = "Impuestos", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Impuestos Impuestos { get; set; }

        [XmlElement(ElementName = "Complemento", Namespace = "http://www.sat.gob.mx/cfd/3")]
        public Complemento Complemento { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "serie")]
        public string Serie { get; set; }
        [XmlAttribute(AttributeName = "folio")]
        public string Folio { get; set; }
        [XmlAttribute(AttributeName = "fecha")]
        public string Fecha { get; set; }
        [XmlAttribute(AttributeName = "sello")]
        public string Sello { get; set; }
        [XmlAttribute(AttributeName = "formaDePago")]
        public string FormaDePago { get; set; }
        [XmlAttribute(AttributeName = "noCertificado")]
        public string NoCertificado { get; set; }
        [XmlAttribute(AttributeName = "certificado")]
        public string Certificado { get; set; }
        [XmlAttribute(AttributeName = "subTotal")]
        public string SubTotal { get; set; }
        [XmlAttribute(AttributeName = "descuento")]
        public string Descuento { get; set; }
        [XmlAttribute(AttributeName = "total")]
        public string Total { get; set; }
        [XmlAttribute(AttributeName = "metodoDePago")]
        public string MetodoDePago { get; set; }
        [XmlAttribute(AttributeName = "LugarExpedicion")]
        public string LugarExpedicion { get; set; }
        [XmlAttribute(AttributeName = "tipoDeComprobante")]
        public string TipoDeComprobante { get; set; }
        [XmlAttribute(AttributeName = "cfdi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Cfdi { get; set; }
        [XmlAttribute(AttributeName = "NumCtaPago")]
        public string NumCtaPago { get; set; }
    }
}
