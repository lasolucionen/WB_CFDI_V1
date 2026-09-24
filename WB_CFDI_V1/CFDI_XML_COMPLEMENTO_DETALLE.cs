using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_XML_COMPLEMENTO_DETALLE
    {
        public int?    ID                 { get; set; }
        public int?    COMPLEMENTO_ID     { get; set; }
        public string  ID_DOCUMENTO       { get; set; }
        public string  SERIE              { get; set; }
        public string  FOLIO              { get; set; }
        public string  MONEDA_DR          { get; set; }
        public string  METODO_DE_PAGO_DR  { get; set; }
        public int?    NUM_PARCIALIDAD    { get; set; }
        public double? IMP_SALDO_ANT      { get; set; }
        public double? IMP_PAGADO         { get; set; }
        public double? IMP_SALDO_INSOLUTO { get; set; }
        public bool    EXISTE             { get; set; }
    }

}