$("#btAbrirCartaP").click(function (event) {

    
    if (info.oracle) {

        $("#Confirmar_msg").html('Quieres abrir la carta pago del contrarecibo: ' + cntr + ' ?');
        $("#confirmar").dialog({
            buttons: {
                'Aceptar': function() {


                    $('#init').removeClass('no-show');
                    setTimeout(function() {
                            $("#confirmar").dialog("close");

                            $("#btAbrirCartaP").button("disable");
                            var rs = getData("Pag3.aspx/AbrirCartaP",
                                {
                                    id: cntr
                                });

                            if (rs.code == 0) {
                                //$("#btCerrarCartaP").button("enable");
                                //info.oracle = true;
                            } else {
                                $("#btAbrirCartaP").button("enable");
                            }

                            buscar2();


                            $("#message").html("<div style='margin-top:15px;'>" +
                                rs.msg +
                                "</div>");
                            $("#message").dialog('open');


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