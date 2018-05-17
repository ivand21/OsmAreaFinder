$('#btn-add-filter').click(addFilter);

function addFilter() {

    var type = $('#in-objtype').val();
    var minmax = $('#in-minmaxtype').val();
    var distance = $('#in-distance').val();

    var btnRemove = '<img src="/Content/Images/icon-remove.png" onclick="delFilter($(this))" class="btn-del-filter"/>';

    var str = '<tr><td>' + type + '</td><td>' + minmax + '</td><td>' + distance + '</td><td>' + btnRemove + '</td></tr>'

    var filterRepeatFlag = $('#filter-table-body').html().indexOf(type.toString());
    var correctDistance = !isNaN(parseFloat(distance)) && isFinite(distance) && distance >= 0;

    if (filterRepeatFlag == -1 && correctDistance)
        $('#filter-table-body').append(str);
}

function delFilter(row) {
    row.closest('tr').remove();
    //$(this).remove();
}
function search() {
    if (lastFeature) {
        var circle = lastFeature.getGeometry();
        var radius = circle.getRadius();
        var circleTransform = circle.transform('EPSG:3857', 'EPSG:4326');
        var coords = circleTransform.getCenter();
        $("#map-text").html("Promień obszaru: " + radius);
        $("#map-text").append("/nPunkt środkowy: Lon:  " + coords[0] + "  Lat:  " + coords[1]);
    }

    var data = JSON.stringify({ 'Radius': radius, 'Lon': coords[0], 'Lat': coords[1], 'Filters': getAllFilters() });
    console.log(data);
    $.ajax({
        type: 'POST',
        url: searchUrl,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'filters': data }),
        success: function (response) {
            console.log(response);
            if (response.Message)
            {
                alert('Znaleziono punkt');
            }
            else
            {
                alert('Nie znaleziono punktu');
            }
        },
        error: function () {
            alert("Coś nie działa");
        }
    });
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
