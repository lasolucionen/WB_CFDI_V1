function buscar() {
    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

    var uuid = $("#txUUID").val().trim();
    var folio = $("#txFolio").val();
    var serie = $("#txSerie").val();
    var contr = $("#txContr").val();

    var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'd');
    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'd');

    var index1 = $("#cbEstatus").jqxDropDownList('selectedIndex');
    var estatus = $("#cbEstatus").jqxDropDownList('getItem', index1).value;

    var index2 = $("#cbRFC").jqxComboBox('selectedIndex');
    if (index2 == -1 && uuid == "") {

        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>Seleccione el RFC Emisor.</div>");
        $("#message").dialog('open');
        return;
    }


    var id = 0;
    var rfc = null;

    if (index2 != -1) {
        id = $("#cbRFC").jqxComboBox('getItem', index2).value;
        rfc = $("#cbRFC").jqxComboBox('getItem', index2).label;
    }

    if (uuid != "") {
        id = 0;
        folio = "";
        serie = "";
        contr = "";
        estatus = 0;

    }

    if (id == 0) {
        rfc = null;
    }

    $('#init').removeClass('no-show');
    setTimeout(function () {


        var data2 = [];

        var rows = $("#jGrid2").jqxGrid('getrows');
        $.each(rows,
                            function (index, value) {
                                data2.push(value.ID);
                            });

        var data = getData("Pag3.aspx/GetRegistros",
                            {
                                req: {
                                    uuid: uuid,
                                    fecha1: fecha1,
                                    fecha2: fecha2,
                                    estatus: estatus,
                                    RFC_EMISOR: rfc,
                                    folio: folio,
                                    serie: serie,
                                    contr: contr,
                                    data2: data2
                                }
                            });


        if (uuid != "" && !isEmpty(data)) {

            $("#cbRFC").jqxComboBox('selectItem', data[0].RFC_EMISOR);

        }

        source.localdata = data;
        dataAdapter.dataBind();
        $("#jGrid").jqxGrid('clearselection');
        $("#jGrid").jqxGrid('updatebounddata', 'cells');

        $("#jGrid").jqxGrid("autoresizecolumns");

        var colDefs = $("#jGrid").jqxGrid('columns').records;
        for (var idx = 0; idx < colDefs.length; idx++) {

            if (colDefs[idx].datafield != "_checkboxcolumn") {
                $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
            }

            if (colDefs[idx].datafield == "UUID") {
                $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 10);
            }

        }
        $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
        $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
        $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
        $("#jGrid").jqxGrid('setcolumnproperty', "INCIDENCIA1", 'width', 70);

        /*source3.localdata = data;
        dataAdapter3.dataBind();*/

        $('#init').addClass('no-show');
    }, 10);


    //window.open("excel.aspx", "xml", "width=200,height=150");
}