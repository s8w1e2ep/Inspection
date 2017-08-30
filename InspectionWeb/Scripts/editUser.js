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
                location.reload();
            } else {
                alert('變更密碼失敗');
            }
        },
        'json'
    ).fail(function () {
        alert('變更密碼失敗');
    });
}

$(document).ready(function () {
    // x-editable for user table
    $('#userName, #email, #tel').editable({
        success: function (response) {
            $('#last').html(response.lastUpdateTime);
        }
    });

    $('#agent').editable({
        value: 'Select status',
        source: [
            { value: 1, text: 'Agent - 1' },
            { value: 2, text: 'Agent - 2' },
            { value: 3, text: 'Agent - 3' }
        ],
        success: function (response) {
            $('#last').html(response.lastUpdateTime);
        }
    });

    $('#groupId').editable({
        value: 'Select status',
        source: [
            { value: 1, text: 'Group - 1' },
            { value: 2, text: 'Group - 2' },
            { value: 3, text: 'Group - 3' }
        ],
        success: function (response) {
            $('#last').html(response.lastUpdateTime);
        }
    });

    $('#active').editable({
        value: ['@Model.active'],
        source: {
            1: 'Yes',
        },
        emptytext: 'No',
        params: function (params) {
            console.log(params);
            var data = {};
            data['name'] = params.name;
            data['pk'] = params.pk;
            data['value'] = params.value.join(",");
            console.log(data);

            return data;
        },
        success: function () {
            location.reload();
        }
    });

    $('#modalYes2').click(function () {
        newPassword = $.trim($('#newPassword').val());
        repeatPassword = $.trim($('#repeatPassword').val());

        if (newPassword === '' || repeatPassword === '') {
            alert('密碼不能為空白!');
            return;
        }
        if (newPassword === repeatPassword) {
            changePassword();
        } else {
            alert('新密碼輸入錯誤!');
        }
    });

    $('#modalYes2').click(function () {
        newPassword = $.trim($('#newPassword').val());
        repeatPassword = $.trim($('#repeatPassword').val());

        if (newPassword === '' || repeatPassword === '') {
            alert('密碼不能為空白!');
            return;
        }
        if (newPassword === repeatPassword) {
            changePassword();
        } else {
            alert('新密碼輸入錯誤!');
        }
    });

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
                } else {
                    alert('變更密碼失敗');
                }
            },
            'json'
        ).fail(function () {
            alert('變更密碼失敗');
        });
    }

    function setSwitchEditable(tag, state) {
        $(tag).editable({
            value: [state],
            source: { 1: 'Yes' },
            emptytext: 'No',

            params: function (params) {
                var data = {};
                data['name'] = params.name;
                data['key'] = params.pk;
                data['value'] = params.value.join(",");

                return data;
            }
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
            ]
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
            ]
        });
    }

});
