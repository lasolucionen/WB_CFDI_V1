<%@ Control Language="C#" CodeBehind="VistaPrevia1.ascx.cs" Inherits="WB_CFDI_V1.Vistas.VistaPrevia1" %>



<!----------------------------------------------------------->
<div id="vistaprevia1" title="Vista Previa">

    <table cellpadding="0" cellspacing="0" style="height: 500px; margin-top: 7px;">
        <tr>
            <td style="border: 1px solid gray;" valign="top">
                <div id="jGrid4" style="margin-bottom: 0px;"></div>
            </td>
            <td style="border: 1px solid gray;" valign="top">
                <div style="border-bottom: 1px solid gray; padding-bottom: 5px; padding-left: 5px; padding-right: 5px;">

                    <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                        <tr>
                            <td align="right" style="padding-right: 5px; width: 127px;">UUID:
                            </td>
                            <td id="vsUUID" style="width: 600px; border: 1px solid gray;"></td>
                        </tr>
                    </table>


                    <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                        <tr>
                            <td align="right" style="padding-right: 5px; width: 127px;">SERIE:
                            </td>
                            <td id="vsSERIE" style="width: 250px; border: 1px solid gray;"></td>
                            <td style="width: 39px;"></td>

                            <td align="right" style="padding-right: 5px;">FOLIO:
                            </td>
                            <td id="vsFOLIO" style="width: 250px; border: 1px solid gray;"></td>
                        </tr>
                    </table>

                    <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                        <tr>
                            <td align="right" style="padding-right: 5px; white-space: nowrap; width: 127px;">Validacion XML:
                            </td>
                            <td id="vsValidacion" style="width: 600px; height: 45px; border: 1px solid gray;"></td>
                        </tr>
                    </table>

                    <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                        <tr>
                            <td align="right" style="padding-right: 5px; white-space: nowrap; width: 127px;">Detalle:
                            </td>
                            <td id="vsDETALLE" style="width: 600px; height: 45px; border: 1px solid gray;"></td>
                        </tr>
                    </table>

                    <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                        <tr>
                            <td align="right" style="padding-right: 5px; white-space: nowrap; width: 127px;">PDF:</td>
                            <td id="vsPDF" style="width: 250px; border: 1px solid gray;"></td>
                            
                            <td align="right" style="padding-left: 20px; padding-right: 5px; white-space: nowrap; width: 100px;">TOTAL:</td>
                            <td id="vsTOTAL" style="width: 250px; border: 1px solid gray;"></td>
                        </tr>
                    </table>


                </div>
                <div style="padding-bottom: 5px; padding-top: 5px; padding-left: 5px; padding-right: 5px;">

                    <!-------------------------------------------------------------->
                    
                    
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                
                                <table>
                                    <tr>
                                        <td style="padding-right: 5px;">
                                            SERIE:
                                        </td>
                                        <td><input class="jTx" id="txSerie5" type="text" value="" style="width: 195px; text-align: center;"/></td>
                                        <td style="width: 32px;">
                                            
                                        </td>
                                        <td style="padding-right: 5px;">
                                            FOLIO:
                                        </td>
                                        <td><input class="jTx" id="txFolio5" type="text" value="" style="width: 195px; text-align: center;"/></td>
                                        <td style="padding-left: 5px;">
                                            <input type="button" id="bAgregar5" class="btn1" style="width: 97px; height: 27px; padding-top: 3px; font-size: 1em;" value="Agregar"/>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                            

                        </tr>
                        <tr>
                            <td style="padding-top: 8px;">
                                <div id="jGrid5" style="margin-bottom: 0px;"></div>
                            </td>

                        </tr>
                    </table>
                    
                    

                    <!-------------------------------------------------------------->

                </div>

            </td>
        </tr>
    </table>
</div>

<!----------------------------------------------------------->

