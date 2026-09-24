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
            var tmpContr = "";

            var source2 = {};
            var source3 = {};
            var source4 = {};
            var source5 = {};
            var source6 = {};
            var source7 = {};
            var source8 = {};
            var source9 = {};
            var source10 = {};
            var source11 = {};
            var source17 = {};

            var tmpData1 = {};
            var tmpData2 = {};

            var DIF_TOTAL = 0;
            var DIF_IMPUESTO = 0;

            var _chkAlb = false;

            var _inx1 = {};
            var _inx2 = {};

            var _ajuste = 0;

            //$(document).ready(function () {

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
                function isEmpty(value) {
                    //return (value == null || value.length === 0);
                    return (value == null || $.trim(value) === '');
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


                //$(".jTx").jqxInput({ width: '100%', height: 25, theme: 'Theme2' });
                $(".jTx").jqxInput({height: 23, theme: 'Theme2' });
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

                $("#jDate9").jqxDateTimeInput({value:'<%=getFecha1() %>' , /*width: '101.3%',*/ height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate10").jqxDateTimeInput({ /*width: '101.3%',*/ height: '25px', theme: 'Theme2',dropDownHorizontalAlignment: 'right',culture: 'es-ES' });

                $("#jBloqueo").jqxDateTimeInput({width: '150px', height: '25px', theme: 'Theme2' ,culture: 'es', textAlign:'center'});
                $("#jBloqueo2").jqxDateTimeInput({width: '150px', height: '25px', theme: 'Theme2' ,culture: 'es', textAlign:'center'});
                $("#jValidacion").jqxDateTimeInput({min: <%=getFecha2() %>, width: '150px', height: '25px', theme: 'Theme2' ,culture: 'es', textAlign:'center'});
                $("#jLimite").jqxDateTimeInput({formatString: 'dd', width: '150px', height: '25px', theme: 'Theme2' ,culture: 'es', textAlign:'center'});

                $("#jArea").jqxTextArea({ height: 100, width: 400, maxLength: 255 });

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
                    displayMember: 'RFC_EMISOR', valueMember: 'RFC_EMISOR', //ID
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

                $("#cbDias").jqxDropDownList({
                    displayMember: 'VALOR', valueMember: 'VALOR',
                    source: [{VALOR:1},{VALOR:2},{VALOR:3},{VALOR:4},{VALOR:5},{VALOR:6},{VALOR:7}],
                    dropDownHeight: 174, 
                    autoDropDownHeight: auto,
                    width: '50px',
                    selectedIndex:0

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


                        source17.localdata = [];
                        dataAdapter17.dataBind();


                        _chkAlb = true;
                        $("#jGrid17").jqxGrid('ensurerowvisible', 0);
                        $("#jGrid17").jqxGrid('clearselection');
                        $("#serieFolio").html("SERIE - FOLIO");
                        $("#chkAlb").jqxCheckBox('uncheck');
                        _chkAlb = false;

                        DIF_IMPUESTO = record.DIF_IMPUESTO;
                        DIF_TOTAL = record.DIF_TOTAL;

                        
                    }
                });



                $("#cbEstatus").bind('select', function (event) {
                    if (event.args && event.args.item) {

                        if (event.args.item.value == 3) {
                            //$("#bAccion").button("disable");
                            $("#bAccion").val("Aceptar");
                        } else {
                            //$("#bAccion").button("enable");
                            $("#bAccion").val("Rechazar");
                        }


                      /*  if (event.args.item.value == 2) {
                            $("#bDesvalidar").button("enable");
                        } else {
                            $("#bDesvalidar").button("disable");
                        }*/

                    }
                });




                var chk1 = true;
                var chk2 = true;
                /*******************************************************************************/

                <%
                var opt = ShowBtn();


                if (opt != null && opt.MOSTRAR_FACTURA_DUPLICADOS)
                { %>

                $("#chkFacDup").jqxCheckBox({ width: 180, height: 25 });


                var chk = getData("Pag2.aspx/GetChkFacDup");

                chk2 = false;
                $("#chkFacDup").jqxCheckBox({ checked: chk });
                chk2 = true;




                $("#chkFacDup").on('change', function (event) {
                    if (chk2) {
                        var checked = event.args.checked;


                        $('#init').removeClass('no-show');
                        setTimeout(function() {

                            var rs = getData("Pag2.aspx/SetChkFacDup",
                                {
                                    chk: checked
                                });

                                                      
                            $('#init').addClass('no-show');
                        },30);
                        

                    }
                });


                <%} %>

                $("#chkXML").jqxCheckBox({ width: 180, height: 25 });
                $("#chkPDF").jqxCheckBox({ width: 180, height: 25 });
                $("#chkFechaValidacion").jqxCheckBox({width: 168 });
                $("#chkFechaLimite").jqxCheckBox({width: 168 });
                $("#chkFechaPago").jqxCheckBox({width: 158 });
                /*******************************************************************************/
                
                $("#chkFechaLimite").on('change', function (event) {
                    if (chk1) {
                        var checked = event.args.checked;


                        $('#init').removeClass('no-show');
                        setTimeout(function() {

                            var rs = getData("Pag2.aspx/SetChkFechaLimite",
                                {
                                    chk: checked
                                });


                            if (!checked) {
                                var fecha1 = getData("Pag2.aspx/GetFechaLimite");
                                $("#fLimite").html(fecha1);
                            }
                            
                                                       
                            $('#init').addClass('no-show');
                        },30);
                        

                    }
                });     

                /*******************************************************************************/
                
                $("#chkFechaValidacion").on('change', function (event) {
                    if (chk1) {
                        var checked = event.args.checked;


                        $('#init').removeClass('no-show');
                        setTimeout(function() {

                            var rs = getData("Pag2.aspx/SetChkFechaValidacion",
                                {
                                    chk: checked
                                });

                            if (!checked) {
                                var fecha1 = getData("Pag2.aspx/GetFechaValidacionStr");
                                $("#fValidacion").html(fecha1);
                            }

                                                       
                            $('#init').addClass('no-show');
                        },30);
                        

                    }
                });

                /*******************************************************************************/
                
                $("#chkFechaPago").on('change', function (event) {
                    if (chk1) {
                        var checked = event.args.checked;


                        $('#init').removeClass('no-show');
                        setTimeout(function() {

                            var rs = getData("Pag2.aspx/SetChkFechaPago",
                                {
                                    chk: checked
                                });

                                                       
                            $('#init').addClass('no-show');
                        },30);
                        

                    }
                });

                //******************************************************************************/

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

                    total1();

                    if (Array.isArray(rowindex)) {
                        if (rowindex.length > 0) {
                            if (arr1.length > 1) {

                                //$("#jGrid").jqxGrid('clearselection');
                                //$('#jGrid').jqxGrid('updatebounddata');
                            } else {
                                //total1();
                            }
                        } else {
                            //total1();
                        }
                    } else {

                        if (arr1.length > 1) {

                            //var prev = arr1[0];
                            //$("#jGrid").jqxGrid('clearselection');
                            //$('#jGrid').jqxGrid('selectrow', prev);




                            //$('#jGrid2').jqxGrid('unselectrow', row);
                            //$('#jGrid').jqxGrid('selectrow', row);
                        } else {
                            //total1();
                        }
                    }

//                    if (Array.isArray(rowindex)) {
//                        if (rowindex.length > 0) {
//                            if (arr1.length > 1 && arr2.length > 1) {

//                                $("#jGrid").jqxGrid('clearselection');
//                                $('#jGrid').jqxGrid('updatebounddata');
//                            } else {
//                                total1();
//                            }
//                        } else {
//                            total1();
//                        }
//                    } else {

//                        if (arr1.length > 1 && arr2.length > 1) {

//                            var prev = arr1[0];
//                            $("#jGrid").jqxGrid('clearselection');
//                            $('#jGrid').jqxGrid('selectrow', prev);
//                            //$('#jGrid2').jqxGrid('unselectrow', row);
//                            //$('#jGrid').jqxGrid('selectrow', row);
//                        } else {
//                            total1();
//                        }
//                    }
                });

                $("#jGrid").bind('rowunselect', function (event) {
                    total1();
                });

                $("#jGrid2").on('rowselect', function (event) {
                    var rowindex = event.args.rowindex;
                    var arr1 = $('#jGrid').jqxGrid('getselectedrowindexes');
                    var arr2 = $('#jGrid2').jqxGrid('getselectedrowindexes');

                    total2();

                    if (Array.isArray(rowindex)) {
                        if (rowindex.length > 0) {
                            if (arr1.length > 1 && arr2.length > 1) {

                                //$("#jGrid2").jqxGrid('clearselection');
                                //$('#jGrid2').jqxGrid('updatebounddata');
                            } else {
                                //total2();
                            }
                        } else {
                            //total2();
                        }
                    } else {

                        if (arr1.length > 1 && arr2.length > 1) {

                            //var prev = arr2[0];
                            //$("#jGrid2").jqxGrid('clearselection');
                            //$('#jGrid2').jqxGrid('selectrow', prev);
                            //$('#jGrid2').jqxGrid('unselectrow', row);
                            //$('#jGrid').jqxGrid('selectrow', row);
                        } else {

                            //total2();
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



                    if (tmpContr != "") {

                        //if (arr1.length > 1) {
                            $("#bDesvalidar").button("disable");
                        //} 
                        
                        if(arr1.length == 0) {
                            var rows = $('#jGrid').jqxGrid('getrows');
                            var asociadas = rows.filter(function(x) {
                                //console.log(x.ESTATUS_ID);
                                return x.ESTATUS_ID == "2";
                            });

                            if (!$.isEmptyObject(asociadas)) {
                                //console.table(asociadas,["ID","ESTATUS_ID"]);
                                $("#bDesvalidar").button("enable");
                            }
                        }

                        if (arr1.length == 1) {
                            var data = $('#jGrid').jqxGrid('getrowdata', arr1[0]);
                            if (data.ESTATUS_ID == 2) {
                                $("#bDesvalidar").button("enable");
                            }
                        } 

                    }
                }

                function total2() {
                    var arr2 = $('#jGrid2').jqxGrid('getselectedrowindexes');
                    var importe = 0;
                    $.each(arr2,
                        function(index, value) {
                            var data = $('#jGrid2').jqxGrid('getrowdata', value);
                            importe += numeral(data.TOTAL).value();
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


                var cellB=function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

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


                var cellC=function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

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
                        return '<div style="margin: 4px; margin-top:4px; text-align: ' +
                            columnproperties.cellsalign +
                            '; color:red;">' +
                            formattedValue +
                            '</div>';
                    } else {
                        return '<div style="margin: 4px; margin-top:4px; text-align: ' +
                            columnproperties.cellsalign +
                            '; color:red;">' +
                            formattedValue +
                            '</div>';
                    }


                }



                var cellclass = function(row, columnfield, value) {

                    var data = $("#jGrid").jqxGrid('getrowdata', row);

                    if (data.BLOQ == true) {
                        return 'Bloq';
                    }
                };

                var cellclass2 = function(row, columnfield, value) {

                    var data = $("#jGrid2").jqxGrid('getrowdata', row);

                    if (data.BLOQ == true) {
                        return 'Bloq';
                    }
                };


                var cellclass3 = function(row, columnfield, value, data) {


                    if (data.COLOR == 1) {
                        return 'cellYellow';
                    }
                };


                var cellclass4 = function(row, columnfield, value, data) {


                    if (data.COLOR2 == 1) {
                        return 'cellOrange';
                    }
                };

                var IDgrid = -1;

        </script>
    
    <script src="Pag2/jGrid.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid5.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid2.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid3.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid4.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid6.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid7.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid8.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid9.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid10.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid11.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid12.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid13.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid14.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid15.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid16.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/jGrid17.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/AgregarAjuste.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/UploadFile.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/UploadFile2.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bDesvalidar.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bAccion.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bProcesar.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/buscar.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bBloqueo2.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bBloqueo.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bFechaValidacion1.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/bFechaLimite1.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/fechaPagoBloqueo.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
    <script src="Pag2/dialog.js?v=<%=DateTime.Now.ToString("ddMMyyyHHmmss") %>"></script>
                <script>
//*****************************************************************************************************************




source16.localdata = getData("Pag2.aspx/tAjuste", {});

dataAdapter16.dataBind();


$("#jGrid16").jqxGrid("autoresizecolumns");

var colDefs = $("#jGrid16").jqxGrid('columns').records;
for (var idx = 0; idx < colDefs.length; idx++) {

    if (colDefs[idx].datafield != "_checkboxcolumn") {
        $("#jGrid16").jqxGrid('setcolumnproperty',
            colDefs[idx].datafield,
            'width',
            colDefs[idx].width + 5);
    }
}

/*******************************************************************************/


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



                //**************************************************************************************************



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



                //---------------------------------------------------------------------------

        



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

                    var fecha1 = $.jqx.dataFormat.formatdate( $("#jDate3").jqxDateTimeInput('getDate') , 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate( $("#jDate4").jqxDateTimeInput('getDate') , 'd');



                    source17.localdata = [];
                    dataAdapter17.dataBind();

                    _chkAlb = true;
                    $("#jGrid17").jqxGrid('ensurerowvisible', 0);
                    $("#jGrid17").jqxGrid('clearselection');
                    $("#serieFolio").html("SERIE - FOLIO");
                    $("#chkAlb").jqxCheckBox('uncheck');
                    _chkAlb = false;



                    var index  = $("#cbSerie").jqxDropDownList('selectedIndex');

                    var serie = "";
                    if (index != -1) {
                        serie = $("#cbSerie").jqxDropDownList('getItem', index).value;    
                    }

                    var rs = getData("Pag2.aspx/GetCompras",{ 
                        rfc: $("#rfc").html(),
                        fecha1: fecha1,
                        fecha2: fecha2,
                        serie: serie
                    });


                    source2.localdata = rs.rs1;


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

                $("#bBuscar4").click(function (event) {
                    buscar4();
                });

                //----------------------------------------------------------------------------//


                function buscar4() {


                    $('#init').removeClass('no-show');

                    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate9").jqxDateTimeInput('getDate'), 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate10").jqxDateTimeInput('getDate'), 'd');



                    //var index = $("#cbSerie").jqxDropDownList('selectedIndex');

                    //var serie = "";
                    //if (index != -1) {
                    //    serie = $("#cbSerie").jqxDropDownList('getItem', index).value;
                    //}

                    source12.localdata = getData("Pag2.aspx/GetCompras2", {
                        //rfc: $("#rfc").html(),
                        fecha1: fecha1,
                        fecha2: fecha2,
                        //serie: serie
                        NUMSERIE:$("#numserie1").val(),
                        NUMALBARAN:$("#numalbaran1").val(),
                        SUALBARAN:$("#sualbaran1").val(),
                    });


                    dataAdapter12.dataBind();
                    $("#jGrid12").jqxGrid('clearselection');
                    $("#jGrid12").jqxGrid('updatebounddata', 'cells');
                    $("#jGrid12").jqxGrid('ensurerowvisible', 0);
                    $("#jGrid12").jqxGrid("autoresizecolumns");

                    var colDefs = $("#jGrid12").jqxGrid('columns').records;
                    for (var idx = 0; idx < colDefs.length; idx++) {
                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                            $("#jGrid12").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                        }
                    }
                    $("#jGrid12").jqxGrid('setcolumnproperty', "detalle", 'width', 60);



                    $('#init').addClass('no-show');
                }


                //----------------------------------------------------------------------------//



                function buscar3()
                {
                    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

                    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate5").jqxDateTimeInput('getDate'), 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate6").jqxDateTimeInput('getDate'), 'd');

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



                $("#btAjuste").click(function(event) {
                    $("#tipoAjuste").dialog('open');
                    position('.mWin');
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


                $("#bGlobal").click(function () {
                    $("#bsglobal").dialog('open');
                    $("#bsglobal").dialog('widget').position({ my: 'center', at: 'center', of: 'body' });
                    
                });


                $("#bConfiguracion").click(function () {
                    $("#config").dialog('open');
                    position('.mWin2');
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

                $("#bDesvalidar").button("disable");

                $('input[type="file"]').attr('title', window.webkitURL ? ' ' : '');

                
                $(".mWin2").css('z-index', 999);
                $(".mWin3").css('z-index', 999);
                $(".mWin").css('z-index', 999);
                $(".zindex").css('z-index', 999);

                $(".ui-dialog-content").css("padding-top", 0);
                $(".ui-dialog-content").css("padding-left", 6);
                $(".ui-dialog-content").css("padding-right", 6);
                $(".ui-dialog-content").css("padding-bottom", 5);

                //$("#cbRFC").jqxComboBox({selectedIndex: 0});
                //$("#jDate1").jqxDateTimeInput({value:new Date(2010, 0, 1) ,height: '25px', theme: 'Theme2' ,culture: 'es'});
                //buscar();
            //});
                </script>
    

        

</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="contenido" runat="server">

<%
    var opt = ShowBtn();
    bool MOSTRAR_BLOQUEO = false;
    bool MOSTRAR_BLOQUEO2 = false;
    bool MOSTRAR_RECHAZAR = false;
    bool MOSTRAR_DESVALIDAR = false;
    bool MOSTRAR_FECHA_VALIDACION = false;
    bool MOSTRAR_FACTURA_DUPLICADOS = false;
    bool MOSTRAR_FECHA_PAGO_BLOQUEO = false;
    bool MOSTRAR_REC_FACTURA_BLOQUEO = false;

    if (opt != null)
    {
        MOSTRAR_BLOQUEO = opt.MOSTRAR_BLOQUEO;
        MOSTRAR_BLOQUEO2 = opt.MOSTRAR_BLOQUEO2;
        MOSTRAR_RECHAZAR = opt.MOSTRAR_RECHAZAR;
        MOSTRAR_DESVALIDAR = opt.MOSTRAR_DESVALIDAR;
        MOSTRAR_FECHA_VALIDACION = opt.MOSTRAR_FECHA_VALIDACION;
        MOSTRAR_FACTURA_DUPLICADOS = opt.MOSTRAR_FACTURA_DUPLICADOS;
        MOSTRAR_FECHA_PAGO_BLOQUEO = opt.MOSTRAR_FECHA_PAGO_BLOQUEO;
        MOSTRAR_REC_FACTURA_BLOQUEO = opt.MOSTRAR_REC_FACTURA_BLOQUEO;
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
                                                    <%
                                                        if (MOSTRAR_DESVALIDAR)
                                                        { %>
                                                        <input type="button" id="bDesvalidar" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Desvalidar"/>
                                                    <% } %>
                                                </td>
                                                <td>
                                                    <%
                                                        if (MOSTRAR_RECHAZAR)
                                                        { %>
                                                        <input type="button" id="bAccion" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Rechazar"/>
                                                    <% } %>
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
                        <td style="padding: 5px; padding-top: 3px;" align="left">
                            
                            <%
                                if (MOSTRAR_FACTURA_DUPLICADOS)
                                { %>

                                <div id='chkFacDup' style="margin-top: 0px">Aceptar duplicados</div>
                            <% } %>

                            <input type="button" id="bCarga" class="btn1" style="width:130px;height: 30px; padding-top: 5px; font-size: 1em;" value="Importar XML"/>

                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px;padding-left: 5px; padding-right: 5px;" align="left">

                            <input type="button" id="bConfiguracion" class="btn1" style="width: 203px; height: 30px; padding-top: 5px; font-size: 1em;" value="Configuración"/>
                            

                        </td>
                        <td style="height: 30px;padding-left: 5px; padding-right: 5px;" align="left">

                            <input type="button" id="bGlobal" class="btn1" style="width: 203px; height: 30px; padding-top: 5px; font-size: 1em;" value="Busqueda Global"/>

                        </td>
                        <td style="height: 30px;padding-left: 5px; padding-right: 5px;" align="left">

                            

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
                                                <td style="padding-left: 5px;">
                                                    
                                                    <%=BuscarPorArticulo2 %>
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
                    <div id="detalle4" title="Detalle">
                        <div id="jGrid7" style="margin-bottom: 0px;margin-top: 7px"></div>
                    </div>
                        
                        
                    <div id="rechazar" title="Rechazar" style="text-align: left;">
                        <div style="padding-top: 5px; padding-bottom: 5px;">Quieres rechazar las facturas seleccionadas?</div>
                        Observación:

                        <textarea id="jArea"></textarea>
                    </div>
                        
                        
                    <div id="observacion1" title="Observación" style="text-align: left; margin-top: 5px;">

                    </div>
                    
                    <div id="incidencia1" title="Incidencia" style="text-align: left; margin-top: 5px;">

                    </div>
                        
                    <%----%>
                    
                    <table style="width: 100%; border: 1px #b4b2b2 solid; margin-top: 5px;">
                        <tr>
                            <td style="border: 1px #b4b2b2 solid;width: 691px" valign="top">
                                <div id="jGrid3" style="margin-bottom: 0px"></div>
                            </td>
                            <td style="width: 890px;" valign="top">
                                <div id="jGrid4" style="margin-bottom: 0px"></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="" align="right">
                                <table style="width: 1180px" cellpadding="3">
                                    <tr>
                                        <td style="width: 70px;" align="right">
                                            Ajustes
                                        </td>
                                        <td style="width: 200px;padding-right:100px;">
                                            <div class="txCenter" id="tAjustes" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;">0</div>
                                        </td>
                                        

                                        <td style="width: 155px;" align="right">
                                            Total Factura
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tTotal1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                        <td align="right">
                                            Total Compra
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tTotal2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            
                                        </td>
                                        

                                        <td style="width: 155px;" align="right">
                                            SubTotal Factura
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tSubTotal1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                        <td align="right">
                                            SubTotal Compra
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tSubTotal2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            
                                        </td>

                                        <td style="width: 155px;" align="right">
                                            IVA Factura
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tIVA1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                        <td align="right">
                                            IVA Compra
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tIVA2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            
                                        </td>

                                        <td style="width: 155px;" align="right">
                                            IEPS Factura
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tIEPS1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                                        </td>
                                        <td align="right">
                                            IEPS Compra
                                        </td>
                                        <td style="width: 240px;">
                                            <div class="txCenter" id="tIEPS2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
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
                        <td colspan="6">
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
                            <td style="width: 120px" align="left">IVA</td>
                            <td style="width: 120px" align="left">IEPS</td>
                            <td style="width: 140px" align="left">SUBTOTAL</td>
                            <td style="width: 140px" align="left">TOTAL</td>
                            <td style="width: 120px" align="left">DESCUENTO</td>
                            <td></td>
                        </tr>

                        <tr>
                            <td align="left">
                                <div class="txCenter" id="cIVA2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cIEPS2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cSubTotal2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cTotal2" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
                            </td>
                            <td align="left">
                                <div class="txCenter" id="cDescuento" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;"></div>
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

<div id="bloqueo" title="Fecha Bloqueo XML">
    <table style="margin-bottom: 0px;margin-top: 7px">
        <tr>
            <td style="width: 125px; padding-bottom: 10px;" align="right">
                Fecha Bloqueo:
            </td>
            <td style="padding-bottom: 10px;">
                <div class="txCenter" id="fBloqueo" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; width: 100% !important;"></div>
            </td>
        </tr>
        <tr>
            <td align="right">
                Fecha:
            </td>
            <td>
                <div id="jBloqueo" style="width: 160px !important;"></div>
            </td>
        </tr>
    </table>
</div>


<!-------------------------------------------------->

<div id="fechaRecLimite" title="Fecha Recepción Facturas">
    <table style="margin-bottom: 0px; margin-top: 7px;">
        <tr>
            <td style="width: 190px; padding-bottom: 10px;" align="right">
                Día Limite de cada mes:
            </td>
            <td style="padding-bottom: 10px;">
                <div class="txCenter" id="fLimite" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; width: 100% !important;"></div>
            </td>
        </tr>
        <tr>
            <td style="" align="right">
                Día:
            </td>
            <td>
                <div id="jLimite" style="width: 160px !important;"></div>
            </td>
        </tr>
        <tr>
           
            <td colspan="2" align="right">
                <div id='chkFechaLimite' style="margin-top: 15px">Usar Día Limite</div>
            </td>
        </tr>
    </table>
</div>


<!-------------------------------------------------->

<div id="fechaValidacion" title="Fecha Validación">
    <table style="margin-bottom: 0px; margin-top: 7px;">
        <tr>
            <td style="width: 140px; padding-bottom: 10px;" align="right">
                Fecha Validación:
            </td>
            <td style="padding-bottom: 10px;">
                <div class="txCenter" id="fValidacion" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; width: 100% !important;"></div>
            </td>
        </tr>
        <tr>
            <td style="" align="right">
                Fecha:
            </td>
            <td>
                <div id="jValidacion" style="width: 160px !important;"></div>
            </td>
        </tr>
        <tr>
           
            <td colspan="2" align="right">
                <div id='chkFechaValidacion' style="margin-top: 15px">Usar Fecha Validación</div>
            </td>
        </tr>
    </table>
</div>

<!-------------------------------------------------->

<div id="fechaPagoBloqueo" title="Fecha Pago Bloqueo">
    <table style="margin-bottom: 0px; margin-top: 7px;">
        <tr>
            <td style="padding-bottom: 10px;" align="right">
                Días Bloqueo:
            </td>
            <td style="width: 50px; padding-bottom: 10px;">
                <div class="txCenter" id="fdias" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; width: 100% !important;"></div>
            </td>
        </tr>
        <tr>
            <td style="" align="right">
                Días:
            </td>
            <td style="width: 50px;">
                <div class="jCb" id="cbDias"></div>
            </td>
        </tr>
        <tr>
           
            <td colspan="2" align="right">
                <div id='chkFechaPago' style="margin-top: 15px">Usar Días Bloqueo</div>
            </td>
        </tr>
    </table>
</div>




<!-------------------------------------------------->

<div id="config" title="Configuración">
    <table style="margin-bottom: 0px;margin-top: 7px">
        <% if (MOSTRAR_BLOQUEO){ %>
        <tr>
            <td style="padding-bottom: 10px;">
                <input type="button" id="bBloqueo" class="btn1" style="width: 220px; height: 30px; padding-top: 5px; font-size: 1em;" value="Fecha Bloqueo XML"/>
            </td>
        </tr>
        <% } %>
        
        <% if (MOSTRAR_BLOQUEO2)
           { %>
            <tr>
                <td style="padding-bottom: 10px;">
                    <input type="button" id="bBloqueo2" class="btn1" style="width: 220px; height: 30px; padding-top: 5px; font-size: 1em;" value="Fecha Bloqueo Albaranes"/>
                </td>
            </tr>
        <% } %>
        
      
        <% if (MOSTRAR_FECHA_VALIDACION){ %>
        <tr>
            <td style="padding-bottom: 10px;">
                <input type="button" id="bFechaValidacion1" class="btn1" style="width: 220px; height: 30px; padding-top: 5px; font-size: 1em; padding-left: 8px;" value="Fecha Validación"/>
            </td>
        </tr>
        <% } %>
        
        
        <% if (MOSTRAR_FECHA_PAGO_BLOQUEO){ %>
            <tr>
                <td style="padding-bottom: 10px;">
                    <input type="button" id="bFechaPagoBloqueo" class="btn1" style="width: 220px; height: 30px; padding-top: 5px; font-size: 1em; padding-left: 8px;" value="Fecha Pago Bloqueo"/>
                </td>
            </tr>
        <% } %>        
        
        <% if (MOSTRAR_REC_FACTURA_BLOQUEO){ %>
            <tr>
                <td style="padding-bottom: 10px;">
                    <input type="button" id="bFechaLimite1" class="btn1" style="width: 220px; height: 30px; padding-top: 5px; font-size: 1em; padding-left: 8px;" value="Recepción Factura Bloqueo"/>
                </td>
            </tr>
        <% } %>
    </table>
</div>
<!-------------------------------------------------->

<div id="bsglobal" title="Busqueda Global">
    
    <table>
       
        <tr>
            <td style="padding-top: 5px;">
            
                
                <table style="width: 1000px;">
                    <tr>
                        <td style="width: 197px;">
                            <div>NUMSERIE</div>
                            <div><input class="jTx" id="numserie1" type="text" value="" style="width: 197px; text-align: center;" /></div>
                        </td>
            
                        <td style="width: 197px;">
                            <div>NUMALBARAN</div>
                            <div><input class="jTx" id="numalbaran1" type="text" value="" style="width: 197px; text-align: center;" /></div>
                        </td>
            
                        <td style="width: 197px;">
                            <div>SUALBARAN</div>
                            <div><input class="jTx" id="sualbaran1" type="text" value="" style="width: 197px; text-align: center;" /></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 100px;">
                            <div>Fecha Inicial</div>
                            <div id="jDate9"></div>
                        </td>
                        <td style="text-align: left; padding: 5px; width: 100px;">
                            <div>Fecha Final</div>
                            <div id="jDate10"></div>
                        </td>
                        <td>

                        </td>
                    </tr>

                </table>
                

            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;">
                    
                
                
                <table style="width: 100%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin: auto;background-color: #fbfbfb">
                    <tr>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        <input type="button" id="bBuscar4" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                

            </td>
        </tr>
        <tr>
            <td style="padding-top: 7px;">
                <div id="jGrid12" style=""></div>
            </td>
        </tr>
    </table>

</div>


<!-------------------------------------------------->

<div id="bloqueo2" title="Fecha Bloqueo Albaranes">
    <table style="margin-bottom: 0px;margin-top: 7px">
        <tr>
            <td style="width: 125px; padding-bottom: 10px;" align="right">
                Fecha Bloqueo:
            </td>
            <td style="padding-bottom: 10px;">
                <div class="txCenter" id="fBloqueo2" style="font-size: 13px; border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 3px; width: 100% !important;"></div>
            </td>
        </tr>
        <tr>
            <td align="right">
                Fecha:
            </td>
            <td>
                <div id="jBloqueo2" style="width: 160px !important;"></div>
            </td>
        </tr>
    </table>
</div>


<!-------------------------------------------------->
<div id="tipoAjuste" title="Tipo Ajuste">
    <div style="padding-top: 15px;">
        <div id="jGrid16" style=""></div>
    </div>
    
</div>
<!-------------------------------------------------->
<div id="ajuste" title="Ajuste">
    <div style="padding-top: 10px;">
        
        <table>
            <tr>
                <td style="width: 70px; padding-bottom: 2px;" align="right" valign="bottom">
                    AJUSTE:
                </td>
                <td style="padding-bottom: 2px;" valign="bottom">
                    
                    <div class="txCenter" id="tAjuste" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100px !important;"></div>
                </td>
                <td style="padding-left: 10px;" valign="bottom">
                    
                    <table>
                        <tr>
                            <td style="padding-top: 22px;">
                                <input type="button" id="btAjuste" class="btn1" style="height: 28px; padding-top: 0px; font-size: 0.5em;" value=".."/>
                            </td>
                            <td>
                                <div>TIPO AJUSTE</div>
                                <div class="txCenter" id="tpAjuste" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 300px !important;"></div>
                            </td>
                        </tr>
                    </table>
                    

                </td>
            </tr>
        </table>
    </div>
</div>
<!-------------------------------------------------->



<div id="ajustes" title="Ajustes">
    
    
    
                        <table style="border: 1px #b4b2b2 solid; margin-top: 5px;">
                        <tr>
                            <td style="border: 1px #b4b2b2 solid;width: 600px" valign="top">
                                <div id="jGrid13" style=""></div>
                            </td>
                            <td style="border: 1px #b4b2b2 solid;width: 600px" valign="top">
                                <div id="jGrid14" style=""></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="" align="center">
                                <div id="jGrid15" style=""></div>
                                <table style="margin-top: 8px; margin-bottom: 5px;">
                                    <tr>
                                        <td style="padding-right: 5px;">
                                           Total Factura:
                                        </td>
                                        <td style="width: 120px;">
                                            <div class="txCenter" id="tfac1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;">0</div>
                                        </td>
                                        
                                        <td style="padding-left: 5px;padding-right: 5px;">
                                            Total Albaran:
                                        </td>
                                        <td style="width: 120px;">
                                            <div class="txCenter" id="talb1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;">0</div>
                                        </td>
                                        
                                        <td style="padding-left: 5px;padding-right: 5px;">
                                            Total Ajuste:
                                        </td>
                                        <td style="width: 120px;">
                                            <div class="txCenter" id="tAj1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;">0</div>
                                        </td>
                                        
                                        <td style="padding-left: 5px;padding-right: 5px;">
                                            Albaran Ajuste:
                                        </td>
                                        <td style="width: 120px;">
                                            <div class="txCenter" id="tAlb1" style="border: solid 1px #c7c7c7; border-radius: 3px; height: 27px; background-color: #EFEFEF; padding-top: 2px; width: 100% !important;">0</div>
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>

                    </table>
    


</div>





<!-------------------------------------------------->

<div id="albaran" title="Albaranes">
    <table style="margin-bottom: 0px;margin-top: 7px">
        <tr>
            <td>
                <div id="jGrid11" style=""></div>
            </td>
        </tr>
    </table>
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



<div id="detalle6" title="Facturas Relacionadas">
    <table style="margin-bottom: 0px;margin-top: 7px">
        <tr>
            <td>
                <div id="jGrid10" style=""></div>
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