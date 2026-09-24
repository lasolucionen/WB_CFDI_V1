<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag1.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        #jGrid .jqx-button
        {
            /*background-color: Yellow;*/
            /*color: Red;
            font-size: larger;*/
            width: 94% !important;
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
            var jGridRow = null;

            var source2 = {};

            var zipForm = {};

            //$(document).ready(function () {
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


                $(".jTx").jqxInput({ /*width: '99%',*/height: 25, theme: 'Theme2' });

                $("#jqxOpt1").jqxRadioButton({ width: 120, height: 25, checked: true });
                $("#jqxOpt2").jqxRadioButton({ width: 120, height: 25 });

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

                /*******************************************************************************/

                $("#jDate1").jqxDateTimeInput({ value: '<%=getFecha1() %>', /*width: '101.3%',*/height: '25px', theme: 'Theme2', culture: 'es' });
                $("#jDate2").jqxDateTimeInput({ /*width: '101.3%',*/height: '25px', theme: 'Theme2', dropDownHorizontalAlignment: 'right', culture: 'es-ES' });

                $(".jCb").jqxDropDownList({ height: 25, /*width: '101.3%',*/searchMode: 'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'
                });

                $("#cbEstatus").jqxDropDownList({
                    displayMember: 'ESTATUS', valueMember: 'ID',
                    source: getAdapter("Pag1.aspx/GetEstatus", {}),
                    selectedIndex: 0,
                    dropDownHeight: 200,
                    autoDropDownHeight: auto
                });


                /*
                $("#cbBD").jqxDropDownList({ height: 25, width: '80px', searchMode: 'containsignorecase',
                dropDownHeight: 200, autoDropDownHeight: true, promptText: '--------------------------',
                source: ['BD_PSI', 'BD_PSO'],
                theme: 'Theme2'
                });

                var bd = getData("Pag1.aspx/GetBD", {});
                $("#cbBD").jqxDropDownList('val', bd);

                $("#cbBD").bind('select', function (event) {
                if (event.args && event.args.item) {
                getData("Pag1.aspx/SetBD", { bd: event.args.item.value });

                source.localdata = [];
                dataAdapter.dataBind();
                $("#jGrid").jqxGrid('clearselection');
                $("#jGrid").jqxGrid('updatebounddata', 'cells');
                }
                });*/

                /*******************************************************************************/
                $("#chkXML").jqxCheckBox({ width: 180, height: 25 });
                $("#chkPDF").jqxCheckBox({ width: 180, height: 25 });
                /*******************************************************************************/

                $('#init').removeClass('no-show');
                setTimeout(function () {


                    $('#init').addClass('no-show');
                }, 1000);

                /*******************************************************************************/

                /*
                var rs = getData("Pag1.aspx/GetSemana");
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



                //---------------------------------------------------------------------------


                $.ajaxSetup({
                    statusCode: {
                        401: function () {
                            //window.location.href = "Login.aspx";
                        }
                    }
                });



            var cellD = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

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
                if (rowdata.BLOQ == true) {
                    return '<div style="font-size:8.5px; margin: 4px; margin-top:1px; text-align: ' +
                        columnproperties.cellsalign +
                        '; color:red;">' +
                        formattedValue +
                        '</div>';
                } else {
                    return '<div style="font-size:8.5px; margin: 4px; margin-top:1px; text-align: ' +
                        columnproperties.cellsalign +
                        ';">' +
                        formattedValue +
                        '</div>';
                }


            }


                //---------------------------------------------------------------------------
                var columnrenderer2 = function (value) {
                    return '<div style="text-align: center;margin-top: 7px;">' + value + '</div>';
                }

                source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'OBSERVACION1' },
                        { name: 'RFC_EMISOR' },
                    //{ name: 'RAZON_SOCIAL_EMISOR' },
                    //{ name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        { name: 'TIENDA' },
                        { name: 'NO_COMPRA' },
                        { name: 'ALBARAN' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'REVISADO' },
                        { name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'FECHA_PAGO', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'NUMEFECTO' },
                        { name: 'FECHA_SALDADO', type: 'date' },
                        { name: 'CHECK_ID' },
                        { name: 'CHECK_DATE', type: 'date' },
                        { name: 'CHECK_NUMBER' }
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
                    editable: false,
                    rendered: function () {

                        /*********************************************************/
                        $("#jGrid").jqxGrid("autoresizecolumns");

                        /*
                        var colDefs = $("#jGrid").jqxGrid('columns').records;
                        for (var idx = 0; idx < colDefs.length; idx++) {
                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                $("#jGrid").jqxGrid('setcolumnproperty',
                                    colDefs[idx].datafield,
                                    'width',
                                    colDefs[idx].width + 5);
                            }

                            if (colDefs[idx].datafield == "UUID") {
                                $("#jGrid").jqxGrid('setcolumnproperty',
                                    colDefs[idx].datafield,
                                    'width',
                                    colDefs[idx].width + 10);
                            }
                        }
                        */

                        $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 40);
                        $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 40);
                        
                        $("#jGrid").jqxGrid('setcolumnproperty', "Show4", 'width', 65);
                        $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 82);
                        $("#jGrid").jqxGrid('setcolumnproperty', "Detalle", 'width', 54);
                        $("#jGrid").jqxGrid('setcolumnproperty', "OBSERVACION1", 'width', 84);
                           /*********************************************************/

                    },
                    rowsheight: 43,
                    enablebrowserselection: true,
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
                        { text: 'CONTR',      dataField: 'CONTRA_RECIBO_ID', width: 130, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'RFC EMISOR', dataField: 'RFC_EMISOR',       width: 100, renderer: columnrenderer2, editable: false }, //, pinned: true 
                        { text: 'TOTAL',      dataField: 'TOTAL',            width: 130, renderer: columnrenderer2, editable: false, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',   dataField: 'SUBTOTAL',         width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',  dataField: 'IMPUESTOS',        width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',        dataField: 'IVA',              width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',       dataField: 'IEPS',             width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SERIE XML',      dataField: 'SERIE',            width: 100, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'FOLIO XML',      dataField: 'FOLIO',            width: 100, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'ALBARAN', dataField: 'ALBARAN', width: 100, renderer: columnrenderer2, editable: false, /*cellsalign: 'center',*/ cellsrenderer: cellD },
                        /*{ text: 'SERIE',     dataField: 'TIENDA',           width: 100, renderer: columnrenderer2, cellsalign: 'center',
                            initeditor: function (row, column, editor) {
                                editor.attr('maxlength', 5);
                            }
                        },
                        { text: 'FOLIO',      dataField: 'NO_COMPRA',    width: 100, renderer: columnrenderer2, cellsalign: 'center' },*/
                        
                        //{ text: 'FECHA COMPRA', dataField: 'FECHA_COMPRA', width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        {
                            text: 'Detalle',
                            datafield: 'Detalle',
                            columntype: 'button',
                            width: 50,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                jGridRow = row;
                                IDgrid = dataRecord.ID;
                                IDrow = row;
                                $("#detalle2").dialog('open');
                                
                                detalle();

                            }
                        },
                        //{ text: 'REVISADO',           dataField: 'REVISADO',        width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                    //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',            dataField: 'ESTATUS',         width: 130, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',      dataField: 'FECHA_FACTURA',   width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'FECHA RECEPCION',    dataField: 'FECHA_RECEPCION', width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                    //{ text: 'UUID',                dataField: 'UUID',                width: 300, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'COMENTARIO',         dataField: 'NUMEFECTO',       width: 130, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'FECHA SALDADO',      dataField: 'FECHA_SALDADO',   width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'FECHA PROMESA PAGO', dataField: 'FECHA_PAGO',      width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        { text: 'CHECK_ID',           dataField: 'CHECK_ID',        width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'CHECK_NUMBER',       dataField: 'CHECK_NUMBER',    width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center' },
                        { text: 'CHECK_DATE',         dataField: 'CHECK_DATE',      width: 160, renderer: columnrenderer2, editable: false, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },

                       /* {
                            text: 'PDF Carta Pago',
                            datafield: 'Show3',
                            columntype: 'button',
                            width: 100,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                getData("cartaPagoPDF.aspx/SetID",
                                    {
                                        id: dataRecord.CONTRA_RECIBO_ID
                                    });

                                window.open("cartaPagoPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }

                        },*/


                        {
                            text: 'Observacion',
                            datafield: 'OBSERVACION1',
                            columntype: 'button',
                            //width: 76,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $("#observacion1")
                                    .html(dataRecord.OBSERVACION1);

                                $("#observacion1").dialog('open');

                                //IDgrid = dataRecord.ID;

                                //$("#detalle5").dialog('open');

                            }
                        },

                        {
                            text: 'CONTR PDF',
                            datafield: 'Show3',
                            columntype: 'button',
                            //width: 80,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                getData("contrareciboPDF.aspx/SetID",
                                    {
                                        id: dataRecord.CONTRA_RECIBO_ID
                                    });

                                window.open("contrareciboPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }

                        },

                        {
                            text: 'XML',
                            datafield: 'Show1',
                            columntype: 'button',
                            //width: 90,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

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
                            //width: 90,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

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
                            datafield: 'Show4',
                            columntype: 'button',
                            //width: 70,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

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




                    /*{ text: 'VERSION',             dataField: 'VERSION',             width:  70, renderer: columnrenderer2, cellsalign: 'center' },
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
                    cellsrenderer: function() {
                    return "--";
                    },
                    buttonclick: function(row) {

                    var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);
                                
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
                    cellsrenderer: function() {
                    return "--";
                    },
                    buttonclick: function(row) {

                    var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);
                                
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
                    }*/

                    //{ text: ' ', minwidth: 0, width: 'auto', sortable: false }
                    ]
                   });

                   //-------------------------------------

                   var modificado = false;
                   function detalle() {

                       $("#bGuardar").button("disable");

                       /*
                           .attr('disabled', true)
                           .addClass("ui-state-disabled");*/


                       $("#bEliminar").button({ disabled: true });
                       $("#bModificar").button({ disabled: true });
                       $("#bAgregar").button({ disabled: true });
                       Editable = false;

                       $('#init').removeClass('no-show');
                       setTimeout(function () {
                           var rs = getData("Pag1.aspx/GetDetalle", { id: IDgrid });

                           //if (rs.data.length == 0) {
                               //Editable = true;

                               $("#bGuardar").button("enable");
                               /*
                               $("#bGuardar")
                                   .attr('disabled', false)
                                   .removeClass("ui-state-disabled");*/

                               //$("#bEliminar").button({ disabled: false });
                               //$("#bModificar").button({ disabled: false });
                               $("#bAgregar").button({ disabled: false });
                           //} 

                           source3.localdata = rs.data;
                           dataAdapter3.dataBind();

                           source.localdata[jGridRow].ALBARAN = rs.data2;
                           dataAdapter.dataBind();







                           //$("#jGrid3").jqxGrid('updatebounddata', 'cells');
                           $("#jGrid3").jqxGrid('ensurerowvisible', 0);
                           $("#jGrid3").jqxGrid('clearselection');


                           $("#jGrid3").jqxGrid("autoresizecolumns");

                           var colDefs = $("#jGrid3").jqxGrid('columns').records;
                           for (var idx = 0; idx < colDefs.length; idx++) {
                               if (colDefs[idx].datafield != "_checkboxcolumn") {
                                   $("#jGrid3").jqxGrid('setcolumnproperty',
                                       colDefs[idx].datafield,
                                       'width',
                                       colDefs[idx].width + 5);
                               }
                           }

                           $('#init').addClass('no-show');
                       }, 30);
                   }


                    //*****************************************************************************************************************
                    source3 =
                    {
                        datatype: "json",
                        datafields: [
                            { name: 'SERIE' },
                            { name: 'FOLIO' }
                        ],
                        localdata: []
                    };

                    var dataAdapter3 = new $.jqx.dataAdapter(source3);

                    $("#jGrid3").jqxGrid({
                        width: 285,
                        height: 390,
                        source: dataAdapter3,
                        theme: 'Theme2',
                        sortable: true,
                        enableHover: false,
                        selectionmode: 'singlerow',
                        enablebrowserselection: true,
                        rowsheight: 36,
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
                        { text: 'SERIE', dataField: 'SERIE', width: 145, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                        { text: 'FOLIO', dataField: 'FOLIO', width: 145, renderer: columnrenderer2, cellsalign: 'center' }

                    ]
                    });


                    $("#jGrid3").bind('rowclick',
                        function(event) {
                            //if (Editable) {
                                $("#bEliminar").button({ disabled: false });
                                $("#bModificar").button({ disabled: false });
                            //}

                        });


                    //---------------------------------------------------------------------------

                    var IDgrid=-1;
                    var IDrow=-1;
                    $("#bAgregar").click(function () {


                        $("#cAgregar").dialog({
                            buttons: {
                                'Agregar': function() {

                                    var it1 = $.trim($("#cSerie1").val());
                                    var it2 = $.trim($("#cFolio1").val());

                                    var pm = new Array();

                                    if ($.trim(it1) == "") {
                                        pm.push("<strong>SERIE</strong>");
                                    }

                                    if ($.trim(it2) == "") {
                                        pm.push("<strong>FOLIO</strong>");
                                    }

                                    if (pm[0]) {
                                        var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                                        $("#message").html("<div style='text-align: left;min-width:250px;margin-top:8px;'>" + param + "</div>");
                                        $("#message").dialog('open');
                                        return false;
                                    }

                                    var rows = $("#jGrid3").jqxGrid('getrows');

                                    var add = true;
                                    try {
                                        $.each(rows,
                                            function(index, value) {
                                                if (value.SERIE.toLowerCase() == it1.toLowerCase() && value.FOLIO.toLowerCase() == it2.toLowerCase() ) {
                                                    add = false;
                                                    throw new TypeError();
                                                }
                                            });
                                    } catch (e) {
                                    }

                                    if (!add) {
                                        $("#message").html("<div style='text-align: left;min-width:250px;margin-top:8px;'>El registro ya existe: <br /><b>SERIE:</b> " + it1 + "<br /><b>FOLIO:</b> " + it2 + "</div>");
                                        $("#message").dialog('open');
                                        return false;
                                    }

                                    var data = { SERIE: it1, FOLIO: it2 };

                                    //$("#jGrid3").jqxGrid('beginupdate');
                                    //$("#jGrid3").jqxGrid('addrow', null, data);
                                    //$("#jGrid3").jqxGrid('endupdate');

                                    source3.localdata.push(data);
                                    dataAdapter3.dataBind();
                                    //$("#jGrid3").jqxGrid('updatebounddata', 'cells');

                                    $("#jGrid3").jqxGrid("autoresizecolumns");

                                    var colDefs = $("#jGrid3").jqxGrid('columns').records;
                                    for (var idx = 0; idx < colDefs.length; idx++) {
                                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                                            $("#jGrid3").jqxGrid('setcolumnproperty',
                                                colDefs[idx].datafield,
                                                'width',
                                                colDefs[idx].width + 5);
                                        }
                                    }

                                    modificado = true;

                                    $("#cSerie1").val('');
                                    $("#cFolio1").val('');


                                },
                                'Cerrar': function() {

                                    $(this).dialog("close");
                                }
                            }
                        });

                        $("#cAgregar").dialog('open');

                    });


                    $("#bModificar").click(function() {
                        var inx = $("#jGrid3").jqxGrid('getselectedrowindex');
                        var id = $("#jGrid3").jqxGrid('getrowid', inx);
                        var data = $("#jGrid3").jqxGrid('getrowdatabyid', id);

                        if (inx != -1) {

                            $("#cSerie2").val(data.SERIE);
                            $("#cFolio2").val(data.FOLIO);

                            $("#cModificar").dialog({
                                buttons: {
                                    'Guardar': function () {

                                        var it1 = $.trim($("#cSerie2").val());
                                        var it2 = $.trim($("#cFolio2").val());

                                        if (data.SERIE == it1 && data.FOLIO == it2) {
                                            $(this).dialog("close");
                                            return false;
                                        }



                                        var pm = new Array();

                                        if ($.trim(it1) == "") {
                                            pm.push("<strong>SERIE</strong>");
                                        }

                                        if ($.trim(it2) == "") {
                                            pm.push("<strong>FOLIO</strong>");
                                        }

                                        if (pm[0]) {
                                            var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                                            $("#message").html("<div style='text-align: left;min-width:250px;margin-top:8px;'>" + param + "</div>");
                                            $("#message").dialog('open');
                                            return false;
                                        }


                                        var rows = $("#jGrid3").jqxGrid('getrows');

                                        var update = true;
                                        try {
                                            $.each(rows,
                                                function(index, value) {
                                                    if (value.SERIE.toLowerCase() == it1.toLowerCase() &&
                                                        value.FOLIO.toLowerCase() == it2.toLowerCase()) {
                                                        update = false;
                                                        throw new TypeError();
                                                    }
                                                });
                                        } catch (e) {
                                        }

                                        if (!update) {
                                            $("#message")
                                                .html(
                                                    "<div style='text-align: left;min-width:250px;margin-top:8px;'>El registro ya existe: <br /><b>SERIE:</b> " +
                                                    it1 +
                                                    "<br /><b>FOLIO:</b> " +
                                                    it2 +
                                                    "</div>");
                                            $("#message").dialog('open');
                                            return false;
                                        }


                                        //$("#jGrid3").jqxGrid('beginupdate');

                                            modificado = true;
                                            source3.localdata[id].SERIE = it1;
                                            source3.localdata[id].FOLIO = it2;

                                            //$("#jGrid3").jqxGrid('setcellvalue', id, "SERIE", it1);
                                            //$("#jGrid3").jqxGrid('setcellvalue', id, "FOLIO", it2);
                                            //source3.localdata = [];

                                            //$("#jGrid3").jqxGrid('updatebounddata');

                                            //console.log("ID: " + id + " INX:" + inx);

                                            dataAdapter3.dataBind();
                                            $(this).dialog("close");

                                    },
                                    'Cancelar': function () {

                                        $(this).dialog("close");
                                    }
                                }
                            });
                            $("#cModificar").dialog('open');
                            
                        }

                    });


                    $("#bEliminar").click(function () {
                        var inx = $("#jGrid3").jqxGrid('getselectedrowindex');
                        var id = $("#jGrid3").jqxGrid('getrowid', inx);
                        var data = $("#jGrid3").jqxGrid('getrowdatabyid', id);

                        if (inx != -1) {
                            //$("#jGrid3").jqxGrid('deleterow', id);

                            modificado = true;

                            var position = $("#jGrid3").jqxGrid('scrollposition');

                            source3.localdata.splice(id,1);
                            $("#jGrid3").jqxGrid('clearselection');
                            dataAdapter3.dataBind();

                            $("#jGrid3").jqxGrid('scrolloffset', position.top, 0);

                            $("#bEliminar").button({ disabled: true });
                            $("#bModificar").button({ disabled: true });
                        }

                    });

                //---------------------------------------------------------------------------
                /*
                $("#jGrid").on('cellendedit', function (event) {
                var args = event.args;
                var columnField = args.datafield;
                var rowIndex = args.rowindex;
                var cellValue = args.value;
                var oldValue = args.oldvalue;

                if (columnField == "TIENDA" && cellValue != oldValue) {

                getData("Pag1.aspx/Save",
                {
                id: args.row.ID,
                tienda: cellValue
                });
                }
                });*/

                $("#jGrid").on('rowselect', function (event) {
                    total1();
                });

                $("#jGrid").bind('rowunselect', function (event) {
                    total1();
                });


                function total1() {
                    var arr1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var importe = 0;
                    $.each(arr1,
                        function (index, value) {
                            var data = $('#jGrid').jqxGrid('getrowdata', value);
                            importe += data.TOTAL;
                        });

                    importe = numeral(importe).format('$0,0.00');
                    $("#total1").html(importe);
                }

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
                    enablebrowserselection: true,
                    rowsheight: 65,
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
                        { text: 'PDF',     dataField: 'PDF',     width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'DETALLE',
                            dataField: 'DETALLE',
                            width: 290,
                            renderer: columnrenderer2,
                            cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                            }
                        },
                        {
                            text: 'Validacion XML',
                            dataField: 'MENSAJE',
                            width: 280,
                            renderer: columnrenderer2,
                            cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                            }
                        }
                    ]
                });

                    //*****************************************************************************************************************


            var click = new Date();
            var lastClick = new Date();
            var lastRow = -1;
            $("#jGrid").bind('rowclick', function (event) {
                click = new Date();
                if (click - lastClick < 250) {
                    if (lastRow == event.args.rowindex) {

                        var dataRecord = $("#jGrid").jqxGrid('getrowdata', lastRow);

                        jGridRow = lastRow;
                        IDgrid = dataRecord.ID;
                        IDrow = lastRow;
                        $("#detalle2").dialog('open');

                        detalle();

                    }
                }
                lastClick = new Date();
                lastRow = event.args.rowindex;
            });



            /*
                var click = new Date();
                var lastClick = new Date();
                var lastRow = -1;
                $("#jGrid").bind('rowclick', function (event) {
                    click = new Date();
                    if (click - lastClick < 250) {
                        if (lastRow == event.args.rowindex) {

                            var datarow = $("#jGrid").jqxGrid('getrowdata', lastRow);


                            $("#txTienda").val(datarow.TIENDA);
                            $("#txNoCompra").val(datarow.NO_COMPRA);

                            $("#modificar").dialog({
                                buttons: {
                                    'Guardar': function () {

                                        var it1 = $.trim($("#txTienda").val());
                                        var it2 = $.trim($("#txNoCompra").val());

                                        var pm = new Array();

                                        if ($.trim(it1) == "") {
                                            pm.push("<strong>SERIE</strong>");
                                        }

                                        if ($.trim(it2) == "") {
                                            pm.push("<strong>FOLIO</strong>");
                                        }

                                        if (pm[0]) {
                                            var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                                            $("#message").html("<div style='text-align: left;min-width:250px;margin-top:8px;'>" + param + "</div>");
                                            $("#message").dialog('open');
                                            return false;
                                        }


                                        $('#init').removeClass('no-show');
                                        setTimeout(function() {

                                                var rs = getData("Pag1.aspx/Save",
                                                    {
                                                        id: datarow.ID,
                                                        tienda: it1,
                                                        compra: it2
                                                    });

                                                if (rs.code == 0) {
                                                    $("#jGrid").jqxGrid('setcellvalue', lastRow, "TIENDA", it1);
                                                    $("#jGrid").jqxGrid('setcellvalue', lastRow, "NO_COMPRA", it2);
                                                    $("#jGrid").jqxGrid('setcellvalue', lastRow, "FECHA_COMPRA", rs.msg);
                                                    $("#modificar").dialog("close");
                                                } else {
                                                    $("#message").html("<div style='text-align: left;min-width:250px;margin-top:8px;'>" + rs.msg + "</div>");
                                                    $("#message").dialog('open');
                                                }

                                                $('#init').addClass('no-show');
                                            }, 50);


                                    },
                                    'Cancelar': function () {

                                        $(this).dialog("close");
                                    }
                                }
                            });
                            $("#modificar").dialog('open');

                        }
                    }
                    lastClick = new Date();
                    lastRow = event.args.rowindex;
                });
                */

                //---------------------------------------------------------------------------


                $("#jGrid").bind('bindingcomplete', function () {
                    //$('#jGrid').jqxGrid('autoresizecolumns');

                    //console.debug(column / 2);

                    $("#jGrid .jqx-grid-column-header.jqx-grid-column-header-Theme2.jqx-widget-header.jqx-widget-header-Theme2 .iconscontainer").each(function () {
                        $(this).next().remove();
                    });

                });



                //---------------------------------------------------------------------------

                $('#jqxMenu').on('itemclick', function (event) {
                    var element = event.args.id;

                    if (element == "ctl00_Contraseña") {


                        $("#txPass1").val('');
                        $("#txPass2").val('');

                        $("#pass").dialog({
                            buttons: {
                                'Modificar': function () {

                                    var info = getData("Pag1.aspx/InfoPass",
                                            {
                                                pass1: $("#txPass1").val().trim()
                                            });


                                    if (info.f == "-1") {
                                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>La contraseña anterior es incorrecta.</div>");
                                        $("#message").dialog('open');
                                        return;
                                    }


                                    $("#confirmar")
                                            .html(
                                                "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres cambiar la contraseña?</div>");

                                    $("#confirmar").dialog({
                                        buttons: {
                                            'Aceptar': function () {

                                                $("#pass").dialog("close");
                                                $(this).dialog("close");


                                                var rs = getData("Pag1.aspx/NewPass",
                                                        {
                                                            pass1: $("#txPass2").val().trim()

                                                        });

                                                if (rs.code == 0) {
                                                    $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>La contraseña se cambio correctamente.</div>");
                                                    $("#message").dialog('open');

                                                } else {
                                                    $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" + rs.msg + "</div>");
                                                    $("#message").dialog('open');
                                                }

                                            },
                                            'Cancelar': function () {

                                                $(this).dialog("close");
                                            }
                                        }
                                    });

                                    $("#confirmar").dialog('open');


                                },
                                'Cancelar': function () {

                                    $(this).dialog("close");
                                }
                            }
                        });

                        $("#pass").dialog('open');

                    }
                });

                //---------------------------------------------------------------------------
                var fLimite = getData("Pag1.aspx/GetFechaLimite");

                $('#bCarga').click(function () {

                    if (fLimite.code == 0) {

                        if (fLimite.chk == 0) {

                            $("#inv").dialog('open');
                            $("#inv").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

                        } else {
                            $("#message").html('<div style="color:red; margin-top:10px; font-size:17px;">No es posible importar facturas, la fecha limite es: ' + fLimite.fecha  + '</div>');
                            $("#message").dialog('open');

                            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });
                        }
                    } else {
                        $("#message").html('<div style="color:red; margin-top:10px;">' + fLimite.msg + '</div>');
                        $("#message").dialog('open');

                        $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });
                    }

                });

                $('#bCarga2').click(function () {

                    if (fLimite.code == 0) {

                        if (fLimite.chk == 0) {


                            $("#inv2").dialog('open');
                            $("#inv2").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

                            //position('.mWin2');
                        } else {
                            $("#message").html('<div style="color:red; margin-top:10px; font-size:17px;">No es posible importar facturas, la fecha limite es: ' + fLimite.fecha + '</div>');
                            $("#message").dialog('open');

                            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });
                        }
                    } else {
                        $("#message").html('<div style="color:red; margin-top:10px;">' + fLimite.msg + '</div>');
                        $("#message").dialog('open');

                        $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });
                    }

                });

                //---------------------------------------------------------------------------
                $("#bExportar").click(function (event) {
                    $("#exportar").dialog('open');
                });

                $("#bBuscar").click(function (event) {

                    $('#init').removeClass('no-show');
                    setTimeout(function() {

                            /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                            var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

                            var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
                            var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');

                            var index = $("#cbEstatus").jqxDropDownList('selectedIndex');
                            var estatus = $("#cbEstatus").jqxDropDownList('getItem', index).value;

                            var folio = $("#txFolio").val();
                            var serie = $("#txSerie").val();
                            var contr = $("#txContr").val();

                            source.localdata = getData("Pag1.aspx/GetRegistros",
                                {
                                    req: {
                                        fecha1: fecha1,
                                        fecha2: fecha2,
                                        estatus: estatus,
                                        folio: folio,
                                        serie: serie,
                                        contr: contr,
                                        opt: opt
                                    }
                                });

                            dataAdapter.dataBind();
                            $("#jGrid").jqxGrid('clearselection');
                            $("#jGrid").jqxGrid('updatebounddata', 'cells');


                            //window.open("excel.aspx", "xml", "width=200,height=150");


                            $('#init').addClass('no-show');
                        },30);
                });

                //UploadRW();
                //---------------------------------------------------------------------------



                //**************************************************************************************************




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


            
            

                $("#confirmar").dialog({
                    autoOpen: false,
                    resizable: false,
                    //width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    open: function (event, ui) {

                    },
                    create: function (event, ui) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });



            $("#confirmar3").dialog({
                autoOpen: false,
                resizable: false,
                //width: 'auto',
                //minWidth: 200,
                //maxWidth: 500,
                fluid: true, //new option
                modal: true,
                dialogClass: 'mWin2',
                open: function (event, ui) {

                },
                create: function (event, ui) {
                    // Set maxWidth
                    //$(this).css("maxWidth", "700px");
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
                create: function (event, ui) {
                    // Set maxWidth
                    //$(this).css("maxWidth", "700px");
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

                $("#modificar").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin',
                    open: function (event, ui) {

                    },
                    create: function (event, ui) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });

                $("#cAgregar").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin',
                    open: function (event, ui) {

                    },
                    create: function (event, ui) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });


                $("#cModificar").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin',
                    open: function (event, ui) {

                    },
                    create: function (event, ui) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });






                $("#pass").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
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
                    create: function (event, ui) {
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

                            $('#init').removeClass('no-show');

                            var rowindexes = $('#jGrid').jqxGrid('getselectedrowindexes');

                            var select = new Array();
                            $.each(rowindexes, function (index, value) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);
                                select.push(data.ID);
                            });

                            if (!$.isEmptyObject(select)) {
                                getData("excel.aspx/SetID", {
                                    data: select,
                                    chkXML: chkXML,
                                    chkPDF: chkPDF
                                });

                                window.open("excel.aspx", "xml", "width=200,height=150");
                            }

                            $('#init').addClass('no-show');
                            $(this).dialog("close");
                        },
                        'Cerrar': function () {

                            $(this).dialog("close");
                        }
                    },
                    open: function (event, ui) {

                    }
                });



                $("#observacion1").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: '400',
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
                    create: function (event, ui) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
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
            //});
        </script>

        <%=Detalle2 %>
    
    
    <script>
        $('input[type="file"]').attr('title', window.webkitURL ? ' ' : '');

        $(".mWin2").css('z-index', 999);
        $(".mWin3").css('z-index', 999);
        $(".mWin").css('z-index', 999);

        $(".ui-dialog-content").css("padding-top", 0);
        $(".ui-dialog-content").css("padding-left", 6);
        $(".ui-dialog-content").css("padding-right", 6);
        $(".ui-dialog-content").css("padding-bottom", 5);
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

    <%--<div style="border: solid 1px black;padding: 0px;max-width: 99%;margin: auto" >--%>
        

        <table style="width: 99%; border: 1px #b4b2b2 solid; margin: auto;">
            
            
            

                    <tr>
                        <td style="text-align: left; padding: 5px; width: 160px;">
                            <div>Estatus</div>
                            <div class="jCb" id="cbEstatus" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 160px;">
                            <div>Serie</div>
                            <div><input class="jTx" id="txSerie" type="text" value="" style="width: 197px;" /></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 160px;">
                            <div>Folio</div>
                            <div><input class="jTx" id="txFolio" type="text" value="" style="width: 197px;" /></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 160px;">                        
                            <div>Fecha Inicial <span id="lbFecha1">Recepción</span></div>
                            <div id="jDate1" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Fecha Final <span id="lbFecha2">Recepción</span></div>
                            <div id="jDate2" style="width: 160px !important;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;" colspan="2">
                            <div>Razon Social</div>
                            <div class="txCenter" id="razonSocial" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"><%=getRazonSocial() %></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Contrarecibo</div>
                            <div><input class="jTx" id="txContr" type="text" value="" style="width: 197px;" /></div>
                        </td>
                        <td style="padding-top: 15px;" colspan="2">
                            <div id="jqxOpt1">Fecha Recepción</div>
                            <div id="jqxOpt2">Fecha Saldado</div>
                        </td>

                    </tr>

                    <tr>
                        <td colspan="5" style="padding: 5px;">
                            
                            
                            <table style="width: 100%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin: auto;background-color: #fbfbfb">
                                <tr>
                                    <td align="left">
                                        Total: <span id="total1"></span>
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
                                                    <input type="button" id="bBuscar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bCarga2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Importar ZIP"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bCarga" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Importar XML"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bExportar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding: 5px; padding-right: 8px;">
                            <div id="jGrid" style=""></div>
                        </td>
                    </tr>
            
                </table>

                
               

