document.addEventListener("DOMContentLoaded", function () {
  const form = document.querySelector("form");
  const nameInput = form.querySelector("input[name='Name']");
  const emailInput = form.querySelector("input[name='email']");
  const pwInput = form.querySelector("input[name='password']");
  const cpwInput = form.querySelector("input[name='Confirm_Password']");
  const nameErr = document.getElementById("name-error");
  const emailErr = document.getElementById("email-error");
  const pwErr = document.getElementById("password-error");
  const cpwErr = document.getElementById("confirm-password-error");

  function validateName(name) {
    return /^[A-Za-z\u0600-\u06FF ]{2,}$/.test(name);
  }
  function validateEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }
  function validatePassword(pw) {
    return /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(
      pw
    );
  }

  nameInput.addEventListener("input", () => {
    const v = nameInput.value.trim();
    nameErr.textContent =
      v === "" || validateName(v)
        ? ""
        : "Name must be ≥2 letters (Arabic or English).";
  });
  emailInput.addEventListener("input", () => {
    const v = emailInput.value.trim();
    emailErr.textContent =
      v === "" || validateEmail(v) ? "" : "Please enter a valid email.";
  });
  pwInput.addEventListener("input", () => {
    const v = pwInput.value;
    pwErr.textContent =
      v === "" || validatePassword(v)
        ? ""
        : "Password must be ≥8 chars, include number & special char.";
  });
  cpwInput.addEventListener("input", () => {
    const v = cpwInput.value;
    cpwErr.textContent =
      v === "" || v === pwInput.value ? "" : "Passwords do not match.";
  });

  form.addEventListener("submit", function (e) {
    e.preventDefault();
    const name = nameInput.value.trim();
    const email = emailInput.value.trim();
    const password = pwInput.value;
    const cpw = cpwInput.value;
    if (!name || !email || !password || !cpw) {
      alert("Please fill in all fields.");
      return;
    }
    if (
      !validateName(name) ||
      !validateEmail(email) ||
      !validatePassword(password) ||
      password !== cpw
    ) {
      alert("Please fix the errors before submitting.");
      return;
    }
    const data = new URLSearchParams({
      Name: name,
      email: email,
      password: password,
      confirm_password: cpw,
    });
    fetch("http://localhost/timebank/php/signup-api.php", {
      method: "POST",
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
      body: data,
    })
      .then((r) => r.json())
      .then((d) => {
        alert(d.message);
        if (d.success) window.location.href = "login.html";
      })
      .catch(() => {
        alert("Something went wrong. Please try again.");
      });
  });
});
