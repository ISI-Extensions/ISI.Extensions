using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.AspNetCore.Tests.Controllers
{
	public partial class PublicController 
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Public.RouteNames.RedirectTest, typeof(Routes.Public), "redirect-test")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> RedirectTestAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Extensions.AspNetCore.Tests.Models.Public.RedirectTestModel();

			var trackingQueryStringKeyValues = new Dictionary<string, string>(Request.Query.Where(queryKeyValue => queryKeyValue.Key.StartsWith("utm_", StringComparison.InvariantCultureIgnoreCase)).Select(queryKeyValue => new KeyValuePair<string, string>(queryKeyValue.Key, queryKeyValue.Value.FirstOrDefault())));

			return RedirectToRoute(Routes.Public.RouteNames.Index, trackingQueryStringKeyValues);

			return result ?? View(ISI.Extensions.AspNetCore.Tests.T4Files.Views.Public.RedirectTest_cshtml, viewModel);
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Public.RouteNames.RedirectTest, typeof(Routes.Public), "redirect-test")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> RedirectTestSaveAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Extensions.AspNetCore.Tests.Models.Public.RedirectTestModel();

			return result ?? View(ISI.Extensions.AspNetCore.Tests.T4Files.Views.Public.RedirectTest_cshtml, viewModel);
		}
	}
}