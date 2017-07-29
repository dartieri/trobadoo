$(document).ready(function () {

    // Ocultar al cargarse la pagina
    if ($('.hiddenOnLoad').length > 0) {
        $('.hiddenOnLoad').addClass('invisible');
    }

    /* Añade color de fondo a los TD de las tablas con class 'table-hover2' al hacer 'hover' */
    $('table.table-hover2, table.table-hover2 td').hover(function () {
        $('table.table-hover2 td').css('background-color', '');
    });
    $('table.table-hover2 td:odd').hover(function () {
        $(this).prev().data('bgcolor', $(this).siblings().css('background-color')).css('background-color', $(this).css('background-color'));
    });
    $('table.table-hover2 td:odd a, table.table-hover2 td:odd .markerToolTip').hover(function () {
        $(this).parent().prev().css('background-color', $(this).parent().css('background-color'));
    });

    //Menú dinámico (cambio de título y de carouseles)
    $('.dinamicMenu .enlaceRotulo > a[data-carouselid]').click(function (e) {
        e.preventDefault();

        var tit = $(this).closest('.dinamicMenu').find("[data-title='titCarousel']");
        var textTit = tit.text();
        var newTextTit = $(this).text();

        //cambio el texto del rótulo por el seleccionado
        $(tit).text(newTextTit);

        //activo la pestaña seleccionada
        $(this).siblings('a.active').removeClass('active invisible');
        $(this).addClass('active invisible');

        var carouselID = $(this).data('carouselid');

        //Ocultamos el carousel activo
        $(this).closest('.dinamicMenu').find('.carousel.active').removeClass('active').addClass('invisible');

        //Para mostrar el nuevo seleccionado
        $(this).closest('.dinamicMenu').find(carouselID).removeClass('invisible').addClass('active');

        //Visualizar las imágenes del carousel que estaba oculto
        $("img.lazy").lazyload();
    });

    //Cuando se pulsa sobre rótulos de tipo collapse (BT 3.0)
    if ($('[data-collapse]').length > 0) {
        $('[data-collapse]').click(function (e) {

            var dataCollapse = $(this).data('collapse');
            var maxResolution = 0;

            if (dataCollapse == "collapse-xs") {
                maxResolution = 768;
            } else if (dataCollapse == "collapse-xs-sm") {
                maxResolution = 992;
            }

            if (windowWidth <= maxResolution) {

                $('[data-toggle="collapse"]').parent().removeClass('active');
                $(this).addClass('active');

                var dataParent = $(this).data('parent').toString();

                if ($(this).find('.fa-chevron-right').length > 0) {
                    $(this).find('.fa-chevron-right').removeClass('fa-chevron-right').addClass('fa-chevron-down');
                } else {
                    $(this).find('.fa-chevron-down').removeClass('fa-chevron-down').addClass('fa-chevron-right');
                }
                //En esta resolución hacemos que está desplegado solamente el pulsado, el resto se pliegan
                var actual = $(this).find('a[data-toggle="collapse"]').attr('href');
                var hermanos = $(dataParent + ' [data-collapse="' + dataCollapse + '"]:visible').not('.active');
                var idHermano;

                $(hermanos).each(function (call, args) {
                    idHermano = $(args).find('a[data-toggle="collapse"]').attr('href');
                    $(idHermano).collapse('hide');
                    $(hermanos).find('.fa-chevron-down').removeClass('fa-chevron-down').addClass('fa-chevron-right');
                });

                setTimeout(function () {
                    $(actual).collapse('toggle');
                }, 200);

                setTimeout(function () {
                    scrollToAnchor(actual);
                }, 300);
                

            } else {
                //Y parar el efecto de collapse
                e.preventDefault();
                e.stopPropagation();
            }
        });
    }

    //Funcion que sirve para determinar cuando NO queremos mostrar un tooltip en funcion de la resolucion
    if ($('.tooltipShow[data-notdisplay]').length > 0) {
        $('.tooltipShow[data-notdisplay]').on('show.bs.tooltip', function (e) {
            var acceptResolutions = $(this).data('notdisplay').toString().split(' ');

            for (var i = 0; i < acceptResolutions.length; i++) {
                switch (acceptResolutions[i]) {
                    case 'lg':
                        if (windowWidth > 1199) {
                            e.preventDefault();
                            e.stopPropagation();
                        }
                        break;
                    case 'md':
                        if (windowWidth > 979 && windowWidth < 1200) {
                            e.preventDefault();
                            e.stopPropagation();
                        }
                        break;
                    case 'sm':
                        if (windowWidth > 767 && windowWidth < 980) {
                            e.preventDefault();
                            e.stopPropagation();
                        }
                        break;
                    case 'xs':
                        if (windowWidth < 768) {
                            e.preventDefault();
                            e.stopPropagation();
                        }
                        break;
                }
            }
        });
    }
    
    //Modales con URL remota
    $('*[data-toggle="modal"]').click(function (e) {
        var remoteID = $(this).data('remote');

        if (remoteID != '' && remoteID != undefined) {
            e.preventDefault();
            e.stopPropagation();
            var frameSrc = $(this).attr('href');
            var title = $(this).data('titlemodal');

            modalRemote(remoteID, frameSrc, title);
        }
    });

    //Mostrar las imagenes con class "lazy" al abrir un Modal
    $('.modal').on('show.bs.modal', function (obj) {
        //Disabled History Back
        if (windowWidth <= 767) {
            window.onbeforeunload = function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                $('.modal:visible').modal('hide');
                return '';
            }
        }
        setTimeout(function () {
            $(obj.target).find('img.lazy').lazyload();
        }, 300);
    });


    //Mostrar las imagenes con class "lazy" al abrir un Accordion
    $('.panel-heading a').click(function (e) {
        $(this).closest('.panel').find('img.lazy').each(function (x, image) {
            if ($(image).hasClass('lazy')) {
                $(image).removeClass('lazy');
                $(image).attr('src', $(image).data('original'));
            }
        });
    });

    //Modulo accordion con hover
    $('.accordion-hover .panel-heading a').hover(function (e) {
        //Si esta abierto o se esta cerrando un accordion
        if (!$(this).parent().siblings('.panel-body').hasClass('in') && $('.accordion-hover .panel-body.collapsing').length == 0) {
            $(this).closest('.panel').find('img.lazy').each(function (x, image) {
                if ($(image).hasClass('lazy')) {
                    $(image).removeClass('lazy');
                    $(image).attr('src', $(image).data('original'));
                }
            });

            //Escondemos los demás
            $(this).closest('.panel-group').find('.panel-body.in').collapse('hide');

            //Mostramos el seleccionado
            $(this).parent().siblings('.panel-body').collapse('show');
        }
    });

    $('.collapse').on('show.bs.collapse', function () {
        $(this).closest('.panel').find('img.lazy').each(function (x, image) {
            if ($(image).hasClass('lazy')) {
                $(image).removeClass('lazy');
                $(image).attr('src', $(image).data('original'));
            }
        });
    });

    //Mostrar las imagenes con class "lazy" al hacer slide en un Carousel
    $('.carousel').on('slide.bs.carousel', function (e) {

        //Si hay slide pero el carousel no es visible en ese momento, no se ejecuta este script
        if ($(this).is(':visible')) {

            //SCROLLING LEFT
            var prevItem = $('.active.item', this).prev('.item');

            //Account for looping to LAST image
            if (!prevItem.length) {
                prevItem = $('.active.item', this).siblings(":last");
            }

            //Get image selector
            prevImage = prevItem.find('img');

            //Remove class to not load again - probably unnecessary
            $(prevImage).each(function (i, val) {
                if ($(this).hasClass('lazy')) {
                    $(this).removeClass('lazy');
                    $(this).attr('src', $(this).data('original'));
                }
            });

            //SCROLLING RIGHT
            var nextItem = $('.active.item', this).next('.item');

            //Account for looping to FIRST image
            if (!nextItem.length) {
                nextItem = $('.active.item', this).siblings(":first");
            }

            //Get image selector
            nextImage = nextItem.find('img');

            //Remove class to not load again - probably unnecessary
            $(nextImage).each(function (x, val) {
                //Remove class to not load again - probably unnecessary
                if ($(this).hasClass('lazy')) {
                    $(this).removeClass('lazy');
                    $(this).attr('src', $(this).data('original'));
                }
            });
        }
    });
   
    //Images outside of viewport (visible part of web page) wont be loaded before user scrolls to them
    if ($('img.lazy').length > 0) {
        $(function () {
            //When load image not found
            $('img').on('error', function (e) {
                if ($(this).attr('data-alternative') != '') {
                    var alternative = $(this).attr('data-alternative');
                    $(this).attr('data-alternative', '');
                    $(this).attr('src', alternative);
                }
            });

            //When page load animate scroll top
            $('img.lazy').each(function () {
                $(this).show().lazyload({
                    skip_invisible: true,
                    effect: "fadeIn"
                });
            });
        });
    }

    //Al cargarse la imagen lazy se elimina esa class
    $('img.lazy').load(function () {
        var originalsrc = $(this).data('original');
        var currentsrc = $(this).attr('src');
        if (currentsrc == originalsrc) {
            $(this).removeClass('lazy');
        }
    });

});



