$(document).ready(function () {
   
    fn_InicializaCampos();
});

function fn_InicializaCampos() { 
    $("#txtDiasAtraso").fn_util_validarNumeros("POS");
    $("#txtMontoMinimo").fn_util_validaDecimal(2, "POS");
 
}


var sNombreUsuario2 = fn_StrongString(fn_ObtenerUsuario());
(function () {
    $("#txtDiasAtraso").fn_util_validarNumeros("POS");
    $("#txtMontoMinimo").fn_util_validaDecimal(2, "POS");
    var $iIdCondicion = $.trim($("#siCodigoTabla").val());


    fn_cargarDatosProductos($iIdCondicion);
    fn_CargarProductosCuadros($iIdCondicion);

    //Producto Ahorro
    fn_cargarDatosProductosAhorro($iIdCondicion);
    fn_CargarProductosCuadrosAhorro($iIdCondicion);

    $("#chkTodosCar").change(function () {
        if ($(this).is(':checked')) {
            $('input[name="chk_pro"]', '#tblReglasProds').prop('checked', true);
        } else {
            $('input[name="chk_pro"]', '#tblReglasProds').prop('checked', false);
        }
    });

    $("#chkTodosDis").change(function () {
        if ($(this).is(':checked')) {
            $("input[name='chkProductos']", "#tblProductoCredito").prop('checked', true);
        } else {
            $("input[name='chkProductos']", "#tblProductoCredito").prop('checked', false);
        }
    });

    $(".ui-tabs-anchor").click(function () {
        $("#divConfiguracion").hide();
    });



    $("#chkTodosCarAhorro").change(function () {
        if ($(this).is(':checked')) {
            $('input[name="chk_proAhorro"]', '#tblReglasProdsAhorro').prop('checked', true);
        } else {
            $('input[name="chk_proAhorro"]', '#tblReglasProdsAhorro').prop('checked', false);
        }
    });

    $("#chkTodosDisAhorro").change(function () {
        if ($(this).is(':checked')) {
            $("input[name='chkProductosAhorro']", "#tblProductoAhorro").prop('checked', true);
        } else {
            $("input[name='chkProductosAhorro']", "#tblProductoAhorro").prop('checked', false);
        }
    });

    $(".ui-tabs-anchor").click(function () {
        $("#divConfiguracion").hide();
    });


})(jQuery);

function fn_GuardarProducto() {
 
    //var iIdDiasAtraso = parseInt($("#txtDiasAtraso").text());
   
    $("#tblReglasProds_filter input[type=search]").val("");

    var oTable = $('#tblReglasProds').dataTable();
    oTable.fnFilter('');

    $("#tblReglasProdsAhorro_filter input[type=search]").val("");

    var oTable1 = $('#tblReglasProdsAhorro').dataTable();
    oTable1.fnFilter('');


    var oCartera = {};
    var loCartera = [];
    var loCarteraAhorro = [];
 

    $('#tblReglasProds tbody tr').each(function (index, element) {
        oCartera = new Object();
        loCartera.push($(element).find("td").eq(1).html());
    });
    //Ahorro
    $('#tblReglasProdsAhorro tbody tr').each(function (index, element) {
        oCartera  = new Object();
        loCarteraAhorro.push($(element).find("td").eq(1).html());
    });
    fn_GrabarProductosCuadro(loCartera, loCarteraAhorro);
    
}
function fnAsignar() {

    if ($("input[name='chkProductos']:checked", "#tblProductoCredito").length == 0) {
        fn_util_alert("", sNombreUsuario + ". Debe seleccionar un producto.", "W", 3);
        return false;
    }
    else {
        fn_util_bloquearPantalla("Cargando");

        $('input:checkbox:checked', '#tblReglasProds').prop("checked", false);
        fnAsignacion();
        $('input:checkbox:checked', '#tblProductoCredito').prop("checked", false);

        var a = $("#tblReglasProds").dataTable().fnGetData().length;
        $('#spnCartera').html('Total de registros: ' + a.toString());
        var b = $("#tblProductoCredito").dataTable().fnGetData().length;
        $('#spnDisponibles').html('Total de registros: ' + b.toString());
    }
}

