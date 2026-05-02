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
 
namespace ISI.Extensions.WinForms
{
	partial class LogForm
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
			this.flpLogs = new System.Windows.Forms.FlowLayoutPanel();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.btnDone = new System.Windows.Forms.Button();
			this.tplForm.SuspendLayout();
			this.flpLogs.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 1;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.flpLogs, 0, 0);
			this.tplForm.Controls.Add(this.flpButtons, 0, 2);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 3;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tplForm.Size = new System.Drawing.Size(603, 575);
			this.tplForm.TabIndex = 0;
			// 
			// flpRepositoryHosts
			// 
			this.flpLogs.AutoScroll = true;
			this.flpLogs.AutoSize = true;
			this.flpLogs.BackColor = System.Drawing.SystemColors.Window;
			this.flpLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flpLogs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpLogs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpLogs.Location = new System.Drawing.Point(3, 3);
			this.flpLogs.Name = "flpLogs";
			this.flpLogs.Size = new System.Drawing.Size(497, 224);
			this.flpLogs.TabIndex = 0;
			this.flpLogs.WrapContents = false;
			// 
			// flpButtons
			// 
			this.flpButtons.Controls.Add(this.btnDone);
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
			this.btnDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnDone.Location = new System.Drawing.Point(415, 3);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(79, 25);
			this.btnDone.TabIndex = 3;
			this.btnDone.Text = "Done";
			this.btnDone.UseVisualStyleBackColor = true;
			// 
			// LogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnDone;
			this.ClientSize = new System.Drawing.Size(603, 575);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(541, 591);
			this.Name = "LogForm";
			this.Text = "Log Form";
			this.tplForm.ResumeLayout(false);
			this.tplForm.PerformLayout();
			this.flpLogs.ResumeLayout(false);
			this.flpLogs.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;
		private System.Windows.Forms.FlowLayoutPanel flpLogs;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.Button btnDone;
	}
}