// *************** Funcion para abrir Modales Remotos ************** //
function modalRemote(id, frameSrc, title, callback, iframeEnabled) {
    var modal = $('.modal' + id);

    $(modal).one('show.bs.modal', function () {
        if (title != '' && title != undefined) {
            $(modal).find('.modal-header #titleModal').html(title);
        }
        if (typeof callback != 'undefined') {
            if (typeof iframeEnabled != 'undefined' && iframeEnabled) {
                if ($(modal).find('.modal-body iframe').length == 0) {
                    $(modal).find('.modal-body').html('<iframe src="" frameborder="0" height="100%" scrolling="no" width="100%"></iframe>');
                }
                if ($(modal).find('.modal-body .modalRemoteLoading').length == 0) {
                    //$('<div class="modalRemoteLoading text-center p-20"><i class="fa fa-spinner fa-spin fa-10x"></i></div>').appendTo($(modal).find('.modal-body'));
                    var domain = 'http://cdn.logitravel.com';
                    if (typeof dominioCDN != 'undefined') {
                        domain = dominioCDN;
                    }
                    $('<div class="modalRemoteLoading text-center p-40"><img src="' + domain + '/comun/images/esperas/loadingNew.gif"/></div>').appendTo($(modal).find('.modal-body'));
                }
                $(modal).find('.modal-body .modalRemoteLoading').show();
                var iframe = $(modal).find('.modal-body iframe');
                $(iframe).hide();
                $(iframe).attr('src', frameSrc);
                $(iframe).load(function () {
                    $(iframe).show();
                    $(modal).find('.modal-body .modalRemoteLoading').hide();
                    $(iframe).height($(iframe).contents().find('body').height());
                }, callback);
            } else {
                $(modal).find('.modal-body').load(frameSrc, callback);
            }
        } else {
            if (typeof iframeEnabled != 'undefined' && iframeEnabled) {
                if ($(modal).find('.modal-body iframe').length == 0) {
                    $(modal).find('.modal-body').html('<iframe src="" frameborder="0" height="100%" scrolling="no" width="100%"></iframe>');
                }
                if ($(modal).find('.modal-body .modalRemoteLoading').length == 0) {
                    //$('<div class="modalRemoteLoading text-center p-20"><i class="fa fa-spinner fa-spin fa-10x"></i></div>').appendTo($(modal).find('.modal-body'));
                    var domain = 'http://cdn.logitravel.com';
                    if (typeof dominioCDN != 'undefined') {
                        domain = dominioCDN;
                    }
                    $('<div class="modalRemoteLoading text-center p-40"><img src="' + domain + '/comun/images/esperas/loadingNew.gif"/></div>').appendTo($(modal).find('.modal-body'));
                }
                $(modal).find('.modal-body .modalRemoteLoading').show();
                var iframe = $(modal).find('.modal-body iframe');
                $(iframe).hide();
                $(iframe).attr('src', frameSrc);
                $(iframe).load(function () {
                    $(iframe).show();
                    $(modal).find('.modal-body .modalRemoteLoading').hide();
                    $(iframe).height($(iframe).contents().find('body').height());
                });
            } else {
                $(modal).find('.modal-body').load(frameSrc);
            }
        }
    });
    
    $(id).modal('show');
}

