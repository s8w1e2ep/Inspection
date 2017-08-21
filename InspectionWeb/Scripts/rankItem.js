$(document).ready(function () {
    $('#fieldTable').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "columnDefs": [
            { "targets": 0, "width": "10%", "orderable": true },
            { "targets": 1, "width": "10%", "orderable": true },
            { "targets": 2, "width": "15%", "orderable": true },
            { "targets": 3, "width": "15%", "orderable": true },
            { "targets": 4, "width": "15%", "orderable": true },
            { "targets": 5, "width": "15%", "orderable": true },
            { "targets": 6, "width": "20%", "orderable": false }
        ],
    });
});