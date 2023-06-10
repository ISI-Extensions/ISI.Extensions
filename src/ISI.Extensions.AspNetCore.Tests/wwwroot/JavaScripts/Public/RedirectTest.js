jQuery.namespace("ISI.Extensions.AspNetCore.Tests.Public.RedirectTest", function(jQuery) {
	var model = {};
	var view = {};
	var controller = {
		setup: function (config) {
			controller.eventBinder();
		},
		eventBinder: function () {
		}
	};

	return {
		Setup: controller.setup
	};
} (jQuery));

jQuery(document).ready(function (jQuery) {
	ISI.Extensions.AspNetCore.Tests.Public.RedirectTest.Setup();
});