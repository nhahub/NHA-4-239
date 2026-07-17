document.addEventListener("DOMContentLoaded", function () {
  const form          = document.querySelector("form");
  const emailInput    = form.querySelector("input[name='email']");
  const passwordInput = form.querySelector("input[name='password']");
  const emailErr      = document.getElementById("email-error");
  const passwordErr   = document.getElementById("password-error");

  function validateEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }
  function validatePassword(pw) {
    return /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$/.test(pw);
  }

  emailInput.addEventListener("input", () => {
    const v = emailInput.value.trim();
    emailErr.textContent = v === "" || validateEmail(v)
      ? ""
      : "Please enter a valid email.";
  });

  passwordInput.addEventListener("input", () => {
    const v = passwordInput.value;
    passwordErr.textContent = v === "" || validatePassword(v)
      ? ""
      : "Password must be ≥8 chars, include number & special char.";
  });

  form.addEventListener("submit", function (e) {
    e.preventDefault();

    const email    = emailInput.value.trim();
    const password = passwordInput.value.trim();

    if (email === "" || password === "") {
      alert("Please fill in both fields.");
      return;
    }
    if (!validateEmail(email)) {
      emailErr.textContent = "Please enter a valid email.";
      return;
    }
    if (!validatePassword(password)) {
      passwordErr.textContent = "Password must be ≥8 chars, include number & special char.";
      return;
    }

    const data = new URLSearchParams({ email, password });

    fetch("http://localhost/timebank/php/login_api.php", {
      method: "POST",
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
      body: data.toString()
    })
      .then(response => response.json())
      .then(data => {
        if (data.success) {
          alert(data.message);
          window.location.href = "home1.html";
        } else {
          alert(data.message);
        }
      })
      .catch(error => {
        console.error("Login error:", error);
        alert("Something went wrong. Please try again.");
      });
  });
});
