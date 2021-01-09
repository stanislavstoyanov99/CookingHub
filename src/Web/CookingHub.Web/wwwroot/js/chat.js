var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();
let currentUser = "";
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
            <div class="ml-auto text-right">
                 <div class="msg">
                     <i class="fas fa-trash" style="font-size:11px;" type="button" onclick="deleteMessage(${message.id})"></i>
                     ${message.content}</br>
                     <small class="text-muted ml-auto text-right" style="font-size:9px">${createdOn.toLocaleString()}</small>
                 </div>
            </div>
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
        let chatInfo;
        if (message.userUsername === currentUser) {
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
    var user = $("#userUsername").val();
    setCurrentUser(user);
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
    currentUser = username;
}