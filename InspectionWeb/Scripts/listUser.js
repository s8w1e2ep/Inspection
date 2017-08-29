var toDeleteUserId;

function deleteUser() {
    $.ajax({
        type: "DELETE",
        url: 'DeleteUser',
        data: $.param({ "userId": toDeleteUserId })
    }).done(function (msg) {
        $("#" + toDeleteUserId).remove();
    });
}

// 設定刪除的 userId
function setId(userId) {
    toDeleteUserId = userId;
}

$(document).ready(function () {
    // 使用者表格設定
    $('#userTable').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "15%", "orderable": true },
            { "targets": 1, "width": "15%", "orderable": true },
            { "targets": 2, "width": "15%", "orderable": true },
            { "targets": 3, "width": "15%", "orderable": true },
            { "targets": 4, "width": "15%", "orderable": true },
            { "targets": 5, "width": "15%", "orderable": true },
            { "targets": 6, "width": "10%", "orderable": false }
        ]
    });

    // delete dialog 確定事件
    $('#modalYes').click(function () {
        deleteUser();
    });
});