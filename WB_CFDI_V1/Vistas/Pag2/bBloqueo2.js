$('#bBloqueo2').click(function () {

    var fecha1 = getData("Pag2.aspx/GetFechaBloqueo2Str");
    $("#fBloqueo2").html(fecha1);


    $("#bloqueo2").dialog({
        buttons: {
            'Fijar Fecha Bloqueo': function () {

                var fecha1 = $.jqx.dataFormat.formatdate($("#jBloqueo2").jqxDateTimeInput('getDate'), 'dd/MM/yyyy');

                $("#confirmar")
                                    .html(
                                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres establecer la fecha de bloqueo a: " + fecha1 + "?</div>");

                $("#confirmar").dialog({
                    buttons: {
                        'Aceptar': function () {
                            $(this).dialog("close");

                            $('#init').removeClass('no-show');
                            setTimeout(function () {

                                var rs = getData("Pag2.aspx/SetFechaBloqueo2",
                                                            {
                                                                fecha: $.jqx.dataFormat.formatdate($("#jBloqueo2").jqxDateTimeInput('getDate'), 'd')
                                                            });

                                $("#fBloqueo2").html(fecha1);

                                var index2 = $("#cbRFC").jqxComboBox('selectedIndex');
                                if (index2 != -1 && source2.localdata.length > 0) {
                                    buscar2();
                                }


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

    $("#bloqueo2").dialog('open');

    position('.mWin2');

});