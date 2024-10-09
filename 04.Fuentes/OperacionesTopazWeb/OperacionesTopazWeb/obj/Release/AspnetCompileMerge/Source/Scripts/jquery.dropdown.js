/*
* Athor: Gustavo Cruzado 
* Date: 26/09/2017
*
*/
(function ($) {
    
    function timeoutSession(message) {
        Session = 1;
        $("body").append('<div id="dialog-message" title="Mensaje de Sistema" style="display:none;"><p>' +
        '<span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>' + message + '</p></div>');
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
                    this.close();
                }
            }
        });
    }



    $.fn['jqueryComboDependency'] = function (options) {

        
        var settings = $.extend({
            parent: null,
            ajaxOptions: {
                url: null,
                cache: null,
                data: null,
                type: 'GET',
                dataType: 'json'
            },
            ajaxResponse: {
                value: 'value',
                text: 'text'
            },
            insertEmptyValue: false,
            defaultText: 'Seleccione una opción'
        }, options);

  
    

        return this.each(function () {
            
            
            var parentElement = $.type(settings.parent) === "string" ? $(settings.parent) : settings.parent,
                selectObj = $(this), self, objChange = {
                    init: function () {
                        self = this;
                        this.bindEvents();
                    },
                    bindEvents: function () {
                        parentElement.on('change', this.getElementsObj);
                    },
                    getElementsObj: function () {
                        var value = $(settings.parent).val();
                        
                        var defaultValue = selectObj.find('option:first').text();
                        selectObj.empty();
                        selectObj.append("<option value></option>");
                        selectObj.find('option').text(defaultValue);
                        if (value) {
                            settings.ajaxOptions.data = { id: value };
                            selectObj.attr("disabled", 'disabled');
                            $.ajax(settings.ajaxOptions)
                                .done(function (response) {

                                    $("#cboDestinoPrestamo").empty();
                                    
                                    var obj = response;
                                    $.each(obj, function (index, optiondata) {                                        
                                        selectObj.append("<option value='" + optiondata.Value + "'>" + optiondata.Value +'- '+ optiondata.Text + "</option>");
                                    });
                                    selectObj.removeAttr("disabled");
                                    selectObj.change();
                                }).fail(function (response) {
                                   
                                    var message = "Se ha superado el tiempo límite de espera, vuelva a ingresar al sistema.";
                                    timeoutSession(message);
                                });
                        }
                    }
                };
            objChange.init();
            

        });
    };

}(jQuery));