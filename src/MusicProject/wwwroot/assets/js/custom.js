// JavaScript Document
function partnersCarouselFit(){var e=jQuery(".partners_carousel").data("ancarouselinstance");if(typeof e!="undefined"){switch(test_device_type()){case"large_screen":e.updateItemsPerRowNumber(4);break;case"ipad":e.updateItemsPerRowNumber(3);break;case"iphone_landscape":case"iphone":e.updateItemsPerRowNumber(2);break}}}function galleryAlbumHoverEffect(e,t){e.imagesLoaded(function(){e.each(function(){var e=jQuery(this),n=e.find(".hover_blank"),r=n.outerWidth(),i=n.outerHeight();n.css({visibility:"visible",left:-r,bottom:-i});e.on({mouseenter:function(){TweenMax.to(n,t,{css:{left:0,bottom:0},ease:Expo.easeOut})},mouseleave:function(){r=n.outerWidth();i=n.outerHeight();TweenMax.to(n,t,{css:{left:-r,bottom:-i},ease:Expo.easeIn})}})})})}function getColumnsCount(e){var t=false,n=1;while(t==false){if(e.hasClass("columns_count_"+n)){t=true;return n}n++}}function getSizeName(){var e="",t=jQuery(window).width();if(t>1170){e="desktop_wide"}else if(t>960&&t<1169){e="desktop"}else if(t>768&&t<959){e="tablet"}else if(t>300&&t<767){e="mobile"}else if(t<300){e="mobile_portrait"}return e}function loadScript(e,t,n){var r=document.createElement("script");r.type="text/javascript";if(r.readyState){r.onreadystatechange=function(){if(r.readyState=="loaded"||r.readyState=="complete"){r.onreadystatechange=null;n()}}}else{}r.src=e;t.get(0).appendChild(r)}function wpb_prepare_tab_content(e,t){vc_carouselBehaviour();var n=jQuery(t.panel).find(".isotope"),r=jQuery(t.panel).find(".wpb_gmaps_widget");if(n.length>0){n.isotope("reLayout")}if(r.length&&!r.is(".map_ready")){var i=r.find("iframe");i.attr("src",i.attr("src"));r.addClass("map_ready")}}(function(e,t,n){function c(){if(u<a.scrollTop()){o.addClass("visible")}else{o.removeClass("visible")}}var r=e("html"),i="";e(".wpb_toggle_content").hide();if(navigator.appVersion.indexOf("Win")!=-1)i="win";if(navigator.appVersion.indexOf("Mac")!=-1)i="mac";if(navigator.appVersion.indexOf("X11")!=-1)i="unix";if(navigator.appVersion.indexOf("Linux")!=-1)i="linux";r.addClass(i);var s=e(".sticky-menu #site_header"),o=s.clone(),u=s.outerHeight(),a=e(t),f,l=25;s.after(o);o.addClass("fixed").wrapInner('<div class="fixed_header"></div>');u*=1.5;e(t).on({scroll:function(e){t.clearTimeout(f);f=setTimeout(c,l)}});c();var h=150;var p=function(n,r){if(!r)r=1;var i=n.find("> ul");var s=i.length>0;var o=.4;var u="easeInOutQuad";var a=300;var f;if(s){var l=0;if(r==1){n.find("> a").append('<i class="has_sub"></i>')}n.addClass("has_sub");var c=i.height();if(c==0){var d=i.parent();for(var m=1;m<=r;m++){d.show();d=d.parent()}c=i.height()}i.data("height",c).addClass("noheight");n.on("mouseenter",function(e){h++;if(l){t.clearTimeout(l);l=0;return}var n=i.data("height");i.css({display:"block",zIndex:h});TweenMax.to(i.fadeTo(0,0),o,{css:{autoAlpha:1,height:n},ease:Power4.easeOut,onComplete:function(){i.removeClass("noheight").height("auto")}})});n.on("mouseleave",function(e){t.clearTimeout(l);l=setTimeout(function(){TweenMax.to(i,o,{css:{autoAlpha:0,height:0},ease:Power4.easeIn,onComplete:function(){i.css("display","none").addClass("noheight")}});l=0},a)});var g=i.find("> li");g.each(function(t){var n=e(this);p(n,parseInt(r,10)+1)})}if(r==1&&!e.browser.msie){var y=n.find("> a");y.on({mouseover:function(){v.addClass("bluring_menu");y.addClass("hover_element")},mouseleave:function(){}});n.on({mouseleave:function(){v.removeClass("bluring_menu");y.removeClass("hover_element")}})}},d=function(n,r){if(!r)r=1;var i=n.find("> ul");var s=i.length>0;var o=.35;var u="easeInOutQuad";var a=50;var f;if(s){var l=0;if(r==1){n.find("> a").append('<i class="has_sub"></i>')}n.addClass("has_sub");i.css({visibility:"hidden",opacity:0});n.on("mouseenter",function(e){h++;if(l){t.clearTimeout(l);l=0;return}i.css({zIndex:h});TweenMax.to(i,o,{css:{autoAlpha:1},ease:Sine.easeInOut})});n.on("mouseleave",function(e){t.clearTimeout(l);l=setTimeout(function(){TweenMax.to(i,o,{css:{autoAlpha:0},ease:Sine.easeInOut});l=0},a)});var c=i.find("> li");c.each(function(t){var n=e(this);d(n,parseInt(r,10)+1)})}if(r==1){var p=n.find("> a");p.on({mouseover:function(){v.addClass("bluring_menu");p.addClass("hover_element")},mouseleave:function(){}});n.on({mouseleave:function(){v.removeClass("bluring_menu");p.removeClass("hover_element")}})}};var v=e("#site_header .main_menu");var m=v.find(" > li");if(!e("#site_header_ul").length){v=e("#site_header .main_menu ul");m=v.find(" > li");v.attr("id","site_header_ul")}m.each(function(t){var n=e(this);d(n)});selectnav("site_header_ul",{activeclass:"current_page_item",label:v.data("title")>0?v.data("title"):":: Main Menu ::",nested:true,indent:"-"});var g=e(".selectnav");g.wrap('<div class="mobile_menu"></div>').on({change:function(){b.html(w+E(g.find("option:selected").html()))}});var y=e(".mobile_menu"),b=e('<div class="mm_placeholder"></div>'),w='<span class="indenter">&raquo;</span> ',E=function(e){if(!e)return"";return e.replace(/^-*/,"")};y.append(b);b.html(w+E(g.find("option:selected").html()));var S=e("<table />");var x=e("<table />");S.addClass("outer_title_table").attr("width","100%");x.addClass("outer_title_table").attr("width","100%");var T=e("<tr />");var N=e("<tr />");var C=e("<td />").addClass("text_env");var k=e("<td />").addClass("right_border");var L=e("<td />").html('<a href="#" class="top">Top</a>').addClass("page_top");var A=e('<td nowrap="nowrap" />').addClass("right_content_title normal");T.append(C).append(k).append(L);N.append(C.clone()).append(k.clone()).append(A.clone());S.append(T);x.append(N);e("h1.title, h2.title:not(footer .title), h3.title, h4.title").each(function(){var t=e(this);var n=t.get(0).tagName;var r=t.html().toString();var i=e("<"+n+" />").html(r);var s=t.find(".right_content"),o,u=t[0].className.split(" ");if(s&&s.length>0){o=x.clone();t.after(o).detach();for(var a=0;a<u.length;a++){if(u[a]!="title"){o.addClass(u[a])}}o.find(".text_env").html(i);o.find(".right_content").remove();o.find(".right_content_title").append(s);s.removeClass("is_hidden")}else{o=S.clone();for(var a=0;a<u.length;a++){if(u[a]!="title"){o.addClass(u[a])}}if(t.hasClass("no_top")){o.find(".page_top").remove()}t.after(o).detach();o.find(".text_env").html(i)}});e("a.top, .top a").on("click",function(t){t.preventDefault();e("html,body").animate({scrollTop:0},{duration:800,easing:"easeInOutQuad"})});var O=e(".partners_carousel"),M=O.data("autoswitch");if(e.isFunction(O.anCarousel)){O.anCarousel({itemsPerRow:4,autoSlide:M,pauseOnHover:true,animationEngine:"gsap"});partnersCarouselFit()}var _=e(".drop_off_effect");if(_.length>0){var D=.3;var P={left:-40,top:-40,autoAlpha:0,scale:2.5};_.each(function(){var t=e(this);var n=t.children();n.addClass("relative");var r=0,i=.1;n.each(function(){var t=e(this);TweenMax.from(t,D,{css:P,delay:r});r+=i})})}if(e.isFunction(e.fn.fadeOSlider)){var H=e("#cycle_slider");var B=H.find(".control_nav");H.fadeOSlider({numbers:B,selector:".slide",keyboard:true});var j=e(".slideshow");j.each(function(t){var n=e(this);n.fadeOSlider({selector:"img"})});var F=e(".portfolio_item_details .images");var I=e(".portfolio_item_details .prev_next_nav");F.fadeOSlider({prevNext:I,keyboard:true})}if(e.isFunction(e.fn.anSwiper)){var q=e("#features_blocks .features_blocks");var R=e("#features_blocks .features_blocks_nav");q.each(function(){var t=e(this),n=parseInt(t.data("navscope"),10),r=parseInt(t.data("navstep"),10);if(n==0)n=4;t.anSwiper({selector:".columns",nextprev:R,navscope:n,navstep:r,duration:700,ease:Power4.easeOut})});var U=e(".team_members");var z=e(".team_members_nav");U.anSwiper({selector:".columns",nextprev:z,navscope:4,navstep:1,duration:700,ease:Power4.easeOut})}if(q){var W=q.find(".feature_block"),X=.2,V=Power0.easeInOut,$=Power2.easeInOut;W.each(function(){var t=e(this),n=e('<div class="on_hover"></div>'),r=t.find("h3"),i=t.find(".content");t.prepend(n);var s=parseInt(n.css("top"),10),o=n.outerHeight(true),u=r.css("color"),a=i.css("color"),f;n.data({top:s,height:o});var l=function(){TweenMax.to([r,i],.1,{css:{color:"#FFF"},ease:Sine.easeInOut})};var c=function(){TweenMax.to(r,.1,{css:{color:u},ease:Sine.easeInOut});TweenMax.to(i,.1,{css:{color:a},ease:Sine.easeInOut})};t.on({mouseenter:function(){f=new TimelineMax({delay:.1});f.append(TweenMax.to(n,X,{css:{width:"100%",left:"0%"},ease:V}));f.append(TweenMax.to(n,X,{css:{height:"100%",top:0},ease:$,onStart:l}))},mouseleave:function(){TweenMax.killTweensOf([n,r,i,f]);f=new TimelineMax;f.append(TweenMax.to(n,X,{css:{height:o,top:s},ease:V,onStart:c}));f.append(TweenMax.to(n,X,{css:{width:"0%",left:"50%"},ease:$}))}})})}var J=e(".portfolio_item");if(e.isFunction(e.fn.hoverdir)){J.each(function(t){var n=e(this);n.hoverdir()})}e("#portfolio_items").on("click",".immediate_open",function(n){n.preventDefault();var r=e(this);if(r.data("standalone")){Shadowbox.open({player:r.attr("href").match(/(youtube\.com|vimeo\.com)/)?"iframe":"img",title:r.data("title"),content:r.attr("href")})}else if(r.data("gallery")){var i=r.data("gallery").split(","),s=[],o,u={continuous:true,counterType:"skip"};for(var a in i){o=i[a];o={player:o.match(/(youtube\.com|vimeo\.com)/)?"iframe":"img",title:r.data("title"),content:o,options:u};s.push(o)}Shadowbox.open(s)}else if(r.attr("target").length>0){t.open(r.attr("href"))}});var K=e(".post_entry .post_image, .member .image"),Q=.2,G=.3,Y=.2;K.imagesLoaded(function(){K.each(function(){var t=e(this),n=t.find(".hover"),r=n.find("a"),i=r.length;if(n.length==0)return;t.on({mouseenter:function(){var i=t.height();r.css({top:-i/2});TweenMax.to(n,Q,{css:{autoAlpha:1}});r.each(function(t){var n=e(this);var r=i/2-n.outerHeight()/2;TweenMax.to(n,G+Y*t,{css:{top:r},ease:Expo.easeOut})})},mouseleave:function(){var s=t.height();TweenMax.killTweensOf([n,r]);TweenMax.to(n,Q,{css:{autoAlpha:0}});r.each(function(t){var n=e(this);var o=s/2-n.outerHeight()/2;TweenMax.to(n,G+Y*t,{css:{top:o},ease:Expo.easeIn});r.each(function(t){var n=e(this);var r=-s/2;TweenMax.to(n,G+Y*(i-t),{css:{top:r}})})})}})})});if(e.isFunction(e.fn.anTweetRoller)){var Z=e(".testimonials"),et=e(".testimonials_nav");Z.anTweetRoller({switchDelay:800,puffScale:1.2,numbers:et,effect:"puff"});var tt=e(".testimonials_2");tt.anTweetRoller({switchDelay:800,effect:"fade",ease:Power0.easeInOut});var nt=e(".tweets_widget");nt.each(function(){var t=e(this),n=t.find(".tweets"),r=t.find(".tweets_nav"),i={switchDelay:800,puffScale:1.2,effect:"puff",ease:Power0.easeInOut,numbers:r,num:3};if(n.data("fetch")){e.getJSON("../api.twitter.com/1/statuses/user_timeline.json@screen_name="+n.data("user")+"&count="+n.data("count")+"&callback=?",function(e){var t={},r,s;n.find("li").remove();for(var o in e){t=e[o];s=new Date(t.created_at);r="<li>";r+='<div class="tweet">';r+=t.text;r+='<span class="timespan">'+s.getDate()+" "+month_names_short[s.getMonth()]+"</span>";r+="</div>";r+="</li>";n.append(r)}n.anTweetRoller(i)})}else{n.anTweetRoller(i)}})}if(e.isFunction(e.fn.wideslider)){var rt=e("#wideslider"),it=e("header").outerHeight();rt.wideslider({wrapper:"#wrap",delay:800,nextPrev:e(".wideslider_nextprev"),prevNextOnKeyboard:true,progressTimer:true,autoCenterCaptions:true,headerHeight:it,autoCenterVerticalOffset:-25});rt.wideslider("autoCenter")}e(".sb_tabs").each(function(){var t=e(this),n=t.find(".sb_tabs_title a"),r=t.find(".sb_tabs_content"),i=n.filter(".active").attr("href"),s;if(i&&i.length>0){s=n.filter(".active");i=r.find(i)}else{s=n.first();var o=s.attr("href");i=r.find(o)}if(i&&i.length){r.find("> div").not(i).hide();i.show();n.not(s).removeClass("active");s.addClass("active")}n.on({click:function(t){t.preventDefault();var n=e(this);var o=n.attr("href");var u=r.find(o);if(o==s.attr("href"))return;var a=u.find(".masonry");if(a&&a.length){u.show();a.masonry("reload");u.hide()}if(u&&u.length){s.removeClass("active");n.addClass("active");i.slideUp("normal");u.slideDown("normal");s=n;i=u}}})});if(e.isFunction(e.fn.masonry)){e(".sidebar_entry .tagcloud").masonry({itemSelector:"a",columnWidth:7})}e(".separated_list").each(function(){var t=e(this);var n=t.children();n.each(function(t){var n=e(this);var r=e('<li class="list_sep">&ndash;</li>');n.after(r);if(t>0){}})});if(e.isFunction(e.fn.isotope)){var st=e("#portfolio_items"),J=e("#portfolio_items .portfolio_item"),ot={masonry:{itemSelector:".portfolio_item",columnWidth:1/4,gutterWidth:0},animationOptions:{duration:750,easing:"linear",queue:false}};st.isotope(ot);var ut=e("#portfolio_filter li a");ut.each(function(){var n=e(this);var r=n.data("filter");n.on({click:function(i){i.preventDefault();ut.parent().removeClass("active");n.parent().addClass("active");t.location.hash=r=="*"?"":"category-"+r;st.isotope({filter:function(t){var n=e(this),i=n.data("filter");if(r=="*"||i.match(new RegExp(r,"i"))){return true}return false}})}})});var at=t.location.hash.toString().replace("#","");if(at.match(new RegExp("^category-(.*?)$","i"))){var ft=at.replace(/^category-/i,"");var lt='[data-filter="'+ft+'"]';ut.parent().removeClass("active");ut.filter(lt).parent().addClass("active");var ct=lt.match(/"(.*?)"/);if(ct.length>1){ct=ct[1]}st.isotope({filter:function(){var t=e(this),n=t.data("filter");if(ct=="*"||n.match(new RegExp(ct,"i"))){return true}return false}})}}var ht=jQuery(".endless_scrolling");if(jQuery.isFunction(jQuery.fn.infinitescroll)&&ht.length>0){var pt=jQuery(".endless_scroll_pagination a"),dt=".portfolio_item";ht.infinitescroll({navSelector:pt,nextSelector:pt,itemSelector:dt,onIsDone:function(){TweenMax.to(pt,.5,{css:{autoAlpha:0}})},loading:{finishedMsg:"",img:"",msgText:""}},function(t){var n=jQuery(t);var r=jQuery(".is_portfolio_filter .active a").data("filter");n.hide();pt.fadeTo(0,0);TweenMax.to(pt,.5,{css:{autoAlpha:1}});n.imagesLoaded(function(){ht.isotope("addItems",n,function(){n.show();ht.isotope({filter:function(){var t=e(this),n=t.data("filter");if(!r||r=="*"||r=="all"){return true}return n.indexOf(r)>=0}});n.each(function(){var t=e(this);t.imagesLoaded(function(){t.hoverdir()})})})})});if(ht.hasClass("endless_scrolling_manual")){jQuery(t).unbind(".infscr")}var vt=ht.data("infinitescroll");pt.on("click",function(e){e.preventDefault();vt.retrieve();if(vt.options.state.isDone){pt.fadeTo(500,0)}})}var mt=.2;e("form div.placeholder").each(function(){var t=e(this);var n=t.find("> label");var r=t.find("> input, > textarea");r.on({focus:function(e){t.addClass("focused");TweenMax.to(n,mt,{css:{autoAlpha:0}})},blur:function(e){t.removeClass("focused");if(r.val().length==0)TweenMax.to(n,mt,{css:{autoAlpha:1}})}});n.on({click:function(e){r.focus()}});if(r.val().length>0){n.css("opacity",0).css("visibility","hidden")}});var gt=e("blockquote");gt.each(function(t){var n=e(this),r=e('<div class="quotes open" />'),i=e('<div class="quotes close" />');n.append(r).append(i)});var yt=e("pre");yt.each(function(t){var n=e(this);var r=e('<div class="hash" />');n.append(r)});if(e.isFunction(e.fn.tabs)){var bt=e("div.tabs");bt.each(function(){var t=e(this);t.tabs()});var wt=e("div.accordion");wt.each(function(){var t=e(this);t.accordion()})}var Et=1;e(".progress_bars").each(function(){var t=e(this);var n=t.find(".bar");n.each(function(){var t=e(this);var n=t.find("span[data-pct]");var r=parseInt(n.data("pct"),10);if(r>100)r=100;TweenMax.to(n,Et,{css:{width:r+"%"},ease:Quad.easeOut})})});e(".testimonials_2 blockquote").each(function(){var t=e('<div class="arrow" />');e(this).append(t)});if(e.isFunction(e.fn.jScrollPane)){e(".scroll-pane").each(function(){var t=e(this);t.jScrollPane({showArrows:false,animateScroll:true,animateDuration:200,mouseWheelSpeed:100,keyboardSpeed:100,animateSteps:true});var n=t.data("jsp")})}e(".portfolio_item_details").each(function(){var t=e(this),n=e(this),r=n.find(".project_info"),i=r.find(".close"),s=n.find(".info_list"),o=n.find(".info_link"),u=n.find(".prev_next_nav");o.add(i).on("click",function(e){e.preventDefault();if(o.data("busy"))return false;o.data("busy",true);var i=o.hasClass("active")||o.data("no_animation"),a=n.width(),f=r.height(),l=r.outerWidth(),c=o.height(),h=o.position().left,p=u.width(),d=f-c-10,v=a-h-25-o.width(),m=1;if(o.data("no_animation")){m=0}if(i){if(typeof Cookies!="undefined"){Cookies.set("portfolio_info_bar","hidden")}var g=new TimelineMax({onComplete:function(){o.removeClass("active");o.data("busy",false)}});TweenMax.to(r,.5*m,{css:{left:-l},ease:Back.easeIn});TweenMax.to(s,.3*m,{css:{top:f},delay:.2*m,ease:Quad.easeIn});TweenMax.to(u,.9*m,{css:{left:300+(s.height()==0?100:0)},delay:.6*m});g.append(TweenMax.to(o,.4*m,{css:{top:d}}));g.append(TweenMax.to(o,.4*m,{css:{left:v},onComplete:function(){if(m==0){t.removeClass("is_hidden")}}}))}else{o.addClass("active");if(typeof Cookies!="undefined"){Cookies.set("portfolio_info_bar","visible")}var g=new TimelineMax({onComplete:function(){o.data("busy",false)}});TweenMax.to(r,.5*m,{css:{left:0},ease:Back.easeIn});TweenMax.to(s,.3*m,{css:{top:0},delay:.2,ease:Quad.easeIn});TweenMax.to(u,.9*m,{css:{left:0},delay:.6});g.append(TweenMax.to(o,.4*m,{css:{left:0}}));g.append(TweenMax.to(o,.4*m,{css:{top:0}}))}if(m==0){o.data("no_animation",false)}});if(t.hasClass("is_hidden")){o.data("no_animation",true).removeClass("active").trigger("click")}});if(e.isFunction(e.fn.anGallery)){var St=e("#an_gallery"),Q=.5;St.anGallery({itemSelector:".gallery_item",slideShow:e(".showing_image"),detectHashChange:true});St.find(".gallery_item").each(function(){var t=e(this);var n=e('<div class="hover" />');t.append(n).on({mouseover:function(){if(!t.hasClass("active"))TweenMax.to(n,Q,{css:{autoAlpha:1}})},mouseout:function(){TweenMax.to(n,Q,{css:{autoAlpha:0}})}})})}var xt=e(".container_404 .sad_icon");if(xt.length){var Tt=new TimelineMax,Nt=Quad.easeInOut,Ct=55;Tt.append(TweenMax.to(xt,.5,{css:{left:-Ct},ease:Nt}));Tt.append(TweenMax.to(xt,.5,{css:{left:Ct},ease:Nt}));Tt.append(TweenMax.to(xt,.5,{css:{left:0},ease:Nt}));Tt.totalDuration(.2).repeat(3)}var kt=e(".post_entry .post_thumb .image:has(.hover_image_up)"),Lt=.3;kt.each(function(t){var n=e(this);var r=n.find("img");var i=n.find(".hover.hover_image_up"),s=0;i.css({visibility:"visible",bottom:-i.outerHeight()});n.on({mouseenter:function(){s=i.outerHeight();TweenMax.to(r,Lt,{css:{top:-s}});TweenMax.to(i,Lt,{css:{bottom:0}})},mouseleave:function(){s=i.outerHeight();TweenMax.to(r,Lt,{css:{top:0}});TweenMax.to(i,Lt,{css:{bottom:-s}})}})});var At=e(".post_entry .post_thumb:has(.fade_hover)"),Ot=.35,Mt=1;At.imagesLoaded(function(){At.each(function(){var t=e(this);var n=t.find(".hover"),r=n.find("span"),i;i=t.height();n.css({visibility:"visible",marginTop:Mt*i});r.css({marginTop:-Mt*i*1.8});t.on({mouseenter:function(){TweenMax.to(n,Ot,{css:{marginTop:0},ease:Sine.easeOut});TweenMax.to(r,Ot,{css:{marginTop:0,ease:Sine.easeOut}})},mouseleave:function(){i=t.height();TweenMax.to(n,Ot,{css:{marginTop:Mt*i}});TweenMax.to(r,Ot,{css:{marginTop:-Mt*i*1.8}})}})})});var _t=e(".gallery .album .image:has(.hover_blank)"),Dt=.3;galleryAlbumHoverEffect(_t,Dt);var Pt=e("#gallery_container");e("#change_gallery_type").on("click","a",function(t){t.preventDefault();var n=e(this),r=n.data("type");e("#change_gallery_type a").removeClass("active");n.addClass("active");Pt.removeClass("grid").removeClass("list").addClass(r);if(typeof Cookies!="undefined"){Cookies.set("gallery_"+Pt.data("id")+"_view_type",r)}Pt.masonry("option",{columnWidth:Pt.hasClass("list")?1/1:1/4,isAnimated:false}).masonry("reload")});if(e.isFunction(e.fn.masonry)){Pt.imagesLoaded(function(){Pt.masonry({columnWidth:Pt.hasClass("list")?1/1:1/4})})}var ht=jQuery(".endless_scrolling");if(jQuery.isFunction(jQuery.fn.infinitescroll)&&ht.length>0){var Ht=jQuery(".endless_scroll_pagination_gallery a"),dt=".album";Ht.infinitescroll({navSelector:Ht,nextSelector:Ht,itemSelector:dt,onIsDone:function(){TweenMax.to(pt,.5,{css:{autoAlpha:0}})},loading:{finishedMsg:"",img:"",msgText:""}},function(e){var t=jQuery(e);galleryAlbumHoverEffect(t,Dt);Pt.append(t).masonry("appended",t,function(){Pt.masonry("option",{isAnimated:true}).masonry("reload")})})}if(e.isFunction(e.fn.socialShare)){var Bt=e(".social-share");Bt.each(function(){var t=e(this);t.socialShare({social:t.data("social"),whenSelect:true,selectContainer:".social-share-env"});t.on("click",function(e){e.preventDefault()})})}if(typeof t.Shadowbox!="undefined"){var jt=jQuery(".full_post .gallery .gallery-item");if(jt.length>0){var Ft=jQuery(".blog_post .gallery").attr("id");jt.each(function(){var e=jQuery(this);var t=e.find(".gallery-icon a");var n=t.find("img").attr("alt");var r=e.find(".gallery-caption").text();var i;if(n.length&&(i=n.match(/v=(\w+)/))){var s=i[1];t.attr("href","../www.youtube.com/v/"+s).attr("rel","width=700;height=450")}else if(n.length&&(i=n.match(/vimeo\.com\/(\d+)/))){var o=i[1];t.attr("href","../vimeo.com/moogaloop.swf@clip_id="+o).attr("rel","width=700;height=450");}t.attr("rel","shadowbox["+Ft+"];"+t.attr("rel"));t.attr("title",r)})}Shadowbox.init({handleOversize:"resize",continuous:true})}e(".go_back_portfolio").click(function(e){if(t.history.length>1){e.preventDefault();t.history.back()}})})(jQuery,window);jQuery(window).load(function(){jQuery(".wpb_flexslider").each(function(){var e=jQuery(this);var t=800,n=parseInt(e.attr("data-interval"))*1e3,r=e.attr("data-flex_fx"),i=true;if(n==0)i=false;e.flexslider({animation:r,slideshow:i,slideshowSpeed:n,sliderSpeed:t,smoothHeight:true})})});jQuery(document).ready(function(e){vc_twitterBehaviour();vc_toggleBehaviour();vc_tabsBehaviour();vc_accordionBehaviour();vc_teaserGrid();vc_carouselBehaviour();vc_slidersBehaviour();vc_prettyPhoto();vc_googleplus();vc_pinterest()});if(typeof window["vc_twitterBehaviour"]!=="function"){function vc_twitterBehaviour(){jQuery(".wpb_twitter_widget .tweets").each(function(e){var t=jQuery(this),n=t.attr("data-tw_name");tw_count=t.attr("data-tw_count");t.tweet({username:n,join_text:"auto",avatar_size:0,count:tw_count,template:"{avatar}{join}{text}{time}",auto_join_text_default:"",auto_join_text_ed:"",auto_join_text_ing:"",auto_join_text_reply:"",auto_join_text_url:"",loading_text:'<span class="loading_tweets">loading tweets...</span>'})})}}if(typeof window["vc_googleplus"]!=="function"){function vc_googleplus(){if(jQuery(".wpb_googleplus").length>0){(function(){var e=document.createElement("script");e.type="text/javascript";e.async=true;e.src="../https@apis.google.com/js/plusone.js";var t=document.getElementsByTagName("script")[0];t.parentNode.insertBefore(e,t)})()}}}if(typeof window["vc_pinterest"]!=="function"){function vc_pinterest(){if(jQuery(".wpb_pinterest").length>0){(function(){var e=document.createElement("script");e.type="text/javascript";e.async=true;e.src="../assets.pinterest.com/js/pinit.js";var t=document.getElementsByTagName("script")[0];t.parentNode.insertBefore(e,t)})()}}}if(typeof window["vc_toggleBehaviour"]!=="function"){function vc_toggleBehaviour(){jQuery(".wpb_toggle").click(function(e){if(jQuery(this).hasClass("wpb_toggle_title_active")){jQuery(this).removeClass("wpb_toggle_title_active").next().slideUp(500)}else{jQuery(this).addClass("wpb_toggle_title_active").next().slideDown(500)}});jQuery(".wpb_toggle_content").each(function(e){if(jQuery(this).next().is("h4.wpb_toggle")==false){jQuery('<div class="last_toggle_el_margin"></div>').insertAfter(this)}})}}if(typeof window["vc_tabsBehaviour"]!=="function"){function vc_tabsBehaviour(){jQuery(".wpb_tabs, .wpb_tour").each(function(e){var t,n=jQuery(this).attr("data-interval"),r=[];t=jQuery(this).find(".wpb_tour_tabs_wrapper").tabs({show:function(e,t){wpb_prepare_tab_content(e,t)}}).tabs("rotate",n*1e3);jQuery(this).find(".wpb_tab").each(function(){r.push(this.id)});jQuery(this).find('.wpb_tab a[href^="#"]').click(function(e){e.preventDefault();if(jQuery.inArray(jQuery(this).attr("href"),r)){t.tabs("select",jQuery(this).attr("href"));return false}});jQuery(this).find(".wpb_prev_slide a, .wpb_next_slide a").click(function(e){e.preventDefault();var n=t.tabs("option","selected");if(jQuery(this).parent().hasClass("wpb_next_slide")){n++}else{n--}if(n<0){n=t.tabs("length")-1}else if(n>=t.tabs("length")){n=0}t.tabs("select",n)})})}}if(typeof window["vc_accordionBehaviour"]!=="function"){function vc_accordionBehaviour(){jQuery(".wpb_accordion").each(function(e){var t,n=jQuery(this).attr("data-interval");t=jQuery(this).find(".wpb_accordion_wrapper").accordion({header:"> div > h3",autoHeight:false,change:function(e,t){if(jQuery.fn.isotope!=undefined){t.newContent.find(".isotope").isotope("reLayout")}vc_carouselBehaviour()}})})}}if(typeof window["vc_teaserGrid"]!=="function"){function vc_teaserGrid(){var e={fitrows:"fitRows",masonry:"masonry"};jQuery(".wpb_grid .teaser_grid_container:not(.wpb_carousel), .wpb_filtered_grid .teaser_grid_container:not(.wpb_carousel)").each(function(){var t=jQuery(this);var n=t.find(".wpb_thumbnails");var r=n.attr("data-layout-mode");n.isotope({itemSelector:".isotope-item",layoutMode:e[r]==undefined?"fitRows":e[r]});t.find(".categories_filter a").data("isotope",n).click(function(e){e.preventDefault();var t=jQuery(this).data("isotope");jQuery(this).parent().parent().find(".active").removeClass("active");jQuery(this).parent().addClass("active");t.isotope({filter:jQuery(this).attr("data-filter")})});jQuery(window).load(function(){n.isotope("reLayout")})})}}if(typeof window["vc_carouselBehaviour"]!=="function"){function vc_carouselBehaviour(){jQuery(".wpb_carousel").each(function(){var e=jQuery(this);if(e.data("carousel_enabled")!==true&&e.is(":visible")){e.data("carousel_enabled",true);var t=jQuery(this).width(),n=getColumnsCount(jQuery(this)),r=500;if(jQuery(this).hasClass("columns_count_1")){r=900}var i=jQuery(this).find(".wpb_thumbnails-fluid li");i.css({"margin-right":i.css("margin-left"),"margin-left":0});jQuery(this).find(".wpb_wrapper:eq(0)").jCarouselLite({btnNext:jQuery(this).find(".next"),btnPrev:jQuery(this).find(".prev"),visible:n,speed:r}).width("100%");var s=jQuery(this).find("ul.wpb_thumbnails-fluid");s.width(s.width()+300);jQuery(window).resize(function(){var e=screen_size;screen_size=getSizeName();if(e!=screen_size){window.setTimeout("location.reload()",20)}})}})}}if(typeof window["vc_slidersBehaviour"]!=="function"){function vc_slidersBehaviour(){jQuery(".wpb_gallery_slides").each(function(e){var t=jQuery(this);var n=0;if(t.hasClass("wpb_slider_nivo")){var r=800,i=t.attr("data-interval")*1e3;if(i==0)i=9999999999;t.find(".nivoSlider").nivoSlider({effect:"boxRainGrow,boxRain,boxRainReverse,boxRainGrowReverse",slices:15,boxCols:8,boxRows:4,animSpeed:r,pauseTime:i,startSlide:0,directionNav:true,directionNavHide:true,controlNav:true,keyboardNav:false,pauseOnHover:true,manualAdvance:false,prevText:"Prev",nextText:"Next"})}else if(t.hasClass("wpb_flexslider")&&1==2){}else if(t.hasClass("wpb_image_grid")){var s=t.find(".wpb_image_grid_ul");s.isotope({itemSelector:".isotope-item",layoutMode:"fitRows"});jQuery(window).load(function(){s.isotope("reLayout")})}})}}if(typeof window["vc_prettyPhoto"]!=="function"){function vc_prettyPhoto(){if(!jQuery.isFunction(jQuery.fn.prettyPhoto))return false;try{jQuery('a.prettyphoto, .gallery-icon a[href*=".jpg"]').prettyPhoto({animationSpeed:"normal",padding:15,opacity:.7,showTitle:true,allowresize:true,counter_separator_label:"/",hideflash:false,modal:false,callback:function(){var e=location.href;var t=e.indexOf("#!prettyPhoto")?true:false;if(t)location.hash="!"},social_tools:""})}catch(e){}}}var screen_size=getSizeName()