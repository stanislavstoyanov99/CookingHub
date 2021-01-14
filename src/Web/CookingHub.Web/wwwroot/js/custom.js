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
    Home page Slider
    ................................................. */
	
	$('#slides').superslides({
		inherit_width_from: '.cover-slides',
		inherit_height_from: '.cover-slides',
		play: 5000,
		animation: 'fade',
	});
	
    $( ".cover-slides ul li" ).append( "<div class='overlay-background'></div>" );
	
	/* ..............................................
    Back to top button
    ................................................. */
	
	$(document).ready(function(){ 
		$(window).on('scroll', function () {
			if ($(this).scrollTop() > 100) { 
				$('#back-to-top').fadeIn();
			} else { 
				$('#back-to-top').fadeOut();
			} 
		}); 
		$('#back-to-top').click(function() { 
			$("html, body").animate({ scrollTop: 0 }, 600); 
			return false; 
		});
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
	/*..............................................
	 * AdminNav
	 ...............................................*/
	var acc = document.getElementsByClassName("admin-nav");
	var i;

	for (i = 0; i < acc.length; i++) {
		acc[i].addEventListener("click", function () {
			this.classList.toggle("active");
			var panel = this.nextElementSibling;
			if (panel.style.display === "block") {
				panel.style.display = "none";
			} else {
				panel.style.display = "block";
			}
		});
	}
}(jQuery));