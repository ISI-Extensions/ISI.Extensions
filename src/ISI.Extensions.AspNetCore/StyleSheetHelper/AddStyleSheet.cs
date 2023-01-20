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

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class StyleSheetHelper
	{
		public static Microsoft.AspNetCore.Html.IHtmlContent AddStyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, HtmlHelpers.StylesheetContentUrl stylesheetContentUrl)
		{
			Microsoft.AspNetCore.Html.IHtmlContent result = null;

			if (IsCombineStyleSheetEnabled)
			{
				var styleSheetUrls = htmlHelper.GetStyleSheetMediaTypeUrls();

				styleSheetUrls.Add(stylesheetContentUrl);
			}
			else
			{
				result = htmlHelper.StyleSheet(stylesheetContentUrl);
			}

			return result ?? Microsoft.AspNetCore.Html.HtmlString.Empty;
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent AddStyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, HtmlHelpers.StylesheetMediaTypes stylesheetMediaTypes = HtmlHelpers.StylesheetMediaTypes.All)
		{
			return AddStyleSheet(htmlHelper, new ISI.Extensions.AspNetCore.ContentUrl(styleSheetUrl), stylesheetMediaTypes);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent AddStyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string styleSheetUrl, string stylesheetMediaType)
		{
			return AddStyleSheet(htmlHelper, new(stylesheetMediaType, styleSheetUrl));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent AddStyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, string stylesheetMediaType)
		{
			return AddStyleSheet(htmlHelper, new(stylesheetMediaType, styleSheetContentUrl));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent AddStyleSheet(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ISI.Extensions.AspNetCore.IContentUrl styleSheetContentUrl, HtmlHelpers.StylesheetMediaTypes stylesheetMediaTypes = HtmlHelpers.StylesheetMediaTypes.All)
		{
			Microsoft.AspNetCore.Html.IHtmlContent result = null;

			if (IsCombineStyleSheetEnabled)
			{
				var styleSheetUrls = htmlHelper.GetStyleSheetMediaTypeUrls();

				if (styleSheetContentUrl is HtmlHelpers.StylesheetContentUrl sheetContentUrl)	
				{
					styleSheetContentUrl = sheetContentUrl.ContentUrl;
				}

				if (stylesheetMediaTypes != HtmlHelpers.StylesheetMediaTypes.All)
				{
					styleSheetContentUrl = new HtmlHelpers.StylesheetContentUrl(HtmlHelpers.CommaDelimitedList(stylesheetMediaTypes), styleSheetContentUrl);
				}

				styleSheetUrls.Add(styleSheetContentUrl);
			}
			else
			{
				if (stylesheetMediaTypes != HtmlHelpers.StylesheetMediaTypes.All)
				{
					result = htmlHelper.StyleSheet(styleSheetContentUrl.GetUrl(true), stylesheetMediaTypes);
				}
				else
				{
					result = htmlHelper.StyleSheet(styleSheetContentUrl.GetUrl(true));
				}
			}

			return result ?? Microsoft.AspNetCore.Html.HtmlString.Empty;
		}
	}
}