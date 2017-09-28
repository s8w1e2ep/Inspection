var toDeletesolutionId;

function deleteUser() {
    $.ajax({
        type: "DELETE",
        url: 'DeleteSolution',
        data: $.param({ "solutionId": toDeletesolutionId })
    }).done(function (msg) {
        $("#" + toDeletesolutionId).remove();
    });
}

// 設定刪除的 solutionId
function setId(solutionId) {
    toDeletesolutionId = solutionId;
}


$(document).ready(function () {
    $('#SolutionTable').DataTable({
        "paging": false,
        "info": false,
        "searching": true,
        "columnDefs": [
            { "targets": 0, "width": "25%", "orderable": true },
            { "targets": 1, "width": "55%", "orderable": true },
            { "targets": 2, "width": "10%", "orderable": true },
            { "targets": 3, "width": "10%", "orderable": false }
        ]
    });

    // delete dialog 確定事件
    $('#modalYes').click(function () {
        deleteUser();
    });
});