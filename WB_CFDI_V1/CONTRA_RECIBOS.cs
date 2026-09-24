using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CONTRA_RECIBOS
    {
        public string   RFC_EMISOR      { get; set; }
        [DisplayName("CONTRA_RECIBO")]
        public int      ID              { get; set; } // int, not null
        public DateTime FECHA           { get; set; } // datetime, not null
        public DateTime FECHA_RECEPCION { get; set; } // datetime, not null
        public DateTime FECHA_PAGO      { get; set; } // datetime, not null
        public DateTime? FECHA_CIERRE    { get; set; }
        public TimeSpan? HORA_CIERRE    { get; set; }
        public double   IMPORTE         { get; set; }
        public double   TOTAL           { get; set; }
        public string   OBSERVACION     { get; set; }
        public bool     ORACLE          { get; set; }
        public bool     CERRADO         { get; set; }

        public string USUARIO { get; set; }
    }
}