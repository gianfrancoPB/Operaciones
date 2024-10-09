var Session = 0;


//****************************************************************
// Funcion		:: 	fn_util_AjaxSyncWM
// Descripción	::	Realiza llamada a WebMethod (SINCRONO)
//****************************************************************
function fn_util_AjaxSyncWM(pstrMetodo, paramArray, successFn, errorFn) {
    //Arma Parametros
    var paramList = '';
    if (paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
        }
    }
    paramList = '{' + paramList + '}';
    if (Session == 0) {
        //Ejecuta Ajax
        $.ajax({
            type: "POST",
            url: pstrMetodo,
            contentType: "application/json; charset=utf-8",
            data: paramList,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.iTipoResultado == -5) {
                    Session = 1;
                    $("body").append('<div id="dialog-message" title="Mensaje de Sistema" style="display:none;"><p>' +
                    '<span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>' + data.message + '</p></div>');
                    $("#dialog-message").dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        dialogClass: 'no-close',
                        closeOnEscape : false,
                        open: function (event, ui) {
                            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                            $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
                        },
                        buttons: {
                            Cerrar: function () {
                                window.close();
                            }
                        }
                    });
                } else {
                    successFn(data);
                }
            },
            error: errorFn
        });
    } else {
        $("#dialog-message").dialog({
            modal: true,
            draggable: false,
            resizable: false,
            dialogClass: 'no-close',
            closeOnEscape: false,
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
            },
            buttons: {
                Cerrar: function () {
                    window.close();
                }
            }
        });
    }
}


//****************************************************************
// Funcion		:: 	fn_util_AjaxWM
// Descripción	::	Realiza llamada a WebMethod (ASINCRONO)
//****************************************************************
function fn_util_AjaxWM(pstrMetodo, paramArray, successFn, errorFn) {
    //Arma Parametros
    var paramList = '';
    if (paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
        }
    }
    paramList = '{' + paramList + '}';

    if (Session == 0) {
        //Ejecuta Ajax
        $.ajax({
            type: "POST",
            url: pstrMetodo,
            contentType: "application/json; charset=utf-8",
            data: paramList,
            dataType: "json",
            async: true,
            success: function (data) {
                if (data.iTipoResultado == -5) {
                    Session = 1;
                    $("body").append('<div id="dialog-message" title="Mensaje de Sistema" style="display:none;"><p>' +
                    '<span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>' + data.message + '</p></div>');
                    $("#dialog-message").dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        dialogClass: 'no-close',
                        open: function (event, ui) {
                            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                            $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
                        },
                        buttons: {
                            Cerrar: function () {
                                window.close();
                            }
                        }
                    });
                } else {
                    successFn(data);
                }
            },
            error: errorFn
        });
    } else {
        $("#dialog-message").dialog({
            modal: true,
            draggable: false,
            resizable: false,
            dialogClass: 'no-close',
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
            },
            buttons: {
                Cerrar: function () {
                    window.close();
                }
            }
        });
    }
}


//****************************************************************
// Funcion		:: 	fn_util_AjaxSyncWM_Obj
// Descripción	::	Realiza llamada a WebMethod (SINCRONO)
//                  Se envia el parametro como OBJETO
//****************************************************************
function fn_util_AjaxSyncWM_Obj(pstrMetodo, pParam, successFn, errorFn) {

    var oParametros = JSON.stringify(pParam);

    //Arma Parametros
    var parametro = '';
    if (oParametros == '') {
        parametro = "{}";
    } else {
        parametro = oParametros;
    }

    if (Session == 0) {
        //Ejecuta Ajax
        $.ajax({
            type: "POST",
            url: pstrMetodo,
            contentType: "application/json; charset=utf-8",
            data: parametro,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.iTipoResultado == -5) {
                    Session = 1;
                    $("body").append('<div id="dialog-message" title="Mensaje de Sistema" style="display:none;"><p>' +
                    '<span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>' + data.message + '</p></div>');
                    $("#dialog-message").dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        dialogClass: 'no-close',
                        open: function (event, ui) {
                            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                            $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
                        },
                        buttons: {
                            Cerrar: function () {
                                window.close();
                            }
                        }
                    });
                } else {
                    successFn(data);
                }
            },
            error: errorFn
        });
    } else {
        $("#dialog-message").dialog({
            modal: true,
            draggable: false,
            resizable: false,
            dialogClass: 'no-close',
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
            },
            buttons: {
                Cerrar: function () {
                    window.close();
                }
            }
        });
    }
}


