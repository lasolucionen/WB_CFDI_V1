<%@ Control Language="C#" CodeBehind="BuscarPorArticulo2.ascx.cs" Inherits="WB_CFDI_V1.Vistas.BuscarPorArticulo2" %>



<input type="button" id="bBuscarPorArticulo2" class="btn1" style="height: 28px; padding-top: 0px; font-size: 0.5em;" value=".."/>


<!------------------------------------------------------------------------------------------->



<div id="bsarticulos" title="Buscar Por Articulo">
    
    <table>
       
        <tr>
            <td style="padding-top: 5px;">
            
                
                <table style="width: 1000px;">
                    <tr>
                        <td style="width: 150px;">
                            <div>NUMSERIE</div>
                            <div><input class="jTx" id="numserie2" type="text" value="" style="width: 150px; text-align: center;" /></div>
                        </td>
            
                        <td style="width: 150px;">
                            <div>NUMALBARAN</div>
                            <div><input class="jTx" id="numalbaran2" type="text" value="" style="width: 150px; text-align: center;" /></div>
                        </td>
                        <td style="width: 150px;">
                            <div>REF</div>
                            <div><input class="jTx" id="ref2" type="text" value="" style="width: 150px; text-align: center;" /></div>
                        </td>
                        <td style="width: 400px;">
                            <div>DESCRIPCION</div>
                            <div><input class="jTx" id="descripcion2" type="text" value="" style="width: 400px; text-align: center;" /></div>
                        </td>
                        <td>

                        </td>
                    </tr>

                </table>
                

            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px;">
                    
                
                
                <table style="width: 100%;text-align: right; border: solid 1px #e4e4e4;border-bottom: solid 1px #e4e4e4;margin: auto;background-color: #fbfbfb">
                    <tr>
                        <td align="left">
                            <table>
                                <tr>
                                    <td style="padding-top: 10px; padding-right: 2px;">
                                        <div id='chkAlb'></div>
                                    </td>
                                    <td style="padding-right: 8px;" id="serieFolio">SERIE - FOLIO</td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        <input type="button" id="bBuscar5" class="btn1" style="height: 30px; padding-top: 5px; font-size: 1em;" value="Buscar"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                

            </td>
        </tr>
        <tr>
            <td style="padding-top: 7px;">
                <div id="jGrid17" style=""></div>
            </td>
        </tr>
    </table>

</div>




<!------------------------------------------------------------------------------------------->


<script>

    //---------------------------------------------------------
    $("#chkAlb").jqxCheckBox({ width: 25, height: 25 });

    $("#chkAlb").on('change', function (event) {
        if (_chkAlb) {
            return;
        }


        var checked = $("#chkAlb").jqxCheckBox('checked');

        var rowindexes = $("#jGrid17").jqxGrid('getselectedrowindexes');

        if (checked) {

            if (rowindexes.length > 0) {
                //source2.localdata


                //alert(source17.localdata[rowindexes[0]].NUMSERIE);

                
                for (i = source2.localdata.length - 1; i >= 0; i--) {

                    if (source2.localdata[i].NUMSERIE == source17.localdata[rowindexes[0]].NUMSERIE &&
                        source2.localdata[i].NUMALBARAN == source17.localdata[rowindexes[0]].NUMALBARAN) {

                        $("#jGrid2").jqxGrid('selectrow', i);
                        break;
                    }
                }
                
            }
        } else {
            if (rowindexes.length > 0) {

                for (i = source2.localdata.length - 1; i >= 0; i--) {

                    if (source2.localdata[i].NUMSERIE == source17.localdata[rowindexes[0]].NUMSERIE &&
                        source2.localdata[i].NUMALBARAN == source17.localdata[rowindexes[0]].NUMALBARAN) {

                        $("#jGrid2").jqxGrid('unselectrow', i);
                        break;
                    }
                }

            }
        }
    });
    //---------------------------------------------------------


    $("#bsarticulos").dialog({
        autoOpen: false,
        resizable: false,
        width: 'auto',
        //minWidth: 200,
        //maxWidth: 500,
        fluid: true, //new option
        modal: false,
        dialogClass: 'zindex',
        buttons: {
            'Cerrar': function () {

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
    
        //****************************************************************************************

    $('#bBuscar5').click(function () {


        var rows = $('#jGrid2').jqxGrid('getrows');


        if (rows.length == 0) {

            $("#message")
                .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>No hay albaranes en el listado</div>");
            $("#message").dialog('open');

            return;
        }

        var rs = getData("Pag2.aspx/GetArticulosAlb", {
            NUMSERIE: $('#numserie2').val().trim(),
            NUMALBARAN: $('#numalbaran2').val().trim(),
            REF: $('#ref2').val().trim(),
            DESCRIPCION: $('#descripcion2').val().trim(),

        }).data;



        source17.localdata = rs;
        dataAdapter17.dataBind();

        _chkAlb = true;
        $("#jGrid17").jqxGrid('ensurerowvisible', 0);
        $("#jGrid17").jqxGrid('clearselection');
        $("#serieFolio").html("SERIE - FOLIO");
        $("#chkAlb").jqxCheckBox('uncheck');
        _chkAlb = false;

    });


        //---------------------------------------------------------------------------

    $('#bBuscarPorArticulo2').click(function () {
        var rows = $('#jGrid2').jqxGrid('getrows');


        if (rows.length == 0) {

            $("#message")
                .html("<div style='text-align: center;min-width:250px;margin-top:20px;'>No hay albaranes en el listado</div>");
            $("#message").dialog('open');
        } else {

            _chkAlb = true;
            $("#jGrid17").jqxGrid('ensurerowvisible', 0);
            $("#jGrid17").jqxGrid('clearselection');
            $("#serieFolio").html("SERIE - FOLIO");
            $("#chkAlb").jqxCheckBox('uncheck');
            _chkAlb = false;


            $("#bsarticulos").dialog('open');
            $("#bsarticulos").dialog('widget').position({ my: 'center', at: 'center', of: 'body' });

        }

    });

        //---------------------------------------------------------------------------



    var columnrenderer17 = function (value) {
        return '<div style="text-align: center;margin-top: 7px;">' + value + '</div>';
    }

        //---------------------------------------------------------------------------





    



        //----------------------------------------------------------------------------------------






        //****************************************************************************************


</script>