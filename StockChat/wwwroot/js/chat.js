"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Home/Index").build();

var roomId = document.getElementById("roomid");

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

var scrollMessages = function () {
    const element = document.getElementById("chatroom");
    element.scrollTop = element.scrollHeight;
}

window.onload = function () {
    scrollMessages();
}

connection.on("ReceiveMessage", function (user, message) {
    let msg = new Message(user, message, new Date().toLocaleString());
    addMessage(msg);

    scrollMessages();
});

connection.start().then(function () {
    connection.invoke("JoinGroup", roomId.value);
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

var sendMessage = function (event) {
    var user = document.getElementById("username").value;
    var message = document.getElementById("messageInput");

    $.ajax({
        url: '/Home/PostMessage',
        type: "POST",
        data: { message: message.value, roomId: roomId.value }
    })

    if (message.value[0] != '/') {
        connection.invoke("SendMessage", user, message.value, roomId.value).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        addMessage(new Message(user, message.value, new Date().toLocaleString()));
        scrollMessages();
    }
    
    message.value = "";
    event.preventDefault();
}

document.getElementById("sendButton").addEventListener("click", sendMessage);

function addMessage(message) {
    let chat = document.getElementById("messageContainer");

    let container = document.createElement('li');
    container.className = "container";

    let header = document.createElement('div');
    header.className = "d-flex";

    let userEl = document.createElement('p');
    userEl.className = "small mb-1 mr-1";
    userEl.innerHTML = message.UserName;

    let timeStampEl = document.createElement('p');
    timeStampEl.className = "small mb-1 text-muted";
    timeStampEl.innerHTML = message.TimeStamp;

    let msgContainer = document.createElement('div');
    msgContainer.className = "d-flex flex-row justify-content-start";
    let msgDiv = document.createElement('div');

    let msg = document.createElement("p");
    msg.className = "small p-2 rounded-pill";
    msg.style = "background-color: #f5f6f7;";
    msg.innerHTML = message.Post;

    header.appendChild(userEl);
    header.appendChild(timeStampEl);
    msgContainer.appendChild(msgDiv).appendChild(msg);

    container.appendChild(header);
    container.appendChild(msgContainer);

    chat.appendChild(container);

}

document.getElementById("messageInput").addEventListener("keydown", function (e) {

    if (e.keyCode == 13 && $(this).val().trim() != "") {
        sendMessage(e);
    }

});

class Message {
    constructor(UserName, Post, TimeStamp) {
        this.UserName = UserName;
        this.Post = Post;
        this.TimeStamp = TimeStamp;
    }
}