document.addEventListener("DOMContentLoaded", () => {
  const servicesContainer = document.getElementById("services-container");

  fetch("../browes_services.php")
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((services) => {
      if (!Array.isArray(services)) {
        throw new Error("Invalid data format");
      }

      servicesContainer.innerHTML = "";  

      if (services.length === 0) {
        servicesContainer.innerHTML = "<p>No services found.</p>";
        return;
      }

      services.forEach((service) => {
        const card = document.createElement("div");
        card.className = "card m-2 p-3 border border-dark";
        card.innerHTML = `
          <h4>${service.name}</h4>
          <p><strong>Description:</strong> ${service.description}</p>
          <p><strong>Category:</strong> ${service.category}</p>
          <p><strong>Price:</strong> ${service.price}</p>
        `;
        servicesContainer.appendChild(card);
      });

      console.log("✅ Services loaded successfully");
    })
    .catch((error) => {
      console.error("❌ Error loading services:", error);
      if (servicesContainer)
        servicesContainer.innerHTML = `<p style="color:red;">Failed to load services.</p>`;
    });
});

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

