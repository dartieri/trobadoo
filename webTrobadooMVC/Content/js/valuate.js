var trobadoo = trobadoo || {};
trobadoo.com = trobadoo.com || {};

//Declare a class, with parameteres
trobadoo.com.valuate =
(function ($) {
    var api = {
        options: {
            //variables comunes
            sendMailUrl: '/AjaxSendMail',
            $modal: $('#loadingModal'),
            $form: $('#formValuate'),
            photoToken: '',
            $photosInfo: {},
            totalPhotos: 0,
            totalUploaded: 0,
            validationLiterals: [],
            thankyouLit:'',
            errorLit:''
        },
        init: function (options) {
            var base = this;
            base.options = $.extend({}, api.options, options);

            api.initFileUpload();

            //api.loadValidationRules();

        },
        initFileUpload: function () {
            api.options.$form.fileupload();

            api.options.$form.fileupload('option', {
                url: '/AjaxFileHandler/UploadFiles',
                maxFileSize: 1000000,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                formData: { photoToken: api.options.photoToken },
                fileTypes: /^image\/(gif|jpeg|png)$/,
                dataType: 'json',
                done: function (e, data) {
                    console.log('upload done');
                    api.options.totalUploaded++;
                }
            });
        },
        sendAjaxMail: function () {
            $.ajax({
                url: api.options.sendMailUrl,
                type: 'POST',
                //contentType: 'application/json',
                //dataType: 'json',
                data: api.options.$form.serialize(),
                beforeSend: function () {
                    $(api.options.$modal).modal('show');
                },
                success: function (data) {
                    $(api.options.$modal).modal('hide');
                    nAlert('Gracias por contactar con nosotros\n. Hemos recibido tu solicitud de valoración\n. Te contestaremos en breve.');
                },
                error: function (err) {
                    //alert(err.status + ' ' + err.statusText);
                    $(api.options.$modal).modal('hide');
                    nAlert('Ha ocurrido un error al enviar el mail\n. Por favor, intentalo mas tarde o manda un mail a infor@trobadoo.com.');
                }
            });
        },
        loadDatePicker: function (dateMin) {
            //Cuando no es cross selling, por defecto cargamos 2 meses
            if (!api.options.isCross) {
                api.options.numMaxMonths = 2;
                api.options.numMonths = 2;
            }
            $('#datesDeparture').datepicker({
                container: '#datesDeparture',
                adaptFullContainer: true,
                showOn: 'both',
                minDate: api.options.isCross ? api.options.dateMinPlaceCross : api.options.dateMinPlace,
                maxDate: api.options.isCross ? api.options.dateMaxPlaceCross : api.options.dateMaxPlace,
                altFormat: 'dd/mm/yy',
                altField: '#dateDeparture',
                dateFormat: 'D, d MM, yy',
                numberOfMonths: api.options.numMonths,
                autoSize: true,
                beforeShowDay: api.unavailable,
                onSelect: function () {
                    $('#dateSelected').html($('#dateDeparture').val());
                    api.loadActivities($('#contentAvailabilityActivities').data('placecode'), $('#contentAvailabilityActivities').data('activitycode'), 0, 1);
                }
            });
            if (typeof dateMin !== 'undefined')
                $('#datesDeparture').datepicker('setDate', dateMin);
        },
        loadValidationRules: function () {
            var validator = $('#formValuate').validate({
                rules: {
                    inputContactName: {
                        required: true,
                        maxlength: 50
                    },
                    inputContactSurnames: {
                        required: true,
                        maxlength: 50
                    },
                    inputContactMail: {
                        required: true,
                        email: true,
                        maxlength: 50
                    },
                    inputContactConfMail: {
                        required: true,
                        email: true,
                        maxlength: 50
                    },
                    inputContactPhone: {
                        required: false,
                        number: true
                    },
                    inputContactCell: {
                        required: false,
                        number: true
                    },
                    inputContactAddress: {
                        required: false
                    },
                    inputContactPostCode: {
                        required: false,
                        number: true
                    },
                    inputContactCityName: {
                        required: true,
                        maxlength: 50
                    },
                    inputContactDesc: {
                        required: true,
                        maxlength: 100
                    },
                    inputContactBuyYear: {
                        required: true
                    },
                    inputContactBuyPrice: {
                        required: true,
                        number: true
                    },
                    inputContactMessage: {
                        required: true,
                        maxlength: 500
                    }
                },
                messages: {
                    inputContactName: {
                        required: 'Tiene que indicar su nombre, máximo 50 caracteres',
                        maxlength: 'Tiene que indicar su nombre, máximo 50 caracteres'
                    },
                    inputContactSurnames: {
                        required: 'Tiene que indicar sus apellidos',
                        maxlength: 'Tiene que indicar sus apellidos, máximo 50 caracteres'
                    },
                    inputContactMail: {
                        required: 'Tiene que indicar su dirección de correo',
                        email: 'Tiene que indicar una dirección de correo válida',
                        maxlength: 'Tiene que indicar su dirección de correo, máximo 50 caracteres'
                    },
                    inputContactConfMail: {
                        required: 'Tiene que confirmar su dirección de correo',
                        email: 'Tiene que indicar una dirección de correo válida',
                        maxlength: 'Tiene que confirmar su dirección de correo'
                    },
                    inputContactPhone: {
                        number: 'Tiene que indicar un número de tel válido'
                    },
                    inputContactCell: {
                        number: 'Tiene que indicar un número de móvil válido'
                    },
                    inputContactPostCode: {
                        number: 'Tiene que indicar un código postal válido'
                    },
                    inputContactCityName: {
                        required: 'Tiene que indicar una ciudad',
                        maxlength: 'Tiene que indicar una ciudad, máximo 50 caracteres'
                    },
                    inputContactDesc: {
                        required: 'Tiene que indicar una descripción',
                        maxlength: 'Tiene que indicar una descripción, máximo 100 caracteres'
                    },
                    inputContactBuyYear: {
                        required: 'Tiene que seleccionar el año  de compra (aprox.)'
                    },
                    inputContactBuyPrice: {
                        required: 'Tiene que indicar un precio orientativo de compra',
                        number: 'Tiene que indicar un precio válido (por ej. 25)'
                    },
                    inputContactMessage: {
                        required: 'Tiene que escribir su mensaje',
                        maxlength: 'Tiene que escribir su mensaje, máximo 300 caracteres'
                    }
                }, errorElement: 'div',
                errorClass: 'errorDatos',
                errorPlacement: function (error, element) {
                    if ($('#formValuate').find('.errorLine').length > 0) {
                        $('#formValuate').find('.errorLine').remove();
                    }
                    $(element).notify(
                    $(error).html(), {
                        // whether to hide the notification on click
                        //clickToHide: true,
                        // whether to auto-hide the notification
                        //autoHide: true,
                        // if autoHide, hide after milliseconds
                        //autoHideDelay: 5000,
                        // show the arrow pointing at the element
                        arrowShow: true,
                        // arrow size in pixels
                        arrowSize: 5,
                        // default positions
                        elementPosition: 'bottom center',
                        //globalPosition: 'middle center',
                        // default style
                        style: 'bootstrap',
                        // default class (string or [string])
                        className: 'error',
                        // show animation
                        showAnimation: 'slideDown',
                        // show animation duration
                        showDuration: 400,
                        // hide animation
                        hideAnimation: 'slideUp',
                        // hide animation duration
                        hideDuration: 200,
                        // padding between element and notification
                        gap: 2
                    });
                    //$(element).parent().append('<tr class="errorLine"><td colspan="3"><div class="contErrorDatos">' + $(error).html() + '</div></td></tr>');
                },
                highlight: function (element, errorClass) {
                    $(element).addClass('error');
                },
                unhighlight: function (element, errorClass) {
                    $(element).removeClass('error');
                    $(element).trigger('notify-hide');
                },
                // set this class to error-labels to indicate valid fields
                success: function (div, element) {
                    $(element).addClass('valid');
                },
                //onkeyup: true,
                // specifying a submitHandler prevents the default submit, good for the demo 
                submitHandler: function (form) {
                    console.log('upload photos');
                    api.options.totalPhotos = $('.template-upload .start').length;
                    $('.template-upload .start').trigger('click');
                    //return false;
                    (function wait() {
                        if (api.options.totalUploaded == api.options.totalPhotos) {
                            //console.log('done: form submit');
                            api.sendAjaxMail();
                        } else {
                            //console.log('wait');
                            setTimeout(wait, 100);
                        }
                    })();
                }
            });
        }
    };
    return api;
})(jQuery);


// IIFE - Immediately Invoked Function Expression
(function ($, window, document) {

    // The $ is now locally scoped
    // Listen for the jQuery ready event on the document
    $(function () {
        //Document Ready Actions
    });
    // The rest of the code goes here!
} (window.jQuery, window, document));