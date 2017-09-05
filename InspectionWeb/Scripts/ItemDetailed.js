$(document).ready(function () {

    $('#close').editable({
        value: [''],
        source: {
            1: '是',
        },
        emptytext: '否',
        params: function (params) {
            var data = {};
            data['name'] = params.name;
            data['pk'] = params.pk;
            data['value'] = params.value.join(",");

            return data;
        },
        //success: function (response) {
        //    $('#last').html(response.lastUpdateTime);
        //}
    });


    $('#addiDescription').editable();


    //select date and time
    $('#HappenTime, #FinishFixTime').daterangepicker({
        timePicker: true,
        timePickerSeconds: true,
        singleDatePicker: true,
        timePickerIncrement: 1,
        format: 'YYYY/MM/DD HH:mm:ss'
    });
});