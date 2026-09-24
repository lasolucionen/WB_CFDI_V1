<%@ Control Language="C#" CodeBehind="Reporte1.ascx.cs" Inherits="WB_CFDI_V1.Vistas.Reporte1" %>



<div id="jGrid3" style=""></div>




<script>

    $(document).ready(function () {
        //****************************************************************************************


        var source3 =
        {
            datatype: "json",
            datafields: [
                { name: 'RFC_EMISOR' },
                { name: 'RAZON_SOCIAL_EMISOR' },
                { name: 'NUEVOS' },
                { name: 'IGNORADOS' },
                { name: 'A_PROCESAR' },
                { name: 'XM_A_PROCESAR' },
                { name: 'CA_PROCESAR' },
                { name: 'VALIDADOS' },
                { name: 'C_VALIDADO' },
                { name: 'XML_VALIDADOS' },
                { name: 'IGNORADOS_AYER' },
                { name: 'INCOMPLETOS_AYER' },
                { name: 'VALIDADOS_AYER' },
                { name: 'VALIDADORA' },
            ],
            localdata: []
        };

        var dataAdapter3 = new $.jqx.dataAdapter(source3);


        //---------------------------------------------------------------------------
        var columnrenderer2 = function (value) {
            return '<div style="text-align: center;margin-top: 7px;">' + value + '</div>';
        }



        $("#jGrid3").jqxGrid({
            //width: '691px',
            width: '100%',
            height: 450,
            source: dataAdapter3,
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
                { text: 'RFC_EMISOR', dataField: 'RFC_EMISOR', width: 130, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'RAZON_SOCIAL_EMISOR', dataField: 'RAZON_SOCIAL_EMISOR', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'NUEVOS', dataField: 'NUEVOS', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'IGNORADOS', dataField: 'IGNORADOS', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'A PROCESAR', dataField: 'A_PROCESAR', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: '# XML A PROCESAR', dataField: 'XM_A_PROCESAR', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: '$ A PROCESAR', dataField: 'CA_PROCESAR', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'VALIDADOS', dataField: 'VALIDADOS', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: '$ VALIDADO', dataField: 'C_VALIDADO', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: '# XML VALIDADOS', dataField: 'XML_VALIDADOS', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'IGNORADOS AYER', dataField: 'IGNORADOS_AYER', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'INCOMPLETOS AYER', dataField: 'INCOMPLETOS_AYER', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'VALIDADOS AYER', dataField: 'VALIDADOS_AYER', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
                { text: 'VALIDADORA', dataField: 'VALIDADORA', width: 170, renderer: columnrenderer2, cellsalign: 'center' }, //, pinned: true
            ]
        });


        //----------------------------------------------------------------------------------------

        $("#bReporte1").click(function (event) {
            
        });




        //****************************************************************************************
    });

</script>