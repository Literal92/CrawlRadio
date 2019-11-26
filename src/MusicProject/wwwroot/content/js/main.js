jQuery(document).ready(function($){

    $(window).bind('resize', handle_resize);
//    console.info($('.preseter-field'))
    $('.preseter-field').bind('change', submit_preseter);
    function handle_resize(e){
        $('.maindemopadder').height($('.mcon-maindemo').outerHeight(false));
        if($('.mcon-maindemo').css('position')!='fixed'){
            $('.maindemopadder').height(0);
        }
    }
    $(window).scroll(function() {
        var _w = $(this);
        //console.log(_w, _w.scrollTop());
        if($('.mcon-maindemo').css('position')=='fixed'){

            $('.mcon-maindemo').css({
                'top' : -(_w.scrollTop()/4)
            })
            $('.mcon-mainmenu').css({
                'top' : -(_w.scrollTop()/6)
            })
        }


    });
})




function submit_preseter(e){

    var auxurl = '';
    if(jQuery('input[name="disable_volume"]').prop('checked')==true){
        auxurl = add_query_arg(window.location.href, 'disable_volume','on');
    }else{
        auxurl = add_query_arg(window.location.href, 'disable_volume','off');
    };
    if(jQuery('input[name="disable_scrub"]').prop('checked')==true){
        auxurl = add_query_arg(auxurl, 'disable_scrub','on');
    }else{
        auxurl = add_query_arg(auxurl, 'disable_scrub','off');
    };
    if(jQuery('input[name="disable_views"]').prop('checked')==true){
        auxurl = add_query_arg(auxurl, 'disable_views','on');
    }else{
        auxurl = add_query_arg(auxurl, 'disable_views','off');
    };


    auxurl = add_query_arg(auxurl, 'rating',jQuery('.option-selecter-object.active[data-label*="rating-"]').attr('data-value'));


    // auxurl = add_query_arg(auxurl, 'skinwave-mode',jQuery('.option-selecter-object.active[data-label*="skinwave-mode-"]').attr('data-value'));



    window.location.href = auxurl;
}



























// -- <img  class="needs-loading " data-src="img/e1.jpg" style="width:100%;" alt="display modes"/>
window.dzs_check_lazyloading_images_use_this_element_css_top_instead_of_window_scroll = null;
window.dzs_check_lazyloading_images = function(){
    //console.info('dzs_check_lazyloading_images()');

    var st = jQuery(window).scrollTop();

    var wh = jQuery(window).height();

    //console.info(st,wh);

    if(window.dzs_check_lazyloading_images_use_this_element_css_top_instead_of_window_scroll){

        st = -(parseInt(window.dzs_check_lazyloading_images_use_this_element_css_top_instead_of_window_scroll.css('top'),10));
    }

    //console.info(st);



    jQuery('img[data-src]').each(function(){
        var _t = jQuery(this);
        //console.info(_t,_t.offset().top,st+wh);

        if(_t.offset().top<=st+wh){


            var auximg = new Image();



            auximg.onload = function(){


                //console.info(this,_t,_t.attr('data-src'));

                if(_t.attr('data-src')){

                    var aux34 = _t.attr('data-src');


                    _t.attr('src', aux34);
                    _t.attr('data-src', '');
                    _t.addClass('loaded');
                }

                if(_t.hasClass('set-height-auto-after-load')){

                    _t.css('height','auto');
                }


                //console.info(_t.parent().parent().parent().parent().parent(), _t.parent().parent().parent().parent().parent().hasClass('.mode-isotope'))
                if(_t.parent().parent().parent().parent().parent().hasClass('mode-isotope')){
                    //console.info('ceva');

                    var _c = _t.parent().parent().parent().parent().parent();
                    if(_c.get(0) && _c.get(0).api_relayout_isotope){
                        _c.get(0).api_relayout_isotope();
                    }
                }



            }

            auximg.src=_t.attr('data-src');

        }
    })
}
if(!(window.dzs_check_lazyloading_images_inited)){

    window.dzs_check_lazyloading_images_inited = false;
}

jQuery(document).ready(function($) {


    if (window.dzs_check_lazyloading_images_inited == false) {

        window.dzs_check_lazyloading_images_inited = true;


        $(window).bind('scroll', window.dzs_check_lazyloading_images);
        window.dzs_check_lazyloading_images();
        setTimeout(function () {
            window.dzs_check_lazyloading_images();
        }, 1500);
        setTimeout(function () {
            window.dzs_check_lazyloading_images();
        }, 2500);
    } else {
        if (window.dzs_check_lazyloading_images) {
            window.dzs_check_lazyloading_images();
            setTimeout(function () {
                if (window.dzs_check_lazyloading_images) {
                    window.dzs_check_lazyloading_images();
                }
            }, 1000);
            setTimeout(function () {
                if (window.dzs_check_lazyloading_images) {
                    window.dzs_check_lazyloading_images();
                }
            }, 2000);
            setTimeout(function () {
                if (window.dzs_check_lazyloading_images) {
                    window.dzs_check_lazyloading_images();
                }
            }, 3000);
        }
    }

});