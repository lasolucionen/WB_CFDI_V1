using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class TmpXML
    {
        public string xml { get; set; }
        public string uuid { get; set; }
        public string emisorNombre { get; set; }
        public string emisorRFC { get; set; }
        public string total { get; set; }
        public string subTotal { get; set; }
        public string impTras { get; set; }
        public string version { get; set; }
        public DateTime fecha { get; set; }
        public byte[] pdf { get; set; }
        public string serie { get; set; }
        public string folio { get; set; }
        public string nombreArchivo { get; set; }
        public int estatus { get; set; }
        public string tipoDeComprobante { get; set; }
        public string receptorRFC { get; set; }
        public CFDI_ACCOUNT user2 { get; set; }
        public string metodoPago { get; set; }
        public string formaPago { get; set; }
        public string impRet { get; set; }
        public List<CFDI_RETENCIONES> lstRet { get; set; }
    }
}