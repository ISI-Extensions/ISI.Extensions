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
 
namespace ISI.Extensions.Nuget.Forms
{
	public partial class SelectNugetPackagesForm
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
			this.NugetPackagesPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.CloseButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.tplForm.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tplForm
			// 
			this.tplForm.ColumnCount = 1;
			this.tplForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.Controls.Add(this.NugetPackagesPanel, 0, 0);
			this.tplForm.Controls.Add(this.flpButtons, 0, 1);
			this.tplForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tplForm.Location = new System.Drawing.Point(0, 0);
			this.tplForm.Name = "tplForm";
			this.tplForm.RowCount = 2;
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tplForm.Size = new System.Drawing.Size(671, 413);
			this.tplForm.TabIndex = 0;
			// 
			// NugetPackagesPanel
			// 
			this.NugetPackagesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.NugetPackagesPanel.AutoScroll = true;
			this.NugetPackagesPanel.BackColor = System.Drawing.SystemColors.Window;
			this.NugetPackagesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.NugetPackagesPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.NugetPackagesPanel.Location = new System.Drawing.Point(3, 3);
			this.NugetPackagesPanel.MinimumSize = new System.Drawing.Size(665, 214);
			this.NugetPackagesPanel.Name = "NugetPackagesPanel";
			this.NugetPackagesPanel.Size = new System.Drawing.Size(665, 372);
			this.NugetPackagesPanel.TabIndex = 0;
			this.NugetPackagesPanel.WrapContents = false;
			// 
			// flpButtons
			// 
			this.flpButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flpButtons.AutoSize = true;
			this.flpButtons.Controls.Add(this.CloseButton);
			this.flpButtons.Controls.Add(this.OkButton);
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 381);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(665, 29);
			this.flpButtons.TabIndex = 0;
			this.flpButtons.WrapContents = false;
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(583, 3);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(79, 25);
			this.CloseButton.TabIndex = 4;
			this.CloseButton.Text = "Cancel";
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(413, 3);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(79, 25);
			this.OkButton.TabIndex = 3;
			this.OkButton.Text = "Install";
			this.OkButton.UseVisualStyleBackColor = true;
			// 
			// NugetPackagesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.CancelButton;
			this.ClientSize = new System.Drawing.Size(671, 413);
			this.ControlBox = false;
			this.Controls.Add(this.tplForm);
			this.MinimumSize = new System.Drawing.Size(687, 429);
			this.Name = "NugetPackagesForm";
			this.Text = "Nuget Package(s)";
			this.tplForm.ResumeLayout(false);
			this.tplForm.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tplForm;
		protected internal System.Windows.Forms.FlowLayoutPanel NugetPackagesPanel;

		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		protected internal System.Windows.Forms.Button OkButton;
		protected internal System.Windows.Forms.Button CloseButton;
	}
}