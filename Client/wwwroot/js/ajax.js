// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/"
}).done((result) => {
    console.log(result.results);
    let temp = "";
    $.each(result.results, (key, val) => {
        temp += `<tr>
                    <td>${key + 1}</td>
                    <td>${capitalizeFirstLetter(val.name) }</td>
                    <td><button onclick="detail('${val.url}')" data-bs-toggle="modal" data-bs-target="#modalPKM" class="btn btn-primary">Detail</button></td>
                </tr>`;
    })
    $("#tbodyPKM").html(temp);
})*/

function detail(stringURL) {
    $.ajax({
        url: stringURL
    }).done(res => {
        $(".modal-title").html(capitalizeFirstLetter(res.name));
        $('#image-front').attr('src', res.sprites.other.home.front_default);
        $('#image-shiny').attr('src', res.sprites.other.home.front_shiny);
        let tempBase = "";
        $.each(res.stats, (key, val) => {
            tempBase += `<tr><td>${capitalizeFirstLetter(val.stat.name)}<div class="progress" id="bar" role="progressbar" aria-label="Animated striped example" aria-valuenow="${val.base_stat}" aria-valuemin="0" aria-valuemax="100">
            <div class="progress-bar progress-bar-striped progress-bar-animated"
            style="width: ${val.base_stat}%">${val.base_stat}</div></div></td></tr>`;
        })
        $(".screen-stats").html(tempBase);

        let tempType = "";
        $.each(res.types, (key, val) => {
            tempType += `<div class="badge rounded-pill bg-secondary">${capitalizeFirstLetter(val.type.name)}</div>`;
        })
        $("#types").html(tempType);

        let tempAbility = "";
        $.each(res.abilities, (key, val) => {
            tempAbility += `<ul>
                    <li>${capitalizeFirstLetter(val.ability.name)}</li>
                </ul>`;
        })
        $(".ability-list").html(tempAbility);

        let tempMove = "";
        $.each(res.moves, (key, val) => {
            tempMove += `<tr>
                    <td>${key + 1}</td>
                    <td>${capitalizeFirstLetter(val.move.name)}</td>
                </tr>`;
        })
        $("#tbodyMove").html(tempMove);

        $(".height").html((res.height / 10) + " m");
        $(".weight").html((res.weight / 10) + " kg");
    })
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

$(document).ready(function () {
    $('#myTable').DataTable({
        ajax: {
            url: "https://pokeapi.co/api/v2/pokemon/",
            dataType: "JSON",
            dataSrc: "results" //data source -> butuh array of object
        },
        columns: [
            {
                data: 'url',
                render: function (data, type, row) {
                    // Mengambil ID dari URL Pokemon API
                    let pokemonId = data.split('/')[6];
                    return pokemonId;
                }
            },
            {
                data: "name",
                render: function (data, type, row) {
                    let nama = capitalizeFirstLetter(data)
                    return nama;
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="detail('${data.url}')" data-bs-toggle="modal" data-bs-target="#modalPKM" class="btn btn-primary">Detail</button>`;
                }
            }
        ]
    });
});