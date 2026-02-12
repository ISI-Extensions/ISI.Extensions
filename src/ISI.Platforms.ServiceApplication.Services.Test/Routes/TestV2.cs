using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.AspNetCore;
using ISI.Extensions.AspNetCore.Extensions;

namespace ISI.Platforms.ServiceApplication.Services.Test
{
	public partial class Routes
	{
		public partial class TestV2 : IHasUrlRoute
		{
			string IHasUrlRoute.UrlRoot => UrlRoot;
			
			#pragma warning disable 649
			public class RouteNames : IRouteNames
			{
				[RouteName] public const string Index = "Index-d0ef8b91-2771-42db-940c-d48166fc2e45";
				//${RouteNames}
			}
			#pragma warning restore 649

			internal static readonly string UrlRoot = Routes.UrlRoot + "test-v2/";
		}
	}
}