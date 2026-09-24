var cntr = 0;
var info;

var cellclass = function (row, datafield, value, rowdata) {

    if (rowdata["ORACLE"] == true) {
        return "cellGreen";
    } else {
        return "";
    }


}

source3 =
{
    datatype: "json",
    datafields: [
        { name: 'ID' },
        { name: 'CERRADO' },
        { name: 'ORACLE' },
        { name: 'FECHA', type: 'date' },
        { name: 'FECHA_PAGO', type: 'date' },
        { name: 'TOTAL' }
    ],
    localdata: []
};

var dataAdapter3 = new $.jqx.dataAdapter(source3);

$("#jGrid3").jqxGrid({
    width: '100%',
    height: 480,
    source: dataAdapter3,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    //selectionmode: 'singlerow',
    enablebrowserselection: true,
    selectionmode: 'checkbox',
    //rowsheight: 36,
    //rowdetails: true,
    //rowdetailstemplate: { rowdetails: "<div style='padding: 10px;'>Prueba</div>", rowdetailsheight: 100 },
    //selectionmode: 'none',
    //filterable: false,
    //showfilterrow: true,
    //rowsheight: 36,
    //altrows: true,
    //autorowheight: true,
    //pageable: true,
    //columnsheight: 40,
    columns: [
        { text: 'CONTRARECIBO', dataField: 'ID', width: 102, renderer: columnrenderer2, cellsalign: 'center', cellclassname: cellclass },
        {
            text: 'FECHA',
            dataField: 'FECHA',
            width: 160,
            renderer: columnrenderer2,
            cellsalign: 'center',
            cellsformat: 'dd/MM/yyyy hh:mm:ss tt'
        },
        {
            text: 'Facturas',
            datafield: 'Show1',
            columntype: 'button',
            width: 60,
            renderer: columnrenderer2,
            cellsrenderer: function () {
                return "--";
            },
            buttonclick: function (row) {

                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);

                $('#init').removeClass('no-show');

                // Creamos el handler 'Quitar' como función reutilizable y la asociamos solo si el permiso existe.
                var quitarHandler = function () {
                    var position = $("#jGrid4").jqxGrid('scrollposition');
                    var selectedrowindexes = $("#jGrid4").jqxGrid('getselectedrowindexes');
                    var rowscount = $("#jGrid4").jqxGrid('getdatainformation').rowscount;

                    var proc = true;

                    if (selectedrowindexes.length > 0) {

                        var select1 = new Array();
                        var arrayOfSelectedIds = [];

                        selectedrowindexes.sort();

                        for (var m = 0; m < selectedrowindexes.length; m++) {
                            var selectedrowindex = selectedrowindexes[selectedrowindexes.length - m - 1];
                            if (selectedrowindex >= 0 && selectedrowindex < rowscount) {

                                var id = $("#jGrid4").jqxGrid('getrowid', selectedrowindex);
                                arrayOfSelectedIds.push(id);

                                var data = $('#jGrid4').jqxGrid('getrowdata', selectedrowindex);

                                if (data.ESTATUS_ID == 2 || data.ESTATUS_ID == 6) {
                                    proc = false;
                                } else {
                                    select1.push({ ID: data.ID });
                                }
                            }
                        }

                        $("#confirmar2").dialog({
                            buttons: {
                                'Quitar': function () {
                                    $("#jGrid4").jqxGrid('beginupdate');

                                    $('#init').removeClass('no-show');

                                    var rs = getData("Pag3.aspx/Quitar",
                                        {
                                            data: select1,
                                            id: dataRecord.ID,
                                            incidencia_msg: $("#jArea").val()
                                        });

                                    if (rs.code == 0 || rs.code == 1) {

                                        $("#jGrid4").jqxGrid('deleterow', arrayOfSelectedIds);
                                        buscar();

                                        if (rs.code == 1) {
                                            buscar2();
                                        }

                                    } else {
                                        $("#message").html("<div style='margin-top:15px;'>" +
                                            rs.msg +
                                            "</div>");
                                        $("#message").dialog('open');
                                    }

                                    $("#jGrid4").jqxGrid('endupdate');

                                    $("#jGrid4").jqxGrid('clearselection');

                                    $('#jGrid4').jqxGrid('scrolloffset', position.top, 0);

                                    $("#jGrid4").jqxGrid("autoresizecolumns");

                                    var colDefs = $("#jGrid4").jqxGrid('columns').records;
                                    for (var idx = 0; idx < colDefs.length; idx++) {

                                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                                            $("#jGrid4").jqxGrid('setcolumnproperty',
                                                colDefs[idx].datafield,
                                                'width',
                                                colDefs[idx].width + 5);
                                        }

                                        if (colDefs[idx].datafield == "UUID") {
                                            $("#jGrid4").jqxGrid('setcolumnproperty',
                                                colDefs[idx].datafield,
                                                'width',
                                                colDefs[idx].width + 10);
                                        }
                                    }

                                    $("#jGrid4").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                                    $("#jGrid4").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                                    $("#jGrid4").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                                    $("#jGrid4").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);

                                    $('#init').addClass('no-show');

                                    $(this).dialog("close");
                                },
                                'Cancelar': function () {
                                    $(this).dialog("close");
                                }
                            }
                        });

                        if (proc) {

                            $("#jArea").val('');
                            $("#confirmar2").dialog('open');
                        } else {
                            $("#message")
                                .html(
                                    "<div style='text-align: center;min-width:250px;margin-top:20px;'>Desmarque las facturas con estatus de asociadas o pagadas.</div>");
                            $("#message").dialog('open');
                        }
                    }
                };

                // Construimos los botones del diálogo #detalle según permiso (jsMostrarAgregarQuitar)
                var detalleButtons = {
                    'Cerrar': function () {
                        $(this).dialog("close");
                    }
                };

                if (typeof jsMostrarAgregarQuitar !== 'undefined' && jsMostrarAgregarQuitar === true) {
                    detalleButtons = {
                        'Quitar': quitarHandler,
                        'Cerrar': function () {
                            $(this).dialog("close");
                        }
                    };
                }

                $("#detalle").dialog({
                    buttons: detalleButtons
                });

                source4.localdata = [];
                dataAdapter4.dataBind();
                $("#jGrid4").jqxGrid('updatebounddata', 'cells');

                $("#detalle").dialog({ width: Math.min(950, $(window).width()) });
                $("#detalle").dialog('open');

                source4.localdata = getData("Pag3.aspx/Detalle", { id: dataRecord.ID });
                dataAdapter4.dataBind();
                $("#jGrid4").jqxGrid('ensurerowvisible', 0);
                $("#jGrid4").jqxGrid('clearselection');

                $("#jGrid4").jqxGrid("autoresizecolumns");

                var colDefs = $("#jGrid4").jqxGrid('columns').records;
                for (var idx = 0; idx < colDefs.length; idx++) {

                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                        $("#jGrid4").jqxGrid('setcolumnproperty',
                            colDefs[idx].datafield,
                            'width',
                            colDefs[idx].width + 5);
                    }

                    if (colDefs[idx].datafield == "UUID") {
                        $("#jGrid4").jqxGrid('setcolumnproperty',
                            colDefs[idx].datafield,
                            'width',
                            colDefs[idx].width + 10);
                    }
                }
                $("#jGrid4").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                $("#jGrid4").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                $("#jGrid4").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                $("#jGrid4").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);

                $('#init').addClass('no-show');

            }
        },
        {
            text: 'PDF',
            datafield: 'Show2',
            columntype: 'button',
            width: 60,
            renderer: columnrenderer2,
            cellsrenderer: function () {
                return "..";
            },
            buttonclick: function (row) {

                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);

                $('#init').removeClass('no-show');

                getData("contrareciboPDF.aspx/SetID",
                    {
                        id: dataRecord.ID
                    });

                window.open("contrareciboPDF.aspx", "xml", "width=200,height=150");

                $('#init').addClass('no-show');

            }
        },
        {
            text: 'PDF Carta Pago',
            datafield: 'Show3',
            columntype: 'button',
            width: 100,
            renderer: columnrenderer2,
            cellsrenderer: function () {
                return "..";
            },
            buttonclick: function (row) {

                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);

                $('#init').removeClass('no-show');

                var rs = getData("cartaPagoPDF.aspx/SetID",
                    {
                        id: dataRecord.ID
                    });


                if (rs == "") {
                    window.open("cartaPagoPDF.aspx", "xml", "width=200,height=150");
                } else {
                    $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" + rs + "</div>");
                    $("#message").dialog('open');
                }

                $('#init').addClass('no-show');

            }
        },
        {
            text: 'Carta Pago',
            datafield: 'Show4',
            columntype: 'button',
            width: 75,
            renderer: columnrenderer2,
            cellsrenderer: function () {
                return "..";
            },
            buttonclick: function (row) {

                var dataRecord = $("#jGrid3").jqxGrid('getrowdata', row);
                cntr = dataRecord.ID;

                info = getData("Pag3.aspx/InfoCartaP",
                    {
                        id: cntr
                    });


                if (info.oracle) {
                    $("#btAbrirCartaP").button("enable");
                    $("#btCerrarCartaP").button("disable");

                } else {
                    $("#btAbrirCartaP").button("disable");
                    $("#btCerrarCartaP").button("enable");

                }


                $("#CartaPago").dialog({
                    buttons: {
                        'Cerrar': function () {

                            $(this).dialog("close");
                        }
                    }
                });



                $("#CartaPago").dialog({
                    title: "Contrarecibo: " + cntr
                });

                $("#CartaPago").dialog('open');

                position('.mWin2');



            }, hidden: !cerrarCarta //true
        },
        {
            text: 'VENCIM',
            dataField: 'FECHA_PAGO',
            width: 90,
            renderer: columnrenderer2,
            cellsalign: 'center',
            cellsformat: 'dd/MM/yyyy'
        },
        { text: 'TOTAL', dataField: 'TOTAL', width: 102, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },

    ]
});