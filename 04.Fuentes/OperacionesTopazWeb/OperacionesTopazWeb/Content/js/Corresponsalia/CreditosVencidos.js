var sNombreUsuario = fn_ObtenerUsuario();


$(document).ready(function () {


})

function fn_GeneracionDeuda() {
    fn_util_bloquearPantalla("Generando Data");



    ///  DATOS DEL REFERENTE         
    var oParametros = {

        "iIAccion": 1
    };

    fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Corresponsalia/GeneracionDeudaCV", oParametros,
    function (data) {
        var oEnCorresponsalia = data.oEnCorresponsalia;
        fn_util_desbloquearPantalla();

        if (data.iError == 0)
        {
           
            if (oEnCorresponsalia.iAccion == 1) {
                fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.sFechaProceso,oEnCorresponsalia.iAccion);
                fn_util_alert("", "Se genero la data satisfactoriamente.", "S");
            } else if (oEnCorresponsalia.iAccion == 2) {
                fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.sFechaProceso, oEnCorresponsalia.iAccion);
                fn_util_alert("", "Se exporto la data satisfactoriamente.", "S");
            } else {
                fn_util_alert("", "Ocurrió un inconveniente para procesar la accion, vuélvalo a intentar o comuníquese con su administrador.", "E");
            }

        }
        else {
            fn_util_alert("", "Ocurrió un inconveniente con la generación de deuda, vuélvalo a intentar o comuníquese con su administrador.", "E");
        }
    },
    function (error) {
        fn_util_desbloquearPantalla();
        fn_util_alert("", "Ocurrió un inconveniente con la generación de deuda, vuélvalo a intentar o comuníquese con su administrador.", "E");
    });

}

function fn_SetearCampos(sNombresCompleto, sDescripcionAccion, iCantidadRegistros, sFechaProceso, iAccion) {

    $("#txtNombreUsuario").val(sNombresCompleto);
    $("#txtMensaje").val(sDescripcionAccion);
    $("#txtCantidadRegistros").val(iCantidadRegistros);
    $("#txtFechaProceso").val(sFechaProceso);
    if (iAccion == 1) {
        $("#dvCampos").show();
        $("#btnDescargarDataCV").show();
        $("#btnVolverGenerarData").show();
        
        $("#btnGenerarData").hide();
        $("#btnDescargarDataCV").prop("disabled", false);
        $("#btnVolverGenerarData").prop("disabled", false);
        $("#btnGenerarData").prop("disabled", true);

        if (iCantidadRegistros > 0) {
            fn_util_alert("", "Se genero la data satisfactoriamente.", "S");
        } else {
            fn_util_alert("", "No hay data para generar.", "W");
        }
    }
    if (iAccion == 2) {
        $("#dvCampos").hide();
        $("#btnDescargarDataCV").hide();
        $("#btnVolverGenerarData").hide();
        $("#btnGenerarData").show();
        $("#btnDescargarDataCV").prop("disabled", true);
        $("#btnVolverGenerarData").prop("disabled", true);
        $("#btnGenerarData").prop("disabled", false);
        fn_util_alert("", "Se exporto la data satisfactoriamente.", "S");
    }

}



function fn_DescargarDataCV()
{
    

    fn_util_bloquearPantalla("Descargando ...");
    fn_util_AjaxWM(
    VG_RUTA_SERVIDOR + 'Corresponsalia/Exportar_PagosCV',
    [],
    function (oResultado) {

        if (oResultado.iTipoResultado == 1) {
            window.location = VG_RUTA_SERVIDOR + 'Corresponsalia/DownloadPagosCV?piFlag=' + 1;
        } else if (oResultado.iTipoResultado == 2) {
            $("#spMensajeAlerta").html("No hay datos para exportar.");
            $("#mdlAlerta").dialog({
                modal: true,
                draggable: false,
                resizable: false,
                closeOnEscape: false,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        fn_util_alert("", "Se descargoron los pagos satisfactoriamente.", "S");
        fn_util_desbloquearPantalla();
    },
    function (result) {
        fn_util_desbloquearPantalla();
        fn_util_alert("", "Ocurrió un problema al descargar, vuélvalo a intentar o comuníquese con su administrador.", "E");
    }
    );

}



