function buscar() {
    $("#bDesvalidar").button("disable");

    /*var fecha1 = $.jqx.dataFormat.formatdate($("#jDate1").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');
    var fecha2 = $.jqx.dataFormat.formatdate($("#jDate2").jqxDateTimeInput('getDate'), 'yyyy/dd/MM');*/

    var uuid = $("#txUUID").val().trim();
    var folio = $("#txFolio").val().trim();
    var serie = $("#txSerie").val().trim();
    var contr = $("#txContr").val().trim();

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


    tmpContr = contr;


    $('#init').removeClass('no-show');
    setTimeout(function () {


        var data = getData("Pag2.aspx/GetRegistros",
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
                                        opt: opt
                                    }
                                });


        var rows = $('#jGrid8').jqxGrid('getrows');

        var filteredArray = rows.filter(function (x) {
            return !data.some(function (item) {
                return item.ID == x.ID;
            });
        });

        source8.localdata = filteredArray;
        dataAdapter8.dataBind();
        $("#jGrid8").jqxGrid('clearselection');
        $("#jGrid8").jqxGrid('updatebounddata', 'cells');


        if (uuid != "" && !isEmpty(data)) {

            //var item = $("#cbRFC").jqxComboBox('getItems');

            $("#cbRFC").jqxComboBox('selectItem', data[0].RFC_EMISOR);
            //data[0].RFC_EMISOR
            //f65509f2-b767-449a-a9bf-25be785c34b7
        }


        source.localdata = data;
        dataAdapter.dataBind();
        $("#jGrid").jqxGrid('clearselection');
        $("#jGrid").jqxGrid('updatebounddata', 'cells');

        $("#jGrid").jqxGrid("autoresizecolumns");

        var colDefs = $("#jGrid").jqxGrid('columns').records;
        for (var idx = 0; idx < colDefs.length; idx++) {

            if (colDefs[idx].datafield != "_checkboxcolumn") {
                $("#jGrid").jqxGrid('setcolumnproperty',
                                        colDefs[idx].datafield,
                                        'width',
                                        colDefs[idx].width + 5);
            }

            if (colDefs[idx].datafield == "UUID") {
                $("#jGrid").jqxGrid('setcolumnproperty',
                                        colDefs[idx].datafield,
                                        'width',
                                        colDefs[idx].width + 10);
            }
        }

        $("#jGrid").jqxGrid('setcolumnproperty', "Albaranes", 'width', 63);
        $("#jGrid").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
        $("#jGrid").jqxGrid('setcolumnproperty', "OBSERVACION1", 'width', 76);
        $("#jGrid").jqxGrid('setcolumnproperty', "INCIDENCIA1", 'width', 70);
        $("#jGrid").jqxGrid('setcolumnproperty', "Link", 'width', 65);
        $("#jGrid").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
        $("#jGrid").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
        $("#jGrid").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
        $("#jGrid").jqxGrid('setcolumnproperty', "IMPUESTOS", 'width', 90);

        if (tmpContr != "") {

            var asociadas = data.filter(function (x) {
                //console.log(x.ESTATUS_ID);
                return x.ESTATUS_ID == "2";
            });

            if (!$.isEmptyObject(asociadas)) {
                //console.table(asociadas,["ID","ESTATUS_ID"]);
                $("#bDesvalidar").button("enable");
            }

        }

        $('#init').addClass('no-show');

    }, 30);
    //window.open("excel.aspx", "xml", "width=200,height=150");
}