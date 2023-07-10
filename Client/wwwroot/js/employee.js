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

/*function detail(stringURL) {
    $.ajax({
        url: stringURL
    }).done(res => {
        
    })
}*/

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

$(document).ready(function () {
    moment.locale('id');
    $('#myTable').DataTable({
        ajax: {
            url: "https://localhost:7239/api/employees/get-all-master",
            dataType: "JSON",
            dataSrc: "data" //data source -> butuh array of object
        },
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copyHtml5',
                text: '<i class="fa fa-files-o"></i>',
                titleAttr: 'Copy'
            },
            {
                extend: 'colvis',
                className: 'btn-colvis',
                collectionLayout: 'fixed three-column',
                postfixButtons: ['colvisRestore']
            },
            {
                extend: 'print',
                className: 'btn btn-danger',
                messageTop: 'PRINT EMPLOYEE',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'collection',
                text:'Export',
                className: 'custom-html-collection',
                buttons: [
                    '<h3>Export</h3>',
                    {
                        extend: 'pdfHtml5',
                        title: 'PDF',
                        text: 'to PDF',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'excelHtml5',
                        title: 'Excel',
                        text: 'to Excel',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                ]
            }
        ],
        autoWidth: false,
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "nik", },
            { data: "fullName", },
            {
                data: "birthDate", 
                render: function (data, type, row) {
                    return moment(data).format('DD MMMM YYYY');
                }
            },
            {
                data: "gender",
                render: function (data, type, row) {
                    if (data == 0) {
                        return 'Female';
                    }
                    return 'Male';
                }
            },
            {
                data: "hiringDate",
                render: function (data, type, row) {
                    return moment(data).format('DD MMMM YYYY');
                }
            },
            { data: "email", },
            { data: "phoneNumber", },
            { data: "major", },
            { data: "degree", },
            { data: "gpa", },
            { data: "universityName", },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="detail('${data.url}')" data-bs-toggle="modal" data-bs-target="#modalPKM" class="btn btn-primary">Detail</button>`;
                }
            }
        ]
    });
});