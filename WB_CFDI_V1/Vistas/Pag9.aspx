<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pag9.aspx.cs" Inherits="WB_CFDI_V1.Vistas.Pag9" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Routing" %>

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
            var js = "21";

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


                /*******************************************************************************/

                $("#jDate1").jqxDateTimeInput({ width: 130, value:'<%=getFecha1() %>' , height: '25px', theme: 'Theme2' ,culture: 'es'});
                $("#jDate2").jqxDateTimeInput({ width: 130, height: '25px', theme: 'Theme2', dropDownHorizontalAlignment: 'right', culture: 'es-ES' });
                //$("#jDate3").jqxDateTimeInput({ width: 110, value: '<%=getFecha2() %>', height: '25px', theme: 'Theme2', culture: 'es' });





                $('#init').removeClass('no-show');
                setTimeout(function () {


                    $('#init').addClass('no-show');
                }, 1000);

                /*******************************************************************************/

                /*
                var rs = getData("Pag7.aspx/GetSemana");
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

 


           

//*****************************************************************************************************************


//*****************************************************************************************************************


                //----------------------------------------------------------------------------//

                //---------------------------------------------------------------------------


                //---------------------------------------------------------------------------
                $("#bExportar").click(function(event) {
                    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
                    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');

                    //var index = $("#cbEstatus").jqxDropDownList('selectedIndex');
                    //var estatus = $("#cbEstatus").jqxDropDownList('getItem', index).value;

                    //var index2 = $("#cbRFC").jqxComboBox('selectedIndex');
                    //var rfc = null;

                    //if (index2 != -1) {
                    //    rfc = $("#cbRFC").jqxComboBox('getItem', index2).label;
                   // }

                    var data = getData("reporte4.aspx/SetID",
                        {
                            req: {
                                fecha1: fecha1,
                                fecha2: fecha2,
                                //estatus: estatus,
                                //RFC_EMISOR: rfc
                            }
                        });

                    window.open("reporte4.aspx", "xml", "width=200,height=150");
                });


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
                

                <table style="">
                  <tr>
                        <td class="a1" style="text-align: left; padding: 5px; padding-bottom: 0px;  width: 150px;" valign="top">
                            <div>Fecha Periodo Inicial</div>
                            <div id="jDate1"></div>
                        </td>
                        <td class="a1" style="text-align: left; padding: 5px; padding-bottom: 0px;  width: 150px;" valign="top">
                            <div>Fecha Periodo Final</div>
                            <div id="jDate2"></div>
                        </td>
                      <td class="a1" style="text-align: left; padding: 5px; padding-top: 22px; width: 150px;" colspan="2">
                          <input type="button" id="bExportar" class="btn1" style="width: 200px; height: 30px; padding-top: 5px; font-size: 1em;" value="Descargar Reporte"/>
                      </td>
                      <td class="" style="text-align: left; padding: 5px; padding-top: 22px; width: 150px !important;" colspan="2">
                          <input type="button" id="bReporte1" class="btn1" style="width: 200px; height: 30px; padding-top: 5px; font-size: 1em;" value="Reporte Contrarecibos"/>
                      </td>
                      <td></td>
                      

                    </tr>


                </table>
                

                
                
            </td>
        </tr>
        <tr>
            <td>
                <%--<%= Reporte1 %>--%>
                
               
            </td>
        </tr>

    </table>


    <%--<div style="border: solid 1px black;padding: 0px;max-width: 99%;margin: auto" >--%>
        <div class="content">



                
                
                <div id="message" title="Mensaje del sistema"></div>

               
            
                
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