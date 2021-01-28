$(window).on('scroll', function () {
    if ($(this).scrollTop() > 100) {
        if (document.getElementById("chat").style.display != "block") {
            $('#chat-btn').fadeIn();
        }
    } else {
        $('#chat-btn').fadeOut();
    }
});

var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();
let currentUserName = "";

connection.on("receiveMessages", showMessages);

connection.on("deleteMessage", showMessages);

connection.on("onError", errorHandler);

connection.on("showMessages", showMessages);

function errorHandler(message) {
    const errorMessage = document.getElementById('error-message');
    errorMessage.style = 'display: block';
    errorMessage.textContent = message;
}

function showMessages(messages) {
    document.getElementById('error-message').style = 'display: none';

    $("#messagesList").empty();

    messages.forEach(message => {
        const createdOn = convertUTCDateToLocalDate(new Date(message.createdOn));

        let chatInfo;
        if (message.userUsername === currentUserName) {
            chatInfo =
                `
                <div class="ml-auto text-right">
                     <div class="msg w-100">
                        <i class="fas fa-trash trash" style="font-size:11px;" type="button" onclick="deleteMessage(${message.id})"></i>
                        ${message.content}</br>
                        <small class="text-muted ml-auto text-right" style="font-size:9px">${createdOn.toLocaleString()}</small>
                     </div>
                </div>           
                `;
        }
        else {
            chatInfo =
                `
                <div>
                    <div class="msg w-100">
                        [${message.userUsername}]: ${message.content}<br/>
                        <small class="text-muted" style="font-size:9px">${createdOn.toLocaleString()}</small>
                    </div>
                </div>         
                `;
        }

        $("#messagesList").append(chatInfo);
    })
}

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

$("#chat-btn").click(function () {
    connection.invoke("GetMessages");
    const username = $("#userUsername").val();
    setCurrentUser(username);
});

$("#sendButton").click(function () {
    var messageContent = $("#messageInput").val();

    connection.invoke("SendMessage", messageContent);
    document.getElementById('messageInput').value = '';
});

function deleteMessage(messageId) {
    connection.invoke("RemoveMessage", messageId);
}

const startSignalRConnection = connection => connection.start()
    .then(() => console.info('Websocket Connection Established'))
    .catch(err => console.error('SignalR Connection Error: ', err));

// re-establish the connection if it is dropped
connection.onclose(() => setTimeout(() => startSignalRConnection(connection), 5000));

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;
}

function setCurrentUser(username) {
    currentUserName = username;
}