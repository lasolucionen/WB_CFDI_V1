using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_XML_COMPLEMENTO_PAGO
    {
        public int       ID                  { get; set; }
        public int       USER_ID             { get; set; }
        public string    UUID                { get; set; }
        public string    XML                 { get; set; }
        public string    RAZON_SOCIAL_EMISOR { get; set; }
        public string    RFC_EMISOR          { get; set; }
        public DateTime  FECHA_RECEPCION     { get; set; }
        public DateTime  FECHA_FACTURA       { get; set; }
        public string    SERIE               { get; set; }
        public string    FOLIO               { get; set; }
        public string    NOMBRE_ARCHIVO      { get; set; }
        public string    TIPO_COMPROBANTE    { get; set; }
        public DateTime? FECHA_PAGO          { get; set; }
        public string    FORMA_DE_PAGO_P     { get; set; }
        public string    MONEDA_P            { get; set; }
        public double?   MONTO               { get; set; }
        public string    NUM_OPERACION       { get; set; }
        public string    RFC_EMISOR_CTA_ORD  { get; set; }
        public string    NOM_BANCO_ORD_EXT   { get; set; }
        public string    CTA_ORDENANTE       { get; set; }
        public string    RFC_EMISOR_CTA_BEN  { get; set; }
        public string    CTA_BENEFICIARIO    { get; set; }
        public byte[]    ARCHIVO             { get; set; }
    }
}