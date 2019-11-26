$(document).ready(function () {

    var owlslider = $('.owl-slider');
    owlslider.owlCarousel({
        navigation: true,
        slideSpeed: 300,
        paginationSpeed: 400,
        items: 1,
        pagination: false,
        rtl: true,
        loop: true,

        rewindSpeed: 500
    });

    $('.owlNextBtn1').click(function () {
        owlslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.owlPrevBtn1').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        owlslider.trigger('prev.owl.carousel', [300]);
    });


    $(".input").each(function (index) {
        if ($(this).val() !== "") {
            $(this).parent().addClass("focus");
        }

    });

    //if ($(".input").val() !== "") {


    //}

    $(".input").focus(function () {
        $(this).parent().addClass("focus");
    }).focusout(function () {
        if ($(this).val() === "") {
            $(this).parent().removeClass("focus");
        }

    });


    var homeslide = $('.home-slider');

    homeslide.owlCarousel({

        loop: false,
        rtl: true,
        margin: 35,
        //  nav: true,
        dots: false,
        // navClass: ["nextHomeSlide", "prevHomeSlide"],

        //navText: [
        //  "<i class='fa fa-chevron-left'></i>",
        //  "<i class='fa fa-chevron-right'></i>"
        //],
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 3
            },
            1000: {
                items: 6
            }
        }
        , onChanged: callback


    });

    var homesplayslide = $('.home-play-slider');

    homesplayslide.owlCarousel({

        loop: false,
        rtl: true,
        margin: 35,
        //  nav: true,
        dots: false,
        // navClass: ["nextHomeSlide", "prevHomeSlide"],

        //navText: [
        //  "<i class='fa fa-chevron-left'></i>",
        //  "<i class='fa fa-chevron-right'></i>"
        //],
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 3
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback


    });
    var playListSlide = $('.playlist-play-slider');
    playListSlide.children().each(function (index) {
        $(this).attr('data-position', index); // NB: .attr() instead of .data()
    });
    playListSlide.owlCarousel({
        center: true,
        dots: false,
        loop: true, items: 6,
        rtl: true, margin: 10,
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 3
            },
            1000: {
                items: 6,
            }
        }
    });
    $(document).on('click', '.playlist-play-slider .owl-item>div', function () {
        playListSlide.trigger('to.owl.carousel', $(this).data('position'));
        var x = window.location.hash;
        window.location.hash = x.replace(/\/[\d]*\d/, '/1');
        var title = $(this).find(".style-playlist-title").text();
        $("#category").text(title.trim());
        var id = $(this).data("id");
        $("#MoreInfoDiv").data("id", id);
        $.ajax({
            type: "POST",
            url: "/playlist/category?id=" + id,
            contentType: "application/json",
            dataType: "html",
            success: function (data) {
                $("#MoreInfoDiv").html(data);
            },
            error: function () {
                alert('Failed');
            }
        });
    });
    $(document).on('click', '.modal-cat', function () {

        playListSlide.trigger('to.owl.carousel', $("#" + $(this).data("id")).data('position'));
        var title = $(this).data("title");
        $("#category").text(title.trim());
        var id = $(this).data("id");
        $("#MoreInfoDiv").data("id", id);
        $.ajax({
            type: "POST",
            url: "/playlist/category?id=" + id,
            contentType: "application/json",
            dataType: "html",
            success: function (data) {
                $("#MoreInfoDiv").html(data);
                $("#exampleModalLong").modal('hide');

                $('html, body').animate({
                    scrollTop: $(".playlist-play-slider").offset().top -100
                }, 1000);
             

            },
            error: function () {
                alert('Failed');
            }
        });
    });

    $('.homePlayNextBtn').click(function () {
        homesplayslide.trigger('next.owl.carousel');
    })
    // Go to the previous item
    $('.homePlayPrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        homesplayslide.trigger('prev.owl.carousel', [300]);
    });


    function callback(event) {

        var element = event.target;
        if (!event.namespace) return;
        var carousel = event.relatedTarget,
            current = carousel.current();
        if (current === carousel.maximum()) {
            $(element).siblings(".slider-left-arrow").fadeOut("slow");
        } else {
            $(element).siblings(".slider-left-arrow").fadeIn("slow");
        }


        // Provided by the core
        // DOM element, in this example .owl-carousel
        var name = event.type;           // Name of the event, in this example dragged
        var namespace = event.namespace;      // Namespace of the event, in this example owl.carousel
        var items = event.item.count;     // Number of items
        var item = event.item.index;     // Position of the current item


        // Provided by the navigation plugin
        var pages = event.page.count;     // Number of pages
        var page = event.page.index;     // Position of the current page


        if (item <= 0) {
            $(element).siblings(".slider-right-arrow").fadeOut("slow");
        } else {
            $(element).siblings(".slider-right-arrow").fadeIn("slow");
        }
    }

    $('.homeNextBtn').click(function () {
        homeslide.trigger('next.owl.carousel');
    })
    // Go to the previous item
    $('.homePrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        homeslide.trigger('prev.owl.carousel', [300]);
    });



    var homelikeslide = $('.home-like-slider');

    homelikeslide.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,
        // navClass: ["nextHomeSlide", "prevHomeSlide"],

        //navText: [
        //  "<i class='fa fa-chevron-left'></i>",
        //  "<i class='fa fa-chevron-right'></i>"
        //],
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 3
            },
            1000: {
                items: 6
            }
        }
        , onChanged: callback
    });

    $('.homeLikeNextBtn').click(function () {
        homelikeslide.trigger('next.owl.carousel');
    })
    // Go to the previous item
    $('.homeLikePrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        homelikeslide.trigger('prev.owl.carousel', [300]);
    });



    var relatedvideoartistslider = $('.related-video-artist-slider');
    relatedvideoartistslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,
        responsive: {
            0: {
                items: 2, margin: 10
            },
            600: {
                items: 3
            },
            1000: {
                items: 3
            }
        }
        , onChanged: callback
    });
    $('.relatedArtistNextBtn').click(function () {
        relatedvideoartistslider.trigger('next.owl.carousel');
    });
    $('.relatedArtistPrevBtn').click(function () {
        relatedvideoartistslider.trigger('prev.owl.carousel', [300]);
    });


    var selectedartistslider = $('.selected-artist-slider');
    selectedartistslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {
        hideOnepage(e);
    });
    selectedartistslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback
    });
    $('.selectedArtistNextBtn').click(function () {
        selectedartistslider.trigger('next.owl.carousel');
    });
    $('.selectedArtistPrevBtn').click(function () {
        selectedartistslider.trigger('prev.owl.carousel', [300]);
    });


    var videoartistslider = $('.video-artist-slider');
    videoartistslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {
        hideOnepage(e);
    });
    videoartistslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback
    });
    $('.videoArtistNextBtn').click(function () {
        videoartistslider.trigger('next.owl.carousel');
    });
    $('.videoArtistPrevBtn').click(function () {
        videoartistslider.trigger('prev.owl.carousel', [300]);
    });


    $(document).on("click", ".album-likep,.album-likeslider", function (e) {

        var span = $(this).find("span");

        var id = $(this).data("id");
        if (getCookie("likemusic" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/album/LikeMusic?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        } else {
            alert('قبلا این آهنگ را لایک کرده اید');
        }
        setCookie("likemusic" + id, "true");
    });

    $(document).on("click", ".file-contentlist-likep,.album-likeslider", function (e) {

        var span = $(this).find("span");

        var id = $(this).data("id");
        if (getCookie("likecontentlistfile" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/playlist/LikeFilePlayList?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        } else {
            alert('قبلا این آهنگ را لایک کرده اید');
        }
        setCookie("likecontentlistfile" + id, "true");
    });

    $(document).on("click", ".album-likec", function () {


        var span = $(this).find("span");

        var id = $(this).data("id");
        if (getCookie("likealbum" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/album/LikeAlbum?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });

        } else {
            alert('قبلا این آلبوم را لایک کرده اید');
        }
        setCookie("likealbum" + id, "true");
    });

    $(document).on("click", ".playList-likec", function () {


        var span = $(this).find("span");

        var id = $(this).data("id");
        if (getCookie("likePlayList" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/playlist/LikePlayList?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });

        } else {
            alert('قبلا این پلی لیست را لایک کرده اید');
        }
        setCookie("likePlayList" + id, "true");
    });

    $(document).on("click", ".comment-likec", function () {

        var span = $(this).find("span");
        var id = $(this).data("id");
        if (getCookie("dislikecomment" + id) == "true") {
            alert("قبلا اعمال کرده اید.")
        }
        if (getCookie("likecomment" + id) !== "true" && getCookie("dislikecomment" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/Comment/LikeComment?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        } if (getCookie("likecomment" + id) == "true") {
            $.ajax({
                type: "POST",
                url: "/Comment/InverseLikeComment?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
    });

    $(document).on("click", ".comment-dislikec", function () {

        var span = $(this).find("span");
        var id = $(this).data("id");
        if (getCookie("likecomment" + id) == "true") {
            alert("قبلا اعمال کرده اید.")
        }
        if (getCookie("dislikecomment" + id) !== "true" && getCookie("likecomment" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/Comment/DisLikeComment?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        } if (getCookie("dislikecomment" + id) == "true") {
            $.ajax({
                type: "POST",
                url: "/Comment/InverseDisLikeComment?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });
        }
    });

    $(document).on("click", ".artist-like", function () {


        var span = $(this).find("span");

        var id = $(this).data("id");
        if (getCookie("likeartist" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/artist/LikeArtist?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });

        } else {
            alert('قبلا این هنرمند را لایک کرده اید');
        }
        setCookie("likeartist" + id, "true");
    });



    //$(".gotop").click(function () {
    //  $("html, body").animate({ scrollTop: 0 }, "slow");
    //  return false;
    //});

    $(document).scroll(function () {
        var scrollUp = $(".gotop");
        var offset = scrollUp.offset();
        var sh = $(window).height();



        if (offset.top > sh) {
            scrollUp.css("opacity", 1);
        }
        else {
            scrollUp.css("opacity", 0);
        }
    });
    $(document).on("click", ".gotop", function () {
        $("html").animate({ scrollTop: -1000 }, '3000');
        //    $(document).scrollTop(0);
    });


    $(document).on("click", ".video-like", function () {

        var span = $(this).find("span");
        var id = $(this).data("id");
        if (getCookie("likevideo" + id) !== "true") {
            $.ajax({
                type: "POST",
                url: "/video/LikeVideo?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    span.text(response);
                },
                failure: function (response) {
                    alert(response);
                }
            });

        } else {
            alert('قبلا این ویدیو را لایک کرده اید');
        }
        setCookie("likevideo" + id, "true");
    });




    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
    function setCookie(cname, cvalue) {
        //  var d = new Date();
        //  d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        //var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";path=/";
    }
    var popularartistslider = $('.popular-artist-slider');
    popularartistslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {
        hideOnepage(e);
    });
    popularartistslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 5
            },
            1000: {
                items: 5
            }
        }
        , onChanged: callback
    });
    $('.popularArtistNextBtn').click(function () {
        popularartistslider.trigger('next.owl.carousel');
    });
    $('.popularArtistPrevBtn').click(function () {
        popularartistslider.trigger('prev.owl.carousel', [300]);
    });

    var playListSlider = $('.popular-playlist-slider');
    playListSlider.on('initialized.owl.carousel resized.owl.carousel', function (e) {
        hideOnepage(e);
    });
    playListSlider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,
        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 5
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback
    });
    $('.playListNextBtn').click(function () {
        playListSlider.trigger('next.owl.carousel');
    });
    $('.playListPrevBtn').click(function () {
        playListSlider.trigger('prev.owl.carousel', [300]);
    });



    var homevideoslider = $('.home-video-slider');

    homevideoslider.owlCarousel({
        loop: false,

        rtl: true,
        margin: 10,
        dots: false,
        // navClass: ["nextHomeSlide", "prevHomeSlide"],

        //navText: [
        //  "<i class='fa fa-chevron-left'></i>",
        //  "<i class='fa fa-chevron-right'></i>"
        //],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
        , onChanged: callback
    });

    $('.homeVideoNextBtn').click(function () {
        homevideoslider.trigger('next.owl.carousel');
    })
    // Go to the previous item
    $('.homeVideoPrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        homevideoslider.trigger('prev.owl.carousel', [300]);
    });




    var homemobilevideoslider = $('.home-mobile-video-slider');

    homemobilevideoslider.owlCarousel({
        loop: false,

        rtl: true,
        margin: 10,
        dots: false,
        // navClass: ["nextHomeSlide", "prevHomeSlide"],

        //navText: [
        //  "<i class='fa fa-chevron-left'></i>",
        //  "<i class='fa fa-chevron-right'></i>"
        //],
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
        , onChanged: callback
    });

    $('.homemobileVideoNextBtn').click(function () {
        homemobilevideoslider.trigger('next.owl.carousel');
    })
    // Go to the previous item
    $('.homemobileVideoPrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        homemobilevideoslider.trigger('prev.owl.carousel', [300]);
    });




    var homestyleslider = $('.home-style-slider');

    homestyleslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 35,
        dots: false,

        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback
    });

    $('.styleNextBtn').click(function () {
        homestyleslider.trigger('next.owl.carousel');
    })
    // Go to the previous item
    $('.stylePrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        homestyleslider.trigger('prev.owl.carousel', [300]);
    });




    var artistSlider = $('.artist-slider');

    artistSlider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 30,
        dots: false,

        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback

    });

    $('.artistNextBtn').click(function () {
        artistSlider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.artistPrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        artistSlider.trigger('prev.owl.carousel', [300]);
    });



    var videoalbumslider = $('.video-album-slider');

    videoalbumslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {
        hideOnepage(e);
    });

    function hideOnepage(e) {



        if (e.item.count === 1) {

            $(e.target).siblings(".slider-left-arrow").fadeOut("slow");
            $(e.target).siblings(".slider-right-arrow").fadeOut("slow");
        }
    }

    function hideOnepage1(e) {
        console.log(e.target);
        if (e.item.count === 0) {
            $(e.target).parent().parent().fadeOut("slow");
        }
        if (e.item.count === 1 || e.item.count === 2) {

            $(e.target).siblings(".slider-left-arrow").fadeOut("slow");
            $(e.target).siblings(".slider-right-arrow").fadeOut("slow");
        }
    }
    videoalbumslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 30,
        dots: false,

        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback

    });

    $('.video-album-NextBtn').click(function () {
        videoalbumslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.video-album-PrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        videoalbumslider.trigger('prev.owl.carousel', [300]);
    });




    var suggestalbumslider = $('.suggest-album-slider');

    suggestalbumslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 30,
        dots: false,

        responsive: {
            0: {
                items: 2, margin: 10
            },
            600: {
                items: 2
            },
            1000: {
                items: 6
            }
        }
        , onChanged: callback

    });

    $('.suggest-album-NextBtn').click(function () {
        suggestalbumslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.suggest-album-PrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        suggestalbumslider.trigger('prev.owl.carousel', [300]);
    });


    var stylealbumslider = $('.style-album-slider');


    stylealbumslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {

        hideOnepage1(e);
    });


    stylealbumslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 20,
        dots: false,

        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 2
            }
        }
        , onChanged: callback

    });

    $('.style-album-NextBtn').click(function () {
        stylealbumslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.style-album-PrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        stylealbumslider.trigger('prev.owl.carousel', [300]);
    });

    var musicalbumslider = $('.music-album-slider');
    musicalbumslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {

        hideOnepage1(e);
    });
    musicalbumslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 20,
        dots: false,

        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 2
            }
        }
        , onChanged: callback

    });

    $('.music-album-NextBtn').click(function () {
        musicalbumslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.music-album-PrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        musicalbumslider.trigger('prev.owl.carousel', [300]);
    });

    var selectedstyleslider = $('.selected-style-slider');

    selectedstyleslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {

        hideOnepage(e);
    });

    selectedstyleslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 30,
        dots: false,

        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
        , onChanged: callback

    });

    $('.selectedStyleNextBtn').click(function () {
        selectedstyleslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.selectedStylePrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        selectedstyleslider.trigger('prev.owl.carousel', [300]);
    });


    var popularmusicslider = $('.popular-music-slider');
    popularmusicslider.on('initialized.owl.carousel resized.owl.carousel', function (e) {
        hideOnepage(e);
    });
    popularmusicslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 30,
        dots: false,

        responsive: {
            0: {
                items: 2,
                margin: 10
            },
            600: {
                items: 5
            },
            1000: {
                items: 5
            }
        }
        , onChanged: callback

    });

    $('.popularMusicNextBtn').click(function () {
        popularmusicslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.popularMusicPrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        popularmusicslider.trigger('prev.owl.carousel', [300]);
    });


    var popularvideoslider = $('.popular-video-slider');

    popularvideoslider.owlCarousel({
        loop: false,
        rtl: true,
        margin: 30,
        dots: false,

        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
        , onChanged: callback

    });

    $('.popularvideoNextBtn').click(function () {
        popularvideoslider.trigger('next.owl.carousel');
    });
    // Go to the previous item
    $('.popularvideoPrevBtn').click(function () {
        // With optional speed parameter
        // Parameters has to be in square bracket '[]'
        popularvideoslider.trigger('prev.owl.carousel', [300]);
    });


    $(".close-popup").click(function () {
        $(".app-popup").slideUp("slow");
    });



    // function getOs() {
    var userAgent = window.navigator.userAgent,
        platform = window.navigator.platform,
        macsPlatforms = ['Macintosh', 'MacIntel', 'MacPPC', 'Mac68K'],
        windowsPlatforms = ['Win32', 'Win64', 'Windows', 'WinCE'],
        iosPlatforms = ['iPhone', 'iPad', 'iPod'],
        os = null;

    if (macsPlatforms.indexOf(platform) !== -1) {
        os = 'Mac OS';
    } else if (iosPlatforms.indexOf(platform) !== -1) {

        os = 'iOS';
        $("body").addClass("app-ios");
    } else if (windowsPlatforms.indexOf(platform) !== -1) {
        os = 'Windows';
    } else if (/Android/.test(userAgent)) {
        os = 'Android';
        $("body").addClass("app-android");

    } else if (!os && /Linux/.test(platform)) {
        os = 'Linux';
    }

    //  return os;
    //}









});


