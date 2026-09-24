source8 =
{
    datatype: "json",
    datafields: [
        { name: 'ID' },
        { name: 'RFC_EMISOR' },
        //{ name: 'RAZON_SOCIAL_EMISOR' },
        { name: 'CONTRA_RECIBO_ID' },
        { name: 'UUID' },
        { name: 'SERIE' },
        { name: 'FOLIO' },
        { name: 'TIENDA' },
        //{ name: 'VERSION' },
        { name: 'TOTAL' },
        { name: 'SUBTOTAL' },
        { name: 'IMPUESTOS' },
        { name: 'IVA' },
        { name: 'IEPS' },
        { name: 'FECHA_RECEPCION', type: 'date' },
        { name: 'FECHA_FACTURA', type: 'date' },
        { name: 'ESTATUS' },
        { name: 'ESTATUS_ID' }
    ],
    localdata: []
};

var dataAdapter8 = new $.jqx.dataAdapter(source8);

$("#jGrid8").jqxGrid({ width: 1000, height: 400,
    source: dataAdapter8,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'checkbox',
    enablebrowserselection: true,

    ready: function () {
        /*                        var colDefs = $("#jGrid").jqxGrid('columns').records;
        for ( var idx = 0; idx < colDefs.length; idx++) {
        $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width+10);
        }*/
        //$("#jGrid").jqxGrid('autoresizecolumns');
        //$("#jGrid").find('.jqx-grid-column-header:first').children().hide();

    },
    //selectionmode: 'none',
    //filterable: false,
    //showfilterrow: true,
    //rowsheight: 36,
    //altrows: true,
    //autorowheight: true,
    //pageable: true,
    //columnsheight: 40,
    columns: [
                        { text: 'CONTR', dataField: 'CONTRA_RECIBO_ID', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL', dataField: 'TOTAL', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL', dataField: 'SUBTOTAL', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS', dataField: 'IMPUESTOS', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA', dataField: 'IVA', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS', dataField: 'IEPS', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        {
                            text: 'Detalle',
                            datafield: 'Detalle',
                            columntype: 'button',
                            width: 45,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);

                                $("#cSubTotal2").html(numeral(dataRecord.SUBTOTAL).format('$0,0.00'));
                                $("#cTotal2").html(numeral(dataRecord.TOTAL).format('$0,0.00'));
                                $("#cIVA2").html(numeral(dataRecord.IVA).format('$0,0.00'));
                                $("#cIEPS2").html(numeral(dataRecord.IEPS).format('$0,0.00'));

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);

                                //$("#detalle3").dialog({ width: Math.min(340, $(window).width()) });

                                source6.localdata = getData("Pag2.aspx/GetDetalle", { id: dataRecord.ID });
                                dataAdapter6.dataBind();
                                $("#jGrid6").jqxGrid('ensurerowvisible', 0);
                                $("#jGrid6").jqxGrid('clearselection');

                                $("#jGrid6").jqxGrid("autoresizecolumns");

                                var colDefs = $("#jGrid6").jqxGrid('columns').records;
                                for (var idx = 0; idx < colDefs.length; idx++) {
                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid6").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                                    }
                                }

                                $("#detalle3").dialog('open');

                                $('#init').addClass('no-show');

                            }
                        },
                        { text: 'SERIE', dataField: 'SERIE', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO', dataField: 'FOLIO', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA', dataField: 'TIENDA', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'ESTATUS', dataField: 'ESTATUS', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA', dataField: 'FECHA_FACTURA', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'FECHA RECEPCION', dataField: 'FECHA_RECEPCION', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'UUID', dataField: 'UUID', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
    //{ text: 'VERSION',             dataField: 'VERSION',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                        text: 'XML',
                        datafield: 'Show1',
                        columntype: 'button',
                        width: 60,
                        renderer: columnrenderer2,
                        cellsrenderer: function () {
                            return "--";
                        },
                        buttonclick: function (row) {

                            var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);

                            $('#init').removeClass('no-show');

                            //alert(dataRecord.ID);
                            getData("archivoXML.aspx/SetID", {
                                id: dataRecord.ID
                            });

                            window.open("archivoXML.aspx", "xml", "width=200,height=150");

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
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                getData("archivoPDF.aspx/SetID", {
                                    id: dataRecord.ID
                                });

                                window.open("archivoPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        }
                        ,
                        {
                            text: 'PDF Prov',
                            datafield: 'Show3',
                            columntype: 'button',
                            width: 70,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid8").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                var pdf = getData("archivoPDF_Prov.aspx/SetID",
                                    {
                                        id: dataRecord.ID
                                    });

                                if (!$.isEmptyObject(pdf)) {
                                    window.open("archivoPDF_Prov.aspx", "xml", "width=200,height=150");
                                } else {
                                    $("#message").html("<div style='margin-top: 7px;font-weight: bold;font-size:12px'>El registro seleccionado<br />no contiene pdf de proveedor.</div>");
                                    $("#message").dialog('open');

                                    position('.mWin');
                                }

                                $('#init').addClass('no-show');

                            }
                        }
                    ]
});