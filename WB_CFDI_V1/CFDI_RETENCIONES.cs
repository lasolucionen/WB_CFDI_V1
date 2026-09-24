using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_RETENCIONES
    {
        public int    ID         { get; set; }
        public int    LINEA      { get; set; }
        public string IMPUESTO   { get; set; }
        public string TASAOCUOTA { get; set; }
        public string IMPORTE    { get; set; }
    }
}