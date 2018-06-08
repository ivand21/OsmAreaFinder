$('#btn-add-filter').click(addFilter);

// Load script
var filerefjs = document.createElement('script');
filerefjs.setAttribute("type", "text/javascript");
filerefjs.setAttribute("src", "~/Scripts/sweetalert.min.js);");

// Load CSS file
var filerefcss = document.createElement("link");
filerefcss.setAttribute("rel", "stylesheet");
filerefcss.setAttribute("type", "text/css");
filerefcss.setAttribute("href", "~/Styles/sweetalert.css");

function addFilter() {

    resultSource.clear();

    var type = $('#in-objtype').val();
    var minmax = $('#in-minmaxtype').val();
    var distance = $('#in-distance').val();

    var btnRemove = '<img src="/Content/Images/icon-remove.png" onclick="delFilter($(this))" class="btn-del-filter"/>';

    var str = '<tr><td>' + type + '</td><td>' + minmax + '</td><td>' + distance + '</td><td>' + btnRemove + '</td></tr>'

    var filterRepeatFlag = $('#filter-table-body').html().indexOf(type.toString());
    var correctDistance = !isNaN(parseFloat(distance)) && isFinite(distance) && distance >= 0;

    if (filterRepeatFlag == -1 && correctDistance)
        $('#filter-table-body').append(str);
    else {
        swal("Niepowodzenie operacji", "Nieprawidłowa odległość lub podany filtr już istnieje", "error");
    }
}

function delFilter(row) {
    row.closest('tr').remove();
    //$(this).remove();
}
function search() {
    if (lastFeature) {
        document.body.style.cursor = 'wait';
        var circle = lastFeature.getGeometry();
        var coords = circle.getCenter();
        var radius = circle.getRadius();
        var edgeCoordinate = [coords[0] + radius, coords[1]];
        var wgs84Sphere = new ol.Sphere(6378137);
        var groundRadius = wgs84Sphere.haversineDistance(
            ol.proj.transform(coords, 'EPSG:3857', 'EPSG:4326'),
            ol.proj.transform(edgeCoordinate, 'EPSG:3857', 'EPSG:4326')
        );

        //var radius = circleTransform.getRadius();
        //$("#map-text").html("Promień obszaru: " + groundRadius);
        //$("#map-text").append("/nPunkt środkowy: Lon:  " + coords[0] + "  Lat:  " + coords[1]);


        var data = JSON.stringify({ 'Radius': radius, 'Lon': coords[0], 'Lat': coords[1], 'Filters': getAllFilters() });
        console.log(data);
        $.ajax({
            type: 'POST',
            url: searchUrl,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ 'filters': data }),
            success: function (response) {
                if (response != "") {
                    var geoJsonFormat = new ol.format.GeoJSON();
                    var features = geoJsonFormat.readFeatures(response);
                    $.each(features, function (index, feature) {
                        console.log(style);
                        feature.setStyle(style);
                        resultSource.addFeature(feature);
                    });
                    swal("Powodzenie operacji", "Znaleziono obszary", "success");
                  
                }
                else {
                    swal("Niepowodzenie operacji", "Nie znaleziono obszarów pasujących do podanych filtrów", "error");
                }

                document.body.style.cursor = 'default';
            },
            error: function () {
                alert("Coś nie działa");
            }
        });
    }
    else {
        swal("", "Nie zaznaczono obszaru", "warning");
        document.body.style.cursor = 'default';

    }
}

function getAllFilters() {
    var filters = [];
    $('#filter-table').find('tr').not('thead tr').each(function (k,v) {
        var cells = $(this).find('td');
        var objectType = cells.eq(0).text();
        var minmaxType = cells.eq(1).text();
        var distance = cells.eq(2).text();
        var filter = { 'ObjectType': objectType, 'MinMaxType': minmaxType, 'Distance': distance };
        filters.push(filter);
    });
    return filters;
}
