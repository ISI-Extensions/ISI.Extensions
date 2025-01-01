#region Copyright & License
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
 
namespace ISI.Extensions.Jenkins.Forms
{
	partial class PushJenkinsConfigToJenkinsForm
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
			this.tplJenkinsServers = new System.Windows.Forms.TableLayoutPanel();
			this.lblJenkinsServers = new System.Windows.Forms.Label();
			this.cboJenkinsServers = new System.Windows.Forms.ComboBox();
			this.btnEditJenkinsServers = new System.Windows.Forms.Button();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnDone = new System.Windows.Forms.Button();
			this.btnPush = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tplForm.SuspendLayout();
			this.tplJenkinsServers.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 1;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.flpJenkinsConfigs, 0, 0);
			this.tplForm.Controls.Add(this.tplJenkinsServers, 0, 2);
			this.tplForm.Controls.Add(this.flpButtons, 0, 4);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 5;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 120F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tplForm.Size = new System.Drawing.Size(671, 421);
			this.tplForm.TabIndex = 0;
			// 
			// flpJenkinsConfigs
			// 
			this.flpJenkinsConfigs.AutoScroll = true;
			this.flpJenkinsConfigs.BackColor = System.Drawing.SystemColors.Window;
			this.flpJenkinsConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpJenkinsConfigs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpJenkinsConfigs.Location = new System.Drawing.Point(3, 3);
			this.flpJenkinsConfigs.Name = "flpJenkinsConfigs";
			this.flpJenkinsConfigs.Size = new System.Drawing.Size(665, 324);
			this.flpJenkinsConfigs.TabIndex = 0;
			this.flpJenkinsConfigs.WrapContents = false;
			// 
			// tplJenkinsServers
			// 
			this.tplJenkinsServers.ColumnCount = 3;
			this.tplJenkinsServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tplJenkinsServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplJenkinsServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.tplJenkinsServers.Controls.Add(this.lblJenkinsServers, 0, 0);
			this.tplJenkinsServers.Controls.Add(this.cboJenkinsServers, 1, 0);
			this.tplJenkinsServers.Controls.Add(this.btnEditJenkinsServers, 2, 0);
			this.tplJenkinsServers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplJenkinsServers.Location = new System.Drawing.Point(0, 340);
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
			// btnEditJenkinsServers
			// 
			this.btnEditJenkinsServers.Location = new System.Drawing.Point(589, 3);
			this.btnEditJenkinsServers.Name = "btnEditJenkinsServers";
			this.btnEditJenkinsServers.Size = new System.Drawing.Size(79, 25);
			this.btnEditJenkinsServers.TabIndex = 4;
			this.btnEditJenkinsServers.Text = "Edit";
			this.btnEditJenkinsServers.UseVisualStyleBackColor = true;
			// 
			// flpButtons
			// 
			this.flpButtons.Controls.Add(this.btnDone);
			this.flpButtons.Controls.Add(this.btnPush);
			this.flpButtons.Controls.Add(this.btnCancel);
			this.flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 384);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(665, 34);
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
			// btnPush
			// 
			this.btnPush.Location = new System.Drawing.Point(498, 3);
			this.btnPush.Name = "btnPush";
			this.btnPush.Size = new System.Drawing.Size(79, 25);
			this.btnPush.TabIndex = 4;
			this.btnPush.Text = "Push";
			this.btnPush.UseVisualStyleBackColor = true;
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
			// PushJenkinsConfigToJenkinsForm
			// 
			this.AcceptButton = this.btnPush;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(671, 421);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(687, 429);
			this.Name = "PushJenkinsConfigToJenkinsForm";
			this.Text = "Push Jenkins Config(s)";
			this.tplForm.ResumeLayout(false);
			this.tplJenkinsServers.ResumeLayout(false);
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;
		private System.Windows.Forms.FlowLayoutPanel flpJenkinsConfigs;

		private System.Windows.Forms.TableLayoutPanel tplJenkinsServers;
		private System.Windows.Forms.Label lblJenkinsServers;
		private System.Windows.Forms.ComboBox cboJenkinsServers;
		private System.Windows.Forms.Button btnEditJenkinsServers;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnPush;
		private System.Windows.Forms.Button btnDone;
	}
}