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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISI.Extensions.WinForms
{
	public delegate void ApplyFormSizeDelegate(System.Windows.Forms.Form form);
	public delegate void RecordFormSizeDelegate(System.Windows.Forms.Form form);
	public delegate void OnShowDialogDelegate(ISI.Extensions.StatusTrackers.AddToLog addToLog);

	public partial class LogForm : Form
	{
		protected List<string> Logs { get; }
		protected List<LogPanel> LogPanels { get; }

		protected ApplyFormSizeDelegate ApplyFormSize { get; }
		protected RecordFormSizeDelegate RecordFormSize { get; }
		protected OnShowDialogDelegate OnShowDialog { get; }

		public bool ShowDoneButtonOnStartUp { get; set; } = true;

		public LogForm(ApplyFormSizeDelegate applyFormSize, RecordFormSizeDelegate recordFormSize, OnShowDialogDelegate onShowDialog)
		{
			ApplyFormSize = applyFormSize;
			RecordFormSize = recordFormSize;
			OnShowDialog = onShowDialog;

			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			ApplyFormSize?.Invoke(this);

			flpLogs.Visible = false;
			btnDone.Visible = false;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			Logs = new();
			LogPanels = new();

			btnDone.Click += (clickSender, clickEventArgs) =>
			{
				if (Modal)
				{
					DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				else
				{
					Close();
				}
			};

			btnCopyToClipboard.Click += (clickSender, clickEventArgs) =>
			{
				var stringBuilder = new StringBuilder();

				foreach (var log in Logs)
				{
					stringBuilder.AppendLine(log);
				}

				foreach (var logPanel in LogPanels)
				{
					stringBuilder.AppendLine(logPanel.Description);
				}

				Clipboard.Clear();
				Clipboard.SetText(stringBuilder.ToString());
			};

			Shown += OnShown;

			FormClosed += OnFormClosed;
		}

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			RecordFormSize?.Invoke(this);

			flpLogs.Controls.Clear();

			foreach (var logPanel in LogPanels)
			{
				//flpLogs.Controls.Remove(logPanel.Panel);
				logPanel.Dispose();
			}
		}

		private void OnShown(object sender, EventArgs eventArgs)
		{
			Cursor = System.Windows.Forms.Cursors.AppStarting;

			flpLogs.Visible = true;

			Cursor = System.Windows.Forms.Cursors.WaitCursor;

			OnShowDialog?.Invoke(AddToLog);

			Cursor = System.Windows.Forms.Cursors.Arrow;

			btnDone.Visible = ShowDoneButtonOnStartUp;
		}

		public void HideDoneButton()
		{
			btnDone.Invoke((System.Windows.Forms.MethodInvoker)delegate
			{
				btnDone.Visible = false;
			});
		}

		public void ShowDoneButton()
		{
			btnDone.Invoke((System.Windows.Forms.MethodInvoker)delegate
			{
				btnDone.Visible = true;
			});
		}

		public void AddToLog(ISI.Extensions.StatusTrackers.LogEntryLevel logEntryLevel, string description)
		{
			flpLogs.Invoke((System.Windows.Forms.MethodInvoker)delegate
			{
				flpLogs.SuspendLayout();

				var logPanel = (LogPanel)null;

				foreach (var log in description.Replace("\r\n", "\n").Split('\n'))
				{
					if (!string.IsNullOrWhiteSpace(log))
					{
						if (LogPanels.Count > 500)
						{
							logPanel = LogPanels.First();

							if (!logPanel.IsBigLogMessage)
							{
								Logs.Add(logPanel.Description);
								logPanel.SetBigLogMessage("Clipped older messages");
							}

							logPanel = LogPanels[1];

							Logs.Add(logPanel.Description);

							LogPanels.RemoveAt(1);
							flpLogs.Controls.Remove(logPanel.Panel);

							logPanel.Description = log;

							LogPanels.Add(logPanel);
							flpLogs.Controls.Add(logPanel.Panel);
						}
						else
						{
							logPanel = new LogPanel(log);

							flpLogs.Controls.Add(logPanel.Panel);

							LogPanels.Add(logPanel);
						}
					}
				}

				flpLogs.ResumeLayout(false);
				flpLogs.PerformLayout();

				if(logPanel != null)
				{
					flpLogs.ScrollControlIntoView(logPanel.Panel);
				}

				if ((LogPanels.Count + Logs.Count) % 10 == 0)
				{
					Application.DoEvents();
				}
			});
		}

		public class LogPanel : IDisposable
		{
			public Control Panel => flpLog;

			private FlowLayoutPanel flpLog { get; }
			private Label lblDescription { get; }

			public string Description
			{
				get => lblDescription.Text;
				set => lblDescription.Text = value;
			}

			public bool IsBigLogMessage { get; private set; } = false;

			public LogPanel(string description)
			{
				flpLog = new()
				{
					AutoSize = true,
					AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly,
					Margin = new(3, 3, 3, 0),
					Size = new(386, 24),
				};

				lblDescription = new()
				{
					AutoSize = true,
					Location = new(1, 1),
					Margin = new(4),
					MinimumSize = new(300, 17),
					Size = new(380, 17),
					Text = description,
				};
				flpLog.Controls.Add(lblDescription);

				ISI.Extensions.WinForms.ThemeHelper.SyncTheme(flpLog);
			}

			public void SetBigLogMessage(string message)
			{
				IsBigLogMessage = true;
				lblDescription.Text = message;
				lblDescription.ForeColor = Color.Red;
			}

			public void Dispose()
			{
				flpLog?.Dispose();
				lblDescription?.Dispose();
			}
		}
	}
}
