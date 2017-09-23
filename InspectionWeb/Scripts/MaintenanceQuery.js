$('#startDateRangePicker, #endDateRangePicker').daterangepicker({
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
    var startDate = '';
    var endDate = '';
    var sourceId = '';
    var abnormalid = '';
    var errorMsg = [1,1,1,1];
    $('#MaintenanceList_Item, #MaintenanceList_Experience, #MaintenanceList_Other').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "10%", "orderable": true },
            { "targets": 1, "width": "10%", "orderable": true },
            { "targets": 2, "width": "15%", "orderable": true },
            { "targets": 3, "width": "15%", "orderable": true },
            { "targets": 4, "width": "10%", "orderable": true },
            { "targets": 5, "width": "15%", "orderable": true },
            { "targets": 6, "width": "15%", "orderable": true },
            { "targets": 7, "width": "10%", "orderable": false }
        ],
    });

    $('#startDateRangePicker').on('apply.daterangepicker', function (ev, picker) {
        startDate = picker.startDate.format('YYYY-MM-DD HH:mm:ss');
        $('#endDateRangePicker').data('daterangepicker').minDate = picker.startDate;
        if (endDate == '' || endDate < startDate) {
            $('#endDateRangePicker').data('daterangepicker').setStartDate(startDate);
            $('#endDateRangePicker').data('daterangepicker').setEndDate(startDate);
            endDate = startDate;
        }
        else {
            $('#endDateRangePicker').data('daterangepicker').setStartDate(endDate);
            $('#endDateRangePicker').data('daterangepicker').setEndDate(endDate);
        }
        errorMsg[0] = 0;
        errorMsg[1] = 0;
    });

    $('#endDateRangePicker').on('apply.daterangepicker', function (ev, picker) {
        endDate = picker.startDate.format('YYYY-MM-DD HH:mm:ss');
        errorMsg[1] = 0;
    });
    $('#startDateRangePicker').on('change', function (ev) {
        startDate = $(this).val();
        if (startDate == '')
            errorMsg[0] = 1;
        else
            errorMsg[0] = 0;
    });
    $('#endDateRangePicker').on('change', function (ev) {
        endDate = $(this).val();
        if (endDate == '')
            errorMsg[1] = 1;
        else
            errorMsg[1] = 0;
    });

    $('#reportSource').on('change', function (ev) {
        sourceId = $(this).val();
        if (sourceId == '')
            errorMsg[2] = 1;
        else
            errorMsg[2] = 0;
    });

    $('#abnormalDefinition').on('change', function (ev) {
        abnormalId = $(this).val();
        if (abnormalId == '')
            errorMsg[3] = 1;
        else
            errorMsg[3] = 0;
    });

    $('#query').on('click', function (ev) {
        if ($('#endDateRangePicker').val() == '')
            alert('000');
        var error = 0;
        for (; error < errorMsg.length; error++)
            if (errorMsg[error] != 0)
                break;
        switch (error) {
            case 0:
                alert('請選擇開始日期!');
                break;
            case 1:
                alert('請選擇結束日期!');
                break;
            case 2:
                alert('請選擇通報來源!');
                break;
            case 3:
                alert('請選擇異常種類!');
                break;
           default:
                chickQuery();
        }
    });
    
    function chickQuery() {
        $('#MaintenanceList_Item').DataTable().clear().draw();
        $('#MaintenanceList_Experience').DataTable().clear().draw();
        $('#MaintenanceList_Other').DataTable().clear().draw();
        $.get(
            'GetQuery',
            {
                startDate: startDate,
                endDate: endDate,
                sourceId: sourceId,
                abnormalId: abnormalId
            },
            function (data) {
                if (data.Success) {
                    for (var item in data.vms) {
                        if (data.vms[item].listName == "Item")
                            addlist('Item', data.vms[item]);
                        else if (data.vms[item].listName == "Experience")
                            addlist('Experience', data.vms[item]);
                        else
                            addlist('Other', data.vms[item]);
                    }
                    checklist();
                } else {
                    nofind('Item');
                    nofind('Experience');
                    nofind('Other');
                }
            },
            'json'
        ).fail(function (e) {
            console.log(e);
            alert('抱歉! 伺服器無法接受您的查詢!');
        });
    };

    function addlist(listName, data) {
        $('#' + listName + 'ErrDiv').css('display', 'none');
        $('#' + listName + 'Table').DataTable().clear().draw();
        str = '<a href="http://' + document.domain + ':' + location.port + '/ReportJob/ItemDetailedData/' + data.recordId +
            '" class="btn btn-sm btn-primary"><i class="glyphicon glyphicon-menu-hamburger"></i></a>';
        $('#MaintenanceList_' + listName).DataTable().row.add([
            data.sourceName,
            data.roomName,
            data.itemName,
            data.description,
            data.deviceId,data.happendedTime,
            data.fixDate,
            str
        ]).draw(false);
    };

    function nofind(listName) {
        $('#' + listName + 'Table').DataTable().clear().draw();
        $('#' + listName + 'ErrDiv').css('display', 'block');
    }

    function checklist() {
        if ($('#MaintenanceList_Item td').length == 1)
            nofind('Item'); 
        if ($('#MaintenanceList_Experience td').length == 1)
            nofind('Experience');
        if ($('#MaintenanceList_Other td').length == 1)
            nofind('Other');
    }
});