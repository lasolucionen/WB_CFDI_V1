source15 =
{
    datatype: "json",
    datafields: [
        { name: 'UUID' },
        { name: 'CANTIDAD' },
        { name: 'UNIDAD' },
        { name: 'CLAVE' },
        { name: 'DESCRIPCION' },
        { name: 'PU' },
        { name: 'IMPORTE' },
        { name: 'DESCUENTO' },

        { name: 'REFERENCIA' },
        { name: 'UNIDADESTOTAL' },
        { name: 'DESCRIPCION2' },
        { name: 'TOTAL' },
        { name: 'REQ' },
        { name: 'IVA' },
        { name: 'TIPOIMPUESTO' },
        { name: 'PRECIO'},
        { name: 'PRECIO_IMP'},
        { name: 'AJUSTE_DESCRIPCION' },


        { name: 'NUMSERIEO' },
        { name: 'NUMALBARANO' },
        { name: 'CODARTICULO' },
    ],
    localdata: []
};

var dataAdapter15 = new $.jqx.dataAdapter(source15);

$("#jGrid15").jqxGrid({
    width: '100%', //1017
    height: 242,
    source: dataAdapter15,
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
                /*{ text: 'CANT',        dataField: 'CANTIDAD',    width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'UNI',         dataField: 'UNIDAD',      width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true */
                { text: 'CLAVE',       dataField: 'CLAVE',       width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'DESCRIPCION', dataField: 'DESCRIPCION', width: 100, renderer: columnrenderer2 }, //, pinned: true 
                /*{ text: 'P/U',         dataField: 'PU',          width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' }, //, pinned: true */
                { text: 'IMPORTE',     dataField: 'IMPORTE',     width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                /*{ text: 'DESC',        dataField: 'DESCUENTO',   width: 100, renderer: columnrenderer2, cellsformat: 'N2', cellsalign: 'center' },*/

                { text: 'Ref', dataField: 'REFERENCIA', width: 90, renderer: columnrenderer2 },
                { text: 'DESCRIPCION2', dataField: 'DESCRIPCION2', width: 130, renderer: columnrenderer2, cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                    return '<div style="margin: 5px;">' + value + '</div>';

                } 
                },
                /*{ text: 'Cant', dataField: 'UNIDADESTOTAL', width: 130, renderer: columnrenderer2, cellsalign: 'center' },*/
                { text: 'Total', dataField: 'TOTAL', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'Req', dataField: 'REQ', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'Iva', dataField: 'IVA', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'TipoImp', dataField: 'TIPOIMPUESTO', width: 100, renderer: columnrenderer2, cellsalign: 'center' },

                { text: 'Ajuste', dataField: 'PRECIO', width: 100, renderer: columnrenderer2,  cellsalign: 'center' },
                { text: 'Ajuste_Imp', dataField: 'PRECIO_IMP', width: 100, renderer: columnrenderer2,  cellsalign: 'center' },
                { text: 'Numserie', dataField: 'NUMSERIEO', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'Numalbaran', dataField: 'NUMALBARANO', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'Tipo Ajuste', dataField: 'AJUSTE_DESCRIPCION', width: 100, renderer: columnrenderer2, cellsalign: 'center' },

            ]
});