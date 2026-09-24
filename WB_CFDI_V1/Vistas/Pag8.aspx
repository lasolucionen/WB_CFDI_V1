<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag8.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>
    .a1 {
        /*border: 1px solid black;*/
    }
</style>
        <script type="text/javascript">

            var auto = true;
            var source = {};
            var intfile = 0;
            var columns = new Array();
            var datafields = new Array();
            var error401 = 0;
            var dataAdapter = null;
            var emp = "";
            var editrow = null;

            var source2 = {};
            var source5 = {};
            var source6 = {};

            $(document).ready(function () {
                //$.jqx.theme = "bootstrap";
                //$.jqx.theme = "customx";
                /*******************************************************************************/
                
                function getData(cUrl, cData) {
                    var result = null;

                    $.ajax({
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        url: cUrl,
                        data: JSON.stringify(cData),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            try {
                                result = JSON.parse(data.d);
                            } catch (err) {
                                try {
                                    result = data.d;
                                } catch (err) {
                                    result = data;
                                }
                            }
                        },
                        error: function (data) {
                            try {
                                console.debug("************* Error ***************");
                                console.debug(data);
                            } catch (err) { }

                            if (data.status == 401) {
                                window.location.href = "Login.aspx";
                            }
                        }
                    });


                    return result;
                }


                /*---------------------------------------------*/

                function getAdapter(cUrl, cData) {
                    auto = true;

                    var cSource = getData(cUrl, cData);

                    if (!$.isEmptyObject(cSource)) {

                        return new $.jqx.dataAdapter(cSource, {
                            autoBind: true,
                            loadComplete: function (data) {
                                if (data.length > 10) {
                                    auto = false;
                                }
                            }
                        });
                    }
                    return null;
                }

                /*---------------------------------------------*/

                function isEmpty(value) {
                    //return (value == null || value.length === 0);
                    return (value == null || $.trim(value) === '');
                }

                /*---------------------------------------------*/

                $(".jTx").jqxInput({ height: 25, theme: 'Theme2' });

                $("#jqxOpt1").jqxRadioButton({ width: 120, height: 25, checked: true });
//                $("#jqxOpt2").jqxRadioButton({ width: 120, height: 25 });

                var opt = 0;

                $("#jqxOpt1").on('change', function (event) {
                    var checked = event.args.checked;

                    if (checked) {
                        $("#lbFecha1, #lbFecha2").html("Recepción");
                        opt = 0;
                    }
                });

                $("#jqxOpt2").on('change', function (event) {
                    var checked = event.args.checked;

                    if (checked) {
                        $("#lbFecha1, #lbFecha2").html("Saldado");
                        opt = 1;
                    }
                });

                //$('#jqxTabs').jqxTabs({ width:'100%' });

                $("#cbEstatus").jqxDropDownList({
                    displayMember: 'ESTATUS', valueMember: 'ID',
                    source: [{ ID: 0, ESTATUS: 'Todas' }, { ID: 1, ESTATUS: 'Con Complemento' }, { ID: 2, ESTATUS: 'Sin Complemento'}],
                    selectedIndex: 0,
                    dropDownHeight: 200,
                    autoDropDownHeight: auto,
                    width:173
                });

                /*******************************************************************************/

                $("#jDate1").jqxDateTimeInput({value:'<%=getFecha1() %>' , height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate2").jqxDateTimeInput({ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });



                $(".jCbX").jqxComboBox({ height: 25,searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'
                });


                $("#cbRFC").jqxComboBox({width: '174px'});

                var prov = getAdapter("Pag8.aspx/GetRFCs", {});
                var prov2 = $.extend(true, {}, prov);

                $("#cbRFC").jqxComboBox({
                    displayMember: 'RFC_EMISOR', valueMember: 'RFC_EMISOR',
                    source: prov2,
                    dropDownHeight: 200, 
                    autoDropDownHeight: auto
                });

                $("#cbRFC").bind('select', function (event) {
                    if (event.args && event.args.item) {
                         
                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        var record = event.args.item.originalItem;
                        $("#razonSocial").html(record.RAZON_SOCIAL_EMISOR);
                    }
                });



                /*******************************************************************************/
                $("#chkXML").jqxCheckBox({ width: 180, height: 25 });
                $("#chkPDF").jqxCheckBox({ width: 180, height: 25 });
                $("#chkPDF2").jqxCheckBox({ width: 180, height: 25 });
                /*******************************************************************************/

                $('#init').removeClass('no-show');
                setTimeout(function () {


                    $('#init').addClass('no-show');
                }, 1000);

                /*******************************************************************************/

                /*
                var rs = getData("Pag8.aspx/GetSemana");
                $("#semana2").html(rs.week);
                $("#semana").html(rs.week);
                */


                /*******************************************************************************/
                function add3Dots(string, limit) {
                    var dots = "...";
                    if (string.length > limit) {
                        // you can also use substr instead of substring
                        string = string.substring(0, limit) + dots;
                    }

                    return string;
                }


                $(".btn1").button();
                $(".fileUpload").click(function () {

                });


                var bar = $('.bar');
                var percent = $('.percent');

                /*******************************************************************************/

                function resetFile() {
                    $("#archivo1").val('');
                    $("#dFile1").html($("#dFile1").html());
                    $("#cfile1").change(function (e) {
                        bar.width('0%');
                        percent.html('0%');

                        var res = e.target.value.split("\\");

                        if (res.length > 0) {
                            $("#archivo1").val(res[res.length - 1]);
                        } else {
                            $("#archivo1").val(res);
                        }
                    });

                    $("#archivo2").val('');
                    $("#dFile2").html($("#dFile2").html());
                    $("#cfile2").change(function (e) {
                        bar.width('0%');
                        percent.html('0%');

                        var res = e.target.value.split("\\");

                        if (res.length > 0) {
                            $("#archivo2").val(res[res.length - 1]);
                        } else {
                            $("#archivo2").val(res);
                        }
                    });


                    $("#archivo3").val('');
                    $("#dFile3").html($("#dFile3").html());
                    $("#cfile3").change(function (e) {
                        bar.width('0%');
                        percent.html('0%');

                        var res = e.target.value.split("\\");

                        if (res.length > 0) {
                            $("#archivo3").val(res[res.length - 1]);
                        } else {
                            $("#archivo3").val(res);
                        }
                    });

                }
                resetFile();

                /*******************************************************************************/

                $('#init').removeClass('no-show');
                setTimeout(function () {


                    $('#init').addClass('no-show');

                }, 1000);


                /*******************************************************************************/

                function doneResizing() {

                    /*
                    $('#customWindow').jqxWindow({
                    position: { x: ($(window).width() / 2) - (330 / 2), y: ($(window).height() / 2)-100 }
                    });*/

                }
                var resizeId;
                $(window).resize(function () {
                    clearTimeout(resizeId);
                    resizeId = setTimeout(doneResizing, 50);
                });


                //---------------------------------------------------------------------------


                $.ajaxSetup({
                    statusCode: {
                        401: function () {
                            //window.location.href = "Login.aspx";
                        }
                    }
                });


                //---------------------------------------------------------------------------
                var columnrenderer2 = function (value) {
                    return '<div style="text-align: center;margin-top: 7px;">' + value + '</div>';
                }

                var cellsrenderer= function(row, columnfield, value, defaulthtml, columnproperties) {
                            
                    return value; //'<input id="" type="text" value="' + value + '" style="width: 290px;height:26px;border: 0px;margin-left:5px;" readonly />';

                }

                source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        //{ name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'FECHA_PAGO', type: 'date' },
                        { name: 'CTA_BENEFICIARIO' },
                        { name: 'CTA_ORDENANTE' },
                        { name: 'FORMA_DE_PAGO_P' },
                        { name: 'MONEDA_P' },
                        { name: 'MONTO' },
                        { name: 'NOM_BANCO_ORD_EXT' },
                        { name: 'NUM_OPERACION' },
                        { name: 'RFC_EMISOR_CTA_BEN' },
                        { name: 'RFC_EMISOR_CTA_ORD' },
                    ],
                    localdata: []
                };
                var dataAdapter = new $.jqx.dataAdapter(source);

                $("#jGrid").jqxGrid({ width: '100%', height: 450,
                    source: dataAdapter,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'checkbox',
                    enablebrowserselection :true,
                    ready: function () {
/*                        var colDefs = $("#jGrid").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {
                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width+10);
                        }*/
                        //$("#jGrid").jqxGrid('autoresizecolumns');
                    },
                    //selectionmode: 'none',
                    //filterable: false,
                    //showfilterrow: true,
                    //rowsheight: 36,
                    //altrows: true,
                    //autorowheight: true,
                    //pageable: true,
                    //columnsheight: 40,
                    columns: [
                        {
                            text: 'Detalle',
                            datafield: 'Detalle',
                            columntype: 'button',
                            width: 45,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                $('#init').removeClass('no-show');
                                setTimeout(function () {

                                    var data = $("#jGrid").jqxGrid('getrowdata', row);

                                    var rs=getData("Pag8.aspx/GetDetalle", { id: data.ID });

                                    
                                    source5.localdata = rs;
                                    dataAdapter5.dataBind();
                                    $("#jGrid5").jqxGrid('ensurerowvisible', 0);
                                    $("#jGrid5").jqxGrid('clearselection');


                                    $("#jGrid5").jqxGrid("autoresizecolumns");

                                    var colDefs = $("#jGrid5").jqxGrid('columns').records;
                                    for ( var idx = 0; idx < colDefs.length; idx++) {
                                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                                            $("#jGrid5").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                        }

                                        if (colDefs[idx].datafield == "ID_DOCUMENTO") {
                                            $("#jGrid5").jqxGrid('setcolumnproperty',
                                                colDefs[idx].datafield,
                                                'width',
                                                colDefs[idx].width + 10);
                                        }
                                    }
                                    
                                        
                                    $("#detalle2").dialog({
                                        buttons: {
                                            'Cerrar': function () {
     
                                                $(this).dialog("close");
                                            }
                                        }
                                    });


                                    $("#detalle2").dialog('open');
                                    $('#init').addClass('no-show');
                                }, 10);

                            }
                        },
                        { text: 'SERIE',               dataField: 'SERIE',               width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA PAGO',          dataField: 'FECHA_PAGO',          width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'MONTO',               dataField: 'MONTO',               width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2'},
                        //{ text: 'FormaDePagoP',        dataField: 'FORMA_DE_PAGO_P',     width: 160, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'NUM OPERACION',       dataField: 'NUM_OPERACION',       width: 160, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'UUID',                dataField: 'UUID',                width: 300, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width:  70, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'XML',
                            datafield: 'Show1',
                            columntype: 'button',
                            width: 60, 
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);
                                
                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                getData("archivoXML2.aspx/SetID", {
                                    id: dataRecord.ID
                                });

                                window.open("archivoXML2.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        },
                        {
                            text: 'PDF',
                            datafield: 'Show2',
                            columntype: 'button',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                getData("archivoPDF2.aspx/SetID", {
                                    id: dataRecord.ID
                                });

                                window.open("archivoPDF2.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        }
                        ,
                        {
                            text: 'PDF Prov',
                            datafield: 'Show3',
                            columntype: 'button',
                            width: 70,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                var pdf = getData("archivoPDF_Prov2.aspx/SetID",
                                    {
                                        id: dataRecord.ID
                                    });

                                if (!$.isEmptyObject(pdf)) {
                                    window.open("archivoPDF_Prov2.aspx", "xml", "width=200,height=150");
                                } else {
                                    $("#message").html("<div style='margin-top: 7px;font-weight: bold;font-size:12px'>El registro seleccionado<br />no contiene pdf de proveedor.</div>");
                                    $("#message").dialog('open');

                                    position('.mWin');
                                }

                                $('#init').addClass('no-show');

                            }
                        }

                    ]
                });





