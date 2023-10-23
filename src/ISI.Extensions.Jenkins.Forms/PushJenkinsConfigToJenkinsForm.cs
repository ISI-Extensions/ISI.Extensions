﻿#region Copyright & License
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
	public partial class PushJenkinsConfigToJenkinsForm : Form
	{
		private static ISI.Extensions.Jenkins.JenkinsSettings _jenkinsSettings = null;
		protected ISI.Extensions.Jenkins.JenkinsSettings JenkinsSettings => _jenkinsSettings ??= new();
		
		private static ISI.Extensions.Jenkins.JenkinsApi _jenkinsApi = null;
		protected ISI.Extensions.Jenkins.JenkinsApi JenkinsApi => _jenkinsApi ??= new();

		protected string[] SelectedItemPaths { get; }

		private ISI.Extensions.Jenkins.JenkinsServer _jenkinsServer = null;
		protected ISI.Extensions.Jenkins.JenkinsServer JenkinsServer
		{
			get
			{
				_jenkinsServer ??= new()
				{
					JenkinsServerUuid = Guid.NewGuid(),
				};

				_jenkinsServer.JenkinsUrl = txtJenkinsUrl.Text ?? string.Empty;
				_jenkinsServer.UserName = txtUserName.Text ?? string.Empty;
				_jenkinsServer.ApiToken = txtApiToken.Text ?? string.Empty;

				return _jenkinsServer;
			}
			set
			{
				_jenkinsServer = value ?? new ISI.Extensions.Jenkins.JenkinsServer()
				{
					JenkinsServerUuid = Guid.NewGuid(),
				};

				txtJenkinsUrl.Text = _jenkinsServer.JenkinsUrl;
				txtUserName.Text = _jenkinsServer.UserName;
				txtApiToken.Text = _jenkinsServer.ApiToken;

				if (cboJenkinsServers.Items.Count > 0)
				{
					var selectedIndex = cboJenkinsServers.SelectedIndex;

					for (int index = 0; index < cboJenkinsServers.Items.Count; index++)
					{
						if (((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[index]).JenkinsServerUuid == _jenkinsServer.JenkinsServerUuid)
						{
							selectedIndex = index;
						}
					}

					if (cboJenkinsServers.SelectedIndex != selectedIndex)
					{
						cboJenkinsServers.SelectedIndex = selectedIndex;
					}
				}
			}
		}

		protected List<JenkinsConfig> JenkinsConfigs { get; }

		public bool ExitOnClose { get; set; }

		public PushJenkinsConfigToJenkinsForm(IEnumerable<string> selectedItemPaths)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SetWindowThemeForms(this);

			JenkinsSettings.ApplyFormSize(nameof(PushJenkinsConfigToJenkinsForm), this);

			flpJenkinsConfigs.Visible = false;
			btnDone.Visible = false;
			btnPush.Visible = false;
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

				tplJenkinsServer.Enabled = (selectedIndex <= 0);

				btnEditJenkinsServers.Text = (selectedIndex <= 0 ? "Add" : "Edit");

				if (cboJenkinsServers.Items.Count > 0)
				{
					JenkinsServer = ((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[selectedIndex]);

					var jenkinsServer = JenkinsServer.GetDecodedJenkinsServer();

					var jobIds = new HashSet<string>(JenkinsApi.GetJobIds(new()
					{
						JenkinsUrl = jenkinsServer.JenkinsUrl,
						UserName = jenkinsServer.UserName,
						ApiToken = jenkinsServer.ApiToken,
					}).JobIds.ToNullCheckedArray(NullCheckCollectionResult.Empty), StringComparer.CurrentCultureIgnoreCase);

					foreach (var jenkinsConfig in JenkinsConfigs)
					{
						jenkinsConfig.Selected = jobIds.Contains(jenkinsConfig.JobId);
					}
				}
				else
				{
					foreach (var jenkinsConfig in JenkinsConfigs)
					{
						jenkinsConfig.Selected = true;
					}
				}
			};

			btnEditJenkinsServers.Click += (sender, args) =>
			{
				using (var form = new ISI.Extensions.Jenkins.Forms.EditJenkinsServerForm(JenkinsServer))
				{
					if (form.ShowDialog() == DialogResult.OK)
					{
						UpdateJenkinsServers();

						JenkinsServer = form.JenkinsServer;
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

			btnPush.Click += (clickSender, clickEventArgs) =>
			{
				btnCancel.Enabled = false;
				btnPush.Enabled = false;

				tplJenkinsServers.Enabled = false;
				tplJenkinsServer.Enabled = false;

				foreach (var jenkinsConfig in JenkinsConfigs)
				{
					jenkinsConfig.Panel.Enabled = false;
				}

				foreach (var jenkinsConfig in JenkinsConfigs.Where(jenkinsConfig => jenkinsConfig.Selected))
				{
					jenkinsConfig.SetStatus(TaskActionStatus.Running, "Pushing");

					try
					{
						var content = System.IO.File.ReadAllText(jenkinsConfig.JenkinsConfigFullName);

						var jenkinsServer = JenkinsServer.GetDecodedJenkinsServer();

						JenkinsApi.SetJobConfigXml(new()
						{
							JenkinsUrl = jenkinsServer.JenkinsUrl,
							UserName = jenkinsServer.UserName,
							ApiToken = jenkinsServer.ApiToken,
							JobId = jenkinsConfig.JobId,
							ConfigXml = content,
						});

						jenkinsConfig.SetStatus(TaskActionStatus.Success, "Done");
					}
					catch (Exception exception)
					{
						Console.WriteLine(exception);

						jenkinsConfig.SetStatus(TaskActionStatus.Errored, "Failed");
					}
				}

				btnCancel.Visible = false;
				btnPush.Visible = false;
				btnDone.Visible = true;
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

			this.Shown += OnShown;
		}

		private void UpdateJenkinsServers()
		{
			cboJenkinsServers.Items.Clear();

			cboJenkinsServers.Items.Add(new ISI.Extensions.Jenkins.JenkinsServer()
			{
				JenkinsServerUuid = Guid.Empty,
				JenkinsUrl = string.Empty,
				Description = string.Empty,
			});

			foreach (var jenkinsServer in JenkinsSettings.GetJenkinsServers())
			{
				cboJenkinsServers.Items.Add(jenkinsServer);
			}

			if (JenkinsConfigs.Any())
			{
				var directory = ISI.Extensions.IO.Path.GetCommonPath(JenkinsConfigs.Select(jenkinsConfig => jenkinsConfig.JenkinsConfigFullName));

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
		}

		private void OnShown(object sender, EventArgs eventArgs)
		{
			Cursor = System.Windows.Forms.Cursors.AppStarting;

			cboJenkinsServers.DisplayMember = nameof(ISI.Extensions.Jenkins.JenkinsServer.DisplayDescription);
			cboJenkinsServers.ValueMember = nameof(ISI.Extensions.Jenkins.JenkinsServer.JenkinsServerUuid);

			foreach (var jenkinsConfigFileName in JenkinsApi.FindJenkinsConfigFileNames(new()
			         {
								 Paths = SelectedItemPaths,
			         }).JenkinsConfigFileNames.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				JenkinsConfigs.Add(new(jenkinsConfigFileName));
			}

			this.flpJenkinsConfigs.Controls.AddRange(JenkinsConfigs.Select(jenkinsConfig => jenkinsConfig.Panel).ToArray());

			UpdateJenkinsServers();

			flpJenkinsConfigs.Visible = true;
			btnCancel.Visible = true;
			btnPush.Visible = true;

			Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		public class JenkinsConfig : IEquatable<JenkinsConfig>
		{
			public string JobId { get; }

			public string JenkinsConfigFullName { get; }

			public Control Panel => flpJenkinsConfig;

			private FlowLayoutPanel flpJenkinsConfig { get; }
			private CheckBox cboJenkinsConfig { get; }
			private Label lblStatus { get; }

			public bool Selected
			{
				get => cboJenkinsConfig.Checked;
				set => cboJenkinsConfig.Checked = value;
			}

			public JenkinsConfig(string jenkinsConfigFullName)
			{
				JenkinsConfigFullName = jenkinsConfigFullName;


				JobId = System.IO.Path.GetFileNameWithoutExtension(JenkinsConfigFullName);

				flpJenkinsConfig = new()
				{
					AutoSize = true,
					AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly,
					Margin = new(3, 3, 3, 0),
					Size = new(659, 24),
				};

				cboJenkinsConfig = new()
				{
					Text = JobId,
					Name = string.Format("cboJenkinsConfig_{0}", this.GetHashCode()),
					Width = 400,
					Height = 17,
					Margin = new(1, 1, 1, 1),
					Checked = true,
				};
				flpJenkinsConfig.Controls.Add(cboJenkinsConfig);

				lblStatus = new()
				{
					AutoSize = true,
					Location = new(83, 1),
					Margin = new(4),
					MinimumSize = new(300, 17),
					Size = new(240, 17),
				};
				flpJenkinsConfig.Controls.Add(lblStatus);
			}

			public void SetStatus(TaskActionStatus taskActionStatus, string status)
			{
				if (lblStatus.InvokeRequired)
				{
					lblStatus.Invoke((System.Windows.Forms.MethodInvoker)delegate
					{
						lblStatus.ForeColor = taskActionStatus.GetColor();
						lblStatus.Text = status;
					});
				}
				else
				{
					lblStatus.ForeColor = taskActionStatus.GetColor();
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
					return JenkinsConfigFullName?.GetHashCode() ?? 0;
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
				return StringComparer.Compare(x.JenkinsConfigFullName, y.JenkinsConfigFullName);
			}
		}
	}
}
