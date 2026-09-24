<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag5.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag5" %>

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


                $(".jTx").jqxInput({ width: '99%', height: 25, theme: 'Theme2' });
                $(".jTx2").jqxInput({ height: 25, theme: 'Theme2' });

                /*******************************************************************************/


                $(".jCb").jqxDropDownList({ height: 25, width: '101.3%',searchMode:'containsignorecase',
                    promptText: '-------------------------------------------------------------------------------------------------------------------------------',
                    theme: 'Theme2'});

                $("#cbRoles").jqxDropDownList({
                    displayMember: 'ROLE_NAME', valueMember: 'ID',
                    source: getAdapter("Pag5.aspx/GetRole",{Todos:true}),
                    selectedIndex: 0,
                    dropDownHeight: 200,
                    autoDropDownHeight: auto
                });

                $(".jCb2").jqxDropDownList({ height: 25, width: '195px', searchMode: 'containsignorecase',
                    promptText: '-------------------',
                    theme: 'Theme2'
                });

                $("#cbRoles2").jqxDropDownList({
                    displayMember: 'ROLE_NAME', valueMember: 'ID',
                    source: getAdapter("Pag5.aspx/GetRole", { Todos: false }),
                    dropDownHeight: 200,
                    autoDropDownHeight: auto
                });

                $("#cbRoles2").bind('select', function (event) {
                    if (event.args && event.args.item) {
                        var record = event.args.item.originalItem;

                        if (record.ID == 1) {
                            $(".subOpt2").css('display', '');
                        } else {
                            $(".subOpt2").css('display', 'none');
                        }

                        if (record.ID == 2) {
                            $(".subOpt").css('display', '');
                        } else {
                            $(".subOpt").css('display', 'none');
                        }
                    }
                });


                $("#cbRoles3").jqxDropDownList({
                    displayMember: 'ROLE_NAME', valueMember: 'ID',
                    source: getAdapter("Pag5.aspx/GetRole", { Todos: false }),
                    selectedIndex: 0,
                    dropDownHeight: 200,
                    autoDropDownHeight: auto
                });

                $("#cbRoles3").bind('select', function (event) {
                    if (event.args && event.args.item) {
                        var record = event.args.item.originalItem;

                        
                        if (record.ID == 1) {
                            $(".subOpt2").css('display', '');
                        } else {
                            $(".subOpt2").css('display', 'none');
                        }

                        if (record.ID == 2) {
                            $(".subOpt").css('display', '');
                        } else {
                            $(".subOpt").css('display', 'none');
                        }
                    }
                });

                /*******************************************************************************/

                $("#chkRechazar1").jqxCheckBox({ height: 25 });
                $("#chkBloqueo1").jqxCheckBox({ height: 25 });
                $("#chkBloqueo3").jqxCheckBox({ height: 25 });
                $("#chkIgBloqueo3").jqxCheckBox({ height: 25 });
                $("#chkDesvalidar1").jqxCheckBox({ height: 25 });
                $("#chkFechaValidacion1").jqxCheckBox({ height: 25 });
                $("#chkAbrirCartaPago1").jqxCheckBox({ height: 25 });
                $("#chkFacDup1").jqxCheckBox({ height: 25 });


                $("#chkRechazar2").jqxCheckBox({ height: 25 });
                $("#chkBloqueo2").jqxCheckBox({ height: 25 });
                $("#chkBloqueo4").jqxCheckBox({ height: 25 });
                $("#chkIgBloqueo4").jqxCheckBox({ height: 25 });
                $("#chkDesvalidar2").jqxCheckBox({ height: 25 });
                $("#chkFechaValidacion2").jqxCheckBox({ height: 25 });
                $("#chkAbrirCartaPago2").jqxCheckBox({ height: 25 });
                $("#chkFacDup2").jqxCheckBox({ height: 25 });

                /*******************************************************************************/

                $('#init').removeClass('no-show');
                setTimeout(function () {


                    $('#init').addClass('no-show');
                }, 1000);

                /*******************************************************************************/

                /*
                var rs = getData("Pag5.aspx/GetSemana");
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


                //---------------------------------------------------------------------------
                var columnrenderer2 = function (value) {
                    return '<div style="text-align: center;margin-top: 7px;">' + value + '</div>';
                }

                source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        { name: 'USER_NAME' },
                        { name: 'PASSWORD' },
                        { name: 'ROLE' },
                        { name: 'ROLE_NAME' },
                        { name: 'DIF_TOTAL' },
                        { name: 'DIF_IMPUESTO' },
                        { name: 'EMAIL' }
                    ],
                    localdata: []
                };
                var dataAdapter = new $.jqx.dataAdapter(source);
                var dataRecord = null;

                $("#jGrid").jqxGrid({ width: '100%', height: 450,
                    source: dataAdapter,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    //selectionmode: 'checkbox',
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
                        { text: 'USUARIO',      dataField: 'USER_NAME',    width: 130, renderer: columnrenderer2 },
                        { text: 'PASSWORD',     dataField: 'PASSWORD',     width: 130, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'ROL',          dataField: 'ROLE_NAME',    width: 130, renderer: columnrenderer2 },
                        { text: 'DIF_TOTAL',    dataField: 'DIF_TOTAL',    width: 130, renderer: columnrenderer2 },
                        { text: 'DIF_IMPUESTO', dataField: 'DIF_IMPUESTO', width: 130, renderer: columnrenderer2 },

                        {
                            text: 'Modificar',
                            datafield: 'Show1',
                            columntype: 'button',
                            width: 80, 
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                if (dataRecord.ROLE == 1) {
                                    $(".subOpt2").css('display', '');
                                } else {
                                    $(".subOpt2").css('display', 'none');
                                }

                                if (dataRecord.ROLE == 2) {
                                    $(".subOpt").css('display', '');
                                } else {
                                    $(".subOpt").css('display', 'none');
                                }

                                
                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                var flag = getData("Pag5.aspx/GetFlag", {
                                    ac: dataRecord
                                });

                                if (flag.f == "1") {


                                    $("#txUser1").html(dataRecord.USER_NAME);
                                    $("#txPass1").val(dataRecord.PASSWORD);
                                    $("#txRol1").html(dataRecord.ROLE_NAME);
                                    $("#txDifTotal1").val(dataRecord.DIF_TOTAL);
                                    $("#txDifImpuesto1").val(dataRecord.DIF_IMPUESTO);

                                    $("#modificar1").dialog('open');
                                }

                                if (flag.f == "0") {


                                    $("#txDifTotal2").val(dataRecord.DIF_TOTAL);
                                    $("#txDifImpuesto2").val(dataRecord.DIF_IMPUESTO);

                                    $("#txUser2").val(dataRecord.USER_NAME);
                                    $("#txPass2").val(dataRecord.PASSWORD);
                                    $("#cbRoles2").jqxDropDownList({ selectedIndex: dataRecord.ROLE-1 });

                                    if (flag.MOSTRAR_BLOQUEO) {
                                        $("#chkBloqueo1").jqxCheckBox('check');
                                    } else {
                                        $("#chkBloqueo1").jqxCheckBox('uncheck');
                                    }

                                    if (flag.MOSTRAR_BLOQUEO2) {
                                        $("#chkBloqueo3").jqxCheckBox('check');
                                    } else {
                                        $("#chkBloqueo3").jqxCheckBox('uncheck');
                                    }

                                    if (flag.IGNORAR_BLOQUEO2) {
                                        $("#chkIgBloqueo3").jqxCheckBox('check');
                                    } else {
                                        $("#chkIgBloqueo3").jqxCheckBox('uncheck');
                                    }

                                    if (flag.MOSTRAR_RECHAZAR) {
                                        $("#chkRechazar1").jqxCheckBox('check');
                                    } else {
                                        $("#chkRechazar1").jqxCheckBox('uncheck');
                                    }

                                    if (flag.MOSTRAR_DESVALIDAR) {
                                        $("#chkDesvalidar1").jqxCheckBox('check');
                                    } else {
                                        $("#chkDesvalidar1").jqxCheckBox('uncheck');
                                    }

                                    if (flag.MOSTRAR_FECHA_VALIDACION) {
                                        $("#chkFechaValidacion1").jqxCheckBox('check');
                                    } else {
                                        $("#chkFechaValidacion1").jqxCheckBox('uncheck');
                                    }
                                    
                                    if (flag.MOSTRAR_ABRIR_CARTA_PAGO) {
                                        $("#chkAbrirCartaPago1").jqxCheckBox('check');
                                    } else {
                                        $("#chkAbrirCartaPago1").jqxCheckBox('uncheck');
                                    }

                                    if (flag.MOSTRAR_FACTURA_DUPLICADOS) {
                                        $("#chkFacDup1").jqxCheckBox('check');
                                    } else {
                                        $("#chkFacDup1").jqxCheckBox('uncheck');
                                    }

                                    $("#modificar2").dialog('open');
                                }
                                
                                
                                $('#init').addClass('no-show');

                            }
                        },

                        {
                            text: 'Eliminar',
                            datafield: 'Show2',
                            columntype: 'button',
                            width: 80,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                var flag = getData("Pag5.aspx/GetFlag", {
                                    ac: dataRecord
                                });

                                $('#init').addClass('no-show');

                                if (flag.f == "1") {
                                    $("#message").html("<div style='text-align: left;min-width:250px;margin-top:7px;'>Esta cuenta no se puede eliminar por que ya contiene facturas agregadas.</div>");
                                    $("#message").dialog('open');
                                }

                                if (flag.f == "0") {

                                    $("#confirmar").dialog({
                                        buttons: {
                                            'Eliminar': function () {

                                                $('#init').removeClass('no-show');
                                                var rs = getData("Pag5.aspx/Eliminar", { ac: dataRecord });


                                                if (rs.code == 0) {

                                                    Buscar();

                                                    $("#message").html("<div style='margin-top:15px;'>La cuenta se elimino correctamente.</div>");
                                                    $("#message").dialog('open');

                                                } else {
                                                    $("#message").html("<div style='margin-top:15px;'>" + rs.msg + "</div>");
                                                    $("#message").dialog('open');
                                                }

                                                $(this).dialog("close");
                                                $('#init').addClass('no-show');
                                            },
                                            'Cancelar': function () {

                                                $(this).dialog("close");
                                            }
                                        }
                                    });

                                    $("#confirmar").html("<div style='text-align: left;min-width:250px;margin-top:7px;'>Quieres eliminar la cuenta: <strong>" + dataRecord.USER_NAME + "</strong>?<div>");
                                    $("#confirmar").dialog('open');
                                }

                                

                            }
                        }
                    ]
                });


                //---------------------------------------------------------------------------

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


                //---------------------------------------------------------------------------
                function Buscar() {

                    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/



                    var index = $("#cbRoles").jqxDropDownList('selectedIndex');
                    var role = $("#cbRoles").jqxDropDownList('getItem', index).value;
                    var usuario = $("#txUsuario").val();


                    source.localdata = getData("Pag5.aspx/GetRegistros",
                        {
                            role: role,
                            usuario: usuario
                        });

                    dataAdapter.dataBind();

                    $("#jGrid").jqxGrid("autoresizecolumns");

                    var colDefs = $("#jGrid").jqxGrid('columns').records;
                    for (var idx = 0; idx < colDefs.length; idx++) {
                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                        }
                    }
                    $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 80);
                    $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 80);

                    //window.open("excel.aspx", "xml", "width=200,height=150");
                }



                $("#bBuscar").click(function (event) {
                    Buscar();
                });


                $("#bDifGlobal").click(function (event) {

                    var difGlobal = getData("Pag5.aspx/GetDifGlobal", {});
                    
                    $("#txDifTotal4").val(difGlobal.DIF_TOTAL);
                    $("#txDifImpuesto4").val(difGlobal.DIF_IMPUESTO);

                    $("#difGlobal").dialog('open');
                });



                $("#bAgregar").click(function (event) {
                    $("#aUser").val("");
                    $("#aPass").val("");
                    $("#chkBloqueo2").jqxCheckBox('uncheck');
                    $("#chkBloqueo4").jqxCheckBox('uncheck');
                    $("#chkIgBloqueo4").jqxCheckBox('uncheck');
                    $("#chkRechazar2").jqxCheckBox('uncheck');
                    $("#chkDesvalidar2").jqxCheckBox('uncheck');
                    $("#chkFechaValidacion2").jqxCheckBox('uncheck');
                    $("#chkAbrirCartaPago2").jqxCheckBox('uncheck');
                    $("#chkFacDup2").jqxCheckBox('uncheck');

                    $("#cuenta").dialog('open');

                    var index = $("#cbRoles3").jqxDropDownList('selectedIndex');
                    var role = $("#cbRoles3").jqxDropDownList('getItem', index).value;

                    if (role == 1) {
                        $(".subOpt2").css('display', '');
                    } else {
                        $(".subOpt2").css('display', 'none');
                    }


                    if (role == 2) {
                        $(".subOpt").css('display', '');
                    } else {
                        $(".subOpt").css('display', 'none');
                    }
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



                $("#difGlobal").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Guardar': function () {
                            /*
                            dataRecord.DIF_TOTAL = $("#txDifTotal1").val();
                            dataRecord.DIF_IMPUESTO = $("#txDifImpuesto1").val();


                            getData("Pag5.aspx/Save", {
                                ac: dataRecord
                            });

                            */


                            $("#confirmar").dialog({
                                buttons: {
                                    'Aceptar': function () {

                                        $('#init').removeClass('no-show');
                                        var rs = getData("Pag5.aspx/SaveDifGlobal",
                                            {
                                                 total: $("#txDifTotal4").val(),
                                                 impuesto: $("#txDifImpuesto4").val()
                                            });


                                        $("#difGlobal").dialog("close");
                                        $(this).dialog("close");
                                        $('#init').addClass('no-show');
                                    },
                                    'Cancelar': function () {

                                        $(this).dialog("close");
                                    }
                                }
                            });

                            $("#confirmar").html("<div style='text-align: left;min-width:250px;margin-top:7px;'>Quieres guardar la tolerancia global?<br /> <strong>TOTAL: " + $("#txDifTotal4").val() + "</strong> <br /> <strong>IMPUESTO: " + $("#txDifImpuesto4").val() + "</strong><div>");
                            $("#confirmar").dialog('open');


                        },

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



                $("#modificar1").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Guardar': function () {
                            dataRecord.PASSWORD = $("#txPass1").val();
                            dataRecord.DIF_TOTAL = $("#txDifTotal1").val();
                            dataRecord.DIF_IMPUESTO = $("#txDifImpuesto1").val();


                            getData("Pag5.aspx/Save", {
                                ac: dataRecord
                            });

                            Buscar();

                            $(this).dialog("close");
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
                    }
                });


                $("#modificar2").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Guardar': function () {
                            dataRecord.PASSWORD = $("#txPass2").val();
                            dataRecord.USER_NAME = $("#txUser2").val();

                            dataRecord.MOSTRAR_BLOQUEO = $("#chkBloqueo1").val();
                            dataRecord.MOSTRAR_BLOQUEO2 = $("#chkBloqueo3").val();
                            dataRecord.IGNORAR_BLOQUEO2 = $("#chkIgBloqueo3").val();
                            dataRecord.MOSTRAR_RECHAZAR = $("#chkRechazar1").val();
                            dataRecord.MOSTRAR_DESVALIDAR = $("#chkDesvalidar1").val();
                            dataRecord.MOSTRAR_FECHA_VALIDACION = $("#chkFechaValidacion1").val();
                            dataRecord.MOSTRAR_ABRIR_CARTA_PAGO = $("#chkAbrirCartaPago1").val();
                            dataRecord.MOSTRAR_FACTURA_DUPLICADOS = $("#chkFacDup1").val();

                            dataRecord.DIF_TOTAL = $("#txDifTotal2").val();
                            dataRecord.DIF_IMPUESTO = $("#txDifImpuesto2").val();

                            var index = $("#cbRoles2").jqxDropDownList('selectedIndex');
                            var role = $("#cbRoles2").jqxDropDownList('getItem', index).value;

                            dataRecord.ROLE = role;

                            

                            getData("Pag5.aspx/Save", {
                                ac: dataRecord
                            });

                            Buscar();

                            $(this).dialog("close");
                        },

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


                $("#cuenta").dialog({
                    autoOpen: false,
                    resizable: false,
                    width: 'auto',
                    //minWidth: 200,
                    //maxWidth: 500,
                    fluid: true, //new option
                    modal: true,
                    dialogClass: 'mWin2',
                    buttons: {
                        'Agregar': function () {

                            var cuenta = {};

                            var index = $("#cbRoles3").jqxDropDownList('selectedIndex');
                            var role = $("#cbRoles3").jqxDropDownList('getItem', index).value;

                            cuenta.USER_NAME = $("#aUser").val().trim();
                            cuenta.PASSWORD = $("#aPass").val().trim();
                            cuenta.ROLE = role;

                            cuenta.MOSTRAR_BLOQUEO = $("#chkBloqueo2").val();
                            cuenta.MOSTRAR_BLOQUEO2 = $("#chkBloqueo4").val();
                            cuenta.IGNORAR_BLOQUEO2 = $("#chkIgBloqueo4").val();
                            cuenta.MOSTRAR_RECHAZAR = $("#chkRechazar2").val();
                            cuenta.MOSTRAR_DESVALIDAR = $("#chkDesvalidar2").val();
                            cuenta.MOSTRAR_FECHA_VALIDACION = $("#chkFechaValidacion2").val();
                            cuenta.MOSTRAR_ABRIR_CARTA_PAGO = $("#chkAbrirCartaPago2").val();
                            cuenta.MOSTRAR_FACTURA_DUPLICADOS = $("#chkFacDup2").val();


                            cuenta.DIF_TOTAL = $("#txDifTotal3").val();
                            cuenta.DIF_IMPUESTO = $("#txDifImpuesto3").val();

                            //console.log(cuenta);


                            var pm = new Array();

                            if ($.trim(cuenta.USER_NAME) == "") {
                                pm.push("<strong>USUARIO</strong>");
                            }

                            if ($.trim(cuenta.PASSWORD) == "") {
                                pm.push("<strong>PASSWORD</strong>");
                            }

                            if (pm[0]) {
                                var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                                $("#message").html("<div style='text-align: left;min-width:250px;margin-top:7px;'>" + param + "</div>");
                                $("#message").dialog('open');
                                return false;
                            }


                            var rs = getData("Pag5.aspx/Agregar", {
                                ac: cuenta
                            });

                            Buscar();

                            $("#message").html("<div style='text-align: left;min-width:250px;margin-top:7px;'>" + rs.msg + "</div>");
                            $("#message").dialog('open');

                            $(this).dialog("close");
                        },

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

    <%--<div style="border: solid 1px black;padding: 0px;max-width: 99%;margin: auto" >--%>
        <div class="content">

                <div class="container full-width show-top-margin">
                    <div class="row margin-grid" style="text-align: left">
                        <div class="col-xs-6 col-sm-3 col-md-3 col-lg-2">
                            <div>Roles</div>
                            <div class="jCb" id="cbRoles"></div>
                        </div>
                        <div class="col-xs-6 col-sm-3 col-md-3 col-lg-2">
                            <div>Usuario</div>
                            <div><input class="jTx" id="txUsuario" type="text" value="" /></div>
                        </div>
                    </div>
                    
                    <table style="width: 100.1%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin-bottom: 5px;margin-top: 8px;background-color: #fbfbfb">
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <input type="button" id="bDifGlobal" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Dif Global"/>
                                        </td>
                                        <td>
                                            <input type="button" id="bAgregar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Agregar Cuenta"/>
                                        </td>
                                        <td>
                                            <input type="button" id="bBuscar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div id="jGrid" style="margin-bottom: 13px"></div>
                </div>
     
                
                <div id="message" title="Mensaje del sistema"></div>
                
        
        
                <div id="difGlobal" title="TOLERANCIA GLOBAL">
                    <table style="margin-top: 5px;">
                        <tr>
                            <td style="width: 130px;padding-right: 2px;" align="right">
                                DIF TOTAL:
                            </td>
                            <td style="width: 150px; padding: 5px;">
                                <input class="jTx2" id="txDifTotal4" type="text" value="" style="width: 150px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                DIF IMPUESTO:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifImpuesto4" type="text" value="" style="width: 150px; text-align: center;" />
                            </td>
                        </tr>
                    </table>
                </div>
        
        

                <div id="modificar1" title="Modificar">
                    <table style="margin-top: 5px;">
                        <tr>
                            <td style="width: 110px;padding-right: 2px;" align="right">
                                USUARIO:
                            </td>
                            <td style="width: 200px; padding: 5px;">
                                <div class="txCenter" id="txUser1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                PASSWORD:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txPass1" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                ROL:
                            </td>
                            <td style="padding: 5px;">
                                <div class="txCenter" id="txRol1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                DIF TOTAL:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifTotal1" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                DIF IMP.:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifImpuesto1" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                    </table>
                </div>
                               
                <div id="modificar2" title="Modificar">
                    <table style="margin-top: 5px;">
                        <tr>
                            <td style="width: 110px;padding-right: 2px;" align="right">
                                USUARIO:
                            </td>
                            <td style="width: 200px; padding: 5px;">
                                <input class="jTx2" id="txUser2" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                PASSWORD:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txPass2" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                ROL:
                            </td>
                            <td style="padding: 5px;">
                                <div class="jCb2" id="cbRoles2"></div>
                            </td>
                        </tr>
                        <tr class="subOpt2">
                            <td style="padding-right: 2px;" align="right">
                                DIF TOTAL:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifTotal2" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr class="subOpt2">
                            <td style="padding-right: 2px;" align="right">
                                DIF IMP.:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifImpuesto2" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; padding-top: 8px; font-size: 1em;" align="left">
                                <div id="chkRechazar1">Mostrar boton "Rechazar"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkBloqueo1">Mostrar boton "Fecha Bloqueo XML"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkBloqueo3">Mostrar boton "Fecha Bloqueo Albaranes"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkIgBloqueo3">Ignorar "Fecha Bloqueo Albaranes"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkDesvalidar1">Mostrar boton "Desvalidar"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkFechaValidacion1">Mostrar boton "Fecha Validación"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkAbrirCartaPago1">Mostrar boton "Abrir Carta Pago"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkFacDup1">Mostrar boton "Aceptar Duplicados"</div>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="cuenta" title="Agregar Cuenta">
                    <table style="margin-top: 5px;">
                        <tr>
                            <td style="width: 110px;padding-right: 2px;" align="right">
                                USUARIO:
                            </td>
                            <td style="width: 200px; padding: 5px;">
                                <input class="jTx2" id="aUser" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                PASSWORD:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="aPass" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 2px;" align="right">
                                ROL:
                            </td>
                            <td style="padding: 5px;">
                                <div class="jCb2" id="cbRoles3"></div>
                            </td>
                        </tr>
                        <tr class="subOpt2">
                            <td style="padding-right: 2px;" align="right">
                                DIF TOTAL:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifTotal3" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr class="subOpt2">
                            <td style="padding-right: 2px;" align="right">
                                DIF IMP.:
                            </td>
                            <td style="padding: 5px;">
                                <input class="jTx2" id="txDifImpuesto3" type="text" value="" style="width: 190px; text-align: center;" />
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; padding-top: 8px; font-size: 1em;" align="left">
                                <div id="chkRechazar2">Mostrar boton "Rechazar"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkBloqueo2">Mostrar boton "Fecha Bloqueo XML"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkBloqueo4">Mostrar boton "Fecha Bloqueo Albaranes"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkIgBloqueo4">Ignorar "Fecha Bloqueo Albaranes"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkDesvalidar2">Mostrar boton "Desvalidar"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkFechaValidacion2">Mostrar boton "Fecha Validación"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkAbrirCartaPago2">Mostrar boton "Abrir Carta Pago"</div>
                            </td>
                        </tr>
                        <tr class="subOpt">
                            <td colspan="2" style="padding-left: 12px; font-size: 1em;" align="left">
                                <div id="chkFacDup2">Mostrar boton "Aceptar Duplicados"</div>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="confirmar" title="Confirmar">
                    
                </div>
                            

        </div>
        
    <%--</div>--%>
    <asp:Label ID="Script1" runat="server"></asp:Label>
</asp:Content>