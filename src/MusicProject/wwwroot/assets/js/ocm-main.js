
(function(window, document, undefined)
{

	var $ = jQuery,
		transformProp = window.Modernizr.prefixed('transform'),
		transitionProp = window.Modernizr.prefixed('transition'),
		transitionEnd = (function() {
			var props = {
				'WebkitTransition' : 'webkitTransitionEnd',
				'MozTransition'	: 'transitionend',
				'OTransition'	: 'oTransitionEnd otransitionend',
				'msTransition'	 : 'MSTransitionEnd',
				'transition'	 : 'transitionend'
			};
			return props.hasOwnProperty(transitionProp) ? props[transitionProp] : false;
		})(),

		hasTT = transitionEnd && transitionProp && transitionProp;

	var log = function(obj)
	{
		if (typeof window.console === 'object' && typeof window.console.log === 'function') {
			//window.console.log(obj);
		}
	};

	window.App = (function()
	{
		var _init = false, app = { };

		app.init = function()
		{
			if (_init) {
				return;
			}
			_init = true;

			app.win	= $(window);
			app.docEl= $(document.documentElement);
			app.bodyEl = $(document.body);
			

			app.docEl.addClass('js-ready js-' + (hasTT ? 'advanced' : 'basic'));
			

			var menuLinkEl = $('#menu-link'),
				menuEl = $('#site_menu'),
				wrapEl = $('#wrap'),
				wpadminbar = $("#wpadminbar");
			
			// Duplicate the Menu
			menuEl = menuEl.clone();
			menuEl.addClass('ocm-menu');
			wrapEl.before(menuEl);
			
			var adminbar = wpadminbar.detach();
			wrapEl.after(adminbar);
			
			
			//var menu_title = $('<div class="menu_block">' + window.menu_title + '</div>');
			var menu_title = $('<div class="menu_block"> :: Menu ::</div>');
			
			menuEl.prepend(menu_title);
			
			

			var closeMenu = function()
			{
				if (hasTT) {
					menuEl.one(transitionEnd, function(e) {
						app.docEl.removeClass('js-offcanvas');
					});
				} else {
					app.docEl.removeClass('js-offcanvas');
				}
				app.docEl.removeClass('js-menu');
			};

			var openMenu = function()
			{
				app.docEl.addClass('js-offcanvas js-menu');
			};

			menuLinkEl.on('click', function(e)
			{
				if (app.docEl.hasClass('js-menu')) {
					closeMenu();
				} else {
					openMenu();
				}
				e.preventDefault();
			});

			menuEl.on('click', 'button[class="close"]', closeMenu);

		};

		return app;

	})();
	
	$(document).ready(function()
	{
		window.App.init();
	});

})(window, window.document);
