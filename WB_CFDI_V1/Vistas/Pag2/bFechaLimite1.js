$('#bFechaLimite1').click(function () {
    
    var chk = getData("Pag2.aspx/GetChkFechaLimite");

    chk1 = false;
    $("#chkFechaLimite").jqxCheckBox({ checked: chk });
    chk1 = true;

    var fecha1 = getData("Pag2.aspx/GetFechaLimite");
    $("#fLimite").html(fecha1);


    $("#fechaRecLimite").dialog({
        buttons: {
            'Fijar Día Limite': function () {

                var dia = $.jqx.dataFormat.formatdate($("#jLimite").jqxDateTimeInput('getDate'), 'dd') * 1;

                $("#confirmar")
                                        .html(
                                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres establecer el limite de la recepción de facturas a: " + dia + " de cada mes?</div>");

                $("#confirmar").dialog({
                    buttons: {
                        'Aceptar': function () {
                            $(this).dialog("close");

                            $('#init').removeClass('no-show');
                            setTimeout(function () {

                                var rs = getData("Pag2.aspx/SetFechaLimite",
                                                            {
                                                                dia: dia
                                                            });

                                $("#fLimite").html(dia);


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

    $("#fechaRecLimite").dialog('open');

    position('.mWin2');

});