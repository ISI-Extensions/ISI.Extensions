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

namespace ISI.Extensions.WinForms
{
	public class ThemeHelper
	{
		public static bool IsDarkTheme => Dark.Net.DarkNet.Instance.EffectiveCurrentProcessThemeIsDark;

		public static void SetWindowThemeForms(System.Windows.Forms.Form form)
		{
			Dark.Net.DarkNet.Instance.SetWindowThemeForms(form, Dark.Net.Theme.Dark, new Dark.Net.ThemeOptions()
			{
				ApplyThemeToDescendentFormsScrollbars = true,
			});

			form.BackColor = IsDarkTheme ? System.Drawing.Color.FromArgb(19, 19, 19) : System.Drawing.Color.White;
			form.ForeColor = IsDarkTheme ? System.Drawing.Color.White : System.Drawing.Color.Black;
			ChangeTheme(form.Controls, form.BackColor, form.ForeColor);

		}

		public static void ChangeTheme(System.Windows.Forms.Control.ControlCollection container, System.Drawing.Color backColor, System.Drawing.Color foreColor)
		{
			foreach (System.Windows.Forms.Control component in container)
			{
				if (component is System.Windows.Forms.Panel)
				{
					ChangeTheme(component.Controls, backColor, foreColor);
					component.BackColor = backColor;
					component.ForeColor = foreColor;
				}
				else if (component is System.Windows.Forms.Button)
				{
					component.BackColor = backColor;
					component.ForeColor = foreColor;
				}
				else if (component is System.Windows.Forms.TextBox)
				{
					component.BackColor = backColor;
					component.ForeColor = foreColor;
				}
			}
		}

	}
}
