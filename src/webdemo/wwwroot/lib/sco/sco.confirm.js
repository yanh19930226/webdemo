;(function($, undefined) {
	"use strict";

	var pluginName = 'scojs_confirm';

	function Confirm(options) {
		this.options = $.extend({}, $.fn[pluginName].defaults, options);
		var $modal = $(this.options.target);
		if (!$modal.length) {
			var confirmStr = `<div id="${this.options.target.substr(1) }" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog modal-sm">
                                                <div class="modal-content">
                                                    <div class="modal-body p-4">
                                                        <div class="text-center">
                                                            <h4>${this.options.content}</h4>
														    <button type="button" class="btn btn-secondary my-2" data-dismiss="modal">${this.options.btncancel}</button>
														    <button type="button" class="btn btn-primary my-2" data-action="1">${this.options.btnconfirm}</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>`;

			$modal = $(confirmStr).appendTo(this.options.appendTo).hide();
			if (typeof this.options.action == 'function') {
				var self = this;
				$modal.find('[data-action]').attr('href', '#').on('click.' + pluginName, function(e) {
					e.preventDefault();
					self.options.action.call(self);
					self.close();
				});
			} else if (typeof this.options.action == 'string') {
				$modal.find('[data-action]').attr('href', this.options.action);
			}
		}
		this.scomodal = $.scojs_modal(this.options);
	}

	$.extend(Confirm.prototype, {
		show: function() {
			this.scomodal.show();
			return this;
		}

		,close: function() {
			this.scomodal.close();
			return this;
		}

		,destroy: function() {
			this.scomodal.destroy();
			return this;
		}
	});


	$.fn[pluginName] = function(options) {
		return this.each(function() {
			var obj;
			if (!(obj = $.data(this, pluginName))) {
				var $this = $(this)
					,data = $this.data()
					,title = $this.attr('title') || data.title
					,opts = $.extend({}, $.fn[pluginName].defaults, options, data)
					;
				if (!title) {
					title = 'this';
				}
				opts.content = opts.content.replace(':title', title);
				if (!opts.action) {
					opts.action = $this.attr('href');
				} else if (typeof window[opts.action] == 'function') {
					opts.action = window[opts.action];
				}
				obj = new Confirm(opts);
				$.data(this, pluginName, obj);
			}
			obj.show();
		});
	};

	$[pluginName] = function(options) {
		return new Confirm(options);
	};

	$.fn[pluginName].defaults = {
		btncancel: '取消',
		btnconfirm: '确认',
		content: 'Are you sure you want to delete :title?'
		,cssclass: 'confirm_modal'
		,target: '#confirm_modal'	// this must be an id. This is a limitation for now, @todo should be fixed
		,appendTo: 'body'	// where should the modal be appended to (default to document.body). Added for unit tests, not really needed in real life.
	};

	$(document).on('click.' + pluginName, '[data-trigger="confirm"]', function() {
		$(this)[pluginName]();
		return false;
	});
})(jQuery);
