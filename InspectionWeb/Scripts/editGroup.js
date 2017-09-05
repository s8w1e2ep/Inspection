function setSwitchEditable(tag, state) {
    $(tag).editable({
        value: [state],
        source: { 1: '是' },
        emptytext: '否',
        params: function (params) {
            var data = {};
            data['name'] = params.name;
            data['pk'] = params.pk;
            data['value'] = params.value.join(",");

            return data;
        },
        success: function (response) {
            $('#last').html(response.lastUpdateTime);
        }
    });
}

$.fn.editable.defaults.mode = 'inline';

$(document).ready(function () {
    // x-editable for user table
    $('#groupName').editable();

    setSwitchEditable('#superUserOnly', '@Model.superUser');
    setSwitchEditable('#systemManagement', '@Model.system');
    setSwitchEditable('#userManagement', '@Model.user');
    setSwitchEditable('#dispatchManagement', '@Model.dispatch');
    setSwitchEditable('#normalUser', '@Model.normal');
});