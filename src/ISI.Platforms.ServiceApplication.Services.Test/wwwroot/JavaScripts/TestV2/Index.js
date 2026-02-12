jQuery.namespace("ISI.Platforms.ServiceApplication.Services.Test.TestV2.Index", function(jQuery) {
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
	ISI.Platforms.ServiceApplication.Services.Test.TestV2.Index.Setup();
});