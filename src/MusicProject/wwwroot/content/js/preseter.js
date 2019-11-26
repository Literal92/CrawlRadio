jQuery(document).ready(function ($) {

    jQuery('.select-wrapper select').unbind();
    jQuery('.select-wrapper select').change(change_select);
    setTimeout(function () {
        jQuery('.preseter').animate({'left': -140}, {duration: 700, queue: false});
    }, 2500)
    jQuery('.preseter > .the-icon').bind("click", function () {
        var _t = jQuery(this);
        //console.log(_t);
        if (parseInt(_t.parent().css('left')) < 0) {
            _t.parent().animate({'left': 0}, {duration: 300, queue: false});
        } else {
            _t.parent().animate({'left': -140}, {duration: 300, queue: false});
            //console.log(_t.parent().find(".picker-con"));
            _t.parent().find(".picker-con").find(".picker").fadeOut('fast');
        }
    })


    //===option-selecter START


    $('.option-selecter-con').each(function(){
        var _t = $(this);

        _t.children().bind('click', function(){
            var _t2 = $(this);

            _t2.parent().children().removeClass('active');
            _t2.addClass('active');

            //--- misc

            submit_preseter();
        })
    })
    //===option-selecter END


    $('.preseter-field').bind('change', submit_preseter);
})
function change_select() {
    var selval = (jQuery(this).find(':selected').text());
    jQuery(this).parent().children('span').text(selval);
}


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
    auxurl = add_query_arg(auxurl, 'skinwave-number',jQuery('.option-selecter-object.active[data-label="skinwave-number"]').attr('data-value'));
    auxurl = add_query_arg(auxurl, 'skinwave-mode',jQuery('.option-selecter-object.active[data-label="skinwave-mode"]').attr('data-value'));
    auxurl = add_query_arg(auxurl, 'reflection_size',jQuery('.option-selecter-object.active[data-label="reflection_size"]').attr('data-value'));
    auxurl = add_query_arg(auxurl, 'skinwave_wave_mode',jQuery('.option-selecter-object.active[data-label="skinwave_wave_mode"]').attr('data-value'));






    window.location.href = auxurl;
}