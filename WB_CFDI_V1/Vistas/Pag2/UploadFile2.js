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