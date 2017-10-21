"use strict";

var com = com || {};
com.trobadoo = com.trobadoo || {};
com.trobadoo.Trobadoo = com.trobadoo.Trobadoo || {};

// Declare a class, with parameteres
com.trobadoo.Trobadoo = (function ($) {
    var api = {
        /*options: {
        },*/
        // Funcion de inicializacion
        init: function (/*options*/) {
            //start menu
            $(".megamenu").megamenu();

            //Image Lazy load
            //$("img").lazyload({ placeholder: "/Content/images/common/loading.gif", threshold: 100 });

            api.configureInputs();

            api.initElements();
        },
        configureInputs: function () {
            $('#globalSearch').on('focus', function () {
                $(this).val('');
            });
            $('#globalSearch').on('keypress', function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    $(this).val().parent('form').submit();
                    return false;
                }
            });
        },
        initElements: function () {

            $("#slider").responsiveSlides({
                auto: true,
                speed: 500,
                namespace: "callbacks",
                pager: true
            });
        },
        nAlert: function (text, layout) {
            if (typeof layout == -1) {
                layout = 'center';
            }
            /*var n = noty({
            text: text,
            type: 'alert',
            dismissQueue: true,
            layout: layout,
            theme: 'defaultTheme'
            });
            console.log('html: ' + n.options.id);
            */
            alert(text);
        },
        showLoadingModalWithCallback: function (callback, title, message) {
            if (callback != undefined) {
                callback();
                var t = setTimeout(function () {
                    api.showLoadingModal(title, message);
                }, 500);
            }

        },
        showLoadingModal: function (title, message) {
            if (typeof title != 'undefined') {
                $("#loadingModalTitle").html(title);
            } else {
                $("#loadingModalTitle").html(api.options.defaultLoadingTitle);
            }
            if (typeof message != 'undefined') {
                $("#loadingModalMessage").html(message);
            } else {
                $("#loadingModalMessage").html(api.options.defaultLoadingMessage);
            }
            /*
            * $('#loadingModal').modal({ backdrop: 'static', keyboard:false });
            * $('#loadingModal').modal('show');
            */
            if (title == "searcher") {
                $('#searcherModal').removeClass('hidden');
            } else {
                $('#loadingModal').removeClass('hidden');
            }
            //$('#loadingModal').removeClass('hidden');
        },
        hideLoadingModal: function () {
            // $('#loadingModal').modal('hide');
            $('#loadingModal').addClass('hidden');
            $('.boton_rojo_transicion').addClass('hidden');
            $('#searcherModal').addClass('hidden');

        },
        sameWidth: function (group) {
            var minWidth = 0;
            group.css({
                "min-width": ''
            });
            for (var i = 0, l = group.length; i < l; i++) {
                var currentElement = group.eq(i).width();
                if (currentElement > minWidth) {
                    minWidth = currentElement;
                }
            }
            group.css({
                "min-width": minWidth
            });
        },
        sameHeight: function (group) {
            var minHeight = 0;
            group.css({
                "min-height": ''
            });
            for (var i = 0, l = group.length; i < l; i++) {
                var currentElement = group.eq(i).height();
                if (currentElement > minHeight) {
                    minHeight = currentElement;
                }
            }
            group.css({
                "min-height": minHeight
            });
        },
        scrollToAnchor: function ($aTag, $container) {
            var topPosition = $aTag.offset().top;

            var $element = $('html,body');
            if ($container && $container.length) {
                topPosition -= $aTag.offsetParent().offset().top;
                $element = $container;
            }
            $element.animate({ scrollTop: topPosition }, 'slow');
        },
        endsWith: function (origin, target) {
            return origin.slice(target.length * -1) === target;
        },
        loadCSS: function (url, id, callback) {
            if (url !== '') {
                var $cssFile = $('#cssFile_' + id);
                if ($cssFile.length > 0) {
                    // Forzamos a cargar los CSS otra vez
                    $cssFile.remove();
                }

                var $tag = $('<link>');
                $tag.attr('href', url);
                $tag.attr('type', 'text/css');
                $tag.attr('rel', 'stylesheet');
                $tag.attr('id', 'cssFile_' + id);
                $('head').append($tag);

                $tag.on('load', function () {
                    api.fireCallback(callback);
                });
            }
        },
        loadJS: function (url, id, callback) {
            $.getScript(url)
           .done(function (script, textStatus) {
               //console.log('Js file end load: ' + url);
               api.fireCallback(callback);
           })
           .fail(function (jqxhr, settings, exception) {
               //console.log('Failed to load script');
           });
        },
        fireCallback: function (callbackFunction) {
            if (typeof callbackFunction !== 'undefined') {
                if (typeof callbackFunction === 'function')
                { callbackFunction(); }
                else {
                    try {
                        //eval(callbackFunction)();
                        window[callbackFunction]();
                        //var tmpFunc = new Function(callbackFunction);
                        //tmpFunc();
                    } catch (e) {

                    }
                }
            }
        }
    };
    return api;
})(jQuery);

/* Control de errores de las llamadas a Ajax */

// $(document).ajaxError(function(event, jqxhr, settings, thrownError) {
// if (jqxhr.readyState == 4) {
// // alert("Error ajax \n" + "status : " + jqxhr.status + "\n " +
// jqxhr.statusText + "\n URL : " + settings.url);
// window.location = com.grupopinero.common.Globals.options.appContext
// +
// "/slt/AjaxError?status="+jqxhr.status+"&statusText="+jqxhr.statusText+"&url="+settings.url;
// }
// });

// IIFE - Immediately Invoked Function Expression
(function ($, window, document) {

    // The $ is now locally scoped
    // Listen for the jQuery ready event on the document
    $(function () {

    });
    // The rest of the code goes here!
} (window.jQuery, window, document));