$("#bQuitar").click(function (event) {

    $("#bQuitar").button({ disabled: true });


    var position = $("#jGrid2").jqxGrid('scrollposition');
    //var left = position.left;
    //var top = position.top;

    // get the indexes of the selected rows.
    var selectedrowindexes = $("#jGrid2").jqxGrid('getselectedrowindexes');

    selectedrowindexes.sort().reverse();

    $.each(selectedrowindexes,
        function (index, value) {
            source2.localdata.splice(value, 1);
        });

    dataAdapter2.dataBind();
    $("#jGrid2").jqxGrid('clearselection');
    $("#jGrid2").jqxGrid('scrolloffset', position.top, 0);


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

    $("#bQuitar").button({ disabled: false });

    if (selectedrowindexes.length > 0) {
        buscar();
    }
});