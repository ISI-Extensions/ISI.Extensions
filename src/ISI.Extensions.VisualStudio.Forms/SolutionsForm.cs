﻿#region Copyright & License
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISI.Extensions.VisualStudio.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.VisualStudio.Forms
{
	public partial class SolutionsForm : Form
	{
		public interface ISolutionsContext
		{
			IList<Solution> Solutions { get; }
		}

		public bool ShowExecuteProjectsCheckBox { get; set; } = false;
		public bool ShowShowProjectExecutionInTaskbarCheckBox { get; set; } = false;

		public delegate ISolutionsContext BuildSolutionsDelegate(SolutionsForm form, Action<Solution> start);
		public delegate void UpdatePreviouslySelectedSolutionsDelegate(SolutionsForm form);
		public delegate void OnCloseFormDelegate(SolutionsForm form);

		private static ISI.Extensions.VisualStudio.VisualStudioSettings _visualStudioSettings = null;
		protected ISI.Extensions.VisualStudio.VisualStudioSettings VisualStudioSettings => _visualStudioSettings ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.VisualStudioSettings>();

		protected internal ISolutionsContext SolutionsContext { get; set; }

		protected internal bool Cancelled { get; set; }

		protected TaskActions BackgroundTask { get; private set; } = null;
		protected Queue<TaskActions> BackgroundTasks { get; } = new();
		protected BackgroundWorker BackgroundTasksWorker { get; } = new();
		protected RunWorkerCompletedEventHandler TaskCompleted { get; } = null;

		protected internal bool IsFirstRefresh { get; private set; } = true;

		protected internal bool CleanSolution { get; set; } = true;
		protected internal bool UpdateSolution { get; set; } = true;
		protected internal bool RestoreNugetPackages { get; set; } = true;
		protected internal bool BuildSolution { get; set; } = true;
		protected internal bool ExecuteProjects { get; set; } = true;
		protected internal bool ShowProjectExecutionInTaskbar { get; set; } = true;
		protected internal bool ExitOnClose { get; set; }

		public SolutionsForm(BuildSolutionsDelegate buildSolutions, UpdatePreviouslySelectedSolutionsDelegate updatePreviouslySelectedSolutions, OnCloseFormDelegate onCloseForm)
		{
			InitializeComponent();

			VisualStudioSettings.ApplyFormSize(this);

			CloseButton.Visible = false;
			StartButton.Visible = false;
			StopButton.Visible = false;

			ForeColor = DefaultForeColor;

			Icon = new Icon(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
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
						solution.Panel.Size = new Size(width, solution.Panel.Size.Height);
					}
				}
			}

			Shown += (shownSender, shownArgs) =>
			{
				SolutionsContext = buildSolutions(this, solution =>
				{
					using (var form = new BuildOptionsForm(CleanSolution, UpdateSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar)
					{
						ShowExecuteProjectsCheckBox = ShowExecuteProjectsCheckBox,
						ShowShowProjectExecutionInTaskbarCheckBox = ShowShowProjectExecutionInTaskbarCheckBox,
					})
					{
						if (form.ShowDialog() == DialogResult.OK)
						{
							solution.RefreshButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { solution.RefreshButton.Visible = false; });
							solution.OpenButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { solution.OpenButton.Visible = false; });
							solution.ViewBuildLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { solution.ViewBuildLogButton.Visible = false; });

							CleanSolution = form.CleanSolution;
							UpdateSolution = form.UpdateSolution;
							RestoreNugetPackages = form.RestoreNugetPackages;
							BuildSolution = form.BuildSolution;
							ExecuteProjects = form.ExecuteProjects;
							ShowProjectExecutionInTaskbar = form.ShowProjectExecutionInTaskbar;

							foreach (var solutionTask in solution.GetTasks(CleanSolution, UpdateSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar))
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
				StopButton.Visible = false;
			};

			StartButton.Click += (_, __) =>
			{
				using (var form = new BuildOptionsForm(CleanSolution, UpdateSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar)
				{
					ShowExecuteProjectsCheckBox = ShowExecuteProjectsCheckBox,
					ShowShowProjectExecutionInTaskbarCheckBox = ShowShowProjectExecutionInTaskbarCheckBox,
				})
				{
					if (form.ShowDialog() == DialogResult.OK)
					{
						StartButton.Visible = false;

						CleanSolution = form.CleanSolution;
						UpdateSolution = form.UpdateSolution;
						RestoreNugetPackages = form.RestoreNugetPackages;
						BuildSolution = form.BuildSolution;
						ExecuteProjects = form.ExecuteProjects;
						ShowProjectExecutionInTaskbar = form.ShowProjectExecutionInTaskbar;

						if (IsFirstRefresh)
						{
							updatePreviouslySelectedSolutions(this);

							IsFirstRefresh = false;
						}

						foreach (var solution in SolutionsContext.Solutions.Where(solution => solution.Selected))
						{
							foreach (var solutionTask in solution.GetTasks(CleanSolution, UpdateSolution, RestoreNugetPackages, BuildSolution, ExecuteProjects, ShowProjectExecutionInTaskbar))
							{
								BackgroundTasks.Enqueue(solutionTask);
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
				VisualStudioSettings.RecordFormSize(this);
			};
		}
	}
}