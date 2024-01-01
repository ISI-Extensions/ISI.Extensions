﻿#region Copyright & License
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.VisualStudio.Forms.Extensions
{
	public static partial class VisualStudioSettingsExtensions
	{
		public static VisualStudioSettingsFormLocationAndSize GetFormLocationAndSize(this IEnumerable<VisualStudioSettingsFormLocationAndSize> formLocationAndSizes, string formName)
		{
			return formLocationAndSizes.NullCheckedFirstOrDefault(formLocationAndSize => string.Equals(formLocationAndSize.FormName, formName, StringComparison.InvariantCultureIgnoreCase));
		}

		public static void ApplyFormSize(this SolutionApi solutionApi, System.Windows.Forms.Form form)
		{
			ApplyFormSize(solutionApi, form.GetType().Name, form);
		}


		public static void ApplyFormSize(this SolutionApi solutionApi, string formName, System.Windows.Forms.Form form)
		{
			var formLocationAndSizes = solutionApi.GetVisualStudioSettings(new()).VisualStudioSettings.FormLocationAndSizes ?? Array.Empty<VisualStudioSettingsFormLocationAndSize>();

			var formSizeAndLocation = formLocationAndSizes.GetFormLocationAndSize(formName);

			if (formSizeAndLocation != null)
			{
				var workingArea = System.Windows.Forms.Screen.FromControl(form).WorkingArea;

				if (formSizeAndLocation.Top + formSizeAndLocation.Height > workingArea.Height)
				{
					formSizeAndLocation.Top = 0;
				}

				if (formSizeAndLocation.Left + formSizeAndLocation.Width > workingArea.Width)
				{
					formSizeAndLocation.Left = 0;
				}

				form.Left = formSizeAndLocation.Left;
				form.Top = formSizeAndLocation.Top;
				form.Width = formSizeAndLocation.Width;
				form.Height = formSizeAndLocation.Height;
			}
		}
	}
}