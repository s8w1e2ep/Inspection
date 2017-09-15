$(function () {
    $(".select2").select2();

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
        format: 'YYYY/MM/DD HH:mm:ss',
        locale:{
            applyLabel: '確定',
            cancelLabel: '取消',
            fromLabel: '從',
            toLabel: '到',
            weekLabel: 'W',
            customRangeLabel: 'Custom Range',
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        }
    });

    

    
});