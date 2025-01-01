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
using ISI.Extensions.Extensions;
using ISI.Extensions.VisualStudioCode.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudioCode.Forms
{
	public class Solution
	{
		private static ISI.Extensions.Scm.ISourceControlClientApi _sourceControlClientApi = null;
		protected ISI.Extensions.Scm.ISourceControlClientApi SourceControlClientApi => _sourceControlClientApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.ISourceControlClientApi>();

		private static ISI.Extensions.VisualStudioCode.SolutionApi _solutionApi = null;
		protected ISI.Extensions.VisualStudioCode.SolutionApi SolutionApi => _solutionApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudioCode.SolutionApi>();

		private static ISI.Extensions.VisualStudioCode.NodeModulesApi _nodeModulesApi = null;
		protected ISI.Extensions.VisualStudioCode.NodeModulesApi NodeModulesApi => _nodeModulesApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudioCode.NodeModulesApi>();

		private string _caption = null;
		public virtual string Caption => _caption ??= SolutionDetails.SolutionName;

		public virtual SolutionDetails SolutionDetails { get; }

		protected bool ShowRunSolutionButton { get; }

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
		protected internal System.Windows.Forms.Button RefreshButton { get; set; }
		protected internal System.Windows.Forms.Button OpenButton { get; set; }
		protected internal System.Windows.Forms.Button RunSolutionButton { get; set; }
		protected internal System.Windows.Forms.Button ViewBuildLogButton { get; set; }

		protected internal void SetButtonVisibility(System.Windows.Forms.Button button, bool isVisible)
		{
			if (button != null)
			{
				if (button.InvokeRequired)
				{
					button.Invoke((System.Windows.Forms.MethodInvoker)delegate
					{
						button.Visible = isVisible;
					});
				}
				else
				{
					button.Visible = isVisible;
				}
			}
		}

		protected internal void SetCheckBoxEnabled(System.Windows.Forms.CheckBox checkBox, bool isEnabled)
		{
			if (checkBox != null)
			{
				if (checkBox.InvokeRequired)
				{
					checkBox.Invoke((System.Windows.Forms.MethodInvoker)delegate
					{
						checkBox.Enabled = isEnabled;
					});
				}
				else
				{
					checkBox.Enabled = isEnabled;
				}
			}
		}


		protected internal System.Diagnostics.Stopwatch Stopwatch { get; } = new();

		public Process.ProcessResponse UpgradeNodeModulesResponse { get; private set; } = new();
		protected bool UpgradeNodeModulesErrored => UpgradeNodeModulesResponse.Errored;

		public void UpgradeNodeModulesPreAction()
		{
			if (!CleanSolutionErrored && !UpdateSolutionErrored)
			{
				SetButtonVisibility(RefreshButton, false);
				SetButtonVisibility(OpenButton, false);
				SetButtonVisibility(RunSolutionButton, false);
				SetCheckBoxEnabled(CheckBox, false);
				SetButtonVisibility(ViewBuildLogButton, false);
				SetStatus(TaskActionStatus.Default, "upgrading node modules ...");

				Logger.LogInformation("Start Upgrade Node Modules");
			}
		}

		public void UpgradeNodeModulesPostAction(bool showOpenButton, bool showRefreshButton)
		{
			Logger.LogInformation("Finish Upgrade Node Modules");

			if (!CleanSolutionErrored && !UpdateSolutionErrored)
			{
				SetStatus((UpgradeNodeModulesErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (UpgradeNodeModulesErrored ? "Errored Upgrading Node Modules" : "Completed"));
				SetCheckBoxEnabled(CheckBox, true);
				SetButtonVisibility(OpenButton, showOpenButton);
				SetButtonVisibility(RefreshButton, showRefreshButton);
				SetButtonVisibility(RunSolutionButton, ShowRunSolutionButton);
			}
			SetButtonVisibility(ViewBuildLogButton, true);
		}

		protected Process.ProcessResponse CleanSolutionResponse { get; private set; } = new();
		protected bool CleanSolutionErrored { get; private set; }
		private TaskActions _cleanSolution = null;
		protected TaskActions CleanSolution => _cleanSolution ??= new()
		{
			PreAction = () =>
			{
				if (!UpgradeNodeModulesErrored)
				{
					SetButtonVisibility(RefreshButton, false);
					SetButtonVisibility(OpenButton, false);
					SetButtonVisibility(RunSolutionButton, false);
					SetCheckBoxEnabled(CheckBox, false);
					SetButtonVisibility(ViewBuildLogButton, false);
					SetStatus(TaskActionStatus.Default, "cleaning ...");
				}
			},
			Action = () =>
			{
				if (!UpgradeNodeModulesErrored)
				{
					Logger.LogInformation("Start Clean Solution");

					CleanSolutionErrored = !(NodeModulesApi.CleanSolution(new()
					{
						Solution = SolutionDetails.SolutionDirectory,

						AddToLog = (logEntryLevel, description) =>
						{
							UpdateStatus(description);
							CleanSolutionResponse.AppendLine(description);
							Logger.LogInformation(description);
						},
					}).Success);

					Logger.LogInformation("Finish Clean Solution");
				}
			},
			PostAction = () =>
			{
				if (!UpgradeNodeModulesErrored)
				{
					SetStatus((CleanSolutionErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (CleanSolutionErrored ? "Errored Cleaning" : "Completed"));
					SetCheckBoxEnabled(CheckBox, true);
					SetButtonVisibility(OpenButton, true);
					SetButtonVisibility(RunSolutionButton, ShowRunSolutionButton);
					SetButtonVisibility(RefreshButton, true);
				}
			}
		};

		public Process.ProcessResponse UpdateSolutionResponse { get; private set; } = new();
		protected bool UpdateSolutionErrored => UpdateSolutionResponse.Errored;
		private TaskActions _updateSolution = null;
		protected TaskActions UpdateSolution => _updateSolution ??= new()
		{
			PreAction = () =>
			{
				SetButtonVisibility(RefreshButton, false);
				SetButtonVisibility(OpenButton, false);
				SetButtonVisibility(RunSolutionButton, false);
				SetCheckBoxEnabled(CheckBox, false);
				SetButtonVisibility(ViewBuildLogButton, false);
				SetStatus(TaskActionStatus.Default, "updating from source control ...");
			},
			Action = () =>
			{
				if (string.IsNullOrWhiteSpace(SolutionDetails.RootSourceDirectory))
				{
					Logger.LogInformation("Not under source control");

					UpdateSolutionResponse.ExitCode = 0;
				}
				else
				{
					Logger.LogInformation("Start Update Solution");

					UpdateSolutionResponse.ExitCode = SourceControlClientApi.UpdateWorkingCopy(new()
					{
						FullName = SolutionDetails.RootSourceDirectory,
						IncludeExternals = true,

						AddToLog = (logEntryLevel, description) =>
						{
							UpdateStatus(description);
							UpdateSolutionResponse.AppendLine(description);
							Logger.LogInformation(description);
						},
					}).Success ? 0 : 1;

					Logger.LogInformation("Finish Update Solution");
				}
			},
			PostAction = () =>
			{
				SetStatus((UpdateSolutionErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (UpdateSolutionErrored ? "Errored Updating" : "Completed"));
				SetCheckBoxEnabled(CheckBox, true);
				SetButtonVisibility(OpenButton, true);
				SetButtonVisibility(RunSolutionButton, ShowRunSolutionButton);
				SetButtonVisibility(RefreshButton, true);
				SetButtonVisibility(ViewBuildLogButton, true);
			}
		};

		protected Process.ProcessResponse InstallNodeModulesResponse { get; private set; } = new();
		protected bool InstallNodeModulesErrored => InstallNodeModulesResponse.Errored;
		private TaskActions _installNodeModules = null;
		protected TaskActions InstallNodeModules => _installNodeModules ??= new()
		{
			PreAction = () =>
			{
				if (!UpgradeNodeModulesErrored && !CleanSolutionErrored && !UpdateSolutionErrored)
				{
					SetButtonVisibility(RefreshButton, false);
					SetButtonVisibility(OpenButton, false);
					SetButtonVisibility(RunSolutionButton, false);
					SetCheckBoxEnabled(CheckBox, false);
					SetButtonVisibility(ViewBuildLogButton, false);
					SetStatus(TaskActionStatus.Default, "installing node modules ...");
				}
			},
			Action = () =>
			{
				if (!UpgradeNodeModulesErrored && !CleanSolutionErrored && !UpdateSolutionErrored)
				{
					Logger.LogInformation("Start Install Node Modules");

					InstallNodeModulesResponse.ExitCode = NodeModulesApi.InstallNodeModules(new()
					{
						Solution = SolutionDetails.SolutionFullName,

						AddToLog = (logEntryLevel, description) =>
						{
							UpdateStatus(description);
							Logger.LogInformation(description);
							InstallNodeModulesResponse.AppendLine(description);
						},
					}).Success ? 0 : 1;

					Logger.LogInformation("Finish Install Node Modules");
				}
			},
			PostAction = () =>
			{
				if (!UpgradeNodeModulesErrored && !CleanSolutionErrored && !UpdateSolutionErrored)
				{
					SetStatus((InstallNodeModulesErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (InstallNodeModulesErrored ? "Errored Installing Node Modules" : "Completed"));
					SetCheckBoxEnabled(CheckBox, true);
					SetButtonVisibility(OpenButton, true);
					SetButtonVisibility(RunSolutionButton, ShowRunSolutionButton);
					SetButtonVisibility(RefreshButton, true);
				}
				SetButtonVisibility(ViewBuildLogButton, true);
			}
		};

		protected Process.ProcessResponse StopWatchResponse { get; private set; } = new();

		protected System.Windows.Forms.Control SolutionPanel { get; private set; }
		public System.Windows.Forms.Control Panel { get; }
		public Microsoft.Extensions.Logging.ILogger Logger { get; }
		public Action<string> UpdateStatus { get; }

		public Solution(string solutionFullName, System.Windows.Forms.Control parentControl, bool highlighted, bool selected, Action<Solution> start, Action onChangeSelected, Action<string> setStatus, Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger ?? new ISI.Extensions.ConsoleLogger();

			if (setStatus == null)
			{
				UpdateStatus = status => { };
			}
			else
			{
				UpdateStatus = status =>
				{
					setStatus((status ?? string.Empty).Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty);
				};
			}

			SolutionDetails = SolutionApi.GetSolutionDetails(new()
			{
				Solution = solutionFullName,
			}).SolutionDetails;

			ShowRunSolutionButton = SolutionApi.CanRunSolution(new()
			{
				Solution = solutionFullName,
			}).CanRun;

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
				Panel.Tag = ISI.Extensions.WinForms.ThemeHelper.IsHighlighted;
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
#if DEBUG
				//BackColor = System.Drawing.Color.Chartreuse,
#endif
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
#if DEBUG
				//BackColor = System.Drawing.Color.Chocolate,
#endif
			};
			SolutionPanel.Controls.Add(StatusLabel);

			OpenButton = new()
			{
				Visible = false,
				Text = "Open",
				Top = 4,
				Left = 430,
				Width = 80,
				Height = 20,
				BackColor = System.Drawing.SystemColors.ButtonFace,
			};
			OpenButton.Click += (_, __) =>
			{
				Task.Run(() => SolutionApi.OpenSolution(new()
				{
					Solution = SolutionDetails.SolutionDirectory,
				}));
			};
			SolutionPanel.Controls.Add(OpenButton);

			if (ShowRunSolutionButton)
			{
				RunSolutionButton = new()
				{
					Visible = false,
					Text = "Run",
					Top = 4,
					Left = 520,
					Width = 80,
					Height = 20,
					BackColor = System.Drawing.SystemColors.ButtonFace,
				};
				RunSolutionButton.Click += (_, __) =>
				{
					Task.Run(() => SolutionApi.RunSolution(new()
					{
						Solution = SolutionDetails.SolutionDirectory,
					}));
				};
				SolutionPanel.Controls.Add(RunSolutionButton);
			}

			ViewBuildLogButton = new()
			{
				Visible = false,
				Text = "View Log",
				Top = 4,
				Left = 610,
				Width = 80,
				Height = 20,
				BackColor = System.Drawing.SystemColors.ButtonFace,
			};
			ViewBuildLogButton.Click += (_, __) =>
			{
				var logs = new StringBuilder();

				if (!string.IsNullOrWhiteSpace(CleanSolutionResponse.Output))
				{
					logs.AppendLine("Clean Solution:");
					logs.AppendLine(CleanSolutionResponse.Output);
				}
				if (!string.IsNullOrWhiteSpace(UpdateSolutionResponse.Output))
				{
					logs.AppendLine("Update Solution:");
					logs.AppendLine(UpdateSolutionResponse.Output);
				}
				if (!string.IsNullOrWhiteSpace(UpgradeNodeModulesResponse.Output))
				{
					logs.AppendLine("Upgrade Node Modules:");
					logs.AppendLine(UpgradeNodeModulesResponse.Output);
				}
				if (!string.IsNullOrWhiteSpace(InstallNodeModulesResponse.Output))
				{
					logs.AppendLine("Install Node Modules:");
					logs.AppendLine(InstallNodeModulesResponse.Output);
				}
				if (!string.IsNullOrWhiteSpace(StopWatchResponse.Output))
				{
					logs.AppendLine(StopWatchResponse.Output);
				}

				(new ViewLogForm(logs.ToString())).ShowDialog();
			};
			SolutionPanel.Controls.Add(ViewBuildLogButton);

			void Resize()
			{
				const int buttonWidth = 90;

				var left = SolutionPanel.Size.Width;

				foreach (var button in new[]
				{
					ViewBuildLogButton,
					OpenButton,
					RunSolutionButton,
					RefreshButton,
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

		public void ResetResponses()
		{
			UpdateSolutionResponse = new();
			StopWatchResponse = new();
		}

		public virtual TaskActions[] GetTasks(bool resetResponses, bool cleanSolution, bool updateSolution, bool installNodeModules)
		{
			var tasks = new List<TaskActions>();

			if (resetResponses)
			{
				ResetResponses();
			}

			tasks.Add(new TaskActions()
			{
				PreAction = () => Stopwatch.Start(),
				Action = () => { },
				PostAction = () => { },
			});

			if (cleanSolution)
			{
				tasks.Add(CleanSolution);
			}

			if (updateSolution)
			{
				tasks.Add(UpdateSolution);
			}

			if (installNodeModules)
			{
				tasks.Add(InstallNodeModules);
			}

			tasks.Add(new TaskActions()
			{
				PreAction = () => { },
				Action = () => { },
				PostAction = () =>
				{
					Stopwatch.Stop();
					StopWatchResponse.Output = $"\nTook {Stopwatch.Elapsed.Formatted(TimeSpanExtensions.TimeSpanFormat.Short)}";
				},
			});

			return tasks.ToArray();
		}
	}
}