//*****************************************************************************************************************



                //---------------------------------------------------------------------------

                
                $("#jGrid").on('rowselect', function (event) {

                });

                $("#jGrid").bind('rowunselect', function (event) {


                });



//*****************************************************************************************************************
                source2 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ARCHIVO' },
                        { name: 'PDF' },
                        { name: 'DETALLE' },
                        { name: 'MENSAJE' }
                    ],
                    localdata: []
                };
                var dataAdapter2 = new $.jqx.dataAdapter(source2);

                $("#jGrid2").jqxGrid({ 
                    width: '100%',
                    height: 390,
                    source: dataAdapter2,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
                    enablebrowserselection :true,
                    rowsheight: 45,
                    //rowdetails: true,
                    //rowdetailstemplate: { rowdetails: "<div style='padding: 10px;'>Prueba</div>", rowdetailsheight: 100 },
                    //selectionmode: 'none',
                    //filterable: false,
                    //showfilterrow: true,
                    //rowsheight: 36,
                    //altrows: true,
                    //autorowheight: true,
                    //pageable: true,
                    //columnsheight: 40,
                    columns: [
                        { text: 'ARCHIVO', dataField: 'ARCHIVO', width: 340, renderer: columnrenderer2 }, //, pinned: true
                        {text: 'PDF', dataField: 'PDF', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'DETALLE',
                            dataField: 'DETALLE',
                            width: 290,
                            renderer: columnrenderer2,
                            cellsrenderer: function(row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                            }
                        },
                        {
                            text: 'Validacion XML',
                            dataField: 'MENSAJE',
                            width: 280,
                            renderer: columnrenderer2,
                            cellsrenderer: function(row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                            }
                        }
                    ]
                });



