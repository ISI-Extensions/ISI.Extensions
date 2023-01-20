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
	partial class EditJenkinsServerForm
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
			this.lblJenkinsUrl = new System.Windows.Forms.Label();
			this.txtJenkinsUrl = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblUserName = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblApiToken = new System.Windows.Forms.Label();
			this.txtApiToken = new System.Windows.Forms.TextBox();
			this.flpDirectories = new System.Windows.Forms.FlowLayoutPanel();
			this.flpAddDirectory = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAddDirectory = new System.Windows.Forms.Button();
			this.lblAddDirectoryDescription = new System.Windows.Forms.Label();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.folderBrowserDialogAddDirectory = new System.Windows.Forms.FolderBrowserDialog();
			this.tplForm.SuspendLayout();
			this.flpDirectories.SuspendLayout();
			this.flpAddDirectory.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 2;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.lblJenkinsUrl, 0, 0);
			this.tplForm.Controls.Add(this.txtJenkinsUrl, 1, 0);
			this.tplForm.Controls.Add(this.lblDescription, 0, 1);
			this.tplForm.Controls.Add(this.txtDescription, 1, 1);
			this.tplForm.Controls.Add(this.lblUserName, 0, 2);
			this.tplForm.Controls.Add(this.txtUserName, 1, 2);
			this.tplForm.Controls.Add(this.lblApiToken, 0, 3);
			this.tplForm.Controls.Add(this.txtApiToken, 1, 3);
			this.tplForm.Controls.Add(this.flpDirectories, 0, 5);
			this.tplForm.Controls.Add(this.flpButtons, 0, 7);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 8;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.Size = new System.Drawing.Size(503, 275);
			this.tplForm.TabIndex = 0;
			// 
			// lblJenkinsUrl
			// 
			this.lblJenkinsUrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblJenkinsUrl.Location = new System.Drawing.Point(3, 0);
			this.lblJenkinsUrl.Name = "lblJenkinsUrl";
			this.lblJenkinsUrl.Size = new System.Drawing.Size(94, 25);
			this.lblJenkinsUrl.TabIndex = 1;
			this.lblJenkinsUrl.Text = "Jenkins Url:";
			this.lblJenkinsUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtJenkinsUrl
			// 
			this.txtJenkinsUrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtJenkinsUrl.Location = new System.Drawing.Point(103, 3);
			this.txtJenkinsUrl.Name = "txtJenkinsUrl";
			this.txtJenkinsUrl.Size = new System.Drawing.Size(397, 20);
			this.txtJenkinsUrl.TabIndex = 1;
			// 
			// lblDescription
			// 
			this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDescription.Location = new System.Drawing.Point(3, 25);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(94, 25);
			this.lblDescription.TabIndex = 1;
			this.lblDescription.Text = "Description:";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtDescription
			// 
			this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDescription.Location = new System.Drawing.Point(103, 28);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(397, 20);
			this.txtDescription.TabIndex = 2;
			// 
			// lblUserName
			// 
			this.lblUserName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblUserName.Location = new System.Drawing.Point(3, 50);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(94, 25);
			this.lblUserName.TabIndex = 2;
			this.lblUserName.Text = "UserName:";
			this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtUserName
			// 
			this.txtUserName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtUserName.Location = new System.Drawing.Point(103, 53);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(397, 20);
			this.txtUserName.TabIndex = 3;
			// 
			// lblApiToken
			// 
			this.lblApiToken.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblApiToken.Location = new System.Drawing.Point(3, 75);
			this.lblApiToken.Name = "lblApiToken";
			this.lblApiToken.Size = new System.Drawing.Size(94, 25);
			this.lblApiToken.TabIndex = 3;
			this.lblApiToken.Text = "Api Token:";
			this.lblApiToken.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtApiToken
			// 
			this.txtApiToken.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtApiToken.Location = new System.Drawing.Point(103, 78);
			this.txtApiToken.Name = "txtApiToken";
			this.txtApiToken.Size = new System.Drawing.Size(397, 20);
			this.txtApiToken.TabIndex = 4;
			// 
			// flpDirectories
			// 
			this.flpDirectories.AutoScroll = true;
			this.flpDirectories.AutoSize = true;
			this.flpDirectories.BackColor = System.Drawing.SystemColors.Window;
			this.flpDirectories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tplForm.SetColumnSpan(this.flpDirectories, 2);
			this.flpDirectories.Controls.Add(this.flpAddDirectory);
			this.flpDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpDirectories.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpDirectories.Location = new System.Drawing.Point(3, 113);
			this.flpDirectories.Name = "flpDirectories";
			this.flpDirectories.Size = new System.Drawing.Size(497, 114);
			this.flpDirectories.TabIndex = 0;
			this.flpDirectories.WrapContents = false;
			// 
			// flpAddDirectory
			// 
			this.flpAddDirectory.AutoSize = true;
			this.flpAddDirectory.Controls.Add(this.btnAddDirectory);
			this.flpAddDirectory.Controls.Add(this.lblAddDirectoryDescription);
			this.flpAddDirectory.Location = new System.Drawing.Point(3, 3);
			this.flpAddDirectory.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.flpAddDirectory.Name = "flpAddDirectory";
			this.flpAddDirectory.Size = new System.Drawing.Size(384, 22);
			this.flpAddDirectory.TabIndex = 0;
			// 
			// btnAddDirectory
			// 
			this.btnAddDirectory.Location = new System.Drawing.Point(1, 1);
			this.btnAddDirectory.Margin = new System.Windows.Forms.Padding(1);
			this.btnAddDirectory.Name = "btnAddDirectory";
			this.btnAddDirectory.Size = new System.Drawing.Size(80, 20);
			this.btnAddDirectory.TabIndex = 1;
			this.btnAddDirectory.TabStop = false;
			this.btnAddDirectory.Text = "Add";
			this.btnAddDirectory.UseVisualStyleBackColor = true;
			// 
			// lblAddDirectoryDescription
			// 
			this.lblAddDirectoryDescription.AutoSize = true;
			this.lblAddDirectoryDescription.Location = new System.Drawing.Point(83, 1);
			this.lblAddDirectoryDescription.Margin = new System.Windows.Forms.Padding(1);
			this.lblAddDirectoryDescription.MinimumSize = new System.Drawing.Size(300, 17);
			this.lblAddDirectoryDescription.Name = "lblAddDirectoryDescription";
			this.lblAddDirectoryDescription.Size = new System.Drawing.Size(300, 17);
			this.lblAddDirectoryDescription.TabIndex = 0;
			// 
			// flpButtons
			// 
			this.tplForm.SetColumnSpan(this.flpButtons, 2);
			this.flpButtons.Controls.Add(this.btnOK);
			this.flpButtons.Controls.Add(this.btnCancel);
			this.flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 243);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(497, 29);
			this.flpButtons.TabIndex = 0;
			this.flpButtons.WrapContents = false;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(415, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(79, 25);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(330, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(79, 25);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// EditJenkinsServerForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(503, 275);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(441, 291);
			this.Name = "EditJenkinsServerForm";
			this.Text = "Jenkins Server";
			this.tplForm.ResumeLayout(false);
			this.tplForm.PerformLayout();
			this.flpDirectories.ResumeLayout(false);
			this.flpDirectories.PerformLayout();
			this.flpAddDirectory.ResumeLayout(false);
			this.flpAddDirectory.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;

		private System.Windows.Forms.Label lblJenkinsUrl;
		private System.Windows.Forms.TextBox txtJenkinsUrl;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblApiToken;
		private System.Windows.Forms.TextBox txtApiToken;

		private System.Windows.Forms.FlowLayoutPanel flpDirectories;

		private System.Windows.Forms.FlowLayoutPanel flpAddDirectory;
		private System.Windows.Forms.Label lblAddDirectoryDescription;
		private System.Windows.Forms.Button btnAddDirectory;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogAddDirectory;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}