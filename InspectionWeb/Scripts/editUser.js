var newPassword;
var repeatPassword;

//function changePassword() {
//    var originalPassword = $('#originalPassword').val();

//    $.post('@Url.Content("~/User/changePassword/")',
//        { userId: '@Model.userId', originalPassword: originalPassword, newPassword: newPassword, repeatPassword: repeatPassword },
//        function (data) {
//            if (data.result) {
//                alert('變更密碼成功');
//            }
//        },
//        'json'
//    ).fail(function () {
//        alert('變更密碼失敗');
//    });
//}


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

function setAgentEditable() {
    $('#agent').editable({
        ajaxOptions: {
            //dataType: 'json' //assuming json response
        },
        value: '',
        source: [
            "Agent - 1",
            "Agent - 2",
            "Agent - 3" 
        ],
    });
}

function setGroupEditable() {
    $('#group').editable({
        ajaxOptions: {
            //dataType: 'json' //assuming json response
        },
        value: '',
        source: [
            "Group - 1",
            "Group - 2",
            "Group - 3"
        ],
    });
}

$.fn.editable.defaults.mode = 'inline';

$(document).ready(function () {
    // x-editable for user table
    $('#name').editable();
    $('#email').editable();
    $('#phone').editable();

    setSwitchEditable('#active', '0');
    setAgentEditable();
    setGroupEditable();

    // get field delete modal result
    //$('#changePasswordModal .modal-footer button').on('click', function (e) {
    //    if ($(this).index() == 1) {
    //        newPassword = $.trim($('#newPassword').val());
    //        repeatPassword = $.trim($('#repeatPassword').val());
    //        if (newPassword == '' || repeatPassword == '') {
    //            alert('密碼不能為空白!');
    //            return;
    //        }
    //        if (newPassword == repeatPassword) {
    //            changePassword();
    //        } else {
    //            alert('新密碼輸入錯誤!');
    //        }
    //    }
    //});
});