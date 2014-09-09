function addField() {
    var numberOfFields = $('#ulmultiple li').length + 1;
    var li = $('<li/>').attr('id', 'multipleli_' + numberOfFields);
    var parameterType = $('<select>').attr('id', 'parameterType_' + numberOfFields).attr('name', 'parameterType_' + numberOfFields);
    parameterType.append('<option value=\'0\'>Please Select</option>');
    parameterType.append('<option value=\'1\'>Query String</option>');
    parameterType.append('<option value=\'2\'>Seo Template</option>');
    parameterType.append('<option value=\'3\'>Cookie</option>');
    parameterType.append('<option value=\'4\'>User Profile Property</option>');
    parameterType.append('<option value=\'5\'>Language</option>');

    var table = $('<table/>').attr('style', 'float:left');
    var tr = $('<tr/>');
    tr.append($('<td/>').append('Type'))
                    .append($('<td/>').append(parameterType));

    var parameterDbType = $('<select>').attr('id', 'parameterDbType_' + numberOfFields).attr('name', 'parameterDbType_' + numberOfFields);
    parameterDbType.append('<option value=\'0\'>Please Select</option>');
    parameterDbType.append('<option value=\'1\'>String</option>');
    parameterDbType.append('<option value=\'2\'>Integer</option>');
    tr.append($('<td/>').append('Db Type'))
                    .append($('<td/>').append(parameterDbType));

    var tr2 = $('<tr/>');
    var parameterName = $('<input />').attr('type', 'text').attr('id', 'parameterName_' + numberOfFields).attr('name', 'parameterName_' + numberOfFields);
    tr2.append($('<td/>').append('Name'))
                    .append($('<td/>').append(parameterName));

    var parameterValue = $('<input />').attr('type', 'text').attr('id', 'parameterValue_' + numberOfFields).attr('name', 'parameterValue_' + numberOfFields);
    tr2.append($('<td/>').append('Value'))
                    .append($('<td/>').append(parameterValue));

    var parameterDefaultValue = $('<input />').attr('type', 'text').attr('id', 'parameterDefaultValue_' + numberOfFields).attr('name', 'parameterDefaultValue_' + numberOfFields);
    tr2.append($('<td/>').append('Default Value'))
                    .append($('<td/>').append(parameterDefaultValue));


    var td = $('<td/>');

    td.append($('<a />').attr('class', 'addRow').attr('href', 'javascript:addField();').append('<img src=\'/content/css/images/menu/addMore_add.png\' />'));

    if (numberOfFields != 1)
        td.append($('<a />').attr('class', 'removeRow').attr('href', 'javascript:removeField(' + numberOfFields + ');').append('<img src=\'/content/css/images/menu/addMore_remove.png\' />'));

    tr2.append(td);
    table.append(tr);
    table.append(tr2);
    li.append(table);
    $('#ulmultiple').append(li);
}

function removeField(id) {
    $('#multipleli_' + id).remove();
}
