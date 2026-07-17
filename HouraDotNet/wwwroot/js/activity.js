console.log("script is connected");

let user_ID = null;

document.addEventListener("DOMContentLoaded", function () {
  // session check
  fetch("../php/get_session.php")
    .then(res => res.json())
    .then(data => {
      if (data.logged_in) {
        console.log("User ID from session:", data.user_id);
        user_ID = data.user_id;
        const userIdElement = document.getElementById("user-id");
        if (userIdElement) userIdElement.textContent = user_ID;

        getRequests(user_ID);
      } else {
        window.location.href = "login.html";
      }
    })
    .catch(err => {
      console.error("Session check error:", err);
    });
});

function getRequests(user_ID) {
  fetch("http://localhost/timeBank/get_requests.php")
    .then(res => res.json())
    .then(data => {
      if (data.status === "success") {
        const requestsContainer = document.getElementById("requests-container");
        if (requestsContainer) {
          requestsContainer.innerHTML = '';
          if (data.data.length > 0) {
            data.data.forEach(request => {
              const requestElement = document.createElement("div");
              requestElement.classList.add("request-item");
              requestElement.innerHTML = `
                <div class="card">
                  <div class="card-header">
                    <h2>${request.Service_Title}</h2>
                    <span class="status ${getStatusClass(request.Status)}">${request.Status}</span>
                  </div>
                  <p><strong>Sender:</strong> ${request.Sender_Name}</p>
                  <p><strong>Receiver:</strong> ${request.Receiver_Name}</p>
                  <p><strong>Requested Time:</strong> ${request.Requested_time}</p>
                  <div class="actions">
                    <button>Message</button>
                    <button>View Details</button>
                  </div>
                </div>
              `;
              requestsContainer.appendChild(requestElement);
            });
          } else {
            requestsContainer.innerHTML = '<p>No requests found.</p>';
          }
        }
      } else {
        alert("Error: " + data.message);
      }
    })
    .catch(err => {
      console.error("Error fetching requests:", err);
    });
}

function getStatusClass(status) {
  switch (status.toLowerCase()) {
    case "pending":
      return "pending";
    case "accepted":
      return "accepted";
    case "rejected":
      return "rejected";
    case "completed":
      return "completed";
    case "under review":
      return "under-review";
    default:
      return "";
  }
}

// Side Navigation (from Eman)
function openNav() {
  $(".side-nav").animate({ left: 0 }, 500);
  $(".manue-i").addClass("fa-xmark").removeClass("fa-bars");

  $(".ul-nav li").each((index, item) => {
    $(item).animate({ top: 0 }, 500 + index * 100);
  });
}

function closeNav() {
  let navWidth = $(".rightSide").outerWidth();
  $(".side-nav").animate({ left: -navWidth }, 500);
  $(".manue-i").removeClass("fa-xmark").addClass("fa-bars");
  $(".ul-nav li").animate({ top: 500 }, 500);
}

$(".manue-i").click(() => {
  if ($(".side-nav").css("left") === "0px") {
    closeNav();
  } else {
    openNav();
  }
});
