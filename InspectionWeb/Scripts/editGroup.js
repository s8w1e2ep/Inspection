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

$.fn.editable.defaults.mode = 'inline';

$(document).ready(function () {
    // x-editable for user table
    $('#name').editable();

    setSwitchEditable('#system', '1');
    setSwitchEditable('#manager', '0');
    setSwitchEditable('#general', '0');
    setSwitchEditable('#inspect', '0');
    setSwitchEditable('#active', '0');
});