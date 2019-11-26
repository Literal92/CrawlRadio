/**
 *	jQuery Swiper
 *
 *	Plugin by: Arlind Nushi
 *	www.arlindnushi.com
 *
 *	Version: 1.0
 *	Date: 11/25/12
 */

;(function($, window, undefined){
	
	$.fn.anSwiper = function(opts, val)
	{
		var $container = $(this);
				
		if($container.data('initialized'))
		{
			var methods = $container.data('methods');
			
			if(methods.hasOwnProperty(opts) && val != 'init')
			{
				return methods[opts](val);
			}
			
			return null;
		}
		
		return this.each(function(i)
		{
			var 
			// public properties
			def = {
				selector: null,
				duration: 250,
				ease: null,
				
				// Next Prev Nav
				nextprev: null,
				navscope: 10, // when navigation should be visible
				navstep: 1,
				
				// Wrapper
				wrapperClass: 'sliding_door',
				
				step: 'auto',
				index: 0,
				
				// Touch Options
				enableTouch: true,
				swipeRatio: .7,
				modernizr: false,
				noTransitionsClass: 'notransition',
				
				// Will Automatically set to true if html[dir=rtl]
				isRTL: false
			},
			// private properties
			p = {				
				wrapper: null,
				items: null,
				nextprev: null,
				use_modernizr: false,
				container_width: 0,
				
				index: 0,
				max_index: 0,
				pixel_offset: 0
			},
			methods = {
				init: function(opts)
				{
					// Extend Options
					$.extend(def, opts);
					
					// Direction
					def.isRTL = $("html[dir=\"rtl\"]").length > 0;
					
					// Set Container
					p.container = $(this);
					p.container.data({initialized: true, methods: methods});
					p.container_width = p.container.outerWidth();
					
					// Items
					if(def.selector == '')
						p.items = p.container.children();
					else
						p.items = p.container.find(def.selector);
					
					var index_unit = p.items.length - 1;
					p.max_index = index_unit - (def.navscope - 1);
					
					
					// Wrap the items environment
					p.wrapper = $('<div></div>').addClass(def.wrapperClass).css({position: 'relative'}).append(p.items);
					p.container.append(p.wrapper);
					
					
					// Calculate the width of wrapper
					var wrapper_width = 0, item;
					
					if(def.step == 'auto')
					{
						def.step = p.items.outerWidth(true);
					}
					
					p.items.each(function(i)
					{
						item = $(this);
						
						wrapper_width += item.outerWidth(true);
					});
					
					p.wrapper.width(wrapper_width);
					
					
					// Generate Next Nav
					if(p.items.length > def.navscope && def.nextprev)
					{
						if(typeof def.nextprev == 'string')
							p.nextprev = $(def.nextprev);
						else
							p.nextprev = def.nextprev;
						
						var next = $('<a href="#" class="next"></a>');
						var prev = $('<a href="#" class="prev"></a>');
						
						p.nextprev.append(prev).append(next);
						
						// NextPrev Events
						next.on('click', function(ev){
							ev.preventDefault();
							
							methods.next();
						});
						
						prev.on('click', function(ev){
							ev.preventDefault();
							
							methods.prev();
						});
					}
					
					
					// Set Initial Position
					methods.setPosition(def.index, true);
					
					
					// Set Up Gesture Events
					if(def.enableTouch)
					{
						methods.setupGestureEvents();
					}
					
					
					// Modernizr Check
					if(def.modernizr && Modernizr.csstransitions)
					{
						p.use_modernizr = true;
						p.wrapper.css({
							
						});
					}
				},
				
				
				// Reload Options
				reload: function(opts)
				{
					var wrapper_width = 0, item;
					var new_step = p.items.outerWidth(true);
					
					// Get New Width and Reposition
					def.step = new_step;
										
					p.items.each(function(i)
					{
						item = $(this);
						
						wrapper_width += item.outerWidth(true);
					});
					
					p.wrapper.width(wrapper_width);
					
					methods.setPosition(p.index);
					
					
					// Extend Options
					if(typeof opts == 'object')
					{
						$.extend(def, opts);
						
						// Reset Max Index if NavScope is set
						if(opts.hasOwnProperty('navscope'))
						{
							var _navscope = opts.navscope;
							
							var index_unit = p.items.length - 1;
							p.max_index = index_unit - (def.navscope - 1);
						}
					}
				},
				
				// Set Position by Index
				setPosition: function(index, no_animation)
				{					
					index = index % (p.max_index+1);
					
					//console.log(index);
					
					if(index < 0)
						index = p.max_index;
						
					// Calculate the pixels
					var step = def.step,
						rtl_mult = def.isRTL ? -1 : 1,
						pixel_offset = index * step * rtl_mult,
						delay = methods.toSeconds(def.duration);
					
					if(no_animation)
					{
						p.wrapper.css({left: -pixel_offset});
					}
					else // With Animation
					{
						if(p.use_modernizr)
						{
							p.wrapper.css({left: -pixel_offset});
						}
						else
						{
							TweenMax.to(p.wrapper, delay, {css: {left: -pixel_offset}, ease: def.ease});
						}
					}
						
					p.index = index;
					p.pixel_offset = pixel_offset;
				},
				
				
				// Next Prev
				next: function()
				{
					var index = p.index + def.navstep;
					
					if(index > p.max_index)
					{
						if('startAgain' in p && p.startAgain == true)
						{
							index = 0;
							p.startAgain = false;
						}
						else
						{
							index = p.max_index;
							p.startAgain = true;
						}
					}
					methods.setPosition(index);	
				},
				
				prev: function()
				{
					var index = p.index - def.navstep;
					
					if(index < 0)
					{
						if(p.index == 0)
						{
							index = p.max_index;
							p.startAgain = true;
						}
						else
						{
							index = 0;
							p.startAgain = false;
						}
					}
						
					methods.setPosition(index);	
				},
				
				
				
				// Setup Touch Gestures (Events)
				setupGestureEvents: function()
				{
					var dist_x, 
						pixel_offset, 
						new_offset,
						delay = methods.toSeconds(def.duration),
						step_lim,
						index_jump,
						start_dist_x,
						max_positive,
						max_negative,
						rtl_mult = def.isRTL ? -1 : 1;
										
					max_positive = p.container_width * 0.5;
					max_negative = -(p.wrapper.width() - max_positive);
					
					// Dragging
					var is_moving_vertically = false, start_y, end_y, start_x, end_x, current_y, diff_y, anim_obj, add_extra;
					
					p.wrapper.on({
						// Move Start + End
						movestart: function(ev)
						{
							start_y = ev.distY;
							start_x = ev.distX;
							
							current_y = $(window).scrollTop();
							
							if(p.use_modernizr)
							{
								p.wrapper.addClass(def.noTransitionsClass);
							}
							else
							{
								TweenMax.killTweensOf(p.wrapper);
							}
							
							start_dist_x = ev.distX;
						},
						
						moveend: function(ev)
						{
							end_y = ev.distY;
							end_x = ev.distX;
							
							is_moving_vertically = Math.abs(end_y - start_y) / (Math.abs(end_x - start_x) + 0.0001) > 2;
								
							if(is_moving_vertically)
							{
								diff_y = current_y + (start_y - end_y);
								add_extra = (diff_y - current_y) * Math.abs(ev.velocityY);
								
								
								diff_y += add_extra;
								
								anim_obj = {scrollY: current_y};
							
								TweenMax.to(anim_obj, .55, {scrollY: diff_y, ease: Power4.easeOut, onUpdate: function()
									{
										window.scrollTo(0, anim_obj.scrollY);
									}
								});
							}
							
							
							if(p.use_modernizr)
							{
								p.wrapper.removeClass(def.noTransitionsClass);
							}
							else
							{
								TweenMax.killTweensOf(p.wrapper);
							}
						
							start_dist_x = ev.distX - start_dist_x;
							
							step_lim = def.step * def.swipeRatio;
							index_jump = p.index - Math.floor(rtl_mult * start_dist_x / step_lim);
							
							if(start_dist_x > 0)
								index_jump--;
							
							
							if(index_jump < 0)
								index_jump = 0;
							else
							if(index_jump > p.max_index)
								index_jump = p.max_index;
							
							methods.setPosition(index_jump);
						},
						
						// Moving
						move: function(ev)
						{
							dist_x = ev.distX;
							pixel_offset = p.pixel_offset;
							
							new_offset = - p.pixel_offset + dist_x;
							
							if( ! def.isRTL)
							{
								if(new_offset > max_positive)
								{
									new_offset = max_positive + Math.sqrt(new_offset);
								}
								else
								if(new_offset < max_negative)
								{
									new_offset = max_negative - Math.sqrt(Math.abs(new_offset));
								}
							}							
							
							methods.translateX(p.wrapper, new_offset);
						}
					});
				},
				
				
				// Misc Functions
				toSeconds: function(milliseconds)
				{
					if(milliseconds > 1)
					{
						return milliseconds / 1000;
					}
					
					return milliseconds;
				},
				
				
				translateX: function(el, dist_x)
				{
					el.css({left: dist_x});
					
					/*el.css({
						'-webkit-transform': 'translateX(' + dist_x + 'px)',
						'-moz-transform': 'translateX(' + dist_x + 'px)',
						'-o-transform': 'translateX(' + dist_x + 'px)',
						'transform': 'translateX(' + dist_x + 'px)',
					});*/
				}
			};
			
			
			if(typeof opts == 'object')
			{
				methods.init.apply(this, [opts]);
			}
		});
	}
	
})(jQuery, window);