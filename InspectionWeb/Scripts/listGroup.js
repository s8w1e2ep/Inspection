$(document).ready(function () {
    $('#userTable').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "10%", "orderable": true },
            { "targets": 1, "width": "10%", "orderable": true },
            { "targets": 2, "width": "10%", "orderable": true },
            { "targets": 3, "width": "10%", "orderable": true },
            { "targets": 4, "width": "10%", "orderable": true },
            { "targets": 5, "width": "10%", "orderable": true },
            { "targets": 6, "width": "15%", "orderable": true },
            { "targets": 7, "width": "15%", "orderable": true },
            { "targets": 8, "width": "10%", "orderable": false }
        ],
    });

    $('#myModal .modal-footer button').on('click', function (e) {
        var $target = $(e.target);
        if ($target.index() == 1) {
            deleteUser();
        }
    });
});