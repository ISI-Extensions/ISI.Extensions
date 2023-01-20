#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class RouteBuilderExtensions
	{
		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRoute<TController>(this Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string url, Expression<Func<TController, object>> actionName) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			return routeBuilder.MapRoute<TController>(url, actionName, (object)null /* defaults */);
		}

		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRoute<TController>(this Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string url, Expression<Func<TController, object>> actionName, object defaults) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			return routeBuilder.MapRoute<TController>(url, actionName, defaults, (object)null /* constraints */);
		}

		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRoute<TController>(this Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string url, Expression<Func<TController, object>> actionName, object defaults, object constraints) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			return MapRouteHelper<TController>(routeBuilder, null, url, actionName, defaults, constraints);
		}

		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRoute<TController>(this Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string routeName, string url, Expression<Func<TController, object>> actionName) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			return routeBuilder.MapRoute<TController>(routeName, url, actionName, (object)null /* defaults */);
		}

		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRoute<TController>(this Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string routeName, string url, Expression<Func<TController, object>> actionName, object defaults) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			return routeBuilder.MapRoute<TController>(routeName, url, actionName, defaults, (object)null /* constraints */);
		}

		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRoute<TController>(this Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string routeName, string url, Expression<Func<TController, object>> actionName, object defaults, object constraints) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			if (routeName == null)
			{
				throw new ArgumentNullException(nameof(routeName));
			}

			return MapRouteHelper<TController>(routeBuilder, routeName, url, actionName, defaults, constraints);
		}




		public static Microsoft.AspNetCore.Routing.IRouteBuilder MapRouteHelper<TController>(Microsoft.AspNetCore.Routing.IRouteBuilder routeBuilder, string routeName, string url, Expression<Func<TController, object>> actionName, object defaults, object constraints) 
			where TController : Microsoft.AspNetCore.Mvc.ControllerBase
		{
			if (routeBuilder == null)
			{
				throw new ArgumentNullException(nameof(routeBuilder));
			}

			if (url == null)
			{
				throw new ArgumentNullException(nameof(url));
			}

			//var defaultValues = new System.Web.Routing.RouteValueDictionary(defaults);

			//defaultValues["controller"] = RouteExtensions.ControllerName<TController>();
			//defaultValues["action"] = RouteExtensions.ActionName<TController>(actionName);

			//if ((constraints != null) && (constraints is System.Web.Routing.IRouteConstraint))
			//{
			//	constraints = new {routeConstraint = constraints};
			//}



			//if (context != null)
			//{
			//	route.DataTokens["area"] = context.AreaName;
			//}

			//if (!string.IsNullOrEmpty(routeName))
			//{
			//	route.DataTokens[RouteName.RouteNameKey] = routeName;
			//}

			//route.DataTokens["Namespaces"] = new [] { RouteExtensions.ControllerNamespace<TController>() };



			return routeBuilder.MapRoute(routeName, url, new { controller = RouteExtensions.ControllerName<TController>(), action = RouteExtensions.ActionName<TController>(actionName) });
		}
	}
}
