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

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class HtmlHelpers
	{
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), StylesheetMediaTypes.All, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), StylesheetMediaTypes.All, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), StylesheetMediaTypes.All, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, StylesheetMediaTypes stylesheetMediaTypes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), stylesheetMediaTypes, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, StylesheetMediaTypes stylesheetMediaTypes, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), stylesheetMediaTypes, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, StylesheetMediaTypes stylesheetMediaTypes, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), stylesheetMediaTypes, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, StylesheetContentUrl stylesheetContentUrl)
		{
			return StyleSheetHelper(htmlHelper, stylesheetContentUrl.ContentUrl, stylesheetContentUrl.Media, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, StylesheetContentUrl stylesheetContentUrl, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, stylesheetContentUrl.ContentUrl, stylesheetContentUrl.Media, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, StylesheetContentUrl stylesheetContentUrl, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, stylesheetContentUrl.ContentUrl, stylesheetContentUrl.Media, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, StylesheetMediaTypes.All, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, StylesheetMediaTypes.All, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, StylesheetMediaTypes.All, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, StylesheetMediaTypes stylesheetMediaTypes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, stylesheetMediaTypes, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, StylesheetMediaTypes stylesheetMediaTypes, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, stylesheetMediaTypes, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, StylesheetMediaTypes stylesheetMediaTypes, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, stylesheetMediaTypes, htmlAttributes);
		}

		internal static Microsoft.AspNetCore.Html.IHtmlContent StyleSheetHelper(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, StylesheetMediaTypes stylesheetMediaTypes, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, (stylesheetMediaTypes != StylesheetMediaTypes.All ? HtmlHelpers.CommaDelimitedList(stylesheetMediaTypes) : null), htmlAttributes);
		}


		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, string stylesheetMediaType)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), stylesheetMediaType, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, string stylesheetMediaType, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), stylesheetMediaType, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, string stylesheetMediaType, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(styleSheetUrl)), stylesheetMediaType, htmlAttributes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, string stylesheetMediaType)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, stylesheetMediaType, (IDictionary<string, object>)null);
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, string stylesheetMediaType, object htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, stylesheetMediaType, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		public static Microsoft.AspNetCore.Html.IHtmlContent StyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, string stylesheetMediaType, IDictionary<string, object> htmlAttributes)
		{
			return StyleSheetHelper(htmlHelper, styleSheetContentUrl, stylesheetMediaType, htmlAttributes);
		}

		internal static Microsoft.AspNetCore.Html.IHtmlContent StyleSheetHelper(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, string stylesheetMediaType, IDictionary<string, object> htmlAttributes)
		{
			var tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder("link");
			tagBuilder.MergeAttributes(htmlAttributes, true);
			tagBuilder.MergeAttribute("rel", "stylesheet");
			tagBuilder.MergeAttribute("type", "text/css");
			tagBuilder.MergeAttribute("href", styleSheetContentUrl.GetUrl(true), true);

			if (styleSheetContentUrl is StylesheetContentUrl url)
			{
				var media = url.Media;

				if (!string.IsNullOrEmpty(media))
				{
					tagBuilder.MergeAttribute("media", media, true);
				}
			}
			
			if (!string.IsNullOrEmpty(stylesheetMediaType))
			{
				tagBuilder.MergeAttribute("media", stylesheetMediaType, true);
			}

			return tagBuilder;
		}
	}
}
