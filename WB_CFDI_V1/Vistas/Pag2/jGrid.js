                source =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'BLOQ' },
                        { name: 'ID' },
                        { name: 'OBSERVACION1' },
                        { name: 'RFC_EMISOR' },
                        //{ name: 'RAZON_SOCIAL_EMISOR' },
                        { name: 'CONTRA_RECIBO_ID' },
                        { name: 'UUID' },
                        { name: 'SERIE' },
                        { name: 'TIPO_REL' },
                        { name: 'FOLIO' },
                        //{ name: 'REVISADO' },
                        //{ name: 'TIENDA' },
                        //{ name: 'NO_COMPRA' },
                        { name: 'ALBARAN' },
                        { name: 'FECHA_COMPRA', type: 'date' },
                        //{ name: 'VERSION' },
                        { name: 'TOTAL' },
                        { name: 'SUBTOTAL' },
                        { name: 'IMPUESTOS' },
                        { name: 'IVA' },
                        { name: 'IEPS' },
                        { name: 'RETENCIONES' },
                        { name: 'DESCUENTO' },
                        { name: 'FECHA_RECEPCION', type: 'date' },
                        { name: 'FECHA_FACTURA', type: 'date' },
                        { name: 'FECHA_PROCESO', type: 'date' },
                        { name: 'ESTATUS' },
                        { name: 'ESTATUS_ID' },
                        { name: 'CHECK_ID' },
                        { name: 'CHECK_DATE', type: 'date' },
                        { name: 'CHECK_NUMBER' },
                        { name: 'INCIDENCIA' },
                        { name: 'INCIDENCIA_MSG' },
                    ],
                    localdata: []
                };
                var dataAdapter = new $.jqx.dataAdapter(source);

                $("#jGrid").jqxGrid({ width: '100%', height: 300,
                    source: dataAdapter,
                    theme: 'Theme2',
                    sortable: true,
                    enableHover: false,
                    selectionmode: 'checkbox', //'singlerow', 'checkbox',
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
                    rowsheight: 53,
                    //altrows: true,
                    //autorowheight: true,
                    //pageable: true,
                    //columnsheight: 40,
                    columns: [
                        //{ text: 'RFC EMISOR',          dataField: 'RFC_EMISOR',          width: 100, renderer: columnrenderer2 }, //, pinned: true 
                        { text: 'CONTR',               dataField: 'CONTRA_RECIBO_ID',    width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'TOTAL',               dataField: 'TOTAL',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'SUBTOTAL',            dataField: 'SUBTOTAL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'IMPUESTOS',           dataField: 'IMPUESTOS',           width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'IVA',                 dataField: 'IVA',                 width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'IEPS',                dataField: 'IEPS',                width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'RETENCIONES',         dataField: 'RETENCIONES',         width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'DESCUENTO', dataField: 'DESCUENTO', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2', cellsrenderer: cellB, cellclassname: cellclass },
                        {
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

                                $("#cSubTotal2").html( numeral(dataRecord.SUBTOTAL).format('$0,0.00') );
                                $("#cTotal2").html( numeral(dataRecord.TOTAL).format('$0,0.00') );
                                $("#cIVA2").html( numeral(dataRecord.IVA).format('$0,0.00') );
                                $("#cIEPS2").html( numeral(dataRecord.IEPS).format('$0,0.00') );

                                $('#init').removeClass('no-show');
                                setTimeout(function() {

                                        //alert(dataRecord.ID);

                                        //$("#detalle3").dialog({ width: Math.min(340, $(window).width()) });

                                        var rs = getData("Pag2.aspx/GetDetalle", { id: dataRecord.ID });

                                        source6.localdata = rs.DATA;
                                        dataAdapter6.dataBind();
                                        $("#jGrid6").jqxGrid('ensurerowvisible', 0);
                                        $("#jGrid6").jqxGrid('clearselection');


                                        $("#jGrid6").jqxGrid("autoresizecolumns");

                                        var colDefs = $("#jGrid6").jqxGrid('columns').records;
                                        for (var idx = 0; idx < colDefs.length; idx++) {
                                            if (colDefs[idx].datafield != "_checkboxcolumn") {
                                                $("#jGrid6").jqxGrid('setcolumnproperty',
                                                    colDefs[idx].datafield,
                                                    'width',
                                                    colDefs[idx].width + 5);
                                            }
                                        }

                                        $("#cRFiscal").html(rs.RFiscal);
                                        $("#cUCfdi").html(rs.UsoCFDI);
                                        $("#cMPago").html(rs.MetodoPago);
                                        $("#cFPago").html(rs.FormaPago);
                                        $("#cMoneda").html(rs.Moneda);
                                        $("#cDescuento").html(rs.Descuento);

                                       /* if (!rs.Revisado) {
                                            $("#detalle3").dialog({
                                                buttons: {
                                                    'Validar Factura': function() {

                                                        $("#confirmar")
                                                            .html(
                                                                "<div style='text-align: center;min-width:250px;margin-top:20px;'>Quieres validar la factura?</div>");

                                                        $("#confirmar").dialog({
                                                            buttons: {
                                                                'Aceptar': function() {
                                                                    $(this).dialog("close");

                                                                    $('#init').removeClass('no-show');
                                                                    getData("Pag2.aspx/Valido",
                                                                        {
                                                                            id: dataRecord.ID
                                                                        });

                                                                    $("#jGrid").jqxGrid('setcellvalue',
                                                                        row,
                                                                        'REVISADO',
                                                                        'OK');
                                                                    $('#init').addClass('no-show');

                                                                    $("#detalle3").dialog("close");

                                                                },
                                                                'Cancelar': function() {

                                                                    $(this).dialog("close");
                                                                }
                                                            }
                                                        });

                                                        $("#confirmar").dialog('open');
                                                    },
                                                    'Cerrar': function() {

                                                        $(this).dialog("close");
                                                    }
                                                }
                                            });
                                        } else {*/
                                            $("#detalle3").dialog({
                                                buttons: {
                                                    'Cerrar': function() {

                                                        $(this).dialog("close");
                                                    }
                                                }
                                            });
                                        //}


                                        $("#detalle3").dialog('open');

                                        $('#init').addClass('no-show');
                                    },30);
                            }
                        },
                        { text: 'TipoRel',             dataField: 'TIPO_REL',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        {
                            text: 'DOC REL',
                            datafield: 'Link',
                            columntype: 'button',
                            width: 65,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);


                                $('#init').removeClass('no-show');
                                var rs = getData("Pag2.aspx/GetRel",
                                    {
                                        uuid: dataRecord.UUID
                                    });


                                source10.localdata = rs;
                                dataAdapter10.dataBind();
                                $("#jGrid10").jqxGrid('clearselection');
                                $("#jGrid10").jqxGrid('updatebounddata', 'cells');

                                $("#jGrid10").jqxGrid("autoresizecolumns");

                                var colDefs = $("#jGrid10").jqxGrid('columns').records;
                                for (var idx = 0; idx < colDefs.length; idx++) {

                                    if (colDefs[idx].datafield != "_checkboxcolumn") {
                                        $("#jGrid10").jqxGrid('setcolumnproperty',
                                            colDefs[idx].datafield,
                                            'width',
                                            colDefs[idx].width + 5);
                                    }

                                    if (colDefs[idx].datafield == "UUID") {
                                        $("#jGrid10").jqxGrid('setcolumnproperty',
                                            colDefs[idx].datafield,
                                            'width',
                                            colDefs[idx].width + 10);
                                    }
                                }

                                $("#jGrid10").jqxGrid('setcolumnproperty', "Albaranes", 'width', 60);
                                $("#jGrid10").jqxGrid('setcolumnproperty', "Detalle", 'width', 45);
                                $("#jGrid10").jqxGrid('setcolumnproperty', "Link", 'width', 65);
                                $("#jGrid10").jqxGrid('setcolumnproperty', "Show1", 'width', 60);
                                $("#jGrid10").jqxGrid('setcolumnproperty', "Show2", 'width', 60);
                                $("#jGrid10").jqxGrid('setcolumnproperty', "Show3", 'width', 70);
                                $("#jGrid10").jqxGrid('setcolumnproperty', "IMPUESTOS", 'width', 90);


                                $('#init').addClass('no-show');



                                $("#detalle6").dialog({
                                    title: "Facturas Relacionadas: " + dataRecord.UUID
                                }).dialog('open');

                                

                            }
                        },

                        { text: 'SERIE XML',               dataField: 'SERIE',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'FOLIO XML',               dataField: 'FOLIO',               width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },


                        /*{ text: 'TIENDA', dataField: 'TIENDA', width: 100, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: '#COMPRA', dataField: 'NO_COMPRA', width: 100, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },*/
                        { text: 'ALBARAN', dataField: 'ALBARAN', width: 100, renderer: columnrenderer2, /*cellsalign: 'center',*/ cellsrenderer: cellD, cellclassname: cellclass },



                        {
                            text: 'Albaranes',
                            datafield: 'Albaranes',
                            columntype: 'button',
                            width: 63,
                            renderer: columnrenderer2,
                            cellsrenderer: function() {
                                return "--";
                            },
                            buttonclick: function(row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                IDgrid = dataRecord.ID;

                                $("#detalle5").dialog('open');

                                detalle();

                            }
                        },
                        {    
                            text: 'Observacion',
                            datafield: 'OBSERVACION1',
                            columntype: 'button',
                            width: 76,
                            renderer: columnrenderer2,
                            cellsrenderer: function () {
                                return "--";
                            },
                            buttonclick: function (row) {

                                var dataRecord = $("#jGrid").jqxGrid('getrowdata', row);

                                $("#observacion1")
                                    .html(dataRecord.OBSERVACION1);

                                $("#observacion1").dialog('open');

                                //IDgrid = dataRecord.ID;

                                //$("#detalle5").dialog('open');

                            }
                        },
                        { text: 'FECHA COMPRA',        dataField: 'FECHA_COMPRA',        width: 160, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy', cellsrenderer: cellB, cellclassname: cellclass },
                        //{ text: 'REVISADO',            dataField: 'REVISADO',            width: 100, renderer: columnrenderer2, cellsalign: 'center' },
                        //{ text: 'RAZON SOCIAL EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 400, renderer: columnrenderer2 },
                        { text: 'ESTATUS',             dataField: 'ESTATUS',             width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'incidencia', dataField: 'INCIDENCIA', width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
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
                        { text: 'FECHA FACTURA',       dataField: 'FECHA_FACTURA',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt', cellsrenderer: cellB, cellclassname: cellclass},
                        { text: 'FECHA RECEPCION',     dataField: 'FECHA_RECEPCION',     width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt', cellsrenderer: cellB, cellclassname: cellclass},
                        { text: 'FECHA PROCESO',       dataField: 'FECHA_PROCESO',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy hh:mm:ss tt', cellsrenderer: cellB, cellclassname: cellclass},
                        { text: 'UUID',                dataField: 'UUID',                width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'ID_PAGO',            dataField: 'CHECK_ID',            width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'NUM_PAGO',        dataField: 'CHECK_NUMBER',        width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsrenderer: cellB, cellclassname: cellclass },
                        { text: 'FEC_PAGO',          dataField: 'CHECK_DATE',          width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'dd/MM/yyyy', cellsrenderer: cellB, cellclassname: cellclass },
                        //{ text: 'VERSION',             dataField: 'VERSION',             width: 130, renderer: columnrenderer2, cellsalign: 'center' },
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