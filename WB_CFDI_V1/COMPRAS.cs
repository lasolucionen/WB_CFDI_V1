using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class COMPRAS
    {
        public bool      BLOQ         { get; set; }
        public string    NUMSERIE     { get; set; } // nvarchar(4), not null
        public int       NUMALBARAN   { get; set; } // int, not null
        public DateTime? FECHAALBARAN { get; set; } // datetime, null
        public string    SUALBARAN    { get; set; } // nvarchar(15), null
        public double?   TOTAL        { get; set; }
        public double?   SUBTOTAL     { get; set; }
        public double?   IVA          { get; set; }
        public double?   IEPS         { get; set; }
        public string    NIF20        { get; set; }
        public string    ST           { get; set; }
        public string    ESTADO       { get; set; }


        /*
         
                      AC.NUMSERIE,
                      AC.NUMALBARAN,
                      AC.FECHAALBARAN,
                      SUALBARAN,
                      SUM(AT.BRUTO)  SUBTOTAL,
                      SUM(AT.TOTIVA) IVA,
                      SUM(AT.TOTREQ) IEPS,
                      SUM(AT.TOTAL)  TOTAL,
                      P.NIF20
         
         */
    }
}