// *************** Funcion Collapses para los Rotulos ************** //
function collapseBasedScreenSize() {
    if ($('[data-resolutionCollapse]').length > 0) {
        //Mobile resolution
        if (windowWidth < 768) {
            $('[data-resolutionCollapse="xs"]').each(function () {
                if ($(this).hasClass('in')) {
                    $(this).collapse('hide');
                    $(this).removeClass('hidden-xs');
                }
            });
        } else {
            $('[data-resolutionCollapse="xs"]').each(function () {
                if (!$(this).hasClass('in')) {
                    $(this).collapse('show');
                }
            });
        }

        //Tablet & Mobile resolution
        if (windowWidth < 992) {
            $('[data-resolutionCollapse="xs-sm"]').each(function () {
                if ($(this).hasClass('in')) {
                    $(this).collapse('hide');
                    $(this).removeClass('hidden-xs');
                    $(this).removeClass('hidden-sm');
                }
            });
        } else {
            $('[data-resolutionCollapse="xs-sm"]').each(function () {
                if (!$(this).hasClass('in')) {
                    $(this).collapse('show');
                }
            });
        }
    }
}

// *************** Funcion scrollToAnchor ************** //
function scrollToAnchor(aid) {
    var restar = 0;
    if (windowWidth < 992) {
        restar = 100;
    } else if (windowWidth < 768) {
        restar = 50;
    } else {
        restar = 0;
    }

    $('html,body').animate({ scrollTop: $(aid).offset().top - restar }, 'slow');
}


