<%@ Control Language="C#" CodeBehind="CargaXML.ascx.cs" Inherits="WB_CFDI_V1.Vistas.CargaXML" %>


<div id="inv" title="Carga XML">
        <form id="UploadFile" style="max-width: 400px; margin: auto;" action="fileUploader.asmx/Upload2" method="POST" enctype="multipart/form-data">
            <div class="" style="margin-top: 0px; margin-bottom: 0px;">

                <table style="width: 400px; margin-top: 10px;">
                    <tr>
                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px;">

                            <input type="button" id="cExaminar1" class="btn1" style="height: 30px; width: 100%; padding-top: 5px; font-size: 1em;" value="(XML) Examinar..."/>
                            <div id="dFile1">
                            <input type="file" class="upload" id="cfile1" name="myfile1" accept=".xml" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="border: 1px solid #e4e4e4; text-align: left; padding: 5px; padding-right: 10px;">
                            <input class="jTx txCenter" id="archivo1" readonly="readonly" type="text" style="width: 100%" value="" autocomplete="off"/>
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
                            <input class="jTx txCenter" id="archivo2" readonly="readonly" type="text" value="" style="width: 100%" autocomplete="off"/>
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


    //***********************************************************************************************
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


    //***********************************************************************************************
    var pSerieFolio = getData("Pag1.aspx/GetPSerieFolio");

    if (pSerieFolio == "1") {
        //-----------------------------------------------------------------------

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
                    } catch (err) { }

                    //var rs = xhr.responseText;

                    var percentVal = '100%';
                    bar.width(percentVal);
                    percent.html(percentVal);

                    $('#init').removeClass('no-show');



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
                    //alert(err.message);
                    //console.debug(xhr.responseText);
                    //console.debug(xhr.responseText.msg);
                    window.location.href = "Pag1.aspx";
                }
            }
        });


        //-----------------------------------------------------------------------
    } else {

        //----------------------------------------------------------------------
        $('#UploadFile').attr('action', 'fileUploader.asmx/Upload');


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
                    } catch (err) { }

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
                    window.location.href = "Pag1.aspx";
                }
            }
        });



        //----------------------------------------------------------------------
    }

    /***********************************************************************************************/



</script>