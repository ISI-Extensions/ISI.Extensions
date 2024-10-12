#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class ControllerExtensions
	{
		public static async System.Threading.Tasks.Task<string> RenderViewToStringAsync<TModel>(this Microsoft.AspNetCore.Mvc.Controller controller, string viewName, TModel model)
		{
			controller.ViewData.Model = model;

			if (string.IsNullOrWhiteSpace(viewName))
			{
				viewName = controller.ControllerContext.ActionDescriptor.ActionName;
			}

			using (var writer = new System.IO.StringWriter())
			{
				try
				{
					var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;

					var viewResult = viewName.EndsWith(".cshtml") ? viewEngine.GetView(viewName, viewName, false) : viewEngine.FindView(controller.ControllerContext, viewName, false);

					if (!viewResult.Success)
					{
						return $"View: \"{viewName}\" not found";
					}

					var viewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer, new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions());

					await viewResult.View.RenderAsync(viewContext);

					return writer.GetStringBuilder().ToString();
				}
				catch (Exception exception)
				{
					return exception.ErrorMessageFormatted();
				}
			}
		}

		public static async System.Threading.Tasks.Task<string> RenderViewToStringAsync(this Microsoft.AspNetCore.Mvc.Controller controller, string viewName)
		{
			if (string.IsNullOrWhiteSpace(viewName))
			{
				viewName = controller.ControllerContext.ActionDescriptor.ActionName;
			}

			using (var writer = new System.IO.StringWriter())
			{
				try
				{
					var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;

					var viewResult = viewName.EndsWith(".cshtml") ? viewEngine.GetView(viewName, viewName, false) : viewEngine.FindView(controller.ControllerContext, viewName, false);

					if (!viewResult.Success)
					{
						return $"View: \"{viewName}\" not found";
					}

					var viewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer, new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions());

					await viewResult.View.RenderAsync(viewContext);

					return writer.GetStringBuilder().ToString();
				}
				catch (Exception exception)
				{
					return exception.ErrorMessageFormatted();
				}
			}
		}
	}
}
