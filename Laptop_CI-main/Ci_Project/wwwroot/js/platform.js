


var checkboxes = document.querySelectorAll(".checkbox");

let filtersSection = document.querySelector(".filters-section");

var listArray = [];

var filterList = document.querySelector(".filter-list");

var len = listArray.length;

for (var checkbox of checkboxes) {
    checkbox.addEventListener("click", function () {
        if (this.checked == true) {
            addElement(this, this.value);
        }
        else {

            removeElement(this.value);
            console.log("unchecked");
        }
    })
}


function addElement(current, value) {
    loadMissions();

    let filtersSection = document.querySelector(".filters-section");

    let createdTag = document.createElement('span');
    createdTag.classList.add('filter-list');
    createdTag.classList.add('ps-3');
    createdTag.classList.add('pe-1');
    createdTag.classList.add('me-2');
    createdTag.innerHTML = value;

    createdTag.setAttribute('id', value);
    let crossButton = document.createElement('button');
    crossButton.classList.add("filter-close-button");
    let cross = '&times;'



    crossButton.addEventListener('click', function () {
        let elementToBeRemoved = document.getElementById(value);

        console.log(elementToBeRemoved);
        console.log(current);
        elementToBeRemoved.remove();

        current.checked = false;




    })

    crossButton.innerHTML = cross;


    // let crossButton = '&times;'

    createdTag.appendChild(crossButton);
    filtersSection.appendChild(createdTag);

}

function removeElement(value) {
    loadMissions();


    let filtersSection = document.querySelector(".filters-section");

    let elementToBeRemoved = document.getElementById(value);
    filtersSection.removeChild(elementToBeRemoved);

}

//Landing Page Cards

let missionToSearch = "";
let sortBy = "";


$(document).ready(function () {
    loadMissions();

});

$("#search-field").on("keyup", function (e) {
    missionToSearch = $("#search-field").val().toLowerCase();
    console.log(missionToSearch);
    loadMissions();
});



function loadMissions(pg,sortVal) {
    var country = [];
    $('#countryDropdown').find("input:checked").each(function (i, ob) {
        country.push($(ob).val());
    });
    console.log(country);

    var city = [];
    $('#cityDropdown').find("input:checked").each(function (i, ob) {
        city.push($(ob).val());
    });

    var themes = [];
    $('#themesDropdown').find("input:checked").each(function (i, ob) {
        themes.push($(ob).val());
    });

    var skills = [];
    $('#skillsDropdown').find("input:checked").each(function (i, ob) {
        skills.push($(ob).val());
    });

    if (sortVal != null) {
        sortBy = sortVal;
    }

    $("#missionSpinner").removeClass("d-none");

    $.ajax({
        url: "/Home/Platform",
        method: "POST",
        dataType: "html",
        data: { "sortVal": sortBy, "search": missionToSearch, "country": country, "city": city, "themes": themes, "skills": skills, "pg": pg},
        success: function (data) {
            //console.log(data);
            $("#missionSpinner").addClass("d-none");

            $('#missions-list').html("");
            $('#missions-list').html(data);
        },
        error: function (error) {

            console.log(error);
        }
    });
}

