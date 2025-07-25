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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public delegate string ModifyApplicationName(string applicationName);
	public static partial class HtmlHelpers
	{
		public static Microsoft.AspNetCore.Html.IHtmlContent GetApplicationName(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ModifyApplicationName modifyApplicationName = null)
		{
			var executingAssembly = System.Reflection.Assembly.GetEntryAssembly();

			var applicationName = executingAssembly.FullName.Split(new[] { ',' }).First();

			if (modifyApplicationName != null)
			{
				applicationName = modifyApplicationName(applicationName);
			}

			return new Microsoft.AspNetCore.Html.HtmlString(applicationName);
		}

		public delegate string ModifyApplicationDisplayName(string applicationDisplayName);
		public static Microsoft.AspNetCore.Html.IHtmlContent GetApplicationDisplayName(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper, ModifyApplicationDisplayName modifyApplicationDisplayName = null)
		{
			var executingAssembly = System.Reflection.Assembly.GetEntryAssembly();

			var applicationDisplayName = executingAssembly.FullName.Split(new[] { ',' }).First().TrimEnd(".ServiceApplication", ".WindowsService");

			if (modifyApplicationDisplayName != null)
			{
				applicationDisplayName = modifyApplicationDisplayName(applicationDisplayName);
			}

			return new Microsoft.AspNetCore.Html.HtmlString(applicationDisplayName);
		}

		public static Microsoft.AspNetCore.Html.IHtmlContent GetApplicationVersion(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper)
		{
			var executingAssembly = System.Reflection.Assembly.GetEntryAssembly();

			var applicationVersion = ISI.Extensions.SystemInformation.GetAssemblyVersion(executingAssembly);

			return new Microsoft.AspNetCore.Html.HtmlString(applicationVersion);
		}
	}
}