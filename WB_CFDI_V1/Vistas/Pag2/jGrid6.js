source6 =
{
    datatype: "json",
    datafields: [
        { name: 'CANTIDAD' },
        { name: 'UNIDAD' },
        { name: 'CLAVE' },
        { name: 'DESCRIPCION' },
        { name: 'PU' },
        { name: 'IMPORTE' },
        { name: 'DESCUENTO' }
    ],
    localdata: []
};

var dataAdapter6 = new $.jqx.dataAdapter(source6);

$("#jGrid6").jqxGrid({
    width: 640, //1017
    height: 450,
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
                { text: 'CANT',        dataField: 'CANTIDAD',    width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'UNI',         dataField: 'UNIDAD',      width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'CLAVE',       dataField: 'CLAVE',       width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'DESCRIPCION', dataField: 'DESCRIPCION', width: 100, renderer: columnrenderer2 }, //, pinned: true 
                { text: 'P/U',         dataField: 'PU',          width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }, //, pinned: true 
                { text: 'IMPORTE',     dataField: 'IMPORTE',     width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' },
                { text: 'DESC',        dataField: 'DESCUENTO',   width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }

            ]
});