<%=VistaPrevia1 %>
<%=CargaXML %>
<%=CargaZIP %>


<div id="confirmar" title="Confirmar"></div>


<div id="confirmar3" title="Confirmar">
    <div style="margin-top:9px;">Quieres importar las facturas?</div>
</div>
                
                <div id="message" title="Mensaje del sistema"></div>
                <div id="detalle" title="Mensaje del sistema"><div id="jGrid2" style="margin-bottom: 0px;margin-top: 7px"></div></div>
                
                <div id="detalle2" title="Detalle">
                    <table style="margin-bottom: 0px;margin-top: 7px">
                        <tr>
                            <td>
                                <div id="jGrid3" style=""></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td><input type="button" id="bEliminar" class="btn1" style="height: 27px; padding-top: 3px; font-size: 1em;" value="Eliminar"/></td>
                                        <td><input type="button" id="bModificar" class="btn1" style="height: 27px; padding-top: 3px; font-size: 1em;" value="Modificar"/></td>
                                        <td><input type="button" id="bAgregar" class="btn1" style="height: 27px; padding-top: 3px; font-size: 1em;" value="Agregar"/></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    

                </div>

                

<!--------------------------------------------------------------------------------------->


<div id="observacion1" title="Observación" style="text-align: left; margin-top: 5px;">

