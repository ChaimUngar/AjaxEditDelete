
$(() => {

    const add = new bootstrap.Modal($("#add-modal")[0]);
    const edit = new bootstrap.Modal($("#edit-modal")[0]);

    function loadPeople() {
        $.get('/home/getpeople', function (people) {
            $("tbody tr").remove();
            people.forEach(person => {
                $("tbody").append(`
<tr data-id="${person.id}">
    <td>${person.id}</td>
    <td>${person.firstName}</td>
    <td>${person.lastName}</td>
    <td>${person.age}</td>
    <td>
        <button class="btn btn-info col-md-3">Edit</button>
        <button class="btn btn-danger col-md-3">Delete</button>
    </td>
</tr>
`);
            });
        });
    }

    loadPeople();

    $("#show-add").on('click', function () {
        $("#firstName").val('');
        $("#lastName").val('');
        $("#age").val('');
        add.show();
    });

    $("#save-person").on('click', function () {
        const firstName = $("#firstName").val();
        const lastName = $("#lastName").val();
        const age = $("#age").val();

        $.post('/home/addperson', { firstName, lastName, age }, function () {
            add.hide();
            loadPeople();
        });
    });

    $(".table").on('click', '.btn-info', function () {
        const tr = $(this).closest('tr')

        console.log(`edit guy, ${tr.data('id')}`)

        $.get(`/home/update?id=${tr.data('id')}`, function (person) {
            console.log(person.firstName)

            $("#firstName").val(person.firstName)
            $("#lastName").val(person.lastName);
            $("#age").val(person.age);
        })
        edit.show()
    })

    $("#update").on('click', function () {
        $.post('/home/update', { firstName, lastName, age }, function () {
            edit.hide()
            loadPeople()
        })
    })

    $(".table").on('click', '.btn-danger', function () {
        const tr = $(this).closest('tr')

        $.post(`/home/delete?id=${tr.data('id') }`, function () {
            loadPeople()
        })
    })
})
