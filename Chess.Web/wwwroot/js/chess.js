"use strict";

$("div").draggable({
    containment: "#chessBoard",
    revert: "invalid",
    scroll: false,
    cursor: "move",
    start: function (event, ui) {
        x = ui.offset.top;
        y = ui.offset.left;
    },
    drag: function () {
    },
    stop: function (event, ui) { }
});

$("td").droppable({
    hoverClass: "ui-state-active",
    drop: function (event, ui) {

        var value = {
            FigureId: ui.draggable[0].id,
            TargetId: event.target.id,
            TargetRow: event.target.id.substr(0, 1),
            TargetCol: event.target.id.substr(1, 1)
        }

        $.ajax({
            url: '@Url.Action("MoveFigure", "Home")',
            type: "POST",
            data: value,
            success: function (str) {
                var result = str.split(';');
                if (result[0] == "True") {
                    counter++;
                    if (counter % 2 == 0) {
                        EnableWhiteFigures();
                    } else {
                        EnableBlackFigures();
                    }
                    if (result[1]) {
                        $("#" + result[1]).hide("explode", 1000);
                        $("#" + result[1]).remove();
                    }
                } else {
                    $("#" + ui.draggable[0].id).offset({ top: x, left: y })
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // TODO: Here is the place the errors should be handled properly
            }
        });
    }
})


$('#username').val(prompt('Enter your name:', ''));

var connection = new signalR.HubConnectionBuilder().withUrl("/chessHub").build();

connection.on("Chess", function (board) {
    // Here the result from the server is received
    var debug = -1;
});

connection.start().then(function () {
    console.log("Connection is established");
});

var dataToSend = board;
connection.invoke("SendMessage", dataToSend).catch(err => console.error(err.toString()));