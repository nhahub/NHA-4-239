document.addEventListener("DOMContentLoaded", function () {
  fetch("../learning_api.php")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Failed to fetch data");
      }
      return response.json();
    })
    .then((data) => {
      if (data.error) {
        console.error("❌ Error:", data.error);
        alert("❌ Access denied: Please log in first");
        return;
      }

      console.log("✅ Data fetched successfully", data);
      alert("✅ Learning data loaded successfully");

      const container = document.getElementById("learning-container");
      if (container) {
        data.forEach((item) => {
          const div = document.createElement("div");
          div.innerHTML = `
            <h3>${item.title}</h3>
            <p>${item.description}</p>
            <hr/>
          `;
          container.appendChild(div);
        });
      }
    })
    .catch((error) => {
      console.error("❌ Fetch error:", error.message);
      alert("❌ An error occurred while loading data");
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