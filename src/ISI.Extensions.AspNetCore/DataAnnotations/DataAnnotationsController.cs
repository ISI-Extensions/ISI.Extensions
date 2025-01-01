#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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

namespace ISI.Extensions.AspNetCore
{
	public class DataAnnotationsController : Microsoft.AspNetCore.Mvc.Controller
	{
		public class Routes
		{
			public const string DataAnnotationValidationRules = "DataAnnotations-b1c358f2-5113-4bd3-993d-ff683bd1e0d2";
		}

		public class Urls
		{
			public const string DataAnnotationValidationRules = "/JavaScripts/data-annotations/validation-rules.js";
		}

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public DataAnnotationsController(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}

		private string _dataAnnotationValidationRulesContent = null;
		protected string DataAnnotationValidationRules => _dataAnnotationValidationRulesContent ??= GetDataAnnotationValidationRules();

		protected string GetDataAnnotationValidationRules()
		{
			var response = new StringBuilder();

			response.AppendLine("(function (jQuery) {");

			response.AppendLine(ISI.Extensions.AspNetCore.DataAnnotations.DataAnnotationsValidationRules.JavaScriptUnobtrusiveValidationRules());

			response.AppendLine("}(jQuery));");

			return response.ToString();
		}


		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.DataAnnotationValidationRules, Urls.DataAnnotationValidationRules)]
		[Microsoft.AspNetCore.Mvc.ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> DataAnnotationValidationRulesAsync()
		{
			return Content(DataAnnotationValidationRules, ISI.Extensions.WebClient.Rest.ContentTypeJavascriptHeaderValue);
		}
	}
}