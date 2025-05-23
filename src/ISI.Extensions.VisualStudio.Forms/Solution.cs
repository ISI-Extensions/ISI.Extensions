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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.VisualStudio.Extensions;
using ISI.Extensions.VisualStudio.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio.Forms
{
	public class Solution
	{
		private static ISI.Extensions.Scm.ISourceControlClientApi _sourceControlClientApi = null;
		protected ISI.Extensions.Scm.ISourceControlClientApi SourceControlClientApi => _sourceControlClientApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.ISourceControlClientApi>();

		private static ISI.Extensions.VisualStudio.SolutionApi _solutionApi = null;
		protected ISI.Extensions.VisualStudio.SolutionApi SolutionApi => _solutionApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

		private static ISI.Extensions.VisualStudio.ProjectApi _projectApi = null;
		protected ISI.Extensions.VisualStudio.ProjectApi ProjectApi => _projectApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.ProjectApi>();

		private static ISI.Extensions.Nuget.NugetApi _nugetApi = null;
		protected ISI.Extensions.Nuget.NugetApi NugetApi => _nugetApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.NugetApi>();

		private static ISI.Extensions.VisualStudio.MSBuildApi _msBuildApi = null;
		protected ISI.Extensions.VisualStudio.MSBuildApi MSBuildApi => _msBuildApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.MSBuildApi>();

		private string _caption = null;
		public virtual string Caption => _caption ??= SolutionDetails.SolutionName;

		public virtual SolutionDetails SolutionDetails { get; }
		public virtual BuildConfiguration ActiveBuildConfiguration { get; }

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

		protected internal SolutionProject[] SolutionProjects { get; }
		protected internal SolutionFilter[] SolutionFilters { get; }

		protected Action OnChangeSelected { get; set; }

		protected internal System.Windows.Forms.CheckBox CheckBox { get; set; }
		protected internal System.Windows.Forms.Label SolutionLabel { get; set; }
		protected internal System.Windows.Forms.Label StatusLabel { get; set; }
		protected internal System.Windows.Forms.Button RefreshButton { get; set; }
		protected internal System.Windows.Forms.Button OpenButton { get; set; }
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

		public Process.ProcessResponse UpgradeNugetPackagesResponse { get; private set; } = new();
		protected bool UpgradeNugetPackagesErrored => UpgradeNugetPackagesResponse.Errored;

		public void UpgradeNugetPackagesPreAction()
		{
			if (!CleanSolutionErrored && !UpdateSolutionErrored)
			{
				SetButtonVisibility(RefreshButton, false);
				SetButtonVisibility(OpenButton, false);
				SetCheckBoxEnabled(CheckBox, false);
				SetButtonVisibility(ViewBuildLogButton, false);
				SetStatus(TaskActionStatus.Default, "upgrading nuget packages ...");

				Logger.LogInformation("Start Upgrade Nuget Packages");
			}
		}

		public void UpgradeNugetPackagesPostAction(bool showOpenButton, bool showRefreshButton)
		{
			Logger.LogInformation("Finish Upgrade Nuget Packages");

			if (!CleanSolutionErrored && !UpdateSolutionErrored)
			{
				SetStatus((UpgradeNugetPackagesErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (UpgradeNugetPackagesErrored ? "Errored Upgrading Nuget Packages" : "Completed"));
				SetCheckBoxEnabled(CheckBox, true);
				SetButtonVisibility(OpenButton, showOpenButton);
				SetButtonVisibility(RefreshButton, showRefreshButton);
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
				if (!UpgradeNugetPackagesErrored)
				{
					SetButtonVisibility(RefreshButton, false);
					SetButtonVisibility(OpenButton, false);
					SetCheckBoxEnabled(CheckBox, false);
					SetButtonVisibility(ViewBuildLogButton, false);
					SetStatus(TaskActionStatus.Default, "cleaning ...");
				}
			},
			Action = () =>
			{
				if (!UpgradeNugetPackagesErrored)
				{
					Logger.LogInformation("Start Clean Solution");

					CleanSolutionErrored = !(SolutionApi.CleanSolution(new()
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
				if (!UpgradeNugetPackagesErrored)
				{
					SetStatus((CleanSolutionErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (CleanSolutionErrored ? "Errored Cleaning" : "Completed"));
					SetCheckBoxEnabled(CheckBox, true);
					SetButtonVisibility(OpenButton, true);
					SetButtonVisibility(RefreshButton, true);
				}
			}
		};

		protected Process.ProcessResponse UpdateSolutionResponse { get; private set; } = new();
		protected bool UpdateSolutionErrored => UpdateSolutionResponse.Errored;
		private TaskActions _updateSolution = null;
		protected TaskActions UpdateSolution => _updateSolution ??= new()
		{
			PreAction = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored)
				{
					SetButtonVisibility(RefreshButton, false);
					SetButtonVisibility(OpenButton, false);
					SetCheckBoxEnabled(CheckBox, false);
					SetButtonVisibility(ViewBuildLogButton, false);
					SetStatus(TaskActionStatus.Default, "updating from source control ...");
				}
			},
			Action = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored)
				{
					Logger.LogInformation("Start Update Solution");

					if (string.IsNullOrWhiteSpace(SolutionDetails.RootSourceDirectory))
					{
						UpdateSolutionResponse.ExitCode = 0;
					}
					else
					{
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
					}

					Logger.LogInformation("Finish Update Solution");
				}
			},
			PostAction = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored)
				{
					SetStatus((UpdateSolutionErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (UpdateSolutionErrored ? "Errored Updating" : "Completed"));
					SetCheckBoxEnabled(CheckBox, true);
					SetButtonVisibility(OpenButton, true);
					SetButtonVisibility(RefreshButton, true);
				}
				SetButtonVisibility(ViewBuildLogButton, true);
			}
		};

		protected Process.ProcessResponse RestoreNugetPackagesResponse { get; private set; } = new();
		protected bool RestoreNugetPackagesErrored => RestoreNugetPackagesResponse.Errored;
		private TaskActions _restoreNugetPackages = null;
		protected TaskActions RestoreNugetPackages => _restoreNugetPackages ??= new()
		{
			PreAction = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored && !UpdateSolutionErrored)
				{
					SetButtonVisibility(RefreshButton, false);
					SetButtonVisibility(OpenButton, false);
					SetCheckBoxEnabled(CheckBox, false);
					SetButtonVisibility(ViewBuildLogButton, false);
					SetStatus(TaskActionStatus.Default, "restoring nuget packages ...");
				}
			},
			Action = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored && !UpdateSolutionErrored)
				{
					Logger.LogInformation("Start Restore Nuget Packages");

					RestoreNugetPackagesResponse.ExitCode = NugetApi.RestoreNugetPackages(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.RestoreNugetPackagesUsingSolutionDetailsRequest()
					{
						SolutionDetails = SolutionDetails,
						MSBuildExe = MSBuildApi.GetMSBuildExeFullName(new()).MSBuildExeFullName,

						AddToLog = (logEntryLevel, description) =>
						{
							UpdateStatus(description);
							Logger.LogInformation(description);
							RestoreNugetPackagesResponse.AppendLine(description);
						},
					}).Success ? 0 : 1;

					Logger.LogInformation("Finish Restore Nuget Packages");
				}
			},
			PostAction = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored && !UpdateSolutionErrored)
				{
					SetStatus((RestoreNugetPackagesErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (RestoreNugetPackagesErrored ? "Errored Restoring Nuget Packages" : "Completed"));
					SetCheckBoxEnabled(CheckBox, true);
					SetButtonVisibility(OpenButton, true);
					SetButtonVisibility(RefreshButton, true);
				}
				SetButtonVisibility(ViewBuildLogButton, true);
			}
		};

		protected Process.ProcessResponse BuildSolutionResponse { get; private set; } = new();
		public bool BuildSolutionErrored => BuildSolutionResponse.Errored;
		private TaskActions _buildSolution = null;
		protected TaskActions BuildSolution => _buildSolution ??= new()
		{
			PreAction = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored && !UpdateSolutionErrored && !RestoreNugetPackagesErrored)
				{
					SetButtonVisibility(RefreshButton, false);
					SetButtonVisibility(OpenButton, false);
					SetCheckBoxEnabled(CheckBox, false);
					SetButtonVisibility(ViewBuildLogButton, false);
					SetStatus(TaskActionStatus.Default, "building ...");
				}
			},
			Action = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored && !UpdateSolutionErrored && !RestoreNugetPackagesErrored)
				{
					Logger.LogInformation("Start Build Solution");

					var msBuildRequest = new ISI.Extensions.VisualStudio.DataTransferObjects.MSBuildApi.MSBuildRequest()
					{
						FullName = SolutionDetails.SolutionFullName,
						MsBuildPlatform = ActiveBuildConfiguration.MSBuildPlatform,

						AddToLog = (logEntryLevel, description) =>
						{
							UpdateStatus(description);
							Logger.LogInformation(description);
							BuildSolutionResponse.AppendLine(description);
						},
					};

					msBuildRequest.Options.Configuration = ActiveBuildConfiguration.Configuration;

					try
					{
						MSBuildApi.MSBuild(msBuildRequest);

						BuildSolutionResponse.ExitCode = 0;
					}
					catch (Exception exception)
					{
						BuildSolutionResponse.ExitCode = 1;
					}

					Logger.LogInformation("Finish Build Solution");
				}
			},
			PostAction = () =>
			{
				if (!UpgradeNugetPackagesErrored && !CleanSolutionErrored && !UpdateSolutionErrored && !RestoreNugetPackagesErrored)
				{
					SetStatus((BuildSolutionErrored ? TaskActionStatus.Errored : TaskActionStatus.Default), (BuildSolutionErrored ? "Errored Building" : "Completed"));
					SetCheckBoxEnabled(CheckBox, true);
					SetButtonVisibility(OpenButton, true);
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

		public Solution(string solutionFullName, System.Windows.Forms.Control parentControl, bool highlighted, bool selected, Action<Solution> start, IEnumerable<ProjectKey> projectKeys, bool showSolutionFilterKeys, bool waitForExecuteProjectResponse, Action onChangeSelected, Action<string> setStatus, Microsoft.Extensions.Logging.ILogger logger)
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

			var getActiveBuildConfigurationResponse = SolutionApi.GetActiveBuildConfiguration(new()
			{
				Solution = solutionFullName,
			});

			SolutionDetails = getActiveBuildConfigurationResponse.SolutionDetails;
			ActiveBuildConfiguration = getActiveBuildConfigurationResponse.ActiveBuildConfiguration;

			Panel = new System.Windows.Forms.Panel()
			{
				Width = parentControl.Width - 19,
				Height = 26,
				Margin = new(0),
			};
			parentControl.Resize += (resizeSender, resizeArgs) => { Panel.Width = parentControl.Width - 19; };

			projectKeys = (projectKeys ?? []).ToArray();
			if (projectKeys.Any() && !projectKeys.Any(projectKey => projectKey.Selected))
			{
				(projectKeys.FirstOrDefault(projectKey => string.Equals(ProjectApi.GetProjectName(new()
				{
					Project = projectKey.ProjectFullName,
				}).ProjectName, Caption, StringComparison.InvariantCultureIgnoreCase)) ?? projectKeys.First()).Selected = true;
			}

			SolutionProjects = projectKeys.OrderBy(projectKey => ProjectApi.GetProjectName(new()
			{
				Project = projectKey.ProjectFullName,
			}).ProjectName, StringComparer.InvariantCultureIgnoreCase).Select(projectKey => new SolutionProject(this, projectKey, start, waitForExecuteProjectResponse, projectKey.Selected, () => OnChangeSelected?.Invoke())).ToArray();

			if (showSolutionFilterKeys)
			{
				var solutionFilters = SolutionDetails.SolutionFilterDetailsSet
					.NullCheckedOrderBy(solutionFilterDetails => System.IO.Path.GetFileNameWithoutExtension(solutionFilterDetails.SolutionFilterFullName))
					.ToNullCheckedArray(solutionFilterDetails => new SolutionFilter(this, new(SolutionDetails.SolutionFullName, solutionFilterDetails.SolutionFilterFullName), null, true, () => OnChangeSelected?.Invoke()))
					.ToList();

				if (solutionFilters.Any())
				{
					solutionFilters.Insert(0, new(this, new(SolutionDetails.SolutionFullName, SolutionDetails.SolutionFullName), null, true, () => OnChangeSelected?.Invoke()));
				}

				SolutionFilters = solutionFilters.ToArray();
			}

			SolutionFilters ??= [];

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

			if (SolutionProjects.Any())
			{
				var top = SolutionPanel.Height;

				Panel.Height += SolutionProjects.Sum(solutionProject => solutionProject.Panel.Height);

				foreach (var solutionProject in SolutionProjects.OrderBy(solutionProject => string.Format("{0} => {1}", (string.Equals(Caption, solutionProject.Caption, StringComparison.InvariantCultureIgnoreCase) ? "A" : "B"), solutionProject.Caption), StringComparer.InvariantCultureIgnoreCase))
				{
					solutionProject.Panel.Top = top;
					solutionProject.Panel.Left = 0;
					top += solutionProject.Panel.Height;
					Panel.Controls.Add(solutionProject.Panel);
				}
			}

			if (SolutionFilters.Any())
			{
				var top = SolutionPanel.Height;

				Panel.Height += SolutionFilters.Sum(solutionFilter => solutionFilter.Panel.Height);

				foreach (var solutionFilter in SolutionFilters.OrderBy(solutionFilter => string.Format("{0} => {1}", (string.Equals(Caption, solutionFilter.Caption, StringComparison.InvariantCultureIgnoreCase) ? "A" : "B"), solutionFilter.Caption), StringComparer.InvariantCultureIgnoreCase))
				{
					solutionFilter.Panel.Top = top;
					solutionFilter.Panel.Left = 0;
					top += solutionFilter.Panel.Height;
					Panel.Controls.Add(solutionFilter.Panel);
				}
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

			if ((SolutionProjects == null) || !SolutionProjects.Any())
			{
				RefreshButton = new()
				{
					Visible = false,
					Text = "Refresh",
					Top = 4,
					Left = 430,
					Width = 80,
					Height = 20,
					BackColor = System.Drawing.SystemColors.ButtonFace,
				};
				RefreshButton.Click += (_, __) =>
				{
					start(this);
				};
				SolutionPanel.Controls.Add(RefreshButton);
			}

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
				var solutionFilterFullName = SolutionFilters.NullCheckedFirstOrDefault(solutionFilter => solutionFilter.Selected)?.SolutionFilterFullName;

				Task.Run(() => SolutionApi.OpenSolution(new()
				{
					Solution = SolutionDetails.SolutionDirectory,
					SolutionFilter = solutionFilterFullName,
				}));
			};
			SolutionPanel.Controls.Add(OpenButton);

			ViewBuildLogButton = new()
			{
				Visible = false,
				Text = "View Log",
				Top = 4,
				Left = 520,
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
				if (!string.IsNullOrWhiteSpace(UpgradeNugetPackagesResponse.Output))
				{
					logs.AppendLine("Upgrade Nuget Packages:");
					logs.AppendLine(UpgradeNugetPackagesResponse.Output);
				}
				if (!string.IsNullOrWhiteSpace(RestoreNugetPackagesResponse.Output))
				{
					logs.AppendLine("Restore Nuget Packages:");
					logs.AppendLine(RestoreNugetPackagesResponse.Output);
				}
				if (!string.IsNullOrWhiteSpace(BuildSolutionResponse.Output))
				{
					logs.AppendLine("Build Solution:");
					logs.AppendLine(BuildSolutionResponse.Output);
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
			CleanSolutionResponse = new();
			UpdateSolutionResponse = new();
			UpgradeNugetPackagesResponse = new();
			RestoreNugetPackagesResponse = new();
			BuildSolutionResponse = new();
			StopWatchResponse = new();
		}

		public virtual TaskActions[] GetTasks(bool resetResponses, bool cleanSolution, bool updateSolution, bool restoreNugetPackages, bool buildSolution, bool executeProjects, bool showProjectExecutionInTaskbar)
		{
			var tasks = new List<TaskActions>();

			if (resetResponses)
			{
				ResetResponses();
			}

			if (!SolutionProjects.Any() || SolutionProjects.All(project => project.ExecuteProjectInstance == null))
			{
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

				if (restoreNugetPackages)
				{
					tasks.Add(RestoreNugetPackages);
				}

				if (buildSolution)
				{
					tasks.Add(BuildSolution);
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

				if (executeProjects)
				{
					foreach (var solutionProject in SolutionProjects)
					{
						solutionProject.StartButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { solutionProject.StartButton.Visible = false; });

						solutionProject.ViewRunLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { solutionProject.ViewRunLogButton.Visible = false; });

						tasks.AddRange(solutionProject.GetExecuteProjectTaskActions(showProjectExecutionInTaskbar));
					}
				}
			}

			return tasks.ToArray();
		}
	}
}
