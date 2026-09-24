using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_EMAIL_MSG
    {
        public int ID { get; set; } // int, not null
        public string UUID { get; set; } // varchar(100), null
        public string EMAIL_TO { get; set; } // varchar(60), null
        public string SERIE { get; set; } // varchar(20), null
        public string FOLIO { get; set; } // varchar(20), null
        public string RFC_EMISOR { get; set; } // varchar(25), null
        public string RFC_RECEPTOR { get; set; } // varchar(25), null
        public string RFC_USER { get; set; } // varchar(25), null
        public int TIPO { get; set; } // int, not null
        public string OBSERVACION { get; set; } // varchar(255), null
        public string MENSAJE { get; set; } // varchar(255), null

        public CFDI_EMAIL_MSG(string uuid, string emailTo, string serie, string folio, string rfcEmisor, string rfcReceptor, string rfcUser, int tipo, string observacion, string mensaje)
        {
            UUID         = uuid;
            EMAIL_TO     = emailTo;
            SERIE        = serie;
            FOLIO        = folio;
            RFC_EMISOR   = rfcEmisor;
            RFC_RECEPTOR = rfcReceptor;
            RFC_USER     = rfcUser;
            TIPO         = tipo;
            OBSERVACION  = observacion;
            MENSAJE      = mensaje;
        }

        public CFDI_EMAIL_MSG()
        {
        }
    }

}