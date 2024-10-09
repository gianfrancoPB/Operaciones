(function() {
    fn_IniciaEventos();
})(jQuery);

function fn_IniciaEventos() {
    fn_CargarCombo(1);
    fn_CargarTablaParametros();
}

function fn_AgregarParametro() {

    var iIdParametro = $("#cboParametros").val();

    if (iIdParametro == '0' || iIdParametro == '' || iIdParametro == undefined) {
        fn_util_alert("", sNombreUsuario + ". Selecione un parámetro válido", "W");
        return false;
    }
     
    var EnCondicionDetalle = {};

    EnCondicionDetalle.iAccion = 1;
    EnCondicionDetalle.iIdDetalle = 0;
    EnCondicionDetalle.iIdCondicion = $("#txtiIdCondicion").val();
    EnCondicionDetalle.iIdParametro = iIdParametro;
    EnCondicionDetalle.vDescripcion = $("#txtSistema").val();

    fn_util_bloquearPantalla("Cargando");

    var sParams = {
        'oDetalle': EnCondicionDetalle
    };


    fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + 'Configuracion/MantenimientoDetalle', sParams, function (data) {

        fn_util_desbloquearPantalla();


        if (data.oResult == 1) {
            fn_CargarCombo(1);
            fn_ActualizarTabla();
            fn_util_alert("", sNombreUsuario + ". Se agregó el nuevo parámetro", "S");
        } else {
            fn_util_alert("", sNombreUsuario + ". No se agregó el nuevo parámetro", "W");
        }


    }, function (reponse) {
        fn_util_desbloquearPantalla();
        fn_util_alert("", sNombreUsuario + ". Ocurrió un problema al agregar el parámetro, vuélvalo a intentar o comuníquese con su administrador", "E");
    });




}

function fn_CargarCombo($iAccion) {

    var $iIdCondicion = $("#txtiIdCondicion").val();
    

    var arrParam = ["iIdAccion", $iAccion, "iIdCondicion", $iIdCondicion];

    fn_util_AjaxWM(VG_RUTA_SERVIDOR + "Configuracion/CondicionClientes", arrParam, function (data) {
            
            var html = "<option value='0'>Seleccione un parámetro</option>";

            $("#cboParametros").empty();

            $.each(data.aaData, function (key, oSectorista) {
                html += '<option value=' + oSectorista.iIdParametro + '>' + oSectorista.vMotivo + '</option>';
            });

            $("#cboParametros").append(html);
            $("#cboParametros").val("0");



            fn_util_desbloquearPantalla();
        },
        function (data) {
            fn_util_desbloquearPantalla();
            fn_util_alert("", sNombreUsuario + ", ocurrió un problema al listar los parámetros, vuélvalo a intentar o comuníquese con su administrador.", "E");
        }
    );
    
}

function fn_Volver() {
  
    $("#frmVolverBandeja").submit();
}


function fn_CargarTablaParametros() {

    fn_util_bloquearPantalla("Cargando");

  

    var oTable = $('#tblParametros').dataTable({
        "bServerSide": true,
        "sAjaxSource": VG_RUTA_SERVIDOR + 'Configuracion/CondicionClientes',
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": false,
        "bSort": false,
        "bLengthChange": false,
        "bInfo": false,
        "paging": false,
        "fnDrawCallback": function (oSettings) {
            fn_util_desbloquearPantalla();
        },
        "aoColumns": [
                        {},
                        { "data": "iIdParametro", "sClass": "center", "sWidth": "5%" },
                        { "data": "vMotivo", "sClass": "left", "sWidth": "5%" },
                        {}
        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {

            //Variables

            var sEcho = aoData[0].value;
            var iNroPagina = parseFloat(fn_util_obtieneNroPagina(aoData[3].value, aoData[4].value)).toFixed();
            var iCantMostrar = aoData[4].value;

            var $iIdCondicion = $("#txtiIdCondicion").val();

            

            var param = ["iIdAccion", 2, "iIdCondicion", $iIdCondicion];

            
            oSettings.jqXHR = fn_util_AjaxWM(sSource, param, function (result) {
                    
                fnCallback(result);
            }
            , function (error) {
                fn_util_desbloquearPantalla();
                fn_util_alert("", "Ocurrió un inconveniente obteniendo la información, vuélvalo a intentar o comuníquese con su administrador.", "E");
            });

        },
        "aoColumnDefs":
        [
            {
                "mRender": function (data, type, row) {

                    var rowIndex = oTable.fnGetData().length - 1;
                    var strHtml = fn_util_CrearInputGrilla("checkbox", "chkAmpliaciones", "chkAmpliacion_" + rowIndex, row.iIdDetalle, "ValidarUnSoloCheck(this,'chkAmpliaciones');", "", "", "", "", "estado=" + row.siCodigoEstado);
                    return strHtml;
                },
                "sWidth": "5px",
                "sClass": "center",
                "bSort": false,
                "aTargets": [0]
            },
            {
                "mRender": function (data, type, row) {
                    var strHtml = "<input type='button' value='Eliminar' class='css-boton css-boton-xs css-boton-rojo' onclick='javascript: fn_EliminarParametro(" + row.iIdDetalle + ");' />";
                    return strHtml;
                },
                "bSort": false,
                "sClass": "right",
                "width": "5px",
                "aTargets": [3]
                
            }
        ]
    });

}

function fn_ActualizarTabla() {
    
    var table = $('#tblParametros').DataTable();
    table.ajax.reload();
}

