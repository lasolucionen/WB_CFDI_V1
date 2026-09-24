$("#prov").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    buttons: {
        Aceptar: function () {

            if ($("#rfc3").html() != '') {
                var index2 = $("#cbRFC3").jqxComboBox('selectedIndex');
                $("#cbRFC").jqxComboBox({ selectedIndex: index2 });
            }

            $(this).dialog("close");
        },
        Cancelar: function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    }
});

$("#inv").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Continuar': function () {
            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    close: function (event, ui) {
        bar.width('0%');
        percent.html('0%');
    }
});

$("#inv2").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Continuar': function () {
            bar.width('0%');
            percent.html('0%');

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    close: function (event, ui) {
        bar.width('0%');
        percent.html('0%');
    }
});

$("#fechaValidacion").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    open: function (event, ui) {

    }
});

$("#fechaRecLimite").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    open: function (event, ui) {

    }
});

$("#fechaPagoBloqueo").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    open: function (event, ui) {

    }
});


$("#bloqueo").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    open: function (event, ui) {

    }
});

$("#bloqueo2").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    open: function (event, ui) {

    }
});

$("#detalle").dialog({
    autoOpen: false,
    resizable: false,
    //width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});

$("#detalle2").dialog({
    autoOpen: false,
    resizable: false,
    //width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: false,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});

$("#detalle3").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: false,
    dialogClass: 'mWin',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    },
    close: function () {
        source6.localdata = [];
        dataAdapter6.dataBind();
        $("#jGrid6").jqxGrid('updatebounddata', 'cells');
    }
});

$("#detalle4").dialog({
    autoOpen: false,
    resizable: false,
    //width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});


$("#detalle5").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Exportar': function () {

            getData("excel4.aspx/SetID", { id: IDgrid });

            window.open("excel4.aspx", "xml", "width=200,height=150");

        },

        'Cerrar': function () {

            source9.localdata = [];
            dataAdapter9.dataBind();
            $("#jGrid9").jqxGrid('ensurerowvisible', 0);
            $("#jGrid9").jqxGrid('clearselection');

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});



$("#detalle6").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: false,
    dialogClass: 'mWin',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    },
    close: function () {
        /*source6.localdata = [];
        dataAdapter6.dataBind();
        $("#jGrid6").jqxGrid('updatebounddata', 'cells');*/
    }
});




$("#rechazar").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});


$("#observacion1").dialog({
    autoOpen: false,
    resizable: false,
    width: '400',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});



$("#incidencia1").dialog({
    autoOpen: false,
    resizable: false,
    width: '400',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});


$("#albaran").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: false,
    dialogClass: 'mWin',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    },
    close: function () {
        /*source6.localdata = [];
        dataAdapter6.dataBind();
        $("#jGrid6").jqxGrid('updatebounddata', 'cells');*/
    }
});


$("#message").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    buttons: {
        Ok: function () {
            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    }
});

$("#confirmar").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin',
    open: function (event, ui) {

    }
});

$("#sub").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});


$("#exportar").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Exportar': function () {

            var chkXML = $("#chkXML").jqxCheckBox('checked');
            var chkPDF = $("#chkPDF").jqxCheckBox('checked');

            $('#init').removeClass('no-show');

            var rowindexes = $('#jGrid').jqxGrid('getselectedrowindexes');

            var select = new Array();
            $.each(rowindexes, function (index, value) {
                var data = $('#jGrid').jqxGrid('getrowdata', value);
                select.push(data.ID);
            });

            if (!$.isEmptyObject(select)) {
                getData("excel.aspx/SetID", {
                    data: select,
                    chkXML: chkXML,
                    chkPDF: chkPDF
                });

                window.open("excel.aspx", "xml", "width=200,height=150");
            }

            $('#init').addClass('no-show');
            $(this).dialog("close");
        },
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    }
});


$("#config").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin2',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});


$("#bsglobal").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: false,
    dialogClass: 'zindex',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});



