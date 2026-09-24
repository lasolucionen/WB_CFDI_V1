source2 =
{
    datatype: "json",
    datafields: [
        { name: 'ID' },
        //{ name: 'RFC_EMISOR' },
        //{ name: 'RAZON_SOCIAL_EMISOR' },
        { name: 'UUID' },
        { name: 'SERIE' },
        { name: 'FOLIO' },
        { name: 'TIENDA' },
        { name: 'NO_COMPRA' },
        { name: 'FECHA_COMPRA', type: 'date' },
        //{ name: 'REVISADO' },
        //{ name: 'VERSION' },
        { name: 'TOTAL' },
        { name: 'SUBTOTAL' },
        { name: 'IMPUESTOS' },
        { name: 'IVA' },
        { name: 'IEPS' },
        { name: 'FECHA_RECEPCION', type: 'date' },
        { name: 'FECHA_FACTURA', type: 'date' },
        { name: 'ESTATUS' },
        { name: 'CONTRA_RECIBO' },
        { name: 'ESTATUS_ID' }
    ],
    localdata: []
};

var dataAdapter2 = new $.jqx.dataAdapter(source2);

$("#jGrid2").jqxGrid({
    width: '100%',
    height: 480,
    source: dataAdapter2,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'checkbox',
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
        //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
        { text: 'TOTAL',           dataField: 'TOTAL',           width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'SUBTOTAL',        dataField: 'SUBTOTAL',        width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'IMPUESTOS',       dataField: 'IMPUESTOS',       width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'IVA',             dataField: 'IVA',             width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'IEPS',            dataField: 'IEPS',            width: 125, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'SERIE',           dataField: 'SERIE',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
        { text: 'FOLIO',           dataField: 'FOLIO',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
        { text: 'TIENDA',          dataField: 'TIENDA',          width: 100, renderer: columnrenderer2, cellsalign: 'center' },
        { text: '#COMPRA',         dataField: 'NO_COMPRA',       width: 100, renderer: columnrenderer2, cellsalign: 'center' },
        { text: 'FECHA COMPRA',    dataField: 'FECHA_COMPRA',    width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
        //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
        {text: 'ESTATUS',          dataField: 'ESTATUS',         width: 130, renderer: columnrenderer2, cellsalign: 'center' },
        { text: 'FECHA FACTURA',   dataField: 'FECHA_FACTURA',   width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
        { text: 'FECHA RECEPCION', dataField: 'FECHA_RECEPCION', width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
        { text: 'UUID',            dataField: 'UUID',            width: 300, renderer: columnrenderer2, cellsalign: 'center' }

               ]
});