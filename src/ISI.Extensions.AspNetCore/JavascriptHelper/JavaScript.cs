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

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class JavascriptHelper
	{
		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string javaScriptUrl, object htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(javaScriptUrl)), false, false, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string javaScriptUrl, IDictionary<string, object> htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(javaScriptUrl)), false, false, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string javaScriptUrl, bool defer = false, bool async = false)
		{
			return JavaScriptHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(javaScriptUrl)), defer, async, (IDictionary<string, object>)null);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string javaScriptUrl, bool defer, bool async, object htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(javaScriptUrl)), defer, async, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string javaScriptUrl, bool defer, bool async, IDictionary<string, object> htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(javaScriptUrl)), defer, async, htmlAttributes);
		}



		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl javaScriptUrl, object htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, javaScriptUrl, false, false, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl javaScriptUrl, IDictionary<string, object> htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, javaScriptUrl, false, false, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl javaScriptUrl, bool defer = false, bool async = false)
		{
			return JavaScriptHelper(htmlHelper, javaScriptUrl, defer, async, (IDictionary<string, object>)null);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl javaScriptUrl, bool defer, bool async, object htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, javaScriptUrl, defer, async, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent JavaScript(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl javaScriptUrl, bool defer, bool async, IDictionary<string, object> htmlAttributes)
		{
			return JavaScriptHelper(htmlHelper, javaScriptUrl, defer, async, htmlAttributes);
		}



		internal static Microsoft.AspNetCore.Html.IHtmlContent JavaScriptHelper(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl javaScriptUrl, bool defer, bool async, IDictionary<string, object> htmlAttributes)
		{
			var tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder("script");
			tagBuilder.MergeAttributes(htmlAttributes, true);
			tagBuilder.MergeAttribute("type", "text/javascript");
			tagBuilder.MergeAttribute("src", javaScriptUrl.GetUrl(true), true);
			if (defer)
			{
				tagBuilder.MergeAttribute("defer", "defer", true);
			}
			if (async)
			{
				tagBuilder.MergeAttribute("async", "async", true);
			}

			return tagBuilder;
		}
	}
}
