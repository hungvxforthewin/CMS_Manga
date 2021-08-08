$(document).ready(function () {
    AOS.init();

  $(".datetime-picker")
    .datetimepicker({
      format: "dd-mm-yyyy",
      startView: 2,
      minView: 2,
      autoclose: true,
    })
    .datetimepicker("setDate", new Date());

  $(".only-month")
    .datetimepicker({
      format: "mm-yyyy",
      startView: 3,
      minView: 3,
      autoclose: true,
    })
    .datetimepicker("setDate", new Date());

  $(".partner-slide").slick({
    slidesToShow: 5,
    slidesToScroll: 5,
    dots: false,
    arrows: false,
    responsive: [
      {
        // <= 991
        breakpoint: 992,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
        },
      },
      {
        // <= 767
        breakpoint: 768,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
        },
      },
      {
        // <= 575
        breakpoint: 576,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        },
      },
    ],
  });

  $(document).scroll(function () {
    const offsetY = $(this).scrollTop();
    const header = $("#header");
    const scrollButton = $(".to-top");
    if (offsetY > 120) {
      header.addClass("scrolled");
      scrollButton.addClass("show");
    } else {
      header.removeClass("scrolled");
      scrollButton.removeClass("show");
    }
  });

  $(".open-menu").on("click", () => {
    $(".menu-side").toggleClass("show");
    $(".overlay").toggleClass("show");
  });

  $(".overlay").on("click", function () {
    $(".menu-side").removeClass("show");
    $(this).removeClass("show");
  });

  $(".to-top").on("click", function () {
    // $(window).scrollTop({top: 0, behavior: 'smooth'})
    $("html, body").animate({ scrollTop: 0 }, "slow");
  });

  // pop up
  $("#pop_up-1st .close-modal").on("click", function () {
    $("#pop_up-1st").removeClass("show");
  });

  function countdown() {
    const date = new Date();
    const endTimeOfDay = date.setHours(23, 59, 59, 999);
    $("#pop_up-1st .countdown")
      .countdown(endTimeOfDay)
      .on("update.countdown", function (event) {
        $(this).children(".hour").text(event.strftime("%H"));
        $(this).children(".minute").text(event.strftime("%M"));
        $(this).children(".second").text(event.strftime("%S"));
      })
      .on("finish.countdown", () => {
        countdown();
      });
  }

  countdown();
});
