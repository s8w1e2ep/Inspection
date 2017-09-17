$(document).ready(function () {
    // 顯示錯誤提醒 
    function fieldValidation(field) {
        if ($.trim($(field).val()) == '') {
            $(field + 'Div').addClass('has-error');
            $(field + 'Div').val('');
            return false;
        } else {
            $(field + 'Div').removeClass('has-error');
        }
        return true;
    }
    // 驗證表單資料是否錯誤
    function formValidation() {
        if (fieldValidation('#account') && fieldValidation('#password') && fieldValidation('#repeatPassword')) {
            var password = $('#passwordDiv input').val();
            var repeatPassword = $('#repeatPasswordDiv input').val();
            if (password == repeatPassword) {
                return true;
            } else {
                $('#repeatPasswordDiv').addClass('has-error');
            }
        }
        return false;
    }
});