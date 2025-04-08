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
            url: "http://lighthouse.centralindia.cloudapp.azure.com/api/expert/ask-quran", // Replace with your REST API
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

        let sessionId = localStorage.getItem("sessionId");
        let userFaith = parseInt(localStorage.getItem("userFaith"));

        fetch("http://lighthouse.centralindia.cloudapp.azure.com/api/expert/chat-stream", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ message: userMessage, userid: "ahmar", sessionid: sessionId, agent: userFaith })
        })
            .then(async (response) => {
                if (!response.body) {
                    throw new Error("No response body");
                }

                const reader = response.body.getReader();
                const decoder = new TextDecoder();
                let aiResponse = "";
                let messageId = `message-${Date.now()}`;
                $("#chat-box").append(`<div id="${messageId}" class="ai-message"><strong>Expert:</strong> <span></span></div>`);
                let messageSpan = $(`#${messageId} span`);

                async function readStream() {
                    const { done, value } = await reader.read();
                    if (done) return;

                    let chunk = decoder.decode(value, { stream: true });
                    aiResponse += chunk;

                    // Append the new chunk to the existing message dynamically
                    messageSpan.append(chunk);

                    readStream(); // Read next chunk
                }

                readStream();
            })
            .catch(() => {
                appendMessage("AI", "Sorry, something went wrong!", "ai");
            });
    }

    function appendMessage(sender, message, type) {
        let className = type === "user" ? "user-message" : "ai-message";
        let messageElement = `<div class="${className}"><strong>${sender}:</strong> ${message}</div>`;
        $("#chat-box").append(messageElement);
        $("#chat-box").scrollTop($("#chat-box")[0].scrollHeight);
    }
});
