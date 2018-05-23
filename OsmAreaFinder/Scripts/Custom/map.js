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

var resultSource = new ol.source.Vector();

var resultLayer = new ol.layer.Vector({
    source: resultSource
});

var map = new ol.Map({
    layers: [raster, vector, resultLayer],
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
        resultSource.clear();
        if (lastFeature)
            source.removeFeature(lastFeature);
    });
}

function drawPolygon(values) {
    var polygon = new ol.geom.Polygon([values]);
    //polygon.transform('EPSG:3857', 'EPSG:4326');

    var style = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: 'rgba(255,0,0,0.3)',
            width: 1
        }),
        fill: new ol.style.Fill({
            color: 'rgba(255,0,0,0.3)'
        })
    });
    
    var feature = new ol.Feature(polygon);
    feature.setStyle(style);
    resultSource.addFeature(feature); 
    
    console.log(polygon.getFirstCoordinate());
}

addInteraction();

