using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class PROVEEDORES
    {
        public int? CODPROVEEDOR { get; set; } // int, null
        public string NOMPROVEEDOR { get; set; } // nvarchar(255), null
        public string NOMCOMERCIAL { get; set; } // nvarchar(255), null
        public string DIRECCION1 { get; set; } // nvarchar(255), null
        public string CODPOSTAL { get; set; } // nvarchar(8), null
        public string PROVINCIA { get; set; } // nvarchar(100), null
        public string POBLACION { get; set; } // nvarchar(100), null
        public string NUMEFECTO { get; set; } // nvarchar(30), null
    }
}