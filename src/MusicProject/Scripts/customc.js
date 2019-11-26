$(document).ready(function () {



  var vid = document.getElementById("myVideo");
  vid.onloadstart = function () { };


  vid.onplay = function () {

    var $menu = $("#menu li");
    $menu.addClass("animated fadeInUpBig");
    $menu.css("animation-duration", "2.5s")
    $menu.eq(0).css('animation-delay', '.3s');
    $menu.eq(1).css('animation-delay', '.5s');
    $menu.eq(2).css('animation-delay', '.7s');
    $menu.eq(3).css('animation-delay', '.9s');
    $menu.eq(4).css('animation-delay', '1.1s');
    $menu.eq(5).css('animation-delay', '1.3s');
    $menu.eq(6).css('animation-delay', '1.5s');



    var $orders = $("#orders img");
    $orders.addClass("animated fadeInUp");
    $orders.eq(0).css('animation-delay', '1.3s');
    $orders.eq(1).css('animation-delay', '1.5s');


    $("#orders > a ").addClass("animated fadeInUp");
    $("#orders > a").css('animation-delay', '1s');



    $(".open-side").addClass("animated fadeInUp");
    $(".open-side").css('animation-delay', '.7s');



    $("#logo-top").addClass("animated fadeInUp");
    $("#logo-top").css('animation-delay', '.5s');


    $("#center-logo").addClass("animated fadeInUp");
    $("#center-logo").css('animation-delay', '1s');
    $("#center-logo").css('animation-duration', '2s');
  };

  vid.onloadeddata = function () { };


  $('#fullpage').fullpage({
    sectionsColor: ['#000', '#fff', '#282828', '#fff', '#ccddff', '#fff', '#272727'],
    anchors: ['home', 'about', 'works', "standards", "customers", "team", "contact"],
    menu: '#menu',
    //easingcss3: 'cubic-bezier(0.175, 0.885, 0.320, 1.275)'
    easingcss3: 'cubic-bezier(0.785, 0.135, 0.150, 0.860)',
    css3: true,
    verticalCentered: true,
    scrollOverflow: true,
    continuousVertical: true,
    scrollingSpeed: 900,

    afterSlideLoad: function (anchorLink, index, slideAnchor, slideIndex) {


      $(".work-title").addClass('animated fadeInUp');
      $(".work-title").css('animation-delay', '.4s');


      $(".work-main").addClass('animated fadeInUp');
      $(".work-main").css('animation-delay', '.8s');


      $(".more-work").addClass('animated fadeInUp');
      $(".more-work").css('animation-delay', '1.2s');
      $(".video-work").addClass('animated fadeInLeft');
      $(".video-work").css('animation-delay', '1.4s');
    },



    afterLoad: function (anchorLink, index) {


      var t = .2;

      switch (anchorLink) {
        case "home":
          {
            $("#currentPage").text('01');

            $("#about-img").removeClass('animated fadeInUp');

            $(".about-text").removeClass('animated fadeInUp');

            break;
          }
        case "about":
          {
            $("#currentPage").text('02');

            $("#about-img").addClass('animated fadeInUp');
            $("#about-img").css('animation-delay', '.2s');

            $(".about-text").addClass('animated fadeInUp');
            $(".about-text").css('animation-delay', '.7s');


            break;
          }
        case "works":
          {
            $("#currentPage").text('03');


            $(".work-title").addClass('animated fadeInUp');
            $(".work-title").css('animation-delay', '.4s');


            $(".work-main").addClass('animated fadeInUp');
            $(".work-main").css('animation-delay', '.8s');


            $(".more-work").addClass('animated fadeInUp');
            $(".more-work").css('animation-delay', '1.2s');


            $(".video-work").addClass('animated fadeInLeft');
            $(".video-work").css('animation-delay', '1.4s');

            $("#standards-text .animated").removeClass('animated fadeInUp');


            break;
          }
        case "standards":
          {
            $("#currentPage").text('04');

            $("#logo-green").addClass('animated fadeInUp');
            $("#logo-green").css('animation-delay', '.4s');

            $("#schedule").addClass('animated fadeInUp');
            $("#schedule").css('animation-delay', '.6s');

            $("#seo").addClass('animated fadeInUp');
            $("#seo").css('animation-delay', '.8s');


            $("#design").addClass('animated fadeInUp');
            $("#design").css('animation-delay', '1s');


            $("#responsive").addClass('animated fadeInUp');
            $("#responsive").css('animation-delay', '1.2s');
            $("#customer-list li").removeClass('animated fadeInUp');

            break;
          }
        case "customers":
          {


            var $customertext = $("customer-text > div");
            $customertext.addClass("animated fadeInUp");

            $customertext.css("animation-duration", "2.5s");

            $customertext.eq(0).css('animation-delay', '.3s');
            $customertext.eq(1).css('animation-delay', '.6s');
            $customertext.eq(2).css('animation-delay', '.9s');


            var $customer_list = $("#customer-list li");

            $customer_list.addClass("animated fadeInUp");

            var $customer_text = $("#customer-text div");

            $customer_text.addClass("animated fadeInUp");


            var t2 = .1;
            $customer_text.each(function () {

              t2 += .2;
              $(this).css('animation-delay', t2 + 's');
            });


            $("#customer-list li").each(function () {

              t += .2;
              $(this).css('animation-delay', t + 's');
            });


            $("#standards-text .animated").removeClass('animated fadeInUp');
            $("#currentPage").text('05');

            break;
          }
        case "team":
          {

            $(".team-title").addClass('animated fadeInUp');
            $(".team-title").css('animation-delay', '.3s');

            $(".team-text").addClass('animated fadeInUp');
            $(".team-text").css('animation-delay', '.6s');


            var $employee_list = $("#employee1 li");

            $employee_list.addClass("animated fadeInUp");

            var t3 = .1;
            $employee_list.each(function () {
              t3 += .2;
              $(this).css('animation-delay', t3 + 's');
            });
            $("#currentPage").text('06');
            break;
          }
        case "contact":
          {
            $("#currentPage").text('07');
            break;
          }

        default:
      }


    },


    onLeave: function (index, nextIndex, direction) {

      //if (direction === "down" && index == 1 && nextIndex == 7) {

      //  return false;
      //}
    }

  });


  $('#loader').click(function (e) {
    e.preventDefault();
    $.fn.fullpage.moveSectionDown();
  });

  setTimeout(
    function () {
      $("#path").addClass("path");
    }, 2500);
});

