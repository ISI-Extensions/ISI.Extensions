#region Copyright & License
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
using ISI.Extensions.Jenkins.Forms.Extensions;

namespace ISI.Extensions.Jenkins.Forms
{
	public partial class PickExistingJenkinsServerForm : Form
	{
		private static ISI.Extensions.Jenkins.JenkinsSettings _jenkinsSettings = null;
		protected ISI.Extensions.Jenkins.JenkinsSettings JenkinsSettings => _jenkinsSettings ??= new();

		protected List<JenkinsServerPanel> JenkinsServerPanels { get; }

		public ISI.Extensions.Jenkins.JenkinsServer JenkinsServer { get; set; }

		public PickExistingJenkinsServerForm()
		{
			InitializeComponent();

			JenkinsSettings.ApplyFormSize(nameof(JenkinsServersForm), this);

			flpJenkinsServers.Visible = false;
			btnCancel.Visible = false;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			JenkinsServerPanels = new();


			btnCancel.Click += (clickSender, clickEventArgs) =>
			{
				if (!this.Modal)
				{
					this.Close();
				}
			};

			btnAddJenkinsServer.Click += (sender, args) =>
			{
				using (var form = new ISI.Extensions.Jenkins.Forms.EditJenkinsServerForm())
				{
					if (form.ShowDialog() == DialogResult.OK)
					{
						AddJenkinsServerPanel(form.JenkinsServer);
					}
				}
			};

			this.Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs eventArgs)
		{
			Cursor = System.Windows.Forms.Cursors.AppStarting;

			foreach (var jenkinsServer in JenkinsSettings.GetJenkinsServers())
			{
				AddJenkinsServerPanel(jenkinsServer);
			}

			flpJenkinsServers.Visible = true;
			btnCancel.Visible = true;

			Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		private JenkinsServerPanel AddJenkinsServerPanel(ISI.Extensions.Jenkins.JenkinsServer jenkinsServer)
		{
			var jenkinsServerPanel = new JenkinsServerPanel(jenkinsServer, UseJenkinsServer);

			this.flpJenkinsServers.Controls.Add(jenkinsServerPanel.Panel);
			this.flpJenkinsServers.Controls.SetChildIndex(jenkinsServerPanel.Panel, this.flpJenkinsServers.Controls.Count - 2);

			jenkinsServerPanel.Refresh();

			JenkinsServerPanels.Add(jenkinsServerPanel);

			return jenkinsServerPanel;
		}

		private void UseJenkinsServer(ISI.Extensions.Jenkins.JenkinsServer jenkinsServer)
		{
			JenkinsServer = jenkinsServer;

			JenkinsSettings.RecordFormSize(this);

			if (this.Modal)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else
			{
				this.Close();
			}
		}

		public class JenkinsServerPanel
		{
			public ISI.Extensions.Jenkins.JenkinsServer JenkinsServer { get; }

			public Action<ISI.Extensions.Jenkins.JenkinsServer> UseJenkinsServer { get; }

			public Control Panel => flpJenkinsServer;

			private FlowLayoutPanel flpJenkinsServer { get; }
			private Label lblDescription { get; }
			private Button btnUse { get; }
			private Button btnEdit { get; }

			public JenkinsServerPanel(ISI.Extensions.Jenkins.JenkinsServer jenkinsServer, Action<ISI.Extensions.Jenkins.JenkinsServer> useJenkinsServer)
			{
				JenkinsServer = jenkinsServer;
				UseJenkinsServer = useJenkinsServer;

				flpJenkinsServer = new()
				{
					AutoSize = true,
					AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly,
					Margin = new(3, 3, 3, 0),
					Size = new(386, 24),
				};

				btnUse = new()
				{
					Location = new(1, 1),
					Margin = new(1),
					Size = new(80, 20),
					Text = "Use",
					UseVisualStyleBackColor = true,
				};
				flpJenkinsServer.Controls.Add(btnUse);
				btnUse.Click += (sender, args) =>
				{
					UseJenkinsServer(JenkinsServer);
				};

				btnEdit = new()
				{
					Location = new(1, 1),
					Margin = new(1),
					Size = new(80, 20),
					Text = "Edit",
					UseVisualStyleBackColor = true,
				};
				flpJenkinsServer.Controls.Add(btnEdit);
				btnEdit.Click += (sender, args) =>
				{
					using (var form = new ISI.Extensions.Jenkins.Forms.EditJenkinsServerForm(JenkinsServer))
					{
						if (form.ShowDialog() == DialogResult.OK)
						{
							Refresh();
						}
					}
				};

				lblDescription = new()
				{
					AutoSize = true,
					Location = new(83, 1),
					Margin = new(4),
					MinimumSize = new(300, 17),
					Size = new(300, 17),
				};
				flpJenkinsServer.Controls.Add(lblDescription);
			}

			public void Refresh()
			{
				lblDescription.Text = (string.IsNullOrWhiteSpace(JenkinsServer.Description) ? JenkinsServer.JenkinsUrl : JenkinsServer.Description);
			}
		}
	}
}
