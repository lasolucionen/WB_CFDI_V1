<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag6.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>
    .a1 {
        /*border: 1px solid black;*/
    }

    .ui-tabs .ui-tabs-nav {
        margin: 0;
        padding: 0;
    }

    .ui-tabs .ui-tabs-nav .ui-tabs-anchor {
        float: left;
        padding: .4em 1em;
        text-decoration: none;
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


                //$(".jTx").jqxInput({ height: 25, theme: 'Theme2' });


                /*******************************************************************************/

                $("#jDate1").jqxDateTimeInput({ width:150, value:'<%=getFecha1() %>' , height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate2").jqxDateTimeInput({ width:150, height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                /*$(".jCb").jqxDropDownList({ height: 25, searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'});*/

                $(".jCbX").jqxComboBox({ height: 25,searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'
                });


                $("#cbProveedor").jqxComboBox({width: '400px'});


                $("#cbProveedor").bind('unselect',
                    function(event) {
                        if (event.args && event.args.item) {
                            
                            //$("#cbProveedor").jqxComboBox({ selectedIndex: -1 });
                            
                        }

                    });


                $( "#cbProveedor input" ).keyup(function() {
                    if ($(this).val() == "") {
                        $("#cbProveedor").jqxComboBox({ selectedIndex: -1 });

                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');
                    }
                });

                var prov = getAdapter("Pag6.aspx/GetProveedores", {});
                var prov2 = $.extend(true, {}, prov);

                $("#cbProveedor").jqxComboBox({
                    displayMember: 'NOMPROVEEDOR', valueMember: 'CODPROVEEDOR',
                    source: prov2,
                    dropDownHeight: 200, 
                    autoDropDownHeight: auto
                });

                $("#cbProveedor").bind('select', function (event) {
                    if (event.args && event.args.item) {
                         
                        source.localdata = [];
                        dataAdapter.dataBind();
                        $("#jGrid").jqxGrid('updatebounddata', 'cells');

                        source2.localdata = [];
                        dataAdapter2.dataBind();
                        $("#jGrid2").jqxGrid('updatebounddata', 'cells');

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
                var rs = getData("Pag6.aspx/GetSemana");
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
                //$('#jqxTabs').jqxTabs({width:'100%'});

                
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
                        { name: 'CODPROVEEDOR' },
                        { name: 'NOMPROVEEDOR' },
                        { name: 'RFC' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'TOTAL' }
                    ],
                    localdata: []
                };
                var dataAdapter = new $.jqx.dataAdapter(source);

                $("#jGrid").jqxGrid({ width: '100%', height: 450,
                    source: dataAdapter,
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
                        { text: 'CODPROVEEDOR', dataField: 'CODPROVEEDOR', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'NOMPROVEEDOR', dataField: 'NOMPROVEEDOR', width: 130, renderer: columnrenderer2 },
                        { text: 'RFC',          dataField: 'RFC',          width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'SUBTOTAL',     dataField: 'SUBTOTAL',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',    dataField: 'IMPUESTOS',    width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'TOTAL',        dataField: 'TOTAL',        width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' }

                    ]
                });



                

                //---------------------------------------------------------------------------


                //*****************************************************************************************************************

                source2 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'CODPROVEEDOR' },
                        { name: 'NOMPROVEEDOR' },
                        { name: 'RFC' },
                        { name: 'FECHA_PROCESO', type: 'date' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'TOTAL' },
                        { name: 'NUMSERIEFAC' },
                        { name: 'NUMFAC' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'NUMSERIE' },
                        { name: 'NUMALBARAN' },
                        { name: 'FECHA_ALBARAN', type: 'date' }
                    ],
                    localdata: []
                };
                var dataAdapter2 = new $.jqx.dataAdapter(source2);

                $("#jGrid2").jqxGrid({ width: '100%', height: 450,
                    source: dataAdapter2,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'singlerow',
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
                        { text: 'CODPROVEEDOR',  dataField: 'CODPROVEEDOR',  width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'NOMPROVEEDOR',  dataField: 'NOMPROVEEDOR',  width: 130, renderer: columnrenderer2 },
                        { text: 'RFC',           dataField: 'RFC',           width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA_PROCESO', dataField: 'FECHA_PROCESO', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'SUBTOTAL',      dataField: 'SUBTOTAL',      width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',     dataField: 'IMPUESTOS',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'TOTAL',         dataField: 'TOTAL',         width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'NUMSERIEFAC',   dataField: 'NUMSERIEFAC',   width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'NUMFAC',        dataField: 'NUMFAC',        width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA_FACTURA', dataField: 'FECHA_FACTURA', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'NUMSERIE',      dataField: 'NUMSERIE',      width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'NUMALBARAN',    dataField: 'NUMALBARAN',    width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA_ALBARAN', dataField: 'FECHA_ALBARAN', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' }
                    ]
                });

                //*****************************************************************************************************************

                

                //---------------------------------------------------------------------------


                $("#jGrid").bind('bindingcomplete', function () {
                    //$('#jGrid').jqxGrid('autoresizecolumns');

                    //console.debug(column / 2);

                    $("#jGrid .jqx-grid-column-header.jqx-grid-column-header-Theme2.jqx-widget-header.jqx-widget-header-Theme2 .iconscontainer").each(function () {
                        $(this).next().remove();
                    });

                });


                $("#jGrid2").bind('bindingcomplete', function () {
                    //$('#jGrid').jqxGrid('autoresizecolumns');

                    //console.debug(column / 2);

                    $("#jGrid2 .jqx-grid-column-header.jqx-grid-column-header-Theme2.jqx-widget-header.jqx-widget-header-Theme2 .iconscontainer").each(function () {
                        $(this).next().remove();
                    });

                });


                //---------------------------------------------------------------------------


                //----------------------------------------------------------------------------//


                //---------------------------------------------------------------------------
                $("#bExportar").click(function (event) {
                    //$("#exportar").dialog('open');

                    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');

                    var index = $("#cbProveedor").jqxComboBox('selectedIndex');
                    var cod = 0;


                    if (index != -1) {

                        cod = $("#cbProveedor").jqxComboBox('getItem', index).value;
                    }


                    $('#init').removeClass('no-show');


                    //var tab = $("#tabs").tabs("option", "active");



                    getData("reporte1.aspx/SetID",
                        {
                            req: {
                                FECHAINICIAL: fecha1,
                                FECHAFINAL: fecha2,
                                CODPROVEEDOR: cod,

                                FECHAINICIALD: fecha1,
                                FECHAFINALD: fecha2,
                                CODPROVEEDORD: cod
                            }
                        });

                        window.open("reporte1.aspx", "xml", "width=200,height=150");

                    

                    $('#init').addClass('no-show');


                    

                });

                $("#bBuscar").click(function (event) {
                    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

                    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');

                    var index   = $("#cbProveedor").jqxComboBox('selectedIndex');
                    var cod = 0;


                    if (index != -1) {
                        
                        cod = $("#cbProveedor").jqxComboBox('getItem', index).value;
                    }


                    $('#init').removeClass('no-show');
                    setTimeout(function () {

                        //var tab = $("#jqxTabs").jqxTabs('val');
                        var tab = $("#tabs").tabs("option", "active");

                        //if (tab == 0) {

                        //$("#tabs").tabs();

                            var data1 = getData("Pag6.aspx/GetResumenCompras",
                                {
                                    req: {
                                        FECHAINICIAL: fecha1,
                                          FECHAFINAL: fecha2,
                                        CODPROVEEDOR: cod
                                    }
                                });

                            source.localdata = data1;
                            dataAdapter.dataBind();

                            $("#jGrid").jqxGrid('clearselection');
                            //$("#jGrid").jqxGrid('updatebounddata', 'cells');

                            $("#jGrid").jqxGrid("autoresizecolumns");

                            var colDefs = $("#jGrid").jqxGrid('columns').records;
                            for (var idx = 0; idx < colDefs.length; idx++) {
                                if (colDefs[idx].datafield != "_checkboxcolumn") {
                                    $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                                }

                            }

                        //} else {

                            var data2 = getData("Pag6.aspx/GetDetalleCompras",
                                {
                                    req: {
                                        FECHAINICIALD: fecha1,
                                          FECHAFINALD: fecha2,
                                        CODPROVEEDORD: cod
                                    }
                                });

                            source2.localdata = data2;
                            dataAdapter2.dataBind();

                            $("#jGrid2").jqxGrid('clearselection');
                            //$("#jGrid2").jqxGrid('updatebounddata', 'cells');

                            $("#jGrid2").jqxGrid("autoresizecolumns");

                            var colDefs = $("#jGrid2").jqxGrid('columns').records;
                            for (var idx = 0; idx < colDefs.length; idx++) {
                                if (colDefs[idx].datafield != "_checkboxcolumn") {
                                    $("#jGrid2").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                                }

                            }


                            //$("#jGrid").jqxGrid({ width: 900 });
                            //$("#jGrid2").jqxGrid({ width: 900 });

                            //$("#jGrid").jqxGrid({ width: '100%' });
                            //$("#jGrid2").jqxGrid({ width: '100%' });

                        //}

                        
                        $('#init').addClass('no-show');

                    }, 30);



                });

                //UploadRW();
                //---------------------------------------------------------------------------


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


                
                $("#tabs").tabs({
                    activate: function(event, ui) {

                        //$("#jGrid").jqxGrid({ width: 100 });
                        //$("#jGrid2").jqxGrid({ width: 100 });

                        //$("#jGrid").jqxGrid({ width: '100%' });
                        //$("#jGrid2").jqxGrid({ width: '100%' });


                    }
                });

                $("#tabs").on("tabsbeforeactivate", function (event, ui) {

                    $("#jGrid").jqxGrid({ width: 100 });
                    $("#jGrid2").jqxGrid({ width: 100 });

                    $("#jGrid").jqxGrid({ width: '100%' });
                    $("#jGrid2").jqxGrid({ width: '100%' });

                    $(window).triggerHandler("resize");
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
                        <td class="a1" style="text-align: left; padding: 5px; padding-top: 5px; padding-bottom: 0px; padding-left: 5px; width: 340px;" valign="top">
                            <div>Proveedores</div>
                            <div class="jCbX" id="cbProveedor"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; padding-top: 5px; width: 145px;">
                            <div>Fecha Inicial</div>
                            <div id="jDate1"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; padding-top: 5px; width: 145px;">
                            <div>Fecha Final</div>
                            <div id="jDate2"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; padding-bottom: 0px; padding-top: 15px;" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="a1" colspan="4" style="padding: 5px;">
                            
                            
                            <table style="width: 100%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin: auto;background-color: #fbfbfb">
                                <tr>
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

                        </td>
                    </tr>
                    <tr>
                        <td class="a1" colspan="4" style="padding: 5px;">
                            <div id="tabs">
                                <ul>
                                    <li><a href="#tabs-1">Resumen De Compras Por Periodo</a></li>
                                    <li><a href="#tabs-2">Detalle De Compras Por Periodo Por Proveedor</a></li>

                                </ul>
                                <div id="tabs-1" style="padding: 5px;">
                                    <div id="jGrid" style="margin-bottom: 0px"></div>
                                </div>
                                <div id="tabs-2" style="padding: 5px;">
                                     <div id="jGrid2" style="margin-bottom: 0px"></div>
                                </div>

                            </div>

                        </td>
                    </tr>


                </table>
                
                
                
                
            </td>
        </tr>


    </table>
	


    <%--<div style="border: solid 1px black;padding: 0px;max-width: 99%;margin: auto" >--%>
        <div class="content">


               
                
                
                <div id="message" title="Mensaje del sistema"></div>
                <div id="detalle" title="Mensaje del sistema"></div>
                
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
        </div>
        
    <%--</div>--%>
    <asp:Label ID="Script1" runat="server"></asp:Label>
</asp:Content>