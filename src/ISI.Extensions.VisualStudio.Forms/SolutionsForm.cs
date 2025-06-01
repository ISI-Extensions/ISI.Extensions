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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISI.Extensions.VisualStudio.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio.Forms
{
	public partial class SolutionsForm : Form
	{
		public interface ISolutionsContext
		{
			IList<Solution> Solutions { get; }
		}

		public bool ShowUpgradeNugetPackagesCheckBox { get; set; } = false;
		public bool ShowCommitSolutionCheckBox { get; set; } = false;
		public bool ShowExecuteProjectsCheckBox { get; set; } = false;
		public bool ShowShowProjectExecutionInTaskbarCheckBox { get; set; } = false;

		public delegate ISolutionsContext ExecuteActionsInSolutionsDelegate(SolutionsForm form, Action<Solution> start);
		public delegate void UpdatePreviouslySelectedSolutionsDelegate(SolutionsForm form);
		public delegate void OnCloseFormDelegate(SolutionsForm form);

		private static ISI.Extensions.VisualStudio.SolutionApi _solutionApi = null;
		protected ISI.Extensions.VisualStudio.SolutionApi SolutionApi => _solutionApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

		private static ISI.Extensions.Nuget.NugetApi _nugetApi = null;
		protected ISI.Extensions.Nuget.NugetApi NugetApi => _nugetApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.NugetApi>();

		protected internal ISolutionsContext SolutionsContext { get; set; }

		protected internal bool Cancelled { get; set; }
		protected internal System.Threading.CancellationTokenSource CancellationTokenSource { get; set; }

		protected TaskActions BackgroundTask { get; private set; } = null;
		protected Queue<TaskActions> BackgroundTasks { get; } = new();
		protected BackgroundWorker BackgroundTasksWorker { get; } = new();
		protected RunWorkerCompletedEventHandler TaskCompleted { get; } = null;

		protected internal bool IsFirstRefresh { get; private set; } = true;

		protected internal bool CleanSolution { get; set; } = true;
		protected internal bool UpdateSolution { get; set; } = true;
		protected internal bool UpgradeNugetPackages { get; set; } = false;
		protected internal bool CommitSolution { get; set; } = false;
		protected internal bool RestoreNugetPackages { get; set; } = true;
		protected internal bool BuildSolution { get; set; } = true;
		protected internal bool ExecuteProjects { get; set; } = false;
		protected internal bool ShowProjectExecutionInTaskbar { get; set; } = true;
		protected internal bool ExitOnClose { get; set; }

		public SolutionsForm(ExecuteActionsInSolutionsDelegate executeActionsInSolutions, UpdatePreviouslySelectedSolutionsDelegate updatePreviouslySelectedSolutions, OnCloseFormDelegate onCloseForm)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			SolutionsPanel.ControlAdded += (sender, args) => ISI.Extensions.WinForms.ThemeHelper.SyncTheme(args.Control);

			SolutionApi.ApplyFormSize(this);

			CloseButton.Visible = false;
			StartButton.Visible = false;
			StopButton.Visible = false;

			ForeColor = DefaultForeColor;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			TaskCompleted = (runWorkerCompletedSender, runWorkerCompletedArgs) =>
			{
				if (!BackgroundTasksWorker.IsBusy)
				{
					BackgroundTask?.PostAction?.Invoke();
				}

				if (!Cancelled && BackgroundTasks.Any())
				{
					if (!BackgroundTasksWorker.IsBusy)
					{
						BackgroundTask = BackgroundTasks.Dequeue();

						if (!Cancelled)
						{
							BackgroundTask?.PreAction?.Invoke();

							BackgroundTasksWorker.RunWorkerAsync();
						}
					}
				}
				else
				{
					StopButton.Visible = false;
					StartButton.Visible = true;
					CloseButton.Enabled = true;
					CloseButton.Visible = true;
				}
			};

			BackgroundTasksWorker.DoWork += (_, __) =>
			{
				if (!Cancelled)
				{
					BackgroundTask.Action?.Invoke();
				}
			};
			BackgroundTasksWorker.RunWorkerCompleted += TaskCompleted;

			void Resize()
			{
				var width = SolutionsPanel.Size.Width - 19;

				foreach (var solution in SolutionsContext.Solutions)
				{
					if (solution.Panel.Size.Width != width)
					{
						solution.Panel.Size = new(width, solution.Panel.Size.Height);
					}
				}
			}

			Shown += (shownSender, shownArgs) =>
			{
				SolutionsContext = executeActionsInSolutions(this, solution =>
				{
					using (var form = new SolutionOptionsForm(CleanSolution, UpdateSolution, UpgradeNugetPackages, CommitSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar)
					{
						ShowUpgradeNugetPackagesCheckBox = ShowUpgradeNugetPackagesCheckBox,
						ShowCommitSolutionCheckBox = ShowUpgradeNugetPackagesCheckBox && ShowCommitSolutionCheckBox,
						ShowExecuteProjectsCheckBox = ShowExecuteProjectsCheckBox,
						ShowShowProjectExecutionInTaskbarCheckBox = ShowShowProjectExecutionInTaskbarCheckBox,
					})
					{
						if (form.ShowDialog() == DialogResult.OK)
						{
							solution.SetButtonVisibility(solution.RefreshButton, false);
							solution.SetButtonVisibility(solution.OpenButton, false);
							solution.SetButtonVisibility(solution.ViewBuildLogButton, false);

							CleanSolution = form.CleanSolution;
							UpdateSolution = form.UpdateSolution;
							CommitSolution = form.CommitSolution;
							UpgradeNugetPackages = form.UpgradeNugetPackages;
							RestoreNugetPackages = form.RestoreNugetPackages;
							BuildSolution = form.BuildSolution;
							ExecuteProjects = form.ExecuteProjects;
							ShowProjectExecutionInTaskbar = form.ShowProjectExecutionInTaskbar;

							foreach (var solutionTask in solution.GetTasks(true, CleanSolution, UpdateSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar))
							{
								BackgroundTasks.Enqueue(solutionTask);
							}

							TaskCompleted.Invoke(null, null);
						}
					}
				});

				SolutionsPanel.Resize += (_, __) => { Resize(); };

				Resize();

				StartButton.Visible = true;
				CloseButton.Visible = true;
			};

			StopButton.Click += (_, __) =>
			{
				Cancelled = true;
				CancellationTokenSource?.Cancel();
				StopButton.Visible = false;
			};

			StartButton.Click += (_, __) =>
			{
				using (var form = new SolutionOptionsForm(CleanSolution, UpdateSolution, UpgradeNugetPackages, CommitSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar)
				{
					ShowUpgradeNugetPackagesCheckBox = ShowUpgradeNugetPackagesCheckBox,
					ShowCommitSolutionCheckBox = ShowUpgradeNugetPackagesCheckBox && ShowCommitSolutionCheckBox,
					ShowExecuteProjectsCheckBox = ShowExecuteProjectsCheckBox,
					ShowShowProjectExecutionInTaskbarCheckBox = ShowShowProjectExecutionInTaskbarCheckBox,
				})
				{
					if (form.ShowDialog() == DialogResult.OK)
					{
						CancellationTokenSource = new();

						StartButton.Visible = false;

						CleanSolution = form.CleanSolution;
						UpdateSolution = form.UpdateSolution;
						CommitSolution = form.CommitSolution;
						UpgradeNugetPackages = form.UpgradeNugetPackages;
						RestoreNugetPackages = form.RestoreNugetPackages;
						BuildSolution = form.BuildSolution;
						ExecuteProjects = form.ExecuteProjects;
						ShowProjectExecutionInTaskbar = form.ShowProjectExecutionInTaskbar;

						if (IsFirstRefresh)
						{
							updatePreviouslySelectedSolutions(this);

							IsFirstRefresh = false;
						}

						SetStatus(string.Empty);

						var resetResponses = true;

						if (UpgradeNugetPackages)
						{
							resetResponses = false;

							BackgroundTasks.Enqueue(new TaskActions()
							{
								PreAction = () => { },
								Action = () =>
								{
									var solutionsBySolutionFullName = SolutionsContext.Solutions.Where(solution => solution.Selected).ToDictionary(solution => solution.SolutionDetails.SolutionFullName, _ => _, StringComparer.InvariantCultureIgnoreCase);

									foreach (var solution in solutionsBySolutionFullName.Values)
									{
										solution.ResetResponses();
									}

									SolutionApi.UpgradeNugetPackages(new()
									{
										SolutionFullNames = solutionsBySolutionFullName.Values.Select(solution => solution.SolutionDetails.SolutionFullName),
										UpdateWorkingCopyFromSourceControl = UpdateSolution,
										CommitWorkingCopyToSourceControl = CommitSolution,
										IgnorePackageIds = NugetApi.GetNugetSettings(new())?.NugetSettings?.UpdateNugetPackages?.IgnorePackageIds,
										NugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary(),
										UpsertAssemblyRedirectsNugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary(),
										AddToLog = (logEntryLevel, description) =>
										{
										},
										PreAction = (solutionFullName) =>
										{
											if (solutionsBySolutionFullName.TryGetValue(solutionFullName, out var solution))
											{
												solution.UpgradeNugetPackagesPreAction();
											}
										},
										SetStatus = (solutionFullName, description) =>
										{
											if (solutionsBySolutionFullName.TryGetValue(solutionFullName, out var solution))
											{
												solution.UpdateStatus?.Invoke(description);
												solution.Logger.LogInformation(description);
												solution.UpgradeNugetPackagesResponse.AppendLine(description);
											}
										},
										PostAction = (solutionFullName) =>
										{
											if (solutionsBySolutionFullName.TryGetValue(solutionFullName, out var solution))
											{
												solution.UpgradeNugetPackagesPostAction(!RestoreNugetPackages && !BuildSolution, RestoreNugetPackages && !BuildSolution);
											}
										},
										BuildScriptError = (solutionFullName) =>
										{
											if (solutionsBySolutionFullName.TryGetValue(solutionFullName, out var solution))
											{
												solution.UpgradeNugetPackagesResponse.ExitCode = -1;
											}
										},
										ContinueOnBuildScriptError = true,
										CancellationToken = CancellationTokenSource.Token,
									});
								},
								PostAction = () => { },
							});
						}

						if (!UpgradeNugetPackages || RestoreNugetPackages || BuildSolution)
						{
							foreach (var solution in SolutionsContext.Solutions.Where(solution => solution.Selected))
							{
								foreach (var solutionTask in solution.GetTasks(resetResponses, CleanSolution, UpdateSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar))
								{
									BackgroundTasks.Enqueue(solutionTask);
								}
							}
						}

						StopButton.Visible = true;
						CloseButton.Enabled = false;

						TaskCompleted.Invoke(null, null);
					}
				}
			};

			CloseButton.Click += (_, __) =>
			{
				onCloseForm(this);

				if (this.Modal)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				else
				{
					this.Close();

					if (ExitOnClose)
					{
						System.Windows.Forms.Application.Exit();
					}
				}
			};

			Closing += (_, __) =>
			{
				SolutionApi.RecordFormSize(this);
			};
		}

		public void SetStatus(string status)
		{
			lblStatus.Invoke((System.Windows.Forms.MethodInvoker)delegate
			{
				lblStatus.Text = status;
			});
		}
	}
}
