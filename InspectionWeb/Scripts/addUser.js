$(document).ready(function () {
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