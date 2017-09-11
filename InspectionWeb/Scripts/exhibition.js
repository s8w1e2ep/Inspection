var startDate = null;
var endDate = null;

$(document).ready(function () {
    //$('#dispatchTable').DataTable({
    //    "paging": true,
    //    "info": false,
    //    "searching": true,
    //    "columnDefs": [
    //        { "targets": 0, "width": "25%", "orderable": true },
    //        { "targets": 1, "width": "25%", "orderable": true },
    //        { "targets": 2, "width": "25%", "orderable": true },
    //        { "targets": 3, "width": "25%", "orderable": true }
    //    ]
    //});

    //$('#autoTable').DataTable({
    //    "paging": true,
    //    "info": false,
    //    "searching": true,
    //    "columnDefs": [
    //        { "targets": 0, "width": "25%", "orderable": true },
    //        { "targets": 1, "width": "25%", "orderable": true },
    //        { "targets": 2, "width": "25%", "orderable": true },
    //        { "targets": 3, "width": "25%", "orderable": true }
    //    ]
    //});

    // single Date picker
    $('#startDatePicker, #endDatePicker').datepicker({
        singleDatePicker: true,
        language: 'zh-TW',
        format: 'yyyy/mm/dd'
    }).on('changeDate', function (ev) {
        startDate = $('#startDatePicker').val();
        endDate = $('#endDatePicker').val();
        console.log(startDate);
        console.log(endDate);
    });
});

function query() {
    if (startDate === null || startDate === "") {
        alert('請選擇開始日期!');
    } else if (endDate === null || endDate === "") {
        alert('請選擇結束日期!');
    } else if (endDate < startDate) {
        alert('開始日期不能比結束日期晚');
    } else {
        setDispatchTable('man');
        setDispatchTable('auto');
    } 
}

function setDispatchTable(type) {
    if ($.fn.dataTable.isDataTable('#' + type + 'Table')) {
        var table = $('#' + type + 'Table').DataTable();
        table.ajax.reload();
    } else {
        $('#' + type + 'Table').DataTable({
            "paging": true,
            "info": false,
            "searching": true,
            "columnDefs": [
                { "targets": 0, "width": "25%", "orderable": true },
                { "targets": 1, "width": "25%", "orderable": true },
                { "targets": 2, "width": "25%", "orderable": true },
                { "targets": 3, "width": "25%", "orderable": true }
            ],
            "ajax": {
                "url": '@Url.Content("~/Statistic/QueryManDispatch")',
                //"type": 'POST',
                "dataType": "json",
                "data": function (d) {
                    d.roomId = $('#exhibitionSelect').val();
                    d.start = startDate;
                    d.end = endDate;
                    d.type = type;
                }
            },
            "columns": [
                { "data": "日期" },
                { "data": "巡檢項目個數" },
                { "data": "正常個數" },
                { "data": "正常率" }
            ]
        });
    }
}