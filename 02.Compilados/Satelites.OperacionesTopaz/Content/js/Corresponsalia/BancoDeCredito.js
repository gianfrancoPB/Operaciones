var iBusquedaSoles = 0;
var iBusquedaDolares = 0;
var iImportaSoles = 0;
var iImportaDolares = 0;
var elemento = ''
var objArchivo = []
var sNombreUsuario = fn_ObtenerUsuario();
var ArchivoSoles = "";
var ArchivoDolares = "";
$(document).ready(function () {

    fn_InicializarTab();
    //fn_GeneracionDeudaInicial(2);
    fn_SetearCamposGeneracionPagos(0);
})
document.getElementById('file-input').addEventListener('change', leerArchivo, false);
function leerArchivo(e) {
    

    if ($('#file-input').val() == "")
    {
        fn_util_alert("", sNombreUsuario + ", no se ha seleccionado ningun archivo", "W");
    }
    else
    {
        if ($('#file-input').val() == ArchivoSoles)
        {
            fn_util_alert("", sNombreUsuario + ", ya se encuentra cargado ah sido exportado el archivo de soles", "W");
        }else if ($('#file-input').val() == ArchivoDolares) {
            fn_util_alert("", sNombreUsuario + ", ya se encuentra cargado ah sido exportado el archivo de dolares", "W");
        }else{
            fn_util_bloquearPantalla("Cargando Archivo")
            $("#iptNombreArchivo").val(e.target.files[0].name);

            var archivo = e.target.files[0];
            if (!archivo) {
                return;
            }
            var lector = new FileReader();
            lector.onload = function (e) {
                var contenido = e.target.result;
                mostrarContenido(contenido);
            };
            lector.readAsText(archivo);
        }

    }

}
$('input[type="file"]').change(function (e) { 
    var fileName = e.target.files[0];
    var files = $(this).val().replace(/C:\\fakepath\\/i, '');
    var piTipoCarga = $("#iptTipoCarga").val();

    if ((fileName.size / 1048576) <= 3) {
        var xhr = new XMLHttpRequest();
        var fd = new FormData();

        $("#iptNombreArchivo").val(document.getElementById('file-input').files[0].name);

        fd.append("file", document.getElementById('file-input').files[0]);
        xhr.open("POST", VG_RUTA_SERVIDOR + "Corresponsalia/SubirArchivosPagos/", true);
        xhr.send(fd);
        xhr.addEventListener("load", function (event) { }, false);
    }
    else {
        $('input[type="file"]').val("");

        fn_util_alert("", "El peso del archivo es mayor a 3MB", "W");

    }


});
function mostrarContenido(contenido) {
    elemento = ''
    elemento = document.getElementById('contenido-archivo').innerHTML = contenido;

    lsBCP_PAGOS = []                                                         // Seteando el Objeto 0
    var sBCP_PAGOS = $('#contenido-archivo').text().split('\n');            // ARRAY DEL ARCHIVO SEPARADOS \n


    sBCP_PAGOS.forEach(sBCP_PAGOS => {                                      // Obteniendo informacion del array hacia el Objeto
        lsBCP_PAGOS.push({ sBCP_PAGOS })
    })



    //if ($('#iptNombreArchivo').val() == "CREP6250.txt") {
    //    fn_SetearCamposGeneracionPagos(1);
    //}
    //else if ($('#iptNombreArchivo').val() == "CREP6251.txt") {
    //    fn_SetearCamposGeneracionPagos(2);
    //}
    var nombreArchivo = $('#iptNombreArchivo').val().toUpperCase();    //Este codigo lo convierte en mayuscula si viene el archivo .TXT en minuscula , y tambien acepta el .txt

    if (nombreArchivo === "CREP6250.TXT") {
        fn_SetearCamposGeneracionPagos(1);
    } else if (nombreArchivo === "CREP6251.TXT") {
        fn_SetearCamposGeneracionPagos(2);
    }

    else {

        fn_SetearCamposGeneracionPagos(3);
        fn_util_alert("", sNombreUsuario + " Debe seleccionar un archivo valido para exportar los pagos", "W");

    }
    
}
function fn_GerenacionPagos(Flag) {
   
    var sMsj = Flag == 1 ? "Generando pagos soles" : "Generando pagos dolares";
    fn_util_bloquearPantalla(sMsj);



    if (Flag == 1) { iImportaSoles++ }
    if (Flag == 2) { iImportaDolares++ }
    if (iImportaSoles > 0) { ArchivoSoles = $('#file-input').val() }
    if (iImportaDolares > 0) { ArchivoDolares = $('#file-input').val() }




    ///  DATOS DEL REFERENTE         
    var oParametros = { "iIAccion": Flag };

    fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Corresponsalia/GeneracionPagos", oParametros, function (data) {


        var oRespuesta = data.aData;

        if (oRespuesta.iError == 1) {
            fn_util_alert("", sNombreUsuario + ", " + oRespuesta.sMensaje, "W", 3);
        }
        else {

            $('.dialog-text').html(sNombreUsuario + ", " + oRespuesta.sMensaje);
            $("#dialog-confirm").dialog({
                title: "MENSAJE",
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                closeOnEscape: false,
                draggable: false,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                },
                beforeClose: function () {

                    if (iImportaSoles > 0 && iImportaDolares > 0) {
                        iBusquedaSoles = 0;
                        iBusquedaDolares = 0;
                        fn_SetearCamposGeneracionPagos(4);
                    } else {
                        fn_SetearCamposGeneracionPagos(0);
                    }

                    $(this).dialog('destroy');
                }
            });


            // fn_util_alert("", sNombreUsuario + ", " + oRespuesta.sMensaje, "S",3);

        }
        fn_util_desbloquearPantalla();

    },
    function (error) {
        fn_util_desbloquearPantalla();
        fn_util_alert("", "Ocurrió un inconveniente con la generación de deuda, vuélvalo a intentar o comuníquese con su administrador.", "E");
    });

}
function fu_GeneracionPagosConfirm(Flag)
{
    
    var sMsj = Flag == 1 ? "pagos soles" : "pagos dolares";

    $('.dialog-text').html(sNombreUsuario + ", ¿Desea generar los " + sMsj + " ?");
    $("#dialog-confirm").dialog({
        title: "MENSAJE DE CONFIRMACIÓN",
        resizable: false,
        height: "auto",
        width: 400,
        modal: true,
        closeOnEscape: false,
        draggable: false,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
                fn_util_bloquearPantalla("Cargando...");
                fn_GerenacionPagos(Flag)

            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        },
        beforeClose: function () {
            $(this).dialog('destroy');
        }
    });
}
function fu_ExportarPagosConfirm()
{
    

    fn_util_bloquearPantalla("Descargando ...");
    fn_util_AjaxWM(
    VG_RUTA_SERVIDOR + 'Corresponsalia/Exportar_Pagos',
    [],
    function (oResultado) {

        if (oResultado.iTipoResultado == 1) {
            window.location = VG_RUTA_SERVIDOR + 'Corresponsalia/DownloadPagos?piFlag=' + 1;
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
        fn_util_alert("", "Ocurrió un problema al descargar los pagos, vuélvalo a intentar o comuníquese con su administrador.", "E");
    }
    );

}
function fn_Limpiar() {
    location.reload();
}
function fn_InicializarTab() {
    $("#tabs-solicitud").tabs();
}
function fn_GeneracionDeudaInicial(Flag) {      
    var oParametros = { "iIAccion": Flag };

    fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Corresponsalia/GeneracionDeuda", oParametros,
    function (data) {
        var oEnCorresponsalia = data.oEnCorresponsalia;
        fn_util_desbloquearPantalla();

        if (data.iError == 0) {


            if (oEnCorresponsalia.iAccion == 1) {
                //fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.iAccion);
                //fn_util_alert("", "Se genero la data satisfactoriamente.", "S");
            } else if (oEnCorresponsalia.iAccion == 2) {
                //fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.iAccion);
                //fn_util_alert("", "Se exporto la data satisfactoriamente.", "S");
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
function fn_GeneracionDeuda(Flag) {

    if (Flag == 1)
        {
            fn_util_bloquearPantalla("Generando Data");
        }
    else if (Flag == 2)
        {
            fn_util_bloquearPantalla("Exportando Archivo");
        }

    ///  DATOS DEL REFERENTE         
    var oParametros = {

        "iIAccion": Flag
    }; 
    fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Corresponsalia/GeneracionDeuda", oParametros,
    function (data) {
        var oEnCorresponsalia = data.oEnCorresponsalia;
        fn_util_desbloquearPantalla();

        if (data.iError == 0)
        {
           
            if (oEnCorresponsalia.iAccion == 1) {
                fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.sFechaProceso, oEnCorresponsalia.iAccion);
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
function fn_Exportar(x) {
    
    var iFlag = x;
    var sMsj = iFlag == 1 ? "archivo soles" : "archivo dolares";

    var bExito = true;


    if (iFlag == 1){iBusquedaSoles++}
    if (iFlag == 2){iBusquedaDolares++}
    if (iBusquedaSoles > 0)     { $("#btnExportaDataSoles").hide(); $("#btnExportaDataSoles").prop("disabled", true); }
    if (iBusquedaDolares > 0)   { $("#btnExportaDataDolares").hide(); $("#btnExportaDataDolares").prop("disabled", true); }

    if (iBusquedaSoles > 0 && iBusquedaDolares > 0)
    {
        iBusquedaSoles = 0;
        iBusquedaDolares = 0;
    
        fn_SetearCampos('', '', '', 0, 2);
    }
        fn_util_bloquearPantalla("Descargando "+sMsj+" ...");
        fn_util_AjaxWM(
        VG_RUTA_SERVIDOR + 'Corresponsalia/Exportar_Archivos',
        ["piiFlag", iFlag],
        function (oResultado) {

            if (oResultado.iTipoResultado == 1) {
                window.location = VG_RUTA_SERVIDOR + 'Corresponsalia/Download?piFlag=' + iFlag;
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
            fn_util_alert("", "Se descargo " + sMsj + " satisfactoriamente.", "S");
            fn_util_desbloquearPantalla();
        },
        function (result) {
            fn_util_desbloquearPantalla();
            fn_util_alert("", "Ocurrió un problema al descargar el" + sMsj + ", vuélvalo a intentar o comuníquese con su administrador.", "E");
        }
        );   
}
function fn_GeneracionDeuda(Flag) {

    var sMsj = Flag == 1 ? "Generando Data" : "Exportando Archivo";
    fn_util_bloquearPantalla(sMsj);


    ///  DATOS DEL REFERENTE         
    var oParametros = {

        "iIAccion": Flag
    };

    fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Corresponsalia/GeneracionDeuda", oParametros,
    function (data) {
        var oEnCorresponsalia = data.oEnCorresponsalia;
        if (data.iError == 0) {


            if (oEnCorresponsalia.iAccion == 1) {
                fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.sFechaProceso, oEnCorresponsalia.iAccion);
                
            } else if (oEnCorresponsalia.iAccion == 2) {
                fn_SetearCampos(oEnCorresponsalia.sNombresCompleto, oEnCorresponsalia.sDescripcionAccion, oEnCorresponsalia.iCantidadRegistros, oEnCorresponsalia.sFechaProceso, oEnCorresponsalia.iAccion);
            } else {
                fn_util_alert("", "Ocurrió un inconveniente para procesar la accion, vuélvalo a intentar o comuníquese con su administrador.", "E");
            }
            fn_util_desbloquearPantalla();
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
function fn_VolverGenerarPagos() {
    window.location.reload(false);
    //$('<a href="tabs-2"></a>')[0].click();
}
function fn_SetearCamposGeneracionPagos(iAccion)
{
    fn_util_bloquearPantalla("Cargando");
    if (iAccion == 0) //NO SE HABILITAN BOTONES
    {
        $("#btnImportarDataSoles").hide();
        $("#btnImportarDataDolares").hide();
        $("#btnDescargarDataDolares").hide();
        $("#btnVolverGenerarPagos").hide();
        $("#btnImportarDataSoles").prop("disabled", true);
        $("#btnImportarDataDolares").prop("disabled", true);
        $("#btnDescargarDataDolares").prop("disabled", true);
        $("#btnVolverGenerarPagos").prop("disabled", true);
        //$("#iptNombreArchivo").val("");
        document.getElementById('contenido-archivo').innerHTML = "";
    }
    if (iAccion == 1) { //SOLES
        $("#btnImportarDataSoles").show();
        $("#btnImportarDataDolares").hide();
        $("#btnDescargarDataDolares").hide();
        $("#btnVolverGenerarPagos").hide();
        $("#btnImportarDataSoles").prop("disabled", false);
        $("#btnImportarDataDolares").prop("disabled", true);
        $("#btnDescargarDataDolares").prop("disabled", true);
        $("#btnVolverGenerarPagos").prop("disabled", true);
        //$("#iptNombreArchivo").val("");
    }
    if (iAccion == 2) { //DOLARES
        $("#btnImportarDataSoles").hide();
        $("#btnImportarDataDolares").show();
        $("#btnDescargarDataDolares").hide();
        $("#btnVolverGenerarPagos").hide();
        $("#btnImportarDataSoles").prop("disabled", true);
        $("#btnImportarDataDolares").prop("disabled", false);
        $("#btnDescargarDataDolares").prop("disabled", true);
        $("#btnVolverGenerarPagos").prop("disabled", true);
        //$("#iptNombreArchivo").val("");
    }
    if (iAccion == 3) { //OCULTAR TODOS
        $("#btnImportarDataSoles").hide();
        $("#btnImportarDataDolares").hide();
        $("#btnDescargarDataDolares").hide();
        $("#btnVolverGenerarPagos").hide();
        $("#btnImportarDataSoles").prop("disabled", true);
        $("#btnImportarDataDolares").prop("disabled", true);
        $("#btnDescargarDataDolares").prop("disabled", true);
        $("#btnVolverGenerarPagos").prop("disabled", true);
        //$("#iptNombreArchivo").val("");
    }
    if (iAccion == 4) {
        $("#btnImportarDataSoles").hide();
        $("#btnImportarDataDolares").hide();
        $("#btnDescargarDataDolares").show();
        $("#btnVolverGenerarPagos").show();
        $("#btnImportarDataSoles").prop("disabled", true);
        $("#btnImportarDataDolares").prop("disabled", true);
        $("#btnDescargarDataDolares").prop("disabled", false);
        $("#btnVolverGenerarPagos").prop("disabled", false);
        $("#contenido-archivo").hide();
        $("#file-input").hide();
        $("#ipAdjuntarArchivos").hide();
        $("#iptNombreArchivo").val("");
        document.getElementById('contenido-archivo').innerHTML = "";
        document.getElementById('file-input').innerHTML = "";
    }

    fn_util_desbloquearPantalla();
}
function fn_SetearCampos(sNombresCompleto, sDescripcionAccion, iCantidadRegistros, sFechaProceso, iAccion)

{

    $("#txtNombreUsuario").val(sNombresCompleto);
    $("#txtMensaje").val(sDescripcionAccion);
    $("#txtCantidadRegistros").val(iCantidadRegistros);
    $("#txtFechaProceso").val(sFechaProceso);
    
    if (iAccion == 1) {
        $("#dvCampos").show();
        $("#btnExportaDataSoles").show();
        $("#btnExportaDataDolares").show();
        $("#btnGenerarData").hide();
        $("#btnExportaDataSoles").prop("disabled", false);
        $("#btnExportaDataDolares").prop("disabled", false);
        $("#btnGenerarData").prop("disabled", true);

        if (iCantidadRegistros > 0) {
            fn_util_alert("", "Se genero la data satisfactoriamente.", "S");
        } else {
            fn_util_alert("", "No hay data para generar.", "W");
        }
    }
    if (iAccion == 2)
    {
        $("#dvCampos").hide();
        $("#btnExportaDataSoles").hide();
        $("#btnExportaDataDolares").hide();
        $("#btnGenerarData").show();
        $("#btnExportaDataSoles").prop("disabled", true);
        $("#btnExportaDataDolares").prop("disabled", true);
        $("#btnGenerarData").prop("disabled", false);
        fn_util_alert("", "Se exporto la data satisfactoriamente.", "S");
    }

}