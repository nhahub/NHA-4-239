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
  noteBox.innerHTML = `<p>${text}</p>`; // استخدام الباكتيك بشكل صحيح هنا

  const container = document.querySelector(".add-note-section");
  container.parentNode.insertBefore(noteBox, container);
}
