$('#btn-add-filter').click(addFilter);

function addFilter() {

    var type = $('#in-objtype').val();
    var distance = $('#in-distance').val();

    var btnRemove = '<img src="/Content/Images/icon-remove.png" onclick="delFilter($(this))" class="btn-del-filter"/>';

    var str = '<tr><td>' + type + '</td><td>' + distance + '</td><td>' + btnRemove + '</td></tr>'

    $('#filter-table-body').append(str);
}

function delFilter(row) {
    row.closest('tr').remove();
    //$(this).remove();
}