// *************** Funciones Pestañas ************** //

function pestanyaClick(obj, e) {
    //Evitamos que al pulsar sobre el enlace la pantalla se vaya a la cabecera
    if (e) {
        e.preventDefault();
    }
    //Marcamos por CSS la pestaña seleccionada excluyendo el LI dropdown en si mismo
    if (!$(obj).parent().hasClass('dropdown')) {
        $(obj).parent().parent().find('li.active').removeClass('active');
        $(obj).parent().addClass('active');
    }
    //Se comprueba si dentro de ese UL hay un LI del tipo dropdown,
    //si lo hay, se debe mover la elegida al primer lugar
    pestanyaElegida($(obj));
}

function pestanyaElegida(obj) {

    //Evitamos cambiar LI's si no hay dropdown
    if (!$(obj).hasClass('dropdown-toggle') && $(obj).parents('ul.nav-tabs').find('.dropdown').length > 0) {

        var ulPadre = $(obj).parents('ul.nav-tabs');
        var liElegido = $(obj).parent();

        //Desmarcamos las pestañas activas del UL
        $(liElegido).parents('ul.nav-tabs').find('li.active').removeClass('active');

        //Se comprueba que el LI elegido esté dentro del dropdown
        if ($(liElegido).hasClass('dropdownTab')) {

            //Aparte del los posibles LI con class dropdown y fixedTab, deben haber
            //otros LI para moverlos fuera del dropdown sino se deben quedar dentro de él
            if (ulPadre.children('li').not('.dropdown, .fixedTab').length > 0) {
                //Sacamos el LI perteneciente al dropdown
                $(liElegido).removeClass('dropdownTab').detach();

                //Marcamos por CSS la pestaña seleccionada excluyendo el LI dropdown en si mismo
                $(liElegido).addClass('active');

                //El LI con class 'fixedTab' debe ir siempre en primer lugar
                if ($(ulPadre).find('li.fixedTab').length == 0) {
                    $(liElegido).prependTo($(ulPadre));
                } else {
                    $(liElegido).insertAfter($(ulPadre).find('li.fixedTab'));
                }

                //Reorganizamos el listado de Tabs se reconfigure en caso de que haya LI's que sean
                //solamente el dropdown o el activo
                if ($(obj).parents('ul.nav-tabs').find('> li').not('.dropdown, .active').length > 0) {
                    pintadoPestanyas();
                }
            }
        }
    }
    //Marcamos por CSS la pestaña seleccionada en caso de ser un LI 'fixedTab'
    $(liElegido).addClass('active');
}

