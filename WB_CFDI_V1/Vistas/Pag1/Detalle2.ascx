<%@ Control Language="C#" CodeBehind="Detalle2.ascx.cs" Inherits="WB_CFDI_V1.Vistas.Detalle2" %>




<script>
    
    $("#detalle2").dialog({
        autoOpen: false,
        resizable: false,
        width: 'auto',
        //minWidth: 200,
        //maxWidth: 500,
        fluid: true, //new option
        modal: true,
        dialogClass: 'mWin2',
        buttons: [
            {
                text: 'Guardar',
                id: 'bGuardar',
                disabled: true,
                click: function () {

                    var rows = $("#jGrid3").jqxGrid('getrows');

                    if (rows.length == 0) {

                        $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>La serie y folio son requeridos.</div>");
                        $("#message").dialog('open');


                        return;
                    }

                    $("#confirmar")
                        .html(
                            "<div style='text-align: center;min-width:250px;margin-top:20px;'>Los datos estan correctos?</div>");

                    $("#confirmar").dialog({
                        buttons: {
                            'Aceptar': function () {
                                $(this).dialog("close");

                                var rs = getData("Pag1.aspx/Save2",
                                    {
                                        id: IDgrid,
                                        data: rows
                                    });

                                detalle();

                                if (rs.code == 0) {
                                    /*$("#jGrid").jqxGrid('setcellvalue', lastRow, "TIENDA", it1);
                                    $("#jGrid").jqxGrid('setcellvalue', lastRow, "NO_COMPRA", it2);
                                    $("#jGrid").jqxGrid('setcellvalue', lastRow, "FECHA_COMPRA", rs.msg);
                                    $(this).dialog("close");*/

                                    modificado = false;
                                    //$("#jGrid").jqxGrid('setcellvalue', IDrow, "FECHA_COMPRA", rs.msg);

                                    //$("#bGuardar").button("disable");
                                    /*.attr('disabled', true)
                                    .addClass("ui-state-disabled");*/

                                    //Editable = false;

                                } else {
                                    $("#message").html("<div style='text-align: center;min-width:250px;margin-top:20px;'>" + rs.msg + "</div>");
                                    $("#message").dialog('open');
                                }

                            },
                            'Cancelar': function () {

                                $(this).dialog("close");
                            }
                        }
                    });

                    $("#confirmar").dialog('open');

                }
            },
            {
                text: 'Exportar',
                click: function () {

                    getData("excel4.aspx/SetID", { id: IDgrid });

                    window.open("excel4.aspx", "xml", "width=200,height=150");
                }
            }, {
                text: 'Cerrar',
                click: function () {

                    var rows = $("#jGrid3").jqxGrid('getrows');

                    if (rows.length > 0 && modificado) {

                        $("#confirmar")
                            .html(
                                "<div style='text-align: center;min-width:250px;margin-top:20px;'>Hay registros sin guardar, quieres cerrar de todos modos?</div>");

                        $("#confirmar").dialog({
                            buttons: {
                                'Aceptar': function () {

                                    source3.localdata = [];
                                    dataAdapter3.dataBind();

                                    //$("#jGrid3").jqxGrid('updatebounddata', 'cells');
                                    $("#jGrid3").jqxGrid('ensurerowvisible', 0);
                                    $("#jGrid3").jqxGrid('clearselection');

                                    $(this).dialog("close");
                                    $("#detalle2").dialog("close");

                                },
                                'Cancelar': function () {

                                    $(this).dialog("close");
                                }
                            }
                        });

                        $("#confirmar").dialog('open');

                    } else {

                        source3.localdata = [];
                        dataAdapter3.dataBind();

                        //$("#jGrid3").jqxGrid('updatebounddata', 'cells');
                        $("#jGrid3").jqxGrid('ensurerowvisible', 0);
                        $("#jGrid3").jqxGrid('clearselection');

                        $(this).dialog("close");
                    }
                }
            }
        ],
        open: function (event, ui) {

        },
        create: function (event, ui) {
            // Set maxWidth
            //$(this).css("maxWidth", "700px");
        }
    });
</script>