//*****************************************************************************************************************

                var cellB = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

                    var formattedValue = value;

                    if ($.jqx.dataFormat) {
                        if ($.jqx.dataFormat.isDate(value)) {
                            formattedValue = $.jqx.dataFormat.formatdate(value, columnproperties.cellsformat);
                        }
                        else if ($.jqx.dataFormat.isNumber(value)) {
                            formattedValue = $.jqx.dataFormat.formatnumber(value, columnproperties.cellsformat);
                        }
                    }
                    //font-weight: bold;


                    if (!rowdata.EXISTE) {
                        return '<div style="margin: 4px; margin-top:4px; text-align: ' +
                            columnproperties.cellsalign +
                            '; color:red;">' +
                            formattedValue +
                            '</div>';
                    } else {
                        return '<div style="margin: 4px; margin-top:4px; text-align: ' +
                            columnproperties.cellsalign +
                            ';">' +
                            formattedValue +
                            '</div>';
                    }


                }


                var cellclass = function (row, columnfield, value) {

                    var data = $("#jGrid6").jqxGrid('getrowdata', row);

                    if (data.BLOQ == true) {
                        return 'Bloq';
                    }
                }

                source5 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        //{ name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'ID_DOCUMENTO' },
                        { name: 'EXISTE' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        { name: 'NUM_PARCIALIDAD' },
                        { name: 'IMP_SALDO_ANT' },
                        { name: 'IMP_PAGADO' },
                        { name: 'IMP_SALDO_INSOLUTO' },
                        { name: 'MONEDA_DR' },
                        { name: 'METODO_DE_PAGO_DR' },

                    ],
                    localdata: []
                };
                var dataAdapter5 = new $.jqx.dataAdapter(source5);

                $("#jGrid5").jqxGrid({ width: '1050', height: 450,
                    source: dataAdapter5,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
                    enablebrowserselection :true,
                    ready: function () {
/*                        var colDefs = $("#jGrid").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {
                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width+10);
                        }*/
                        //$("#jGrid").jqxGrid('autoresizecolumns');
                    },
                    //selectionmode: 'none',
                    //filterable: false,
                    //showfilterrow: true,
                    //rowsheight: 36,
                    //altrows: true,
                    //autorowheight: true,
                    //pageable: true,
                    //columnsheight: 40,
                    columns: [
                        {
                            text: 'Detalle',
                            datafield: 'Detalle',
                            columntype: 'button',
                            width: 45,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var data = $("#jGrid5").jqxGrid('getrowdata', row);
                                if (isEmpty(data.ID_DOCUMENTO)) {
                                    return;
                                }

                                $('#init').removeClass('no-show');
                                setTimeout(function () {

                                    var rs=getData("Pag8.aspx/GetDetalle2", { id: data.ID_DOCUMENTO });

                                    
                                    source6.localdata = rs;
                                    dataAdapter6.dataBind();
                                    $("#jGrid6").jqxGrid('ensurerowvisible', 0);
                                    $("#jGrid6").jqxGrid('clearselection');


                                    $("#jGrid6").jqxGrid("autoresizecolumns");

                                    var colDefs = $("#jGrid6").jqxGrid('columns').records;
                                    for ( var idx = 0; idx < colDefs.length; idx++) {
                                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                                            $("#jGrid6").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                        }
                                    }
                                    
                                        
                                    $("#detalle3").dialog({
                                        buttons: {
                                            'Cerrar': function () {
     
                                                $(this).dialog("close");
                                            }
                                        }
                                    });


                                    $("#detalle3").dialog('open');
                                    $('#init').addClass('no-show');
                                }, 10);

                            }
                        },
                        { text: 'SERIE',               dataField: 'SERIE',               width: 100, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 100, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB },
                        { text: 'NumParcialidad',      dataField: 'NUM_PARCIALIDAD',     width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB },
                        { text: 'ImpSaldoAnt',         dataField: 'IMP_SALDO_ANT',       width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'ImpPagado',           dataField: 'IMP_PAGADO',          width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'ImpSaldoInsoluto',    dataField: 'IMP_SALDO_INSOLUTO',  width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'MonedaDR',            dataField: 'MONEDA_DR',           width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'MetodoDePagoDR',      dataField: 'METODO_DE_PAGO_DR',   width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'ID_DOCUMENTO',        dataField: 'ID_DOCUMENTO',        width: 300, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB },

                    ]
                });





