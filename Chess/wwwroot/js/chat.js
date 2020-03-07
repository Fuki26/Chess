"use strict";


$('#displayname').val(prompt('Enter your name:', ''));

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendmessage").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + ": " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("discussion").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendmessage").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendmessage").addEventListener("click", function (event) {
    var user = $('#displayname').val();
    var message = document.getElementById("message").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});