(function($) {
    "use strict";
	
	/* ..............................................
	Loader 
    ................................................. */
	
	$(window).on('load', function() { 
		$('.preloader').fadeOut('slow'); 
		$('body').css({'overflow':'visible'});
	});
	
	/* ..............................................
    Fixed Menu
    ................................................. */
    
	$(window).on('scroll', function () {
		if ($(window).scrollTop() > 50) {
			$('.top-header').addClass('fixed-menu');
		} else {
			$('.top-header').removeClass('fixed-menu');
		}
	});
	
	/* ..............................................
    Gallery
    ................................................. */
	
	$('#slides').superslides({
		inherit_width_from: '.cover-slides',
		inherit_height_from: '.cover-slides',
		play: 5000,
		animation: 'fade',
	});
	
    $( ".cover-slides ul li" ).append( "<div class='overlay-background'></div>" );
	
	/* ..............................................
    Map Full
    ................................................. */
	
	$(document).ready(function(){ 
		$(window).on('scroll', function () {
			if ($(this).scrollTop() > 100) { 
				$('#back-to-top').fadeIn();
				if (document.getElementById("chat").style.display != "block") {
					$('#chat-btn').fadeIn();
				}
			} else { 
				$('#back-to-top').fadeOut();
				$('#chat-btn').fadeOut(); 
			} 
		}); 
		$('#back-to-top').click(function() { 
			$("html, body").animate({ scrollTop: 0 }, 600); 
			return false; 
		});
	});
	
	/* ..............................................
    BaguetteBox
    ................................................. */
	
	baguetteBox.run('.tz-gallery', {
		animation: 'fadeIn',
		noScrollbars: true
	});

	/*...............................................
	 Navbar
	 ................................................ */
	window.onscroll = function () { scrollFunction() };

	function scrollFunction() {
		if (document.body.scrollTop > 80 || document.documentElement.scrollTop > 80) {
			document.getElementById("navbar").style.padding = "0px 10px";
			document.getElementById("logo").style.width = "150px";
			
		} else {
			document.getElementById("navbar").style.padding = "30px 10px";
			document.getElementById("logo").style.width = "300px";
		}
	}
	
}(jQuery));