var registerCompleted = function (xhr) {
    if (xhr.responseText === "Captcha") {

        $(".refresh-captcha").click();
        $(".captcha-error").fadeIn(500);
    }
    else {

        $("#registerUserId").val(xhr.responseJSON.id);
        $("#inpUsername").val(xhr.responseJSON.username);

        $(".register-wrapper").fadeOut(300);
        $(".active-wrapper").fadeIn(500);
    }
};

var loginCompleted = function (xhr) {

    if (xhr.responseJSON.result === "false") {
        $(".login-captcha").click();
        $(".login-error").text(xhr.responseJSON.err);

    } else {

        $(".profile-menu").removeClass("d-none");
        $("#loginModal").modal('hide');
        $(".register-li").fadeOut(500);
    }

};
var activeCompleted = function (xhr) {

    if (xhr.responseText === "true") {

        $(".register-wrapper").fadeIn(300);
        $(".active-wrapper").fadeOut(500);
        $('[href="#login"]').tab('show');

    } else {
        $(".active-error").fadeIn(500);
    }
};
if (typeof jQuery === "undefined") {
    throw ("jQuery Required")
}

$(window).on('load',
    function () {


        setTimeout(function () {
            $("body").removeClass("showLoader");

        },
            600);

    });
var Path = {
    'version': "0.8.4",
    'map': function (path) {
        if (Path.routes.defined.hasOwnProperty(path)) {
            return Path.routes.defined[path];
        } else {
            return new Path.core.route(path);
        }
    },
    'root': function (path) {
        Path.routes.root = path;
    },
    'rescue': function (fn) {
        Path.routes.rescue = fn;
    },
    'history': {
        'initial': {}, // Empty container for "Initial Popstate" checking variables.
        'pushState': function (state, title, path) {
            if (Path.history.supported) {
                if (Path.dispatch(path)) {
                    history.pushState(state, title, path);
                }
            } else {
                if (Path.history.fallback) {
                    window.location.hash = "#" + path;
                }
            }
        },
        'popState': function (event) {
            var initialPop = !Path.history.initial.popped && location.href == Path.history.initial.URL;
            Path.history.initial.popped = true;
            if (initialPop) return;
            Path.dispatch(document.location.pathname);
        },
        'listen': function (fallback) {
            Path.history.supported = !!(window.history && window.history.pushState);
            Path.history.fallback = fallback;

            if (Path.history.supported) {
                Path.history.initial.popped = ('state' in window.history), Path.history.initial.URL = location.href;
                window.onpopstate = Path.history.popState;
            } else {
                if (Path.history.fallback) {
                    for (route in Path.routes.defined) {
                        if (route.charAt(0) != "#") {
                            Path.routes.defined["#" + route] = Path.routes.defined[route];
                            Path.routes.defined["#" + route].path = "#" + route;
                        }
                    }
                    Path.listen();
                }
            }
        }
    },
    'match': function (path, parameterize) {
        var params = {}, route = null, possible_routes, slice, i, j, compare;
        for (route in Path.routes.defined) {
            if (route !== null && route !== undefined) {
                route = Path.routes.defined[route];
                possible_routes = route.partition();
                for (j = 0; j < possible_routes.length; j++) {
                    slice = possible_routes[j];
                    compare = path;
                    if (slice.search(/:/) > 0) {
                        for (i = 0; i < slice.split("/").length; i++) {
                            if ((i < compare.split("/").length) && (slice.split("/")[i].charAt(0) === ":")) {
                                params[slice.split('/')[i].replace(/:/, '')] = compare.split("/")[i];
                                compare = compare.replace(compare.split("/")[i], slice.split("/")[i]);
                            }
                        }
                    }
                    if (slice === compare) {
                        if (parameterize) {
                            route.params = params;
                        }
                        return route;
                    }
                }
            }
        }
        return null;
    },
    'dispatch': function (passed_route) {
        var previous_route, matched_route;
        if (Path.routes.current !== passed_route) {
            Path.routes.previous = Path.routes.current;
            Path.routes.current = passed_route;
            matched_route = Path.match(passed_route, true);

            if (Path.routes.previous) {
                previous_route = Path.match(Path.routes.previous);
                if (previous_route !== null && previous_route.do_exit !== null) {
                    previous_route.do_exit();
                }
            }

            if (matched_route !== null) {
                matched_route.run();
                return true;
            } else {
                if (Path.routes.rescue !== null) {
                    Path.routes.rescue();
                }
            }
        }
    },
    'listen': function () {
        var fn = function () { Path.dispatch(location.hash); }

        if (location.hash === "") {
            if (Path.routes.root !== null) {
                location.hash = Path.routes.root;
            }
        }

        // The 'document.documentMode' checks below ensure that PathJS fires the right events
        // even in IE "Quirks Mode".
        if ("onhashchange" in window && (!document.documentMode || document.documentMode >= 8)) {
            window.onhashchange = fn;
        } else {
            setInterval(fn, 50);
        }

        if (location.hash !== "") {
            Path.dispatch(location.hash);
        }
    },
    'core': {
        'route': function (path) {
            this.path = path;
            this.action = null;
            this.do_enter = [];
            this.do_exit = null;
            this.params = {};
            Path.routes.defined[path] = this;
        }
    },
    'routes': {
        'current': null,
        'root': null,
        'rescue': null,
        'previous': null,
        'defined': {}
    }
};
Path.core.route.prototype = {
    'to': function (fn) {
        this.action = fn;
        return this;
    },
    'enter': function (fns) {
        if (fns instanceof Array) {
            this.do_enter = this.do_enter.concat(fns);
        } else {
            this.do_enter.push(fns);
        }
        return this;
    },
    'exit': function (fn) {
        this.do_exit = fn;
        return this;
    },
    'partition': function () {
        var parts = [], options = [], re = /\(([^}]+?)\)/g, text, i;
        while (text = re.exec(this.path)) {
            parts.push(text[1]);
        }
        options.push(this.path.split("(")[0]);
        for (i = 0; i < parts.length; i++) {
            options.push(options[options.length - 1] + parts[i]);
        }
        return options;
    },
    'run': function () {
        var halt_execution = false, i, result, previous;

        if (Path.routes.defined[this.path].hasOwnProperty("do_enter")) {
            if (Path.routes.defined[this.path].do_enter.length > 0) {
                for (i = 0; i < Path.routes.defined[this.path].do_enter.length; i++) {
                    result = Path.routes.defined[this.path].do_enter[i].apply(this, null);
                    if (result === false) {
                        halt_execution = true;
                        break;
                    }
                }
            }
        }
        if (!halt_execution) {
            Path.routes.defined[this.path].action();
        }
    }
};


