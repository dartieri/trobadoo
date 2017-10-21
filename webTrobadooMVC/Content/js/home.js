"use strict";

var com = com || {};
com.trobadoo = com.trobadoo || {};
com.trobadoo.Home = com.trobadoo.Home || {};

// Declare a class, with parameteres
com.trobadoo.Home = (function ($) {
    var api = {
        /*options: {
        },*/
        // Funcion de inicializacion
        init: function (/*options*/) {
            api.configureInputs();
        },
        configureInputs: function () {
            //end slider
            $("#flexiselDemo").flexisel({
                visibleItems: 4,
                animationSpeed: 1000,
                autoPlay: false,
                autoPlaySpeed: 3000,
                pauseOnHover: true,
                enableResponsiveBreakpoints: true,
                responsiveBreakpoints: {
                    portrait: {
                        changePoint: 480,
                        visibleItems: 1
                    },
                    landscape: {
                        changePoint: 640,
                        visibleItems: 2
                    },
                    tablet: {
                        changePoint: 768,
                        visibleItems: 3
                    }
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

    });
    // The rest of the code goes here!
}(window.jQuery, window, document));