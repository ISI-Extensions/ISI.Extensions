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
using System.Threading.Tasks;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class HtmlHelpers
	{
		public partial class ClassNames
		{
			public const string FileInput = "file-input";
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent FileInput(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string name)
		{
			return FileInput(htmlHelper, name, null /* value */);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent FileInput(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string name, object value)
		{
			return FileInput(htmlHelper, name, value, (object)null /* htmlAttributes */);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent FileInput(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			return FileInput(htmlHelper, name, value, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent FileInput(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
		{
			return FileInputHelper(htmlHelper, name, null, value, (value == null) /* useViewData */, false /* isChecked */, true /* setId */, true /* isExplicitValue */, htmlAttributes);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static Microsoft.AspNetCore.Html.IHtmlContent FileInputFor<TModel, TProperty>(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression)
		{
			return htmlHelper.FileInputFor(expression, (IDictionary<string, object>)null);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static Microsoft.AspNetCore.Html.IHtmlContent FileInputFor<TModel, TProperty>(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return htmlHelper.FileInputFor(expression, Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static Microsoft.AspNetCore.Html.IHtmlContent FileInputFor<TModel, TProperty>(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			var expressionProvider = new Microsoft.AspNetCore.Mvc.ViewFeatures.ModelExpressionProvider(htmlHelper.MetadataProvider);

			var modelExpression = expressionProvider.CreateModelExpression(htmlHelper.ViewData, expression);

			return FileInputHelper(htmlHelper, expressionProvider.GetExpressionText(expression), modelExpression, modelExpression.Model, false /* useViewData */, false /* isChecked */, true /* setId */, true /* isExplicitValue */, htmlAttributes);
		}

		// Helper methods

		private static Microsoft.AspNetCore.Html.IHtmlContent FileInputHelper(Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string name, Microsoft.AspNetCore.Mvc.ViewFeatures.ModelExpression modelExpression, object model, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, IDictionary<string, object> htmlAttributes)
		{
			name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Value cannot be null or empty.", nameof(name));
			}

			htmlAttributes ??= new Microsoft.AspNetCore.Routing.RouteValueDictionary();

			var tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder("input");
			tagBuilder.MergeAttributes(htmlAttributes, true);
			tagBuilder.MergeAttribute("type", "file", true);
			tagBuilder.MergeAttribute("name", name, true);

			tagBuilder.AddCssClass(ClassNames.FileInput);

			if (setId)
			{
				tagBuilder.GenerateId(name, "_");
			}

			// If there are any errors for a named field, we add the css attribute.
			if (htmlHelper.ViewContext.ViewData.ModelState.TryGetValue(name, out var modelState))
			{
				if (modelState.Errors.Count > 0)
				{
					tagBuilder.AddCssClass(Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.ValidationInputCssClassName);
				}
			}

			using (var writer = new System.IO.StringWriter())
			{
				tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);

				return new Microsoft.AspNetCore.Html.HtmlString(writer.ToString());
			}
		}
	}
}
