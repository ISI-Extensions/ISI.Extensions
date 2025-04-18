﻿#region Copyright & License
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
using System.Text;

namespace ISI.Extensions.AspNetCore
{
	public interface IHasTitleModel
	{
		Microsoft.AspNetCore.Html.IHtmlContent Title { get; set; }
	}
}

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class HtmlHelpers
	{
		public static Microsoft.AspNetCore.Html.IHtmlContent GetTitle(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper)
		{
			if ((htmlHelper.ViewData.Model is ISI.Extensions.AspNetCore.IHasTitleModel hasTitleModel) && (hasTitleModel.Title != null))
			{
				return hasTitleModel.Title;
			}

			if (!string.IsNullOrEmpty(htmlHelper.ViewBag.Title))
			{
				return new Microsoft.AspNetCore.Html.HtmlString(htmlHelper.ViewBag.Title);
			}

			return Microsoft.AspNetCore.Html.HtmlString.Empty;
		}

		public static void SetTitle(this ISI.Extensions.AspNetCore.IHasTitleModel model, Microsoft.AspNetCore.Html.HtmlString title)
		{
			if (model != null)
			{
				model.Title = title;
			}
		}

		public static void SetTitle(this ISI.Extensions.AspNetCore.IHasTitleModel model, string title)
		{
			if (model != null)
			{
				model.Title = new Microsoft.AspNetCore.Html.HtmlString(title);
			}
		}

		public static void SetTitle(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, Microsoft.AspNetCore.Html.HtmlString title)
		{
			if (htmlHelper.ViewData.Model is ISI.Extensions.AspNetCore.IHasTitleModel hasTitleModel)
			{
				hasTitleModel.Title = title;
			}
			else
			{
				htmlHelper.ViewBag.Title = title;
			}
		}

		public static void SetTitle(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, string title)
		{
			if (htmlHelper.ViewData.Model is ISI.Extensions.AspNetCore.IHasTitleModel hasTitleModel)
			{
				hasTitleModel.Title = new Microsoft.AspNetCore.Html.HtmlString(title);
			}
			else
			{
				htmlHelper.ViewBag.Title = new Microsoft.AspNetCore.Html.HtmlString(title);
			}
		}
	}
}