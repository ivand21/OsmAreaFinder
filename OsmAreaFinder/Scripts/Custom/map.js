var raster = new ol.layer.Tile({
    source: new ol.source.OSM()
});

var source = new ol.source.Vector({ wrapX: false });

var vector = new ol.layer.Vector({
    source: source
});

var map = new ol.Map({
    layers: [raster, vector],
    target: 'map',
    view: new ol.View({
        center: [0, 0],
        zoom: 12
    })
});

var lon = 18.638306;
var lat = 54.372158;

map.getView().setCenter(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857'));

var draw;
var lastFeature;
function addInteraction() {
    draw = new ol.interaction.Draw({
        source: source,
        type: "Circle"
    });
    map.addInteraction(draw);

    draw.on('drawend', function (e) {
        lastFeature = e.feature;
    });

    draw.on('drawstart', function (e) {
        if (lastFeature)
            source.removeFeature(lastFeature);
    });
}

addInteraction();

$('#btn-start').click(startSearching);

function startSearching() {
    if (lastFeature) {
        var circle = lastFeature.getGeometry();
        var radius = circle.getRadius();
        $("#map-text").html("Promień obszaru: " + radius);
        //var coords = new OpenLayers.LonLat(circle.getCenter()[0], circle.getCenter()[1]).transform('EPSG:4326', 'EPSG:3857');

        //vector.addMarker(new OpenLayers.Marker(circle.getCenter()));
    }
}

