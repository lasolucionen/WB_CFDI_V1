source4 =
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
        //{ name: 'VERSION' },
        { name: 'TOTAL' },
        { name: 'SUBTOTAL' },
        { name: 'IVA' },
        { name: 'IEPS' },
        { name: 'IMPUESTOS' },
        { name: 'FECHA_RECEPCION', type: 'date' },
        { name: 'FECHA_FACTURA', type: 'date' },
        { name: 'ESTATUS' },
        { name: 'CONTRA_RECIBO_ID' },
        { name: 'ESTATUS_ID' }
    ],
    localdata: []
};

var dataAdapter4 = new $.jqx.dataAdapter(source4);

$("#jGrid4").jqxGrid({
    width: '100%',
    height: 450,
    source: dataAdapter4,
    theme: 'Theme2',
    sortable: true,
    enableHover: false,
    //selectionmode: 'singlerow',
    selectionmode: 'checkbox',
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
    //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        {text: 'CONTR', dataField: 'CONTRA_RECIBO_ID', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL', dataField: 'TOTAL', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL', dataField: 'SUBTOTAL', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS', dataField: 'IMPUESTOS', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA', dataField: 'IVA', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS', dataField: 'IEPS', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
    /*                        {
    text: 'Detalle',
    datafield: 'Detalle',
    columntype: 'button',
    width: 45,
    renderer: columnrenderer2,
    cellsrenderer: function() {
    return "--";
    },
    buttonclick: function(row) {

    var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);

    $('#init').removeClass('no-show');

    //alert(dataRecord.ID);

    $("#detalle3").dialog({ width: Math.min(340, $(window).width()) });
    $("#detalle3").dialog('open');

    source6.localdata = getData("Pag3.aspx/GetDetalle", { id: dataRecord.ID });
    dataAdapter6.dataBind();
    $("#jGrid6").jqxGrid('ensurerowvisible', 0);
    $("#jGrid6").jqxGrid('clearselection');

    $('#init').addClass('no-show');

    }
    },*/
                        {text: 'SERIE', dataField: 'SERIE', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO', dataField: 'FOLIO', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TIENDA', dataField: 'TIENDA', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: '#COMPRA', dataField: 'NO_COMPRA', width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA COMPRA', dataField: 'FECHA_COMPRA', width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
    //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        {text: 'ESTATUS', dataField: 'ESTATUS', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FECHA FACTURA', dataField: 'FECHA_FACTURA', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'FECHA RECEPCION', dataField: 'FECHA_RECEPCION', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt' },
                        { text: 'UUID', dataField: 'UUID', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
    //{ text: 'VERSION',             dataField: 'VERSION',             width: 130,  renderer: columnrenderer2, cellsalign: 'center' },
                        {
                        text: 'XML',
                        datafield: 'Show1',
                        columntype: 'button',
                        width: 60,
                        renderer: columnrenderer2,
                        cellsrenderer: function () {
                            return "--";
                        },
                        buttonclick: function (row) {

                            var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);

                            $('#init').removeClass('no-show');

                            //alert(dataRecord.ID);
                            getData("archivoXML.aspx/SetID", {
                                id: dataRecord.ID
                            });

                            window.open("archivoXML.aspx", "xml", "width=200,height=150");

                            $('#init').addClass('no-show');

                        }
                    },
                        {
                            text: 'PDF',
                            datafield: 'Show2',
                            columntype: 'button',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                getData("archivoPDF.aspx/SetID", {
                                    id: dataRecord.ID
                                });

                                window.open("archivoPDF.aspx", "xml", "width=200,height=150");

                                $('#init').addClass('no-show');

                            }
                        }
                        ,
                        {
                            text: 'PDF Prov',
                            datafield: 'Show3',
                            columntype: 'button',
                            width: 70,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);

                                $('#init').removeClass('no-show');

                                //alert(dataRecord.ID);
                                var pdf = getData("archivoPDF_Prov.aspx/SetID",
                                    {
                                        id: dataRecord.ID
                                    });

                                if (!$.isEmptyObject(pdf)) {
                                    window.open("archivoPDF_Prov.aspx", "xml", "width=200,height=150");
                                } else {
                                    $("#message").html("<div style='margin-top: 7px;font-weight: bold;font-size:12px'>El registro seleccionado<br />no contiene pdf de proveedor.</div>");
                                    $("#message").dialog('open');

                                    position('.mWin');
                                }

                                $('#init').addClass('no-show');

                            }
                        }
                    ]
});