(function (a) {
    a.fn.InfiniteScroll = function (e) {

        var c = {
            moreInfoDiv: "#MoreInfo",
            progressDiv: "#Progress",
            loadInfoUrl: "/",
            loginUrl: "/login",
            errorHandler: null,
            completeHandler: null,
            noMoreInfoHandler: null,
            pagerSortById: "#sortArea > #pagerSortBy",
            pagerSortOrderId: "#sortArea > #pagerSortOrder",
            mainNonAjaxContentDivId: "#mainNonAjaxContent",
            paramName: "",
            pageName: "صفحه",
            nextPageUrlId: "#nextPageUrl",
            currentPage: 1,
            searchType: "album",
            searchTerm: '',
            type: '',
            typeId: ''
        };
        var e = a.extend(c, e);
        var f = function () {
            a(e.progressDiv).css("display", "block");
        };
        var d = function () {
            a(e.progressDiv).css("display", "none");
        };
        var b = function () {
            a(e.moreInfoDiv).html("");
            a(e.mainNonAjaxContentDivId).html("");
            //window.scrollTo(0, 0);
        };

        return this.each(function () {
            var h = a(this);
            var i = e.currentPage;
            var j = document.title;
            var k = function () {
                if (!a(e.pagerSortById).val()) {
                    return;
                }
                var m = "#/page/" + (i + 1) + "/" + a(e.pagerSortById).val() + "/" + a(e.pagerSortOrderId).val();
                try {
                    history.popState();
                    history.pushState({}, "", m);

                } catch (l) {
                    window.location.hash = m;
                }
                document.title = j + " / " + e.pageName + " " + (i + 1);
            };
            var g = function (l) {
                a(e.nextPageUrlId).hide();
                l.parent().hide();
                f();
                var m = a(e.pagerSortById).val();
                var n = a(e.pagerSortOrderId).val();
                var playId = $("#MoreInfoDiv").data("id");
                a.ajax({
                    type: "POST",
                    url: e.loadInfoUrl,
                    data: JSON.stringify({ page: i, pagerSortBy: m, pagerSortOrder: n, name: e.paramName, searchTerm: e.searchTerm, searchType: e.searchType, type: e.type, typeId: e.typeId, playId: playId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function (s, r) {
                        var q = s.responseText;
                        if (s.status == 403) {
                            window.location = e.loginUrl
                        } else {
                            if (r === "error" || !q) {
                                if (e.errorHandler) {
                                    e.errorHandler(this);
                                }
                            } else {
                                if (q == "no-more-info") {
                                    d();
                                    l.parent().hide();
                                    if (e.noMoreInfoHandler) {
                                        e.noMoreInfoHandler(this);
                                    }
                                } else if (q == "no-more-info-playlist") {
                                    d();
                                    l.parent().show();
                                    i = 0;
                                    if (e.noMoreInfoHandler) {
                                        e.noMoreInfoHandler(this);
                                    }
                                } else {
                                    var o = a(q);
                                    var p = a(e.moreInfoDiv).append(q);
                                    k();
                                    d();
                                    l.parent().show();
                                    if (e.completeHandler) {
                                        e.completeHandler(p, o);

                                    }
                                }
                                i++;
                            }
                        }
                    }
                });
            };

            Path.map("#/page(/:page)(/:sortby)(/:order)").to(function () {
                var m = this.params.sortby || "date";
                var l = this.params.order || "desc";
                var n = parseInt(this.params.page, 10);
                if (n == i && m == a(e.pagerSortById).val() && l == a(e.pagerSortOrderId).val()) {
                    return;
                }
                a(e.pagerSortById).val(m);
                a(e.pagerSortOrderId).val(l);
                i = n - 1;
                b();
                g(h);
            });
            Path.root("#/page/" + e.currentPage + "/date/desc");
            if (a(e.pagerSortById).val()) {
                Path.listen()
            }
            a(e.pagerSortById + "," + e.pagerSortOrderId).change(function () {
                i = 0;
                b();
                a(h).click();
            });
            a(h).click(function (l) {
                if (l.originalEvent === undefined) {
                    i = 0;
                }
                g(h);
            });
        });
    };



})(jQuery);