//*****************************************************************************************************************


                source6 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        { name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        {name: 'CONTRA_RECIBO_ID' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'TIPO_REL' },
                        { name: 'FOLIO' },
                        //{ name: 'REVISADO' },
                        {name: 'TIENDA' },
                        { name: 'NO_COMPRA' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'VERSION' },
                        {name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'DESCUENTO' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'FECHA_PROCESO', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'ESTATUS_ID' }
                    ],
                    localdata: []
                };
                var dataAdapter6 = new $.jqx.dataAdapter(source6);

                $("#jGrid6").jqxGrid({ 
                    width: 1024, //1017
                    height: 100,
                    source: dataAdapter6,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
                    enablebrowserselection :true,
                    ready: function () {
/*                        var colDefs = $("#jGrid").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {
                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width+10);
                        }*/
                        //$("#jGrid").jqxGrid('autoresizecolumns');
                    },
                    //selectionmode: 'none',
                    //filterable: false,
                    //showfilterrow: true,
                    //rowsheight: 36,
                    //altrows: true,
                    //autorowheight: true,
                    //pageable: true,
                    //columnsheight: 40,
                    columns: [
                    //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'CONTR',           dataField: 'CONTRA_RECIBO_ID', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL',           dataField: 'TOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',        dataField: 'SUBTOTAL',         width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',       dataField: 'IMPUESTOS',        width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',             dataField: 'IVA',              width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',            dataField: 'IEPS',             width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'DESCUENTO',       dataField: 'DESCUENTO',        width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SERIE',           dataField: 'SERIE',            width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',           dataField: 'FOLIO',            width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA',          dataField: 'TIENDA',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: '#COMPRA',         dataField: 'NO_COMPRA',        width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA COMPRA',    dataField: 'FECHA_COMPRA',     width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy'},
                    //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                    //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',         dataField: 'ESTATUS',          width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',   dataField: 'FECHA_FACTURA',    width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'FECHA RECEPCION', dataField: 'FECHA_RECEPCION',  width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'FECHA PROCESO',   dataField: 'FECHA_PROCESO',    width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'UUID',            dataField: 'UUID',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                    //{ text: 'VERSION',             dataField: 'VERSION',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                        text: 'XML',
                        datafield: 'Show1',
                        columntype: 'button',
                        width: 60,
                        renderer: columnrenderer2,
                        cellsrenderer: function () {
                            return "--";
                        },
                        buttonclick: function (row) {

                            var dataRecord = $("#jGrid6").jqxGrid('getrowdata', row);

                            $('#init').removeClass('no-show');

                            //alert(dataRecord.ID);
                            getData("archivoXML.aspx/SetID", {
                                id: dataRecord.ID
                            });

                            window.open("archivoXML.aspx", "xml", "width=200,height=150");

                            $('#init').addClass('no-show');

                        }
                    },
                        {
                            text: 'PDF',
                            datafield: 'Show2',
                            columntype: 'button',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid6").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                getData("archivoPDF.aspx/SetID", {
                                    id: dataRecord.ID
                                });

                                window.open("archivoPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        }
                        ,
                        {
                            text: 'PDF Prov',
                            datafield: 'Show3',
                            columntype: 'button',
                            width: 70,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid6").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                var pdf = getData("archivoPDF_Prov.aspx/SetID",
                                    {
                                        id: dataRecord.ID
                                    });

                                if (!$.isEmptyObject(pdf)) {
                                    window.open("archivoPDF_Prov.aspx", "xml", "width=200,height=150");
                                } else {
                                    $("#message").html("<div style='margin-top: 7px;font-weight: bold;font-size:12px'>El registro seleccionado<br />no contiene pdf de proveedor.</div>");
                                    $("#message").dialog('open');

                                    position('.mWin');
                                }

                                $('#init').addClass('no-show');

                            }
                        }

                        //{ text: ' ', minwidth: 0, width: 'auto', sortable: false }
                    ]
                });

