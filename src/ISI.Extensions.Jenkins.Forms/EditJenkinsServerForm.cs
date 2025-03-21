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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISI.Extensions.Jenkins.Extensions;
using ISI.Extensions.Jenkins.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Jenkins.Forms
{
	public partial class EditJenkinsServerForm : Form
	{
		private static ISI.Extensions.Jenkins.JenkinsApi _jenkinsApi = null;
		protected ISI.Extensions.Jenkins.JenkinsApi JenkinsApi => _jenkinsApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Jenkins.JenkinsApi>();

		protected List<DirectoryPanel> DirectoryPanels { get; }

		public ISI.Extensions.Jenkins.JenkinsServer JenkinsServer { get; } = null;

		public EditJenkinsServerForm()
			: this(null, null)
		{
		}

		public EditJenkinsServerForm(Guid jenkinsServerUuid)
			: this(jenkinsServerUuid, null)
		{
		}

		public EditJenkinsServerForm(ISI.Extensions.Jenkins.JenkinsServer jenkinsServer)
			: this(null, jenkinsServer)
		{
		}

		private EditJenkinsServerForm(Guid? jenkinsServerUuid, ISI.Extensions.Jenkins.JenkinsServer jenkinsServer)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			JenkinsApi.ApplyFormSize(nameof(EditJenkinsServerForm), this);

			flpDirectories.Visible = false;
			btnCancel.Visible = false;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			JenkinsServer = jenkinsServer;

			if (jenkinsServerUuid.HasValue)
			{
				JenkinsServer = JenkinsApi.GetJenkinsServer(jenkinsServerUuid.Value);
			}

			JenkinsServer ??= new()
			{
				JenkinsServerUuid = Guid.NewGuid(),
				UserName = "%USERNAME%",
			};

			btnAddDirectory.Click += (sender, args) =>
			{
				folderBrowserDialogAddDirectory.Description = "Select directory";
				folderBrowserDialogAddDirectory.ShowNewFolderButton = false;

				if (folderBrowserDialogAddDirectory.ShowDialog() == DialogResult.OK)
				{
					AddDirectoryPanel(folderBrowserDialogAddDirectory.SelectedPath);
				}
			};

			DirectoryPanels = new();

			Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs eventArgs)
		{
			Cursor = System.Windows.Forms.Cursors.AppStarting;

			txtJenkinsUrl.Text = JenkinsServer.JenkinsUrl;
			txtDescription.Text = JenkinsServer.Description;
			txtUserName.Text = JenkinsServer.UserName;
			txtApiToken.Text = JenkinsServer.ApiToken;

			var directories = (JenkinsServer.Directories ?? []);
			foreach (var directory in directories)
			{
				AddDirectoryPanel(directory);
			}

			folderBrowserDialogAddDirectory.RootFolder = Environment.SpecialFolder.MyComputer;
			if (directories.Any())
			{
				folderBrowserDialogAddDirectory.SelectedPath = ISI.Extensions.IO.Path.GetCommonPath(directories);
			}

			btnOK.Click += (clickSender, clickEventArgs) =>
			{
				JenkinsApi.RecordFormSize(this);

				JenkinsServer.JenkinsUrl = txtJenkinsUrl.Text;
				JenkinsServer.Description = txtDescription.Text;
				JenkinsServer.UserName = txtUserName.Text;
				JenkinsServer.ApiToken = txtApiToken.Text;
				JenkinsServer.Directories = DirectoryPanels.Where(directoryPanel => !directoryPanel.Delete).Select(directoryPanel => directoryPanel.Directory).ToArray();
				
				JenkinsApi.SetJenkinsServerSettings(new()
				{
					JenkinsServer = JenkinsServer,
				});

				if (this.Modal)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				else
				{
					this.Close();
				}
			};
			btnCancel.Click += (clickSender, clickEventArgs) =>
			{
				if (!this.Modal)
				{
					this.Close();
				}
			};

			flpDirectories.Visible = true;
			btnCancel.Visible = true;

			Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		private void AddDirectoryPanel(string directory)
		{
			var directoryPanel = new DirectoryPanel(directory);

			this.flpDirectories.Controls.Add(directoryPanel.Panel);
			this.flpDirectories.Controls.SetChildIndex(directoryPanel.Panel, this.flpDirectories.Controls.Count - 2);

			directoryPanel.Refresh();

			DirectoryPanels.Add(directoryPanel);
		}

		public class DirectoryPanel
		{
			public string Directory { get; }
			public bool Delete { get; set; }

			public Control Panel => flpDirectory;

			private FlowLayoutPanel flpDirectory { get; }
			private Label lblDirectory { get; }
			private Button btnDelete { get; }

			public DirectoryPanel(string directory)
			{
				Directory = directory;

				flpDirectory = new()
				{
					AutoSize = true,
					AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly,
					Margin = new(3, 3, 3, 0),
					Size = new(386, 24),
				};

				btnDelete = new()
				{
					Location = new(1, 1),
					Margin = new(1),
					Size = new(80, 20),
					Text = "Delete",
					UseVisualStyleBackColor = true,
				};
				flpDirectory.Controls.Add(btnDelete);
				btnDelete.Click += (sender, args) =>
				{
					Delete = true;
					btnDelete.Enabled = false;
					lblDirectory.ForeColor = Color.Red;
				};

				lblDirectory = new()
				{
					AutoSize = true,
					Location = new(83, 1),
					Margin = new(4),
					MinimumSize = new(300, 17),
					Size = new(300, 17),
				};
				flpDirectory.Controls.Add(lblDirectory);
			}

			public void Refresh()
			{
				lblDirectory.Text = Directory;
			}
		}
	}
}