function fnAsignacion() {
    fn_util_bloquearPantalla("Cargando");
    var oTableDisponibles = $('#tblProductoCredito').dataTable();
    var oTableCartera = $('#tblReglasProds').dataTable();
    var loProducto = [];
    var oProducto = {};
    $('input:checkbox:checked', '#tblProductoCredito tbody tr').each(function (i) {

        if ($(this).is(":checked")) {

            var row = $(this).closest('tr');

            oProducto = oTableDisponibles.fnGetData(row);
            oProducto.iRelevancia = 0;
            loProducto.push(oProducto);

            oTableDisponibles.fnDeleteRow(row.get(0));

        }
    });
    oTableCartera.fnAddData(loProducto);
    fn_LimpiarBuscarTabla('tblReglasProds');
    fn_LimpiarBuscarTabla('tblProductoCredito');
    fn_util_desbloquearPantalla();
}

function fnDesasignar() {

    if ($("input[name='chk_pro']:checked", "#tblReglasProds").length == 0) {
        fn_util_alert("", sNombreUsuario + ". Debe seleccionar un producto.", "W", 3);
        return false;
    }
    else {

        fn_util_bloquearPantalla("Cargando");
        $('input:checkbox:checked', '#tblProductoCredito').prop("checked", false);
        fnDesasignacion();
        $('input:checkbox:checked', '#tblReglasProds').prop("checked", false);

        var a = $("#tblReglasProds").dataTable().fnGetData().length;
        $('#spnCartera').html('Total de registros: ' + a.toString());
        var b = $("#tblProductoCredito").dataTable().fnGetData().length;
        $('#spnDisponibles').html('Total de registros: ' + b.toString());
    }
}

function fnDesasignacion() {
    fn_util_bloquearPantalla("Cargando");
    var oTableDisponibles = $('#tblProductoCredito').dataTable();
    var oTableCartera = $('#tblReglasProds').dataTable();
    var loProducto = [];
    var oProducto = {};
    $('input:checkbox:checked', '#tblReglasProds tbody tr').each(function (i) {
        if ($(this).is(":checked")) {

            var row = $(this).closest('tr');

            oProducto = oTableCartera.fnGetData(row);
            oProducto.iRelevancia = 1;
            loProducto.push(oProducto);
            // poEliominado.push(oProducto);
            oTableCartera.fnDeleteRow(row.get(0));
        }
    });

    oTableDisponibles.fnAddData(loProducto);
    fn_LimpiarBuscarTabla('tblReglasProds');
    fn_LimpiarBuscarTabla('tblProductoCredito');
    fn_util_desbloquearPantalla();

}

function fn_CargarProductosCuadros(idCuadro) {
    var oTable = $('#tblReglasProds').dataTable({
        "bServerSide": false,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Configuracion/ListarProductosCondicionCreditoVencidos",
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": true,
        "bSort": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
        "aoColumns": [
            {},
            { "sName": "nom_pro", "mDataProp": "sDescripcionTipoMoneda", "sClass": "left", },
            { "sName": "tip_mon", "mDataProp": "sDescripcion", "sClass": "left", },
            { "sName": "cod_pro", "mDataProp": "iIdProducto", "sClass": "center", "sWidth": "100px" }
        ],
        "fnDrawCallback": function (oSettings) {
            $(".dataTables_filter").show();
            fn_ProductoDisponibles();
        },
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {

            //Cargando
            fn_util_bloquearPantalla("Buscando");

            //Parámetros


            var sParams = { "idCuadro": idCuadro, "iiActivo": 2 };

            //Llamada JQuery-JSON
            oSettings.jqXHR = fn_util_AjaxWM_Obj(sSource, sParams, function (result) {

                fnCallback(result);
            }, function (error) {
                fn_util_desbloquearPantalla();
                fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al listar las tablas, vuélvalo a intentar o comuníquese con su administrador.", "E");
            });
        },
        "aoColumnDefs":
            [
                {
                    "mRender": function (data, type, row) {
                        var rowIndex = oTable.fnGetData().length - 1;
                        var strNameIdChek = 'chk_1_' + rowIndex;
                        var x = row.iCodigoCartera;

                        var strHtml = "<input type='checkbox' name='chk_pro' id='" + strNameIdChek + "' value='" + x + "' style='width:15px;' iRow='" + rowIndex + "' />";

                        return strHtml;
                    },
                    "sWidth": "50px",
                    "sClass": "center",
                    "bSort": false,
                    "aTargets": [0]
                }
            ]
    });

}