//****************************************************************
// Funcion		:: 	fn_util_AjaxWM_Obj
// Descripción	::	Realiza llamada a WebMethod (ASINCRONO) 
//                  Se envia el parametro como OBJETO
//****************************************************************
function fn_util_AjaxWM_Obj(pstrMetodo, pParam, successFn, errorFn) {

    var oParametros = JSON.stringify(pParam);

    //Arma Parametros
    var parametro = '';
    if (oParametros == '') {
        parametro = "{}";
    } else {
        parametro = oParametros;
    }

    if (Session == 0) {
        //Ejecuta Ajax
        $.ajax({
            type: "POST",
            url: pstrMetodo,
            contentType: "application/json; charset=utf-8",
            data: parametro,
            dataType: "json",
            async: true,
            success: function (data) {
                if (data.iTipoResultado == -5) {
                    Session = 1;
                    $("body").append('<div id="dialog-message" title="Mensaje de Sistema" style="display:none;"><p>' +
                    '<span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>' + data.message + '</p></div>');
                    $("#dialog-message").dialog({
                        modal: true,
                        draggable: false,
                        resizable: false,
                        dialogClass: 'no-close',
                        open: function (event, ui) {
                            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                            $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
                        },
                        buttons: {
                            Cerrar: function () {
                                window.close();
                            }
                        }
                    });
                } else {
                    successFn(data);
                }
            },
            error: errorFn
        });
    } else {
        $("#dialog-message").dialog({
            modal: true,
            draggable: false,
            resizable: false,
            dialogClass: 'no-close',
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                $(this).closest('.ui-dialog').find('.ui-dialog-buttonset').css("text-align", "center").css("float", "none");
            },
            buttons: {
                Cerrar: function () {
                    window.close();
                }
            }
        });
    }
}


//****************************************************************
// Funcion		:: 	fn_util_AjaxSyncTypeHTML
// Descripción	::	Realiza llamada a WebMethod (SINCRONO)
//****************************************************************
function fn_util_AjaxSyncTypeHTML(pstrMetodo, paramArray, successFn, errorFn) {
    //Arma Parametros
    var paramList = '';
    if (paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
        }
    }
    paramList = '{' + paramList + '}';

    $.ajax({
        type: "POST",
        url: pstrMetodo,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "html",
        async: false,
        success: successFn,
        error: errorFn
    });
}

//****************************************************************
// Funcion		:: 	fn_util_AjaxTypeHTML
// Descripción	::	Realiza llamada a WebMethod (ASINCRONO)
//****************************************************************
function fn_util_AjaxTypeHTML(pstrMetodo, paramArray, successFn, errorFn) {
    //Arma Parametros
    var paramList = '';
    if (paramArray.length > 0) {
        for (var i = 0; i < paramArray.length; i += 2) {
            if (paramList.length > 0) paramList += ',';
            paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
        }
    }
    paramList = '{' + paramList + '}';

    $.ajax({
        type: "POST",
        url: pstrMetodo,
        contentType: "application/json; charset=utf-8",
        data: paramList,
        dataType: "html",
        async: true,
        success: successFn,
        error: errorFn
    });

}

//****************************************************************
// Funcion		:: 	fn_util_AjaxSyncTypeHTML_Obj
// Descripción	::	Realiza llamada a WebMethod (SINCRONO)
//                  Se envia el parametro como OBJETO
//****************************************************************
function fn_util_AjaxSyncTypeHTML_Obj(pstrMetodo, pParam, successFn, errorFn) {

    var oParametros = JSON.stringify(pParam);

    //Arma Parametros
    var parametro = '';
    if (oParametros == '') {
        parametro = "{}";
    } else {
        parametro = oParametros;
    }

    //Ejecuta Ajax
    $.ajax({
        type: "POST",
        url: pstrMetodo,
        contentType: "application/json; charset=utf-8",
        data: parametro,
        dataType: "html",
        async: false,
        success: successFn,
        error: errorFn
    });
}

//****************************************************************
// Funcion		:: 	fn_util_AjaxTypeHTML
// Descripción	::	Realiza llamada a WebMethod (ASINCRONO) 
//                  Se envia el parametro como OBJETO
//****************************************************************
function fn_util_AjaxTypeHTML_Obj(pstrMetodo, pParam, successFn, errorFn) {

    var oParametros = JSON.stringify(pParam);

    //Arma Parametros
    var parametro = '';
    if (oParametros == '') {
        parametro = "{}";
    } else {
        parametro = oParametros;
    }

    //Ejecuta Ajax
    $.ajax({
        type: "POST",
        url: pstrMetodo,
        contentType: "application/json; charset=utf-8",
        data: parametro,
        dataType: "html",
        async: true,
        success: successFn,
        error: errorFn
    });
}
