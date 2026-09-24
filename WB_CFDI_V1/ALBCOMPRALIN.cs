using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class ALBCOMPRALIN
    {
        public string NUMSERIE { get; set; } // nvarchar(4), not null
        public int NUMALBARAN { get; set; } // int, not null
        public int NUMLIN { get; set; } // int, not null
        public string REFERENCIA { get; set; } // nvarchar(15), null
        public string DESCRIPCION { get; set; } // nvarchar(40), null
        public double? UNIDADESTOTAL { get; set; } // float, null
        public double? PRECIO { get; set; } // float, null
        public double? DTO { get; set; } // float, null
        public double? TOTAL { get; set; } // float, null
        public string REQ { get; set; } // float, null
        public string IVA { get; set; } // float, null

        public short? TIPOIMPUESTO { get; set; } // smallint, null
        public string CODALMACEN { get; set; } // nvarchar(3), null

        [Description("FECHA ALBARAN")]
        public string FECHA_ALBARAN { get; set; }

        [Description("ULTIMO COSTO ACTUALIZADO")]
        public double ULTIMO_COSTO_ACTUALIZADO { get; set; }


        [Description("FECHA ACTUALIZACION")]
        public DateTime FECHA_ACTUALIZACION { get; set; }
        public string FECHA_ACTUALIZACION_STR { get; set; }

        [Description("CENTRALIZADOR DE COSTOS")]
        public string CENTRALIZADOR_DE_COSTOS { get; set; }

        public int COLOR { get; set; }
        public int COLOR2 { get; set; }

    }

}