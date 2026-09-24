$("#bProcesar").click(function (event) {


    var select1 = new Array();
    var select2 = new Array();



    var rowindexes1 = $('#jGrid').jqxGrid('getselectedrowindexes');
    var rowindexes2 = $('#jGrid2').jqxGrid('getselectedrowindexes');

    var isOK = true;
    try {

        $.each(rowindexes1,
            function(index, value) {
                var data = $('#jGrid').jqxGrid('getrowdata', value);

                if (data.ESTATUS == "Rechazadas") {
                    isOK = false;
                    throw new TypeError();
                }

            });

    } catch (e) {
    }

    if (!isOK) {
        $("#bAgregar").button({ disabled: false });

        $("#message")
            .html(
                "<div style='margin-top:15px;'>No se puede procesar, alguna factura seleccionada esta rechazada.</div>");
        $("#message").dialog('open');
        return;
    }


    var isOK2 = true;
    try {

        $.each(rowindexes1,
            function(index, value) {
                var data = $('#jGrid').jqxGrid('getrowdata', value);

                if (data.BLOQ == true) {
                    isOK2 = false;
                    throw new TypeError();
                }
            });

    } catch (e) {
    }

    var isOK3 = true;
    try {

        $.each(rowindexes2,
            function(index, value) {
                var data = $('#jGrid2').jqxGrid('getrowdata', value);

                if (data.BLOQ == true) {
                    isOK3 = false;
                    throw new TypeError();
                }
            });

    } catch (e) {
    }

    var isOK4 = true;
    try {

        $.each(rowindexes2,
            function(index, value) {
                var data = $('#jGrid2').jqxGrid('getrowdata', value);

                if (data.ST == "1") {
                    isOK4 = false;
                    throw new TypeError();
                }
            });

    } catch (e) {
    }

    if (!isOK2) {
        $("#bAgregar").button({ disabled: false });

        $("#message")
            .html(
                "<div style='margin-top:15px;'>No se puede procesar, alguna factura seleccionada esta bloqueada.</div>");
        $("#message").dialog('open');
        return;
    }

    if (!isOK3) {
        $("#bAgregar").button({ disabled: false });

        $("#message")
            .html(
                "<div style='margin-top:15px;'>No se puede procesar, algun albaran seleccionado esta bloqueado.</div>");
        $("#message").dialog('open');
        return;
    }


    if (!isOK4) {
        $("#bAgregar").button({ disabled: false });

        $("#message")
            .html(
                "<div style='margin-top:15px;'>No se puede procesar, El total del albarán no coincide con las líneas de tesorería.</div>");
        $("#message").dialog('open');
        return;
    }

    var importe_total1 = 0;
    var importe_iva1 = 0;
    var importe_ieps1 = 0;
    var importe_subtotal1 = 0;
    var importe_descuento1 = 0;

    select1 = new Array();
    $.each(rowindexes1,
        function(index, value) {
            var data = $('#jGrid').jqxGrid('getrowdata', value);
            select1.push({
                CONTRA_RECIBO_ID: data.CONTRA_RECIBO_ID,
                TOTAL: data.TOTAL,
                SUBTOTAL: data.SUBTOTAL,
                IVA: data.IVA,
                IEPS: data.IEPS,
                DESCUENTO: data.DESCUENTO,
                SERIE: data.SERIE,
                FOLIO: data.FOLIO,
                UUID: data.UUID,
                ESTATUS_ID: data.ESTATUS_ID,
                ID: data.ID,
                RFC_EMISOR: data.RFC_EMISOR,
                FECHA_FACTURA: data.FECHA_FACTURA,
                RETENCIONES: data.RETENCIONES
            });
            importe_total1 += (data.TOTAL) + (data.RETENCIONES);
            importe_subtotal1 += (data.SUBTOTAL);
            importe_iva1 += data.IVA;
            importe_ieps1 += data.IEPS;
            importe_descuento1 += data.DESCUENTO;
        });

    importe_subtotal1 = importe_subtotal1 - importe_descuento1;

    source3.localdata = select1;
    dataAdapter3.dataBind();
    $("#jGrid3").jqxGrid('clearselection');
    $("#jGrid3").jqxGrid('updatebounddata', 'cells');


    var importe_total2 = 0;
    var importe_subtotal2 = 0;
    var importe_iva2 = 0;
    var importe_ieps2 = 0;

    select2 = new Array();
    $.each(rowindexes2,
        function(index, value) {
            var data = $('#jGrid2').jqxGrid('getrowdata', value);
            select2.push({
                TOTAL: data.TOTAL,
                SUBTOTAL: data.SUBTOTAL,
                IVA: data.IVA,
                IEPS: data.IEPS,
                IMPUESTOS: data.IVA + data.IEPS,
                NUMALBARAN: data.NUMALBARAN,
                SUALBARAN: data.SUALBARAN,
                NUMSERIE: data.NUMSERIE
            });
            importe_total2 += (data.TOTAL);
            importe_subtotal2 += (data.SUBTOTAL);
            importe_iva2 += data.IVA;
            importe_ieps2 += data.IEPS;
        });

    source4.localdata = select2;
    dataAdapter4.dataBind();
    $("#jGrid4").jqxGrid('clearselection');
    $("#jGrid4").jqxGrid('updatebounddata', 'cells');


    $("#detalle").dialog({
        buttons:
        {
            'Cerrar': function() {

                $(this).dialog("close");
            }
        }
    });

    $("#msgIMP").html("");

    if (!$.isEmptyObject(select1) && !$.isEmptyObject(select2)) {

        if (select1.length > 1) {
            $("#message")
                .html(
                    "<div style='text-align: center;min-width:250px;margin-top:20px;'>Error: Solo se puede validar una factura con uno o más albaranes.</div>");
            $("#message").dialog('open');

            return;
        }




        var dif1 = Math.abs(importe_total1 - importe_total2);
        var dif2 = Math.abs(importe_subtotal1 - importe_subtotal2);

        var dif3 = Math.abs(importe_iva1 - importe_iva2);
        var dif4 = Math.abs(importe_ieps1 - importe_ieps2);


        try {
            console.log(dif1);
        } catch (e) {

        }

        var _importe_total1 = numeral(importe_total1).format('$0,0.00');
        var _importe_total2 = numeral(importe_total2).format('$0,0.00');

        var _importe_subtotal1 = numeral(importe_subtotal1).format('$0,0.00');
        var _importe_subtotal2 = numeral(importe_subtotal2).format('$0,0.00');

        var _importe_iva1 = numeral(importe_iva1).format('$0,0.00');
        var _importe_iva2 = numeral(importe_iva2).format('$0,0.00');

        var _importe_ieps1 = numeral(importe_ieps1).format('$0,0.00');
        var _importe_ieps2 = numeral(importe_ieps2).format('$0,0.00');

        $("#tTotal1").html(_importe_total1);
        $("#tTotal2").html(_importe_total2);

        $("#tSubTotal1").html(_importe_subtotal1);
        $("#tSubTotal2").html(_importe_subtotal2);

        $("#tIVA1").html(_importe_iva1);
        $("#tIVA2").html(_importe_iva2);

        $("#tIEPS1").html(_importe_ieps1);
        $("#tIEPS2").html(_importe_ieps2);

        var totalOK = false;
        var impuestosOK = false;


        if (dif1 >= 0 && dif1 <= DIF_TOTAL && dif2 >= 0 && dif2 <= DIF_TOTAL) { // 0.99
            //if (importe_total1 != importe_total2) {
            totalOK = true;
        }

        if (dif3 >= 0 && dif3 <= DIF_IMPUESTO && dif4 >= 0 && dif4 <= DIF_IMPUESTO) { // 0.30
            //if (importe_total1 != importe_total2) {
            impuestosOK = true;
        }

        if (totalOK == true && impuestosOK == true) {
            $("#detalle").dialog({
                buttons:
                {
                    'Procesar': function () {
                        //return;

                        $('#init').removeClass('no-show');
                        setTimeout(function() {


                                var fechaProd = "";
                                /*
                                <% if( User.IsInRole("5") )
                                   { %>
                                
                                fechaProd = $.jqx.dataFormat.formatdate($("#jDate7").jqxDateTimeInput('getDate'), 'd');
                                <% } %>

                                console.log(fechaProd);
                                */

                                var rs = getData("Pag2.aspx/Procesar",
                                    {
                                        facturas: select1,
                                        compras: select2,
                                        fechaProd: fechaProd /*,
                                                        dif: numeral(importe_total1 - importe_total2).format('0.00')*/
                                    });

                                //if (rs.code == 0) {
                                buscar2();
                                buscar();
                                //}

                                $("#message")
                                    .html(
                                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>" +
                                        rs.msg +
                                        "</div>");

                                $("#message").dialog('open');
                                position('.mWin');

                                if (rs.code == 2) {
                                    //window.open("cartaPagoPDF.aspx", "xml", "width=200,height=150");
                                }

                                $('#init').addClass('no-show');

                                $("#detalle").dialog("close");

                            },
                            1000);


                    },
                    'Cerrar': function() {

                        $(this).dialog("close");
                    }
                }
            });




            var facturaCierre = getData("Pag2.aspx/GetValidaFacturaCierre", { facturas: select1 });

            if (facturaCierre == true) {
                $("#message")
                    .html(
                        "<div style='text-align: center;min-width:250px;margin-top:20px;'>Error: No puede validar facturas de enero, sin antes cerrar diciembre.</div>");
                $("#message").dialog('open');
                return;
            }


            var chkFecha = getData("Pag2.aspx/GetChkFechaValidacion", {});
            var valFecha = getData("Pag2.aspx/GetFechaValidacionStr", {});


            if (chkFecha==true) {

                $("#msgIMP").html("<span style='color:blue'>Se validará con la fecha fija: " + valFecha  + " </span>");
            }
        } else {

            $("#msgIMP").html("<span style='color:red'>Los importes no coinciden.</span>");
        }






        //////////////////////////////////////////////////////////////////

        var inx1 = $("#jGrid").jqxGrid('getselectedrowindexes');
        var dt1 = $('#jGrid').jqxGrid('getrowdata', inx1[0]);

        if (dt1.ESTATUS_ID == 1) {
            $("#jGrid4").jqxGrid('showcolumn', 'Ajuste');
        } else {
            $("#jGrid4").jqxGrid('hidecolumn', 'Ajuste');
        }


        var rs = getData("Pag2.aspx/GetDetalle", { id: dt1.ID });


        tmpData1 = JSON.parse(JSON.stringify(rs.DATA));

        source13.localdata = JSON.parse(JSON.stringify(rs.DATA));
        dataAdapter13.dataBind();
        $("#jGrid13").jqxGrid('ensurerowvisible', 0);
        $("#jGrid13").jqxGrid('clearselection');


        $("#jGrid13").jqxGrid("autoresizecolumns");

        var colDefs = $("#jGrid13").jqxGrid('columns').records;
        for (var idx = 0; idx < colDefs.length; idx++) {
            if (colDefs[idx].datafield != "_checkboxcolumn") {
                $("#jGrid13").jqxGrid('setcolumnproperty',
                    colDefs[idx].datafield,
                    'width',
                    colDefs[idx].width + 5);
            }
        }


        source15.localdata = [];
        dataAdapter15.dataBind();

        $("#tAjustes").html(source15.localdata.length);


        $("#tfac1").html('0');
        $("#talb1").html('0');

        //////////////////////////////////////////////////////////////////

        
        $("#detalle").dialog({ width: Math.min(1230, $(window).width()) });
        $("#detalle").dialog('open');

        $("#jGrid3").jqxGrid("autoresizecolumns");
        var colDefs = $("#jGrid3").jqxGrid('columns').records;
        for (var idx = 0; idx < colDefs.length; idx++) {
            if (colDefs[idx].datafield != "_checkboxcolumn") {
                $("#jGrid3").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 10);
            }
        }

        $("#jGrid4").jqxGrid("autoresizecolumns");
        colDefs = $("#jGrid4").jqxGrid('columns').records;
        for (var idx = 0; idx < colDefs.length; idx++) {
            if (colDefs[idx].datafield != "_checkboxcolumn") {
                $("#jGrid4").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 10);
            }
        }

    } else {
        $("#message")
            .html(
                "<div style='text-align: center;min-width:250px;margin-top:20px;'>Relacione las facturas con las compras.</div>");
        $("#message").dialog('open');
    }

});