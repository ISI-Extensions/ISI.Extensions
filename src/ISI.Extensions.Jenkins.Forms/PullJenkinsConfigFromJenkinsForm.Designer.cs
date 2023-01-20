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
	partial class PullJenkinsConfigFromJenkinsForm
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
			this.flpJenkinsConfigs = new System.Windows.Forms.FlowLayoutPanel();
			this.tplJenkinsServer = new System.Windows.Forms.TableLayoutPanel();
			this.lblJenkinsUrl = new System.Windows.Forms.Label();
			this.txtJenkinsUrl = new System.Windows.Forms.TextBox();
			this.btnPickJenkinsServer = new System.Windows.Forms.Button();
			this.lblUserName = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblApiToken = new System.Windows.Forms.Label();
			this.txtApiToken = new System.Windows.Forms.TextBox();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnDone = new System.Windows.Forms.Button();
			this.btnPull = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tplForm.SuspendLayout();
			this.tplJenkinsServer.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 1;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.flpJenkinsConfigs, 0, 0);
			this.tplForm.Controls.Add(this.tplJenkinsServer, 0, 1);
			this.tplForm.Controls.Add(this.flpButtons, 0, 3);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 4;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.Size = new System.Drawing.Size(671, 413);
			this.tplForm.TabIndex = 0;
			// 
			// flpJenkinsConfigs
			// 
			this.flpJenkinsConfigs.AutoScroll = true;
			this.flpJenkinsConfigs.BackColor = System.Drawing.SystemColors.Window;
			this.flpJenkinsConfigs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flpJenkinsConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpJenkinsConfigs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpJenkinsConfigs.Location = new System.Drawing.Point(3, 3);
			this.flpJenkinsConfigs.MinimumSize = new System.Drawing.Size(665, 214);
			this.flpJenkinsConfigs.Name = "flpJenkinsConfigs";
			this.flpJenkinsConfigs.Size = new System.Drawing.Size(665, 269);
			this.flpJenkinsConfigs.TabIndex = 0;
			this.flpJenkinsConfigs.WrapContents = false;
			// 
			// tplJenkinsServer
			// 
			this.tplJenkinsServer.ColumnCount = 3;
			this.tplJenkinsServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tplJenkinsServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplJenkinsServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
			this.tplJenkinsServer.Controls.Add(this.lblJenkinsUrl, 0, 0);
			this.tplJenkinsServer.Controls.Add(this.txtJenkinsUrl, 1, 0);
			this.tplJenkinsServer.Controls.Add(this.btnPickJenkinsServer, 2, 0);
			this.tplJenkinsServer.Controls.Add(this.lblUserName, 0, 1);
			this.tplJenkinsServer.Controls.Add(this.txtUserName, 1, 1);
			this.tplJenkinsServer.Controls.Add(this.lblApiToken, 0, 2);
			this.tplJenkinsServer.Controls.Add(this.txtApiToken, 1, 2);
			this.tplJenkinsServer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplJenkinsServer.Location = new System.Drawing.Point(0, 275);
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
			this.txtJenkinsUrl.Enabled = false;
			this.txtJenkinsUrl.Location = new System.Drawing.Point(103, 3);
			this.txtJenkinsUrl.Name = "txtJenkinsUrl";
			this.txtJenkinsUrl.Size = new System.Drawing.Size(479, 20);
			this.txtJenkinsUrl.TabIndex = 1;
			// 
			// btnPickJenkinsServer
			// 
			this.btnPickJenkinsServer.Location = new System.Drawing.Point(588, 3);
			this.btnPickJenkinsServer.Name = "btnPickJenkinsServer";
			this.btnPickJenkinsServer.Size = new System.Drawing.Size(79, 25);
			this.btnPickJenkinsServer.TabIndex = 4;
			this.btnPickJenkinsServer.Text = "Pick";
			this.btnPickJenkinsServer.UseVisualStyleBackColor = true;
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
			this.txtUserName.Enabled = false;
			this.txtUserName.Location = new System.Drawing.Point(103, 34);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(479, 20);
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
			this.txtApiToken.Enabled = false;
			this.txtApiToken.Location = new System.Drawing.Point(103, 65);
			this.txtApiToken.Name = "txtApiToken";
			this.txtApiToken.Size = new System.Drawing.Size(479, 20);
			this.txtApiToken.TabIndex = 4;
			// 
			// flpButtons
			// 
			this.flpButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flpButtons.AutoSize = true;
			this.flpButtons.Controls.Add(this.btnDone);
			this.flpButtons.Controls.Add(this.btnPull);
			this.flpButtons.Controls.Add(this.btnCancel);
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 381);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(665, 29);
			this.flpButtons.TabIndex = 0;
			this.flpButtons.WrapContents = false;
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(583, 3);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(79, 25);
			this.btnDone.TabIndex = 4;
			this.btnDone.Text = "Done";
			this.btnDone.UseVisualStyleBackColor = true;
			// 
			// btnPull
			// 
			this.btnPull.Location = new System.Drawing.Point(498, 3);
			this.btnPull.Name = "btnPull";
			this.btnPull.Size = new System.Drawing.Size(79, 25);
			this.btnPull.TabIndex = 4;
			this.btnPull.Text = "Pull";
			this.btnPull.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(413, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(79, 25);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// PullJenkinsConfigFromJenkinsForm
			// 
			this.AcceptButton = this.btnPull;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(671, 413);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(687, 429);
			this.Name = "PullJenkinsConfigFromJenkinsForm";
			this.Text = "Jenkins Config(s)";
			this.tplForm.ResumeLayout(false);
			this.tplForm.PerformLayout();
			this.tplJenkinsServer.ResumeLayout(false);
			this.tplJenkinsServer.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;
		private System.Windows.Forms.FlowLayoutPanel flpJenkinsConfigs;

		private System.Windows.Forms.Button btnPickJenkinsServer;

		private System.Windows.Forms.TableLayoutPanel tplJenkinsServer;
		private System.Windows.Forms.Label lblJenkinsUrl;
		private System.Windows.Forms.TextBox txtJenkinsUrl;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblApiToken;
		private System.Windows.Forms.TextBox txtApiToken;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnDone;
		private System.Windows.Forms.Button btnPull;
		private System.Windows.Forms.Button btnCancel;
	}
}