function fn_ProductoDisponibles() {
    var a = $("#tblReglasProds").dataTable().fnGetData().length;
    $('#spnCartera').html('Total de registros: ' + a.toString());
    var b = $("#tblProductoCredito").dataTable().fnGetData().length;
    $('#spnDisponibles').html('Total de registros: ' + b.toString());
}

function fn_ContarProductos() {

    var b = $("#tblProductoCredito").dataTable().fnGetData().length;
    $('#spnDisponibles').html('Total de registros: ' + b.toString());
}

function fn_cargarDatosProductos(idCuadro) {

    //*******************************************
    //Declaración de Grilla
    //*******************************************
    var oTable = $('#tblProductoCredito').dataTable({
        "bServerSide": false,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Configuracion/ListarProductosCondicionCreditoVencidos",
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": true,
        "bSort": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
        "fnDrawCallback": function (oSettings) {
            $(".dataTables_filter").show();
            fn_ContarProductos();
            fn_util_desbloquearPantalla();

        },
        "aoColumns": [
            {},
            { "sName": "Código de Producto", "mDataProp": "sDescripcionTipoMoneda", "sClass": "center" },
            { "sName": "Nombre de Producto", "mDataProp": "sDescripcion", "sClass": "left" },
            { "sName": "Moneda", "mDataProp": "iIdProducto", "sClass": "left" },


        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            //Variables
            var sParams = { "idCuadro": idCuadro, "iiActivo": 1 };
            //Llamada JQuery-JSON
            oSettings.jqXHR = fn_util_AjaxWM_Obj(sSource, sParams, function (result) {

                fnCallback(result);
            }
                , function (error) {
                    fn_util_desbloquearPantalla();
                    fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al listar los datos, vuélvalo a intentar o comuníquese con su administrador.", "E");
                });
        },
        "aoColumnDefs":
            [
                {
                    "mRender": function (data, type, row) {
                        var rowIndex = oTable.fnGetData().length - 1;
                        var strNameIdImg = "IMG";
                        var strHtml = fn_util_CrearInputGrilla("checkbox", "chkProductos", "chkProducto_" + rowIndex, row.iIdProducto, "", "", "", "", "", "");
                        return strHtml;
                    },
                    "sWidth": "5px",
                    "sClass": "center",
                    "bSort": false,
                    "aTargets": [0]
                }
            ]
    });


}

function fn_LimpiarBuscarTabla($NombreTabla) {
    $("#tblProductoCredito_filter input[type=search]").val("");
    $("#tblCarteraProds_filter input[type=search]").val("");
    var oTable = $('#' + $NombreTabla + '').dataTable();
    oTable.fnFilter('');

    $('input:checkbox:checked', '#tblProductoCredito').prop("checked", false);
    $('input:checkbox:checked', '#tblReglasProds').prop("checked", false);
    $("#tblProductoAhorro_filter input[type=search]").val("");
    $("#tblReglasProdsAhorro_filter input[type=search]").val("");
    var oTable1 = $('#' + $NombreTabla + '').dataTable();
    oTable1.fnFilter('');

    $('input:checkbox:checked', '#tblProductoAhorro').prop("checked", false);
    $('input:checkbox:checked', '#tblReglasProdsAhorro').prop("checked", false);

}

