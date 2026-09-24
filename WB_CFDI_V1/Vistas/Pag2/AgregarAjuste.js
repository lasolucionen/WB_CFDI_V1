function addAjuste() {



    //////////////////////////////////////////////////////////

    var data1 = $("#jGrid13").jqxGrid('getrowdata', _inx1[0]);
    var data2 = $("#jGrid14").jqxGrid('getrowdata', _inx2[0]);


    var _inx3 = $("#jGrid16").jqxGrid('getselectedrowindexes');
    var data3 = $("#jGrid16").jqxGrid('getrowdata', _inx3[0]);


    var _inx4 = $("#jGrid").jqxGrid('getselectedrowindexes');
    var data4 = $("#jGrid").jqxGrid('getrowdata', _inx4[0]);

    var _inx5 = $("#jGrid4").jqxGrid('getselectedrowindexes');
    var data5 = $("#jGrid4").jqxGrid('getrowdata', _inx5[0]);

    var data0 = {};



    data0.UUID = data4.UUID;

    data0.CLAVE = data1.CLAVE;
    data0.DESCRIPCION = data1.DESCRIPCION;
    data0.IMPORTE = numeral(data1.IMPORTE).value();

    data0.REFERENCIA = data2.REFERENCIA;
    data0.DESCRIPCION2 = data2.DESCRIPCION;
    //data1.UNIDADESTOTAL = data2.UNIDADESTOTAL;
    data0.TOTAL = numeral(data2.TOTAL).value();
    data0.REQ = data2.REQ;
    data0.IVA = data2.IVA;
    data0.TIPOIMPUESTO = data2.TIPOIMPUESTO;

    data0.PRECIO = numeral(_ajuste).value();
    data0.PRECIO_IMP = numeral($("#tAjuste").html().trim()).value();
    data0.AJUSTE_DESCRIPCION = data3.DESCRIPCION;
    data0.CODARTICULO = data3.CODARTICULO;

    data0.NUMSERIEO = data5.NUMSERIE;
    data0.NUMALBARANO = data5.NUMALBARAN;

    source15.localdata.push(data0);
    dataAdapter15.dataBind();

    $("#tAjustes").html(source15.localdata.length);

    
    
    //////////////////////////////////////////////////////////
    var imp1 = 0.0;
    var imp2 = 0.0;
    var ajuste = 0.0;
    _ajuste2 = 0.0;

    $.each(source15.localdata, function (index, value) {

        //imp1 += value.IMPORTE;
        //imp2 += value.TOTAL + value.PRECIO
        ajuste += value.PRECIO_IMP;
        

    });


    $("#tAj1").html(numeral(ajuste).format('0.00'));


    $("#tAlb1").html(numeral(numeral($("#talb1").html()).value() + ajuste).format('0.00'));

    //$("#tfac1").html(numeral(imp1).format('0.00'));
    //$("#talb1").html(numeral(imp2).format('0.00'));


    /*
    $.each(source15.localdata, function (index, value) {

    for (i = source13.localdata.length - 1; i >= 0; i--) {

    if (source13.localdata[i].DESCRIPCION == value.DESCRIPCION) {
    source13.localdata.splice(i, 1);
    break;
    }
    }
    });*/

    source13.localdata.splice(_inx1[0], 1);

    dataAdapter13.dataBind();
    $("#jGrid13").jqxGrid('clearselection');
    //////////////////////////////////////////////////////////



    source14.localdata.splice(_inx2[0], 1);

    dataAdapter14.dataBind();
    $("#jGrid14").jqxGrid('clearselection');




    $("#jGrid15").jqxGrid("autoresizecolumns");

    var colDefs = $("#jGrid15").jqxGrid('columns').records;
    for (var idx = 0; idx < colDefs.length; idx++) {
        if (colDefs[idx].datafield != "_checkboxcolumn") {

            $("#jGrid15").jqxGrid('setcolumnproperty', colDefs[idx].datafield, 'width', colDefs[idx].width + 5);
        }
    }
}