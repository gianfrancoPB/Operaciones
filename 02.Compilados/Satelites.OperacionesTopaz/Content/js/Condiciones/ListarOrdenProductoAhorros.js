$(document).ready(function () {

    fn_InicializaCampos();
    fn_cargarDatosProductosAhorro("1");
});

function fn_InicializaCampos() {


    $(function () {
        $("#tblReglasProdsAhorroOrden").sortable({
            items: 'tr:not(:has(th))',
            dropOnEmpty: true,
            start: function (event, ui) {
                ui.item.addClass("select");
            },
            stop: function (event, ui) {
                ui.item.removeClass("select");
                $(this).find("tr:not(:has(th))").each(function (index) {
              
                    $(this).find("td").eq(0).html(index + 1);
                });
            }
        });
    });


}
 
function fn_cargarDatosProductosAhorro(idCuadro) { 
    var oTable = $('#tblReglasProdsAhorroOrden').dataTable({
        "bServerSide": false,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Configuracion/ListarProductosCondicionProductoAhorroOrden",
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": false,
        "bSort": false, 
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,
        "aoColumns": [
            
            {},
           
            { "sName": "nom_pro", "mDataProp": "psDescripcionProducto", "sClass": "center", },
            { "sName": "tip_mon", "mDataProp": "iCodigoProducto", "sClass": "center", },
        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            var sParams = { "idCuadro": idCuadro, "iiActivo": 1 };

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
                        var x = row.iNumeroOrden;
                        var strHtml = x  ;
                        return strHtml  ;
                    },
                    "sWidth": "50px",
                    "sClass": "center",
                    "bSort": false,
                    "aTargets": [0]
                }
            ]
    });
 
}

function fn_NuevaAutonomiaAhorro() {

    fn_util_AjaxTypeHTML(VG_RUTA_SERVIDOR + 'Configuracion/ListaProductoAhorro', [], function (data) {
 
        var botones = [{ "text": "Guardar", "class": "btnGrabar", "id": "btnGrabar", "click": function () { fn_GuardarProductoAhorroOrdenado(); } },
        { "text": "Cancelar", "class": "btnCerrar", "id": "btnCancelar", "click": function () { $(this).dialog("close"); } },];
 
        var title = "Priorizar Producto Ahorros";
        var width = $(window).width();
        var height = $(window).height();
        $("#dialog-form").attr('title', title);
        $("#dialog-form").html(data);
        $("#dialog-form").dialog({
            autoOpen: true,
            height: "445", //480
            width: "530px", //520
            modal: true,
            resizable: false,
            closeOnEscape: false,
            draggable: true,
            buttons: botones,
            dialogClass: 'no-close',
            beforeClose: function () {
                $(this).html("");
                $(this).dialog('destroy');
            }
        });
    });
}

function fn_GuardarProductoAhorroOrdenado() {
  
    var oCartera12 = {};
     
  var loCarteraAhorro = [];

    $('#tblReglasProdsAhorroOrden tbody tr').each(function (index, element) {
        var fila = {};
        fila.iNumeroOrden = $(element).find("td").eq(0).html();
        fila.iCodigoProducto = $(element).find("td").eq(1).html();
        fila.psDescripcionProducto = $(element).find("td").eq(2).html();
        loCarteraAhorro.push(fila);
    });

   fn_GrabarProductosCuadroAhorroOrden(loCarteraAhorro);
}

function fn_GrabarProductosCuadroAhorroOrden(loCarteraAhorro) {


    var sDescripcion = $("#txtSistema").val();
    var piIdCuadro = $("#siCodigoTabla").val();

    var psMensaje = sNombreUsuario + ", ¿Confirma que desea guardar el orden de los productos?";
    var sMensaje = 'Se agregaron los productos al cuadro <b>' + sDescripcion + '</b> satisfactoriamente.';
 
    var oParams = {
    
        "ploProductosAhorro": loCarteraAhorro,
        "loProductoAhorro": loCarteraAhorro,
        "psDescripcion": sDescripcion,
        "piCodigoCartera": piIdCuadro,
        "psiTipoOperacion": 1,
     
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

                fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Configuracion/GuardarOrdenProductoAhorro", oParams, function (result) {

                    fn_util_desbloquearPantalla();
                    if (result.iResultado) {
                        window.location.reload()
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

 