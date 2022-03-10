#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Razor
{
	public partial class RazorService : IRazorService
	{
		protected IServiceProvider ServiceProvider { get; }
		protected Microsoft.AspNetCore.Mvc.Razor.IRazorViewEngine RazorViewEngine { get; }

		public RazorService(Microsoft.AspNetCore.Mvc.Razor.IRazorViewEngine razorViewEngine, IServiceProvider serviceProvider)
		{
			RazorViewEngine = razorViewEngine;
			ServiceProvider = serviceProvider;
		}

		public async Task<string> Render(string viewPath, object model = null)
		{
			var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
			{
				RequestServices = ServiceProvider
			};

			var routeData = new Microsoft.AspNetCore.Routing.RouteData();
			var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();
			var modelStateDictionary = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
			var modelMetadataProvider = new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider();
			var tempDataProvider = new VirtualTempDataProvider();
			var htmlHelperOptions = new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions();

			var actionContext = new Microsoft.AspNetCore.Mvc.ActionContext(httpContext, routeData, actionDescriptor, modelStateDictionary);
			var viewDataDictionary = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(modelMetadataProvider, modelStateDictionary);
			var tempDataDictionary = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(httpContext, tempDataProvider);

			viewDataDictionary.Model = model;

			using (var stringWriter = new System.IO.StringWriter())
			{
				var view = RazorViewEngine.GetView(string.Empty, viewPath, true);

				var viewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(actionContext, view.View, viewDataDictionary, tempDataDictionary, stringWriter, htmlHelperOptions);

				await view.View.RenderAsync(viewContext);

				return stringWriter.ToString();
			}
		}
	}
}