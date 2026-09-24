source9 =
{
    datatype: "json",
    datafields: [
        { name: 'SERIE' },
        { name: 'FOLIO' }
    ],
    localdata: []
};

var dataAdapter9 = new $.jqx.dataAdapter(source9);

$("#jGrid9").jqxGrid({
    width: 325,
    height: 390,
    source: dataAdapter9,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
    enablebrowserselection: true,
    rowsheight: 36,
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
        { text: 'SERIE', dataField: 'SERIE', width: 145, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
        {text: 'FOLIO', dataField: 'FOLIO', width: 145, renderer: columnrenderer2, cellsalign: 'center' }

    ]
});