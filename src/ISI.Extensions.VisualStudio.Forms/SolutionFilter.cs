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

namespace ISI.Extensions.VisualStudio.Forms
{
	public class SolutionFilter
	{
		public Solution Solution { get; }
		public SolutionFilterKey SolutionFilterKey { get; }

		public string SolutionFilterFullName => SolutionFilterKey.SolutionFilterFullName;

		private string _solutionFilterName = null;
		public string SolutionFilterName => _solutionFilterName ??= System.IO.Path.GetFileNameWithoutExtension(SolutionFilterFullName);

		private string _caption = null;
		public virtual string Caption => _caption ??= SolutionFilterName;

		public bool Selected
		{
			get => RadioButton.Checked;
			set
			{
				if (RadioButton.Checked != value)
				{
					if (RadioButton.InvokeRequired)
					{
						RadioButton.Invoke((System.Windows.Forms.MethodInvoker)delegate { RadioButton.Checked = value; });
					}
					else
					{
						RadioButton.Checked = value;
					}
				}
			}
		}

		protected Action OnChangeSelected { get; set; }

		protected internal System.Windows.Forms.RadioButton RadioButton { get; set; }
		protected internal System.Windows.Forms.Label SolutionFilterLabel { get; set; }

		public System.Windows.Forms.Control Panel { get; }

		public SolutionFilter(Solution solution, SolutionFilterKey solutionFilterKey, Action<Solution> start, bool selected, Action onChangeSelected)
		{
			Solution = solution;
			SolutionFilterKey = solutionFilterKey;

			Panel = new System.Windows.Forms.Panel()
			{
				Width = solution.Panel.Width,
				Height = 24,
				Margin = new(0),
			};
			solution.Panel.Resize += (resizeSender, resizeArgs) =>
			{
				Panel.Width = solution.Panel.Width;
			};

			PopulatePanel(start);

			Selected = selected;

			OnChangeSelected = onChangeSelected;
		}

		public virtual void PopulatePanel(Action<Solution> start)
		{
			RadioButton = new()
			{
				Top = 6,
				Left = 36,
				Width = 17,
				Height = 17,
				Checked = SolutionFilterKey.Selected,
			};
			RadioButton.CheckedChanged += (_, __) =>
			{
				if (RadioButton.Checked)
				{
					foreach (var solutionFilter in Solution.SolutionProjects)
					{
						if (!string.Equals(SolutionFilterKey.Value, solutionFilter.ProjectKey.Value, StringComparison.InvariantCultureIgnoreCase))
						{
							solutionFilter.Selected = false;
						}
					}
				}

				OnChangeSelected?.Invoke();
			};
			Panel.Controls.Add(RadioButton);

			SolutionFilterLabel = new()
			{
				Text = Caption,
				AutoSize = false,
				Top = 7,
				Left = 54,
				Width = 240,
				Height = 17,
#if DEBUG
				//BackColor = System.Drawing.Color.Violet,
#endif
			};
			SolutionFilterLabel.Click += (clickSender, clickEventArgs) => Selected = !Selected;
			Panel.Controls.Add(SolutionFilterLabel);

			void Resize()
			{
				const int buttonWidth = 90;

				var left = Panel.Size.Width;

				var width = (left - SolutionFilterLabel.Left - 20) / 2;

				SolutionFilterLabel.Width = width;
			}

			Resize();

			Panel.Resize += (resizeSender, resizeArgs) => { Resize(); };
		}
	}
}
