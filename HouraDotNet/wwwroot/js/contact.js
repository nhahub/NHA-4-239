// ---------------------- Contact Form Submission ----------------------
document.getElementById('contactForm').addEventListener('submit', function(event) {
    event.preventDefault(); 

    const name = document.getElementById('name').value;
    const email = document.getElementById('email').value;
    const message = document.getElementById('message').value;

    if (name === '' || email === '' || message === '') {
        alert('All fields are required!');
        return;
    }

    const formData = new FormData();
    formData.append('name', name);
    formData.append('email', email);
    formData.append('message', message);

    fetch('../php/Contact.php', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())  
    .then(data => {
        if (data.status === 'success') {
            alert('Your message has been sent successfully!');
            document.getElementById('contactForm').reset(); 
        } else {
            alert('Error: ' + data.message);
        }
    })
    .catch(error => {
        console.error('Error:', error); 
        alert('There was an error with the request.');
    });
});

// ---------------------- Sidebar Navigation ----------------------
function openNav() {
  $(".side-nav").animate({ left: 0 }, 500);

  $(".manue-i").addClass("fa-xmark");
  $(".manue-i").removeClass("fa-bars");

  $(".ul-nav li").each(function(index) {
    $(this).animate({ top: 0 }, 500 + (index * 100));
  });
}

function closeNav() {
  let navWidth = $(".rightSide").outerWidth();

  $(".side-nav").animate({ left: -navWidth }, 500);
  $(".manue-i").removeClass("fa-xmark");
  $(".manue-i").addClass("fa-bars");
  $(".ul-nav li").animate({ top: 500 }, 500);
}

$(".manue-i").click(() => {
  if ($(".side-nav").css("left") == "0px") {
    closeNav();
  } else {
    openNav();
  }
});

// ---------------------- Initial Close of Nav ----------------------
closeNav();
