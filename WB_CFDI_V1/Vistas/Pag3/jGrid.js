                source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'ID' },
                        //{ name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'FOLIO' },
                        //{ name: 'TIENDA' },
                        //{ name: 'NO_COMPRA' },
                        { name: 'ALBARAN' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'REVISADO' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'CONTRA_RECIBO' },
                        { name: 'ESTATUS_ID' },
                        { name: 'INCIDENCIA' },
                        { name: 'INCIDENCIA_MSG' },
                    ],
                    localdata: []
                };
                var dataAdapter = new $.jqx.dataAdapter(source);

                $("#jGrid").jqxGrid({ width: '100%', height: 450,
                    source: dataAdapter,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'checkbox',
                    enablebrowserselection :true,
                    ready: function () {
/*                        var colDefs = $("#jGrid").jqxGrid('columns').records;
                        for ( var idx = 0; idx < colDefs.length; idx++) {
                            $("#jGrid").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width+10);
                        }*/
                        //$("#jGrid").jqxGrid('autoresizecolumns');
                        //$("#jGrid").find('.jqx-grid-column-header:first').children().hide();

                    },
                    //selectionmode: 'none',
                    //filterable: false,
                    //showfilterrow: true,
                    rowsheight: 43,
                    //altrows: true,
                    //autorowheight: true,
                    //pageable: true,
                    //columnsheight: 40,
                    columns: [
                        //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'CONTR',               dataField: 'CONTRA_RECIBO_ID',    width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'TOTAL',               dataField: 'TOTAL',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'SUBTOTAL',            dataField: 'SUBTOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IMPUESTOS',           dataField: 'IMPUESTOS',           width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IVA',                 dataField: 'IVA',                 width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
                        { text: 'IEPS',                dataField: 'IEPS',                width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' },
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

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

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
                        { text: 'SERIE XML',               dataField: 'SERIE',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'FOLIO XML',               dataField: 'FOLIO',               width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'TIENDA',              dataField: 'TIENDA',              width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: '#COMPRA',             dataField: 'NO_COMPRA',           width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'ALBARAN', dataField: 'ALBARAN', width: 100, renderer: columnrenderer2, /*cellsalign: 'center',*/ cellsrenderer: cellD},

                        { text: 'FECHA COMPRA',        dataField: 'FECHA_COMPRA',        width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy' },
                        //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        { text: 'incidencia', dataField: 'INCIDENCIA', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'incidencia',
                            datafield: 'INCIDENCIA1',
                            columntype: 'button',
                            width: 70,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $("#incidencia1")
                                    .html(dataRecord.INCIDENCIA_MSG);

                                $("#incidencia1").dialog('open');
                            }
                        },
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt'},
                        { text: 'UUID',                dataField: 'UUID',                width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width: 130,  renderer: columnrenderer2, cellsalign: 'center' },
                        {
                            text: 'XML',
                            datafield: 'Show1',
                            columntype: 'button',
                            width: 60, 
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);
                                
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
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);
                                
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
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);
                                
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
                        //<% if (User.IsInRole("superuser")){  %>
/*                        ,
                        {
                            text: 'Editar',
                            datafield: 'Edit',
                            width: 60,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return '<div style="text-align: center;margin-top: 7px;font-weight: bold;font-size:12px"><a class="cEdit" href="#">Edit</a></div>';
                            }
                        }*/
                        //<% }  %>
                        //{ text: ' ', minwidth: 0, width: 'auto', sortable: false }
                    ]
                });