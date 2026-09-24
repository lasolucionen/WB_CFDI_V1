using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class TESORERIA
    {
        public string    SERIE            { get; set; } // nvarchar(4), null
        public int?      NUMERO           { get; set; } // int, null
        public short?    POSICION         { get; set; } // smallint, null
        public string    DESCRIPCION      { get; set; } // nvarchar(30), null
        public DateTime? FECHADOCUMENTO   { get; set; } // datetime, null
        public string    SUDOCUMENTO      { get; set; } // nvarchar(15), null
        public DateTime? FECHAVENCIMIENTO { get; set; } // datetime, null
        public double?   IMPORTE          { get; set; } // float, null
        public string    NUMEFECTO        { get; set; } // nvarchar(30), null
        public int       AJUSTE           { get; set; }
    }
}