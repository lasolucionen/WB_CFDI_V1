using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class DETALLE_COMPRAS
    {
        public int      CODPROVEEDOR  { get; set; }
        public string   NOMPROVEEDOR  { get; set; }
        public string   RFC           { get; set; }

        public DateTime FECHA_PROCESO { get; set; }
        public double   SUBTOTAL      { get; set; }
        public double   IMPUESTOS     { get; set; }
        public double   TOTAL         { get; set; }
        public string   NUMSERIEFAC   { get; set; }
        public int      NUMFAC        { get; set; }

        [DisplayName("FECHA_FACTURA")]
        [Description("FECHA FACTURA")]
        public DateTime FECHA_FACTURA { get; set; }
        public string   NUMSERIE      { get; set; }
        public int      NUMALBARAN    { get; set; }

        [DisplayName("FECHA_ALBARAN")]
        [Description("FECHA ALBARAN")]
        public DateTime FECHA_ALBARAN { get; set; }

    }
}