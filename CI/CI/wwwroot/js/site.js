// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Recommenrded
var dialog = document.getElementById('myDialog');
document.getElementById('show').onclick = function () { dialog.showModal(); };
document.getElementById('hide').onclick = function () { dialog.close(); };

function recommend(id) {
    var to = [];
    $("input:checkbox[id=check]:checked").each(function () {
        to.push($(this).val());
    });
    $.ajax({
        url: "/Home/Recommend",
        type: "POST",
        data: {
            "missionid": id,
            to: to
        }
    });
}


