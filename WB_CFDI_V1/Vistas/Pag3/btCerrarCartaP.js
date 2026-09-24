$("#btCerrarCartaP").click(function(event) {

    
    if (!info.oracle) {

        $("#Confirmar_msg").html('Quieres cerrar la carta pago del contrarecibo: ' + cntr + ' ?');
        $("#confirmar").dialog({
            buttons: {
                'Aceptar': function() {


                    $('#init').removeClass('no-show');
                    setTimeout(function() {
                            $("#confirmar").dialog("close");

                            $("#btCerrarCartaP").button("disable");
                            var rs = getData("Pag3.aspx/CerrarCartaP",
                                {
                                    id: cntr
                                });

                            if (rs.code == 0) {
                                //$("#btAbrirCartaP").button("enable");
                                //info.oracle = false;

                            } else {
                                $("#btCerrarCartaP").button("enable");
                            }

                            buscar2();


                            $("#message").html("<div style='margin-top:15px;'>" +
                                rs.msg +
                                "</div>");
                            $("#message").dialog('open');

                            if (rs.code == -1) {
                                $('#jArea').jqxTextArea({ height: 305, width: 410, minLength: 1 });
                            }


                            $('#init').addClass('no-show');
                        }, 20);
                },
                'Cancelar': function() {


                    $("#confirmar").dialog("close");
                }
            }
        });

        $("#confirmar").dialog('open');
    }

});