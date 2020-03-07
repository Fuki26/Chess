var x, y;
var counter = 0;
var pieces;
var color = "";

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
$("div[data-color=Black]").draggable('disable');
$("td").droppable({
    hoverClass: "ui-state-active",
    drop: function (event, ui) {

        var value = {
            id: ui.draggable[0].id,
            target: event.target.id,
            col: event.target.id.substr(1, 1),
            row: event.target.id.substr(0, 1)
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
                        $("div[data-color=Black]").draggable('disable');
                        $("div[data-color=White]").draggable('enable');
                    } else {
                        $("div[data-color=Black]").draggable('enable');
                        $("div[data-color=White]").draggable('disable');
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