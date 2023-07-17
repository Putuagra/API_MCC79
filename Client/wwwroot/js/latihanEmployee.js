// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

$(document).ready(function () {
    moment.locale('id');
    $('#myTable').DataTable({
        ajax: {
            url: "https://localhost:7239/api/employees",
            dataType: "JSON",
            dataSrc: "data" //data source -> butuh array of object
        },
        dom: "<'row'<'col-sm-2'l><'col-sm-5'B><'col-md-5'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-5'p>>" ,
        buttons: [
            {
                extend: 'copyHtml5',
                text: '<i class="fa fa-files-o"></i>'
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
            { data: 'fullName', render: function (data, type, row) { return row.firstName + ' ' + row.lastName; } },
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
           /* { data: "major", },
            { data: "degree", },
            { data: "gpa", },
            { data: "universityName", },*/
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="GetById('${row.guid}')" data-bs-toggle="modal" data-bs-target="#updateModal" class="btn btn-success"><i class="bi bi-arrow-repeat"></i>Update</button>`;
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="Delete('${row.guid}')"class="btn btn-danger"><i class="bi bi-trash3"></i>Delete</button>`;
                }
            }
        ]
    });
});

function Insert() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    let gender = $("input[name='gender']:checked").val();
    let genderEnum;
    if (gender == "Female") {
        genderEnum = 0;
    } else {
        genderEnum = 1;
    }
    obj.FirstName = $("#formNamaDepan").val();
    obj.LastName = $("#formNamaBelakang").val();
    obj.Email = $("#formEmail").val();
    obj.PhoneNumber = $("#formNomorHP").val();
    obj.BirthDate = $("#formTanggalLahir").val();
    obj.HiringDate = $("#formTanggalMasuk").val();
    obj.Gender = genderEnum;
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:7239/api/employees",
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json"
    }).done((result) => {
        Swal.fire('Saved!', 'You created data', 'success').then(() => {
            location.reload();
        });
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!'
        });
    })
}

function Update() {
    var obj = new Object();
    let gender = $("input[name='updateGender']:checked").val();
    let genderEnum;
    if (gender == "Female") {
        genderEnum = 0;
    } else {
        genderEnum = 1;
    }
    obj.FirstName = $("#formUpdateNamaDepan").val();
    obj.LastName = $("#formUpdateNamaBelakang").val();
    obj.Email = $("#formUpdateEmail").val();
    obj.PhoneNumber = $("#formUpdateNomorHP").val();
    obj.BirthDate = $("#formUpdateTanggalLahir").val();
    obj.HiringDate = $("#formUpdateTanggalMasuk").val();
    obj.Gender = genderEnum;
    obj.Guid = $("#guidUpdate").val();
    obj.Nik = $("#nikUpdate").val();
    $.ajax({
        url: "https://localhost:7239/api/employees",
        type: "PUT",
        data: JSON.stringify(obj),
        contentType: "application/json"
    })
        .done(function (result) {
            Swal.fire('Saved!', 'You updated data', 'success').then(() => {
                location.reload();
            });
        })
        .fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            });
        });
}

function GetById(updateId) {
    $.ajax({
        url: "https://localhost:7239/api/employees/" + updateId,
        type: "GET",
        dataType: "JSON"
    })
        .done(response => {
            console.log(response);
            let formatBirthDate = moment(response.data.birthDate).format("yyyy-MM-DD");
            let formatHiringDate = moment(response.data.hiringDate).format("yyyy-MM-DD");

            $("#guidUpdate").val(response.data.guid);
            $("#nikUpdate").val(response.data.nik);

            if (response.data.gender === 0) {
                $("input[name='updateGender'][value='Female']").prop('checked', true);
            } else {
                $("input[name='updateGender'][value='Male']").prop('checked', true);
            }

            $("#formUpdateNamaDepan").val(response.data.firstName);
            $("#formUpdateNamaBelakang").val(response.data.lastName);
            $("#formUpdateEmail").val(response.data.email);
            $("#formUpdateNomorHP").val(response.data.phoneNumber);
            $("#formUpdateTanggalLahir").val(formatBirthDate);
            $("#formUpdateTanggalMasuk").val(formatHiringDate);
        })
        .fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            });
        });
}

