$(document).ready(function () {
    $("#proceed-btn").click(function () {
        let selectedFaith = $("#faith-select").val();
        if (selectedFaith) {
            localStorage.setItem("userFaith", selectedFaith);
            localStorage.setItem("sessionId", $("#session-name").val());
            window.location.href = "chat.html"; // Redirect to chat page
        } else {
            alert("Please select your faith to continue.");
        }
    });
});