//*****************************************************************************************************************


                //*****************************************************************************************************************


                //---------------------------------------------------------------------------


                $("#jGrid").bind('bindingcomplete', function () {
                    //$('#jGrid').jqxGrid('autoresizecolumns');

                    //console.debug(column / 2);

                    $("#jGrid .jqx-grid-column-header.jqx-grid-column-header-Theme2.jqx-widget-header.jqx-widget-header-Theme2 .iconscontainer").each(function () {
                        $(this).next().remove();
                    });

                });



                //---------------------------------------------------------------------------

                $('#bCarga').click(function () {
                    $("#inv").dialog('open');
                    position('.mWin2');
                    
                });

                $('#bCarga2').click(function () {
                    $("#inv2").dialog('open');
                    position('.mWin2');
                    
                });



                $('#bRep').click(function () {

                    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');


                    var index1 = $("#cbEstatus").jqxDropDownList('selectedIndex');
                    var estatus = $("#cbEstatus").jqxDropDownList('getItem', index1).value;

                    var index2 = $("#cbRFC").jqxComboBox('selectedIndex');
                    if (index2 == -1 && uuid == "") {

                        $("#message")
                            .html(
                                "<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
                        $("#message").dialog('open');
                        return;
                    }

                    
                    var rfc = $("#cbRFC").jqxComboBox('getItem', index2).label;
                    
                    
                    getData("excel6.aspx/SetID", {
                        req: {
                            fecha1: fecha1,
                            fecha2: fecha2,
                            RFC_EMISOR: rfc,
                            estatus: estatus
                        }
                    });

                    window.open("excel6.aspx", "xml", "width=200,height=150");
                });
                //----------------------------------------------------------------------------//

                $("#bProv").click(function(event) {

 
                    $("#prov").dialog('open');


                    $("#xcb").html('<div class="jCbX" id="cbRFC3"></div>');

                    $("#cbRFC3").jqxComboBox({
                        width: '460px',
                        height: 25,
                        searchMode: 'containsignorecase',
                        promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                        theme: 'Theme2',
                        displayMember: 'RAZON_SOCIAL_EMISOR', valueMember: 'ID',
                        source: prov,
                        dropDownHeight: 200, 
                        autoDropDownHeight: auto
                    });

                    $("#cbRFC3").bind('select', function (event) {
                        if (event.args && event.args.item) {
                            var record = event.args.item.originalItem;
                            $("#rfc3").html(record.RFC_EMISOR);
                       
                        }
                    });

                
                    $("#cbRFC3").bind('unselect', function (event) {

                        if (event.args && event.args.item) {
                            $("#rfc3").html('');
                            ///$("#cbRFC3").jqxComboBox({selectedIndex: -1});
                        }
                    });


                    $("#rfc3").html('');

                    $('.mWin').css({
                        left: 100,
                        top: 100
                    });
                });

                //---------------------------------------------------------------------------
                $("#bExportar").click(function (event) {
                    $("#exportar").dialog('open');
                });

                $("#bBuscar").click(function (event) {
                    buscar();
                });

                
                function buscar() {
                    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/
                  
                            var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
                            var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');

                            var uuid = $("#txUUID").val().trim();
                            var folio = $("#txFolio").val();
                            var serie = $("#txSerie").val();

                            var index1 = $("#cbEstatus").jqxDropDownList('selectedIndex');
                            var estatus = $("#cbEstatus").jqxDropDownList('getItem', index1).value;

                            var index2 = $("#cbRFC").jqxComboBox('selectedIndex');
                            if (index2 == -1 && uuid == "") {

                                $("#message")
                                    .html(
                                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
                                $("#message").dialog('open');
                                return;
                            }


                            $('#init').removeClass('no-show');
                            setTimeout(function () {

                                    var id = 0;
                                    var rfc = null;

                                    if (index2 != -1) {
                                        id = $("#cbRFC").jqxComboBox('getItem', index2).value;
                                        rfc = $("#cbRFC").jqxComboBox('getItem', index2).label;
                                    }

                                    if (uuid != "") {
                                        id = 0;
                                        folio = "";
                                        serie = "";
                                    }

                                    if (id == 0) {
                                        rfc = null;
                                    }

                                    
                                    var data = getData("Pag8.aspx/GetRegistros",
                                        {
                                            req: {
                                                fecha1: fecha1,
                                                fecha2: fecha2,
                                                folio: folio,
                                                serie: serie,
                                                RFC_EMISOR: rfc,
                                                opt: opt,
                                                uuid: uuid,
                                                estatus: estatus
                                            }
                                        });

                                    if (uuid != "" && !isEmpty(data)) {
                                        $("#cbRFC").jqxComboBox('selectItem', data[0].RFC_EMISOR);
                                    }
                                    
                                    source.localdata = data;
                                    dataAdapter.dataBind();

                                    $("#jGrid").jqxGrid('clearselection');
                                    $("#jGrid").jqxGrid('updatebounddata', 'cells');

                                    $("#jGrid").jqxGrid("autoresizecolumns");

                                    var colDefs = $("#jGrid").jqxGrid('columns').records;
                                    for (var idx = 0; idx < colDefs.length; idx++) {
                                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                                        }

                                        if (colDefs[idx].datafield == "UUID") {
                                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 10);
                                        }
                                    }

                                    $("#jGrid").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
                                    $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                                    $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                                    $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                                    //$("#jGrid").jqxGrid('setcolumnproperty', "UUID", 'width', 290);

                                    $('#init').addClass('no-show');

                                }, 10);


                            /*var rows = $('#jGrid').jqxGrid('getrows');
                            for(var i = 0; i < rows.length; i++)
                            {
                                var row = rows[i];
                                $("#jGrid").jqxGrid("setcellvalue", i, "UUID", '<input id="" type="text" value="' + row.UUID + '" style="width: 99%;height:20px;margin-bottom:5px;" readonly />');
                            }*/


                            //window.open("excel.aspx", "xml", "width=200,height=150");
                }

                //UploadRW();
                //---------------------------------------------------------------------------

                $('#UploadFile').ajaxForm({

                    beforeSubmit: function () {
                        $("#Proc").button({ disabled: true });
                        $("#cExaminar1").button({ disabled: true });
                        $("#cExaminar2").button({ disabled: true });

                        //status.empty();
                        var percentVal = '0%';
                        bar.width(percentVal);
                        percent.html(percentVal);

                        var pm = new Array();

                        if ($.trim($("#archivo1").val()) == "") {
                            //param += ", <strong>Archivo</strong>";
                            pm.push("<strong>Archivo XML</strong>");
                        }

                        if ($.trim($("#archivo2").val()) == "") {
                            //param += ", <strong>Archivo</strong>";
                            //pm.push("<strong>Archivo PDF</strong>");
                        }


                        if (pm[0]) {
                            $("#Proc").button({ disabled: false });
                            $("#cExaminar1").button({ disabled: false });
                            $("#cExaminar2").button({ disabled: false });
                            //$("#serie, #semana, #anio").jqxInput({ disabled: false });

                            var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                            $("#message").html("<div style='text-align: left;min-width:250px;'>" + param + "</div>");
                            $("#message").dialog('open');
                            return false;
                        }

                        $('#init').removeClass('no-show');
                    },
                    uploadProgress: function (event, position, total, percentComplete) {
                        var percentVal = percentComplete + '%';
                        bar.width(percentVal);
                        percent.html(percentVal);
                    },
                    success: function (response, textStatus, xhr, form) {

                    },
                    complete: function (xhr) {

                        $('#init').addClass('no-show');
                        $("#Proc").button({ disabled: false });
                        $("#cExaminar1").button({ disabled: false });
                        $("#cExaminar2").button({ disabled: false });
                        //$("#serie, #semana, #anio").jqxInput({ disabled: false });
                        $("#archivo1").val('');
                        $("#archivo2").val('');

                        ////////////
                        resetFile();
                        
                        ////////////
                        
                        try {
                            
                            var rs = JSON.parse(xhr.responseText);

                            try {
                                //console.debug(JSON.parse(xhr.responseText));
                                //console.debug(xhr.responseText);
                                //source.localdata = rs.result;
                                //dataAdapter.dataBind();
                            } catch (err) {}

                            //var rs = xhr.responseText;

                            var percentVal = '100%';
                            bar.width(percentVal);
                            percent.html(percentVal);

                                $("#message").html(rs.msg);
                                $("#message").dialog('open');

                                position('.mWin');
                            

                        }
                        catch (err) {
                            //alert(err.message);
                            //console.debug(xhr.responseText);
                            //console.debug(xhr.responseText.msg);
                            window.location.href = "Pag8.aspx";
                        }
                    }
                });

                //**************************************************************************************************

                $('#UploadFile2').ajaxForm({

                    beforeSubmit: function () {
                        $("#Proc2").button({disabled: true});
                        $("#cExaminar3").button({ disabled: true });

                        //status.empty();
                        var percentVal = '0%';
                        bar.width(percentVal);
                        percent.html(percentVal);

                        var pm = new Array();

                        if ($.trim($("#archivo3").val() ) == "") {
                            //param += ", <strong>Archivo</strong>";
                            pm.push("<strong>Archivo ZIP</strong>");
                        }


                        if (pm[0]) {
                            $("#Proc2").button({disabled: false});
                            $("#cExaminar3").button({ disabled: false });

                            var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                            $("#message").html("<div style='text-align: left;min-width:250px;'>" + param + "</div>");
                            $("#message").dialog('open');
                            return false;
                        }

                        $('#init').removeClass('no-show');
                    },
                    uploadProgress: function (event, position, total, percentComplete) {
                        var percentVal = percentComplete + '%';
                        bar.width(percentVal);
                        percent.html(percentVal);
                    },
                    success: function (response, textStatus, xhr, form) {

                    },
                    complete: function (xhr) {
                    
                        $('#init').addClass('no-show');
                        $("#Proc2").button({ disabled: false });
                        $("#cExaminar3").button({ disabled: false });
                        $("#archivo3").val('');

                        ////////////
                        resetFile();
                        
                        ////////////
                        
                        try {
                            
                            var rs = JSON.parse(xhr.responseText);

                            try {
                                //console.debug(JSON.parse(xhr.responseText));
                                //console.debug(xhr.responseText);
                                //source.localdata = rs.result;
                                //dataAdapter.dataBind();
                            } catch (err) {}

                            //var rs = xhr.responseText;

                            var percentVal = '100%';
                            bar.width(percentVal);
                            percent.html(percentVal);

                                //$("#message").html(rs.msg);
                                //$("#message").dialog('open');

                            $('#init').removeClass('no-show');

                            $("#detalle").dialog({ width: Math.min(1050, $(window).width()) });
                            $("#detalle").dialog('open');
                            source2.localdata = rs.result;
                            dataAdapter2.dataBind();
                            $("#jGrid2").jqxGrid('clearselection');
                            $("#jGrid2").jqxGrid('ensurerowvisible', 0);

                            $('#init').addClass('no-show');
                            

                        }
                        catch (err) {
                            //alert(err.message);
                            //console.debug(xhr.responseText);
                            //console.debug(xhr.responseText.msg);
                            window.location.href = "Pag8.aspx";
                        }
                    }
                });


                //**************************************************************************************************
                function position(wnd) {
                    var windowHeight = ($(window).height() / 2) - ($(wnd).outerHeight() / 2);
                    var windowWidth = ($(window).width() / 2) - ($(wnd).outerWidth() / 2);
                    if (windowHeight < 0) windowHeight = 5;

/*                    $('.ui-dialog').css({
                        left: windowWidth,
                        top: windowHeight
                    });*/
                    $(wnd).css({
                        left: windowWidth,
                        top: windowHeight
                    });
                }

                $("#prov").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin',
                    buttons: {
                        Aceptar: function () {

                            if ($("#rfc3").html() != '') {
                                var index2  = $("#cbRFC3").jqxComboBox('selectedIndex');
                                $("#cbRFC").jqxComboBox({selectedIndex: index2});
                            }

                            $(this).dialog("close");
                        },
                        Cancelar: function () {

                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    }
                });

                $("#confirmar").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin',
                    open: function (event, ui) {

                    }
                });

                $("#dialog").dialog({
                    close: function () {
                        $('#jqxFileUpload').jqxFileUpload('cancelAll');
                    },
                    autoOpen: false,
                    resizable: false,
                    modal: true,
                    width: 'auto',
                    minHeight: 'auto',
                    dialogClass: 'myClass'
                });

                $("#message").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin',
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    }
                });



                $("#inv").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Continuar': function () {
                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    },
                    close: function (event, ui) {
                        bar.width('0%');
                        percent.html('0%');
                    }
                });

                $("#inv2").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Continuar': function () {
                            bar.width('0%');
                            percent.html('0%');

                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    },
                    close: function (event, ui) {
                        bar.width('0%');
                        percent.html('0%');
                    }
                });




                $("#detalle2").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin',
                    buttons: {
                        'Cerrar': function () {
 
                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    },
                    create: function( event, ui ) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    },
                    close: function() {
                        source5.localdata = [];
                        dataAdapter5.dataBind();
                        $("#jGrid5").jqxGrid('updatebounddata', 'cells');
                    }
                });


                $("#detalle3").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin',
                    buttons: {
                        'Cerrar': function () {
 
                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    },
                    create: function( event, ui ) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    },
                    close: function() {
                        source6.localdata = [];
                        dataAdapter6.dataBind();
                        $("#jGrid6").jqxGrid('updatebounddata', 'cells');
                    }
                });




                $("#detalle").dialog({
                    autoOpen: false,
                    resizable: false,
                    //width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Cerrar': function () {
 
                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    },
                    create: function( event, ui ) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });

                $("#exportar").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Exportar': function () {

                            var chkXML = $("#chkXML").jqxCheckBox('checked');
                            var chkPDF = $("#chkPDF").jqxCheckBox('checked');
                            var chkPDF2 = $("#chkPDF2").jqxCheckBox('checked');

                            $('#init').removeClass('no-show');

                            var rowindexes = $('#jGrid').jqxGrid('getselectedrowindexes');
                        
                            var select = new Array();
                            $.each(rowindexes, function( index, value ) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);
                                select.push(data.ID);
                            });

                            if (!$.isEmptyObject(select)) {
                                getData("excel5.aspx/SetID", {
                                    data:select,
                                    chkXML:chkXML,
                                    chkPDF: chkPDF,
                                    chkPDF2: chkPDF2
                                });

                                window.open("excel5.aspx", "xml", "width=200,height=150");
                            }

                            $('#init').addClass('no-show');
                            $(this).dialog("close");
                        },
                        'Cerrar': function() {

                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    }
                });


                $("#cExaminar1").click(function () {
                    $("#cfile1").click();
                });

                $("#cExaminar2").click(function () {
                    $("#cfile2").click();
                });

                $("#cExaminar3").click(function () {
                    $("#cfile3").click();
                });

                $('input[type="file"]').attr('title', window.webkitURL ? ' ' : '');

                
                $(".mWin2").css('z-index', 999);
                $(".mWin3").css('z-index', 999);
                $(".mWin").css('z-index', 999);

                $(".ui-dialog-content").css("padding-top", 0);
                $(".ui-dialog-content").css("padding-left", 6);
                $(".ui-dialog-content").css("padding-right", 6);
                $(".ui-dialog-content").css("padding-bottom", 5);
            });
        </script>
        

