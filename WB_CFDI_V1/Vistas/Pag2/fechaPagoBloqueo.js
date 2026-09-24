$('#bFechaPagoBloqueo').click(function () {

    
    var chk = getData("Pag2.aspx/GetChkFechaPago");

    chk1 = false;
    $("#chkFechaPago").jqxCheckBox({ checked: chk });
    chk1 = true;

    var dias = getData("Pag2.aspx/GetFechaPagoBloqueo");
    $("#cbDias").val(dias);
    $("#fdias").html(dias);


    $("#fechaPagoBloqueo").dialog({
        buttons: {
            'Fijar Días Bloqueo': function () {

                var dias = $("#cbDias").val();

                $("#confirmar")
                                        .html(
                                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres bloquear la fecha pago del día: 1 al " + dias + " de cada mes?</div>");

                $("#confirmar").dialog({
                    buttons: {
                        'Aceptar': function () {
                            $(this).dialog("close");

                            $('#init').removeClass('no-show');
                            setTimeout(function () {

                                var rs = getData("Pag2.aspx/SetFechaPagoBloqueo",
                                                            {
                                                                dias: dias
                                                            });


                                $("#fdias").html(dias);

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

    $("#fechaPagoBloqueo").dialog('open');

    position('.mWin2');

});