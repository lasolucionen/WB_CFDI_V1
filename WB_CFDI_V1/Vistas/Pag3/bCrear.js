$("#bCrear").click(function (event) {

    $("#bCrear").button({ disabled: true });

    var select1 = new Array();

    var rows = $("#jGrid2").jqxGrid('getrows');
    $.each(rows,
                        function (index, value) {
                            select1.push({
                                ID: value.ID,
                                CONTRA_RECIBO_ID: value.CONTRA_RECIBO_ID,
                                RFC_EMISOR: $(".rfc").eq(0).html(),
                                ESTATUS2: value.ESTATUS
                            });
                        });


    if (rows.length > 0) {

        $("#Confirmar_msg").html('Quieres crear el contrarecibo de las facturas agregadas?');
        $("#confirmar").dialog({
            buttons: {
                'Crear': function () {


                    $('#init').removeClass('no-show');
                    setTimeout(function () {
                        $("#confirmar").dialog("close");

                        //var fecha1 = $("#jDate5").jqxDateTimeInput('getDate');

                        var fecha2 = $.jqx.dataFormat.formatdate($("#jDate6").jqxDateTimeInput('getDate'), 'd');

                        var rs = getData("Pag3.aspx/Crear",
                                                {
                                                    data: select1,
                                                    //fecha1: fecha1,
                                                    fecha2: fecha2,
                                                    observacion: $("#txObservacion").val()
                                                });

                        if (rs.code == 0) {

                            $("#txObservacion").val('');

                            source2.localdata = [];
                            dataAdapter2.dataBind();
                            $("#jGrid2").jqxGrid('clearselection');
                            $("#jGrid2").jqxGrid('updatebounddata', 'cells');

                            total2();
                            buscar();

                            $("#message")
                                                    .html(
                                                        "<div style='margin-top:15px;'>El contrarecibo <span style='color:red'>" +
                                                        rs.contr +
                                                        "</span> se creo correctamente.</div>");
                            $("#message").dialog('open');

                            window.open("contrareciboPDF.aspx", "xml", "width=200,height=150");
                        } else {
                            $("#message").html("<div style='margin-top:15px;'>" +
                                                    rs.msg +
                                                    "</div>");
                            $("#message").dialog('open');
                        }



                        $("#bCrear").button({ disabled: false });
                        $('#init').addClass('no-show');
                    }, 20);
                },
                'Cancelar': function () {

                    $("#bCrear").button({ disabled: false });

                    $(this).dialog("close");
                }
            }
        });

        $("#confirmar").dialog('open');
    } else {
        $("#bCrear").button({ disabled: false });

        $("#message").html("<div style='margin-top:15px;'>Agregue las facturas</div>");
        $("#message").dialog('open');
    }



});