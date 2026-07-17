console.log(`notification.js is connected`);

let user_ID = null;

// small close (x) button — takes the user back to wherever they came from
document.addEventListener(`DOMContentLoaded`, function () {
    const closeBtn = document.getElementById(`closeNotifications`);
    if (closeBtn) {
        closeBtn.addEventListener(`click`, function () {
            if (document.referrer) {
                window.history.back();
            } else {
                window.location.href = `home1.html`;
            }
        });
    }

    fetch(`../php/get_session.php`)
        .then(res => res.json())
        .then(data => {
            if (data.logged_in) {
                user_ID = data.user_id;
                console.log(`User ID from session: ${user_ID}`);
                document.getElementById(`user_id`).textContent = user_ID;

                fetchNotifications(user_ID);
            } else {
                window.location.href = `login.html`;
            }
        })
        .catch(err => {
            console.error(`Session check error:`, err);
        });
});

function fetchNotifications(userId) {
    fetch(`../get_notifications.php`)
        .then(res => res.json())
        .then(data => {
            if (Array.isArray(data) && data.length > 0) {
                data.forEach(notification => {
                    displayNotification(notification);
                });
            } else if (Array.isArray(data)) {
                showEmptyState();
            } else {
                console.error(`Error fetching notifications: ${data.message}`);
                showEmptyState();
            }
        })
        .catch(err => {
            console.error(`Fetch error:`, err);
            showEmptyState();
        });
}

function displayNotification(notification) {
    const container = document.getElementById(`notifications-container`);

    if (!container) {
        console.error(`Notification container not found.`);
        return;
    }

    const notificationDiv = document.createElement(`div`);
    notificationDiv.className = `notification`;

    notificationDiv.innerHTML = `
    <div class="icon"><i class="fas fa-bell"></i></div>
    <div class="text">${notification.Message}</div>
    <div class="time">${formatDate(notification.Created_at)}</div>
  `;

    container.appendChild(notificationDiv);
}

function showEmptyState() {
    const container = document.getElementById(`notifications-container`);
    if (!container) return;

    container.innerHTML = `
    <div class="empty-state">
      <i class="fa-regular fa-bell-slash"></i>
      <p>No notifications yet</p>
    </div>
  `;
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString();
}