</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="contenido" runat="server">
    <div id="init" class="no-show se-pre-con2">
        <div class="jqx-rc-all jqx-fill-state-normal" style="z-index: 99999; margin-left: -66px; left: 50%; top: 50%; margin-top: -24px; position: relative; width: 110px; height: 33px; padding: 5px; font-family: verdana; font-size: 12px; color: #767676; border-color: #898989; border-width: 1px; border-style: solid; background: #f6f6f6; border-collapse: collapse;">
            <div style="float: left;">
                <div style="float: left; overflow: hidden; width: 32px; height: 32px;" class="jqx-grid-load"></div>
                <span style="margin-top: 10px; float: left; display: block; margin-left: 5px;">Loading...</span>
            </div>
        </div>
    </div>
    
    
    <table style="width: 99%; border: 1px #b4b2b2 solid; margin: auto;">
        <tr>
            <td style="border:1px #b4b2b2 solid" valign="top">
                

                <table style="width: 100%">
                    <tr>
                        <td class="a1" style="text-align: left; padding: 5px; width: 170px;">
                            <table style="">
                                <tr>
                                    <td style="padding-top: 20px;">
                                        <input type="button" id="bProv" class="btn1" style="height: 28px; padding-top: 0px; font-size: 0.5em;" value=".."/>
                                    </td>
                                    <td>
                                        <div>RFC Emisor</div>
                                        <div class="jCbX" id="cbRFC" style="width: 50px !important;"></div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; width: 170px;">
                            <div>Serie</div>
                            <div><input class="jTx" id="txSerie" type="text" value="" style="width: 200px;" /></div>

                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; width: 170px;">
                            <div>Folio</div>
                            <div><input class="jTx" id="txFolio" type="text" value="" style="width: 170px;" /></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; padding-top: 22px;">
                            <div id="jqxOpt1">Fecha Recepción</div>
                            <%--<div id="jqxOpt2">Fecha Saldado</div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="a1" style="text-align: left; padding: 5px;">
                            <div>Fecha Inicial <span id="lbFecha1">Recepción</span></div>
                            <div id="jDate1" style="width: 170px !important;"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px;">
                            <div>Fecha Final <span id="lbFecha2">Recepción</span></div>
                            <div id="jDate2" style="width: 170px !important;"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px;">
                            <div>UUID</div>
                            <div><input class="jTx" id="txUUID" type="text" value="" style="width: 170px;" /></div>
                        </td>
                        <td style="text-align: left; padding: 5px; padding-top: 22px;">
                            <input type="button" id="bRep" class="btn1" style="width: 145px; height: 30px; padding-top: 5px; font-size: 1em;" value="Generar Reporte"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="a1" style="text-align: left; padding: 5px;" colspan="2">
                             <div>Razon Social</div>
                             <div class="txCenter" id="razonSocial" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px;">
                            <div>Estatus</div>
                            <div class="jCb" id="cbEstatus" style="width: 160px !important;"></div>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="a1" colspan="4" style="padding: 5px;">
                            


                        </td>
                    </tr>

                </table>
                

                
                
            </td>
        </tr>

    </table>

                                <div style="padding: 10px;">

                                    <table style="width: 100%; text-align: right; border: solid 1px #e4e4e4; border-bottom: solid 1px #e4e4e4; margin: auto; background-color: #fbfbfb">
                                        <tr>
                                            <td align="left">
                                                <%--Total: <span id="total1"></span>--%>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <!--
                                                        <td>
                                                            BD:
                                                        </td>
                                                        <td>
                                                            <div class="" id="cbBD" style="margin-left: 4px; margin-right: 4px;"></div>
                                                        </td>-->

                                                        <td>
                                                            <input type="button" id="bCarga" class="btn1" style="width: 130px; height: 30px; padding-top: 5px; font-size: 1em;" value="Importar XML"/>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="bCarga2" class="btn1" style="width: 130px; height: 30px; padding-top: 5px; font-size: 1em;" value="Importar ZIP"/>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="bBuscar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                                        </td>
                                                        <%--<td>
                                                                    <input type="button" id="bCarga2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Importar ZIP"/>
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="bCarga" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Importar XML"/>
                                                                </td>--%>
                                                        <td>
                                                            <input type="button" id="bExportar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="jGrid" style="margin-bottom: 0px; margin-top: 5px;"></div>
                                </div>


    <%--<div style="border: solid 1px black;padding: 0px;max-width: 99%;margin: auto" >--%>
        <div class="content">


                
                <div id="inv" title="Carga XML">
                        <form id="UploadFile" style="max-width: 400px; margin: auto;" action="fileUploader2.asmx/Upload3" method="POST" enctype="multipart/form-data">
                            <div class="" style="margin-top: 0px; margin-bottom: 0px;">

                                <table style="width: 400px; margin-top: 10px;">
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">
    <%--                                        <div class="fileUpload btn1" style="width: 100%">
                                            </div>--%>
                                            <input type="button" id="cExaminar1" class="btn1" style="height: 30px; width: 100%; padding-top: 5px; font-size: 1em;" value="(XML) Examinar..."/>
                                            <div id="dFile1">
                                            <input type="file" class="upload" id="cfile1" name="myfile1" accept=".xml" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px; padding-right: 10px;">
                                            <input class="jTx txCenter" id="archivo1" style="width: 100%" readonly="readonly" type="text" value="" autocomplete="off"/>
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 400px; margin-top: 10px;">
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">
                                            <%--                                        <div class="fileUpload btn1" style="width: 100%">
                                            </div>--%>
                                            <input type="button" id="cExaminar2" class="btn1" style="height: 30px; width: 100%; padding-top: 5px; font-size: 1em;" value="(PDF) Examinar..."/>
                                            <div id="dFile2">
                                                <input type="file" class="upload" id="cfile2" name="myfile2" accept=".pdf" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px; padding-right: 10px;">
                                            <input class="jTx txCenter" id="archivo2" style="width: 100%" readonly="readonly" type="text" value="" autocomplete="off"/>
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 400px; margin-top: 10px;">
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">
                                        
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="border:1px solid #e4e4e4;text-align: left; padding: 5px; width: 90px;">
                                                            <input type="submit" id="Proc" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Procesar Archivo"/>
                                                        </td>
                                                        <td style=" padding: 5px; border: 1px solid #e4e4e4">
                                                            <div class="progress">
                                                                <div class="bar"></div >
                                                                <div class="percent">0%</div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                        </form>
                    </div>
                
                
                
                <div id="inv2" title="Carga ZIP">
                        <form id="UploadFile2" style="max-width: 400px; margin: auto;" action="fileUploader2.asmx/Upload4" method="POST" enctype="multipart/form-data">
                            <div class="" style="margin-top: 0px; margin-bottom: 0px;">

                                <table style="width: 400px; margin-top: 10px;">
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">
    <%--                                        <div class="fileUpload btn1" style="width: 100%">
                                            </div>--%>
                                            <input type="button" id="cExaminar3" class="btn1" style="height: 30px; width: 100%; padding-top: 5px; font-size: 1em;" value="(ZIP) Examinar..."/>
                                            <div id="dFile3">
                                            <input type="file" class="upload" id="cfile3" name="myfile1" accept=".zip" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px; padding-right: 10px;">
                                            <input class="jTx txCenter" id="archivo3" style="width: 100%" readonly="readonly" type="text" value="" autocomplete="off"/>
                                        </td>
                                    </tr>
                                </table>

                                <table style="width: 400px; margin-top: 10px;">
                                    <tr>
                                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">
                                        
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="border:1px solid #e4e4e4;text-align: left; padding: 5px; width: 90px;">
                                                            <input type="submit" id="Proc2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Procesar Archivo"/>
                                                        </td>
                                                        <td style=" padding: 5px; border: 1px solid #e4e4e4">
                                                            <div class="progress">
                                                                <div class="bar"></div >
                                                                <div class="percent">0%</div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </form>
                    </div>
                
                
                
                <div id="message" title="Mensaje del sistema"></div>
                <div id="confirmar" title="Confirmar"></div>
                <div id="detalle" title="Mensaje del sistema"><div id="jGrid2" style="margin-bottom: 0px;margin-top: 7px"></div></div>

            
                <div id="detalle2" title="Detalle">
                    <div id="jGrid5" style="margin-top: 10px"></div>

                </div>

                <div id="detalle3" title="Detalle">
                    <div id="jGrid6" style="margin-top: 10px"></div>
                </div>

        
        
                
                <div id="exportar" title="Exportar">
                    <div id='chkXML' style="margin-top: 20px">Agregar XML</div>
                    <div id='chkPDF'>Agregar PDF proveedor</div>
                    <div id='chkPDF2'>Agregar PDF generico</div>
                </div>
                
            <div id="prov" title="Proveedores">
                <table style="margin-top: 10px;">
                    <tr>
                        <td style="text-align: left">
                            <div>Nombre Proveedor</div>
                            <span id="xcb">
                                <div class="jCbX" id="cbRFC3"></div>
                            </span>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <div>RFC</div>
                            <div class="txCenter" id="rfc3" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 200px !important;"></div>
                        </td>
                    </tr>
                </table>
            </div>
                
                <% FormsIdentity id = (FormsIdentity) HttpContext.Current.User.Identity;%>
                <% if (User.IsInRole("superuser")){  %>
                       <%--<%:id.Ticket.UserData%>--%>
                <% } else{  %>
                        <%--<%:id.Ticket.UserData%>--%>
                <%   }  %>
        </div>
        
    <%--</div>--%>
    <asp:Label ID="Script1" runat="server"></asp:Label>
</asp:Content>