$(document).ready(function () {
    $('#fieldTable').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "columnDefs": [
            { "targets": 0, "width": "20%", "orderable": true },
            { "targets": 1, "width": "20%", "orderable": true },
            { "targets": 2, "width": "40%", "orderable": true },
            { "targets": 3, "width": "20%", "orderable": false }
        ],
    });

    // single Date picker
    $('#startDatePicker, #endDatePicker').daterangepicker({
        singleDatePicker: true,
        format: 'YYYY/MM/DD'
    }).on('apply.daterangepicker', function (ev, picker) {
        // todo
        //beaconAlarmDateStart = picker.startDate.format('YYYY/MM/DD');
        //beaconAlarmDateEnd = picker.endDate.format('YYYY/MM/DD');
        //console.log(scheduleDateStart);
        //console.log(scheduleDateEnd);
    });
});