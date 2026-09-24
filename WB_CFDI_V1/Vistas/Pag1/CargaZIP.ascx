<%@ Control Language="C#" CodeBehind="CargaZIP.ascx.cs" Inherits="WB_CFDI_V1.Vistas.CargaZIP" %>


<div id="inv2" title="Carga ZIP">
        <form id="UploadFile2" style="max-width: 400px; margin: auto;" action="fileUploader.asmx/Upload2" method="POST" enctype="multipart/form-data">
            <div class="" style="margin-top: 0px; margin-bottom: 0px;">

                <table style="width: 400px; margin-top: 10px;">
                    <tr>
                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">
                            <input type="button" id="cExaminar3" class="btn1" style="height: 30px; width: 100%; padding-top: 5px; font-size: 1em;" value="(ZIP) Examinar..."/>
                            <div id="dFile3">
                            <input type="file" class="upload" id="cfile3" name="myfile1" accept=".zip" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px; padding-right: 10px;">
                            <input class="jTx txCenter" id="archivo3" readonly="readonly" type="text" value="" style="width: 100%" autocomplete="off"/>
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
<!----------------------------------------------------------->


<%--<div id="detalle" title="Mensaje del sistema"><div id="jGrid2" style="margin-bottom: 0px;margin-top: 7px"></div></div>--%>


<!----------------------------------------------------------->

<script>

    function isEmpty(value) {
        //return (value == null || value.length === 0);
        return (value == null || $.trim(value) === '');
    }

    //*****************************************************************************************************************


    //$("#detalle").dialog({
    //    autoOpen: false,
    //    resizable: false,
    //    //width: 'auto',
    //    //minWidth: 200,
    //    //maxWidth: 500,
    //    fluid: true, //new option
    //    modal: true,
    //    dialogClass: 'mWin2',
    //    buttons: {
    //        'Cerrar': function () {

    //            $(this).dialog("close");
    //        }
    //    },
    //    open: function (event, ui) {

    //    },
    //    create: function (event, ui) {
    //        // Set maxWidth
    //        //$(this).css("maxWidth", "700px");
    //    }
    //});


    //***********************************************************************************************

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

    //***********************************************************************************************

    if (pSerieFolio == "1") {
        //-----------------------------------------------------------------------
        
        $('#UploadFile2').ajaxForm({

            beforeSubmit: function () {
                $("#Proc2").button({ disabled: true });
                $("#cExaminar3").button({ disabled: true });

                //status.empty();
                var percentVal = '0%';
                bar.width(percentVal);
                percent.html(percentVal);

                var pm = new Array();

                if ($.trim($("#archivo3").val()) == "") {
                    //param += ", <strong>Archivo</strong>";
                    pm.push("<strong>Archivo ZIP</strong>");
                }


                if (pm[0]) {
                    $("#Proc2").button({ disabled: false });
                    $("#cExaminar3").button({ disabled: false });

                    var param = "Ingrese el valor del campo:<br />" + pm.join(", ");
                    $("#message").html("<div style='text-align: left;min-width:250px;'>" + param + "</div>");
                    $("#message").dialog('open');
                    $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

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
                    } catch (err) { }

                    //var rs = xhr.responseText;

                    var percentVal = '100%';
                    bar.width(percentVal);
                    percent.html(percentVal);

                    //$("#message").html(rs.msg);
                    //$("#message").dialog('open');

                    $('#init').removeClass('no-show');


                    /*
                    $("#detalle").dialog({ width: Math.min(1050, $(window).width()) });
                    $("#detalle").dialog('open');
                    source2.localdata = rs.result;
                    dataAdapter2.dataBind();
                    $("#jGrid2").jqxGrid('clearselection');
                    $("#jGrid2").jqxGrid('ensurerowvisible', 0);
                    */

                    $("#vsTOTAL").html('');
                    $("#vsUUID").html('');
                    $("#vsSERIE").html('');
                    $("#vsFOLIO").html('');
                    $("#vsValidacion").html('');
                    $("#vsDETALLE").html('');
                    $("#vsPDF").html('');


                    source4.localdata = [];
                    dataAdapter4.dataBind();
                    $("#jGrid4").jqxGrid('clearselection');

                    $("#vistaprevia1").dialog('open');
                    $("#vistaprevia1").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

                    zipForm = rs.result;

                    for (var i = 0; i < rs.result.length; i++) {
                        //console.log(rs.result[i].UUID)

                        source4.localdata.push({ ARCHIVO: rs.result[i].ARCHIVO, SERIE_FOLIO: rs.result[i].SERIE_FOLIO });
                    }
                    dataAdapter4.dataBind();
                    $("#jGrid4").jqxGrid('selectrow', 0);

                    $('#init').addClass('no-show');


                }
                catch (err) {
                    //console.debug(xhr.responseText);
                    //console.debug(xhr.responseText.msg);
                    window.location.href = "Pag1.aspx";
                }
            }
        });

        //-----------------------------------------------------------------------
    } else {
        //-----------------------------------------------------------------------
        $('#UploadFile2').attr('action', 'fileUploader3.asmx/Upload2');

        $('#UploadFile2').ajaxForm({

            beforeSubmit: function () {
                $("#Proc2").button({ disabled: true });
                $("#cExaminar3").button({ disabled: true });

                //status.empty();
                var percentVal = '0%';
                bar.width(percentVal);
                percent.html(percentVal);

                var pm = new Array();

                if ($.trim($("#archivo3").val()) == "") {
                    //param += ", <strong>Archivo</strong>";
                    pm.push("<strong>Archivo ZIP</strong>");
                }


                if (pm[0]) {
                    $("#Proc2").button({ disabled: false });
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
                    } catch (err) { }

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
                    window.location.href = "Pag1.aspx";
                }
            }
        });

        //-----------------------------------------------------------------------
    }




  


    /***********************************************************************************************/



</script>