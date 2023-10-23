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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISI.Extensions.Extensions;
using ISI.Extensions.Jenkins.Forms.Extensions;

namespace ISI.Extensions.Jenkins.Forms
{
	public partial class PullJenkinsConfigFromJenkinsForm : Form
	{
		private static ISI.Extensions.Jenkins.JenkinsSettings _jenkinsSettings = null;
		protected ISI.Extensions.Jenkins.JenkinsSettings JenkinsSettings => _jenkinsSettings ??= new();

		private static ISI.Extensions.Jenkins.JenkinsApi _jenkinsApi = null;
		protected ISI.Extensions.Jenkins.JenkinsApi JenkinsApi => _jenkinsApi ??= new();

		protected string[] SelectedItemPaths { get; }

		protected List<JenkinsConfig> JenkinsConfigs { get; }

		public bool ExitOnClose { get; set; }

		public PullJenkinsConfigFromJenkinsForm(IEnumerable<string> selectedItemPaths)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			JenkinsSettings.ApplyFormSize(nameof(PullJenkinsConfigFromJenkinsForm), this);

			flpJenkinsConfigs.Visible = false;
			btnDone.Visible = false;
			btnPull.Visible = false;
			btnCancel.Visible = false;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			SelectedItemPaths = selectedItemPaths.ToArray();

			JenkinsConfigs = new();

