function GenerateLayout(options) {
    var tableLayout = "";

    var added = false;



    var optionsCount = options.length;

    for (var i = 0; i < optionsCount; i++) {
        tableLayout += "<li><label class='jform_name-lbl'> $$label:" + options[i].value + "$$</label>$$input:" + options[i].value + "$$$$validation:" + options[i].value + "$$</li>";
        added = true;
    }

    if (added) {
        tableLayout = "<ul class='adminformlist'>" + tableLayout + "</ul>";
    }

    return tableLayout;
}

function InsertHTML(htmlString) {
    var oEditor = FCKeditorAPI.GetInstance(fkcEditorID);

    if (oEditor.EditMode == FCK_EDITMODE_WYSIWYG) {
        oEditor.InsertHtml(htmlString);
    }
    else
        alert('You must be on WYSIWYG mode');
}

function InsertField(htmlString) {
    var content = GetContent();

    if (!IsInContent(content, htmlString)) {
        InsertHTML(htmlString);
        content = GetContent();
        if (!IsInContent(content, htmlString)) {
            InsertHTML(htmlString);
        }
    }
    else {
        alert("Field " + htmlString + " is already exists");
    }
}

function SetContent(options) {
    var newContent = GenerateLayout(options);

    var oEditor = FCKeditorAPI.GetInstance(fkcEditorID);

    oEditor.SetHTML(newContent);
}

function GetContent() {
    var oEditor = FCKeditorAPI.GetInstance(fkcEditorID);

    return oEditor.GetXHTML(true);
}

function IsInContent(content, htmlString) {
    return (content.toLowerCase().indexOf(htmlString.toLowerCase()) != -1);
}