$("#bAccion").click(function(event) {
    //if()
    var getselectedrowindexes = $('#jGrid').jqxGrid('getselectedrowindexes');
    if (getselectedrowindexes.length > 0) {

        var proc = true;
        var select1 = new Array();

        $.each(getselectedrowindexes,
            function(index, value) {
                var data = $('#jGrid').jqxGrid('getrowdata', value);

                if ($("#bAccion").val() == "Rechazar") {
                    if (data.ESTATUS_ID == 2 || data.ESTATUS_ID == 6) {
                        proc = false;
                    } else {
                        select1.push({
                            ESTATUS: data.ESTATUS_ID,
                            ID: data.ID
                        });
                    }
                } else {
                    if (data.ESTATUS_ID == 3) {
                        select1.push({
                            ESTATUS: data.ESTATUS_ID,
                            ID: data.ID
                        });
                    }
                }


            });

        if ($("#bAccion").val() == "Rechazar") {
            if (proc) {

                if (!$.isEmptyObject(select1)) {

                    $("#jArea").val('');

                    $("#rechazar").dialog({
                        buttons: {
                            'Aceptar': function () {
                                $(this).dialog("close");

                                $('#init').removeClass('no-show');
                                var rs = getData("Pag2.aspx/SetAccion",
                                    {
                                        data: select1,
                                        estatus: 3,
                                        observacion1: $("#jArea").val()
                            });

                                if (rs.code == -1) {
                                    $("#message")
                                        .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                            rs.msg +
                                            "</div>");
                                    $("#message").dialog('open');
                                } else {
                                    buscar();
                                }


                                $('#init').addClass('no-show');

                            },
                            'Cancelar': function () {

                                $(this).dialog("close");
                            }
                        }
                    });

                    $("#rechazar").dialog('open');


                /*
                    $("#confirmar")
                        .html(
                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres rechazar las facturas seleccionadas?</div>");

                    $("#confirmar").dialog({
                        buttons: {
                            'Aceptar': function() {
                                $(this).dialog("close");

                                $('#init').removeClass('no-show');
                                var rs = getData("Pag2.aspx/SetAccion",
                                    {
                                        data: select1,
                                        estatus: 3
                                    });

                                if (rs.code == -1) {
                                    $("#message")
                                        .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                            rs.msg +
                                            "</div>");
                                    $("#message").dialog('open');
                                } else {
                                    buscar();
                                }


                                $('#init').addClass('no-show');

                            },
                            'Cancelar': function() {

                                $(this).dialog("close");
                            }
                        }
                    });

                    $("#confirmar").dialog('open');*/

                }

            } else {
                $("#message")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>Desmarque las facturas con estatus de asociadas o pagadas.</div>");
                $("#message").dialog('open');
            }
        } else {
            if (!$.isEmptyObject(select1)) {

                $("#confirmar")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres aceptar las facturas seleccionadas?</div>");


                $("#confirmar").dialog({
                    buttons: {
                        'Aceptar': function() {
                            $(this).dialog("close");

                            $('#init').removeClass('no-show');
                            getData("Pag2.aspx/SetAccion",
                                {
                                    data: select1,
                                    estatus: 1,
                                    observacion1: ""
                                });
                            buscar();
                            $('#init').addClass('no-show');

                        },
                        'Cancelar': function() {

                            $(this).dialog("close");
                        }
                    }
                });
                $("#confirmar").dialog('open');

            }
        }

    } else {
        $("#message")
            .html(
                "<div style='text-align: center;min-width:250px;margin-top:20px;'>No hay facturas seleccionadas</div>");
        $("#message").dialog('open');
    }
});