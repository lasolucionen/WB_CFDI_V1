var jGrid17Rendered = function () {
    
    //$("#jGrid17").jqxGrid('clearselection');
    //$("#jGrid17").jqxGrid('updatebounddata', 'cells');
    //$("#jGrid17").jqxGrid('ensurerowvisible', 0);
    $("#jGrid17").jqxGrid("autoresizecolumns");



}



source17 =
{
    datatype: "json",
    datafields: [
        { name: 'NUMSERIE' },
        { name: 'NUMALBARAN' },
        //{ name: 'NUMLIN' },
        { name: 'REFERENCIA' },
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

var dataAdapter17 = new $.jqx.dataAdapter(source17);

$("#jGrid17").jqxGrid({
    width: '100%',
    height: 450,
    source: dataAdapter17,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    selectionmode: 'singlerow',
    rendered: jGrid17Rendered,
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
        { text: 'NUMSERIE',      dataField: 'NUMSERIE',      width: 70, renderer: columnrenderer2 }, //, pinned: true 
        { text: 'NUMALBARAN',    dataField: 'NUMALBARAN',    width: 90,  renderer: columnrenderer2, cellsalign: 'center' },
        //{ text: 'NUMLIN',     dataField: 'NUMLIN',        width: 70, renderer: columnrenderer2, cellsalign: 'center' },
        { text: 'Ref',           dataField: 'REFERENCIA', width: 90, renderer: columnrenderer17 },
        { text: 'Descripcion',   dataField: 'DESCRIPCION', width: 130, renderer: columnrenderer17},
        { text: 'Cant',          dataField: 'UNIDADESTOTAL', width: 130, renderer: columnrenderer17, cellsalign: 'center' },
        { text: 'Precio',        dataField: 'PRECIO', width: 90, renderer: columnrenderer17, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'DTO',           dataField: 'DTO', width: 90, renderer: columnrenderer17, cellsalign: 'center' },
        { text: 'Total',         dataField: 'TOTAL', width: 90, renderer: columnrenderer17, cellsalign: 'center', cellsformat: 'C2' },
        { text: 'TipoImp',       dataField: 'TIPOIMPUESTO', width: 90, renderer: columnrenderer17, cellsalign: 'center' },
        { text: 'ALM',           dataField: 'CODALMACEN', width: 90, renderer: columnrenderer17, cellsalign: 'center' },
        { text: 'fecha albaran', dataField: 'FECHA_ALBARAN', width: 90, renderer: columnrenderer17, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
        { text: 'ult costo',     dataField: 'ULTIMO_COSTO_ACTUALIZADO', width: 90, renderer: columnrenderer17, cellsalign: 'center' },
        { text: 'fecha',         dataField: 'FECHA_ACTUALIZACION_STR', width: 90, renderer: columnrenderer17, cellsalign: 'center' },
        { text: 'centralizador', dataField: 'CENTRALIZADOR_DE_COSTOS', width: 90, renderer: columnrenderer17, cellsalign: 'center' },


    ]
});



$("#jGrid17").on('rowselect', function (event) {

    var inx = event.args.rowindex;

    //jGrid2

    //$("#jGrid2").jqxGrid('selectrow', 0);

    var data = $("#jGrid17").jqxGrid('getrowdata', inx);

    $("#serieFolio").html(data.NUMSERIE + " - " + data.NUMALBARAN);


    var rowindexes2 = $("#jGrid2").jqxGrid('getselectedrowindexes');

    _chkAlb = true;


    var unchk = true;

    if (rowindexes2.length > 0) {

        for (i = rowindexes2.length - 1; i >= 0; i--) {

            if (source2.localdata[rowindexes2[i]].NUMSERIE == data.NUMSERIE &&
                source2.localdata[rowindexes2[i]].NUMALBARAN == data.NUMALBARAN) {

                unchk = false;
                var checked = $("#chkAlb").jqxCheckBox('checked');
                if (!checked) {
                    $("#chkAlb").jqxCheckBox('check');
                }

                break;
            }
        }

    }

    if (unchk) {
        $("#chkAlb").jqxCheckBox('uncheck');
    }

     _chkAlb = false;

});