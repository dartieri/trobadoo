$(document).ready(function () {

    //start menu
    $(".megamenu").megamenu();

    //Image Lazy load
    //$("img").lazyload({ placeholder: "/Content/images/loading.gif", threshold: 100 });
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