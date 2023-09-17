;(function($, undefined) {
	"use strict";
	var pluginName = 'scojs_modal';
	function Modal(options) {
		this.options = $.extend({}, $.fn[pluginName].defaults, options);
		this.$modal = $(this.options.target).attr('class', 'modal fade').hide();
		var self = this;
		function init() {
			if (self.options.title === '') {
				self.options.title = '&nbsp;';
			}
		}
		init();
	}


	$.extend(Modal.prototype, {
		show: function() {
			var self = this
				,$backdrop;

			if (!this.options.noBackdrop) {
				$backdrop = $('.modal-backdrop');
			}
		
			if (!this.$modal.length) {
				this.$modal = $('<div class="modal fade" aria-labelledby="modalLabel" aria-modal="true" id="' + this.options.target.substr(1) + '" tabindex="-1" role="dialog" aria-hidden="true"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title"></h4><button type="button" class="btn-close" data-dismiss="modal" aria-hidden="true"></button></div><div class="inner"/></div></div></div>').appendTo(this.options.appendTo).hide();			}

			this.$modal.find('.modal-header h4').html(this.options.title);

			if (this.options.cssClass !== undefined) {
				this.$modal.find('.modal-dialog').attr('class', 'modal-dialog ' + this.options.cssClass);
			}

			if (this.options.keyboard) {
				this.escape();
			}
			if (!this.options.noBackdrop) {
				if (!$backdrop.length) {
					$backdrop = $('<div class="modal-backdrop fade" />').appendTo(this.options.appendTo);
				}
				$backdrop[0].offsetWidth;
				$backdrop.addClass('show');
			}

			this.$modal.off('close.' + pluginName).on('close.' + pluginName, function() {
				self.close.call(self);
			});
			if (this.options.isImage && this.options.remote !== undefined) {
				this.options.content = '<img class="remote" src="' + this.options.remote + '">';
				delete this.options.remote;
			}
			if (this.options.remote !== undefined && this.options.remote != '' && this.options.remote !== '#') {
				var spinner;
				if (typeof Spinner == 'function') {
					spinner = new Spinner({color: '#3d9bce'}).spin(this.$modal[0]);
				}
				this.$modal.find('.inner').load(this.options.remote, function() {
					if (spinner) {
						spinner.stop();
					}
					if (self.options.cache) {
						self.options.content = $(this).html();
						delete self.options.remote;
					}
				});
			} else {
				this.$modal.find('.inner').html(this.options.content);
			}

			$('body').addClass('modal-open');
			this.$modal.show().addClass('show');

			//try {
			//	var $modaldialog = $(this.$modal).find('.modal-dialog');
			//	var m_top = (this.$modal.height() - $modaldialog.height()) / 2.5;
			//	$modaldialog.css({ 'margin-top': m_top + 'px' });
			//}
			//catch (e) {
			
			//}
			return this;
		}

		,close: function() {
			this.$modal.hide().off('.' + pluginName).find('.inner').html('');
			if (this.options.cssclass !== undefined) {
				this.$modal.removeClass(this.options.cssclass);
			}
			$(document).off('keyup.' + pluginName);
			$('.modal-backdrop').remove();
			$('body').removeClass('modal-open');
			if (typeof this.options.onClose === 'function') {
				this.options.onClose.call(this, this.options);
			}
			return this;
		}

		,destroy: function() {
			this.$modal.remove();
			$(document).off('keyup.' + pluginName);
			$('.modal-backdrop').remove();
			this.$modal = null;
			return this;
		}

		,escape: function() {
			var self = this;
			$(document).on('keyup.' + pluginName, function(e) {
				if (e.which == 27) {
					self.close();
				}
			});
		}
	});


	$.fn[pluginName] = function(options) {
		return this.each(function() {
			var obj;
			if (!(obj = $.data(this, pluginName))) {
				var  $this = $(this)
					,data = $this.data()
					,opts = $.extend({}, options, data)
					;
				if ($this.attr('href') !== '' && $this.attr('href') != '#') {
					opts.remote = $this.attr('href');
				}
				obj = new Modal(opts);
				$.data(this, pluginName, obj);
			}
			obj.show();
		});
	};


	$[pluginName] = function(options) {
		return new Modal(options);
	};


	$.fn[pluginName].defaults = {
		title: '&nbsp;'// modal title
		,cssClass: ''   
		,target: '#modal'	// the modal id. MUST be an id for now.
		,content: ''		// the static modal content (in case it's not loaded via ajax)
		,appendTo: 'body'	// where should the modal be appended to (default to document.body). Added for unit tests, not really needed in real life.
		,cache: false		// should we cache the output of the ajax calls so that next time they're shown from cache?
		,keyboard: false
		,noBackdrop: false
		,isImage: false		// if this is true then the remote url points to an image so we should wrap the url in an <img src="remote"> tag
	};


	$(document).on('click.' + pluginName, '[data-trigger="modal"]', function() {
		$(this)[pluginName]();
		return !$(this).is('a');
	}).on('click.' + pluginName, '[data-dismiss="modal"]', function(e) {
		e.preventDefault();
		$(this).closest('.modal').trigger('close');
	});
})(jQuery);