function fn_GrabarProductosCuadro(loCartera, loCarteraAhorro) {

    
    var sDescripcion = $("#txtSistema").val();
    var piIdCuadro = $("#siCodigoTabla").val();

    var psMensaje = sNombreUsuario + ", ¿Confirma que desea guardar los productos?";
    var sMensaje = 'Se agregaron los productos al cuadro <b>' + sDescripcion + '</b> satisfactoriamente.';

    var diasAtraso = parseInt($("#txtDiasAtraso").val());
    var MontoMinimo = parseInt($("#txtMontoMinimo").val());

    if (isNaN(diasAtraso) || diasAtraso == null || diasAtraso == undefined ) {
        diasAtraso = 0;
    } 

    if (isNaN(MontoMinimo) || MontoMinimo == null || MontoMinimo == undefined ) {
        MontoMinimo = 0.00;
    }

  
    var oParams = {
        "ploProductos": loCartera.map(Number),
        "ploProductosAhorro": loCarteraAhorro.map(Number),
        "psDescripcion": sDescripcion,
        "piCodigoCartera": piIdCuadro,
        "psiTipoOperacion": 1,
        "DiasAtraso": diasAtraso,
        "MontoMinimo": MontoMinimo
    };

    $('.dialog-text').html(psMensaje);



    $("#dialog-confirm").dialog({
        title: "Confirmación",
        resizable: false,
        height: "auto",
        width: 400,
        modal: true,
        draggable: false,
        closeOnEscape: false,
        buttons: {
            "SI": function (button) {

                fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Configuracion/MantenerProductoCondicionCreditoVencido", oParams, function (result) {

                    fn_util_desbloquearPantalla();
                    if (result.iResultado) {

                        // $("#iptProductos").val($('#tblCarteraProds tbody tr').length);
                        fn_util_alert("", sMensaje, "S");

                    }
                    else {
                        fn_util_alert("", sNombreUsuario + ", hubo un problema al momento de registrar los productos.", "W");
                    }


                }, function (data) {

                    fn_util_desbloquearPantalla();
                    fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al registrar los productos, vuélvalo a intentar o comuníquese con su administrador.", "E");
                });


                $(this).dialog("close");
            },
            "NO": function () {
                fn_util_desbloquearPantalla();
                $(this).dialog("close");
            }
        }
    });

}
function fn_StrongString(sPrimerNombre) {
    return "<strong>" + sPrimerNombre + "</strong>";
}

function fn_Volver() {
    $("#frmBandejaCon").submit();
}

//Parametrizacion Cuenta Ahorros
function fn_ProductoDisponiblesAhorro() {
    var a = $("#tblReglasProdsAhorro").dataTable().fnGetData().length;
    $('#spnCartera1').html('Total de registros: ' + a.toString());
    var b = $("#tblProductoAhorro").dataTable().fnGetData().length;
    $('#spnDisponibles1').html('Total de registros: ' + b.toString());
}

function fn_ContarProductosAhorros() {

    var b = $("#tblProductoAhorro").dataTable().fnGetData().length;
    $('#spnDisponibles1').html('Total de registros: ' + b.toString());
}

