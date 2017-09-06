$(document).ready(function () {
    //let display window float obove the line
    $.fn.editable.defaults.mode = 'inline';
    $('#reportsouce').editable({
            value: 'Select status',
            source: [
                "其它",
                "巡檢填單",
                "6000填單",
                "NM",
                "通報按鈕",
                "監控軟體",
                "app"

                //@foreach(var item in Model.agents)
                //{
                //    <text>{value: '@item.userId', text: '@item.userCode' },</text>
                //}
            ],
            //success: function (response) {
            //    $('#last').html(response.lastUpdateTime);
            //}
    });
    $('#abnormal').editable({
        value: 'Select status',
        source: [
            
            "abnormal1",
            "abnormal2",
            "abnormal3",

            //@foreach(var item in Model.agents)
            //{
            //    <text>{value: '@item.userId', text: '@item.userCode' },</text>
            //}
        ],
        //success: function (response) {
        //    $('#last').html(response.lastUpdateTime);
        //}
    });

    $('#selectSolution').editable({
        value: 'Select status',
        source: [

            "abnormal1",
            "abnormal2",
            "abnormal3",
        ]
    });

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

    $('#HappenTime, #FinishFixTime').daterangepicker({
        timePicker: true,
        timePickerSeconds: true,
        singleDatePicker: true,
        timePickerIncrement: 1,
        format: 'YYYY/MM/DD HH:mm:ss'
    });

    //$('#HappenTime').editable();

    /////////////////
    $('#handleTime').editable({
        ajaxOptions: {
            dataType: 'json' //assuming json response
        },
        //success: function (response, newValue) {
        //    //console.log(newValue);
        //   // $('#HappenTime').spectrum('set', newValue);
        //    // change update time
        //    var date = new Date(response.lastUpdateTime * 1000);
        //    var lastUpdateTime = $.format.date(date, 'yyyy-MM-dd HH:mm:ss');
        //   // $('#lastUpdateTime').html(lastUpdateTime);
        //}
    });
    //$('#HappenTime').daterangepicker({
    //    color: '@Model.handheldColor',
    //    change: function (color) {
    //        // x-editable column changes to new value
    //        $('#handleTime').editable('setValue', color.toHexString());
    //        $('#handleTime').editable('submit', {
    //            ajaxOptions: {
    //                dataType: 'json' //assuming json response
    //            },
    //            success: function (response, newValue) {
    //                //$('#colorPicker').spectrum('set', newValue);
    //                // change update time
    //                var date = new Date(response.lastUpdateTime * 1000);
    //                var lastUpdateTime = $.format.date(date, 'yyyy-MM-dd HH:mm:ss');
    //                $('#lastUpdateTime').html(lastUpdateTime);
    //            }
    //        }); // will triger
    //    }
    //});


    //$('#HappenTime').editable({
    //    ajaxOptions: {
    //        //dataType: 'json' //assuming json response
    //    },
    //    success: function (response, newValue) {
            

    //        ////console.log(newValue);
    //        //$('#SelectTime1').spectrum('set', newValue);
    //        //// change update time
    //        //var date = new Date(response.lastUpdateTime * 1000);
    //        //var lastUpdateTime = $.format.date(date, 'yyyy-MM-dd HH:mm:ss');
    //        //$('#lastUpdateTime').html(lastUpdateTime);
    //    }
    //});

    //select date and time
    //$('#HappenTime, #FinishFixTime').daterangepicker({
    //    timePicker: true,
    //    timePickerSeconds: true,
    //    singleDatePicker: true,
    //    timePickerIncrement: 1,
    //    format: 'YYYY/MM/DD HH:mm:ss'
    //});
});