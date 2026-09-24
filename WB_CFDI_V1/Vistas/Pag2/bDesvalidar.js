$("#bDesvalidar").click(function (event) {
    //if()
    var rowsindexes = $('#jGrid').jqxGrid('getselectedrowindexes');

    if (rowsindexes.length == 0) {
        var rows = $('#jGrid').jqxGrid('getrows');

        if (!$.isEmptyObject(rows)) {
            //-----------------------------------------------------------

            var info = getData("Pag3.aspx/InfoCartaP",
                {
                    id: rows[0].CONTRA_RECIBO_ID
                });


                if (info.oracle == true) {
                    $("#message")
                        .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>No se puede desvalidar el contrarecibo: " + rows[0].CONTRA_RECIBO_ID + " porque la carta pago esta cerrada.</div>");
                    $("#message").dialog('open');

                    return;
                }



                var uuids = "";

                $.each(rows,
                    function (index, value) {
                        uuids += value.UUID + "\n";
                    });


            $("#confirmar")
                .html(
                    "<div style='text-align: left;min-width:250px;margin-top:20px;'><strong>Quieres desvalidar las siguientes facturas<br />asociadas al contrarecibo:</strong> " +
                    rows[0].CONTRA_RECIBO_ID +
                    "?<br /><textarea id='jArea2'>" + uuids + "</textarea>");

            $("#confirmar").dialog({
                buttons: {
                    'Aceptar': function() {
                        $(this).dialog("close");

                        $('#init').removeClass('no-show');
                        var rs = getData("Pag2.aspx/SetDesvalidarContr",
                            {
                                contrarecibo: rows[0].CONTRA_RECIBO_ID
                            });

                        if (rs.code == -1) {
                            $("#message")
                                .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                    rs.msg +
                                    "</div>");
                            $("#message").dialog('open');
                        } else {
                            buscar();

                            $("#message")
                                .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                    rs.msg +
                                    "</div>");
                            $("#message").dialog('open');
                        }


                        $('#init').addClass('no-show');

                    },
                    'Cancelar': function() {

                        $(this).dialog("close");
                    }
                }
            });

            $("#confirmar").dialog('open');



            $('#jArea2').jqxTextArea({ height: 300, width: 350, minLength: 1 });
            //-----------------------------------------------------------


        }

        /*
        var rows = $('#jGrid').jqxGrid('getrows');
        var asociadas = rows.filter(function(x) {
        return x.ESTATUS_ID == "2";
        });

        if (!$.isEmptyObject(asociadas)) {
        alert("ok");
        }*/

    }

    if (rowsindexes.length == 1) {
        var data = $('#jGrid').jqxGrid('getrowdata', rowsindexes[0]);

        if (data.ESTATUS_ID == 2) {


            //-----------------------------------------------------------
            var info = getData("Pag3.aspx/InfoCartaP",
                {
                    id: data.CONTRA_RECIBO_ID
                });



            if (info.oracle == true) {
                $("#message")
                    .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>No se puede desvalidar el contrarecibo: " + data.CONTRA_RECIBO_ID + " porque la carta pago esta cerrada.</div>");
                $("#message").dialog('open');

                return;
            }



            $("#confirmar")
                .html(
                    "<div style='text-align: left;min-width:250px;margin-top:20px;'><strong>Quieres desvalidar la factura:</strong> " +
                    data.UUID +
                    "?");

            $("#confirmar").dialog({
                buttons: {
                    'Aceptar': function() {
                        $(this).dialog("close");

                        $('#init').removeClass('no-show');

                        setTimeout(function () {

                            var rs = getData("Pag2.aspx/SetDesvalidarUUID",
                                {
                                    contrarecibo: data.CONTRA_RECIBO_ID,
                                    uuid: data.UUID
                                });

                            if (rs.code == -1) {
                                $("#message")
                                    .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                        rs.msg +
                                        "</div>");
                                $("#message").dialog('open');
                            } else {
                                buscar();

                                $("#message")
                                    .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                        rs.msg +
                                        "</div>");
                                $("#message").dialog('open');
                            }


                            $('#init').addClass('no-show');
                        }, 100);





                    },
                    'Cancelar': function() {

                        $(this).dialog("close");
                    }
                }
            });

            $("#confirmar").dialog('open');
            //-----------------------------------------------------------
        }
    }


    /*
    var asociadas = data.filter(function(x) {
    //console.log(x.ESTATUS_ID);
    return x.ESTATUS_ID == "2";
    });

    if (!$.isEmptyObject(asociadas)) {
    //console.table(asociadas,["ID","ESTATUS_ID"]);
    $("#bDesvalidar").button("enable");
    }*/


});