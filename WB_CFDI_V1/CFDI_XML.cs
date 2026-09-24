using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_XML
    {
        public string    OBSERVACION1        { get; set; }
        public string    TIPO_REL            { get; set; }
        public bool      BLOQ                { get; set; }
        public DateTime? FECHA_PAGO          { get; set; }
        public int?      ID                  { get; set; } // int, not null
        public int?      USER_ID             { get; set; } // int, not null
        public string    UUID                { get; set; } // varchar(40), not null
        public string    XML                 { get; set; } // text, not null
        public string    RAZON_SOCIAL_EMISOR { get; set; } // varchar(100), not null
        public string    RFC_EMISOR          { get; set; } // varchar(25), not null
        public double?   TOTAL               { get; set; } // float, not null
        public double?   SUBTOTAL            { get; set; } // float, not null
        public double?   IMPUESTOS           { get; set; } // float, not null
        public string    NUMSERIE            { get; set; }
        //public string    NUMALBARAN          { get; set; }
        public string    NUMSERIEFAC         { get; set; }
        public string    NUMFAC              { get; set; }

        [DisplayName("CONTRA_RECIBO")]
        public int? CONTRA_RECIBO_ID               { get; set; } // int, null
        public string    VERSION                   { get; set; }
        public DateTime? FECHA_RECEPCION           { get; set; } // datetime, null
        public DateTime? FECHA_FACTURA             { get; set; } // datetime, null
        public DateTime? FECHA_PROCESO             { get; set; } // datetime, null
        public byte[]    ARCHIVO                   { get; set; } // varbinary(max), not null
        public string    SERIE                     { get; set; } // varchar(20), not null
        public string    FOLIO                     { get; set; } // varchar(20), not null
        public int       ESTATUS                   { get; set; } // int, not null
        public string    ESTATUS2                  { get; set; }
        public string    NOMBRE_ARCHIVO            { get; set; } // varchar(100), not null
        public double?   IVA                       { get; set; }
        public double?   IEPS                      { get; set; }
        public double?   RETENCIONES               { get; set; }
        public double    DESCUENTO                 { get; set; }
        public string    TIPO_COMPROBANTE          { get; set; } // varchar(50), null
        public double?   TOTAL_IMPUESTOS_RETENIDOS { get; set; }


        [DisplayName("ID_PAGO")]
        public string CHECK_ID     { get; set; }

        [DisplayName("NUM_PAGO")]
        public string CHECK_NUMBER { get; set; }

        [DisplayName("FEC_PAGO")]
        public string CHECK_DATE   { get; set; }

        [DisplayName("SERIE_COMPRA")]
        public string    TIENDA              { get; set; } // varchar(10), null

        [DisplayName("FOLIO_COMPRA")]
        public string    NO_COMPRA           { get; set; }
        public DateTime? FECHA_COMPRA        { get; set; }

        [DisplayName("COMENTARIOS")]
        public string    NUMEFECTO           { get; set; } // nvarchar(30), null
        public DateTime? FECHA_SALDADO       { get; set; } // datetime, null

        public bool      REVISADO            { get; set; }

        public double    DIF_TOTAL           { get; set; }
        public double    DIF_IMPUESTO        { get; set; }


        public string    ALBARANES           { get; set; }

        [DisplayName("NUMSERIE - NUMALBARAN")]
        public string    NUMSERIE_NUMALBARAN { get; set; }

        public string METODO_PAGO { get; set; }
        public string FORMA_PAGO  { get; set; }


        public bool INCIDENCIA { get; set; }
        public string INCIDENCIA_MSG { get; set; }
        public string ALBARAN { get; set; }




        public DateTime? FECHA_CONTRA_RECIBO { get; set; }
        public TimeSpan? HORA_CONTRA_RECIBO { get; set; }
        public DateTime? FECHA_CIERRE { get; set; }
        public TimeSpan? HORA_CIERRE { get; set; }
        public string     VALIDADORA { get; set; }
        public string     A_PROCESAR { get; set; }

        public CFDI_XML(int? userId, string uuid, string xml, string razonSocialEmisor, string rfcEmisor, double? total, double? subtotal, double? impuestos, string version, DateTime? fechaFactura, byte[] archivo, string serie, string folio, string nombreArchivo, int estatus, string tipoComprobante, string metodoPago, string formaPago, double? totalImpuestosRetenidos)
        {
            USER_ID                   = userId;
            UUID                      = uuid;
            XML                       = xml;
            RAZON_SOCIAL_EMISOR       = razonSocialEmisor;
            RFC_EMISOR                = rfcEmisor;
            TOTAL                     = total;
            SUBTOTAL                  = subtotal;
            IMPUESTOS                 = impuestos;
            VERSION                   = version;
            FECHA_FACTURA             = fechaFactura;
            ARCHIVO                   = archivo;
            SERIE                     = serie;
            FOLIO                     = folio;
            NOMBRE_ARCHIVO            = nombreArchivo;
            ESTATUS                   = estatus;
            TIPO_COMPROBANTE          = tipoComprobante;
            METODO_PAGO               = metodoPago;
            FORMA_PAGO                = formaPago;
            TOTAL_IMPUESTOS_RETENIDOS = totalImpuestosRetenidos;
        }

        public CFDI_XML()
        {
        }
    }
}