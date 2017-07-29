$(document).ready(function () {

    $(function () {
        $("img").lazyload({ placeholder: "/Content/images/loading.gif", threshold: 100 });
    });

    var owl = $("#carouselPrincipal");

    owl.owlCarousel({
        navigation: false,
        singleItem: true,
        autoPlay: true,
        stopOnHover: true,
        transitionStyle: "fade"
    });
});

function nAlert(text, layout) {
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
}