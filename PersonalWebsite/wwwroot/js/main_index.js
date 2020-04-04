$(() => {
    $('#main-nav').removeClass('box-shadow');
    $('#main-nav').removeClass('border-bottom');
});


$(window).scroll(() => {

    if ($(window).scrollTop() >= 50) {
        $('#main-nav').addClass('active');
        $('#main-nav').addClass('box-shadow');
        $('#main-nav').addClass('border-bottom');
    }
    else {
        $('#main-nav').removeClass('active');
        $('#main-nav').removeClass('box-shadow');
        $('#main-nav').removeClass('border-bottom');
    }
});