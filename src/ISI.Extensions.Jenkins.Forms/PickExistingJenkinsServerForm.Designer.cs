#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
	partial class PickExistingJenkinsServerForm
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
			this.flpJenkinsServers = new System.Windows.Forms.FlowLayoutPanel();
			this.flpAddJenkinsServer = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAddJenkinsServer = new System.Windows.Forms.Button();
			this.lblAddJenkinsServerDescription = new System.Windows.Forms.Label();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tplForm.SuspendLayout();
			this.flpJenkinsServers.SuspendLayout();
			this.flpAddJenkinsServer.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 1;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.flpJenkinsServers, 0, 0);
			this.tplForm.Controls.Add(this.flpButtons, 0, 2);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 3;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tplForm.Size = new System.Drawing.Size(503, 275);
			this.tplForm.TabIndex = 0;
			// 
			// flpJenkinsServers
			// 
			this.flpJenkinsServers.AutoScroll = true;
			this.flpJenkinsServers.AutoSize = true;
			this.flpJenkinsServers.BackColor = System.Drawing.SystemColors.Window;
			this.flpJenkinsServers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flpJenkinsServers.Controls.Add(this.flpAddJenkinsServer);
			this.flpJenkinsServers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpJenkinsServers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpJenkinsServers.Location = new System.Drawing.Point(3, 3);
			this.flpJenkinsServers.Name = "flpJenkinsServers";
			this.flpJenkinsServers.Size = new System.Drawing.Size(497, 224);
			this.flpJenkinsServers.TabIndex = 0;
			this.flpJenkinsServers.WrapContents = false;
			// 
			// flpAddJenkinsServer
			// 
			this.flpAddJenkinsServer.AutoSize = true;
			this.flpAddJenkinsServer.Controls.Add(this.btnAddJenkinsServer);
			this.flpAddJenkinsServer.Controls.Add(this.lblAddJenkinsServerDescription);
			this.flpAddJenkinsServer.Location = new System.Drawing.Point(3, 3);
			this.flpAddJenkinsServer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.flpAddJenkinsServer.Name = "flpAddJenkinsServer";
			this.flpAddJenkinsServer.Size = new System.Drawing.Size(384, 22);
			this.flpAddJenkinsServer.TabIndex = 0;
			// 
			// btnAddJenkinsServer
			// 
			this.btnAddJenkinsServer.Location = new System.Drawing.Point(1, 1);
			this.btnAddJenkinsServer.Margin = new System.Windows.Forms.Padding(1);
			this.btnAddJenkinsServer.Name = "btnAddJenkinsServer";
			this.btnAddJenkinsServer.Size = new System.Drawing.Size(80, 20);
			this.btnAddJenkinsServer.TabIndex = 1;
			this.btnAddJenkinsServer.Text = "Add";
			this.btnAddJenkinsServer.UseVisualStyleBackColor = true;
			// 
			// lblAddJenkinsServerDescription
			// 
			this.lblAddJenkinsServerDescription.AutoSize = true;
			this.lblAddJenkinsServerDescription.Location = new System.Drawing.Point(83, 1);
			this.lblAddJenkinsServerDescription.Margin = new System.Windows.Forms.Padding(1);
			this.lblAddJenkinsServerDescription.MinimumSize = new System.Drawing.Size(300, 17);
			this.lblAddJenkinsServerDescription.Name = "lblAddJenkinsServerDescription";
			this.lblAddJenkinsServerDescription.Size = new System.Drawing.Size(300, 17);
			this.lblAddJenkinsServerDescription.TabIndex = 0;
			// 
			// flpButtons
			// 
			this.flpButtons.Controls.Add(this.btnCancel);
			this.flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 243);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(497, 29);
			this.flpButtons.TabIndex = 0;
			this.flpButtons.WrapContents = false;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(415, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(79, 25);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// PickJenkinsServerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(503, 275);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(441, 291);
			this.Name = "PickJenkinsServerForm";
			this.Text = "Jenkins Servers";
			this.tplForm.ResumeLayout(false);
			this.tplForm.PerformLayout();
			this.flpJenkinsServers.ResumeLayout(false);
			this.flpJenkinsServers.PerformLayout();
			this.flpAddJenkinsServer.ResumeLayout(false);
			this.flpAddJenkinsServer.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;
		private System.Windows.Forms.FlowLayoutPanel flpJenkinsServers;

		private System.Windows.Forms.FlowLayoutPanel flpAddJenkinsServer;
		private System.Windows.Forms.Label lblAddJenkinsServerDescription;
		private System.Windows.Forms.Button btnAddJenkinsServer;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnCancel;
	}
}