(function () {

    fn_iniciaEventos();

})(jQuery);

function fn_iniciaEventos() {

    fn_CargarCondiciones();
}
 
function fn_CargarCondiciones() {
    
    fn_util_bloquearPantalla("Cargando");
  
    var oTable = $('#tblCondicionesCredito').dataTable({
        "bServerSide": true,
        "sAjaxSource": VG_RUTA_SERVIDOR + 'Configuracion/ListarCondicionesCreditosVencidos',
        "bProcessing": false,
        "sPaginationType": "full_numbers",
        "bFilter": false,
        "bSort": false,

        "bLengthChange": false,
        "fnDrawCallback": function (oSettings) {

            $("#tblCondicionesCredito_paginate").hide();
            fn_util_desbloquearPantalla();
        },
        "aoColumns": [
            {},
            { "data": "iCodigo", "sClass": "center" },
            { "data": "vCondicion", "sClass": "left" },
            { "data": "vDescripcion", "sClass": "left" },
            { "data": "sEstado", "sClass": "left" },

        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {

            //Variables
            var sEcho = aoData[0].value;
            var iNroPagina = parseFloat(fn_util_obtieneNroPagina(aoData[3].value, aoData[4].value, aoData[4].value)).toFixed();
            var iCantMostrar = aoData[4].value;


            var oCondicion = {};
            oCondicion.NumeroPagina = iNroPagina;
            oCondicion.NumeroRegistros = iCantMostrar;


            var params = { 'oCondicion': oCondicion };

            //Llamada JQuery-JSON
            oSettings.jqXHR = fn_util_AjaxWM_Obj(sSource, params, function (result) {


                fn_util_desbloquearPantalla();
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

                        var strHtml = "";
                        strHtml += fn_util_CrearInputGrilla("checkbox", "chkCondi", "chkCondi_" + rowIndex, row.iIdCondicion, "ValidarUnSoloCheck(this,'chkCondi');", "", "", "", "");
                        return strHtml;
                    },
                    "sWidth": "5px",
                    "sClass": "center",
                    "bSort": false,
                    "aTargets": [0]
                },

            ]
    });

}

function fn_VerDetalles() {

    var iCheck = $("input[name=chkCondi]:checked");

    var psDatosTabla;
    if (iCheck.length == 0) {
        fn_util_alert("", sNombreUsuario + ", debe elegir un registro para poder eliminar.", "W");
        return false;
    } else {
        psDatosTabla = iCheck[0].value;

    }
    $("#txtIdCondicion").val(psDatosTabla);

    $("#frmDetalleCondicion").submit();

}

function fn_CambioEstado() {

    var iCheck = $("input[name=chkCondi]:checked");

    var psDatosTabla;
    if (iCheck.length == 0) {
        fn_util_alert("", sNombreUsuario + ", debe elegir un registro para poder eliminar.", "W");
        return false;
    } else {
        psDatosTabla = iCheck[0].value;
    }


    var sParams = ["iIdCondicion", psDatosTabla];

 
    fn_util_AjaxWM(VG_RUTA_SERVIDOR + 'Configuracion/ObtenerCondicion', sParams, function (data) {

        fn_util_desbloquearPantalla();


        var oCondicion = data.oCondicion;
        var sEstado = "Activo";


        var sTexto = "";

        if (oCondicion.iIdCondicion > 0) {

            /************************************/
            if (oCondicion.iEstado == 1) {

                sEstado = "Inactivo";
            }

            var oCon = {};
            oCon.iAccion = 4;
            oCon.iIdCondicion = psDatosTabla;
            oCon.iCodigo = 0;
            oCon.vCondicion = '-';
            oCon.vDescripcion = '-';
            oCon.iEstado = 0;
            oCon.iTipo = 0;

            var params = { 'oCondicion': oCon };


            sTexto += "¿Desea cambiar el estado de la condición: <b>" + oCondicion.vCondicion + "</b>  a : <b>" + sEstado + "</b>?";
            var botones = [
                {
                    "text": "Sí",
                    "class": "btnGrabar",
                    "click": function () {
                        $(this).dialog("close");


                        fn_util_bloquearPantalla("Cargando");



                        fn_util_AjaxWM_Obj(VG_RUTA_SERVIDOR + 'Configuracion/MantenimientoCondicion', params, function (data) {

                            fn_util_desbloquearPantalla();


                            if (data.oNotificacion.iId > 0) {
                                fn_util_alert("", sNombreUsuario + "," + data.oNotificacion.sModulo, "S");
                            }
                            fn_Actualizar();


                        }, function (reponse) {
                            fn_util_desbloquearPantalla();
                            fn_util_alert("", sNombreUsuario + ". Ocurrió un problema al agregar el parámetro, vuélvalo a intentar o comuníquese con su administrador", "E");
                        });



                    }
                },
                {
                    "text": "No",
                    "class": "btnCancelar",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }];

            $('.dialog-text').html(sNombreUsuario + ", " + sTexto);

            $("#dialog-confirm").dialog({
                title: "Confirmación",
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                closeOnEscape: true,
                draggable: true,
                buttons: botones
            });

        }




    }, function (reponse) {
        fn_util_desbloquearPantalla();
        fn_util_alert("", sNombreUsuario + ". Ocurrió un problema al agregar el parámetro, vuélvalo a intentar o comuníquese con su administrador", "E");
    });




}

function fn_Actualizar() {

    var table = $('#tblCondiciones').DataTable();
    table.ajax.reload();
}