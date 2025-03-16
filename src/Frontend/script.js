$(document).ready(function() {
    $("#send-btn").click(function() {
        sendMessage();
    });

    $("#user-input").keypress(function(event) {
        if (event.which == 13) {
            sendMessage();
        }
    });

    $("#ask-btn").click(function () {
        Ask();
    });

    function Ask() {
        let userMessage = $("#user-input").val().trim();
        if (userMessage === "") return;

        appendMessage("You", userMessage, "user");
        $("#user-input").val("");

        // Send message to API
        $.ajax({
            url: "https://localhost:7162/api/expert/ask-quran", // Replace with your REST API
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ message: userMessage, userid: "ahmar", sessionid: localStorage.getItem("sessionId"), agent: parseInt(localStorage.getItem("userFaith")) }),
            crossDomain: true,
            success: function (response) {
                appendMessage("Quran", response, "ai");
            },
            error: function () {
                appendMessage("AI", "Sorry, something went wrong!", "ai");
            }
        });
    }
    function sendMessage() {
        let userMessage = $("#user-input").val().trim();
        if (userMessage === "") return;

        appendMessage("You", userMessage, "user");
        $("#user-input").val("");
 
        // Send message to API
        $.ajax({
            url: "https://localhost:7162/api/expert/chat", // Replace with your REST API
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ message: userMessage, userid: "ahmar", sessionid: localStorage.getItem("sessionId"), agent: parseInt(localStorage.getItem("userFaith")) }),
            crossDomain: true, 
            success: function(response) {
                appendMessage("Expert", response, "ai");
            },
            error: function() {
                appendMessage("AI", "Sorry, something went wrong!", "ai");
            }
        });
    }

    function appendMessage(sender, message, type) {
        let className = type === "user" ? "user-message" : "ai-message";
        let messageElement = `<div class="${className}"><strong>${sender}:</strong> ${message}</div>`;
        $("#chat-box").append(messageElement);
        $("#chat-box").scrollTop($("#chat-box")[0].scrollHeight);
    }
});
