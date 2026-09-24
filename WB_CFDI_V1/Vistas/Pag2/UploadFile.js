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

            //position('.mWin');
            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: window });

        }
        catch (err) {
            //alert(err.message);
            //console.debug(xhr.responseText);
            //console.debug(xhr.responseText.msg);
            window.location.href = "Pag2.aspx";
        }
    }
});