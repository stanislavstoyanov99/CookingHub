var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();

document.getElementById("sendButton").disabled = true;

connection.on("receiveMessage", addMessageToChat);

connection.on("deleteMessage", showMessages);

connection.on("onError", errorHandler);

connection.on("showMessages", showMessages);

function addMessageToChat(message) {
    const sanitizedContent = message.content.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const createdOn = convertUTCDateToLocalDate(new Date(message.createdOn));
    const chatInfo =
        `
            <div>
                [${message.userUsername}] ${sanitizedContent}
                <i class="fas fa-trash" type="button" onclick="deleteMessage(${message.id})"></i>
            </div>
            <small class="text-muted">${createdOn.toLocaleString()}</small>
        `;

    $("#messagesList").append(chatInfo);
}

function errorHandler(message) {
    const errorMessage = document.getElementById('error-message');
    errorMessage.style = 'display: block';
    errorMessage.textContent = message;
}

function showMessages(messages) {
    $("#messagesList").empty();

    messages.forEach(message => {
        const createdOn = convertUTCDateToLocalDate(new Date(message.createdOn));
        const chatInfo =
            `
                <div>
                    [${message.userUsername}] ${message.content}
                    <i class="fas fa-trash" type="button" onclick="deleteMessage(${message.id})"></i>
                </div>
                <small class="text-muted">${createdOn.toLocaleString()}</small>
            `;

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