        source4 =
        {
            datatype: "json",
            datafields: [
                { name: 'TOTAL' },
                { name: 'SUBTOTAL' },
                { name: 'IVA' },
                { name: 'IEPS' },
                { name: 'IMPUESTOS' },
                { name: 'NUMSERIE' },
                { name: 'NUMALBARAN' },
                { name: 'SUALBARAN' }
            ],
            localdata: []
        };
        var dataAdapter4 = new $.jqx.dataAdapter(source4);

        $("#jGrid4").jqxGrid({ 
            //width: '100%',
            width: '512px',
            height: 450,
            source: dataAdapter4,
            theme: 'Theme2',
            sortable: true,
            enableHover: false,
            selectionmode: 'singlerow',
            enablebrowserselection :true,
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
                { text: 'TOTAL',      dataField: 'TOTAL',      width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' }, //, pinned: true 
                { text: 'SUBTOTAL',   dataField: 'SUBTOTAL',   width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' }, //, pinned: true 
                { text: 'IVA',        dataField: 'IVA',        width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' }, //, pinned: true 
                { text: 'IEPS',       dataField: 'IEPS',       width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' }, //, pinned: true 
                //{ text: 'IMPUESTOS',  dataField: 'IMPUESTOS',  width: 130, renderer: columnrenderer2, cellsalign: 'center', cellsformat: 'C2' }, //, pinned: true 
                { text: 'NUMSERIE',   dataField: 'NUMSERIE', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'NUMALBARAN', dataField: 'NUMALBARAN', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                { text: 'SUALBARAN', dataField: 'SUALBARAN', width: 130, renderer: columnrenderer2, cellsalign: 'center' },
                {
                    text: 'Ajuste',
                    datafield: 'Ajuste',
                    columntype: 'button',
                    width: 63,
                    renderer: columnrenderer2,
                    cellsrenderer: function () {
                        return "--";
                    },
                    buttonclick: function (row) {

                        var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);


                        var rowdata = $("#jGrid3").jqxGrid('getrowdatabyid', 0);



                        $("#tfac1").html(rowdata.TOTAL);
                        $("#talb1").html(dataRecord.TOTAL);



                        

                        var rs= getData("Pag2.aspx/Detalle", { data: dataRecord });
                        tmpData2 = JSON.parse(JSON.stringify(rs));


                        
                        $.each(source15.localdata, function (index, value) {

                            for (i = rs.length - 1; i >= 0; i--) {

                                if (rs[i].DESCRIPCION == value.DESCRIPCION2) {
                                    rs.splice(i, 1);
                                    break;
                                }
                            }
                        });


                        source14.localdata = rs;
                        dataAdapter14.dataBind();
                        $("#jGrid14").jqxGrid('ensurerowvisible', 0);
                        $("#jGrid14").jqxGrid('clearselection');


                        $("#jGrid14").jqxGrid("autoresizecolumns");

                        var colDefs = $("#jGrid14").jqxGrid('columns').records;
                        for (var idx = 0; idx < colDefs.length; idx++) {
                        if (colDefs[idx].datafield != "_checkboxcolumn") {
                        $("#jGrid14").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
                        }
                        }

                        $("#jGrid14").jqxGrid('setcolumnproperty', "DESCRIPCION", 'width', 200);

                        
                        //////////////////////////////////////////////////////////////////

                        

                        $("#ajustes").dialog('open');


                    }
                }

            ]
        });