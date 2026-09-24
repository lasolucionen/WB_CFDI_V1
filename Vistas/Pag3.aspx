<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag3.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            var source3 = {};
            var source4 = {};
            var source6 = {};
            var source7 = {};

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

                        return new $.jqx.dataAdapter(cSource,
                            {
                                autoBind: true,
                                loadComplete: function(data) {
                                    if (data.length > 10) {
                                        auto = false;
                                    }
                                }
                            });
                    }
                    return null;
                }

                /*---------------------------------------------*/

                $("#jqxOpt1").jqxRadioButton({ width: 120, height: 25, checked: true });
                $("#jqxOpt2").jqxRadioButton({ width: 120, height: 25 });

                var opt = 0;

                $("#jqxOpt1").on('change', function (event) {
                    var checked = event.args.checked;

                    if (checked) {
                        //$("#lbFecha1, #lbFecha2").html("de Creación");
                        opt = 0;
                    }
                });

                $("#jqxOpt2").on('change', function (event) {
                    var checked = event.args.checked;

                    if (checked) {
                        //$("#lbFecha1, #lbFecha2").html("Pago");
                        opt = 1;
                    }
                });


                //$(".jTx").jqxInput({ width: '100%', height: 25, theme: 'Theme2' });
                $(".jTx").jqxInput({height: 25, theme: 'Theme2' });
                $('#jqxTabs').jqxTabs({ height: 677, width: 734 });
                /*******************************************************************************/

                $("#jDate1").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate2").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                $("#jDate3").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate4").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });


                //$("#jDate5").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });
                $("#jDate6").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                $("#jDate7").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });
                $("#jDate8").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                $(".jCb").jqxDropDownList({ height: 25, /*width: '100%',*/searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'});

                $(".jCbX").jqxComboBox({ height: 25, /*width: '100%',*/searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'
                });

                $("#cbRFC").jqxComboBox({width: '174px'});
                $("#cbRFC3").jqxComboBox({width: '460px'});

                /*
                $("#cbBD").jqxDropDownList({ height: 25, width: '80px', searchMode: 'containsignorecase',
                    dropDownHeight: 200, autoDropDownHeight: true, promptText: '--------------------------',
                    source: ['BD_PSI', 'BD_PSO'],
                    theme: 'Theme2'
                });

                var bd = getData("Pag3.aspx/GetBD", {});
                $("#cbBD").jqxDropDownList('val', bd);

                $("#cbBD").bind('select', function (event) {
                    if (event.args && event.args.item) {
                        getData("Pag3.aspx/SetBD", { bd: event.args.item.value });

                        $("#razonSocial").html("");

                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('clearselection');
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('clearselection');
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');

                        source3.localdata = [];
                        dataAdapter3.dataBind();
                        $("#jGrid3").jqxGrid('clearselection');
                        $("#jGrid3").jqxGrid('updatebounddata', 'cells');

                        $("#cbRFC").jqxComboBox({
                            displayMember: 'RFC_EMISOR', valueMember: 'ID',
                            source: getAdapter("Pag3.aspx/GetRFCs",{}),
                            dropDownHeight: 200,
                            autoDropDownHeight: auto,
                            selectedIndex: -1
                        });

                        total2();
                        $(".rfc").html("");
                    }
                });
                */

                $('#init').removeClass('no-show');

                $("#cbEstatus").jqxDropDownList({
                    displayMember: 'ESTATUS', valueMember: 'ID',
                    source: getAdapter("Pag3.aspx/GetEstatus",{}),
                    selectedIndex: 1,
                    dropDownHeight: 200,
                    autoDropDownHeight: auto
                });


                var prov = getAdapter("Pag3.aspx/GetRFCs", {});
                var prov2 = $.extend(true, {}, prov);

                $("#cbRFC").jqxComboBox({
                    displayMember: 'RFC_EMISOR', valueMember: 'ID',
                    source: prov2,
                    dropDownHeight: 200, 
                    autoDropDownHeight: auto
                });

                $('#init').addClass('no-show');

                $("#cbRFC").bind('select', function (event) {
                    if (event.args && event.args.item) {
                        $(".rfc").html(event.args.item.label);
                       
                        var record = event.args.item.originalItem;
                        $("#razonSocial").html(record.RAZON_SOCIAL_EMISOR);
                        
                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');
                        
                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');
                        
                        source3.localdata = [];
                        dataAdapter3.dataBind();
                        $("#jGrid3").jqxGrid('updatebounddata', 'cells');
                    }
                });


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
                var rs = getData("Pag3.aspx/GetSemana");
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
                //
                
                $("#jGrid").on('rowselect', function (event) {
                    total1();
                });

                $("#jGrid").bind('rowunselect', function (event) {
                    total1();
                });

                $("#jGrid2").on('rowselect', function (event) {

                });

                $("#jGrid2").bind('rowunselect', function (event) {

                });
                function total1() {
                    var arr1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var importe = 0;
                    $.each(arr1,
                        function(index, value) {
                            var data = $('#jGrid').jqxGrid('getrowdata', value);
                            importe += data.TOTAL;
                        });

                    importe = numeral(importe).format('$0,0.00');
                    $("#total1").html(importe);
                }

                function total2() {

                    var rows = $("#jGrid2").jqxGrid('getrows');
                    var importe = 0;

                    $.each(rows,
                        function(index, value) {
                            importe += value.TOTAL;
                        });

                    importe = numeral(importe).format('$0,0.00');
                    $("#total2").html(importe);
                }
                

/*
                $("#theGrid").bind('rowselect', function (event) {
                    if (Array.isArray(event.args.rowindex)) {
                        if (event.args.rowindex.length > 0) {
                            alert("All rows selected");
                        } else {
                            alert("All rows unselected");
                        }
                    } else {
                        alert("Selected row has index = " + event.args.rowindex);
                    }
                });

                $("#theGrid").bind('rowunselect', function (event) {
                    alert("Unselected row has index = " + event.args.rowindex);
                });*/

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

                var IDgrid=-1;

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
                        { name: 'TIENDA' },
                        { name: 'NO_COMPRA' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'REVISADO' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'CONTRA_RECIBO' },
                        { name: 'ESTATUS_ID' }
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
                        //$("#jGrid").find('.jqx-grid-column-header:first').children().hide();

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
                        { text: 'CONTR',               dataField: 'CONTRA_RECIBO_ID',    width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL',               dataField: 'TOTAL',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',            dataField: 'SUBTOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',           dataField: 'IMPUESTOS',           width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',                 dataField: 'IVA',                 width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',                dataField: 'IEPS',                width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
/*                        {
                            text: 'Detalle',
                            datafield: 'Detalle',
                            columntype: 'button',
                            width: 45,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);

                                $("#detalle3").dialog({ width: Math.min(340, $(window).width()) });
                                $("#detalle3").dialog('open');

                                source6.localdata = getData("Pag3.aspx/GetDetalle", { id: dataRecord.ID });
                                dataAdapter6.dataBind();
                                $("#jGrid6").jqxGrid('ensurerowvisible', 0);
                                $("#jGrid6").jqxGrid('clearselection');

                                $('#init').addClass('no-show');

                            }
                        },*/
                        { text: 'SERIE',               dataField: 'SERIE',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA',              dataField: 'TIENDA',              width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: '#COMPRA',             dataField: 'NO_COMPRA',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA COMPRA',        dataField: 'FECHA_COMPRA',        width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'UUID',                dataField: 'UUID',                width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width: 130,  renderer: columnrenderer2, cellsalign: 'center' },
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
                        }
                        <% if (User.IsInRole("superuser")){  %>
/*                        ,
                        {
                            text: 'Editar',
                            datafield: 'Edit',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return '<div style="text-align: center;margin-top: 7px;font-weight: bold;font-size:12px"><a class="cEdit" href="#">Edit</a></div>';
                            }
                        }*/
                        <% }  %>
                        //{ text: ' ', minwidth: 0, width: 'auto', sortable: false }
                    ]
                });


