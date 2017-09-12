$(function () {

    $('#ReportList_Item, #ReportList_Experience, #ReportList_Other').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "15%", "orderable": true },
            { "targets": 1, "width": "15%", "orderable": true },
            { "targets": 2, "width": "15%", "orderable": true },
            { "targets": 3, "width": "15%", "orderable": true },
            { "targets": 4, "width": "20%", "orderable": true },
            { "targets": 5, "width": "10%", "orderable": true },
            { "targets": 6, "width": "10%", "orderable": false }
        ],
    });
    //select date and time
    $('#dateAndTime1, #dateAndTime2').daterangepicker({
        timePicker: true,
        timePickerSeconds: true,
        singleDatePicker: true,
        timePickerIncrement: 1,
        format: 'YYYY/MM/DD HH:mm:ss'
    });

    

    
});