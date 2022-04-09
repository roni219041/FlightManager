// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddNewPassengerForm() {
    var mainContainer = document.getElementsByClassName("addPassenger")[0]
    var sampleValueHTML = document.getElementsByClassName("sampleValue")[0].innerHTML
    mainContainer.innerHTML += sampleValueHTML
}