//*****************************************************************************************************************
                source2 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        //{ name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        { name: 'TIENDA' },
                        { name: 'NO_COMPRA' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'REVISADO' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'CONTRA_RECIBO' },
                        { name: 'ESTATUS_ID' }
                    ],
                    localdata: []
                };
                var dataAdapter2 = new $.jqx.dataAdapter(source2);

                $("#jGrid2").jqxGrid({
                    width: '100%',
                    height: 480,
                    source: dataAdapter2,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'checkbox',
                    enablebrowserselection :true,
                    //selectionmode: 'checkbox',
                    //rowsheight: 36,
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
                        //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'TOTAL',               dataField: 'TOTAL',               width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',            dataField: 'SUBTOTAL',            width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',           dataField: 'IMPUESTOS',           width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',                 dataField: 'IVA',                 width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',                dataField: 'IEPS',                width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SERIE',               dataField: 'SERIE',               width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA',              dataField: 'TIENDA',              width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: '#COMPRA',             dataField: 'NO_COMPRA',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA COMPRA',        dataField: 'FECHA_COMPRA',        width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'UUID',                dataField: 'UUID',                width: 300, renderer: columnrenderer2, cellsalign: 'center' }

                    ]
                });

//*****************************************************************************************************************
                source3 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        { name: 'FECHA', type: 'date' }
                    ],
                    localdata: []
                };
                var dataAdapter3 = new $.jqx.dataAdapter(source3);

                $("#jGrid3").jqxGrid({ 
                    width: '100%',
                    height: 480,
                    source: dataAdapter3,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    //selectionmode: 'singlerow',
                    enablebrowserselection :true,
                    selectionmode: 'checkbox',
                    //rowsheight: 36,
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
                        { text: 'CONTRARECIBO', dataField: 'ID',    width: 102, renderer: columnrenderer2, cellsalign: 'center'},
                        { text: 'FECHA',        dataField: 'FECHA', width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        {
                            text: 'Facturas',
                            datafield: 'Show1',
                            columntype: 'button',
                            width: 60, 
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {
                               
                                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);
                                
                                $('#init').removeClass('no-show');


                                $("#detalle").dialog({
                                    buttons: {
                                        'Quitar': function() {



                                            var position = $("#jGrid4").jqxGrid('scrollposition');
                                            //var left = position.left;
                                            //var top = position.top;
                    
                                            // get the indexes of the selected rows.
                                            var selectedrowindexes = $("#jGrid4").jqxGrid('getselectedrowindexes');
                                            var rowscount = $("#jGrid4").jqxGrid('getdatainformation').rowscount;

                                            if (selectedrowindexes.length > 0) {

                                                $("#confirmar2").dialog({
                                                    buttons: {
                                                        'Quitar': function() {


                                                            var select1 = new Array();
                                                            var arrayOfSelectedIds = [];

                                                            // begin update. Stops the Grid's rendering.
                                                            $("#jGrid4").jqxGrid('beginupdate');
                                                            selectedrowindexes.sort();

                                                            // delete the selected rows by using the 'deleterow' method of jqxGrid.
                                                            for (var m = 0; m < selectedrowindexes.length; m++) {
                                                                var selectedrowindex =
                                                                    selectedrowindexes[selectedrowindexes.length - m - 1];
                                                                if (selectedrowindex >= 0 && selectedrowindex < rowscount) {

                                                                    var id = $("#jGrid4").jqxGrid('getrowid', selectedrowindex);
                                                                    arrayOfSelectedIds.push(id);

                                                                    var data = $('#jGrid4').jqxGrid('getrowdata', selectedrowindex);
                                                                    select1.push({ID: data.ID});
                                                                }
                                                            }
                                                            
                                                            
                                                            $('#init').removeClass('no-show');

                                                            var rs = getData("Pag3.aspx/Quitar",
                                                                {
                                                                    data: select1
                                                                });

                                                            if (rs.code == 0) {

                                                                $("#jGrid4").jqxGrid('deleterow', arrayOfSelectedIds);
                                                                buscar();

                                                            } else {
                                                                $("#message").html("<div style='margin-top:15px;'>" + rs.msg + "</div>");
                                                                $("#message").dialog('open');
                                                            }

                                                            // end update. Resume the Grid's rendering.
                                                            $("#jGrid4").jqxGrid('endupdate');

                                                            $("#jGrid4").jqxGrid('clearselection');
                                                            
                                                            $('#jGrid4').jqxGrid('scrolloffset', position.top, 0);

                                                            $("#jGrid4").jqxGrid("autoresizecolumns");

                                                            var colDefs = $("#jGrid4").jqxGrid('columns').records;
                                                            for ( var idx = 0; idx < colDefs.length; idx++) {

                                                                if (colDefs[idx].datafield != "_checkboxcolumn") {
                                                                    $("#jGrid4").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                                                }

                                                                if (colDefs[idx].datafield == "UUID") {
                                                                    $("#jGrid4").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                                                                }
                                                            }

                                                            $("#jGrid4").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                                                            $("#jGrid4").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                                                            $("#jGrid4").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                                                            $("#jGrid4").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);

                                                            $('#init').addClass('no-show');

                                                            $(this).dialog("close");
                                                        },
                                                        'Cancelar': function() {

                                                            $(this).dialog("close");
                                                        }
                                                    }
                                                });
                                                $("#confirmar2").dialog('open');

                                            }

                                        },
                                        'Cerrar': function () {
 
                                            $(this).dialog("close");
                                        }
                                    }});


                                source4.localdata = [];
                                dataAdapter4.dataBind();
                                $("#jGrid4").jqxGrid('updatebounddata', 'cells');

                                $("#detalle").dialog({ width: Math.min(950, $(window).width()) });
                                $("#detalle").dialog('open');
                                
                                
                                source4.localdata = getData("Pag3.aspx/Detalle", {id: dataRecord.ID});
                                dataAdapter4.dataBind();
                                $("#jGrid4").jqxGrid('ensurerowvisible', 0);
                                $("#jGrid4").jqxGrid('clearselection');

                                $("#jGrid4").jqxGrid("autoresizecolumns");

                                var colDefs = $("#jGrid4").jqxGrid('columns').records;
                                for ( var idx = 0; idx < colDefs.length; idx++) {

                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid4").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                    }

                                    if (colDefs[idx].datafield == "UUID") {
                                        $("#jGrid4").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                                    }
                                }
                                $("#jGrid4").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                                $("#jGrid4").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                                $("#jGrid4").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                                $("#jGrid4").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
                                
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
                                return "..";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);
                                
                                $('#init').removeClass('no-show');

                                getData("contrareciboPDF.aspx/SetID", {
                                    id: dataRecord.ID
                                });

                                window.open("contrareciboPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        },
                        {
                            text: 'PDF Carta Pago',
                            datafield: 'Show3',
                            columntype: 'button',
                            width: 100, 
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "..";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);
                                
                                $('#init').removeClass('no-show');

                                getData("cartaPagoPDF.aspx/SetID", {
                                    id: dataRecord.ID
                                });
                                
                                window.open("cartaPagoPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        }
                    ]
                });

//*****************************************************************************************************************
                source4 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        //{ name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        { name: 'TIENDA' },
                        { name: 'NO_COMPRA' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'IMPUESTOS' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'ESTATUS_ID' }
                    ],
                    localdata: []
                };
                var dataAdapter4 = new $.jqx.dataAdapter(source4);

                $("#jGrid4").jqxGrid({ 
                    width: '100%',
                    height: 450,
                    source: dataAdapter4,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    //selectionmode: 'singlerow',
                    selectionmode: 'checkbox',
                    enablebrowserselection :true,
                    //rowsheight: 36,
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
                        //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'CONTR',               dataField: 'CONTRA_RECIBO_ID',    width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL',               dataField: 'TOTAL',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',            dataField: 'SUBTOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',           dataField: 'IMPUESTOS',           width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',                 dataField: 'IVA',                 width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',                dataField: 'IEPS',                width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
/*                        {
                            text: 'Detalle',
                            datafield: 'Detalle',
                            columntype: 'button',
                            width: 45,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);

                                $("#detalle3").dialog({ width: Math.min(340, $(window).width()) });
                                $("#detalle3").dialog('open');

                                source6.localdata = getData("Pag3.aspx/GetDetalle", { id: dataRecord.ID });
                                dataAdapter6.dataBind();
                                $("#jGrid6").jqxGrid('ensurerowvisible', 0);
                                $("#jGrid6").jqxGrid('clearselection');

                                $('#init').addClass('no-show');

                            }
                        },*/
                        { text: 'SERIE',               dataField: 'SERIE',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA',              dataField: 'TIENDA',              width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: '#COMPRA',             dataField: 'NO_COMPRA',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA COMPRA',        dataField: 'FECHA_COMPRA',        width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'UUID',                dataField: 'UUID',                width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width: 130,  renderer: columnrenderer2, cellsalign: 'center' },
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

                                var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);
                                
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

                                var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);
                                
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

                                var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);
                                
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
                    ]
                });

