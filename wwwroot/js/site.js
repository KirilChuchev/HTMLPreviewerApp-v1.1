// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function checkOriginal(value) {
    alert(`This sample is ${value} with the original.`);
}

function showSaveNotification(number, notificationText) {
    alert(`HTML Sample #${number}. was ${notificationText}!`);
}

function copyUrl(id) {
    let copyUrlText = document.getElementById(id);
    copyUrlText.select();
    copyUrlText.setSelectionRange(0, 99999)
    document.execCommand("copy");
    alert("Copied URL: " + copyUrlText.value);
}