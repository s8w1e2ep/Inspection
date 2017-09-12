var toDeleteGroupId;

function deleteGroup() {
    $.ajax({
        type: "DELETE",
        url: 'DeleteGroup',
        data: $.param({ "groupId": toDeleteGroupId })
    }).done(function (msg) {
        $("#" + toDeleteGroupId).remove();
    });
}

// 設定刪除的 groupId
function setId(groupId) {
    toDeleteGroupId = groupId;
}

$(document).ready(function () {
    $('#groupTable').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "10%", "orderable": true },
            { "targets": 1, "width": "10%", "orderable": true },
            { "targets": 2, "width": "10%", "orderable": true },
            { "targets": 3, "width": "10%", "orderable": true },
            { "targets": 4, "width": "10%", "orderable": true },
            { "targets": 5, "width": "10%", "orderable": true },
            { "targets": 6, "width": "15%", "orderable": true },
            { "targets": 7, "width": "15%", "orderable": true },
            { "targets": 8, "width": "10%", "orderable": false }
        ]
    });

    // delete dialog 確定事件
    $('#modalYes').click(function () {
        deleteGroup();
    });
});