//*****************************************************************************************************************
                source6 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'Impuesto' },
                        { name: 'TasaOCuota' },
                        { name: 'Importe' }
                    ],
                    localdata: []
                };
                var dataAdapter6 = new $.jqx.dataAdapter(source6);

                $("#jGrid6").jqxGrid({ 
                    width: '100%',
                    height: 300,
                    source: dataAdapter6,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
                    enablebrowserselection :true,
                    //selectionmode: 'checkbox',
                    //rowsheight: 36,
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
                        { text: 'IMPUESTO',     dataField: 'Impuesto',   width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'TASA O CUOTA', dataField: 'TasaOCuota', width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'IMPORTE',      dataField: 'Importe',    width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }

                    ]
                });



                //---------------------------------------------------------------------------


                $("#jGrid").bind('bindingcomplete', function () {
                    //$('#jGrid').jqxGrid('autoresizecolumns');

                    //console.debug(column / 2);

                    $("#jGrid .jqx-grid-column-header.jqx-grid-column-header-Theme2.jqx-widget-header.jqx-widget-header-Theme2 .iconscontainer").each(function () {
                        $(this).next().remove();
                    });

                });



                //---------------------------------------------------------------------------
                function numberAsc(a, b)
                {
                    return a - b;
                }

                function numberDesc(a, b)
                {
                    return b - a;
                }

                function move(arr, val) {
                    var j = 0;
                    for (var i = 0, l = arr.length; i < l; i++) {
                        if (arr[i] !== val) {
                            arr[j++] = arr[i];
                        }
                    }
                    arr.length = j;
                }
                //---------------------------------------------------------------------------


                $("#bAgregar").click(function (event) {
                    $("#bAgregar").button({ disabled: true });

                    var isOK = true;

                    var position = $("#jGrid").jqxGrid('scrollposition');
                    //var left = position.left;
                    //var top = position.top;
                    
                    // get the indexes of the selected rows.
                    var selectedrowindexes = $("#jGrid").jqxGrid('getselectedrowindexes');
                    var rowscount = $("#jGrid").jqxGrid('getdatainformation').rowscount;


                    try {

                        $.each(selectedrowindexes,
                            function(index, value) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);

                                if (data.CONTRA_RECIBO_ID != null) {
                                    isOK = false;
                                    throw new TypeError();
                                }
                            });

                    } catch (e) {
                    }


                    if (!isOK) {
                        $("#bAgregar").button({ disabled: false });

                        $("#message").html("<div style='margin-top:15px;'>No se puede agregar, alguna factura seleccionada ya tiene contrarecibo.</div>");
                        $("#message").dialog('open');
                        return;
                    }

                    
                    try {

                        $.each(selectedrowindexes,
                            function(index, value) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);

                                if (data.ESTATUS == "Rechazadas") {
                                    isOK = false;
                                    throw new TypeError();
                                }
                            });

                    } catch (e) {
                    }

                    if (!isOK) {
                        $("#bAgregar").button({ disabled: false });

                        $("#message").html("<div style='margin-top:15px;'>No se puede agregar, alguna factura seleccionada esta rechazada.</div>");
                        $("#message").dialog('open');
                        return;
                    }


                    var arrayOfSelectedIds = [];

                    // begin update. Stops the Grid's rendering.
                    $("#jGrid").jqxGrid('beginupdate');
                    $("#jGrid2").jqxGrid('beginupdate');

                    selectedrowindexes.sort().reverse();

                    // delete the selected rows by using the 'deleterow' method of jqxGrid.
                    for (var m = 0; m < selectedrowindexes.length; m++) {
                        var selectedrowindex = selectedrowindexes[selectedrowindexes.length - m - 1];
                        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {

                            var data = $('#jGrid').jqxGrid('getrowdata', selectedrowindex);
                            $("#jGrid2").jqxGrid('addrow', null, data);

                            var id = $("#jGrid").jqxGrid('getrowid', selectedrowindex);
                            arrayOfSelectedIds.push(id);
                            
                        }
                    }


                    $("#jGrid").jqxGrid('deleterow', arrayOfSelectedIds);
                    // end update. Resume the Grid's rendering.
                    $("#jGrid").jqxGrid('endupdate');
                    $("#jGrid2").jqxGrid('endupdate');

                    $("#jGrid").jqxGrid('clearselection');


                    var verticalScrollOffset = $("#jqxScrollAreaUpverticalScrollBar" + "jGrid2").height();
                    $('#jGrid2').jqxGrid('scrolloffset', verticalScrollOffset - 1, 0);
                    $('#jGrid2').jqxGrid('scrolloffset', verticalScrollOffset + 1, 0);
                    $('#jGrid').jqxGrid('scrolloffset', position.top, 0);

                    $("#jGrid2").jqxGrid("autoresizecolumns");
                    var colDefs = $("#jGrid2").jqxGrid('columns').records;
                    for ( var idx = 0; idx < colDefs.length; idx++) {

                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                            $("#jGrid2").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                        }

                        if (colDefs[idx].datafield == "UUID") {
                            $("#jGrid2").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                        }
                    }

                    total2();

                    $("#bAgregar").button({ disabled: false });

                    
                    return;



                    var select1 = new Array();
                    var selectedrowindexes = $('#jGrid').jqxGrid('getselectedrowindexes');

                    if (selectedrowindexes.length > 0) {

                        selectedrowindexes.sort(numberAsc);

                        var rows = $("#jGrid2").jqxGrid('getrows');

                        $("#jGrid2").jqxGrid('beginupdate');

                        $.each(selectedrowindexes,
                            function(index, value) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);

                                var add = true;
                                try {
                                    $.each(rows,
                                        function(index2, value2) {
                                            if (value2.ID == data.ID) {
                                                add = false;
                                                throw new TypeError();
                                            }
                                        });
                                } catch (e) {
                                }


                                if (add) {
                                    $("#jGrid2").jqxGrid('addrow', null, data);
                                    select1.push(value);
                                    //alert(value + " " + id);
                                }
                            });

                        $("#jGrid2").jqxGrid('endupdate');


                        select1.sort(numberDesc);
                        $("#jGrid").jqxGrid('beginupdate');
                        $.each(select1,
                            function(index, value) {
                                var id = $("#jGrid").jqxGrid('getrowid', value);
                                $("#jGrid").jqxGrid('deleterow', id);
                            });

                        //console.log(select1);

                        $("#jGrid").jqxGrid('endupdate');
                        $("#jGrid").jqxGrid('clearselection');
                        //$("#jGrid2").jqxGrid('clearselection');


                        

                        //source2.localdata = select1;
                        //dataAdapter2.dataBind();
                        //$("#jGrid2").jqxGrid('clearselection');
                        //$("#jGrid2").jqxGrid('updatebounddata', 'cells');

                        //$("#confirmar").dialog({ width: Math.min(800, $(window).width()) });
                        //$("#confirmar").dialog('open');
                    }

                    $("#bAgregar").button({ disabled: false });
                });


                $("#bQuitar").click(function (event) {

                    $("#bQuitar").button({ disabled: true });


                    var position = $("#jGrid2").jqxGrid('scrollposition');
                    //var left = position.left;
                    //var top = position.top;
                    
                    // get the indexes of the selected rows.
                    var selectedrowindexes = $("#jGrid2").jqxGrid('getselectedrowindexes');
                    var rowscount = $("#jGrid2").jqxGrid('getdatainformation').rowscount;

                    var arrayOfSelectedIds = [];

                    // begin update. Stops the Grid's rendering.
                    $("#jGrid2").jqxGrid('beginupdate');
                    selectedrowindexes.sort();

                    // delete the selected rows by using the 'deleterow' method of jqxGrid.
                    for (var m = 0; m < selectedrowindexes.length; m++) {
                        var selectedrowindex = selectedrowindexes[selectedrowindexes.length - m - 1];
                        if (selectedrowindex >= 0 && selectedrowindex < rowscount) {

                            var id = $("#jGrid2").jqxGrid('getrowid', selectedrowindex);
                            arrayOfSelectedIds.push(id);

                        }
                    }

                    $("#jGrid2").jqxGrid('deleterow', arrayOfSelectedIds);
                    // end update. Resume the Grid's rendering.
                    $("#jGrid2").jqxGrid('endupdate');

                    $("#jGrid2").jqxGrid('clearselection');
                    

                    $('#jGrid2').jqxGrid('scrolloffset', position.top, 0);

                    $("#jGrid2").jqxGrid("autoresizecolumns");
                    var colDefs = $("#jGrid2").jqxGrid('columns').records;
                    for ( var idx = 0; idx < colDefs.length; idx++) {

                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                            $("#jGrid2").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                        }

                        if (colDefs[idx].datafield == "UUID") {
                            $("#jGrid2").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                        }
                    }

                    total2();

                    $("#bQuitar").button({ disabled: false });

                    if (arrayOfSelectedIds.length > 0) {
                        buscar();
                    }

                });

                $("#bCrear").click(function (event) {

                    //$("#bCrear").button({ disabled: true });

                    var select1 = new Array();

                    var rows = $("#jGrid2").jqxGrid('getrows');
                    $.each(rows,
                        function(index, value) {
                            select1.push({
                                ID: value.ID,
                                CONTRA_RECIBO_ID: value.CONTRA_RECIBO_ID,
                                RFC_EMISOR: $(".rfc").eq(0).html(),
                                ESTATUS2: value.ESTATUS
                            });
                        });


                    if (rows.length > 0) {
                        $("#confirmar").dialog({
                            buttons: {
                                'Crear': function() {

                                    //var fecha1 = $("#jDate5").jqxDateTimeInput('getDate');
                                    var fecha2 = $("#jDate6").jqxDateTimeInput('getDate');

                                    var rs = getData("Pag3.aspx/Crear",
                                        {
                                            data: select1,
                                            //fecha1: fecha1,
                                            fecha2: fecha2,
                                            observacion: $("#txObservacion").val()
                                        });

                                    if (rs.code == 0) {

                                        $("#txObservacion").val('');

                                        source2.localdata = [];
                                        dataAdapter2.dataBind();
                                        $("#jGrid2").jqxGrid('clearselection');
                                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');

                                        total2();
                                        buscar();

                                        $("#message").html("<div style='margin-top:15px;'>El contrarecibo <span style='color:red'>" + rs.contr + "</span> se creo correctamente.</div>");
                                        $("#message").dialog('open');

                                        window.open("contrareciboPDF.aspx", "xml", "width=200,height=150");
                                    } else {
                                        $("#message").html("<div style='margin-top:15px;'>" + rs.msg + "</div>");
                                        $("#message").dialog('open');
                                    }

                                    $(this).dialog("close");
                                },
                                'Cancelar': function() {

                                    $(this).dialog("close");
                                }
                            }
                        });

                        $("#confirmar").dialog('open');
                    } else {
                        
                        $("#message").html("<div style='margin-top:15px;'>Agregue las facturas</div>");
                        $("#message").dialog('open');
                    }
                    //$("#bCrear").button({ disabled: false });


                });

                function buscar()
                {
                    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

                    var fecha1 = $("#jDate1").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate2").jqxDateTimeInput('getDate');

                    var index1  = $("#cbEstatus").jqxDropDownList('selectedIndex');
                    var estatus = $("#cbEstatus").jqxDropDownList('getItem', index1).value;

                    var index2  = $("#cbRFC").jqxComboBox('selectedIndex');
                    if (index2 == -1) {
                        
                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
                        $("#message").dialog('open');
                        return;
                    }

                    var id      = $("#cbRFC").jqxComboBox('getItem', index2).value;
                    var rfc     = $("#cbRFC").jqxComboBox('getItem', index2).label;

                    if (id == 0) {
                        rfc = null;
                    }

                    var folio = $("#txFolio").val();
                    var serie = $("#txSerie").val();
                    var contr = $("#txContr").val();

                    $('#init').removeClass('no-show');
                    setTimeout(function () {

                        var data = getData("Pag3.aspx/GetRegistros",
                            {
                            req: {
                                    fecha1:     fecha1,
                                    fecha2:     fecha2,
                                    estatus:    estatus,
                                    RFC_EMISOR: rfc,
                                    folio:folio,
                                    serie:serie,
                                    contr:contr
                                }
                            });

                        

                        /*if (!$.isEmptyObject(data)) {
                            $("#razonSocial").html(data[0].RAZON_SOCIAL_EMISOR);
                        } else {
                            $("#razonSocial").html("");
                        }*/

                        var rows = $("#jGrid2").jqxGrid('getrows');

                        var edata = [];
                        $.each(data, function( index, value ) {
                            
                            var add = true;
                            try {
                                $.each(rows, function( index2, value2 ) {
                                    if (value.ID == value2.ID) {
                                        add = false;
                                        throw new TypeError();
                                    }
                                });
                            } catch (e) {
                            }

                            if (add) {
                                edata.push(value);
                            }
                        });

                        /*
                        var rows = $("#jGrid2").jqxGrid('getrows');


                        $.each(data, function( index, value ) {
                            
                            var add = true;
                            try {
                                $.each(rows, function( index2, value2 ) {
                                    if (value.ID == value2.ID) {
                                        add = false;

                                        data = $.grep(data,
                                            function(element, index) { return element.ID == value2.ID },
                                            true);

                                        throw new TypeError();
                                    }
                                });
                            } catch (e) {
                            }
                            
                        });*/

                        source.localdata = edata;
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('clearselection');
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        $("#jGrid").jqxGrid("autoresizecolumns");

                        var colDefs = $("#jGrid").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {

                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                $("#jGrid").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                            }

                            if (colDefs[idx].datafield == "UUID") {
                                $("#jGrid").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                            }

                        }
                        $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                        $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                        $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 70);

                        /*source3.localdata = data;
                        dataAdapter3.dataBind();*/

                        $('#init').addClass('no-show');
                    }, 10);
                    

                    //window.open("excel.aspx", "xml", "width=200,height=150");
                }

                $("#bBuscar").click(function (event) {
                    buscar();
                });

                function buscar2() {
                    if ($(".rfc").html() == "") {
                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
                        $("#message").dialog('open');
                        return;
                    }


                    $('#init').removeClass('no-show');
                    setTimeout(function () {

                        var fecha1 = $("#jDate3").jqxDateTimeInput('getDate');
                        var fecha2 = $("#jDate4").jqxDateTimeInput('getDate');

                        source3.localdata = getData("Pag3.aspx/GetContr",{ 
                            rfc: $(".rfc").html(),
                            fecha1: fecha1,
                            fecha2: fecha2,
                            Contr: $("#txContr2").val()
                        });


                        dataAdapter3.dataBind();
                        $("#jGrid3").jqxGrid('clearselection');
                        $("#jGrid3").jqxGrid('updatebounddata', 'cells');
                        $("#jGrid3").jqxGrid('ensurerowvisible', 0);

                        $('#init').addClass('no-show');

                    }, 10);
                }

                $("#bBuscar2").click(function (event) {
                    buscar2();
                });


                $("#bExportar").click(function (event) {
                    $('#init').removeClass('no-show');

                    var fecha1 = $("#jDate7").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate8").jqxDateTimeInput('getDate');

                    getData("excelContr.aspx/SetFiltr", {
                        req: {
                            fecha1: fecha1,
                            fecha2: fecha2,
                            opt:opt
                        }
                    });
                    
                    window.open("excelContr.aspx", "xml", "width=200,height=150");


                    $('#init').addClass('no-show');
                    
                });

                //---------------------------------------------------------------------------
                $("#bExportar2").click(function (event) {
                    $("#exportar").dialog('open');
                });

                //UploadRW();
                //---------------------------------------------------------------------------


                $("#Rep1").click(function (event) {
                    $('#init').removeClass('no-show');

                    
                    var fecha1 = $("#jDate7").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate8").jqxDateTimeInput('getDate');

                    getData("reportes.aspx/SetID", {
                        req: {
                            rep: 1,
                            fecha1: fecha1,
                            fecha2: fecha2
                        }
                    });
                    
                    window.open("reportes.aspx", "xml", "width=200,height=150");


                    $('#init').addClass('no-show');
                    
                });

                $("#Rep2").click(function(event) {
                    $('#init').removeClass('no-show');

                    var fecha1 = $("#jDate7").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate8").jqxDateTimeInput('getDate');

                    getData("reportes.aspx/SetID",
                        {
                            req: {
                                rep: 2,
                                fecha1: fecha1,
                                fecha2: fecha2
                            }
                        });
                    
                    window.open("reportes.aspx", "xml", "width=200,height=150");


                    $('#init').addClass('no-show');
                    
                });

                $("#Rep3").click(function(event) {
                    $('#init').removeClass('no-show');

                    var fecha1 = $("#jDate7").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate8").jqxDateTimeInput('getDate');

                    getData("reportes.aspx/SetID",
                        {
                            req: {
                                rep: 3,
                                fecha1: fecha1,
                                fecha2: fecha2
                            }
                        });
                    
                    window.open("reportes.aspx", "xml", "width=200,height=150");


                    $('#init').addClass('no-show');
                    
                });

                $("#Rep4").click(function(event) {
                    $('#init').removeClass('no-show');

                    var fecha1 = $("#jDate7").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate8").jqxDateTimeInput('getDate');

                    getData("reportes.aspx/SetID",
                        {
                            req: {
                                rep: 4,
                                fecha1: fecha1,
                                fecha2: fecha2
                            }
                        });
                    
                    window.open("reportes.aspx", "xml", "width=200,height=150");


                    $('#init').addClass('no-show');
                    
                });


                $("#Proceso").click(function (event) {
                    $('#init').removeClass('no-show');

                    var rs=getData("Pag3.aspx/ProcesoPago", {});


                    if (rs.code == 0) {

                    }

                    $("#message")
                        .html(
                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                            rs.msg +
                            "</div>");

                    $("#message").dialog('open');
                    position('.mWin');
                    

                    $('#init').addClass('no-show');
                    
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
                            $.each(rowindexes, function( index, value ) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);
                                select.push(data.ID);
                            });

                            if (!$.isEmptyObject(select)) {
                                getData("excel2.aspx/SetID", {
                                    data:select,
                                    chkXML:chkXML,
                                    chkPDF:chkPDF
                                });

                                window.open("excel2.aspx", "xml", "width=200,height=150");
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



                $("#detalle").dialog({
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
                    create: function( event, ui ) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });


                $("#detalle3").dialog({
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
                    create: function( event, ui ) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });

                $("#confirmar2").dialog({
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
                    create: function( event, ui ) {
                        // Set maxWidth
                        //$(this).css("maxWidth", "700px");
                    }
                });



                $('input[type="file"]').attr('title', window.webkitURL ? ' ' : '');

                
                $(".mWin2").css('z-index', 999);
                $(".mWin3").css('z-index', 999);
                $(".mWin").css('z-index', 999);

                $(".ui-dialog-content").css("padding-top", 0);
                $(".ui-dialog-content").css("padding-left", 6);
                $(".ui-dialog-content").css("padding-right", 6);
                $(".ui-dialog-content").css("padding-bottom", 5);


                /************************/
                //$("#cbRFC").jqxComboBox({selectedIndex: 0});
                //$("#jDate1").jqxDateTimeInput({value:new Date(2010, 0, 1) ,height: '25px', theme: 'Theme2' ,culture: 'es'});
                //buscar();
                /************************/
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

    <%--<div style="border: solid 1px black;padding: 0px;max-width: 99%;margin: auto" >--%>
    <table style="width: 99%; border: 1px #b4b2b2 solid; margin: auto;">
        <tr>
            <td style="border:1px #b4b2b2 solid" valign="top">
                

                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left; padding: 0px; padding-left: 5px; width: 160px;">
                            <table>
                                <tr>
                                    <td style="padding-top: 20px;">
                                        <input type="button" id="bProv" class="btn1" style="height: 28px; padding-top: 0px; font-size: 0.5em;" value=".."/>
                                    </td>
                                    <td>
                                        <div>RFC Emisor</div>
                                        <div class="jCbX" id="cbRFC" style="width: 160px !important;"></div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 160px;">
                            <div>Estatus</div>
                            <div class="jCb" id="cbEstatus" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Serie</div>
                            <div><input class="jTx" id="txSerie" type="text" value="" style="width: 197px;" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;">
                            <div>Fecha Inicial Recepción</div>
                            <div id="jDate1" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Fecha Final Recepción</div>
                            <div id="jDate2" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Folio</div>
                            <div><input class="jTx" id="txFolio" type="text" value="" style="width: 197px;" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;" colspan="2">
                             <div>Razon Social</div>
                             <div class="txCenter" id="razonSocial" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Contrarecibo</div>
                            <div><input class="jTx" id="txContr" type="text" value="" style="width: 197px;" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding: 5px;">
                            
                            
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
                                                    <input type="button" id="bExportar2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bAgregar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Agregar"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding: 5px; padding-right: 8px;">
                            <div id="jGrid" style=""></div>
                        </td>
                    </tr>
                </table>
                

                
                
            </td>
            <td style="width: 735px;" valign="top">

                <div id='jqxTabs'>
                    <ul style='margin-left: 30px;'>
                        <li>Crear</li>
                        <li>Mostrar</li>
                        <li>Reporte</li>
                    </ul>
                    <div>
                        
                        
                        <table style="width: 100%">

                            <tr>
                                <td style="text-align: left; padding: 5px; width: 200px;">
                                    <div>RFC</div>
                                    <div class="txCenter rfc" id="" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 200px !important;"></div>
                                    <%--<div><input class="jTx" id="txRFC" type="text" value="" style="width: 197px;"/></div>--%>
                                </td>
                                <td style="text-align: left; padding: 5px;">
                                    <div>Fecha Pago</div>
                                    <div id="jDate6" style="width: 160px !important;"></div>
                                </td>
                            </tr>
                            <tr>
                                
                                <!--
                                <td style="text-align: left; padding: 5px;">
                                    <div>Fecha Recepción</div>
                                    <div id="jDate5" style="width: 160px !important;"></div>
                                </td>-->
                                <td style="text-align: left; padding: 5px;" colspan="2">
                                    <div>Observación</div>
                                    <div><input class="jTx" id="txObservacion" type="text" maxlength="65" value="" style="width: 407px;" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">

                                    <table style="width: 100%; text-align: right; border: solid 1px #e4e4e4; border-bottom: solid 1px #e4e4e4; margin: auto; background-color: #fbfbfb">
                                        <tr>
                                            <td align="left">
                                                Total: <span id="total2">$0.00</span>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="bCrear" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Crear contra recibo"/>
                                                        </td>
                                                        <td>
                                                            <input type="button" id="bQuitar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Quitar"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px; padding-right: 8px;">
                                    <div id="jGrid2" style="margin-bottom: 0px"></div>
                                </td>
                            </tr>
                        </table>


                    </div>
                    <div>

                        <table style="width: 100%">

                            <tr>
                                <td style="text-align: left; padding: 5px; width: 200px;">
                                    <div>RFC</div>
                                    <div class="txCenter rfc" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 200px !important;"></div>
                                    <%--<div><input class="jTx" id="txRFC" type="text" value="" style="width: 197px;"/></div>--%>
                                </td>
                                <td style="text-align: left; padding: 5px;">
                                    <div>Contrarecibo</div>
                                    <div><input class="jTx" id="txContr2" type="text" value="" style="width: 197px;" /></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; padding: 5px;">
                                    <div>Fecha Inicial</div>
                                    <div id="jDate3"></div>
                                </td>
                                <td style="text-align: left; padding: 5px;">
                                    <div>Fecha Final</div>
                                    <div id="jDate4"></div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">

                                    <table style="width: 100%; text-align: right; border: solid 1px #e4e4e4; border-bottom: solid 1px #e4e4e4; margin: auto; background-color: #fbfbfb">
                                        <tr>
                                            <td align="left">
                                                
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <!--
                                                        <td>
                                                            <input type="button" id="bExportar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                                        </td>-->
                                                        <td>
                                                            <input type="button" id="bBuscar2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px; padding-right: 8px;">
                                    <div id="jGrid3" style="margin-bottom: 0px"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        
                        <table style="width: 100%">
                        
                            <tr>
                                <td style="text-align: left; padding: 5px;">
                                    <div>Fecha Inicial <span id="lbFecha1"></span></div>
                                    <div id="jDate7"></div>
                                </td>
                                <td style="text-align: left; padding: 5px;">
                                    <div>Fecha Final <span id="lbFecha2"></span></div>
                                    <div id="jDate8"></div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">

                                    <table style="width: 100%; text-align: right; border: solid 1px #e4e4e4; border-bottom: solid 1px #e4e4e4; margin: auto; background-color: #fbfbfb">
                                        <tr>
                                            <td align="left">
                                                
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; padding: 5px; width: 200px;padding-bottom: 0px;">
                                    <div id="jqxOpt1">Fecha Recepción</div>
                                    <div id="jqxOpt2">Fecha Pago</div>
                                </td>
                                <td style="text-align: left; padding: 5px;">
                                    <input type="button" id="bExportar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">

                                    <table style="width: 100%; text-align: right; border: solid 1px #e4e4e4; border-bottom: solid 1px #e4e4e4; margin: auto; background-color: #fbfbfb">
                                        <tr>
                                            <td align="left">
                                                
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">

                                    <input type="button" id="Rep1" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Reporte 1"/>
                                    <input type="button" id="Rep2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Reporte 2"/>
                                    <input type="button" id="Rep3" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Reporte 3"/>
                                    <input type="button" id="Proceso" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Proceso Pago"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">
                                    <input type="button" id="Rep4" class="btn1" style="width: 192px; height: 30px; padding-top: 5px; font-size: 1em;" value="Facturas Modificadas"/>
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                </div>


            </td>
        </tr>

    </table>

                <div id="message" title="Mensaje del sistema"></div>
                <div id="confirmar" title="Confirmar">
                    <div style="margin-top:9px;">Quieres crear el contrarecibo de las facturas agregadas?</div>
                </div>
                
                <div id="confirmar2" title="Confirmar">
                    <div style="margin-top:9px;">Quieres quitar las facturas seleccionadas del contrarecibo?</div>
                </div>
                
                <div id="detalle" title="Detalle">
                    <div id="jGrid4" style="margin-top: 10px;"></div>
                </div>

                <div id="detalle3" title="Detalle">
                    <div id="jGrid6" style="margin-top: 10px"></div>
                </div>

                <div id="exportar" title="Exportar">
                    <div id='chkXML' style="margin-top: 20px">Agregar XML</div>
                    <div id='chkPDF'>Agregar PDF proveedor</div>
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
        
        
    <%--</div>--%>
    <asp:Label ID="Script1" runat="server"></asp:Label>
</asp:Content>