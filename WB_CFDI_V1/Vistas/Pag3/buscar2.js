function buscar2() {


    if ($(".rfc").html() == "") {
        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
        $("#message").dialog('open');
        return;
    }


    $('#init').removeClass('no-show');
    setTimeout(function () {



        var fecha1 = $.jqx.dataFormat.formatdate($("#jDate3").jqxDateTimeInput('getDate'), 'd');
        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate4").jqxDateTimeInput('getDate'), 'd');

        source3.localdata = getData("Pag3.aspx/GetContr", {
            rfc: $(".rfc").html(),
            fecha1: fecha1,
            fecha2: fecha2,
            Contr: $("#txContr2").val()
        });

        dataAdapter3.dataBind();
        $("#jGrid3").jqxGrid('clearselection');
        $("#jGrid3").jqxGrid('updatebounddata', 'cells');
        $("#jGrid3").jqxGrid('ensurerowvisible', 0);

        $('#init').addClass('no-show');

    }, 10);
}