using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Platforms.ServiceApplication.Test.Controllers
{
	public partial class PublicController 
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.Authorize(Policy = Program.AuthorizationPolicyName)]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Public.RouteNames.AuthorizationTest, typeof(Routes.Public), "authorization-test")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> AuthorizationTestAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Platforms.ServiceApplication.Test.Models.Public.AuthorizationTestModel();

			return result ?? View(ISI.Platforms.ServiceApplication.Test.T4Files.Views.Public.AuthorizationTest_cshtml, viewModel);
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[Microsoft.AspNetCore.Authorization.Authorize(Policy = Program.AuthorizationPolicyName)]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Public.RouteNames.AuthorizationTest, typeof(Routes.Public), "authorization-test")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> AuthorizationTestSaveAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new ISI.Platforms.ServiceApplication.Test.Models.Public.AuthorizationTestModel();

			return result ?? View(ISI.Platforms.ServiceApplication.Test.T4Files.Views.Public.AuthorizationTest_cshtml, viewModel);
		}
	}
}