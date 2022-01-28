#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Nuget.Forms
{
	public class NugetPackage
	{
		public NugetPackageKey NugetPackageKey { get; }

		private string _caption = null;
		public virtual string Caption => _caption ??= string.Format("{0} -version {1}", NugetPackageKey.Package, NugetPackageKey.Version);

		public bool Selected
		{
			get => CheckBox.Checked;
			set => CheckBox.Checked = value;
		}

		protected internal System.Windows.Forms.CheckBox CheckBox { get; set; }
		protected internal System.Windows.Forms.Label NugetLabel { get; set; }

		public System.Windows.Forms.Control Panel { get; }

		public NugetPackage(NugetPackageKey nugetPackageKey, System.Windows.Forms.Control parentControl, bool highlighted, bool selected)
		{
			NugetPackageKey = nugetPackageKey;

			Panel = new System.Windows.Forms.Panel()
			{
				Width = parentControl.Width - 19,
				Height = 26,
				Margin = new System.Windows.Forms.Padding(0),
			};
			parentControl.Controls.Add(Panel);

			PopulatePanel(parentControl, highlighted, selected);
		}

		public virtual void PopulatePanel(System.Windows.Forms.Control parentControl, bool highlighted, bool selected)
		{
			if (highlighted)
			{
				Panel.BackColor = System.Drawing.Color.LightSkyBlue;
			}

			CheckBox = new System.Windows.Forms.CheckBox()
			{
				Top = 6,
				Left = 6,
				Width = 17,
				Height = 17,
				Checked = selected,
			};
			Panel.Controls.Add(CheckBox);

			NugetLabel = new System.Windows.Forms.Label()
			{
				Text = Caption,
				Top = 7,
				Left = 24,
				Width = 240,
				Height = 17,
#if DEBUG
				BackColor = System.Drawing.Color.Chartreuse,
#endif
			};
			NugetLabel.Click += (clickSender, clickEventArgs) => Selected = !Selected;
			Panel.Controls.Add(NugetLabel);

		

			//void Resize()
			//{
			//	const int buttonWidth = 90;

			//	var left = SolutionPanel.Size.Width;

			//	foreach (var button in new[]
			//	{
			//		ViewLogButton,
			//		StopButton,
			//		OpenButton,
			//	})
			//	{
			//		left -= buttonWidth;

			//		button.Left = left;
			//	}

			//	var width = (left - SolutionLabel.Left - 20) / 2;

			//	SolutionLabel.Width = width;
			//	StatusLabel.Left = SolutionLabel.Left + width + 5;
			//	StatusLabel.Width = width;
			//}

			//Resize();

			//SolutionPanel.Resize += (resizeSender, resizeArgs) => { Resize(); };
		}
	}
}
