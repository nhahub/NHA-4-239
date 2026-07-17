console.log("note.js is connected");

let user_ID = null;

document.addEventListener("DOMContentLoaded", function () {
  fetch("../php/get_session.php")
    .then(res => res.json())
    .then(data => {
      if (data.logged_in) {
        console.log("User ID from session:", data.user_id);
        user_ID = data.user_id;
      } else {
        window.location.href = "login.html";
      }
    })
    .catch(err => {
      console.error("Session check error:", err);
    });

  const addBtn = document.getElementById("addNoteBtn");
  const input = document.getElementById("noteInput");

  if (addBtn && input) {
    addBtn.addEventListener("click", function () {
      const note = input.value.trim();
      if (note === "") {
        alert("Please write a note.");
        return;
      }

      if (!user_ID) {
        alert("User not logged in.");
        return;
      }

      fetch("../php/submit_notes.php", {
        method: "POST",
        headers: {
          "Content-Type": "application/x-www-form-urlencoded",
        },
        body: `content=${encodeURIComponent(note)}&receiver_id=3`
      })
        .then(res => res.json())
        .then(data => {
          if (data.success) {
            addNoteToUI(note);
            input.value = "";
          } else {
            alert("Error: " + data.message);
          }
        })
        .catch(err => {
          console.error(err);
          alert("Something went wrong.");
        });
    });
  }
});

function addNoteToUI(text) {
  const noteBox = document.createElement("div");
  noteBox.className = "note-box";
  noteBox.innerHTML = `<p>${text}</p>`;

  const container = document.querySelector(".add-note-section");
  container.parentNode.insertBefore(noteBox, container);
}

closeNav();

function openNav() {
  $(".side-nav").animate({ left: 0 }, 500);

  $(".manue-i").addClass("fa-xmark");
  $(".manue-i").removeClass("fa-bars");
  $(".ul-nav li ").eq(0).animate({ top: 0 }, 500);
  $(".ul-nav li ").eq(1).animate({ top: 0 }, 600);
  $(".ul-nav li ").eq(2).animate({ top: 0 }, 700);
  $(".ul-nav li ").eq(3).animate({ top: 0 }, 800);
  $(".ul-nav li ").eq(4).animate({ top: 0 }, 900);
  $(".ul-nav li ").eq(5).animate({ top: 0 }, 1000);
  $(".ul-nav li ").eq(6).animate({ top: 0 }, 1100);
  $(".ul-nav li ").eq(7).animate({ top: 0 }, 1200);
  $(".ul-nav li ").eq(8).animate({ top: 0 }, 1300);
  $(".ul-nav li ").eq(9).animate({ top: 0 }, 1400);
  $(".ul-nav li ").eq(10).animate({ top: 0 }, 1500);
  $(".ul-nav li ").eq(11).animate({ top: 0 }, 1600);
}
function closeNav() {
  let navWidth = $(".rightSide").outerWidth();

  $(".side-nav").animate({ left: -navWidth }, 500);
  $(".manue-i").removeClass("fa-xmark");
  $(".manue-i").addClass("fa-bars");
  $(".ul-nav li ").animate({ top: 500 }, 500);
}
$(".manue-i").click(() => {
  if ($(".side-nav").css("left") == "0px") {
    closeNav();
  } else {
    openNav();
  }
});
