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