function fn_cargarDatosProductosAhorro(idCuadro) {

    //*******************************************
    //Declaración de Grilla
    //*******************************************
    var oTable = $('#tblProductoAhorro').dataTable({
        "bServerSide": false,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Configuracion/ListarProductosCondicionCreditoVencidosAhorro",
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": true,
        "bSort": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
        "fnDrawCallback": function (oSettings) {
            $(".dataTables_filter").show();
            fn_ContarProductosAhorros();
            fn_util_desbloquearPantalla();

        },
        "aoColumns": [
            {},
            { "sName": "Código de Producto", "mDataProp": "sDescripcionTipoMoneda", "sClass": "center" },
            { "sName": "Nombre de Producto", "mDataProp": "sDescripcion", "sClass": "left" },
            { "sName": "Moneda", "mDataProp": "iIdProducto", "sClass": "left" },


        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
         
            //Variables
            var sParams = { "idCuadro": idCuadro, "iiActivo": 1 };
            //Llamada JQuery-JSON
            oSettings.jqXHR = fn_util_AjaxWM_Obj(sSource, sParams, function (result) {

                fnCallback(result);
            }
                , function (error) {
                    fn_util_desbloquearPantalla();
                    fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al listar los datos, vuélvalo a intentar o comuníquese con su administrador.", "E");
                });
        },
        "aoColumnDefs":
            [
                {
                    "mRender": function (data, type, row) {
                        var rowIndex = oTable.fnGetData().length - 1;
                        var strNameIdImg = "IMG";
                        var strHtml = fn_util_CrearInputGrilla("checkbox", "chkProductosAhorro", "chkProductoAhorro_" + rowIndex, row.iIdProducto, "", "", "", "", "", "");
                        return strHtml;
                    },
                    "sWidth": "5px",
                    "sClass": "center",
                    "bSort": false,
                    "aTargets": [0]
                }
            ]
    });


}

function fn_CargarProductosCuadrosAhorro(idCuadro) {
 
    var oTable = $('#tblReglasProdsAhorro').dataTable({
        "bServerSide": false,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Configuracion/ListarProductosCondicionCreditoVencidosAhorro",
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": true,
        "bSort": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
        "aoColumns": [
            {},
            { "sName": "nom_pro", "mDataProp": "sDescripcionTipoMoneda", "sClass": "left", },
            { "sName": "tip_mon", "mDataProp": "sDescripcion", "sClass": "left", },
            { "sName": "cod_pro", "mDataProp": "iIdProducto", "sClass": "center", "sWidth": "100px" }
        ],
        "fnDrawCallback": function (oSettings) {
            $(".dataTables_filter").show();
            fn_ProductoDisponiblesAhorro();
        },
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
      
            //Cargando
            fn_util_bloquearPantalla("Buscando");

            //Parámetros


            var sParams = { "idCuadro": idCuadro, "iiActivo": 2 };

            //Llamada JQuery-JSON
            oSettings.jqXHR = fn_util_AjaxWM_Obj(sSource, sParams, function (result) {

                fnCallback(result);
            }, function (error) {
                fn_util_desbloquearPantalla();
                fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al listar las tablas, vuélvalo a intentar o comuníquese con su administrador.", "E");
            });
        },
        "aoColumnDefs":
            [
                {
                    "mRender": function (data, type, row) {
                        var rowIndex = oTable.fnGetData().length - 1;
                        var strNameIdChek = 'chk_1_' + rowIndex;
                        var x = row.iCodigoCartera;

                        var strHtml = "<input type='checkbox' name='chk_proAhorro' id='" + strNameIdChek + "' value='" + x + "' style='width:15px;' iRow='" + rowIndex + "' />";

                        return strHtml;
                    },
                    "sWidth": "50px",
                    "sClass": "center",
                    "bSort": false,
                    "aTargets": [0]
                }
            ]
    });

}

 //Asignacion de Productos Vencidos
function fnAsignarAhorro() {

    if ($("input[name='chkProductosAhorro']:checked", "#tblProductoAhorro").length == 0) {
        fn_util_alert("", sNombreUsuario + ". Debe seleccionar un producto.", "W", 3);
        return false;
    }
    else {
        fn_util_bloquearPantalla("Cargando");

        $('input:checkbox:checked', '#tblReglasProdsAhorro').prop("checked", false);
        fnAsignacionAhoro();
        $('input:checkbox:checked', '#tblProductoAhorro').prop("checked", false);

        var a = $("#tblReglasProdsAhorro").dataTable().fnGetData().length;
        $('#spnCartera1').html('Total de registros: ' + a.toString());
        var b = $("#tblProductoAhorro").dataTable().fnGetData().length;
        $('#spnDisponibles1').html('Total de registros: ' + b.toString());
    }
}

