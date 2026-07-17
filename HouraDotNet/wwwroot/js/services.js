document.addEventListener("DOMContentLoaded", function () {
  fetch("../services_api.php")
    .then(response => response.json())
    .then(data => {
      if (data.status === "success") {
        const services = data.data;

        console.log("My Services:", services);

        const container = document.getElementById("servicesContainer");
        services.forEach(service => {
          const card = document.createElement("div");
          card.className = "service-card";
          card.innerHTML = `
            
            <h2>${service.Title}</h2>
            <p>${service.Description}</p>
            <div class="service-details">
              <span><i class="fa-regular fa-clock"></i> ${service.Hours} Hours</span>
              <span class="rating">
                ${generateStars(service.Rating)}
              </span>
              <span><i class="fa-solid fa-users"></i> ${service.Views}K</span>
            </div>
            <button class="btn manage-btn">Manage</button>
          `;
          container.appendChild(card);
        });
      } else {
        alert("No services found.");
      }
    })
    .catch(error => {
      console.error("Error fetching services:", error);
    });
});

function generateStars(rating) {
  let stars = '';
  for (let i = 0; i < rating; i++) {
    stars += `<i class="fa-solid fa-star"></i>`;
  }
  return stars;
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
