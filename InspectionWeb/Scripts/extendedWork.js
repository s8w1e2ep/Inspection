
$('#startDatePicker, #endDatePicker').timepicker({
    minuteStep: 1,
    showSeconds: true,
    showMeridian: false,
    defaultTime: false
});

$('#startDatePicker, #endDatePicker').datepicker({
    singleDatePicker: true,
    language: 'zh-TW',
    "setDate": new Date(),
    format: 'yyyy-mm-dd'
});


$(document).ready(function () {

    // single Date picker

    var st = 1;
    var date = "";
    var time = "";
    var obj;
    var recordId;

    $('#startDatePicker, #endDatePicker').on('click', function (ev) {
        $(this).timepicker('hideWidget');
        $(this).datepicker('hide');
        recordId = $(this).attr("data-pk");
        if ($(this).val() == "")
            date = "";
        if (st == 1) {
            obj = $(this);
            $(this).datepicker('show');
            var tt = document.getElementsByClassName('table table-condensed')[0];
            if (tt.lastElementChild.className == "") {
                date = "";
                var text2 = document.createElement("tfoot");
                text2.className = "timebutton";
                text2.innerHTML = '<div class="col-md-8" ><tr><td colspan="7" ><i class="fa fa-clock-o" id="timebutton" ></i></td></tr></div>';
                tt.lastElementChild.after(text2);
            }
            $('#timebutton').on('click', function (ev) {
                if (date == "") {
                }
                else{
                    obj.datepicker('hide');
                    st = 0;
                    obj.timepicker('showWidget');
                }
            })
        }
        else {
            $(this).timepicker('showWidget');
        }
        //obj.val(date);
    })

    $('#startDatePicker, #endDatePicker').on('changeDate', function (ev) {
        date = $(this).val();
        $(this).val(date);
    });

    $('#startDatePicker, #endDatePicker').on('show.timepicker', function (e) {
        if (st == 0) {
            time = '00:00:00';
            $(this).val(date + " " + time);
        }
    });

    $('#startDatePicker, #endDatePicker').on('changeTime.timepicker', function (ev) {
        time = $(this).val();
    });
    $('#startDatePicker, #endDatePicker').on('hide.timepicker', function (ev) {
        if (st === 0) {
            $(this).val(date + " " + time);
            time = "";
            st = 1;
            changedatetime();
        }
    });

    function changedatetime() {
        var newdatetime = obj.val();
        $.post(
            'PendingList1',
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