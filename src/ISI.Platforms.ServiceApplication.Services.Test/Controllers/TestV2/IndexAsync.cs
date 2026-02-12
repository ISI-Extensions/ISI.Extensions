using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Platforms.ServiceApplication.Services.Test.Controllers
{
	public partial class TestV2Controller 
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.TestV2.RouteNames.Index, typeof(Routes.TestV2), "")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> IndexAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Platforms.ServiceApplication.Services.Test.Models.TestV2.IndexModel();

			
			return result ?? View(ISI.Platforms.ServiceApplication.Services.Test.T4Files.Views.TestV2.Index_cshtml, viewModel);
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.TestV2.RouteNames.Index, typeof(Routes.TestV2), "")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> IndexSaveAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Platforms.ServiceApplication.Services.Test.Models.TestV2.IndexModel();

			return result ?? View(ISI.Platforms.ServiceApplication.Services.Test.T4Files.Views.TestV2.Index_cshtml, viewModel);
		}
	}
}