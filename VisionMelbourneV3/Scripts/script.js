(function ($) {
    "use strict";

    /*==========  Responsive Navigation  ==========*/
    $('.main-nav').children().clone().appendTo('.responsive-nav');
    $('.responsive-menu-open').on('click', function (event) {
        event.preventDefault();
        $('body').addClass('no-scroll');
        $('.responsive-menu').addClass('open');
        return false;
    });
    $('.responsive-menu-close').on('click', function (event) {
        event.preventDefault();
        $('body').removeClass('no-scroll');
        $('.responsive-menu').removeClass('open');
        return false;
    });
    $('.responsive-nav li').each(function (index) {
        if ($(this).find('ul').length) {
            var text = $(this).find('> a').text();
            var id = text.replace(/\s+/g, '-').toLowerCase();
            id = id.replace('&', '');
            $(this).find('> a').attr('href', '#collapse-' + id);
            $(this).find('> a').attr('data-toggle', 'collapse');
            $(this).find('> a').append('<i class="fa fa-angle-down"></i>');
            $(this).find('> ul').attr('id', 'collapse-' + id);
            $(this).find('> ul').addClass('collapse');
        }
    });
    $('.responsive-nav a').on('click', function () {
        if ($(this).parent().hasClass('collapse-active')) {
            $(this).parent().removeClass('collapse-active');
        } else {
            $(this).parent().addClass('collapse-active');
        }
    });

    /*==========  Login / Signup  ==========*/
    $('.login-open').on('click', function (event) {
        event.preventDefault();
        $('.login-wrapper').addClass('open');
        $('.signup-wrapper').removeClass('open');
    });
    $(document).on('click', function (event) {
        if (!$(event.target).closest('.login').length && !$(event.target).closest('.login-open').length) {
            $('.login-wrapper').removeClass('open');
        }
    });
    $('.signup-open').on('click', function (event) {
        event.preventDefault();
        $('.signup-wrapper').addClass('open');
        $('.login-wrapper').removeClass('open');
    });
    $(document).on('click', function (event) {
        if (!$(event.target).closest('.signup').length && !$(event.target).closest('.signup-open').length) {
            $('.signup-wrapper').removeClass('open');
        }
    });

    /*==========  Accordion  ==========*/
    $('.panel-heading a').on('click', function () {
        if ($(this).parents('.panel-heading').hasClass('active')) {
            $('.panel-heading').removeClass('active');
        } else {
            $('.panel-heading').removeClass('active');
            $(this).parents('.panel-heading').addClass('active');
        }
    });
})(jQuery);