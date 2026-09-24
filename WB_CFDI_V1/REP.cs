using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WB_CFDI_V1
{
    public class REP
    {
        public string RFC_EMISOR { get; set; }
        public string RAZON_SOCIAL_EMISOR { get; set; }
        public int NUEVOS { get; set; }

        [DisplayName("SIN PROCESAR")]
        public int SIN_PROCESAR { get; set; }
        public int INCOMPLETOS { get; set; }

        //[DisplayName("POR VALIDAR")]
        //public int POR_VALIDAR { get; set; }

        [DisplayName("No. XML POR VALIDAR")]
        public int XML_POR_VALIDAR { get; set; }


        [DisplayName("$ POR VALIDAR")]
        public double dXML_POR_VALIDAR { get; set; }
        public int VALIDADOS { get; set; }

        [DisplayName("$ VALIDADO")]
        public double dXML_VALIDADO { get; set; }

        [DisplayName("No. XML VALIDADO")]
        public int XML_VALIDADO { get; set; }


        [DisplayName("SIN PROCESAR AYER")]
        public int SIN_PROCESAR_AYER { get; set; }

        [DisplayName("INCOMPLETOS AYER")]
        public int INCOMPLETOS_AYER { get; set; }

        [DisplayName("VALIDADOS AYER")]
        public int VALIDADOS_AYER { get; set; }
        public string VALIDADORA { get; set; }

        public double dXML_FINAL_DIA { get; set; }
        public int XML_FINAL_DIA { get; set; }
    }
}



//wb.Worksheets.Add("Detalle");

//var ws = wb.Worksheets.Worksheet(2);

//var trs1 = rs1.Select(
//    s => new {
//        s.RFC_EMISOR,
//        s.RAZON_SOCIAL_EMISOR,
//        s.UUID,
//        //s.SERIE,
//        //s.FOLIO,
//        //s.SUBTOTAL,
//        //s.IMPUESTOS,
//        //s.TOTAL,
//        //s.TIPO_COMPROBANTE,
//        s.ESTATUS2,
//        s.FECHA_RECEPCION,
//        s.FECHA_FACTURA,
//        //s.FECHA_PROCESO,
//        //s.CHECK_ID,
//        //s.CHECK_NUMBER,
//        //s.CHECK_DATE,
//        //s.NUMSERIE_NUMALBARAN,
//        s.CONTRA_RECIBO_ID,
//        FECHA_CONTRA_RECIBO = s.FECHA_CONTRA_RECIBO != null ? s.FECHA_CONTRA_RECIBO.Value.ToString("dd/MM/yyyy") : "",
//        HORA_CONTRA_RECIBO = s.FECHA_CONTRA_RECIBO != null ? s.FECHA_CONTRA_RECIBO.Value.ToString("hh:mm:ss tt") : "",
//        s.FECHA_CIERRE,
//        HORA_CIERRE = s.FECHA_CIERRE != null ? new DateTime(s.FECHA_CIERRE.Value.Year, s.FECHA_CIERRE.Value.Month, s.FECHA_CIERRE.Value.Day, s.HORA_CIERRE.Value.Hours, s.HORA_CIERRE.Value.Minutes, s.HORA_CIERRE.Value.Seconds).ToString("hh:mm:ss tt") : "",
//        //s.NUMSERIE,
//        //s.NUMALBARAN,
//        //s.NUMSERIEFAC,
//        //s.NUMFAC,
//        //s.FECHA_PROCESO,
//        s.VALIDADORA
//    }).ToDataTable(true);

//if (trs1.Rows.Count > 0)
//{

//    int row    = 2;
//    int column = 2;

//    for (int j = 1; j < column; j++)
//    {
//        ws.Column(j).Width = 2.5;
//    }

//    IXLCell cell = ws.Cell(row, column+1);
//    IXLRange ran = null;


//    cell.Value           = "Periodo " + f1 + " a " + f2;
//    cell.Style.Font.Bold = true;
//    cell.Style.Font.FontSize=13;

//    cell = ws.Cell(row+1, column);

//    var tbl = cell.InsertTable(trs1, true);
//    tbl.Theme = XLTableTheme.TableStyleMedium2;

//    ran = ws.Range(
//            tbl.FirstCell().CellBelow(),
//            tbl.LastCell());



//    if (ran != null)
//    {

//        ran.Column(tbl.Field("CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
//        //ran.Column(tbl.Field("SERIE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
//        //ran.Column(tbl.Field("FOLIO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
//        ran.Column(tbl.Field("ESTATUS2").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

//        //ran.Column(tbl.Field("TIENDA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
//        //ran.Column(tbl.Field("TIPO_COMPROBANTE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
//        ran.Column(tbl.Field("FECHA_CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
//        ran.Column(tbl.Field("HORA_CONTRA_RECIBO").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
//        ran.Column(tbl.Field("HORA_CIERRE").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
//        ran.Column(tbl.Field("VALIDADORA").Index + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

//        ran.AdjustToContents();

//    }

//}



//for (var index = acc.Count - 1; index >= 0; index--)
//{
//    var acumulado = acc[index];


//    /*
//    var query = (from x in tb.Rows.OfType<DataRow>()
//        where x.Field<string>("VALIDADORA").Str().Trim() == acumulado.VALIDADORA.Trim()
//        select x).ToList();
//    */




//    if (String.IsNullOrEmpty(acumulado.VALIDADORA) && acumulado.IGNORADOS == 0 && acumulado.NUEVOS == 0 && acumulado.VALIDADOS == 0 && acumulado.INCOMPLETOS==0)
//    {
//        //var list = tb.Rows.Cast<DataRow>().Where(dr => dr["VALIDADORA"].Str() == acumulado.VALIDADORA.Str()).ToList();


//        //for (var i = list.Count - 1; i >= 0; i--)
//        //{
//        //    var dataRow = list[i];
//        //    tb.Rows.Remove(dataRow);
//        //}

//        ls1.RemoveAll(r => r.VALIDADORA.Str() == acumulado.VALIDADORA.Str());

//        //tb.Rows.Find();
//        acc.Remove(acumulado);
//    }
//    else
//    {
//        acumulado.A_VALIDAR =
//            acumulado.INCOMPLETOS + acumulado.NUEVOS + acumulado.IGNORADOS;
//    }

//}