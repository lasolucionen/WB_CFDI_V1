source5 =
                {
                    datatype: "json",
                    datafields: [
                    //{ name: 'NUMSERIE' },
                    //{ name: 'NUMALBARAN' },
                    //{ name: 'NUMLIN' },
                        {name: 'REFERENCIA' },
                        { name: 'UNIDADESTOTAL' },
                        { name: 'DESCRIPCION' },
                        { name: 'UNIDADESTOTAL' },
                        { name: 'PRECIO' },
                        { name: 'DTO' },
                        { name: 'TOTAL' },
                        { name: 'TIPOIMPUESTO' },
                        { name: 'CODALMACEN' },
                        { name: 'FECHA_ALBARAN', type: 'date' },
                        { name: 'ULTIMO_COSTO_ACTUALIZADO' },
                        { name: 'FECHA_ACTUALIZACION_STR' },
                        { name: 'CENTRALIZADOR_DE_COSTOS' },
                        { name: 'COLOR' },
                        { name: 'COLOR2' },
                    ],
                    localdata: []
                };

var dataAdapter5 = new $.jqx.dataAdapter(source5);

$("#jGrid5").jqxGrid({
    width: '100%',
    height: 450,
    source: dataAdapter5,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
    enablebrowserselection: true,
    //selectionmode: 'checkbox',
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
    //{ text: 'NUMSERIE',   dataField: 'NUMSERIE',      width: 70, renderer: columnrenderer2 }, //, pinned: true 
    //{ text: 'NUMALBARAN', dataField: 'NUMALBARAN',    width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
    //{ text: 'NUMLIN',     dataField: 'NUMLIN',        width: 70, renderer: columnrenderer2, cellsalign: 'center' },
                        {text: 'Ref', dataField: 'REFERENCIA', width: 90, renderer: columnrenderer2 },
                        { text: 'Descripcion', dataField: 'DESCRIPCION', width: 130, renderer: columnrenderer2, cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                            return '<div style="margin: 5px;">' + value + '</div>';

                        } 
                        },
                        { text: 'Cant',           dataField: 'UNIDADESTOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'Precio',         dataField: 'PRECIO',                   width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'DTO',            dataField: 'DTO',                      width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellclassname: cellclass4 },
                        { text: 'Total',          dataField: 'TOTAL',                    width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'TipoImp',        dataField: 'TIPOIMPUESTO',             width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'ALM',            dataField: 'CODALMACEN',               width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'fecha albaran',  dataField: 'FECHA_ALBARAN',            width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        { text: 'ult costo',      dataField: 'ULTIMO_COSTO_ACTUALIZADO', width: 90,  renderer: columnrenderer2, cellsalign: 'center', cellclassname: cellclass3 },
                        { text: 'fecha',          dataField: 'FECHA_ACTUALIZACION_STR',  width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'centralizador',  dataField: 'CENTRALIZADOR_DE_COSTOS',  width: 90,  renderer: columnrenderer2, cellsalign: 'center' },


                    ]
});