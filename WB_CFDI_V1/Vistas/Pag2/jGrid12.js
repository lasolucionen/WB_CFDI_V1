source12 =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'BLOQ' },
                        { name: 'NUMSERIE' },
                        { name: 'NUMALBARAN' },
                        { name: 'NIF20' },
                        { name: 'FECHAALBARAN', type: 'date' },
                        { name: 'SUALBARAN' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'ST' },
                        { name: 'ESTADO' },
                    ],
                    localdata: []
                };
var dataAdapter12 = new $.jqx.dataAdapter(source12);

$("#jGrid12").jqxGrid({
    width: '100%',
    height: 335,
    source: dataAdapter12,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
    //selectionmode: 'checkbox',
    enablebrowserselection: true,
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

    ready: function () {

    },
    columns: [
                        { text: 'NUMSERIE', dataField: 'NUMSERIE', width: 90, renderer: columnrenderer2, cellsrenderer: cellB }, //, pinned: true 
                        {text: 'NUMALBARAN', dataField: 'NUMALBARAN', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB },
                        { text: 'SUALBARAN', dataField: 'SUALBARAN', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB},
                        { text: 'RFC',       dataField: 'NIF20', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB },
                        { text: 'TOTAL', dataField: 'TOTAL', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'SUBTOTAL', dataField: 'SUBTOTAL', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB},
                        { text: 'IVA', dataField: 'IVA', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        { text: 'IEPS', dataField: 'IEPS', width: 90, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB },
                        {
                            text: 'DETALLE',
                            datafield: 'detalle',
                            columntype: 'button',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return '--';
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid12").jqxGrid('getrowdata', row);

                                $("#cSubTotal").html(numeral(dataRecord.SUBTOTAL).format('$0,0.00'));
                                $("#cTotal").html(numeral(dataRecord.TOTAL).format('$0,0.00'));
                                $("#cIVA").html(numeral(dataRecord.IVA).format('$0,0.00'));
                                $("#cIEPS").html(numeral(dataRecord.IEPS).format('$0,0.00'));

                                $('#init').removeClass('no-show');

                                //$("#jGrid12").dialog({ width: Math.min(800, $(window).width()) });
                                $("#detalle2").dialog({ width: 930 });
                                $("#detalle2").dialog('open');

                                source5.localdata = [];
                                dataAdapter5.dataBind();
                                $("#jGrid5").jqxGrid('clearselection');

                                source5.localdata = getData("Pag2.aspx/Detalle", { data: dataRecord });
                                dataAdapter5.dataBind();
                                $("#jGrid5").jqxGrid('ensurerowvisible', 0);


                                $("#jGrid5").jqxGrid("autoresizecolumns");

                                var colDefs = $("#jGrid5").jqxGrid('columns').records;
                                for (var idx = 0; idx < colDefs.length; idx++) {
                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid5").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                                    }
                                }

                                $("#jGrid5").jqxGrid('setcolumnproperty', "DESCRIPCION", 'width', 200);

                                $('#init').addClass('no-show');

                            }
                        },
                        { text: 'FECHAALBARAN', dataField: 'FECHAALBARAN', width: 100, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt', cellsrenderer: cellB },
                        /*{ text: 'ST', dataField: 'ST', width: 90, renderer: columnrenderer2, cellsrenderer: cellB },*/
                        { text: 'ESTADO', dataField: 'ESTADO', width: 90, renderer: columnrenderer2, cellsrenderer: cellB }
                    ]
});