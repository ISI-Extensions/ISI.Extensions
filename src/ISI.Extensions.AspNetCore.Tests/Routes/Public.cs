using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.AspNetCore.Tests
{
	public partial class Routes
	{
		public partial class Public : IHasUrlRoute
		{
			string IHasUrlRoute.UrlRoot => UrlRoot;
			
			#pragma warning disable 649
			public class RouteNames : IRouteNames
			{
				[RouteName] public const string Index = "Index-d5e85c00-0711-412d-853c-53b2a5ac7f8f";
				//${RouteNames}
			}
			#pragma warning restore 649

			internal static readonly string UrlRoot = Routes.UrlRoot;
		}
	}
}