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
using ISI.Extensions.Extensions;
using ISI.Extensions.Nuget.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget.Forms
{
	public class Solution
	{
		private static ISI.Extensions.Nuget.NugetApi _nugetApi = null;
		protected ISI.Extensions.Nuget.NugetApi NugetApi => _nugetApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.NugetApi>();

		private static ISI.Extensions.VisualStudio.SolutionApi _solutionApi = null;
		protected ISI.Extensions.VisualStudio.SolutionApi SolutionApi => _solutionApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

		private string _caption = null;
		public virtual string Caption => _caption ??= SolutionDetails.SolutionName;

		public virtual ISI.Extensions.VisualStudio.SolutionDetails SolutionDetails { get; }

		public bool Selected
		{
			get => CheckBox.Checked;
			set
			{
				if (CheckBox.Checked != value)
				{
					if (CheckBox.InvokeRequired)
					{
						CheckBox.Invoke((System.Windows.Forms.MethodInvoker)delegate { CheckBox.Checked = value; });
					}
					else
					{
						CheckBox.Checked = value;
					}
				}
			}
		}

		public void SetStatus(TaskActionStatus taskActionStatus, string status)
		{
			if (StatusLabel.InvokeRequired)
			{
				StatusLabel.Invoke((System.Windows.Forms.MethodInvoker)delegate
			 {
				 StatusLabel.ForeColor = taskActionStatus.GetColor(StatusLabel);
				 StatusLabel.Text = status;
			 });
			}
			else
			{
				StatusLabel.ForeColor = taskActionStatus.GetColor(StatusLabel);
				StatusLabel.Text = status;
			}
		}

		protected Action OnChangeSelected { get; set; }

		protected internal System.Windows.Forms.CheckBox CheckBox { get; set; }
		protected internal System.Windows.Forms.Label SolutionLabel { get; set; }
		protected internal System.Windows.Forms.Label StatusLabel { get; set; }
		protected internal System.Windows.Forms.Button UpgradeNugetPackagesButton { get; set; }
		protected internal System.Windows.Forms.Button ViewLogButton { get; set; }

		protected Process.ProcessResponse UpgradeNugetPackagesResponse { get; private set; } = new();
		public bool UpgradeNugetPackagesErrored => UpgradeNugetPackagesResponse.Errored;
		private TaskActions _upgradeNugetPackages = null;
		protected TaskActions UpgradeNugetPackages => _upgradeNugetPackages ??= new()
		{
			PreAction = () =>
			{
				if (UpgradeNugetPackagesButton != null)
				{
					UpgradeNugetPackagesButton.Visible = false;
				}
				CheckBox.Enabled = false;
				ViewLogButton.Visible = false;
				SetStatus(TaskActionStatus.Default, "upgrading ...");
			},
			Action = () =>
			{
				Logger.LogInformation("Start Upgrading Solution");

				UpgradeNugetPackagesResponse = new();

				try
				{
					var upsertAssemblyRedirectsNugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

					SolutionApi.UpgradeNugetPackages(new()
					{
						SolutionFullNames = new [] { SolutionDetails.SolutionFullName },
						UpdateWorkingCopyFromSourceControl = true,
						CommitWorkingCopyToSourceControl = true,
						IgnorePackageIds = NugetApi.GetNugetSettings(new ())?.NugetSettings?.UpdateNugetPackages?.IgnorePackageIds,
						NugetPackageKeys = NugetPackageKeys,
						UpsertAssemblyRedirectsNugetPackageKeys = upsertAssemblyRedirectsNugetPackageKeys,
						AddToLog = value => UpgradeNugetPackagesResponse.AppendLine(value),
					});

					UpgradeNugetPackagesResponse.ExitCode = 0;
				}
				catch (Exception exception)
				{
					UpgradeNugetPackagesResponse.ExitCode = 1;
				}

				Logger.LogInformation("Finish Build Solution");
			},
			PostAction = () =>
			{
				SetStatus((UpgradeNugetPackagesErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (UpgradeNugetPackagesErrored ? "Errored Updating" : "Completed"));
				CheckBox.Enabled = true;

				ViewLogButton.Visible = true;
				if (UpgradeNugetPackagesButton != null)
				{
					UpgradeNugetPackagesButton.Visible = true;
				}
			}
		};

		protected System.Windows.Forms.Control SolutionPanel { get; private set; }
		public System.Windows.Forms.Control Panel { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.Nuget.NugetPackageKeyDictionary NugetPackageKeys { get; }

		public Solution(string solutionFullName, System.Windows.Forms.Control parentControl, bool highlighted, bool selected, Action<Solution> start, Action onChangeSelected, Microsoft.Extensions.Logging.ILogger logger, ISI.Extensions.Nuget.NugetPackageKeyDictionary nugetPackageKeys = null)
		{
			Logger = logger ?? new ISI.Extensions.ConsoleLogger();
			NugetPackageKeys = nugetPackageKeys ?? new NugetPackageKeyDictionary();

			var getActiveBuildConfigurationResponse = SolutionApi.GetActiveBuildConfiguration(new()
			{
				Solution = solutionFullName,
			});

			SolutionDetails = getActiveBuildConfigurationResponse.SolutionDetails;

			Panel = new System.Windows.Forms.Panel()
			{
				Width = parentControl.Width - 19,
				Height = 26,
				Margin = new(0),
			};
			parentControl.Resize += (resizeSender, resizeArgs) => { Panel.Width = parentControl.Width - 19; };

			PopulatePanel(highlighted, start);

			Selected = selected;

			OnChangeSelected = onChangeSelected;
			NugetPackageKeys = nugetPackageKeys;
		}

		protected Solution()
		{

		}

		public virtual void PopulatePanel(bool highlighted, Action<Solution> start)
		{
			SolutionPanel = new System.Windows.Forms.Panel()
			{
				Top = 0,
				Left = 0,
				Width = Panel.Width,
				Height = 24,
				Margin = new(0),
			};
			Panel.Controls.Add(SolutionPanel);
			Panel.Resize += (resizeSender, resizeArgs) => { SolutionPanel.Width = Panel.Width; };

			if (highlighted)
			{
				Panel.BackColor = (ISI.Extensions.WinForms.ThemeHelper.IsDarkTheme ? System.Drawing.Color.LightSlateGray : System.Drawing.Color.LightSkyBlue);
			}

			CheckBox = new()
			{
				Top = 6,
				Left = 6,
				Width = 17,
				Height = 17,
			};
			CheckBox.CheckedChanged += (_, __) => OnChangeSelected?.Invoke();
			SolutionPanel.Controls.Add(CheckBox);

			SolutionLabel = new()
			{
				Text = Caption,
				Top = 7,
				Left = 24,
				Width = 240,
				Height = 17,
			};
			SolutionLabel.Click += (clickSender, clickEventArgs) => Selected = !Selected;
			SolutionPanel.Controls.Add(SolutionLabel);

			StatusLabel = new()
			{
				Text = string.Empty,
				Top = 7,
				Left = 270,
				Width = 150,
				Height = 17,
			};
			SolutionPanel.Controls.Add(StatusLabel);

			UpgradeNugetPackagesButton = new()
			{
				Visible = false,
				Text = "Update",
				Top = 4,
				Left = 430,
				Width = 80,
				Height = 20,
				BackColor = System.Drawing.SystemColors.ButtonFace,
			};
			UpgradeNugetPackagesButton.Click += (_, __) =>
			{
				start(this);
			};
			SolutionPanel.Controls.Add(UpgradeNugetPackagesButton);

			ViewLogButton = new()
			{
				Visible = false,
				Text = "View Log",
				Top = 4,
				Left = 520,
				Width = 80,
				Height = 20,
				BackColor = System.Drawing.SystemColors.ButtonFace,
			};
			ViewLogButton.Click += (_, __) =>
			{
				var logs = new StringBuilder();

				if (!string.IsNullOrWhiteSpace(UpgradeNugetPackagesResponse.Output))
				{
					logs.AppendLine("Build Solution:");
					logs.AppendLine(UpgradeNugetPackagesResponse.Output);
				}

				(new ViewLogForm(logs.ToString())).ShowDialog();
			};
			SolutionPanel.Controls.Add(ViewLogButton);

			void Resize()
			{
				const int buttonWidth = 90;

				var left = SolutionPanel.Size.Width;

				foreach (var button in new[]
				{
					ViewLogButton,
					UpgradeNugetPackagesButton,
				})
				{
					if (button != null)
					{
						left -= buttonWidth;

						button.Left = left;
					}
				}

				var width = (left - SolutionLabel.Left - 20) / 2;

				SolutionLabel.Width = width;
				StatusLabel.Left = SolutionLabel.Left + width + 5;
				StatusLabel.Width = width;
			}

			Resize();

			SolutionPanel.Resize += (resizeSender, resizeArgs) => { Resize(); };
		}

		public virtual TaskActions[] GetTasks()
		{
			var tasks = new List<TaskActions>();

			tasks.Add(UpgradeNugetPackages);

			return tasks.ToArray();
		}
	}
}
