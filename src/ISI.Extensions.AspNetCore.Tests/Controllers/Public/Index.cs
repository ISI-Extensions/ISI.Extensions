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
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Public.RouteNames.Index, typeof(Routes.Public), "")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> IndexAsync()
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Extensions.AspNetCore.Tests.Models.Public.IndexModel();

			result = View(ISI.Extensions.AspNetCore.Tests.T4Files.Views.Public.Index_cshtml, viewModel);

			return result;
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Public.RouteNames.Index, typeof(Routes.Public), "")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> IndexSaveAsync()
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Extensions.AspNetCore.Tests.Models.Public.IndexModel();

			result = View(ISI.Extensions.AspNetCore.Tests.T4Files.Views.Public.Index_cshtml, viewModel);

			return result;
		}
	}
}