function openNav() {
  document.getElementById("sidenav").className += " openedmenu";
  document.body.className += " opened-menu";

}

function closeNav() {
  document.getElementById("sidenav").classList.remove("openedmenu");
  document.body.classList.remove("opened-menu");

}
function initMap() {
  var uluru = { lat: 36.320579, lng: 59.513738 };
  var map = new google.maps.Map(document.getElementById('map'), {
    zoom: 16,
    styles:
      [{ "featureType": "all", "elementType": "labels.text.fill", "stylers": [{ "saturation": 36 }, { "color": "#000000" }, { "lightness": 40 }] }, { "featureType": "all", "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#000000" }, { "lightness": 16 }] }, { "featureType": "all", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "administrative", "elementType": "geometry.fill", "stylers": [{ "color": "#000000" }, { "lightness": 20 }] }, { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#000000" }, { "lightness": 17 }, { "weight": 1.2 }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 20 }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 21 }] }, { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#000000" }, { "lightness": 17 }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#000000" }, { "lightness": 29 }, { "weight": 0.2 }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 18 }] }, { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 16 }] }, { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 19 }] }, { "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#000000" }, { "lightness": 17 }] }]


    ,

    center: uluru
  });
  var marker = new google.maps.Marker({
    position: uluru,
    map: map,
    icon: 'http://demo.afragroup.art/images/mapMarker.png'
  });
}

$(function () {
  $('body').removeClass('fade-out');
});


