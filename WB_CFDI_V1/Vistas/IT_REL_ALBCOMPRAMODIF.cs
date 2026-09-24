using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1.Vistas
{
    public class IT_REL_ALBCOMPRAMODIF
    {
        public string   UUID          { get; set; }
        public string   NUMSERIEO     { get; set; }
        public int      NUMALBARANO   { get; set; }
        public int      NUMLIN        { get; set; }
        public int      CODARTICULO   { get; set; }
        public int      UNIDADESTOTAL { get; set; }
        public double   PRECIO        { get; set; }
        public int      TIPOIMPUESTO  { get; set; }
        public int      IVA           { get; set; }
        public int      REQ           { get; set; }
        public string   NUMSERIE      { get; set; }
        public int      NUMALBARAN    { get; set; }
        public int      ESTADO        { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public string   USUARIO       { get; set; }
    }
}