</div>


<!--------------------------------------------------------------------------------------->


<div id="pass" title="Modificar Contraseña">
    <table style="margin-top: 15px;">
        <tr>
            <td style="width: 170px;" align="right">
                Contraseña Anterior:
            </td>
            <td>
                <input class="jTx" id="txPass1" type="text" value="" style="width: 200px; text-align: center" maxlength="100" />
            </td>
        </tr>
        <tr>
            <td align="right">
                Nueva Contraseña:
            </td>
            <td>
                <input class="jTx" id="txPass2" type="text" value="" style="width: 200px; text-align: center" maxlength="100" />
            </td>
        </tr>
    </table>
</div>
<!--------------------------------------------------------------------------------------->


                <div id="modificar" title="Modificar">
                    <table style="margin-top: 15px;">
                        <tr>
                            <td style="width: 93px;" align="right">
                                SERIE:
                            </td>
                            <td>
                                <input class="jTx" id="txTienda" type="text" value="" style="width: 120px; text-align: center" maxlength="5" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                FOLIO:
                            </td>
                            <td>
                                <input class="jTx" id="txNoCompra" type="text" value="" style="width: 120px; text-align: center" maxlength="10" />
                            </td>
                        </tr>
                    </table>
                </div>

<!--------------------------------------------------------------------------------------->

                <div id="cAgregar" title="Detalle">
                    <table style="margin-top: 15px;">
                        <tr>
                            <td style="width: 60px;" align="right">
                                SERIE:
                            </td>
                            <td>
                                <input class="jTx" id="cSerie1" type="text" value="" style="width: 150px; text-align: center" maxlength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                FOLIO:
                            </td>
                            <td>
                                <input class="jTx" id="cFolio1" type="text" value="" style="width: 150px; text-align: center" maxlength="20" />
                            </td>
                        </tr>
                    </table>
                </div>