function pintadoPestanyas() {

    //Recorremos los UL que no tengan la class fixedTabs, que nos servirá para indicar que no queremos que se creen pestañas
    $('ul.nav-tabs').not('.fixedTabs, .ajustaTabs, .nav-justified').each(function (i, e) {

        //Por defecto la pestaña desplegable tendrá el siguiente ancho + margen (.tabsPeque)
        var anchoTabDW = 42;
        if ($(e).hasClass('tabsMedio')) {
            anchoTabDW = 45;
        } else if ($(e).hasClass('tabsGrande')) {
            anchoTabDW = 52;
        }
        
        //Si acabamos de entrar o se hace la ventana más pequeña y además el UL es visible
        if ((opNavTab == 0 || opNavTab == 2) && $(e).is(':visible')) {

            //Variable que suma el ancho de los LI
            var totalWidthLi = 0;

            //Recorremos todos los LI menos los que tienen la class 'dropdown' que es el desplegable
            $($(e).children('li').not('li.dropdown')).each(function (x, li) {

                //Se obtiene el ancho del actual LI
                totalWidthLi = totalWidthLi + parseInt($(this).outerWidth(true));

                //Si hemos excedido con este LI el ancho del UL (añadiendo tamaño del botón dropdown)
                if ((totalWidthLi + anchoTabDW) >= $(e).outerWidth(true)) {
                    //Añadimos la class dropdownTab, incluyendo el LI anterior para
                    //poder colocar un LI desplegable que contendrá todos los que tengan esta class
                    if (!$(this).hasClass('dropdownTab')) {
                        $(this).addClass('dropdownTab');
                    }
                }
            });

            //Si acabamos de entrar en la página o no hay LI dentro del 'dropdown'
            //o el UL todavía no tiene width (solución al pasar a menos de 768px)
            if (opNavTab == 2 || $(e).find('li.dropdownTab').length > 0) {

                //Creamos el UL donde insertaremos los LI que exceden el tamaño del UL
                if ($(e).find('li.dropdown').length == 0 && $(e).find('li.dropdownTab').length > 0) {
                    $(e).append('<li class="dropdown"><a href="#" data-toggle="dropdown" class="dropdown-toggle"><i class="fa fa-caret-down icon-caret-down"></i></a><ul class="dropdown-menu right"></ul></li>');
                }

                //Añadimos los LI sobrantes dentro del UL class 'dropdown-menu'
                $(e).find('li.dropdownTab').detach().appendTo($(e).find('ul.dropdown-menu'));

            } else {

                //Añadimos los LI sobrantes dentro del UL padre
                $(e).find('li.dropdownTab').removeClass('dropdownTab').detach().appendTo($(e));

                //Borramos el dropdown en caso de que no tenga LI dentro de él
                if ($(e).find('li.dropdownTab').length == 0) {
                    $(e).find('li.dropdown').detach();
                }
            }

        } else if (opNavTab == 1 && $(e).outerWidth(true) > 0) {
            //Al ampliar la resolución se debe comprobar si los LI dentro del dropdown se pueden volver a insertar en el UL padre

            var totalWidthLi = 0;
            var totalWidthLiDw = 0;

            //Recorremos todos los LI menos los que tienen la class 'dropdown' que es el desplegable
            $($(e).children('li').not('li.dropdown')).each(function (x, li) {

                //Se obtiene el ancho del actual LI
                totalWidthLi = totalWidthLi + parseInt($(this).outerWidth(true));

                //Si hemos excedido con este LI el ancho del UL (añadiendo tamaño del botón dropdown)
                if ((totalWidthLi + anchoTabDW) >= $(e).outerWidth(true)) {

                    //Añadimos la class dropdownTab, incluyendo el LI anterior para
                    //poder colocar un LI desplegable que contendrá todos los que tengan esta class
                    if (!$(this).hasClass('dropdownTab')) {
                        $(this).addClass('dropdownTab');
                    }
                }
            });

            if ($(e).find('li.dropdown').length == 0 && $(e).find('li.dropdownTab').length > 0) {
                $(e).append('<li class="dropdown"><a href="#" data-toggle="dropdown" class="dropdown-toggle"><i class="fa fa-caret-down icon-caret-down"></i></a><ul class="dropdown-menu right"></ul></li>');
            }

            //Añadimos los LI sobrantes dentro del UL class 'dropdown-menu'
            $(e).find('li.dropdownTab').detach().appendTo($(e).find('ul.dropdown-menu'));

            //Comprobamos que el LI con class 'dropdown' exista
            if ($(e).find('li.dropdown').length > 0) {

                //Guardamos el ancho de todos los LI (padre)
                $($(e).children('li').not('li.dropdown')).each(function (x, li) {
                    //Se obtiene el ancho del actual LI
                    totalWidthLi = totalWidthLi + parseInt($(li).outerWidth(true));
                });

                //Abrimos el dropdown para poder medir los LI
                $(e).find('li.dropdown').addClass('open');

                ////Recorremos todos los LI con la class 'dropdown' que es el desplegable para obtener el ancho total
                $($(e).find('li.dropdownTab')).each(function (x, liDw) {

                    //Se modifica el display para que no obtenga el tamaño del LI más grande del listado dropdown
                    $(this).css('display', 'table');

                    totalWidthLiDw = totalWidthLiDw + parseInt($(this).outerWidth(true));
                    totalWidthLi = 0;

                    //Se vuelve a conceder los estilos originales al LI
                    $(this).css('display', 'block');

                    $($(e).children('li').not('li.dropdown')).each(function (x, li) {
                        //Se obtiene el ancho de todos los LI fuera del dropdown
                        totalWidthLi = totalWidthLi + parseInt($(li).outerWidth(true));
                    });

                    //Comprobamos para cada LI dentro del desplegable a ver si cabe fuera de él
                    if (parseInt(totalWidthLi + anchoTabDW + $(this).outerWidth(true)) < parseInt($(e).outerWidth(true))) {

                        //Movemos el LI del desplegable fuera
                        $(this).removeClass('dropdownTab').detach().appendTo($(e));

                        //Colocamos el LI dropdown al final de la lista
                        $(e).find('li.dropdown').detach().appendTo($(this).parents('ul.nav-tabs'));
                    }
                });

                //Cerramos el dropdown
                $(e).find('li.dropdown').removeClass('open');

                //Si no hay elementos en el menu dropdown eliminamos el LI desplegable
                if ($(e).find('ul.dropdown-menu > li').length == 0) {
                    $(e).find('li.dropdown').detach();
                }
            }
        }
    });

    //Al arrancar la web sacamos los LI con class 'active' del dropdown a primera posición
    for (var i = 0; i <= $('li.dropdown li.active').length - 1; i++) {
        pestanyaElegida($('li.dropdown li.active a')[i]);
    }
}

