var trobadoo = trobadoo || {};
trobadoo.com = trobadoo.com || {};

//Declare a class, with parameteres
trobadoo.com.contact =
(function ($) {
    var api = {
        options: {
            //variables comunes
            sendMailUrl: '/AjaxSendMail',
            $modal: $('#loadingModal'),
            $form: $('#formContact'),
            validationLiterals: [],
            thankyouLit:'',
            errorLit:''
        },
        init: function (options) {
            var base = this;
            base.options = $.extend({}, api.options, options);

            //api.loadValidationRules();

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
                    nAlert(api.options.thankyouLit);
                },
                error: function (err) {
                    //nAlert(err.status + ' ' + err.statusText);
                    $(api.options.$modal).modal('hide');
                    nAlert(api.options.errorLit);
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
            var validator = $('#formContact').validate({
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
                    inputContactMessage: {
                        required: true,
                        maxlength: 300
                    }
                },
                messages: {
                    inputContactName: {
                        required: api.options.validationLiterals[1],
                        maxlength: api.options.validationLiterals[2]
                    },
                    inputContactSurnames: {
                        required: api.options.validationLiterals[3],
                        maxlength: api.options.validationLiterals[4]
                    },
                    inputContactMail: {
                        required: api.options.validationLiterals[5],
                        email: api.options.validationLiterals[6],
                        maxlength: api.options.validationLiterals[7]
                    },
                    inputContactConfMail: {
                        required: api.options.validationLiterals[8],
                        email: api.options.validationLiterals[9],
                        maxlength: api.options.validationLiterals[10]
                    },
                    inputContactPhone: {
                        number: api.options.validationLiterals[11]
                    },
                    inputContactCell: {
                        number: api.options.validationLiterals[12]
                    },
                    inputContactPostCode: {
                        number: api.options.validationLiterals[13]
                    },
                    inputContactCityName: {
                        required: api.options.validationLiterals[14],
                        maxlength: api.options.validationLiterals[15]
                    },
                    inputContactMessage: {
                        required: api.options.validationLiterals[16],
                        maxlength: api.options.validationLiterals[17]
                    }
                }, errorElement: 'div',
                errorClass: 'errorDatos',
                errorPlacement: function (error, element) {
                    if ($('#formContact').find('.errorLine').length > 0) {
                        $('#formContact').find('.errorLine').remove();
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
                    api.sendAjaxMail();
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