$("#ajustes").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'zindex',
    buttons: {

        'Eliminar Ajuste': function () {

            var inx1 = $("#jGrid15").jqxGrid('getselectedrowindexes');

            if (isEmpty(inx1)) {
                return;
            }

            source15.localdata.splice(inx1[0], 1);

            dataAdapter15.dataBind();
            $("#jGrid15").jqxGrid('clearselection');



            $("#tAjustes").html(source15.localdata.length);



            var imp1 = 0.0;
            var imp2 = 0.0;
            var ajuste = 0.0;

            $.each(source15.localdata, function (index, value) {

                //imp1 += value.IMPORTE;
                //imp2 += value.TOTAL + value.PRECIO;

                ajuste += value.PRECIO;

            });



            if (ajuste != 0.0) {

                $("#tAj1").html(numeral(ajuste).format('0.00'));
                $("#tAlb1").html(numeral(numeral($("#talb1").html()).value() + ajuste).format('0.00'));
            } else {
                $("#tAlb1").html('0');
                $("#tAj1").html('0');
            }


            //$("#tfac1").html(numeral(imp1).format('0.00'));
            //$("#talb1").html(numeral(imp2).format('0.00'));

            /////////////////////////////////

            source13.localdata = JSON.parse(JSON.stringify(tmpData1));


            $.each(source15.localdata, function (index, value) {

                for (i = source13.localdata.length - 1; i >= 0; i--) {

                    if (source13.localdata[i].DESCRIPCION == value.DESCRIPCION) {
                        source13.localdata.splice(i, 1);
                        break;
                    }
                }
            });



            dataAdapter13.dataBind();
            $("#jGrid13").jqxGrid('ensurerowvisible', 0);
            $("#jGrid13").jqxGrid('clearselection');

            /////////////////////////////////

            source14.localdata = JSON.parse(JSON.stringify(tmpData2));


            $.each(source15.localdata, function (index, value) {

                for (i = source14.localdata.length - 1; i >= 0; i--) {

                    if (source14.localdata[i].DESCRIPCION == value.DESCRIPCION2) {
                        source14.localdata.splice(i, 1);
                        break;
                    }
                }
            });

            dataAdapter14.dataBind();
            $("#jGrid14").jqxGrid('ensurerowvisible', 0);
            $("#jGrid14").jqxGrid('clearselection');

            /////////////////////////////////
            
        },

        'Agregar Ajuste': function () {


            _inx1 = $("#jGrid13").jqxGrid('getselectedrowindexes');
            _inx2 = $("#jGrid14").jqxGrid('getselectedrowindexes');


            if (isEmpty(_inx1) || isEmpty(_inx2)) {


                $("#message")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                        "Seleccione la relación de artículos." +
                        "</div>");

                $("#message").dialog('open');
                position('.mWin');

                return;
            }


            var data1 = $("#jGrid13").jqxGrid('getrowdata', _inx1[0]);
            var data2 = $("#jGrid14").jqxGrid('getrowdata', _inx2[0]);


            var importe = numeral(data1.IMPORTE).value();
            var importe2 = numeral(data1.IMPORTE).value();
            var descuento = numeral(data1.DESCUENTO).value();

            var total = numeral(data2.TOTAL).value();
            var total2 = numeral(data2.TOTAL).value();


            importe = importe - descuento;
            importe2 = importe2 - descuento;

            
            if (data2.IVA != 0) {
                var iva = data2.IVA / 100;
                var impuesto1 = total * iva;
                var impuesto2 = importe * iva;

                total = total + impuesto1;
                importe = importe + impuesto2;

            }

            if (data2.REQ != 0) {
                var ieps = data2.REQ / 100;
                var impuesto1 = total * ieps;
                var impuesto2 = importe * ieps;

                total = total + impuesto1;
                importe = importe + impuesto2;
            }

            _ajuste = numeral(importe2 - total2).format('0.00');

            
            $("#tAjuste").html(numeral(importe - total).format('0.00'));


            $("#ajuste").dialog('open');
            position('.mWin');





        },

        'Procesar Ajustes': function () {


            var data = $("#jGrid15").jqxGrid('getrows');



            if (isEmpty(data)) {


                $("#message")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                        "Agregue los ajustes." +
                        "</div>");

                $("#message").dialog('open');
                position('.mWin');

                return;
            }



            $("#confirmar")
                .html(
                    "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres procesar los ajustes agregados?</div>");


            $("#confirmar").dialog({
                buttons: {
                    'Aceptar': function () {
                        $(this).dialog("close");

                        $('#init').removeClass('no-show');
                        var rs=getData("Pag2.aspx/Ajustes",
                            {
                                data: source15.localdata
                            });



                        if (rs.code == -1) {
                            $("#message")
                                .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                    rs.msg +
                                    "</div>");
                            $("#message").dialog('open');
                        } else {
                            $("#message")
                                .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                    "Registros procesados correctamente." +
                                    "</div>");
                            $("#message").dialog('open');


                            source15.localdata = [];
                            dataAdapter15.dataBind();
                            $("#jGrid15").jqxGrid('ensurerowvisible', 0);
                            $("#jGrid15").jqxGrid('clearselection');


                            $("#tfac1").html('0');
                            $("#talb1").html('0');

                            $("#tAlb1").html('0');
                            $("#tAj1").html('0');
                        }


                        
                        $('#init').addClass('no-show');

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
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});



$("#ajuste").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'zindex',
    buttons: {
        'Cancelar': function () {

            $(this).dialog("close");
        },
        'Agregar': function () {


            if (isEmpty($("#tAjuste").html().trim())) {


                $("#message")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                        "Coloca un ajuste." +
                        "</div>");

                $("#message").dialog('open');
                position('.mWin');

                return;
            }


            if ($("#tpAjuste").html() == "") {
                $("#message")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                        "Seleccione el tipo ajuste." +
                        "</div>");

                $("#message").dialog('open');
                position('.mWin');

                return;
            }

            addAjuste();

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});



$("#tipoAjuste").dialog({
    autoOpen: false,
    resizable: false,
    width: 'auto',
    //minWidth: 200,
    //maxWidth: 500,
    fluid: true, //new option
    modal: true,
    dialogClass: 'mWin',
    buttons: {
        'Cerrar': function () {

            $(this).dialog("close");
        }
    },
    open: function (event, ui) {

    },
    create: function (event, ui) {
        // Set maxWidth
        //$(this).css("maxWidth", "700px");
    }
});