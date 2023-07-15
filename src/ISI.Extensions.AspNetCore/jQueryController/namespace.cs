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
using System.Threading.Tasks;

namespace ISI.Extensions.AspNetCore
{
	public partial class jQueryController
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[Microsoft.AspNetCore.Authorization.AllowAnonymous]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.jQueryNamespace, "JavaScripts/jquery.namespace.js")]
		[Microsoft.AspNetCore.Mvc.ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> jQueryNamespaceAsync()
		{
			var content = @"
(function (jQuery, global) {
	jQuery.namespaceUseEvalIndex = -1;
	if (navigator.userAgent.match(/MSIE\s(?!9.0)/)) {
		// ie less than version 9
		jQuery.namespaceUseEvalIndex = 0;
	}
	jQuery.namespace = function (fullNamespace, value) {
		var result = global;

		if (typeof value === ""undefined"") {
			value = {};
		}

		var pieces = fullNamespace.split('.');

		jQuery.each(pieces, function (index, piece) {
			if (typeof result[piece] === ""undefined"") {
				if (index === jQuery.namespaceUseEvalIndex) {
					eval(piece + "" = {};"");
				} else {
					result[piece] = (index + 1 >= pieces.length ? value : {});
				}
			}
			result = result[piece];
		});

		return result;
	};
})(jQuery, this);
";

			return Content(content, ISI.Extensions.WebClient.Rest.ContentTypeJavascriptHeaderValue);
		}
	}
}