"use strict";
var el = document.getElementById('editor');
el.style.height = '500px';

// window.editor is accessible.
var editor = null;
var init = function () {

    require(['vs/editor/editor.main'], function () {

        editor = monaco.editor.create(el, {
            language: 'html'
        });

        editor.layout();
    });

    // no point in keeping this around.
    window.removeEventListener("load", init);

    var type = Number.parseInt($('#text-format-select').val());
    if (type === 0) {
        monaco.editor.setModelLanguage(editor.getModel(), 'markdown');
    }
    else if (type === 1) {
        monaco.editor.setModelLanguage(editor.getModel(), 'html');
    }

    editor.setValue($('#Data_CustomExperience').val());
};

function OnLanguageChange(value) {
    value = Number.parseInt(value);
    if (value === 0) {
        monaco.editor.setModelLanguage(editor.getModel(), 'markdown');
    }
    else if (value === 1) {
        monaco.editor.setModelLanguage(editor.getModel(), 'html');
    }
}

$('#edit-form').submit(function (e) {
    $('#Data_CustomExperience').val(editor.getValue());
});

window.addEventListener("load", init);