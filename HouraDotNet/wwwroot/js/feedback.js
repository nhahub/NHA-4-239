document.addEventListener("DOMContentLoaded", function () {
  const submitFeedbackBtn = document.getElementById("submitFeedbackButton");
  const messageInput = document.getElementById("like");
  const suggestionInput = document.getElementById("suggestion");
  const feedbackStatus = document.getElementById("feedbackStatus");
  let user_ID = null;

  fetch("../php/get_session.php")
    .then(res => res.json())
    .then(data => {
      if (data.logged_in) {
        user_ID = data.user_id;
      } else {
        alert("You must be logged in to submit feedback.");
        window.location.href = "login.html";
      }
    })
    .catch(err => {
      console.error("Error fetching session:", err);
    });

  const form = document.querySelector("form");
  form.addEventListener("submit", function (event) {
    event.preventDefault(); 

    const message = messageInput.value.trim();
    const suggestion = suggestionInput.value.trim();

    if (!message || !suggestion) {
      alert("Please provide both a message and suggestion.");
      return;
    }

    const feedbackData = {
      message: message,
      suggestion: suggestion
    };

    fetch("../feedback-api.php", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(feedbackData)
    })
      .then(response => response.json())
      .then(data => {
        if (data.success) {
          alert(data.success);
          if (feedbackStatus) {
            feedbackStatus.textContent = "Feedback submitted successfully!";
          }
        } else if (data.error) {
          alert(data.error);
        }
      })
      .catch(error => {
        console.error("Error:", error);
        alert("Something went wrong while submitting your feedback.");
      });
  });
});