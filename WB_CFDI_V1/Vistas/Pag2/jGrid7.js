source7 =
{
    datatype: "json",
    datafields: [
        { name: 'ARCHIVO' },
        { name: 'PDF' },
        { name: 'DETALLE' },
        { name: 'MENSAJE' }
    ],
    localdata: []
};

var dataAdapter7 = new $.jqx.dataAdapter(source7);

$("#jGrid7").jqxGrid({
    width: '100%',
    height: 390,
    source: dataAdapter7,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
    enablebrowserselection: true,
    rowsheight: 65,
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
                        { text: 'ARCHIVO', dataField: 'ARCHIVO', width: 340, renderer: columnrenderer2 }, //, pinned: true 
                        {text: 'PDF', dataField: 'PDF', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'DETALLE',
                            dataField: 'DETALLE',
                            width: 290,
                            renderer: columnrenderer2,
                            cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                            }
                        },
                        {
                            text: 'Validacion XML',
                            dataField: 'MENSAJE',
                            width: 280,
                            renderer: columnrenderer2,
                            cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                                return '<div style="margin: 5px;">' + value + '</div>';

                            }
                        }
                    ]
});