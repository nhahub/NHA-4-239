document.addEventListener("DOMContentLoaded", function () {
  const purchaseBtn = document.getElementById("purchaseButton");
  const statusText = document.getElementById("hoursStatus");
  let user_ID = null;

  purchaseBtn.disabled = true;

  fetch("../php/get_session.php")
    .then(res => res.json())
    .then(data => {
      if (data.logged_in) {
        user_ID = data.user_id;
        console.log("Logged in user ID:", user_ID);
        purchaseBtn.disabled = false;
      } else {
        alert("You must be logged in to purchase hours.");
        window.location.href = "login.html";
      }
    })
    .catch(err => {
      console.error("Error fetching session:", err);
      alert("Session check failed.");
    });

  purchaseBtn.addEventListener("click", function () {
    if (!user_ID) {
      alert("User session not loaded yet.");
      return;
    }

    fetch("../php/buy_2_hours.php", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ purchase: true })
    })
    .then(response => response.json())
    .then(data => {
      if (data.message) {
        alert(data.message);
        console.log("Purchase data:", data.data);

        if (statusText) {
          statusText.textContent = `You now have ${data.data.total_credits} hours available`;
        }

      } else if (data.error) {
        alert("Error: " + data.error);
      }
    })
    .catch(error => {
      console.error("Error:", error);
      alert("Something went wrong while purchasing.");
    });
  });
});
