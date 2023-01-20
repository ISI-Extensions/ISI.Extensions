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
 
namespace ISI.Extensions.Jenkins.Forms
{
	partial class PickJenkinsServerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tplForm = new System.Windows.Forms.TableLayoutPanel();
			this.tplJenkinsServers = new System.Windows.Forms.TableLayoutPanel();
			this.lblJenkinsServers = new System.Windows.Forms.Label();
			this.cboJenkinsServers = new System.Windows.Forms.ComboBox();
			this.btnEditJenkinsServer = new System.Windows.Forms.Button();
			this.tplJenkinsServer = new System.Windows.Forms.TableLayoutPanel();
			this.lblJenkinsUrl = new System.Windows.Forms.Label();
			this.txtJenkinsUrl = new System.Windows.Forms.TextBox();
			this.lblUserName = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblApiToken = new System.Windows.Forms.Label();
			this.txtApiToken = new System.Windows.Forms.TextBox();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tplForm.SuspendLayout();
			this.tplJenkinsServers.SuspendLayout();
			this.tplJenkinsServer.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 1;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.tplJenkinsServers, 0, 0);
			this.tplForm.Controls.Add(this.tplJenkinsServer, 0, 1);
			this.tplForm.Controls.Add(this.flpButtons, 0, 3);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 4;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.Size = new System.Drawing.Size(671, 194);
			this.tplForm.TabIndex = 0;
			// 
			// tplJenkinsServers
			// 
			this.tplJenkinsServers.ColumnCount = 3;
			this.tplJenkinsServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tplJenkinsServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplJenkinsServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.tplJenkinsServers.Controls.Add(this.lblJenkinsServers, 0, 0);
			this.tplJenkinsServers.Controls.Add(this.cboJenkinsServers, 1, 0);
			this.tplJenkinsServers.Controls.Add(this.btnEditJenkinsServer, 2, 0);
			this.tplJenkinsServers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplJenkinsServers.Location = new System.Drawing.Point(0, 0);
			this.tplJenkinsServers.Margin = new System.Windows.Forms.Padding(0);
			this.tplJenkinsServers.Name = "tplJenkinsServers";
			this.tplJenkinsServers.RowCount = 1;
			this.tplJenkinsServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tplJenkinsServers.Size = new System.Drawing.Size(671, 31);
			this.tplJenkinsServers.TabIndex = 1;
			// 
			// lblJenkinsServers
			// 
			this.lblJenkinsServers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblJenkinsServers.Location = new System.Drawing.Point(3, 0);
			this.lblJenkinsServers.Name = "lblJenkinsServers";
			this.lblJenkinsServers.Size = new System.Drawing.Size(94, 31);
			this.lblJenkinsServers.TabIndex = 1;
			this.lblJenkinsServers.Text = "Jenkins Servers:";
			this.lblJenkinsServers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cboJenkinsServers
			// 
			this.cboJenkinsServers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboJenkinsServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboJenkinsServers.Location = new System.Drawing.Point(103, 3);
			this.cboJenkinsServers.Name = "cboJenkinsServers";
			this.cboJenkinsServers.Size = new System.Drawing.Size(480, 21);
			this.cboJenkinsServers.TabIndex = 1;
			// 
			// btnEditJenkinsServer
			// 
			this.btnEditJenkinsServer.Location = new System.Drawing.Point(589, 3);
			this.btnEditJenkinsServer.Name = "btnEditJenkinsServer";
			this.btnEditJenkinsServer.Size = new System.Drawing.Size(79, 25);
			this.btnEditJenkinsServer.TabIndex = 4;
			this.btnEditJenkinsServer.Text = "Edit";
			this.btnEditJenkinsServer.UseVisualStyleBackColor = true;
			// 
			// tplJenkinsServer
			// 
			this.tplJenkinsServer.ColumnCount = 2;
			this.tplForm.SetColumnSpan(this.tplJenkinsServer, 2);
			this.tplJenkinsServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tplJenkinsServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplJenkinsServer.Controls.Add(this.lblJenkinsUrl, 0, 0);
			this.tplJenkinsServer.Controls.Add(this.txtJenkinsUrl, 1, 0);
			this.tplJenkinsServer.Controls.Add(this.lblUserName, 0, 1);
			this.tplJenkinsServer.Controls.Add(this.txtUserName, 1, 1);
			this.tplJenkinsServer.Controls.Add(this.lblApiToken, 0, 2);
			this.tplJenkinsServer.Controls.Add(this.txtApiToken, 1, 2);
			this.tplJenkinsServer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplJenkinsServer.Location = new System.Drawing.Point(0, 31);
			this.tplJenkinsServer.Margin = new System.Windows.Forms.Padding(0);
			this.tplJenkinsServer.Name = "tplJenkinsServer";
			this.tplJenkinsServer.RowCount = 3;
			this.tplJenkinsServer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tplJenkinsServer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tplJenkinsServer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tplJenkinsServer.Size = new System.Drawing.Size(671, 93);
			this.tplJenkinsServer.TabIndex = 5;
			// 
			// lblJenkinsUrl
			// 
			this.lblJenkinsUrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblJenkinsUrl.Location = new System.Drawing.Point(3, 0);
			this.lblJenkinsUrl.Name = "lblJenkinsUrl";
			this.lblJenkinsUrl.Size = new System.Drawing.Size(94, 31);
			this.lblJenkinsUrl.TabIndex = 1;
			this.lblJenkinsUrl.Text = "Jenkins Url:";
			this.lblJenkinsUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtJenkinsUrl
			// 
			this.txtJenkinsUrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtJenkinsUrl.Location = new System.Drawing.Point(103, 3);
			this.txtJenkinsUrl.Name = "txtJenkinsUrl";
			this.txtJenkinsUrl.Size = new System.Drawing.Size(565, 20);
			this.txtJenkinsUrl.TabIndex = 1;
			// 
			// lblUserName
			// 
			this.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblUserName.Location = new System.Drawing.Point(3, 31);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(94, 31);
			this.lblUserName.TabIndex = 2;
			this.lblUserName.Text = "UserName:";
			this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtUserName
			// 
			this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtUserName.Location = new System.Drawing.Point(103, 34);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(565, 20);
			this.txtUserName.TabIndex = 3;
			// 
			// lblApiToken
			// 
			this.lblApiToken.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblApiToken.Location = new System.Drawing.Point(3, 62);
			this.lblApiToken.Name = "lblApiToken";
			this.lblApiToken.Size = new System.Drawing.Size(94, 31);
			this.lblApiToken.TabIndex = 3;
			this.lblApiToken.Text = "Api Token:";
			this.lblApiToken.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtApiToken
			// 
			this.txtApiToken.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtApiToken.Location = new System.Drawing.Point(103, 65);
			this.txtApiToken.Name = "txtApiToken";
			this.txtApiToken.Size = new System.Drawing.Size(565, 20);
			this.txtApiToken.TabIndex = 4;
			// 
			// flpButtons
			// 
			this.flpButtons.Controls.Add(this.btnOK);
			this.flpButtons.Controls.Add(this.btnCancel);
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 137);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(665, 29);
			this.flpButtons.TabIndex = 0;
			this.flpButtons.WrapContents = false;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(583, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(79, 25);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(498, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(79, 25);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// PickJenkinsServerForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(671, 194);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(687, 210);
			this.Name = "PickJenkinsServerForm";
			this.Text = "Pick Jenkins Server";
			this.tplForm.ResumeLayout(false);
			this.tplJenkinsServers.ResumeLayout(false);
			this.tplJenkinsServer.ResumeLayout(false);
			this.tplJenkinsServer.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;

		private System.Windows.Forms.TableLayoutPanel tplJenkinsServers;
		private System.Windows.Forms.Label lblJenkinsServers;
		private System.Windows.Forms.ComboBox cboJenkinsServers;
		private System.Windows.Forms.Button btnEditJenkinsServer;

		private System.Windows.Forms.TableLayoutPanel tplJenkinsServer;
		private System.Windows.Forms.Label lblJenkinsUrl;
		private System.Windows.Forms.TextBox txtJenkinsUrl;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblApiToken;
		private System.Windows.Forms.TextBox txtApiToken;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}