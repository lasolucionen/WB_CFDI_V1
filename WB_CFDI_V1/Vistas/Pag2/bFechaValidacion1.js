$('#bFechaValidacion1').click(function () {
    
    var chk = getData("Pag2.aspx/GetChkFechaValidacion");

    chk1 = false;
    $("#chkFechaValidacion").jqxCheckBox({ checked: chk });
    chk1 = true;

    var fecha1 = getData("Pag2.aspx/GetFechaValidacionStr");
    $("#fValidacion").html(fecha1);


    $("#fechaValidacion").dialog({
        buttons: {
            'Fijar Fecha Validación': function () {

                var fecha1 = $.jqx.dataFormat.formatdate($("#jValidacion").jqxDateTimeInput('getDate'), 'dd/MM/yyyy');

                $("#confirmar")
                                        .html(
                                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres establecer la fecha de validación a: " + fecha1 + "?</div>");

                $("#confirmar").dialog({
                    buttons: {
                        'Aceptar': function () {
                            $(this).dialog("close");

                            $('#init').removeClass('no-show');
                            setTimeout(function () {

                                var rs = getData("Pag2.aspx/SetFechaValidacion",
                                                            {
                                                                fecha: $.jqx.dataFormat.formatdate($("#jValidacion").jqxDateTimeInput('getDate'), 'd')
                                                            });

                                $("#fValidacion").html(fecha1);


                                $('#init').addClass('no-show');
                            }, 30);

                        },
                        'Cancelar': function () {

                            $(this).dialog("close");
                        }
                    }
                });

                $("#confirmar").dialog('open');

            },
            'Cerrar': function () {

                $(this).dialog("close");
            }
        }
    });

    $("#fechaValidacion").dialog('open');

    position('.mWin2');

});