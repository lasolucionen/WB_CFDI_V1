source11 =
{
    datatype: "json",
    datafields: [
        { name: 'NUMSERIE' },
        { name: 'NUMALBARAN' }
    ],
    localdata: []
};

var dataAdapter11 = new $.jqx.dataAdapter(source11);

$("#jGrid11").jqxGrid({
    width: 325,
    height: 390,
    source: dataAdapter11,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
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
    columns: [
        { text: 'NUMSERIE', dataField: 'NUMSERIE', width: 145, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
        {text: 'NUMALBARAN', dataField: 'NUMALBARAN', width: 145, renderer: columnrenderer2, cellsalign: 'center' }

    ]
});