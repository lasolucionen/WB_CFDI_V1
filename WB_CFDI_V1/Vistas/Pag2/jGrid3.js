source3 =
{
    datatype: "json",
    datafields: [
        { name: 'UUID' },
        { name: 'CONTRA_RECIBO_ID' },
        { name: 'TOTAL' },
        { name: 'SUBTOTAL' },
        { name: 'IVA' },
        { name: 'IEPS' },
        { name: 'RETENCIONES' },
        { name: 'DESCUENTO' },
        { name: 'SERIE' },
        { name: 'FOLIO' },
    ],
    localdata: []
};

var dataAdapter3 = new $.jqx.dataAdapter(source3);

$("#jGrid3").jqxGrid({
    width: '691px',
    //width: '100%',
    height: 450,
    source: dataAdapter3,
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
                        { text: 'CONTR',       dataField: 'CONTRA_RECIBO_ID', width: 130, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                        { text: 'TOTAL',       dataField: 'TOTAL',            width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'SUBTOTAL',    dataField: 'SUBTOTAL',         width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'IVA',         dataField: 'IVA',              width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'IEPS',        dataField: 'IEPS',             width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'RETENCIONES', dataField: 'RETENCIONES',      width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'DESCUENTO',   dataField: 'DESCUENTO',        width: 130, renderer: columnrenderer2, cellsformat: 'C2', cellsalign: 'center' }, //, pinned: true 
                        { text: 'SERIE',       dataField: 'SERIE',            width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO',       dataField: 'FOLIO',            width: 130, renderer: columnrenderer2, cellsalign: 'center' }

                    ]
});