function closeMasterTabDefault() {
    $('.navegacion').find('.masterTab.fixedTab').removeClass('open fixedTab');
}

// Ajusta las Pestañas al 100% del UL
function configMenuPrincipal() {
    if ($('ul.navPrincipal > li').length > 0) {
        var uls = $('ul.navPrincipal');

        uls.each(function () {
            //Si son los LI de la cabecera del menú de Logi
            if ($(this).parent().parent().hasClass('navegacion')) {
                //Si la pestaña esta dentro del menú
                if (parseFloat($(this).css('padding-left').replace('px', '')) == 0) {
                    if ($(this).find('.masterTab').hasClass('fixedTabTabletNone')) {
                        $(this).find('.masterTab').addClass('open fixedTab').removeClass('fixedTabTabletNone');
                    } else {
                        if ($(this).find('.masterTab.fixedTab').length > 0) {
                            $(this).find('.masterTab.fixedTab').addClass('open');
                        }
                    }
                } else {
                    //Si está fuera en resolución grande
                    if ($(this).find('.masterTab').hasClass('fixedTab')) {
                        $(this).find('.masterTab').removeClass('open fixedTab').addClass('fixedTabTabletNone');
                    } else {
                        $(this).find('.masterTab').removeClass('open');
                    }
                }

            }
        });
    }
}