function fnAsignacionAhoro() {
    fn_util_bloquearPantalla("Cargando");
    var oTableDisponibles = $('#tblProductoAhorro').dataTable();
    var oTableCartera = $('#tblReglasProdsAhorro').dataTable();
    var loProducto = [];
    var oProducto = {};


    $('input:checkbox:checked', '#tblProductoAhorro tbody tr').each(function (i) {

        if ($(this).is(":checked")) {

            var row = $(this).closest('tr');

            oProducto = oTableDisponibles.fnGetData(row);
            oProducto.iRelevancia = 0;
            loProducto.push(oProducto);

            oTableDisponibles.fnDeleteRow(row.get(0));

        }
    });
    oTableCartera.fnAddData(loProducto);
    fn_LimpiarBuscarTabla('tblReglasProdsAhorro');
    fn_LimpiarBuscarTabla('tblProductoAhorro');
    fn_util_desbloquearPantalla();
}

function fnDesasignarAhorro() {

    if ($("input[name='chk_proAhorro']:checked", "#tblReglasProdsAhorro").length == 0) {
        fn_util_alert("", sNombreUsuario + ". Debe seleccionar un producto.", "W", 3);
        return false;
    }
    else {

        fn_util_bloquearPantalla("Cargando");
        $('input:checkbox:checked', '#tblProductoAhorro').prop("checked", false);
        fnDesasignacionAhorro();
        $('input:checkbox:checked', '#tblReglasProdsAhorro').prop("checked", false);

        var a = $("#tblReglasProdsAhorro").dataTable().fnGetData().length;
        $('#spnCartera1').html('Total de registros: ' + a.toString());
        var b = $("#tblProductoAhorro").dataTable().fnGetData().length;
        $('#spnDisponibles1').html('Total de registros: ' + b.toString());
    }
}

function fnDesasignacionAhorro() {
    fn_util_bloquearPantalla("Cargando");
    var oTableDisponibles = $('#tblProductoAhorro').dataTable();
    var oTableCartera = $('#tblReglasProdsAhorro').dataTable();
    var loProducto = [];
    var oProducto = {};
    $('input:checkbox:checked', '#tblReglasProdsAhorro tbody tr').each(function (i) {
        if ($(this).is(":checked")) {

            var row = $(this).closest('tr');

            oProducto = oTableCartera.fnGetData(row);
            oProducto.iRelevancia = 1;
            loProducto.push(oProducto);
            // poEliominado.push(oProducto);
            oTableCartera.fnDeleteRow(row.get(0));


        }
    });

    oTableDisponibles.fnAddData(loProducto);
    fn_LimpiarBuscarTabla('tblReglasProdsAhorro');
    fn_LimpiarBuscarTabla('tblProductoAhorro');
    fn_util_desbloquearPantalla();

}

function  fn_GuardarProductoAhorroOrdenado() {
   
 
    $("#tblReglasProds_filter input[type=search]").val("");

    var oTable = $('#tblReglasProds').dataTable();
    oTable.fnFilter('');

    $("#tblReglasProdsAhorro_filter input[type=search]").val("");

    var oTable1 = $('#tblReglasProdsAhorro').dataTable();
    oTable1.fnFilter('');



    var oCartera = {};

    var loCartera = [];
    var loCarteraAhorro = [];


    $('#tblReglasProds tbody tr').each(function (index, element) {
        oCartera = new Object();
        loCartera.push($(element).find("td").eq(1).html());
    });

    //Ahorro
    $('#tblReglasProdsAhorro tbody tr').each(function (index, element) {
        oCartera = new Object();
        loCarteraAhorro.push($(element).find("td").eq(1).html());
    });

  
    fn_GrabarProductosCuadro(loCartera, loCarteraAhorro);
    
}

 