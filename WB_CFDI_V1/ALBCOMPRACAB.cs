using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class ALBCOMPRACAB
    {
        public string NUMSERIE { get; set; }
        public int NUMALBARAN { get; set; }
        public string N { get; set; }
        public string SUALBARAN { get; set; }
        public string FACTURADO { get; set; }
        public string NUMSERIEFAC { get; set; }
        public int? NUMFAC { get; set; }
        public string NFAC { get; set; }
        public string ESUNDEPOSITO { get; set; }
        public string ESDEVOLUCION { get; set; }
        public int? CODPROVEEDOR { get; set; }
        public DateTime? FECHAALBARAN { get; set; }
        public string ENVIOPOR { get; set; }
        public string PORTESPAG { get; set; }
        public double? DTOCOMERCIAL { get; set; }
        public double? TOTDTOCOMERCIAL { get; set; }
        public double? DTOPP { get; set; }
        public double? TOTDTOPP { get; set; }
        public double? TOTALBRUTO { get; set; }
        public double? TOTALIMPUESTOS { get; set; }
        public double? TOTALNETO { get; set; }
        public string SELECCIONADO { get; set; }
        public int? CODMONEDA { get; set; }
        public float? FACTORMONEDA { get; set; }
        public string IVAINCLUIDO { get; set; }
        public DateTime? FECHAENTRADA { get; set; }
        public int? TIPODOC { get; set; }
        public int? TIPODOCFAC { get; set; }
        public int? IDESTADO { get; set; }
        public DateTime? FECHAMODIFICADO { get; set; }
        public DateTime? HORA { get; set; }
        public int TRANSPORTE { get; set; }
        public int? NBULTOS { get; set; }
        public double? TOTALCARGOSDTOS { get; set; }
        public int CODCLIENTE { get; set; }
        public string CHEQUEADO { get; set; }
        public string NORECIBIDO { get; set; }
        public DateTime? FECHAALBARANVENTA { get; set; }
        public DateTime? FECHACREACION { get; set; }
        public int? NUMIMPRESIONES { get; set; }
    }
}