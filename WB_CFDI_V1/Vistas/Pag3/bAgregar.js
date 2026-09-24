$("#bAgregar").click(function (event) {
    $("#bAgregar").button({ disabled: true });

    var isOK = true;

    var position = $("#jGrid").jqxGrid('scrollposition');
    //var left = position.left;
    //var top = position.top;

    // get the indexes of the selected rows.
    var selectedrowindexes = $("#jGrid").jqxGrid('getselectedrowindexes');
    var rowscount = $("#jGrid").jqxGrid('getdatainformation').rowscount;


    try {

        $.each(selectedrowindexes,
                            function (index, value) {
                                var data = $('#jGrid').jqxGrid('getrowdata', value);

                                if (data.CONTRA_RECIBO_ID != null) {
                                    isOK = false;
                                    throw new TypeError();
                                }
                            });

    } catch (e) {
    }


    if (!isOK) {
        $("#bAgregar").button({ disabled: false });

        $("#message").html("<div style='margin-top:15px;'>No se puede agregar, alguna factura seleccionada ya tiene contrarecibo.</div>");
        $("#message").dialog('open');
        return;
    }


    try {

        $.each(selectedrowindexes,
                            function (index, value) {
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

        $("#message").html("<div style='margin-top:15px;'>No se puede agregar, alguna factura seleccionada esta rechazada.</div>");
        $("#message").dialog('open');
        return;
    }

    var position = $("#jGrid").jqxGrid('scrollposition');

    selectedrowindexes.sort().reverse();

    $.each(selectedrowindexes,
                        function (index, value) {
                            var it = $('#jGrid').jqxGrid('getrowdata', value);
                            source2.localdata.push(it);
                            source.localdata.splice(value, 1);
                        });

    dataAdapter.dataBind();
    $("#jGrid").jqxGrid('clearselection');
    $("#jGrid").jqxGrid('scrolloffset', position.top, 0);

    dataAdapter2.dataBind();
    $("#jGrid2").jqxGrid('clearselection');


    $("#jGrid2").jqxGrid("autoresizecolumns");
    var colDefs = $("#jGrid2").jqxGrid('columns').records;
    for (var idx = 0; idx < colDefs.length; idx++) {

        if (colDefs[idx].datafield != "_checkboxcolumn") {
            $("#jGrid2").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
        }

        if (colDefs[idx].datafield == "UUID") {
            $("#jGrid2").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 10);
        }
    }

    total2();

    $("#bAgregar").button({ disabled: false });

});