<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag3.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag3" %>
<%@ Import Namespace="WB_CFDI_V1.Vistas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
        <script type="text/javascript">

            var cerrarCarta = false;
            <% if (User.IsInRole("2")){  %>
                cerrarCarta = true;
            <% }  %>

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
            function isEmpty(value) {
                //return (value == null || value.length === 0);
                return (value == null || $.trim(value) === '');
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
            $(".jTx").jqxInput({height: 21, theme: 'Theme2' });
            $('#jqxTabs').jqxTabs({ height: 677, width: 734 });
            /*******************************************************************************/

            var jsMostrarAgregarQuitar = <%= (ShowBtn() != null && ShowBtn().MOSTRAR_AGREGAR_QUITAR_FAC) ? "true" : "false" %>;

            /* Si no puede, ocultamos la etiqueta del tab "Crear" pero dejamos el panel en el DOM */
            if (!jsMostrarAgregarQuitar) {
                $('#jqxTabs ul li').filter(function () {
                    return $.trim($(this).text()) === 'Crear';
                }).hide();

                try {
                    var sel = $('#jqxTabs').jqxTabs('selectedItem');
                    if (sel === 0) {
                        $('#jqxTabs').jqxTabs('select', 1);
                    }
                } catch (e) {
                    console.debug('jqxTabs select/selectedItem no disponible', e);
                }
            }

            /*******************************************************************************/

            $("#jDate1").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
            $("#jDate2").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

            $("#jDate3").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
            $("#jDate4").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });


            //$("#jDate5").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });
            $("#jDate6").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });
            $("#jDate6").jqxDateTimeInput('setDate', <%=getFecha2() %>);

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
            $("#cbRFC3").jqxComboBox({ width: '460px' });


            $("#jArea").jqxTextArea({ height: 100, width: 475, maxLength: 255 });

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
                displayMember: 'RFC_EMISOR', valueMember: 'RFC_EMISOR', //ID
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



            var chkFechaPago = getData("Pag2.aspx/GetChkFechaPago");
            var diasFechaPago = getData("Pag2.aspx/GetFechaPagoBloqueo");

            $("#jDate6").on('change',
                function(event) {




                    if (chkFechaPago) {

                        var jsDate = event.args.date;
                        //var type = event.args.type; // keyboard, mouse or null depending on how the date was selected.
                        var dia = $.jqx.dataFormat.formatdate(jsDate, 'dd');


                        if (dia >= 1 && dia <= diasFechaPago) {
                            $("#message").html("<div style='margin-top:15px;'>Los días del 1 al " + diasFechaPago + " no están permitidos.</div>");
                            $("#message").dialog('open');

                            $("#jDate6").jqxDateTimeInput('setDate', <%=getFecha2() %>);
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

            $("#jGrid3").on('rowselect', function (event) {
                total3();
            });

            $("#jGrid3").bind('rowunselect', function (event) {
                total3();
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


            function total3() {
                var arr1 = $('#jGrid3').jqxGrid('getselectedrowindexes');
                var importe = 0;
                $.each(arr1,
                    function(index, value) {
                        var data = $('#jGrid3').jqxGrid('getrowdata', value);
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

            var IDgrid=-1;
        </script>
                
    <script src="Pag3/jGrid.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/jGrid2.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/jGrid3.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/jGrid4.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/jGrid6.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/bAgregar.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/bQuitar.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/bCrear.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/buscar.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/buscar2.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/btCerrarCartaP.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag3/btAbrirCartaP.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    
    
<script>



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
                






    $("#bBuscar").click(function (event) {
        buscar();
    });



    $("#bBuscar2").click(function (event) {
        buscar2();
    });


    $("#bExportar").click(function (event) {
        $('#init').removeClass('no-show');

                    
        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

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

                    
        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

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

        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

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

        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

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

        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

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

    $("#Rep5").click(function(event) {
        $('#init').removeClass('no-show');

        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

        getData("reportes.aspx/SetID",
            {
                req: {
                    rep: 5,
                    fecha1: fecha1,
                    fecha2: fecha2
                }
            });
                    
        window.open("reportes.aspx", "xml", "width=200,height=150");


        $('#init').addClass('no-show');
                    
    });

    $("#Rep6").click(function(event) {
        $('#init').removeClass('no-show');

        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate8").jqxDateTimeInput('getDate'), 'd');

        getData("reportes.aspx/SetID",
            {
                req: {
                    rep: 6,
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
        width:500,
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


    $("#CartaPago").dialog({
        autoOpen: false,
        resizable: false,
        width: 'auto',
        modal: true,
        dialogClass: 'mWin2',
        open: function (event, ui) {

        }
    });


    $("#incidencia1").dialog({
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
    // });
</script>
        

</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="contenido" runat="server">


<%
    var opt = ShowBtn();
    bool MOSTRAR_ABRIR_CARTA_PAGO = false;
    bool mostrarAgregarQuitar = false;

    if (opt != null)
    {
        MOSTRAR_ABRIR_CARTA_PAGO = opt.MOSTRAR_ABRIR_CARTA_PAGO;

    }

%>



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
                        <td rowspan="3" style="text-align: left; padding: 5px;">
							
							<div>UUID</div>
                            <div><input class="jTx" id="txUUID" type="text" value="" style="width: 197px;" /></div>
							
                            <div>Serie</div>
                            <div><input class="jTx" id="txSerie" type="text" value="" style="width: 197px;" /></div>
                        
                            <div>Folio</div>
                            <div><input class="jTx" id="txFolio" type="text" value="" style="width: 197px;" /></div>
                        
                            <div>Contrarecibo</div>
                            <div><input class="jTx" id="txContr" type="text" value="" style="width: 197px;" /></div>
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
                    </tr>
                    <tr>
                        <td style="text-align: left; padding: 5px;" colspan="2">
                             <div>Razon Social</div>
                             <div class="txCenter" id="razonSocial" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
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
                                                    <% if (opt != null && opt.MOSTRAR_AGREGAR_QUITAR_FAC) { %>
                                                        <input type="button" id="bAgregar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Agregar"/>
                                                    <% } else { %>
                                                        <input type="button" id="bAgregar" class="btn1" style="display:none; height: 30px; padding-top: 5px; font-size: 1em;" value="Agregar"/>
                                                    <% } %>
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
                                                Total: <span id="total3"></span>

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
                                    <input type="button" id="Rep5" class="btn1" style="width: 214px; height: 30px; padding-top: 5px; font-size: 1em;" value="Facturas No Validadas"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding: 5px;">
                                    <input type="button" id="Rep6" class="btn1" style="width: 192px; height: 30px; padding-top: 5px; font-size: 1em;" value="Facturas Rechazadas"/>
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
                    <div id="Confirmar_msg" style="margin-top:9px;">Quieres crear el contrarecibo de las facturas agregadas?</div>
                </div>
                
                <div id="confirmar2" title="Confirmar">
                    
                    <table style="width: 500px; margin-bottom: 5px; margin-top: 5px;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                Incidencia:
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 2px;">
                                <textarea id="jArea"></textarea>
                            </td>
                        </tr>
                    </table>

                    <div style="margin-top:9px;">Quieres quitar las facturas seleccionadas del contrarecibo?</div>
                </div>
                
                <div id="detalle" title="Detalle">
                    <div style="padding-left:1px !important;padding-right:1px !important;">
                        <div id="jGrid4" style="margin-top: 10px;"></div>
                    </div>
                    
                </div>

                <div id="detalle3" title="Detalle">
                    <div id="jGrid6" style="margin-top: 10px"></div>
                </div>

                <div id="exportar" title="Exportar">
                    <div id='chkXML' style="margin-top: 20px">Agregar XML</div>
                    <div id='chkPDF'>Agregar PDF proveedor</div>
                </div>



<!-------------------------------------------------->

<div id="CartaPago" title="Carta Pago">
    <table style="margin-bottom: 0px; margin-top: 7px;">
        <tr>
            <td style="width: 140px; padding-bottom: 10px;" align="right">
                <input type="button" id="btCerrarCartaP" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em; width: 250px;" value="Cerrar Carta Pago"/>
            </td>
        </tr>
        <%
            if (MOSTRAR_ABRIR_CARTA_PAGO)
            { %>
        <tr>
            <td style="width: 140px; padding-bottom: 10px;" align="right">
                <input type="button" id="btAbrirCartaP" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em; width: 250px;" value="Abrir Carta Pago"/>
            </td>
        </tr>
        <% } %>
    </table>
</div>


<!-------------------------------------------------->

<div id="incidencia1" title="Incidencia" style="text-align: left; margin-top: 5px;">

</div>

<!-------------------------------------------------->


<div id="prov" title="Proveedores">
    <table style="margin-top: 15px;">
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