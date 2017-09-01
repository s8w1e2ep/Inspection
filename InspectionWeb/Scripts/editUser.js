var newPassword;
var repeatPassword;

$.fn.editable.defaults.mode = 'inline';

function changePassword() {
    var originalPassword = $('#originalPassword').val();
    $.post(
        '/User/EditPassword',
        {
            userId: userId,
            originalPassword: originalPassword,
            newPassword: newPassword,
            repeatPassword: repeatPassword
        },
        function (data) {
            if (data.Success) {
                alert('變更密碼成功');
                $('#last').html(data.lastUpdateTime);
            } else {
                alert('變更密碼失敗');
            }
        },
        'json'
    ).fail(function () {
        alert('變更密碼失敗');
    });
}
