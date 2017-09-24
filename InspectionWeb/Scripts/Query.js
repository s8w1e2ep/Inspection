var startDate = null;
var endDate = null;

$(document).ready(function () {

    $('#itemTable').DataTable({
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
        ]
    });
    $('#expTable').DataTable({
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
        ]
    });
    $('#otherTable').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "15%", "orderable": true },
            { "targets": 1, "width": "25%", "orderable": true },
            { "targets": 2, "width": "20%", "orderable": true },
            { "targets": 3, "width": "20%", "orderable": true },
            { "targets": 4, "width": "10%", "orderable": true },
            { "targets": 5, "width": "10%", "orderable": false }

        ]
    });

    // single Date picker
    $('#startTime, #endTime').datepicker({
        language: 'zh-TW',
        format: 'yyyy/mm/dd'
    }).on('changeDate', function (ev) {
        startDate = $('#startTime').val();
        endDate = $('#endTime').val();
        $('.datepicker').hide();
    });
});

function query() {

    if (startDate === null || startDate === "") {
        alert('請選擇開始日期!');
    }
    else if (endDate === null || endDate === "") {
        alert('請選擇結束日期!');
    }
    else if (endDate < startDate) {
        alert('結束日期不能比開始日期早');
    }
    else {
        setInspectTable('item');
        setInspectTable('exp');
        setInspectTable('other');
    }


}
function setInspectTable(type) {

    $.post(
        '/ReportJob/GetQuery',
        {
            sourceId: $('#reportSource').val(),
            abnormalId: $('#abnormal').val(),
            isClose: $('#Close').val(),
            startDate: startDate,
            endDate: endDate,
            type: type
        },
        function (data) {
            if (data.length !== 0) {
                var str;

                $('#' + type + 'ErrDiv').css('display', 'none');
                $('#' + type + 'Table').DataTable().clear().draw();
                str = '<a href="' + 'ItemDetailedData' + '" class="btn btn-sm btn-primary"><i class="glyphicon glyphicon-menu-hamburger"></i></a>';

                for (var i = 0; i < data.length; i++) {
                    str = '<a href="ItemDetailedData/' + data[i].recordId + '" class="btn btn-sm btn-primary"><i class="glyphicon glyphicon-menu-hamburger"></i></a>';
                    if (type !== 'other') {
                        $('#' + type + 'Table').DataTable().row.add([
                            data[i].sourceName,
                            data[i].roomName,
                            data[i].itemName,
                            data[i].happenedTime,
                            data[i].fixDate,
                            data[i].isClose,
                            str
                        ]).draw();
                    }
                    else {
                        $('#otherTable').DataTable().row.add([
                            data[i].sourceName,
                            data[i].itemName,
                            data[i].happenedTime,
                            data[i].fixDate,
                            data[i].isClose,
                            str
                        ]).draw();
                    }
                }
            }
            else {
                $('#' + type + 'Table').DataTable().clear().draw();
                $('#' + type + 'ErrDiv').css('display', 'block');
            }
        },
        'json'
    ).fail(function () {
        //console.log(e);
        alert("抱歉! 伺服器無法接受您的查詢!");
    });
}