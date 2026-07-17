document.addEventListener("DOMContentLoaded", function () {
    fetch(`../php/get_session.php`)
        .then(res => res.json())
        .then(data => {
            if (data.logged_in) {
                const userId = data.user_id;

                fetch(`http://localhost/timeBank/dashboard.php?user_id=${userId}`, {
                    method: 'GET'
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            document.getElementById("active-services").textContent = data.data.active_services;
                            document.getElementById("time-balance").textContent = data.data.time_balance;
                            document.getElementById("ongoing-requests").textContent = data.data.ongoing_requests;
                            document.getElementById("completed-requests").textContent = data.data.completed_requests;
                        } else {
                            console.error('Error:', data.message);
                            alert('Error fetching dashboard data');
                        }
                    })
                    .catch(error => {
                        console.error('Error fetching dashboard data:', error);
                        alert('Error fetching dashboard data');
                    });
            } else {
                console.log("User not logged in");
                window.location.href = "login.html";
            }
        })
        .catch(err => {
            console.error("Session check error:", err);
        });
});

// دمج كود التنقل الجانبي (sidebar) بكفاءة
function openNav() {
    $(".side-nav").animate({ left: 0 }, 500);
    $(".manue-i").removeClass("fa-bars").addClass("fa-xmark");

    // تأثير تدريجي للعناصر
    $(".ul-nav li").each(function (index) {
        $(this).animate({ top: 0 }, 500 + (index * 100));
    });
}

function closeNav() {
    let navWidth = $(".rightSide").outerWidth();
    $(".side-nav").animate({ left: -navWidth }, 500);
    $(".manue-i").removeClass("fa-xmark").addClass("fa-bars");
    $(".ul-nav li").animate({ top: 500 }, 500);
}

// زر القائمة
$(".manue-i").click(() => {
    if ($(".side-nav").css("left") == "0px") {
        closeNav();
    } else {
        openNav();
    }
});
