using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class CFDI_ACCOUNT
    {
        public int    ID                          { get; set; } // int, not null
        public string USER_NAME                   { get; set; } // varchar(50), not null
        public string PASSWORD                    { get; set; } // varchar(50), not null
        public string EMAIL                       { get; set; } // varchar(60), null
        public int    ROLE                        { get; set; } // int, not null
        public string ROLE_NAME                   { get; set; } // varchar(50), not null
        public bool   MOSTRAR_BLOQUEO             { get; set; }
        public bool   MOSTRAR_BLOQUEO2            { get; set; }
        public bool   IGNORAR_BLOQUEO2            { get; set; }
        public bool   MOSTRAR_RECHAZAR            { get; set; }
        public bool   MOSTRAR_DESVALIDAR          { get; set; }
        public bool   MOSTRAR_FECHA_VALIDACION    { get; set; }
        public bool   MOSTRAR_ABRIR_CARTA_PAGO    { get; set; }
        public bool   MOSTRAR_FACTURA_DUPLICADOS  { get; set; }
        public bool   MOSTRAR_FECHA_PAGO_BLOQUEO  { get; set; }
        public bool   MOSTRAR_REC_FACTURA_BLOQUEO { get; set; }
        public bool   PEDIR_SERIE_FOLIO           { get; set; }
        public bool   MOSTRAR_AGREGAR_QUITAR_FAC  { get; set; }
        public string DIF_TOTAL                   { get; set; }
        public string DIF_IMPUESTO                { get; set; }
    }
}