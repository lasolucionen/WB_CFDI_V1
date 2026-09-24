using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class IT_RELFACTURAS_COMPRAS
    {
        public string    FOLIOFISCAL   { get; set; } // nvarchar(100), not null
        public string    SERIEINTERNA  { get; set; } // nvarchar(10), null
        public string    FOLIOINTERNO  { get; set; } // int, null
        public int       LIN_FAC       { get; set; } // int, not null
        public int       LIN           { get; set; } // int, not null
        public string    NUMSERIE      { get; set; } // nvarchar(4), not null
        public int       NUMALBARAN    { get; set; } // int, not null
        public string    NUMSERIEFAC   { get; set; } // nvarchar(4), null
        public int?      NUMFAC        { get; set; } // int, null
        public DateTime? FECHA_PROCESO { get; set; } // datetime, null
        public string    USUARIO       { get; set; } // nvarchar(50), not null

        public IT_RELFACTURAS_COMPRAS(string foliofiscal, string serieinterna, string foliointerno, int linFac, int lin, string numserie, int numalbaran, string usuario)
        {
            FOLIOFISCAL = foliofiscal;
            SERIEINTERNA = serieinterna;
            FOLIOINTERNO = foliointerno;
            LIN_FAC = linFac;
            LIN = lin;
            NUMSERIE = numserie;
            NUMALBARAN = numalbaran;
            USUARIO = usuario;
        }

        public IT_RELFACTURAS_COMPRAS()
        {
        }
    }
}
