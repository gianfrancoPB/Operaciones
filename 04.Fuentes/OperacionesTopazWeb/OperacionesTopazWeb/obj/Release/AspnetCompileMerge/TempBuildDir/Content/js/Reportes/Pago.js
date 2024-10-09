var FechaInicio = 0;
var FechaFin = 0;
var rowsTotales = 0;
var iBusqueda = 0;

var oConfiguracionActual = {
    dateFormat: 'dd/mm/yy',
    monthNamesShort: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
    changeMonth: true,
    changeYear: true,
    constrainInput: true
}

$(document).ready(function () {

    $(function () {
        $("#txtFechaInicio").datepicker(oConfiguracionActual);
        $("#txtFechaFin").datepicker(oConfiguracionActual);
    });
 
})

function fn_Limpiar() {
    location.reload();
}

function fn_Exportar() {
 
    var sMsj = " reporte gráfico";

    var bExito = true;
    var iTipoResultado = 1;

    if (iBusqueda === 0) {
        fn_util_alert("", "No puede exportar sin realizar alguna búsqueda.", "W");
        bExito = false;
   
    } else if (rowsTotales.length === 0) {
        fn_util_alert("", "No puede exportar porque la búsqueda no dio resultados.", "W");
        bExito = false;
    }
 
 
    if (bExito) {
        fn_util_bloquearPantalla("Descargando Reporte Excel..."); 
        fn_util_AjaxWM(
            VG_RUTA_SERVIDOR + 'Reportes/Exportar',
            { /* Aquí van los parámetros que necesitas enviar al servidor, si los hay */ },
            function (result) {
                // La función de éxito se llama cuando la llamada Ajax es exitosa.
                if (iTipoResultado === 1) {
                    window.location = VG_RUTA_SERVIDOR + 'Reportes/DownloadReportePagos?piFlag=1';
                } else if (iTipoResultado === 2) {
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
                fn_util_desbloquearPantalla();
            },
            function () {
                // La función de error se llama cuando la llamada Ajax falla.
                fn_util_desbloquearPantalla();
                fn_util_alert("", "Ocurrió un problema al descargar el" + sMsj + ", vuélvalo a intentar o comuníquese con su administrador.", "E", 10);
            }
        );
    }
}
 
var oTable; // Variable para almacenar la instancia del objeto DataTable
 
function fn_Buscar(idCuadro) {
    var idCuadro = '1';

    var FechaInicio = $("#txtFechaInicio").val();
    var FechaFin = $("#txtFechaFin").val();

    if (!FechaInicio) {
        txtFechaInicio.focus();
        // Crear una nueva fila con el mensaje de error
        fn_util_alert("", sNombreUsuario + ", la fecha de inicio es requerida", "W");
        return;
    }
    if (!FechaFin) {
        txtFechaFin.focus();
        // Crear una nueva fila con el mensaje de error
        fn_util_alert("", sNombreUsuario + ", la fecha de Fin es requerida", "W");
        return;
    }

    var oTable;

    if ($.fn.DataTable.isDataTable('#tblReportePagos')) {
        // Si la tabla ya existe, destruir la instancia actual y luego crearla nuevamente
        oTable = $('#tblReportePagos').DataTable();
        oTable.destroy();
    }

    // Crear la tabla DataTable
    oTable = $('#tblReportePagos').DataTable({
        // DataTable configurations...
        "bServerSide": false,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Reportes/ListarPago",
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": false,
        "bSort": false,
        "bLengthChange": false,
        "bPaginate": false,
        "bInfo": false,      
            "aoColumns": [
                { "data": "NRO_LINEA", "sClass": "center" }, // Show the value of NRO_LINEA in the first column
                { "data": "sDOI", "sClass": "center" },
                { "data": "vNombreCliente", "sClass": "center" },
                { "data": "vMoneda", "sClass": "center" },
                { "data": "vDescripcion", "sClass": "center" },
                { "data": "nNTransaccion", "sClass": "center" },
                { "data": "dFechaVencimiento", "sClass": "center" },
                { "data": "vAsiento_Contable", "sClass": "center" },
                { "data": "vNroTicket", "sClass": "center" },
                { "data": "sNumero_Credito", "sClass": "center" },
                { "data": "sRubro_Cuenta", "sClass": "center" },

            ],
     
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            var sParams = { "idCuadro": 1, "iiActivo": 1, "FechaInicio": FechaInicio, "FechaFin": FechaFin };
            // Mostrar mensaje de "Cargando Lista Reporte..."
            fn_util_bloquearPantalla("Cargando Lista Reporte...");
            oSettings.jqXHR = fn_util_AjaxWM_Obj(sSource, sParams, function (result) {
                fnCallback(result);
                iBusqueda = 1;
                rowsTotales = result.aaData;
                // Ocultar mensaje de "Cargando Lista Reporte..." cuando se complete la carga de la tabla
                fn_util_desbloquearPantalla();
            }, function (error) {
                fn_util_desbloquearPantalla();
                fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al listar las tablas, vuélvalo a intentar o comuníquese con su administrador.", "E");
            });
        }
    });
}

  
 
 