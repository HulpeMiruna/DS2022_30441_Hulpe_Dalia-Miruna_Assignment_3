$(document).ready(function () {
    sendMessage();
    typeOn();
    typeOff();
});

function sendMessage() {
    $(".send-message-button").click(function () {
        var message = $(".input-message").val();
        var fromUser = $(".from-user").val();
        var toUser = $(".to-user").val();
        var container = $(".message-container");

        $.ajax({
            type: 'POST',
            data: { message: message, toUser: toUser, fromUser: fromUser},
            success: function (res) {
                container.append(' <p class="sent-message">' + message + '</p >')
                $(".input-message").val('');
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
}

function typeOn() {
    $(".input-message").focus(function () {
        var fromUser = $(".from-user").val();
        var toUser = $(".to-user").val();
        var stringId = '#' + toUser;
        if (toUser === 'e3b3943d-02c8-4f50-e9c2-08daba791f1c') {
            stringId = stringId + '-' + fromUser;
        }

        $.ajax({
            type: 'POST',
            data: { message: "Enable", toUser: toUser, fromUser: fromUser },
            success: function (res) {
                console.log(res);
            },
            error: function (err) {
                console.log(err);
            }
        })
    })
       
}

function typeOff() {
    $(".input-message").focusout(function () {
        var fromUser = $(".from-user").val();
        var toUser = $(".to-user").val();
        var stringId = '#' + toUser;
        if (toUser === 'e3b3943d-02c8-4f50-e9c2-08daba791f1c') {
            stringId = stringId + '-' + fromUser;
        }

        $.ajax({
            type: 'POST',
            data: { message: "Disable", toUser: toUser, fromUser: fromUser },
            success: function (res) {
                console.log(res);
            },
            error: function (err) {
                console.log(err);
            }
        })
    })

}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established
connection.on("ReceiveMessage", function (message, toUser, fromUser) {
    var stringId = '#' + toUser;
    if (toUser === 'e3b3943d-02c8-4f50-e9c2-08daba791f1c') {
        stringId = stringId + '-' + fromUser;
    }

    var container = $(stringId);
    container.append(' <p class="recieved-message">' + message + '</p >')
});

connection.on("EnableType", function (toUser, fromUser) {
    var stringId = '#' + toUser;
    if (toUser === 'e3b3943d-02c8-4f50-e9c2-08daba791f1c') {
        stringId = stringId + '-' + fromUser;
    }

    var container = $(stringId);
    var type = container.find(".info");
    type.removeClass('d-none');
});

connection.on("DisableType", function (toUser, fromUser) {
    var stringId = '#' + toUser;
    if (toUser === 'e3b3943d-02c8-4f50-e9c2-08daba791f1c') {
        stringId = stringId + '-' + fromUser;
    }

    var container = $(stringId);
    var type = container.find(".info");
    type.addClass('d-none');
});

connection.start().then(function () {
    console.log("Start");
}).catch(function (err) {
    return console.error(err.toString());
});