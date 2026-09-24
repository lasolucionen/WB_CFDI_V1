<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag2.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag2" %>

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
            var source5 = {};
            var source6 = {};
            var source7 = {};
            var source8 = {};
            var source9 = {};

            $(document).ready(function () {
                //$.jqx.theme = "bootstrap";
                //$.jqx.theme = "customx";
                /*******************************************************************************/

/*                var v1 = 450.60;
                var v2 = v1 + 0.5;
                var v3 = Math.abs(v1-v2);
                if (v3 >= 0 && v3 <= 0.5) {
                    alert("SI: " + v3);
                } else {
                    alert("NO: " + v3);
                }*/



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
                        },
                        beforeSend: function() {
                            // setting a timeout

                        }
                    });

                    

/*                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", cUrl, false);
                    xhr.setRequestHeader('Content-type','application/json; charset=utf-8');
                    xhr.onload = function () {
                        

                        if (xhr.status === 200 ) {
                            result = JSON.parse(xhr.responseText);

                            try {
                                result = JSON.parse(result.d);
                            } catch (err) {

                            }
                        }
                        else if (xhr.status === 401) {
                            window.location.href = "Login.aspx";
                        }


                    }
                    xhr.send(JSON.stringify(cData));*/


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


                //$(".jTx").jqxInput({ width: '100%', height: 25, theme: 'Theme2' });
                $(".jTx").jqxInput({height: 25, theme: 'Theme2' });
                /*******************************************************************************/

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
                        $("#lbFecha1, #lbFecha2").html("Proceso");
                        opt = 1;
                    }
                });

                /*******************************************************************************/

                $("#jDate1").jqxDateTimeInput({value:'<%=getFecha1() %>' ,width: '150px', height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate2").jqxDateTimeInput({ width: '150px', height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                $("#jDate3").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate4").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                $("#jDate5").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate6").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });


                <% if( User.IsInRole("5") )
                   { %>
                $("#jDate7").jqxDateTimeInput({ width: '110px', height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });
                <% } %>

                $(".jCb").jqxDropDownList({ height: 25, /*width: '100%',*/searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'});

                $(".jCbX").jqxComboBox({ height: 25, /*width: '100%',*/searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'
                });

                $("#cbRFC").jqxComboBox({width: '190px'});


                /*
                $("#cbBD").jqxDropDownList({ height: 25, width: '80px', searchMode: 'containsignorecase',
                    dropDownHeight: 200, autoDropDownHeight: true, promptText: '--------------------------',
                    source: ['BD_PSI', 'BD_PSO'],
                    theme: 'Theme2'
                });

                var bd = getData("Pag2.aspx/GetBD", {});
                $("#cbBD").jqxDropDownList('val', bd);

                $("#cbBD").bind('select', function (event) {
                    if (event.args && event.args.item) {
                        getData("Pag2.aspx/SetBD", { bd: event.args.item.value });

                        $("#razonSocial, #razonSocial2").html("");

                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('clearselection');
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('clearselection');
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');

                        source8.localdata = [];
                        dataAdapter8.dataBind();
                        $("#jGrid8").jqxGrid('clearselection');
                        $("#jGrid8").jqxGrid('updatebounddata', 'cells');

                        $("#cbRFC").jqxComboBox({
                            displayMember: 'RFC_EMISOR', valueMember: 'ID',
                            source: getAdapter("Pag3.aspx/GetRFCs",{}),
                            dropDownHeight: 200,
                            autoDropDownHeight: auto,
                            selectedIndex: -1
                        });


                        $("#rfc, #rfc2").html("");
                    }
                });
                */



                $('#init').removeClass('no-show');

                $("#cbEstatus, #cbEstatus2").jqxDropDownList({
                    displayMember: 'ESTATUS', valueMember: 'ID',
                    source: getAdapter("Pag2.aspx/GetEstatus",{}),
                    selectedIndex: 1,
                    dropDownHeight: 200, 
                    autoDropDownHeight: auto
                });

                var prov = getAdapter("Pag2.aspx/GetRFCs", {});
                var prov2 = $.extend(true, {}, prov);

                $("#cbRFC").jqxComboBox({
                    displayMember: 'RFC_EMISOR', valueMember: 'ID',
                    source: prov2,
                    dropDownHeight: 200, 
                    autoDropDownHeight: auto
                });

                $("#cbSerie").jqxDropDownList({
                    displayMember: 'DESCRIPCION', valueMember: 'SERIE',
                    source: getAdapter("Pag2.aspx/GetSeries",{}),
                    dropDownHeight: 200, 
                    autoDropDownHeight: auto
                });

                $('#init').addClass('no-show');


                $("#cbRFC").bind('select', function (event) {
                    if (event.args && event.args.item) {
                       $("#rfc, #rfc2").html(event.args.item.label);
                        
                        var record = event.args.item.originalItem;
                        $("#razonSocial, #razonSocial2").html(record.RAZON_SOCIAL_EMISOR);
                         
                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');

                        source8.localdata = [];
                        dataAdapter8.dataBind();
                        $("#jGrid8").jqxGrid('updatebounddata', 'cells');
                    }
                });



                $("#cbEstatus").bind('select', function (event) {
                    if (event.args && event.args.item) {

                        if (event.args.item.value == 3) {
                            $("#bAccion").button("disable");
                        } else {
                            $("#bAccion").button("enable");
                        }

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
                var rs = getData("Pag2.aspx/GetSemana");
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
                    $("#cfile1").change(function(e) {
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
                    $("#cfile2").change(function(e) {
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
                    $("#cfile3").change(function(e) {
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
                //
                
                $("#jGrid").on('rowselect', function (event) {
                    var rowindex = event.args.rowindex;
                    var arr1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var arr2 = $('#jGrid2').jqxGrid('getselectedrowindexes');

                    if (Array.isArray(rowindex)) {
                        if (rowindex.length > 0) {
                            if (arr1.length > 1 && arr2.length > 1) {

                                $("#jGrid").jqxGrid('clearselection');
                                $('#jGrid').jqxGrid('updatebounddata');
                            } else {
                                total1();
                            }
                        } else {
                            total1();
                        }
                    } else {

                        if (arr1.length > 1 && arr2.length > 1) {

                            var prev = arr1[0];
                            $("#jGrid").jqxGrid('clearselection');
                            $('#jGrid').jqxGrid('selectrow', prev);
                            //$('#jGrid2').jqxGrid('unselectrow', row);
                            //$('#jGrid').jqxGrid('selectrow', row);
                        } else {
                            total1();
                        }
                    }
                });

                $("#jGrid").bind('rowunselect', function (event) {
                    total1();
                });

                $("#jGrid2").on('rowselect', function (event) {
                    var rowindex = event.args.rowindex;
                    var arr1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var arr2 = $('#jGrid2').jqxGrid('getselectedrowindexes');

                    if (Array.isArray(rowindex)) {
                        if (rowindex.length > 0) {
                            if (arr1.length > 1 && arr2.length > 1) {

                                $("#jGrid2").jqxGrid('clearselection');
                                $('#jGrid2').jqxGrid('updatebounddata');
                            } else {
                                total2();
                            }
                        } else {
                            total2();
                        }
                    } else {

                        if (arr1.length > 1 && arr2.length > 1) {

                            var prev = arr2[0];
                            $("#jGrid2").jqxGrid('clearselection');
                            $('#jGrid2').jqxGrid('selectrow', prev);
                            //$('#jGrid2').jqxGrid('unselectrow', row);
                            //$('#jGrid').jqxGrid('selectrow', row);
                        } else {

                            total2();
                        }
                    }

                });

                $("#jGrid2").bind('rowunselect', function (event) {
                    total2();
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
                    var arr2 = $('#jGrid2').jqxGrid('getselectedrowindexes');
                    var importe = 0;
                    $.each(arr2,
                        function(index, value) {
                            var data = $('#jGrid2').jqxGrid('getrowdata', value);
                            importe += data.TOTAL;
                        });

                    importe = numeral(importe).format('$0,0.00');
                    $("#total2").html(importe);
                }

                function total3() {
                    var arr1 = $('#jGrid8').jqxGrid('getselectedrowindexes');
                    var importe = 0;
                    $.each(arr1,
                        function(index, value) {
                            var data = $('#jGrid8').jqxGrid('getrowdata', value);
                            importe += data.TOTAL;
                        });

                    importe = numeral(importe).format('$0,0.00');
                    $("#total3").html(importe);
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

                var IDgrid = -1;

                source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        { name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        //{ name: 'REVISADO' },
                        { name: 'TIENDA' },
                        { name: 'NO_COMPRA' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'FECHA_PROCESO', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'ESTATUS_ID' }
                    ],
                    localdata: []
                };
                var dataAdapter = new $.jqx.dataAdapter(source);

                $("#jGrid").jqxGrid({ width: '100%', height: 456,
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

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $("#cSubTotal2").html( numeral(dataRecord.SUBTOTAL).format('$0,0.00') );
                                $("#cTotal2").html( numeral(dataRecord.TOTAL).format('$0,0.00') );
                                $("#cIVA2").html( numeral(dataRecord.IVA).format('$0,0.00') );
                                $("#cIEPS2").html( numeral(dataRecord.IEPS).format('$0,0.00') );

                                $('#init').removeClass('no-show');
                                setTimeout(function() {

                                        //alert(dataRecord.ID);

                                        //$("#detalle3").dialog({ width: Math.min(340, $(window).width()) });

                                        var rs = getData("Pag2.aspx/GetDetalle", { id: dataRecord.ID });

                                        source6.localdata = rs.DATA;
                                        dataAdapter6.dataBind();
                                        $("#jGrid6").jqxGrid('ensurerowvisible', 0);
                                        $("#jGrid6").jqxGrid('clearselection');


                                        $("#jGrid6").jqxGrid("autoresizecolumns");

                                        var colDefs = $("#jGrid6").jqxGrid('columns').records;
                                        for (var idx = 0; idx < colDefs.length; idx++) {
                                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                                $("#jGrid6").jqxGrid('setcolumnproperty',
                                                    colDefs[idx].datafield,
                                                    'width',
                                                    colDefs[idx].width + 5);
                                            }
                                        }

                                        $("#cRFiscal").html(rs.RFiscal);
                                        $("#cUCfdi").html(rs.UsoCFDI);
                                        $("#cMPago").html(rs.MetodoPago);
                                        $("#cFPago").html(rs.FormaPago);
                                        $("#cMoneda").html(rs.Moneda);


                                       /* if (!rs.Revisado) {
                                            $("#detalle3").dialog({
                                                buttons: {
                                                    'Validar Factura': function() {

                                                        $("#confirmar")
                                                            .html(
                                                                "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres validar la factura?</div>");

                                                        $("#confirmar").dialog({
                                                            buttons: {
                                                                'Aceptar': function() {
                                                                    $(this).dialog("close");

                                                                    $('#init').removeClass('no-show');
                                                                    getData("Pag2.aspx/Valido",
                                                                        {
                                                                            id: dataRecord.ID
                                                                        });

                                                                    $("#jGrid").jqxGrid('setcellvalue',
                                                                        row,
                                                                        'REVISADO',
                                                                        'OK');
                                                                    $('#init').addClass('no-show');

                                                                    $("#detalle3").dialog("close");

                                                                },
                                                                'Cancelar': function() {

                                                                    $(this).dialog("close");
                                                                }
                                                            }
                                                        });

                                                        $("#confirmar").dialog('open');
                                                    },
                                                    'Cerrar': function() {

                                                        $(this).dialog("close");
                                                    }
                                                }
                                            });
                                        } else {*/
                                            $("#detalle3").dialog({
                                                buttons: {
                                                    'Cerrar': function() {

                                                        $(this).dialog("close");
                                                    }
                                                }
                                            });
                                        //}


                                        $("#detalle3").dialog('open');

                                        $('#init').addClass('no-show');
                                    },30);
                            }
                        },
                        { text: 'SERIE',               dataField: 'SERIE',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA',              dataField: 'TIENDA',              width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: '#COMPRA',             dataField: 'NO_COMPRA',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'Albaranes',
                            datafield: 'Albaranes',
                            columntype: 'button',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                IDgrid = dataRecord.ID;

                                $("#detalle5").dialog('open');

                                detalle();

                            }
                        },
                        { text: 'FECHA COMPRA',        dataField: 'FECHA_COMPRA',        width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA PROCESO',       dataField: 'FECHA_PROCESO',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'UUID',                dataField: 'UUID',                width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
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
                source5 =
                {
                    datatype: "json",
                    datafields: [
                        //{ name: 'NUMSERIE' },
                        //{ name: 'NUMALBARAN' },
                        //{ name: 'NUMLIN' },
                        { name: 'REFERENCIA' },
                        { name: 'UNIDADESTOTAL' },
                        { name: 'DESCRIPCION' },
                        { name: 'UNIDADESTOTAL' },
                        { name: 'PRECIO' },
                        { name: 'DTO' },
                        { name: 'TOTAL' },
                        { name: 'TIPOIMPUESTO' },
                        { name: 'CODALMACEN' }
                    ],
                    localdata: []
                };
                var dataAdapter5 = new $.jqx.dataAdapter(source5);

                $("#jGrid5").jqxGrid({ 
                    width: '100%',
                    height: 450,
                    source: dataAdapter5,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
                    enablebrowserselection :true,
                    //selectionmode: 'checkbox',
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
                        //{ text: 'NUMSERIE',   dataField: 'NUMSERIE',      width: 70, renderer: columnrenderer2 }, //, pinned: true 
                        //{ text: 'NUMALBARAN', dataField: 'NUMALBARAN',    width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'NUMLIN',     dataField: 'NUMLIN',        width: 70, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'REFERENCIA',   dataField: 'REFERENCIA',    width: 90,  renderer: columnrenderer2},
                        { text: 'DESCRIPCION',  dataField: 'DESCRIPCION',   width: 130, renderer: columnrenderer2, cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                        } },
                        { text: 'CANTIDAD',     dataField: 'UNIDADESTOTAL', width: 130, renderer: columnrenderer2, cellsalign: 'center'},
                        { text: 'PRECIO',       dataField: 'PRECIO',        width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'DTO',          dataField: 'DTO',           width: 90,  renderer: columnrenderer2, cellsalign: 'center'},
                        { text: 'TOTAL',        dataField: 'TOTAL',         width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'TIPOIMPUESTO', dataField: 'TIPOIMPUESTO',  width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'ALM',          dataField: 'CODALMACEN',    width: 90,  renderer: columnrenderer2, cellsalign: 'center' }
                        

                    ]
                });

//*****************************************************************************************************************




                source2 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'NUMSERIE' },
                        { name: 'NUMALBARAN' },
                        { name: 'FECHAALBARAN', type: 'date' },
                        { name: 'SUALBARAN' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IVA' },
                        { name: 'IEPS' }
                    ],
                    localdata: []
                };
                var dataAdapter2 = new $.jqx.dataAdapter(source2);

                $("#jGrid2").jqxGrid({ 
                    width: '100%',
                    height: 525,
                    source: dataAdapter2,
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
                    
                    ready: function () {
                        
                    },
                    columns: [
                        { text: 'NUMSERIE',   dataField: 'NUMSERIE',   width: 90, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'NUMALBARAN', dataField: 'NUMALBARAN', width: 90, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'SUALBARAN',  dataField: 'SUALBARAN',  width: 90, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL',      dataField: 'TOTAL',      width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',   dataField: 'SUBTOTAL',   width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',        dataField: 'IVA',        width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',       dataField: 'IEPS',       width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        {
                            text: 'DETALLE',
                            datafield: 'detalle',
                            columntype: 'button',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return '--';
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid2").jqxGrid('getrowdata', row);

                                $("#cSubTotal").html( numeral(dataRecord.SUBTOTAL).format('$0,0.00') );
                                $("#cTotal").html( numeral(dataRecord.TOTAL).format('$0,0.00') );
                                $("#cIVA").html( numeral(dataRecord.IVA).format('$0,0.00') );
                                $("#cIEPS").html( numeral(dataRecord.IEPS).format('$0,0.00') );

                                $('#init').removeClass('no-show');

                                $("#detalle2").dialog({ width: Math.min(800, $(window).width()) });
                                $("#detalle2").dialog('open');

                                source5.localdata = [];
                                dataAdapter5.dataBind();
                                $("#jGrid5").jqxGrid('clearselection');

                                source5.localdata = getData("Pag2.aspx/Detalle", { data: dataRecord });
                                dataAdapter5.dataBind();
                                $("#jGrid5").jqxGrid('ensurerowvisible', 0);


                                $("#jGrid5").jqxGrid("autoresizecolumns");

                                var colDefs = $("#jGrid5").jqxGrid('columns').records;
                                for ( var idx = 0; idx < colDefs.length; idx++) {
                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid5").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                    }
                                }

                                $("#jGrid5").jqxGrid('setcolumnproperty', "DESCRIPCION", 'width', 210);

                                $('#init').addClass('no-show');

                            }
                        },
                        { text: 'FECHAALBARAN', dataField: 'FECHAALBARAN', width: 100, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' }
                    ]
                });

//*****************************************************************************************************************
                source3 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'TOTAL' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                    ],
                    localdata: []
                };
                var dataAdapter3 = new $.jqx.dataAdapter(source3);

                $("#jGrid3").jqxGrid({ 
                    width: '100%',
                    height: 450,
                    source: dataAdapter3,
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
                        { text: 'CONTR', dataField: 'CONTRA_RECIBO_ID', width: 130, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                        { text: 'TOTAL', dataField: 'TOTAL',            width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'SERIE', dataField: 'SERIE',            width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO', dataField: 'FOLIO',            width: 130, renderer: columnrenderer2, cellsalign: 'center' }

                    ]
                });

//*****************************************************************************************************************
                source4 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'TOTAL' },
                        { name: 'NUMALBARAN' },
                        { name: 'SUALBARAN' }
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
                        { text: 'TOTAL',      dataField: 'TOTAL',      width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', }, //, pinned: true 
                        { text: 'NUMALBARAN', dataField: 'NUMALBARAN', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'SUALBARAN',  dataField: 'SUALBARAN',  width: 130, renderer: columnrenderer2, cellsalign: 'center' }

                    ]
                });

//*****************************************************************************************************************
                source6 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'CANTIDAD' },
                        { name: 'UNIDAD' },
                        { name: 'CLAVE' },
                        { name: 'DESCRIPCION' },
                        { name: 'PU' },
                        { name: 'IMPORTE' }
                    ],
                    localdata: []
                };
                var dataAdapter6 = new $.jqx.dataAdapter(source6);

                $("#jGrid6").jqxGrid({ 
                    width: 700, //1017
                    height: 450,
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
                        { text: 'CANT',        dataField: 'CANTIDAD',    width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                        { text: 'UNI',         dataField: 'UNIDAD',      width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                        { text: 'CLAVE',       dataField: 'CLAVE',       width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                        { text: 'DESCRIPCION', dataField: 'DESCRIPCION', width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'P/U',         dataField: 'PU',          width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'IMPORTE',     dataField: 'IMPORTE',     width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }

                    ]
                });


//*****************************************************************************************************************


                source7 =
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
                var dataAdapter7 = new $.jqx.dataAdapter(source7);

                $("#jGrid7").jqxGrid({ 
                    width: '100%',
                    height: 390,
                    source: dataAdapter7,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
                    enablebrowserselection :true,
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
                        { text: 'ARCHIVO', dataField: 'ARCHIVO', width: 340, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'PDF',     dataField: 'PDF',     width: 100, renderer: columnrenderer2, cellsalign: 'center' },
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



                source8 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        { name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        { name: 'TIENDA' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'ESTATUS_ID' }
                    ],
                    localdata: []
                };
                var dataAdapter8 = new $.jqx.dataAdapter(source8);

                $("#jGrid8").jqxGrid({ width: 1000, height: 400,
                    source: dataAdapter8,
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
                        { text: 'CONTR',               dataField: 'CONTRA_RECIBO_ID',    width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL',               dataField: 'TOTAL',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',            dataField: 'SUBTOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',           dataField: 'IMPUESTOS',           width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',                 dataField: 'IVA',                 width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',                dataField: 'IEPS',                width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
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

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);

                                $("#cSubTotal2").html( numeral(dataRecord.SUBTOTAL).format('$0,0.00') );
                                $("#cTotal2").html( numeral(dataRecord.TOTAL).format('$0,0.00') );
                                $("#cIVA2").html( numeral(dataRecord.IVA).format('$0,0.00') );
                                $("#cIEPS2").html( numeral(dataRecord.IEPS).format('$0,0.00') );

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);

                                //$("#detalle3").dialog({ width: Math.min(340, $(window).width()) });

                                source6.localdata = getData("Pag2.aspx/GetDetalle", { id: dataRecord.ID });
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

                                $("#detalle3").dialog('open');

                                $('#init').addClass('no-show');

                            }
                        },
                        { text: 'SERIE',               dataField: 'SERIE',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',               dataField: 'FOLIO',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA',              dataField: 'TIENDA',              width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'UUID',                dataField: 'UUID',                width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
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

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);
                                
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

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);
                                
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

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);
                                
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


                //-------------------------------------


                function detalle() {

                    $('#init').removeClass('no-show');
                    setTimeout(function () {
                        var rs = getData("Pag2.aspx/GetDetalle2", { id: IDgrid });

                        source9.localdata = rs.data;
                        dataAdapter9.dataBind();
                        $("#jGrid9").jqxGrid('ensurerowvisible', 0);
                        $("#jGrid9").jqxGrid('clearselection');


                        $("#jGrid9").jqxGrid("autoresizecolumns");

                        var colDefs = $("#jGrid9").jqxGrid('columns').records;
                        for (var idx = 0; idx < colDefs.length; idx++) {
                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                $("#jGrid9").jqxGrid('setcolumnproperty',
                                    colDefs[idx].datafield,
                                    'width',
                                    colDefs[idx].width + 5);
                            }
                        }

                        $('#init').addClass('no-show');
                    }, 30);
                }
//*****************************************************************************************************************

                source9 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'SERIE' },
                        { name: 'FOLIO' }
                    ],
                    localdata: []
                };

                var dataAdapter9 = new $.jqx.dataAdapter(source9);

                $("#jGrid9").jqxGrid({
                    width: 325,
                    height: 390,
                    source: dataAdapter9,
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

                //---------------------------------------------------------------------------


                //---------------------------------------------------------------------------

                $('#UploadFile').ajaxForm({

                    beforeSubmit: function () {
                        $("#Proc").button({disabled: true});
                        $("#cExaminar1").button({ disabled: true });
                        $("#cExaminar2").button({ disabled: true });

                        //status.empty();
                        var percentVal = '0%';
                        bar.width(percentVal);
                        percent.html(percentVal);

                        var pm = new Array();

                        if ($.trim($("#archivo1").val() ) == "") {
                            //param += ", <strong>Archivo</strong>";
                            pm.push("<strong>Archivo XML</strong>");
                        }

                        if ($.trim($("#archivo2").val() ) == "") {
                            //param += ", <strong>Archivo</strong>";
                            //pm.push("<strong>Archivo PDF</strong>");
                        }


                        if (pm[0]) {
                            $("#Proc").button({disabled: false});
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
                            window.location.href = "Pag2.aspx";
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

                            $("#detalle4").dialog({ width: Math.min(1050, $(window).width()) });
                            $("#detalle4").dialog('open');
                            source7.localdata = rs.result;
                            dataAdapter7.dataBind();
                            $("#jGrid7").jqxGrid('clearselection');
                            $("#jGrid7").jqxGrid('ensurerowvisible', 0);

                            $('#init').addClass('no-show');
                            

                        }
                        catch (err) {
                            //alert(err.message);
                            //console.debug(xhr.responseText);
                            //console.debug(xhr.responseText.msg);
                            window.location.href = "Pag2.aspx";
                        }
                    }
                });

                //**************************************************************************************************


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

                //---------------------------------------------------------------------------

                $("#bAccion").click(function (event) {
                    //if()
                    var getselectedrowindexes = $('#jGrid').jqxGrid('getselectedrowindexes');
                    if (getselectedrowindexes.length > 0) {
                        
                        var proc = true;
                        var select1 = new Array();

                        $.each(getselectedrowindexes, function( index, value ) {
                            var data = $('#jGrid').jqxGrid('getrowdata', value);

                            //if ($("#bAccion").val() == "Rechazar") {
                                if (data.ESTATUS_ID == 2 || data.ESTATUS_ID == 6) {
                                    proc = false;
                                } else {
                                    select1.push({
                                        ESTATUS: data.ESTATUS_ID,
                                        ID: data.ID
                                    });
                                }
                            /*} else {
                                if (data.ESTATUS_ID == 3) {
                                    select1.push({
                                        ESTATUS: data.ESTATUS_ID,
                                        ID: data.ID
                                    });
                                }
                            }*/


                        });

                        //if ($("#bAccion").val() == "Rechazar") {
                            if (proc) {

                                if (!$.isEmptyObject(select1)) {

                                    $("#confirmar")
                                        .html(
                                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres rechazar las facturas seleccionadas?</div>");

                                    $("#confirmar").dialog({
                                        buttons: {
                                            'Aceptar': function() {
                                                $(this).dialog("close");

                                                $('#init').removeClass('no-show');
                                                var rs = getData("Pag2.aspx/SetAccion",
                                                    {
                                                        data: select1,
                                                        estatus: 3
                                                    });

                                                if (rs.code == -1) {
                                                    $("#message")
                                                        .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                                            rs.msg +
                                                            "</div>");
                                                    $("#message").dialog('open');
                                                } else {
                                                    buscar();
                                                }

                                                
                                                $('#init').addClass('no-show');

                                            },
                                            'Cancelar': function() {

                                                $(this).dialog("close");
                                            }
                                        }
                                    });

                                    $("#confirmar").dialog('open');

                                }

                            } else {
                                $("#message")
                                    .html(
                                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>Desmarque las facturas con estatus de asociadas o pagadas.</div>");
                                $("#message").dialog('open');
                            }
                        /*} else {
                            if (!$.isEmptyObject(select1)) {

                                $("#confirmar").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres aceptar las facturas seleccionadas?</div>");


                                $("#confirmar").dialog({
                                    buttons: {
                                        'Aceptar': function() {
                                            $(this).dialog("close");

                                            $('#init').removeClass('no-show');
                                            getData("Pag2.aspx/SetAccion",{ 
                                                data: select1,
                                                estatus: 1
                                            });
                                            buscar();
                                            $('#init').addClass('no-show');

                                        },
                                        'Cancelar': function() {

                                            $(this).dialog("close");
                                        }
                                    }
                                });
                                $("#confirmar").dialog('open');

                            }
                        }*/

                    }else {
                        $("#message")
                            .html(
                                "<div style='text-align: center;min-width:250px;margin-top:20px;'>No hay facturas seleccionadas</div>");
                        $("#message").dialog('open');
                    }
                });



                $("#bExportar").click(function (event) {
                    $("#exportar").dialog('open');
                });

                $("#bExportar3").click(function (event) {
                    $('#init').removeClass('no-show');

   
                    var select1 = new Array();
                    var select2 = new Array();

                    var rowindexes1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var rowindexes2 = $('#jGrid2').jqxGrid('getselectedrowindexes');

                    $.each(rowindexes1, function( index, value ) {
                        var data = $('#jGrid').jqxGrid('getrowdata', value);
                        select1.push(data.ID);
                    });

                    $.each(rowindexes2, function( index, value ) {
                        var data = $('#jGrid2').jqxGrid('getrowdata', value);
                        select2.push({
                            TOTAL:        data.TOTAL,
                            NUMALBARAN:   data.NUMALBARAN,
                            SUALBARAN:    data.SUALBARAN,
                            NUMSERIE:     data.NUMSERIE,
                            FECHAALBARAN: data.FECHAALBARAN
                        });
                    });

                    

                    if (!$.isEmptyObject(select1) || !$.isEmptyObject(select2)) {
                        getData("excel3.aspx/SetID", {
                            data1:select1,
                            data2:select2
                        });

                        window.open("excel3.aspx", "xml", "width=200,height=150");
                    }

                    $('#init').addClass('no-show');
                });

                function wait(ms)
                {
                    var d = new Date();
                    var d2 = null;
                    do {
                        d2 = new Date();
                    }
                    while(d2-d < ms);
                }

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
                    setTimeout(function() {


                            var data = getData("Pag2.aspx/GetRegistros",
                                {
                                    req: {
                                        fecha1: fecha1,
                                        fecha2: fecha2,
                                        estatus: estatus,
                                        RFC_EMISOR: rfc,
                                        folio: folio,
                                        serie: serie,
                                        contr: contr,
                                        opt: opt
                                    }
                                });

                            var rows = $('#jGrid8').jqxGrid('getrows');

                            var filteredArray = rows.filter(function(x) {
                                return !data.some(function(item) {
                                    return item.ID == x.ID;
                                });
                            });

                            source8.localdata = filteredArray;
                            dataAdapter8.dataBind();
                            $("#jGrid8").jqxGrid('clearselection');
                            $("#jGrid8").jqxGrid('updatebounddata', 'cells');


                            source.localdata = data;
                            dataAdapter.dataBind();
                            $("#jGrid").jqxGrid('clearselection');
                            $("#jGrid").jqxGrid('updatebounddata', 'cells');

                            $("#jGrid").jqxGrid("autoresizecolumns");

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

                            $("#jGrid").jqxGrid('setcolumnproperty', "Albaranes", 'width', 60);
                            $("#jGrid").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
                            $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                            $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                            $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                            $("#jGrid").jqxGrid('setcolumnproperty', "IMPUESTOS", 'width', 90);

                            $('#init').addClass('no-show');

                        },30);
                    //window.open("excel.aspx", "xml", "width=200,height=150");
                }

                $("#bBuscar").click(function (event) {
                    buscar();
                });

                function buscar2() {
                    if ($("#rfc").html() == "") {
                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
                        $("#message").dialog('open');
                        return;
                    }

                    $('#init').removeClass('no-show');

                    var fecha1 = $("#jDate3").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate4").jqxDateTimeInput('getDate');

                    var index  = $("#cbSerie").jqxDropDownList('selectedIndex');

                    var serie = "";
                    if (index != -1) {
                        serie = $("#cbSerie").jqxDropDownList('getItem', index).value;    
                    }

                    source2.localdata = getData("Pag2.aspx/GetCompras",{ 
                        rfc: $("#rfc").html(),
                        fecha1: fecha1,
                        fecha2: fecha2,
                        serie: serie
                    });


                    dataAdapter2.dataBind();
                    $("#jGrid2").jqxGrid('clearselection');
                    $("#jGrid2").jqxGrid('updatebounddata', 'cells');
                    $("#jGrid2").jqxGrid('ensurerowvisible', 0);
                    $("#jGrid2").jqxGrid("autoresizecolumns");

                    var colDefs = $("#jGrid2").jqxGrid('columns').records;
                    for ( var idx = 0; idx < colDefs.length; idx++) {
                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                            $("#jGrid2").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                        }
                    }
                    $("#jGrid2").jqxGrid('setcolumnproperty', "detalle", 'width', 60);



                    $('#init').addClass('no-show');
                }

                $("#bBuscar2").click(function (event) {
                    buscar2();
                });

                //----------------------------------------------------------------------------//



                function buscar3()
                {
                    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

                    var fecha1 = $("#jDate5").jqxDateTimeInput('getDate');
                    var fecha2 = $("#jDate6").jqxDateTimeInput('getDate');

                    var index1  = $("#cbEstatus2").jqxDropDownList('selectedIndex');
                    var estatus = $("#cbEstatus2").jqxDropDownList('getItem', index1).value;

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

                    var folio = $("#txFolio2").val();
                    var serie = $("#txSerie2").val();
                    var contr = $("#txContr2").val();

                    $('#init').removeClass('no-show');
                    
                    var data = getData("Pag2.aspx/GetRegistros",
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

                    var rows = $('#jGrid').jqxGrid('getrows');

                    var filteredArray = data.filter(function(x) {
                        return !rows.some(function(item) {
                            return item.ID == x.ID;
                        });
                    });

                    source8.localdata = filteredArray; //data;
                    dataAdapter8.dataBind();
                    $("#jGrid8").jqxGrid('clearselection');
                    $("#jGrid8").jqxGrid('updatebounddata', 'cells');

                    $("#jGrid8").jqxGrid("autoresizecolumns");

                    var colDefs = $("#jGrid8").jqxGrid('columns').records;
                    for ( var idx = 0; idx < colDefs.length; idx++) {
                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                            $("#jGrid8").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                        }
                    }

                    $("#jGrid8").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
                    $("#jGrid8").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                    $("#jGrid8").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                    $("#jGrid8").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                    $("#jGrid8").jqxGrid('setcolumnproperty', "IMPUESTOS", 'width', 90);

                    $('#jGrid8').jqxGrid('scrolloffset', 0, 0);

                    $('#init').addClass('no-show');
                    //window.open("excel.aspx", "xml", "width=200,height=150");
                }

                $("#bBuscar3").click(function (event) {
                    buscar3();
                });



                $("#jGrid8").on('rowselect', function (event) {
                   total3();

                });

                $("#jGrid8").bind('rowunselect', function (event) {
                    total3();
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


                var select1 = new Array();
                var select2 = new Array();
                $("#bProcesar").click(function (event) {

                    var rowindexes1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var rowindexes2 = $('#jGrid2').jqxGrid('getselectedrowindexes');

                    var isOK = true;
                    try {

                        $.each(rowindexes1,
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

                        $("#message").html("<div style='margin-top:15px;'>No se puede procesar, alguna factura seleccionada esta rechazada.</div>");
                        $("#message").dialog('open');
                        return;
                    }

                    var importe1 = 0;
                    select1 = new Array();
                    $.each(rowindexes1, function( index, value ) {
                        var data = $('#jGrid').jqxGrid('getrowdata', value);
                        select1.push({
                             CONTRA_RECIBO_ID: data.CONTRA_RECIBO_ID,
                             TOTAL:            data.TOTAL,
                             SERIE:            data.SERIE,
                             FOLIO:            data.FOLIO,
                             UUID:             data.UUID,
                             ESTATUS_ID:       data.ESTATUS_ID,
                             ID:               data.ID,
                             RFC_EMISOR:       data.RFC_EMISOR
                        });
                        importe1 += data.TOTAL;
                    });

                    source3.localdata = select1;
                    dataAdapter3.dataBind();
                    $("#jGrid3").jqxGrid('clearselection');
                    $("#jGrid3").jqxGrid('updatebounddata', 'cells');


                    var importe2 = 0;
                    select2 = new Array();
                    $.each(rowindexes2, function( index, value ) {
                        var data = $('#jGrid2').jqxGrid('getrowdata', value);
                        select2.push({
                            TOTAL:      data.TOTAL,
                            NUMALBARAN: data.NUMALBARAN,
                            SUALBARAN:  data.SUALBARAN,
                            NUMSERIE:   data.NUMSERIE
                        });
                        importe2 += data.TOTAL;
                    });

                    source4.localdata = select2;
                    dataAdapter4.dataBind();
                    $("#jGrid4").jqxGrid('clearselection');
                    $("#jGrid4").jqxGrid('updatebounddata', 'cells');


                    $("#detalle").dialog({buttons:
                    {
                        'Cerrar':function() {

                                $(this).dialog("close");
                            }
                    }});

                    $("#msgIMP").html("");

                    if (!$.isEmptyObject(select1) && !$.isEmptyObject(select2)) {

                        var dif = Math.abs(importe1 - importe2);
                        try {
                            console.log(dif);
                        } catch (e) {

                        } 

                        var _importe1 = numeral(importe1).format('$0,0.00');
                        var _importe2 = numeral(importe2).format('$0,0.00');

                        $("#imp1").html(_importe1);
                        $("#imp2").html(_importe2);

                        if (dif >= 0 && dif <= 0.99) { //importe1 == importe2
                        //if (importe1 != importe2) {
                            $("#detalle").dialog({
                                buttons:
                                {
                                    'Procesar': function() {

                                        $('#init').removeClass('no-show');
                                        setTimeout(function() {


                                                var fechaProd = "";
                                                
                                                <% if( User.IsInRole("5") )
                                                   { %>
                                                fechaProd = $("#jDate7").jqxDateTimeInput('getDate');
                                                <% } %>
                                                console.log(fechaProd);
                                                var rs = getData("Pag2.aspx/Procesar",
                                                    {
                                                         facturas: select1,
                                                          compras: select2,
                                                        fechaProd: fechaProd/*,
                                                        dif: numeral(importe1 - importe2).format('0.00')*/
                                                    });

                                                if (rs.code == 0) {
                                                    buscar2();
                                                    buscar();
                                                }

                                                $("#message")
                                                    .html(
                                                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                                        rs.msg +
                                                        "</div>");

                                                $("#message").dialog('open');
                                                position('.mWin');

                                                if (rs.code == 2) {
                                                    //window.open("cartaPagoPDF.aspx", "xml", "width=200,height=150");
                                                }

                                                $('#init').addClass('no-show');

                                                $("#detalle").dialog("close");

                                            },
                                            1000);

                                        
                                    },
                                    'Cerrar': function() {

                                        $(this).dialog("close");
                                    }
                                }
                            });
                        } else {
                            
                            $("#msgIMP").html("<span style='color:red'>Los importes no coinciden.</span>");
                        }

                        $("#detalle").dialog({ width: Math.min(800, $(window).width()) });
                        $("#detalle").dialog('open');

                        $("#jGrid3").jqxGrid("autoresizecolumns");
                        var colDefs = $("#jGrid3").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {
                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                $("#jGrid3").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                            }
                        }

                        $("#jGrid4").jqxGrid("autoresizecolumns");
                        colDefs = $("#jGrid4").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {
                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                $("#jGrid4").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 10);
                            }
                        }

                    } else {
                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Relacione las facturas con las compras.</div>");
                        $("#message").dialog('open');
                    }

                });

                //UploadRW();
                //---------------------------------------------------------------------------



                $('#bSub').click(function () {

                    var index2  = $("#cbRFC").jqxComboBox('selectedIndex');
                    if (index2 == -1) {
                        
                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
                        $("#message").dialog('open');
                        return;
                    }
                    

                    $("#sub").dialog({
                        buttons:
                        {
                            'Agregar': function() {
                                $(".ui-button").removeClass( "ui-state-focus" );
                                $(".ui-button").removeClass( "ui-state-hover" );



                                var isOK = true;

                                var position = $("#jGrid8").jqxGrid('scrollposition');
                                //var left = position.left;
                                //var top = position.top;
                    
                                // get the indexes of the selected rows.
                                var selectedrowindexes = $("#jGrid8").jqxGrid('getselectedrowindexes');
                                var rowscount = $("#jGrid8").jqxGrid('getdatainformation').rowscount;

                                if ($.isEmptyObject(selectedrowindexes)) {
                                    $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione alguna factura.</div>");
                                    $("#message").dialog('open');
                                    return;
                                }

                                selectedrowindexes.sort().reverse();

                                //var selectedrowindex = $("#jGrid8").jqxGrid('getselectedrowindex');
                                //var id = $("#jGrid8").jqxGrid('getrowid', selectedrowindex);
                                //var data = $('#jGrid8').jqxGrid('getrowdatabyid', id);

                                var select = new Array();
                                var arrayOfSelectedIds = [];

                                $.each(selectedrowindexes, function( index, value ) {
                                    var id = $("#jGrid8").jqxGrid('getrowid', value);
                                    var data = $('#jGrid8').jqxGrid('getrowdatabyid', id);

                                    select.push(data);
                                    arrayOfSelectedIds.push(id);
                                });

                                $("#jGrid").jqxGrid('addrow', null, select);
                                $("#jGrid8").jqxGrid('deleterow', arrayOfSelectedIds);

                                $("#jGrid8").jqxGrid('clearselection');

                                
                                var verticalScrollOffset = $("#jqxScrollAreaUpverticalScrollBar" + "jGrid").height();
                                $('#jGrid').jqxGrid('scrolloffset', verticalScrollOffset - 1, 0);
                                $('#jGrid').jqxGrid('scrolloffset', verticalScrollOffset + 1, 0);
                                $('#jGrid').jqxGrid('scrolloffset', position.top, 0);

                                $("#jGrid").jqxGrid("autoresizecolumns");
                                var colDefs = $("#jGrid").jqxGrid('columns').records;
                                for ( var idx = 0; idx < colDefs.length; idx++) {
                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                    }
                                }

                                $("#jGrid").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
                                $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                                $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                                $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                                $("#jGrid").jqxGrid('setcolumnproperty', "IMPUESTOS", 'width', 90);

                                /*
                                var arrayOfSelectedIds = [];

                                // begin update. Stops the Grid's rendering.
                                $("#jGrid").jqxGrid('beginupdate');
                                $("#jGrid8").jqxGrid('beginupdate');
                                selectedrowindexes.sort().reverse();
                                // delete the selected rows by using the 'deleterow' method of jqxGrid.
                                for (var m = 0; m < selectedrowindexes.length; m++) {
                                    var selectedrowindex = selectedrowindexes[selectedrowindexes.length - m - 1];
                                    if (selectedrowindex >= 0 && selectedrowindex < rowscount) {

                                        var data = $('#jGrid8').jqxGrid('getrowdata', selectedrowindex);
                                        $("#jGrid").jqxGrid('addrow', null, data);

                                        var id = $("#jGrid8").jqxGrid('getrowid', selectedrowindex);
                                        arrayOfSelectedIds.push(id);
                                        
                                    }
                                }


                                $("#jGrid8").jqxGrid('deleterow', arrayOfSelectedIds);
                                // end update. Resume the Grid's rendering.
                                $("#jGrid").jqxGrid('endupdate');
                                $("#jGrid8").jqxGrid('endupdate');

                                $("#jGrid8").jqxGrid('clearselection');

                                
                                var verticalScrollOffset = $("#jqxScrollAreaUpverticalScrollBar" + "jGrid").height();
                                $('#jGrid').jqxGrid('scrolloffset', verticalScrollOffset - 1, 0);
                                $('#jGrid').jqxGrid('scrolloffset', verticalScrollOffset + 1, 0);
                                $('#jGrid').jqxGrid('scrolloffset', position.top, 0);

                                $("#jGrid").jqxGrid("autoresizecolumns");
                                var colDefs = $("#jGrid").jqxGrid('columns').records;
                                for ( var idx = 0; idx < colDefs.length; idx++) {
                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid").jqxGrid('setcolumnproperty',colDefs[idx].datafield,'width',colDefs[idx].width + 5);
                                    }
                                }
                                */

                            },
                            'Cerrar': function() {

                                $(this).dialog("close");
                            }
                        }
                    });


                    $("#sub").dialog('open');

                    position('.mWin2');
                    
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


                /*$("#compras").dialog({
                    autoOpen: false,
                    resizable: false,
                    //width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Buscar': function () {
 
                            var fecha1 = $("#jDate3").jqxDateTimeInput('getDate');
                            var fecha2 = $("#jDate4").jqxDateTimeInput('getDate');

                            var index  = $("#cbSerie").jqxDropDownList('selectedIndex');
                            if (index == -1) {
                                index = 0;
                            }

                            var serie = $("#cbSerie").jqxDropDownList('getItem', index).value;

                            source2.localdata = getData("Pag2.aspx/GetCompras",{ 
                                 rfc: $("#rfc").html(),
                                 fecha1: fecha1,
                                 fecha2: fecha2,
                                 serie: serie
                            });


                            dataAdapter2.dataBind();
                            $("#jGrid2").jqxGrid('ensurerowvisible', 0);
                            
                        },
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
                    close: function(event, ui) {
                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('clearselection');
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');
                    }
                });*/

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

                $("#detalle2").dialog({
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

                $("#detalle4").dialog({
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


                $("#detalle5").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Exportar': function () {

                            getData("excel4.aspx/SetID", { id: IDgrid });

                            window.open("excel4.aspx", "xml", "width=200,height=150");

                        },

                        'Cerrar': function () {

                            source9.localdata = [];
                            dataAdapter9.dataBind();
                            $("#jGrid9").jqxGrid('ensurerowvisible', 0);
                            $("#jGrid9").jqxGrid('clearselection');

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
                    width: 'auto',
                    modal: true,
                    dialogClass: 'mWin',
                    open: function (event, ui) {

                    }
                });

                $("#sub").dialog({
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
                                getData("excel.aspx/SetID", {
                                    data:select,
                                    chkXML:chkXML,
                                    chkPDF:chkPDF
                                });

                                window.open("excel.aspx", "xml", "width=200,height=150");
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

                //$("#cbRFC").jqxComboBox({selectedIndex: 0});
                //$("#jDate1").jqxDateTimeInput({value:new Date(2010, 0, 1) ,height: '25px', theme: 'Theme2' ,culture: 'es'});
                //buscar();
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
                        <td style="text-align: left; padding: 5px; padding-left: 0px;" colspan="2">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="text-align: left; padding: 5px; padding-right: 5px;">
                                        <div>Fecha Inicial <span id="lbFecha1">Recepción</span></div>
                                        <div id="jDate1" style="width: 160px !important;"></div>
                                    </td>
                                    <td style="text-align: left; padding: 5px; padding-right: 5px;">
                                        <div>Fecha Final <span id="lbFecha2">Recepción</span></div>
                                        <div id="jDate2" style="width: 160px !important;"></div>
                                    </td>
                                    <td style="padding-top: 15px;">
                                        <div id="jqxOpt1">Recepción</div>
                                        <div id="jqxOpt2">Proceso</div>
                                    </td>
                                </tr>
                            </table>

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
                                                    <input type="button" id="bAccion" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Rechazar"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bBuscar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bExportar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                                </td>
                                                <!--<td>
                                                    <input type="button" id="bSub" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="+"/>
                                                </td>-->
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
                
                
                
                <table style="width: 100%">

                    <tr>
                        <td style="text-align: left; padding: 5px; width: 200px;">
                            <div>RFC</div>
                            <div class="txCenter" id="rfc" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 200px !important;"></div>
                            <%--<div><input class="jTx" id="txRFC" type="text" value="" style="width: 197px;"/></div>--%>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Serie</div>
                            <div class="jCb" id="cbSerie" style="width: 100px;"></div>
                        </td>
                        <td style="padding: 5px;" align="left">
                            <div>&nbsp;</div>
                            <input type="button" id="bCarga2" class="btn1" style="width:130px;height: 30px; padding-top: 5px; font-size: 1em;" value="Importar ZIP"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;">
                            <div>Fecha Inicial</div>
                            <div id="jDate3"></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 100px;">
                            <div>Fecha Final</div>
                            <div id="jDate4"></div>
                        </td>
                        <td style="padding: 5px;" align="left">
                            <div>&nbsp;</div>
                            <input type="button" id="bCarga" class="btn1" style="width:130px;height: 30px; padding-top: 5px; font-size: 1em;" value="Importar XML"/>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding: 5px;">
                            
                            <table style="width: 100%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin: auto;background-color: #fbfbfb">
                                <tr>
                                    <td align="left">
                                        Total: <span id="total2"></span>
                                    </td>
                                    <td align="right">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="bExportar3" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Exportar"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="bBuscar2" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>                                                    
                                                </td>
                                                <td>
                                                    <input type="button" id="bProcesar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Procesar"/>
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
                            <div id="jGrid2" style="margin-bottom: 0px"></div>
                        </td>
                    </tr>
                </table>
                


            </td>
        </tr>

    </table>

                <div id="message" title="Mensaje del sistema"></div>
                <div id="confirmar" title="Confirmar"></div>
                <div id="detalle" title="Detalle">
                <div id="detalle4" title="Detalle"><div id="jGrid7" style="margin-bottom: 0px;margin-top: 7px"></div>
                </div>
                    
                <%----%>
                
    <table style="width: 100%; border: 1px #b4b2b2 solid; margin-top: 5px;">
        <tr>
            <td style="border: 1px #b4b2b2 solid;width: 700px" valign="top">
                <div id="jGrid3" style="margin-bottom: 0px"></div>
            </td>
            <td style="width: 735px;" valign="top">
                <div id="jGrid4" style="margin-bottom: 0px"></div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table style="width: 100%" cellpadding="3">
                    <tr>
                        <td style="width: 120px;">
                            Total Factura
                        </td>
                        <td style="width: 260px;">
                            <div class="txCenter" id="imp1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                        </td>
                        <td>
                            Total Compra
                        </td>
                        <td style="width: 260px;">
                            <div class="txCenter" id="imp2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                        </td>
                    </tr>
                    
                    <% if( User.IsInRole("5") )
                       { %>
                    <tr>
                        <td>

                        </td>
                        <td>

                        </td>
                        <td>
                            Fecha Proceso
                        </td>
                        <td>
                            <div id="jDate7" style="width: 160px !important;"></div>
                        </td>
                    </tr>
                    <% } %>
                </table>
                
            </td>
        </tr>
        <tr>
            <td id="msgIMP" colspan="2" style="text-align: center; padding: 2px;">
                
            </td>
        </tr>
    </table>
                <%----%>
                
                </div>
                              
                <div id="detalle2" title="Detalle">
                    <div id="jGrid5" style="margin-top: 10px"></div>
                    <table style="width: 100%; margin-top: 5px;">
                        <tr>
                            <td style="width: 160px" align="left">SUBTOTAL</td>
                            <td style="width: 160px" align="left">IVA</td>
                            <td style="width: 160px" align="left">IEPS</td>
                            <td style="width: 160px" align="left">TOTAL</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="txCenter" id="cSubTotal" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cIVA" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cIEPS" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cTotal" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                
                <div id="detalle3" title="Detalle">
                    <div id="jGrid6" style="margin-top: 10px"></div>
                    <table style="width: 100%; margin-top: 5px;">
                        <td colspan="5">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left">
                                        METODO DE PAGO
                                    </td>
                                    <td align="left">
                                        FORMA PAGO
                                    </td>
                                    <td style="width: 100px;" align="left">
                                        MONEDA
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 6px;">
                                        <div class="txCenter" id="cMPago" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; padding-left: 5px; width: 100% !important; text-align: left;"></div>
                                    </td>
                                    <td style="padding-bottom: 6px;">
                                        <div class="txCenter" id="cFPago" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; padding-left: 5px; width: 100% !important; text-align: left;"></div>
                                    </td>
                                    <td style="padding-bottom: 6px;">
                                        <div class="txCenter" id="cMoneda" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; width: 100% !important;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        USO CFDI
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 6px;" colspan="3">
                                        <div class="txCenter" id="cUCfdi" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; padding-left: 5px; width: 100% !important; text-align: left;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3">
                                        REGIMEN FISCAL
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 6px;" colspan="3">
                                        <div class="txCenter" id="cRFiscal" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; padding-left: 5px; width: 100% !important; text-align: left;"></div>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <tr>
                            <td style="width: 160px" align="left">SUBTOTAL</td>
                            <td style="width: 160px" align="left">IVA</td>
                            <td style="width: 160px" align="left">IEPS</td>
                            <td style="width: 160px" align="left">TOTAL</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="txCenter" id="cSubTotal2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cIVA2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cIEPS2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cTotal2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                
                <div id="exportar" title="Exportar">
                    <div id='chkXML' style="margin-top: 20px">Agregar XML</div>
                    <div id='chkPDF'>Agregar PDF proveedor</div>
                </div>



<!-------------------------------------------------->

                <div id="detalle5" title="Detalle">
                    <table style="margin-bottom: 0px;margin-top: 7px">
                        <tr>
                            <td>
                                <div id="jGrid9" style=""></div>
                            </td>
                        </tr>
                    </table>
                </div>


<!------------------- Sub -------------------------->
<div id="sub" title="Sub Consulta">
    
    
<table style="width: 99%; border: 1px #b4b2b2 solid; margin: auto; margin-top: 10px;font-family: 'Segoe UI', Verdana, Helvetica, Sans-Serif;font-size: 14px;">
        <tr>
            <td style="border:1px #b4b2b2 solid" valign="top">
                

                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left; padding: 5px; width: 160px;">
                            <div>RFC</div>
                            <div class="txCenter" id="rfc2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 202px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 160px;">
                            <div>Estatus</div>
                            <div class="jCb" id="cbEstatus2" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Serie</div>
                            <div><input class="jTx" id="txSerie2" type="text" value="" style="width: 197px;" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;">
                            <div>Fecha Inicial Recepción</div>
                            <div id="jDate5" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Fecha Final Recepción</div>
                            <div id="jDate6" style="width: 160px !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Folio</div>
                            <div><input class="jTx" id="txFolio2" type="text" value="" style="width: 197px;" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;" colspan="2">
                             <div>Razon Social</div>
                             <div class="txCenter" id="razonSocial2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                        </td>
                        <td style="text-align: left; padding: 5px;">
                            <div>Contrarecibo</div>
                            <div><input class="jTx" id="txContr2" type="text" value="" style="width: 197px;" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding: 5px;">
                            
                            
                            <table style="width: 100%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin: auto;background-color: #fbfbfb">
                                <tr>
                                    <td align="left">
                                        Total: <span id="total3"></span>
                                    </td>
                                    <td align="right">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="bBuscar3" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
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
                            <div id="jGrid8" style="margin-top: 10px"></div>
                        </td>
                    </tr>
                </table>
                

                
                
            </td>
        </tr>

    </table>
    
    
</div>
<!------------------- /Sub -------------------------->
                

                <div id="inv" title="Carga XML">
                        <form id="UploadFile" style="max-width: 400px; margin: auto;" action="fileUploader.asmx/Upload" method="POST" enctype="multipart/form-data">
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
                        <form id="UploadFile2" style="max-width: 400px; margin: auto;" action="fileUploader3.asmx/Upload2" method="POST" enctype="multipart/form-data">
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

    <asp:Label ID="Script1" runat="server"></asp:Label>
</asp:Content>