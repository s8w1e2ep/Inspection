/*
function setSwitchEditable(tag, state) {
    $(tag).editable({
        value: [state],
        source: { 1: 'Yes' },
        emptytext: 'No',

        params: function (params) {
            var data = {};
            data['name'] = params.name;
            data['pk'] = params.pk;
            data['value'] = params.value.join(",");

            return data;
        },
    });
}
*/
function setSourceCodeEditable() {
    $('#sourceCode').editable({
        ajaxOptions: {
            //dataType: 'json' //assuming json response
        },
        value: '00',
        source: [
            "00 - 其它",
            "01 - 巡檢填單",
            "02 - 6000填單",
            "03 - NM",
            "04 - 通報按鈕",
            "05 - 監控軟體",
            "06 - app"
        ],
    });
}

function setIsEndEditable() {
    $('#isEnd').editable({
        ajaxOptions: {
            //dataType: 'json' //assuming json response
        },
        value: '否',
        source: [
            "是",
            "否"
        ],
    });
}

$.fn.editable.defaults.mode = 'inline';

$(document).ready(function () {
    // x-editable for user table
    $('#itemName').editable();
    $('#roomName').editable();
    $('#caption').editable();
    $('#createTime').editable();
    $('#description').editable();
    $('#lastUpdateTime').editable();
    $('#solution').editable();

    //setSwitchEditable('#active', '0');
    setSourceCodeEditable();
    setIsEndEditable();

});