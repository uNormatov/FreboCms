if (typeof Frebo === "undefined") var Frebo = {};

Frebo.formsubmit = function (action, validate) {
    if (typeof validate === "undefined") validate = false;
    var form = $(".mainForm");
    if (typeof action !== "undefined") $("#form-action").val(action);
    if (!validate)
        form.submit();
    else {
        $(".mainForm").validate({
            onsubmit: false
        });
        var isValid = form.valid();
        if (isValid) {
            form.submit();
            $("#form-action").val("");
        }
        else {
            $("#form-action").val("");
        }
    }
};

Frebo.listTask = function (action, value, name) {
    if (typeof name === "undefined") name = "chbxRow";
    $("input[name=" + name + "]").each(function () {

        if (this.value = value)
            this.checked = true;
        else
            this.checked = false;
    });
    $("#checkboxCount").val(1);
    Frebo.formsubmit(action);
};

Frebo.checked = function (checked) {
    var value = $("#checkboxCount").val();
    checked == !0 ? value++ : value--;
    $("#checkboxCount").val(value);
};

Frebo.selectAll = function (b, name) {
    var value = 0;
    var checkedStatus = b.checked;
    if (typeof name === "undefined") name = "chbxRow";
    $("input[name=" + name + "]").each(function () {
        this.checked = checkedStatus;
        value = value + 1;
    });
    $("#checkboxCount").val(value);
};

Frebo.selectQuery = function (id, queryName) {
}

$(document).ready(function () {
    $(".queryselectors").fancybox({
        title: 'Queries',
        maxWidth: 800,
        maxHeight: 700,
        fitToView: true,
        width: '60%',
        height: '75%',
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        type: 'iframe'
    });
    $(".transselectors").fancybox({
        title: 'Transformations',
        maxWidth: 800,
        maxHeight: 700,
        fitToView: true,
        width: '60%',
        height: '75%',
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        type: 'iframe'
    });

    $(".queryeditor").fancybox({
        title: 'Edit Query',
        maxWidth: 800,
        maxHeight: 600,
        fitToView: true,
        width: '60%',
        height: '62%',
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        type: 'iframe'
    });
    $(".transformationeditor").fancybox({
        title: 'Edit Query',
        maxWidth: 800,
        maxHeight: 600,
        fitToView: true,
        width: '60%',
        height: '62%',
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none',
        type: 'iframe'
    });
});