// Ajusta las Pestañas al 100% del UL
function ajustaTabs() {
    if ($('ul.ajustaTabs > li').length > 0) {
        var uls = $('ul.ajustaTabs');

        uls.each(function () {
            //Si son los LI de cajas
            //$(this).children().next().css('border-left', '1px dotted #64AFDC');
            var widthLIpx = parseFloat(parseFloat($(this).outerWidth(true) - parseFloat($(this).children().length - 1)) / $(this).children().length);
            var widthLItpc = parseFloat(parseFloat(widthLIpx * 100) / $(this).outerWidth(true));
            $(this).children().css('width', widthLItpc + '%').css('margin', 0);
            mismaAltura($(this).children().find('a'));
        });
    }
}


// *************** Función Igualar altura ************** //

function mismaAltura(grupo) {
    masAlto = 0;

    //Reset de la altura mínima
    grupo.css({ "min-height": 'inherit' });

    grupo.each(function () {
        elementoAltura = $(this).outerHeight(true);
        if(elementoAltura > masAlto) {
            masAlto = elementoAltura;
        }
    });
    //Se aplica la nueva altura
    grupo.css({"min-height": masAlto});	
}

function mismoAncho(grupo) {
    masLargo = 0;

    //Reset del ancho mínimo
    grupo.css({ "min-width": 'inherit' });

    grupo.each(function () {
        elementoAncho = $(this).width();
        if (elementoAncho > masLargo) {
            masLargo = elementoAncho;
        }
    });
    //Se aplica el nuevo ancho
    grupo.css({ "min-width": masLargo });
}


// *************** Se comprueba si un elemento está dentro de otro con class 'desactivado' ************** //
function compruebaEstado(event, obj) {
    if ($(obj).parents('.desactivado').length > 0) {
        event.preventDefault();
        return false;
    }
}

// *************** Añadir clase ultimo elemento ************** //
function ultimo(elemento, clase) {
    $(elemento+":last-child").addClass(clase);
};


// *************** Añadir clase primer elemento ************** //
function primero(elemento, clase) {
    $(elemento+":first-child").addClass(clase);
};

// *************** Pinta clase para los elementos pares ************** //
function pintarpares(elemento, clase) {
    $(elemento+":nth-child(2n+1)").addClass(clase);
};

// *************** Pinta clase para los elementos Impares ************** //
function pintarimpares(elemento, clase) {
    $(elemento+":nth-child(2n)").addClass(clase);
};