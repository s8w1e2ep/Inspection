$('i[name="DateRangePicker"]').daterangepicker({
    singleDatePicker: true,
    timePicker: true,
    timePicker12Hour: false,
    timePickerSeconds: true,
    timePickerIncrement: 1,
    autoApply: true,
    format: 'YYYY-MM-DD HH:mm:ss',
    locale: {
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

$(document).ready(function () {

    $('#PendingList').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "10%", "orderable": true },
            { "targets": 1, "width": "10%", "orderable": true },
            { "targets": 2, "width": "15%", "orderable": true },
            { "targets": 3, "width": "15%", "orderable": true },
            { "targets": 4, "width": "15%", "orderable": true },
            { "targets": 5, "width": "15%", "orderable": true },
            { "targets": 6, "width": "10%", "orderable": false }
        ],
    });

    $('i[name="DateRangePicker"]').on('apply.daterangepicker', function (ev, picker) {
        recordId = $(this).attr("data-pk");
        changedatetime(picker.startDate.format('YYYY-MM-DD HH:mm:ss'));
    });
    
    function changedatetime(date) {
        var newdatetime = date;
        $.post(
            'ExtendedWorkUpdateDate',
            {
                recordId: recordId,
                fixDate: newdatetime
            },
            function (data) {
                if (data.Success) {
                    alert('變更預估完工日期成功');
                    $('#' + recordId).html(newdatetime);
                } else {
                    alert('變更預估完工日期失敗');
                }
            },
            'json'
        ).fail(function (e) {
            console.log(e);
            alert('變更預估完工日期失敗');
        });
    }

});