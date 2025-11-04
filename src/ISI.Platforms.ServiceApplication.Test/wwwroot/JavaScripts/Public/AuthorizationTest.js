jQuery.namespace("ISI.Platforms.ServiceApplication.Test.Public.AuthorizationTest", function(jQuery) {
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
	ISI.Platforms.ServiceApplication.Test.Public.AuthorizationTest.Setup();
});