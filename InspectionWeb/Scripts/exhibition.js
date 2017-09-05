var beginDate = null;
var endDate = null;

$(document).ready(function () {
    $('#artificialTable').DataTable({
        "paging": true,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "25%", "orderable": true },
            { "targets": 1, "width": "25%", "orderable": true },
            { "targets": 2, "width": "25%", "orderable": true },
            { "targets": 3, "width": "25%", "orderable": true }
        ],
    });

    $('#autoTable').DataTable({
        "paging": true,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "25%", "orderable": true },
            { "targets": 1, "width": "25%", "orderable": true },
            { "targets": 2, "width": "25%", "orderable": true },
            { "targets": 3, "width": "25%", "orderable": true }
        ],
    });

    // single Date picker
    $('#startDatePicker, #endDatePicker').datepicker({
        singleDatePicker: true,
        language: 'zh-TW',
        format: 'yyyy/mm/dd'
    }).on('changeDate', function (ev) {
        beginDate = $('#startDatePicker').val();
        endDate = $('#endDatePicker').val();
        console.log(beginDate);
        console.log(endDate);
    });
});

function query() {
    if (beginDate == null || endDate == null || beginDate == "" || endDate == "") {
        alert('error1');
    } else if (endDate < beginDate) {
        alert('error2');
    }
}