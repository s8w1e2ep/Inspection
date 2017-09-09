$(document).ready(function () {
    $('#SolutionTable').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "70%", "orderable": true },
            { "targets": 1, "width": "20%", "orderable": true },
            { "targets": 2, "width": "10%", "orderable": false },
            
        ],
    });

    $('#myModal .modal-footer button').on('click', function (e) {
        var $target = $(e.target);
        if ($target.index() == 1) {
            deleteUser();
        }
    });
});