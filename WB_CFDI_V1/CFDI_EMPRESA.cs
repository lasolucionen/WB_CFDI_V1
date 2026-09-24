using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_EMPRESA
    {
        public int    ID              { get; set; } // int, not null
        public string ALIAS           { get; set; } // varchar(50), null
        public string EMPRESA_RFC     { get; set; } // varchar(25), not null
        public int    TIPO_VALIDADOR  { get; set; } // int, not null
        public string TIPO_VALIDADOR2 { get; set; }
        public string EMPRESA_BD      { get; set; } // varchar(50), not null
        public int?   EMPRESA         { get; set; } // int, null
        public string ICG_BD          { get; set; } // varchar(50), null
        public string RAZON_SOCIAL    { get; set; } // varchar(100), not null
        public string SERIE           { get; set; } // varchar(5), null
        public string DIRECCION1      { get; set; } // varchar(255), null
        public string CODPOSTAL       { get; set; } // varchar(10), null
        public string POBLACION       { get; set; } // varchar(100), null
        public string PROVINCIA       { get; set; } // varchar(100), null
        public int?   CORREO_EMPRESA  { get; set; } // int, null
    }
}