<script>

    function isEmpty(value) {
        //return (value == null || value.length === 0);
        return (value == null || $.trim(value) === '');
    }

   

    //*****************************************************************************************************************
    var columnrenderer4 = function (value) {
        return '<div style="text-align: center;margin-top: 7px;">' + value + '</div>';
    }



    source4 =
    {
        datatype: "json",
        datafields: [
            { name: 'ARCHIVO' },
            { name: 'SERIE_FOLIO' },
        ],
        localdata: []
    };
    var dataAdapter4 = new $.jqx.dataAdapter(source4);

    $("#jGrid4").jqxGrid({
        width: '525',
        height: 525,
        source: dataAdapter4,
        theme: 'Theme2',
        sortable: true,
        enableHover: false,
        selectionmode: 'singlerow',
        enablebrowserselection: true,
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
            {
                text: 'ARCHIVO', dataField: 'ARCHIVO', width: 400, renderer: columnrenderer4,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                    return '<div style="margin: 3px; overflow-wrap: break-word;">' + value + '</div>';

                }

            },

            {
                text: 'SERIE-FOLIO', dataField: 'SERIE_FOLIO', width: 100, renderer: columnrenderer4, 
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                    return '<div style="margin: 3px; overflow-wrap: break-word; text-align: center;">' + value + '</div>';

                }

            },




            //, pinned: true
            /*{ text: 'PDF', dataField: 'PDF', width: 100, renderer: columnrenderer4, cellsalign: 'center' },
            {
                text: 'DETALLE',
                dataField: 'DETALLE',
                width: 290,
                renderer: columnrenderer4,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                    return '<div style="margin: 5px;">' + value + '</div>';

                }
            },
            {
                text: 'Validacion XML',
                dataField: 'MENSAJE',
                width: 280,
                renderer: columnrenderer4,
                cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {

                    return '<div style="margin: 5px;">' + value + '</div>';

                }
            }*/
        ]
    });


    //***********************************************************************************************
    var inx4 = 0;
    $("#jGrid4").on('rowselect', function (event) {

        inx4 = event.args.rowindex;
        var data = $("#jGrid4").jqxGrid('getrowdata', inx4);

        //console.log(zipForm[inx]);



        if (zipForm[inx4].ERROR) {
            

            $("#txSerie5").jqxInput({ disabled: true });
            $("#txFolio5").jqxInput({ disabled: true });
            $("#txSerie5").css('background-color', '#e0f9ff');
            $("#txFolio5").css('background-color', '#e0f9ff');
            $("#bAgregar5").button("disable")
        } else {
            $("#txSerie5").jqxInput({ disabled: false });
            $("#txFolio5").jqxInput({ disabled: false });
            $("#txSerie5").css('background-color', '');
            $("#txFolio5").css('background-color', '');
            $("#bAgregar5").button("enable")
        }

        $("#vsTOTAL").html(zipForm[inx4].TOTAL);
        $("#vsUUID").html(zipForm[inx4].UUID);
        $("#vsSERIE").html(zipForm[inx4].SERIE);
        $("#vsFOLIO").html(zipForm[inx4].FOLIO);
        $("#vsValidacion").html(zipForm[inx4].MENSAJE);
        $("#vsDETALLE").html(zipForm[inx4].DETALLE);
        $("#vsPDF").html(zipForm[inx4].PDF);

        $("#txSerie5").val('');
        $("#txFolio5").val('');


        source5.localdata = JSON.parse(JSON.stringify(zipForm[inx4].SF));
        dataAdapter5.dataBind();

    });


    //***********************************************************************************************
    $("#vistaprevia1").dialog({
        autoOpen: false,
        resizable: false,
        width: 1290,
        //minWidth: 200,
        //maxWidth: 500,
        fluid: true, //new option
        modal: true,
        dialogClass: 'mWin2',
        buttons: {
            'Importar': function () {


                //var id = getData("Pag1.aspx/GetEmpresaID", {});


                var sfRequerido = false;
                var fac = false;

                for (var i = 0; i < zipForm.length; i++) {

                    if (zipForm[i].ERROR) {
                        continue;
                    }

                    fac = true;

                    if (zipForm[i].REQUERIDO) {

                        if (isEmpty(zipForm[i].SF)) {
                            sfRequerido = true;
                        } else {
                            if (zipForm[i].SF.length == 0) {
                                sfRequerido = true;
                            }
                        }
                    }
                }

                if (sfRequerido) {
                    $("#message").html("<div style='text-align: left;min-width:250px; padding-top:10px;'>Agregue la serie y folio en tu listado de facturas.</div>");
                    $("#message").dialog('open');
                    $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

                    return;
                }

                if (!fac) {
                    $("#message").html("<div style='text-align: left;min-width:250px; padding-top:10px;'>No hay facturas para importar.</div>");
                    $("#message").dialog('open');
                    $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

                    return;
                }




                $("#confirmar3").dialog({
                    buttons: {
                        'Aceptar': function () {
                            $('#init').removeClass('no-show');

                            $(this).dialog("close");

                            var rs= getData("Pag1.aspx/SetTmpXML", {
                                zipForm: zipForm
                            });


                            $("#message").html("<div style='text-align: left;min-width:250px; padding-top:10px;'>" + rs.msg + "</div>");
                            $("#message").dialog('open');
                            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });


                            if (rs.code == 0) {


                                $('#vistaprevia1').dialog("close");

                            } else {

                            }

                            $('#init').addClass('no-show');


                        },
                        'Cancelar': function () {

                            $(this).dialog("close");
                        }
                    }
                });

                $("#confirmar3").dialog('open');


                //$(this).dialog("close");
            },
            'Cancelar': function () {

                $(this).dialog("close");
            }
        },
        open: function (event, ui) {

        },
        create: function (event, ui) {
            // Set maxWidth
            //$(this).css("maxWidth", "700px");
        }
    });
    
    /***********************************************************************************************/
                    source5 =
                    {
                        datatype: "json",
                        datafields: [
                            { name: 'SERIE' },
                            { name: 'FOLIO' }
                        ],
                        localdata: []
                    };

                    var dataAdapter5 = new $.jqx.dataAdapter(source5);

                    $("#jGrid5").jqxGrid({
                        width: 725,
                        height: 290,
                        source: dataAdapter5,
                        theme: 'Theme2',
                        sortable: true,
                        enableHover: false,
                        selectionmode: 'singlerow',
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
                            { text: 'SERIE', dataField: 'SERIE', width: 200, /*renderer: columnrenderer2,*/ cellsalign: 'center' }, //, pinned: true
                            { text: 'FOLIO', dataField: 'FOLIO', width: 200, /*renderer: columnrenderer2,*/ cellsalign: 'center' },
                            {
                                text: 'Eliminar',
                                datafield: 'Eliminar1',
                                columntype: 'button',
                                width: 60,
                                renderer: columnrenderer4,
                                cellsrenderer: function () {
                                    return "--";
                                },
                                buttonclick: function (row) {

                                    //var dataRecord = $("#jGrid4").jqxGrid('getrowdata', row);


                                    zipForm[inx4].SF.splice(row, 1);

                                    source5.localdata.splice(row, 1);
                                    dataAdapter5.dataBind();
                                }
                            },

                    ]
                    });
    /***********************************************************************************************/

    $("#bAgregar5").click(function () {

        if (isEmpty(zipForm[inx4].SF)) {
            zipForm[inx4].SF = new Array();
        }


        var serie = $("#txSerie5").val().trim().toUpperCase();
        var folio = $("#txFolio5").val().trim().toUpperCase();

        if (serie == "") {

            $("#message").html("<div style='text-align: center;min-width:250px; padding-top:10px;'>Falta la serie.</div>");
            $("#message").dialog('open');
            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });
            return;
        }

        if (folio == "") {

            $("#message").html("<div style='text-align: center;min-width:250px; padding-top:10px;'>Falta el folio.</div>");
            $("#message").dialog('open');
            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });
            return;
        }


        var err = false;
        for (var i = 0; i < zipForm[inx4].SF.length; i++) {

            var sf = zipForm[inx4].SF[i];

            if (sf.SERIE.toUpperCase() == serie.toUpperCase() && sf.FOLIO.toUpperCase() == folio.toUpperCase()) {
                err = true;
                break;
            }

        }

        if (err) {

            $("#message").html("<div style='text-align: center;min-width:250px; padding-top:10px;'>La serie: " + serie + " y el Folio: " + folio + " ya existen.</div>");
            $("#message").dialog('open');
            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });


            return;
        }



        var rs = getData("Pag1.aspx/CheckSF", {
            SERIE: serie,
            FOLIO: folio
        });


        if (rs.code == 0) {
            zipForm[inx4].SF.push({ SERIE: serie, FOLIO: folio });

            $("#txSerie5").val('');
            $("#txFolio5").val('');



            source5.localdata = JSON.parse(JSON.stringify(zipForm[inx4].SF));
            dataAdapter5.dataBind();
        } else {
            $("#message").html("<div style='text-align: left;min-width:250px; padding-top:10px;'>" + rs.msg + "</div>");
            $("#message").dialog('open');
            $("#message").dialog('widget').position({ my: 'center', at: 'center', of: $(window) });

        }


        

    });

    


    /***********************************************************************************************/



</script>