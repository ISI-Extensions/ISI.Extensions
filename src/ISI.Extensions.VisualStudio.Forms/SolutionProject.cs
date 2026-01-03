#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.VisualStudio.Forms
{
	public class SolutionProject
	{
		[System.Runtime.InteropServices.DllImport("User32.dll")]
		private static extern bool SetForegroundWindow(IntPtr handle);

		private static ISI.Extensions.VisualStudio.ProjectApi _projectApi = null;
		protected ISI.Extensions.VisualStudio.ProjectApi ProjectApi => _projectApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.ProjectApi>();

		public Solution Solution { get; }
		public ProjectKey ProjectKey { get; }

		public string ProjectFullName => ProjectKey.ProjectFullName;

		private string _projectName = null;
		public string ProjectName => _projectName ??= ProjectApi.GetProjectName(new()
		{
			Project = ProjectFullName,
		}).ProjectName;

		private string _caption = null;
		public virtual string Caption => _caption ??= ProjectName;

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
		protected internal System.Windows.Forms.Label ProjectLabel { get; set; }
		protected internal System.Windows.Forms.Label StatusLabel { get; set; }
		protected internal System.Windows.Forms.Button StartButton { get; set; } = null;
		protected internal System.Windows.Forms.Button StopButton { get; set; } = null;
		protected internal System.Windows.Forms.Button ViewRunLogButton { get; set; } = null;

		protected bool WaitForExecuteProjectResponse { get; }
		protected internal Process.ProcessResponse ExecuteProjectResponse { get; private set; } = new();
		protected internal bool ExecuteProjectErrored => ExecuteProjectResponse.Errored;
		protected internal System.Diagnostics.Process ExecuteProjectInstance { get; set; } = null;

		public System.Windows.Forms.Control Panel { get; }

		public SolutionProject(Solution solution, ProjectKey projectKey, Action<Solution> start, bool waitForExecuteProjectResponse, bool selected, Action onChangeSelected)
		{
			Solution = solution;
			ProjectKey = projectKey;
			WaitForExecuteProjectResponse = waitForExecuteProjectResponse;

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
				Checked = ProjectKey.Selected,
			};
			RadioButton.CheckedChanged += (_, __) =>
			{
				if (RadioButton.Checked)
				{
					foreach (var solutionProject in Solution.SolutionProjects)
					{
						if (!string.Equals(ProjectKey.Value, solutionProject.ProjectKey.Value, StringComparison.InvariantCultureIgnoreCase))
						{
							solutionProject.Selected = false;
						}
					}
				}

				OnChangeSelected?.Invoke();
			};
			Panel.Controls.Add(RadioButton);

			ProjectLabel = new()
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
			ProjectLabel.Click += (clickSender, clickEventArgs) => Selected = !Selected;
			Panel.Controls.Add(ProjectLabel);

			StatusLabel = new()
			{
				Text = string.Empty,
				Top = 7,
				Left = 270,
				Width = 150,
				Height = 17,
#if DEBUG
				//BackColor = System.Drawing.Color.GreenYellow,
#endif
			};
			Panel.Controls.Add(StatusLabel);

			if (!WaitForExecuteProjectResponse)
			{
				StopButton = new()
				{
					Visible = false,
					Text = "Stop",
					Top = 4,
					Left = 430,
					Width = 80,
					Height = 20,
					BackColor = System.Drawing.SystemColors.ButtonFace,
				};
				StopButton.Click += (_, __) => { StopService(); };
				Panel.Controls.Add(StopButton);

				StartButton = new()
				{
					Visible = false,
					Text = "Start",
					Top = 4,
					Left = 430,
					Width = 80,
					Height = 20,
					BackColor = System.Drawing.SystemColors.ButtonFace,
				};
				StartButton.Click += (_, __) => { start(Solution); };
				Panel.Controls.Add(StartButton);
			}

			ViewRunLogButton = new()
			{
				Visible = false,
				Text = "View Log",
				Top = 4,
				Left = 520,
				Width = 80,
				Height = 20,
				BackColor = System.Drawing.SystemColors.ButtonFace,
			};
			ViewRunLogButton.Click += (_, __) =>
			{
				var viewLogForm = new ViewLogForm(ExecuteProjectResponse.Output);
				ExecuteProjectResponse.OnChange += (isAppend, output) => viewLogForm.OnChange(isAppend, output);
				viewLogForm.ShowDialog();
			};
			Panel.Controls.Add(ViewRunLogButton);

			void Resize()
			{
				const int buttonWidth = 90;

				var left = Panel.Size.Width;

				foreach (var button in new[]
				{
					ViewRunLogButton,
					StopButton,
					StartButton,
				})
				{
					if (button != null)
					{
						left -= buttonWidth;

						button.Left = left;
					}
				}

				var width = (left - ProjectLabel.Left - 20) / 2;

				ProjectLabel.Width = width;
				StatusLabel.Left = ProjectLabel.Left + width + 5;
				StatusLabel.Width = width;
			}

			Resize();

			Panel.Resize += (resizeSender, resizeArgs) => { Resize(); };
		}

		public virtual TaskActions[] GetExecuteProjectTaskActions(bool showProjectExecutionInTaskbar)
		{
			var tasks = new List<TaskActions>();

			if (Selected)
			{
				tasks.Add(new()
				{
					PreAction = () =>
					{
						if (!Solution.BuildSolutionErrored)
						{
							Solution.OpenButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { Solution.OpenButton.Visible = false; });
							ViewRunLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { ViewRunLogButton.Visible = false; });

							var exeFileName = ProjectApi.GetExeFileName(new()
							{
								ProjectFileName = ProjectFullName,
								BuildConfiguration = Solution.ActiveBuildConfiguration,
							}).ExeFileName;

							if (!string.IsNullOrEmpty(exeFileName) && System.IO.File.Exists(exeFileName))
							{
								if (WaitForExecuteProjectResponse)
								{
									StatusLabel?.Invoke((System.Windows.Forms.MethodInvoker)delegate
								 {
									 StatusLabel.ForeColor = System.Drawing.Color.Green;
									 StatusLabel.Text = "running ...";
								 });

									var processResponse = ISI.Extensions.Process.WaitForProcessResponse(exeFileName, null, "noWaitAtFinish -noWaitAtFinish");

									ExecuteProjectResponse.ExitCode = processResponse.ExitCode;
									ExecuteProjectResponse.Output = $"{exeFileName}\n{processResponse.Output}";

									StatusLabel?.Invoke((System.Windows.Forms.MethodInvoker)delegate
									{
										if (ExecuteProjectErrored)
										{
											StatusLabel.ForeColor = System.Drawing.Color.Red;
											StatusLabel.Text = "errored ...";
										}
										else
										{
											StatusLabel.ForeColor = System.Windows.Forms.Control.DefaultForeColor;
											StatusLabel.Text = "finished ...";
										}
									});

									Solution.Selected = true;
									Solution.OpenButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { Solution.OpenButton.Visible = true; });

									StartButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { StartButton.Visible = true; });
									ViewRunLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { ViewRunLogButton.Visible = true; });
								}
								else
								{
									StartButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { StartButton.Visible = false; });
									StopButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { StopButton.Visible = true; });

									var processStartInfo = new System.Diagnostics.ProcessStartInfo(exeFileName)
									{
										WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
										//CreateNoWindow = !showProjectExecutionInTaskbar,
										//UseShellExecute = showProjectExecutionInTaskbar,
									};

									if (!showProjectExecutionInTaskbar)
									{
										processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
										processStartInfo.UseShellExecute = false;
										processStartInfo.CreateNoWindow = true;
										processStartInfo.RedirectStandardOutput = true;
										processStartInfo.RedirectStandardError = true;
										processStartInfo.RedirectStandardInput = true;
									}

									//ExecuteProjectInstance = System.Diagnostics.Process.Start(processStartInfo);
									ExecuteProjectInstance = new();
									ExecuteProjectInstance.StartInfo = processStartInfo;
									ExecuteProjectInstance.Start();

									if (!showProjectExecutionInTaskbar)
									{
										//ExecuteProjectResponse.Reset();

										ExecuteProjectInstance.OutputDataReceived += (sender, dataReceivedEventArgs) =>
										{
											var data = dataReceivedEventArgs.Data;

											if (!string.IsNullOrEmpty(data))
											{
												ExecuteProjectResponse.AppendLine(data);
											}
										};

										ExecuteProjectInstance.ErrorDataReceived += (sender, dataReceivedEventArgs) =>
										{
											var data = dataReceivedEventArgs.Data;

											if (!string.IsNullOrEmpty(data))
											{
												ExecuteProjectResponse.AppendLine(data);
											}
										};

										ExecuteProjectInstance.BeginOutputReadLine();

										ViewRunLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { ViewRunLogButton.Visible = true; });
									}

									StatusLabel?.Invoke((System.Windows.Forms.MethodInvoker)delegate
								 {
									 StatusLabel.ForeColor = System.Drawing.Color.Green;
									 StatusLabel.Text = "running ...";
								 });

									ExecuteProjectInstance.Exited += (sender, args) =>
									{
										ExecuteProjectInstance = null;

										StopButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { StopButton.Visible = false; });

										StatusLabel?.Invoke((System.Windows.Forms.MethodInvoker)delegate
									 {
										 StatusLabel.ForeColor = System.Windows.Forms.Control.DefaultForeColor;
										 StatusLabel.Text = "exited ...";
									 });

										Solution.Selected = true;

										StartButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { StartButton.Visible = true; });

										Solution.OpenButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { Solution.OpenButton.Visible = true; });
									};

									ViewRunLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { ViewRunLogButton.Visible = true; });

									ExecuteProjectInstance.EnableRaisingEvents = true;
								}
							}
							else
							{
								ExecuteProjectResponse.ExitCode = 1;
								ExecuteProjectResponse.Output = $"could not find \"{exeFileName}\"";
								ViewRunLogButton?.Invoke((System.Windows.Forms.MethodInvoker)delegate { ViewRunLogButton.Visible = true; });
							}
						}
					},
				});
			}

			return tasks.ToArray();
		}

		public void StopService()
		{
			if ((ExecuteProjectInstance != null) && !ExecuteProjectInstance.HasExited)
			{
				if (StopButton != null)
				{
					StopButton.Visible = false;
				}

				StatusLabel?.Invoke((System.Windows.Forms.MethodInvoker)delegate { StatusLabel.Text = "closing ..."; });

				if (ExecuteProjectInstance.MainWindowHandle != IntPtr.Zero)
				{
					SetForegroundWindow(ExecuteProjectInstance.MainWindowHandle);
					System.Windows.Forms.SendKeys.SendWait("^(c)");
				}
				else
				{
					ExecuteProjectInstance.Kill();
					//ISI.Extensions.Process.GenerateConsoleCtrlEvent(Process.ConsoleCtrlEvent.CTRL_C, ExecuteProjectInstance.SessionId);
					//ExecuteProjectInstance.WaitForExit();

					//ExecuteProjectInstance.StandardInput.WriteLine("\x3");
					//ExecuteProjectInstance.StandardInput.Flush();
					////ExecuteProjectResponse.AppendLine(ExecuteProjectInstance.StandardOutput.ReadToEnd());
				}

				StatusLabel?.Invoke((System.Windows.Forms.MethodInvoker)delegate
			 {
				 StatusLabel.ForeColor = System.Windows.Forms.Control.DefaultForeColor;
				 StatusLabel.Text = "closed ...";
			 });

				Selected = true;

				ExecuteProjectInstance = null;
			}
		}
	}
}
