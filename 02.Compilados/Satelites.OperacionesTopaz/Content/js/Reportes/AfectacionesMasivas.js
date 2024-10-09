var iRegistros = 0;
var oConfiguracionActual = {
    dateFormat: 'dd/mm/yy',
    monthNamesShort: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
    changeMonth: true,
    changeYear: true,
    constrainInput: true
}

$(document).ready(function () {
    fn_Inicializar();
})

function fn_Limpiar() {
    location.reload();
}
function fn_Inicializar() {
    $("#txtFechaInicio").datepicker(oConfiguracionActual);
    $("#txtFechaFin").datepicker(oConfiguracionActual);
}
function fn_ValidarBusqueda() {

    var sFechaDesde = $("#txtFechaInicio").val();
    var sFechaHasta = $("#txtFechaFin").val();

    if (!sFechaDesde) {
        txtFechaInicio.focus();
        fn_util_alert("", sNombreUsuario + ", Debe ingresar la fecha inicial", "W");
        return;
    }
    if (!sFechaHasta) {
        txtFechaFin.focus();
        fn_util_alert("", sNombreUsuario + ", Debe ingresar la fecha final", "W");
        return;
    }

    fn_BuscarAfectasiones(sFechaDesde, sFechaHasta);
}
function fn_BuscarAfectasiones(sFechaDesde, sFechaHasta) {

    if ($.fn.DataTable.isDataTable('#tblAfectasiones')) {
        $("#tblAfectasiones").DataTable().destroy();
    }

    var oTable = $('#tblAfectasiones').dataTable({
        "bServerSide": true,
        "sAjaxSource": VG_RUTA_SERVIDOR + "Reportes/ReporteAfectacionesMasivasCabecera",
        "bProcessing": true,
        "sPaginationType": "full_numbers",
        "bFilter": true,
        "bSort": false,
        "sFilter": "dataTables_filter",
        "buttons": ['copy', 'excel', 'pdf', 'colvis'],
        "bLengthChange": false,
        "fnDrawCallback": function (oSettings) {
            fn_util_desbloquearPantalla();
        },
        "aoColumns": [

            { "sClass": "center", "data": "RowNumber" },
            { "sClass": "center", "data": "nNumeroTicket" },
            { "sClass": "center", "data": "sCargoFecha" },
            { "sClass": "center", "data": "sCargoHora" },
            { "sClass": "left", "data": "sNombreProceso" },
            { "sClass": "center", "data": "sCodigoUsuario" },
            { "sClass": "center", "data": "sNombreUsuario" },
            { "sClass": "center", "data": "iNumeroRegistros" },
            { "sClass": "center", "data": "iNumeroRegistrosCorrectos" },
            { "sClass": "center", "data": "iNumeroRegistrosFallidos" },

        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            //Variables
            var sEcho = aoData[0].value;
            var iNroPagina = parseFloat(fn_util_obtieneNroPagina(aoData[3].value, aoData[4].value)).toFixed();
            var iCantMostrar = aoData[4].value;
            iRegistros = aoData.length;
            //Parametros
            var sParams = {
                "sFechaInicio": sFechaDesde,
                "sFechaFin": sFechaHasta,
                "NumeroPagina": iNroPagina,
                "NumeroRegistros": iCantMostrar
            };
            //Llamada JQuery-JSON
            oSettings.jqXHR = fn_util_AjaxWM_Obj(
                sSource,
                sParams,
                function (result) {

                    iCantidadRegistros = result.iTotalRecords;
                    fnCallback(result);
                }
                , function (error) {
                    fn_util_desbloquearPantalla();
                    fn_util_alert("", sNombreUsuario + ", ocurrió un inconveniente al listar las tablas, vuélvalo a intentar o comuníquese con su administrador.", "E");
                });
        },
        "aoColumnDefs":
            [
            ]
    });

}
function fn_RestablecerFechaInicio() {
    if ($("#txtFechaInicio").val() != '') {
        var sFecha = $("#txtFechaInicio").val().split('/');
        var sDia = parseInt(sFecha[0]);// + 1;
        var sMes = sFecha[1] - 1;
        var sYear = sFecha[2];
        $("#txtFechaFin").datepicker('option', { minDate: new Date(sYear, sMes, sDia), dateFormat: 'dd/mm/yy' });
    }
}

function fn_Exportar() {

    var bExito = true;
    var iTipoResultado = 1;

    var sFechaDesde = $("#txtFechaInicio").val();
    var sFechaHasta = $("#txtFechaFin").val();



    if (iRegistros == 0) {
        fn_util_alert("", "No puede exportar porque la búsqueda no dio resultados.", "W");
        bExito = false;
    }
    if (bExito) {
        fn_util_bloquearPantalla("Descargando Reporte Excel...");

        var sParams = {
            "sFechaInicio": sFechaDesde,
            "sFechaFin": sFechaHasta
        };

        fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + "Reportes/ExportarAfectacionesMasivas", sParams,
            function (oResultado) {
                fn_util_desbloquearPantalla();

                if (iTipoResultado === 1) {
                    window.location = VG_RUTA_SERVIDOR + 'Reportes/DownloadReporteAfectacionesMasivas?piFlag=1';
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
            },
            function () {
                // La función de error se llama cuando la llamada Ajax falla.
                fn_util_desbloquearPantalla();
                fn_util_alert("", "Ocurrió un problema al descargar el reporte gráfico, vuélvalo a intentar o comuníquese con su administrador.", "E", 10);
            }
        );
    }
}
