$(document).ready(function () {

    let current_fs, next_fs, previous_fs; //fieldsets
    let opacity;

    const nextBtns = Array.from(document.getElementsByClassName('next action-button'));
    const prevBtns = Array.from(document.getElementsByClassName('previous action-button-previous'));

    nextBtns.forEach(x => x.addEventListener('click', function onClickEventHandler() {
        current_fs = $(this).parent();
        next_fs = $(this).parent().next();

        //Add Class Active
        $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

        next_fs.show();

        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making fielset appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                next_fs.css({ 'opacity': opacity });
            },
            duration: 900
        });
    }));

    prevBtns.forEach(x => x.addEventListener('click', function onClickEventHandler() {
        current_fs = $(this).parent();
        previous_fs = $(this).parent().prev();

        //Remove class active
        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

        //show the previous fieldset
        previous_fs.show();

        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making fielset appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                previous_fs.css({ 'opacity': opacity });
            },
            duration: 900
        });
    }));

    $('.submit').on('click', () => {
        $("#msform").validate();

        if ($("#msform").valid() === false) {
            previous_fs.show();
        }
    });
});