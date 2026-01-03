#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
	[Microsoft.AspNetCore.Mvc.ModelBinderAttribute(BinderType = typeof(AllRequestParametersModelBinder))]
	public class AllRequestParameters
	{
		public Dictionary<string, string> QueryStringValues { get; } = new(StringComparer.InvariantCultureIgnoreCase);
		public Dictionary<string, string> FormValues { get; } = new(StringComparer.InvariantCultureIgnoreCase);
		public Dictionary<string, string> RouteDataValues { get; } = new(StringComparer.InvariantCultureIgnoreCase);

		public bool TryGetValue(string key, out string value)
		{
			if (QueryStringValues.TryGetValue(key, out var queryStringValue))
			{
				value = queryStringValue;
				return true;
			}

			if (FormValues.TryGetValue(key, out var formValue))
			{
				value = formValue;
				return true;
			}

			if (RouteDataValues.TryGetValue(key, out var routeDataValues))
			{
				value = routeDataValues;
				return true;
			}

			value = null;
			return false;
		}
	}

	public class AllRequestParametersModelBinder : Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder
	{
		public Task BindModelAsync(Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext bindingContext)
		{
			var model = new AllRequestParameters();

			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			foreach (var key in bindingContext.HttpContext.Request.Query.Keys)
			{
				if(bindingContext.HttpContext.Request.Query.TryGetValue(key, out var queryStringValues))
				{
					model.QueryStringValues.TryAdd(key, queryStringValues.First());
				}
			}

			foreach (var key in bindingContext.HttpContext.Request.Form?.Keys ?? [])
			{
				if(bindingContext.HttpContext.Request.Form.TryGetValue(key, out var formStringValues))
				{
					model.FormValues.TryAdd(key, formStringValues.First());
				}
			}

			foreach (var key in bindingContext.HttpContext.Request.RouteValues?.Keys ?? [])
			{
				if(bindingContext.HttpContext.Request.RouteValues.TryGetValue(key, out var routeValue))
				{
					model.RouteDataValues.TryAdd(key, $"{routeValue}");
				}
			}
			
			bindingContext.Result = Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.Success(model);

			return Task.CompletedTask;
		}
	}
}
