var s_lon = 18.638306;
var s_lat = 54.372158;
var draw;
var lastFeature;


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

map.getView().setCenter(ol.proj.transform([s_lon, s_lat], 'EPSG:4326', 'EPSG:3857'));

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

