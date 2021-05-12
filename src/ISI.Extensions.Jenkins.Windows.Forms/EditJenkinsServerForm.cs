﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISI.Extensions.Windows.Forms.Extensions;

namespace ISI.Extensions.Jenkins.UI
{
	public partial class EditJenkinsServerForm : Form
	{
		private static ISI.Extensions.Jenkins.JenkinsSettings _jenkinsSettings = null;
		protected ISI.Extensions.Jenkins.JenkinsSettings JenkinsSettings => _jenkinsSettings ??= new ISI.Extensions.Jenkins.JenkinsSettings();

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

			//this.ApplyFormLocationAndSize(nameof(EditJenkinsServerForm), new List<>());
			//JenkinsSettings.ApplyFormSize(nameof(EditJenkinsServerForm), this);

			flpDirectories.Visible = false;
			btnCancel.Visible = false;

			Icon = new Icon(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			JenkinsServer = jenkinsServer;

			if (jenkinsServerUuid.HasValue)
			{
				JenkinsServer = JenkinsSettings.GetJenkinsServer(jenkinsServerUuid.Value);
			}

			if (JenkinsServer == null)
			{
				JenkinsServer = new ISI.Extensions.Jenkins.JenkinsServer()
				{
					JenkinsServerUuid = Guid.NewGuid(),
					UserName = "%USERNAME%",
				};
			}

			btnAddDirectory.Click += (sender, args) =>
			{
				folderBrowserDialogAddDirectory.Description = "Select directory";
				folderBrowserDialogAddDirectory.ShowNewFolderButton = false;

				if (folderBrowserDialogAddDirectory.ShowDialog() == DialogResult.OK)
				{
					AddDirectoryPanel(folderBrowserDialogAddDirectory.SelectedPath);
				}
			};

			DirectoryPanels = new List<DirectoryPanel>();

			this.Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs eventArgs)
		{
			Cursor = System.Windows.Forms.Cursors.AppStarting;

			txtJenkinsUrl.Text = JenkinsServer.JenkinsUrl;
			txtDescription.Text = JenkinsServer.Description;
			txtUserName.Text = JenkinsServer.UserName;
			txtApiToken.Text = JenkinsServer.ApiToken;

			var directories = (JenkinsServer.Directories ?? new string[0]);
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
				//JenkinsSettings.RecordFormSize(this);

				JenkinsServer.JenkinsUrl = txtJenkinsUrl.Text;
				JenkinsServer.Description = txtDescription.Text;
				JenkinsServer.UserName = txtUserName.Text;
				JenkinsServer.ApiToken = txtApiToken.Text;
				JenkinsServer.Directories = DirectoryPanels.Where(directoryPanel => !directoryPanel.Delete).Select(directoryPanel => directoryPanel.Directory).ToArray();

				//JenkinsSettings.SetJenkinsServer(JenkinsServer);

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

				flpDirectory = new FlowLayoutPanel()
				{
					AutoSize = true,
					AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly,
					Margin = new System.Windows.Forms.Padding(3, 3, 3, 0),
					Size = new System.Drawing.Size(386, 24),
				};

				btnDelete = new Button()
				{
					Location = new System.Drawing.Point(1, 1),
					Margin = new System.Windows.Forms.Padding(1),
					Size = new System.Drawing.Size(80, 20),
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

				lblDirectory = new Label()
				{
					AutoSize = true,
					Location = new System.Drawing.Point(83, 1),
					Margin = new System.Windows.Forms.Padding(4),
					MinimumSize = new System.Drawing.Size(300, 17),
					Size = new System.Drawing.Size(300, 17),
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