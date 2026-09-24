source16 =
{
    datatype: "json",
    datafields: [
        { name: 'CODARTICULO' },
        { name: 'REFPROVEEDOR' },
        { name: 'DESCRIPCION' },
    ],
    localdata: []
};

var dataAdapter16 = new $.jqx.dataAdapter(source16);

$("#jGrid16").jqxGrid({
    width: 500, //1017
    height: 200,
    source: dataAdapter16,
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
                { text: 'CODARTICULO',  dataField: 'CODARTICULO',  width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'REFPROVEEDOR', dataField: 'REFPROVEEDOR', width: 100, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true 
                { text: 'DESCRIPCION',  dataField: 'DESCRIPCION',  width: 100, renderer: columnrenderer2 }, //, pinned: true 

            ]
});


$("#jGrid16").on('rowselect', function (event) {

    var inx = event.args.rowindex;



    var data = $("#jGrid16").jqxGrid('getrowdata', inx);

    $("#tpAjuste").html(data.DESCRIPCION);
});