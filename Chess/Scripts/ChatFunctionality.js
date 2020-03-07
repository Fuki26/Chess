$(function () {
    $('#displayname').val(prompt('Enter your name:', ''));

    var chat = $.connection.chatHub;
    chat.client.broadcastMessage = appendMessageToDiscussion;

    $('#message').focus();
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(sendMessageToServer);
        $('#message').keypress((event) => {
            if (event.which == 13 || event.keyCode == 13) {
                sendMessageToServer();
            }
        });
    })

    function appendMessageToDiscussion(name, message) {

        var senderName = $('#displayname').val();
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();

        var alignClass = "float-right";
        if (senderName === encodedName) {
            alignClass = "float-left";
        }

        $('#discussion').append('<li><div class="' + alignClass + '"><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' +
            encodedMsg + '</div></li>');
    }

    function sendMessageToServer() {
        var senderName = $('#displayname').val();
        var message = $('#message').val();
        chat.server.send(senderName, message);
        $('#message').val('').focus();
    }
});