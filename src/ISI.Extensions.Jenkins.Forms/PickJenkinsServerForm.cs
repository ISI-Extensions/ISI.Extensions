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
using ISI.Extensions.Jenkins.Extensions;
using ISI.Extensions.Jenkins.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Jenkins.Forms
{
	public partial class PickJenkinsServerForm : Form
	{
		private static ISI.Extensions.Jenkins.JenkinsApi _jenkinsApi = null;
		protected ISI.Extensions.Jenkins.JenkinsApi JenkinsApi => _jenkinsApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Jenkins.JenkinsApi>();

		private ISI.Extensions.Jenkins.JenkinsServer _jenkinsServer = null;
		public ISI.Extensions.Jenkins.JenkinsServer JenkinsServer
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

					for (var index = 0; index < cboJenkinsServers.Items.Count; index++)
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

		public PickJenkinsServerForm(ISI.Extensions.Jenkins.JenkinsServer jenkinsServer)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			JenkinsApi.ApplyFormSize(nameof(PickJenkinsServerForm), this);

			btnOK.Visible = false;
			btnCancel.Visible = false;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			JenkinsServer = jenkinsServer;

			cboJenkinsServers.SelectedValueChanged += (sender, args) =>
			{
				var selectedIndex = (cboJenkinsServers.SelectedIndex < 0 ? 0 : cboJenkinsServers.SelectedIndex);

				tplJenkinsServer.Enabled = (selectedIndex <= 0);

				btnEditJenkinsServer.Text = (selectedIndex <= 0 ? "Add" : "Edit");

				if (cboJenkinsServers.Items.Count > 0)
				{
					_jenkinsServer = ((ISI.Extensions.Jenkins.JenkinsServer)cboJenkinsServers.Items[selectedIndex]);
					txtJenkinsUrl.Text = _jenkinsServer.JenkinsUrl;
					txtUserName.Text = _jenkinsServer.UserName;
					txtApiToken.Text = _jenkinsServer.ApiToken;
				}
			};

			btnEditJenkinsServer.Click += (sender, args) =>
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
				}
			};

			btnOK.Click += (clickSender, clickEventArgs) =>
			{
				JenkinsApi.RecordFormSize(this);

				if (this.Modal)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				else
				{
					this.Close();
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

			foreach (var jenkinsServer in JenkinsApi.GetJenkinsServers())
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

			btnCancel.Visible = true;
			btnOK.Visible = true;

			Cursor = System.Windows.Forms.Cursors.Arrow;
		}
	}
}