function GetAll() {
    $.ajax({
        url: "https://localhost:7239/api/employees/",
        type: "GET",
        dataType: "JSON"
    })
        .done(response => {
            console.log(response);
            let maleCount = 0;
            let femaleCount = 0;

            for (let i = 0; i < response.data.length; i++) {
                if (response.data[i].gender === 1) {
                    maleCount++;
                } else if (response.data[i].gender === 0) {
                    femaleCount++;
                }
            }

            let totalCount = maleCount + femaleCount;

            let malePercentage = (maleCount / totalCount) * 100;
            let femalePercentage = (femaleCount / totalCount) * 100;

            let ctx = document.getElementById('genderChart').getContext('2d');
            let genderChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: ['Female', 'Male'],
                    datasets: [{
                        data: [femalePercentage, malePercentage],
                        backgroundColor: ['#FF6384', '#36A2EB'],
                        hoverBackgroundColor: ['#FF6384', '#36A2EB']
                    }]
                },
                options: {
                    responsive: true,
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                let label = data.labels[tooltipItem.index];
                                let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                                return label + ': ' + value.toFixed(2) + '% (' + Math.round(value * totalCount / 100) + ')';
                            }
                        }
                    }
                }
            });
            $('#chartModal').modal('show');
        })
        .fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            });
        });
}

function showUniversityChart() {
    $.ajax({
        url: "https://localhost:7239/api/employees/get-all-master",
        type: "GET",
        dataType: "json"
    }).done(response => {
        let gpa4Count = 0;
        let gpaLess4Count = 0;

        for (let i = 0; i < response.data.length; i++) {
            if (response.data[i].gpa == 4) {
                gpa4Count++;
            } else if (response.data[i].gpa < 4) {
                gpaLess4Count++;
            }
        }

        let totalCount = gpa4Count + gpaLess4Count;

        let gpa4Percentage = (gpa4Count / totalCount) * 100;
        let gpaLess4Percentage = (gpaLess4Count / totalCount) * 100;

        let ctx = document.getElementById('universityChart').getContext('2d');
        let universityChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ['GPA 4', 'GPA kurang 4'],
                datasets: [{
                    label: 'GPA',
                    data: [gpa4Percentage, gpaLess4Percentage],
                    backgroundColor: ['#FF6384', '#36A2EB'],
                    hoverBackgroundColor: ['#FF6384', '#36A2EB']
                }]
            },
            options: {
                responsive: true,
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.labels[tooltipItem.index];
                            let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                            return label + ': ' + value.toFixed(2) + '% (' + Math.round(value * totalCount / 100) + ')';
                        }
                    }
                }
            }
        });
        $('#chartModal2').modal('show');
    }).fail(error => {
        alert("Failed to fetch data from API.");
    });
}

function Delete(deleteId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to recover this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
        $.ajax({
                url: "https://localhost:7239/api/employees?guid=" + deleteId,
                type: "DELETE",
        }).done((result) => {
            Swal.fire(
                'Deleted!',
                'Your data has been deleted.',
                'success'
            ).then(() => {
                location.reload();
            });
        }).fail((error) => {
            alert("Failed to delete data. Please try again.");
        });
        }
    });
}

$(document).ready(function () {
    // Menggunakan event handler untuk form submission
    $('#createForm').on('submit', function (event) {
        event.preventDefault(); // Mencegah perilaku default formulir

        // Panggil method insert di sini
        Insert();
    });
});

$(document).ready(function () {
    // Menggunakan event handler untuk form submission
    $('#updateForm').on('submit', function (event) {
        event.preventDefault(); // Mencegah perilaku default formulir

        Update();
    });
});