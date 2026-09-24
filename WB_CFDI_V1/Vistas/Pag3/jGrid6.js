source6 =
{
    datatype: "json",
    datafields: [
        { name: 'Impuesto' },
        { name: 'TasaOCuota' },
        { name: 'Importe' }
    ],
    localdata: []
};

var dataAdapter6 = new $.jqx.dataAdapter(source6);

$("#jGrid6").jqxGrid({
    width: '100%',
    height: 300,
    source: dataAdapter6,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
    enablebrowserselection: true,
    //selectionmode: 'checkbox',
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
        { text: 'IMPUESTO', dataField: 'Impuesto', width: 100, renderer: columnrenderer2 }, //, pinned: true 
        {text: 'TASA O CUOTA', dataField: 'TasaOCuota', width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }, //, pinned: true 
        {text: 'IMPORTE', dataField: 'Importe', width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }

    ]
});