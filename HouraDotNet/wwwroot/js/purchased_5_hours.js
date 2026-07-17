document.addEventListener("DOMContentLoaded", function () {
  const purchaseBtn = document.getElementById("purchase5Button");
  const statusText = document.getElementById("hoursStatus");
  let user_ID = null;

  purchaseBtn.disabled = true;

  fetch("../php/get_session.php")
    .then(res => res.json())
    .then(data => {
      if (data.logged_in) {
        user_ID = data.user_id;
        purchaseBtn.disabled = false;
      } else {
        alert("You must be logged in to purchase hours.");
        window.location.href = "login.html";
      }
    })
    .catch(err => {
      console.error("Error fetching session:", err);
    });

  purchaseBtn.addEventListener("click", function () {
    if (!user_ID) {
      alert("User session not loaded yet.");
      return;
    }

    fetch("../php/buy_5_hours.php", {
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
