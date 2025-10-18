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

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class HtmlAttributeHelpers
	{
		private const string ClassAttributeName = "class";

		public static HtmlAttributeCollection CssClass(this HtmlAttributeCollection htmlAttributes, string cssClass)
		{
			CssClassHelper(htmlAttributes, cssClass);

			return htmlAttributes;
		}

		public static HtmlAttributeCollection CssClass(this HtmlAttributeCollection htmlAttributes, IEnumerable<string> cssClasses)
		{
			CssClassHelper(htmlAttributes, cssClasses);

			return htmlAttributes;
		}

		public static HtmlAttributeCollection<TModel> CssClass<TModel>(this HtmlAttributeCollection<TModel> htmlAttributes, string cssClass)
		{
			CssClassHelper(htmlAttributes, cssClass);

			return htmlAttributes;
		}

		public static HtmlAttributeCollection<TModel> CssClass<TModel>(this HtmlAttributeCollection<TModel> htmlAttributes, IEnumerable<string> cssClasses)
		{
			CssClassHelper(htmlAttributes, cssClasses);

			return htmlAttributes;
		}

		private static HashSet<string> GetCssClasses(AbstractHtmlAttributeCollection htmlAttributes)
		{
			object cssClasses = string.Empty;

			htmlAttributes.TryGetValue(ClassAttributeName, out cssClasses);

			return new HashSet<string>($"{cssClasses}".Split([' '], StringSplitOptions.RemoveEmptyEntries), StringComparer.InvariantCulture);
		}

		private static void CssClassHelper(AbstractHtmlAttributeCollection htmlAttributes, string cssClass)
		{
			var currentCssClasses = GetCssClasses(htmlAttributes);

			currentCssClasses.UnionWith(cssClass.Split([' '], StringSplitOptions.RemoveEmptyEntries));

			htmlAttributes[ClassAttributeName] = string.Join(" ", currentCssClasses);
		}

		private static void CssClassHelper(AbstractHtmlAttributeCollection htmlAttributes, IEnumerable<string> cssClasses)
		{
			var currentCssClasses = GetCssClasses(htmlAttributes);

			currentCssClasses.UnionWith(cssClasses);

			htmlAttributes[ClassAttributeName] = string.Join(" ", currentCssClasses);
		}
	}
}
