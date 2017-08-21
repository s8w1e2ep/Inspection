﻿$(document).ready(function () {
    $('#fieldTable').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "columnDefs": [
            { "targets": 0, "width": "25%", "orderable": true },
            { "targets": 1, "width": "25%", "orderable": true },
            { "targets": 2, "width": "25%", "orderable": true },
            { "targets": 3, "width": "25%", "orderable": true }
        ],
    });

    $('#fieldTable2').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "columnDefs": [
            { "targets": 0, "width": "25%", "orderable": true },
            { "targets": 1, "width": "25%", "orderable": true },
            { "targets": 2, "width": "25%", "orderable": true },
            { "targets": 3, "width": "25%", "orderable": true }
        ],
    });
});