﻿function CustomMarker(latlng, map) {
    this.latlng_ = latlng;

    // Once the LatLng and text are set, add the overlay to the map.  This will
    // trigger a call to panes_changed which should in turn call draw.
    this.setMap(map);
}

CustomMarker.prototype = new google.maps.OverlayView();

CustomMarker.prototype.draw = function () {
    var me = this;

    // Check if the div has been created.
    var div = this.div_;
    if (!div) {
        // Create a overlay text DIV
        div = this.div_ = document.createElement('DIV');
        // Create the DIV representing our CustomMarker
        div.style.border = "none";
        div.style.position = "absolute";
        div.style.paddingLeft = "0px";
        div.style.cursor = 'pointer';

        var img = document.createElement("img");
        img.src = "/Content/images/icons/mapMarker.png";
        div.appendChild(img);
        google.maps.event.addDomListener(div, "click", function (event) {
            google.maps.event.trigger(me, "click");
        });

        // Then add the overlay to the DOM
        var panes = this.getPanes();
        panes.overlayImage.appendChild(div);
    }

    // Position the overlay 
    var point = this.getProjection().fromLatLngToDivPixel(this.latlng_);
    if (point) {
        div.style.left = (point.x - 15) + 'px';
        div.style.top = (point.y - 35) + 'px';
    }
};

CustomMarker.prototype.remove = function () {
    // Check if the overlay was on the map and needs to be removed.
    if (this.div_) {
        this.div_.parentNode.removeChild(this.div_);
        this.div_ = null;
    }
};

CustomMarker.prototype.getPosition = function () {
    return this.latlng_;
};

var map;
var overlay;

function initialize() {
    //console.log('init map');
    var latlng = new google.maps.LatLng(39.55747, 2.68375);
    var myOptions = {
        zoom: 15,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("mapaContacto"),
            myOptions);

    //var image = '/Content/images/icons/mapMarker.png';
    
    /*var mapMarker = new google.maps.Marker({
    position: myLatLng,
    map: map,
    icon: image
    });
    var iw = new google.maps.InfoWindow({ content: getInfoWindowContent(), pixelOffset: new google.maps.Size(5, 0)});
    iw.open(map, mapMarker);
    google.maps.event.addListener(mapMarker, "click", function () {
    iw.open(map, mapMarker);
    });*/
    
    overlay = new CustomMarker(map.getCenter(), map);
    var iw = new google.maps.InfoWindow({ content: getInfoWindowContent(), pixelOffset: new google.maps.Size(5, 0) });
    iw.open(map, overlay);
    google.maps.event.addListener(overlay, "click", function () {
        iw.open(map, overlay);
    });
}

function addOverlay() {
    overlay.setMap(map);
}

function removeOverlay() {
    overlay.setMap(null);
}

$(document).ready(function () {
    initialize();
});

function getInfoWindowContent() {
    var str = '';
    str += '<div class="mb-10"><img src="/Content/images/icons/icoTrobadoo.png"/></div>';
    str += '<div>';
    //str+='<p>TROBADOO SEGUNDA MANO SL</p>';
    str += '<p>Calle LLucmajor, 152<br/></p>';
    str += '<p>07007 PALMA DE MALLORCA<br/></p>';
    str += '<p>ES MOLINAR';
    return str;
}