<!--------------------------------------------------------------------------------------->

                <div id="cModificar" title="Detalle">
                    <table style="margin-top: 15px;">
                        <tr>
                            <td style="width: 60px;" align="right">
                                SERIE:
                            </td>
                            <td>
                                <input class="jTx" id="cSerie2" type="text" value="" style="width: 150px; text-align: center" maxlength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                FOLIO:
                            </td>
                            <td>
                                <input class="jTx" id="cFolio2" type="text" value="" style="width: 150px; text-align: center" maxlength="20" />
                            </td>
                        </tr>
                    </table>
                </div>

<!--------------------------------------------------------------------------------------->
                
                <div id="exportar" title="Exportar">
                    <div id='chkXML' style="margin-top: 20px">Agregar XML</div>
                    <div id='chkPDF'>Agregar PDF proveedor</div>
                </div>
                
                
                <% FormsIdentity id = (FormsIdentity) HttpContext.Current.User.Identity;%>
                <% if (User.IsInRole("superuser")){  %>
                       <%--<%:id.Ticket.UserData%>--%>
                <% } else{  %>
                        <%--<%:id.Ticket.UserData%>--%>
                <%   }  %>

        
    <%--</div>--%>
    <asp:Label ID="Script1" runat="server"></asp:Label>
</asp:Content>