			cboJenkinsServers.SelectedValueChanged += (sender, args) =>
			{
				var selectedIndex = (cboJenkinsServers.SelectedIndex < 0 ? 0 : cboJenkinsServers.SelectedIndex);

				btnEditJenkinsServers.Text = (selectedIndex <= 0 ? "Add" : "Edit");
				btnPull.Enabled = (selectedIndex > 0);

				JenkinsConfigs.Clear();
				flpJenkinsConfigs.Controls.Clear();

				if (selectedIndex > 0)
				{
					Cursor = System.Windows.Forms.Cursors.AppStarting;

					var jenkinsServer = ((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[selectedIndex]).GetDecodedJenkinsServer();

					var jobIds = JenkinsApi.GetJobIds(new()
					{
						JenkinsUrl = jenkinsServer.JenkinsUrl,
						UserName = jenkinsServer.UserName,
						ApiToken = jenkinsServer.ApiToken,
					}).JobIds.NullCheckedOrderBy(jobId => jobId, StringComparer.InvariantCultureIgnoreCase, NullCheckCollectionResult.Empty);

					var localJobIds = new HashSet<string>(JenkinsApi.FindJenkinsConfigFileNames(new()
					{
						Paths = SelectedItemPaths,
					}).JenkinsConfigFileNames.ToNullCheckedArray(System.IO.Path.GetFileNameWithoutExtension, NullCheckCollectionResult.Empty), StringComparer.InvariantCultureIgnoreCase);

					foreach (var jobId in jobIds)
					{
						JenkinsConfigs.Add(new(jobId)
						{
							Selected = !localJobIds.Any() || localJobIds.Contains(jobId)
						});
					}

					flpJenkinsConfigs.Controls.AddRange(JenkinsConfigs.Select(jenkinsConfig => jenkinsConfig.Panel).ToArray());

					Cursor = System.Windows.Forms.Cursors.Arrow;
				}
			};

			btnEditJenkinsServers.Click += (sender, args) =>
			{
				var selectedIndex = (cboJenkinsServers.SelectedIndex < 0 ? 0 : cboJenkinsServers.SelectedIndex);

				var jenkinsServer = ((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[selectedIndex]);

				using (var form = new ISI.Extensions.Jenkins.Forms.EditJenkinsServerForm(jenkinsServer))
				{
					if (form.ShowDialog() == DialogResult.OK)
					{
						UpdateJenkinsServers();

						for (int index = 0; index < cboJenkinsServers.Items.Count; index++)
						{
							if (((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[index]).JenkinsServerUuid == form.JenkinsServer.JenkinsServerUuid)
							{
								cboJenkinsServers.SelectedIndex = index;
							}
						}
					}
				}
			};

			btnCancel.Click += (clickSender, clickEventArgs) =>
			{
				if (!this.Modal)
				{
					this.Close();

					if (ExitOnClose)
					{
						System.Windows.Forms.Application.Exit();
					}
				}
			};

			btnPull.Click += (clickSender, clickEventArgs) =>
			{
				var selectedIndex = (cboJenkinsServers.SelectedIndex < 0 ? 0 : cboJenkinsServers.SelectedIndex);

				if (selectedIndex > 0)
				{
					Cursor = System.Windows.Forms.Cursors.AppStarting;

					var directory = ISI.Extensions.IO.Path.GetCommonPath(SelectedItemPaths);

					var jenkinsServer = ((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[selectedIndex]).GetDecodedJenkinsServer();

					btnCancel.Enabled = false;
					btnPull.Enabled = false;

					foreach (var jenkinsConfig in JenkinsConfigs)
					{
						jenkinsConfig.Enabled = false;
					}

					var directoryJobIds = JenkinsApi.FindJenkinsConfigFileNames(new()
					{
						Paths = SelectedItemPaths,
					}).JenkinsConfigFileNames.ToDictionary(System.IO.Path.GetFileNameWithoutExtension, jenkinsConfigFileName => jenkinsConfigFileName, StringComparer.InvariantCultureIgnoreCase);

					foreach (var jenkinsConfig in JenkinsConfigs.Where(jenkinsConfig => jenkinsConfig.Selected))
					{
						jenkinsConfig.SetStatus(TaskActionStatus.Running, "Pulling");

						try
						{
							var content = JenkinsApi.GetJobConfigXml(new()
							{
								JenkinsUrl = jenkinsServer.JenkinsUrl,
								UserName = jenkinsServer.UserName,
								ApiToken = jenkinsServer.ApiToken,
								JobId = jenkinsConfig.JobId,
							}).ConfigXml;

							if (!directoryJobIds.TryGetValue(jenkinsConfig.JobId, out var jenkinsConfigFileName))
							{
								jenkinsConfigFileName = System.IO.Path.Combine(directory, string.Format("{0}{1}", jenkinsConfig.JobId, JenkinsApi.JenkinsConfigFileNameExtension));
							}

							System.IO.File.WriteAllText(jenkinsConfigFileName, content);

							jenkinsConfig.SetStatus(TaskActionStatus.Success, "Done");
						}
						catch (Exception exception)
						{
							Console.WriteLine(exception);

							jenkinsConfig.SetStatus(TaskActionStatus.Errored, "Failed");
						}
					}

					btnCancel.Visible = false;
					btnPull.Visible = false;
					btnDone.Visible = true;

					Cursor = System.Windows.Forms.Cursors.Arrow;
				}
			};

			btnDone.Click += (clickSender, clickEventArgs) =>
			{
				JenkinsSettings.RecordFormSize(this);

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

			Shown += OnShown;
		}

		private void UpdateJenkinsServers()
		{
			cboJenkinsServers.Items.Clear();

			cboJenkinsServers.Items.Add(new ISI.Extensions.Jenkins.JenkinsServer()
			{
				JenkinsServerUuid = Guid.NewGuid(),
				JenkinsUrl = string.Empty,
				Description = string.Empty,
			});

			foreach (var jenkinsServer in JenkinsSettings.GetJenkinsServers())
			{
				cboJenkinsServers.Items.Add(jenkinsServer);
			}
		}

		private void OnShown(object sender, EventArgs eventArgs)
		{
			Cursor = System.Windows.Forms.Cursors.AppStarting;

			cboJenkinsServers.DisplayMember = nameof(ISI.Extensions.Jenkins.JenkinsServer.DisplayDescription);
			cboJenkinsServers.ValueMember = nameof(ISI.Extensions.Jenkins.JenkinsServer.JenkinsServerUuid);

			UpdateJenkinsServers();

			if (JenkinsConfigs.Any())
			{
				var directory = ISI.Extensions.IO.Path.GetCommonPath(SelectedItemPaths);

				var jenkinsServerUuid = JenkinsSettings.FindJenkinsServerByDirectory(directory, true)?.JenkinsServerUuid ?? Guid.Empty;

				if (jenkinsServerUuid != Guid.Empty)
				{
					for (int index = 0; index < cboJenkinsServers.Items.Count; index++)
					{
						if (((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[index]).JenkinsServerUuid == jenkinsServerUuid)
						{
							cboJenkinsServers.SelectedIndex = index;
						}
					}
				}
			}

			flpJenkinsConfigs.Visible = true;
			btnCancel.Visible = true;
			btnPull.Visible = true;

			Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		public class JenkinsConfig : IEquatable<JenkinsConfig>
		{
			public string JobId { get; }

			public Control Panel => flpJenkinsConfig;

			private FlowLayoutPanel flpJenkinsConfig { get; }
			private CheckBox cboJenkinsConfig { get; }
			private Label lblJobId { get; }
			private Label lblStatus { get; }

			public bool Enabled
			{
				get => cboJenkinsConfig.Enabled;
				set => cboJenkinsConfig.Enabled = value;
			}

			public bool Selected
			{
				get => cboJenkinsConfig.Checked;
				set => cboJenkinsConfig.Checked = value;
			}

			public JenkinsConfig(string jobId)
			{
				JobId = jobId;

				flpJenkinsConfig = new()
				{
					AutoSize = true,
					AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly,
					Margin = new(3, 3, 3, 0),
					Size = new(600, 24),
					WrapContents = false,
				};

				cboJenkinsConfig = new()
				{
					Name = string.Format("cboJenkinsConfig_{0}", this.GetHashCode()),
					Width = 20,
					Height = 17,
					Margin = new(1, 1, 1, 1)
				};
				flpJenkinsConfig.Controls.Add(cboJenkinsConfig);

				lblJobId = new()
				{
					Text = JobId,
					AutoSize = true,
					Margin = new(4),
					MinimumSize = new(300, 17),
					Size = new(200, 17),
				};
				flpJenkinsConfig.Controls.Add(lblJobId);

				lblStatus = new()
				{
					AutoSize = true,
					Margin = new(4),
					MinimumSize = new(300, 17),
					Size = new(200, 17),
				};
				flpJenkinsConfig.Controls.Add(lblStatus);
			}

			public void SetStatus(TaskActionStatus taskActionStatus, string status)
			{
				if (lblStatus.InvokeRequired)
				{
					lblStatus.Invoke((System.Windows.Forms.MethodInvoker)delegate
					{
						lblStatus.ForeColor = taskActionStatus.GetColor(lblStatus);
						lblStatus.Text = status;
					});
				}
				else
				{
					lblStatus.ForeColor = taskActionStatus.GetColor(lblStatus);
					lblStatus.Text = status;
				}
			}

			public bool Equals(JenkinsConfig other)
			{
				if (ReferenceEquals(null, other))
				{
					return false;
				}
				if (ReferenceEquals(this, other))
				{
					return true;
				}

				return (JenkinsConfigComparer.InvariantCultureIgnoreCase.Compare(this, other) == 0);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return JobId?.GetHashCode() ?? 0;
				}
			}
		}

		public class JenkinsConfigComparer : IComparer<JenkinsConfig>
		{
			private static JenkinsConfigComparer _invariantCultureIgnoreCase = null;
			public static JenkinsConfigComparer InvariantCultureIgnoreCase => _invariantCultureIgnoreCase ??= new(StringComparer.InvariantCultureIgnoreCase);

			private StringComparer StringComparer { get; }

			private JenkinsConfigComparer(StringComparer stringComparer)
			{
				StringComparer = stringComparer;
			}

			public int Compare(JenkinsConfig x, JenkinsConfig y)
			{
				return StringComparer.Compare(x.JobId, y.JobId);
			}
		}
	}
}
