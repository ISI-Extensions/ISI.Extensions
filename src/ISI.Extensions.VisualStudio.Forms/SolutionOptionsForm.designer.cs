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
 
namespace ISI.Extensions.VisualStudio.Forms
{
	partial class SolutionOptionsForm
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
			this.tlpForm = new System.Windows.Forms.TableLayoutPanel();
			this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.cboShowProjectExecutionInTaskbar = new System.Windows.Forms.CheckBox();
			this.cboExecuteProjects = new System.Windows.Forms.CheckBox();
			this.cboBuildSolution = new System.Windows.Forms.CheckBox();
			this.cboRestoreNugetPackages = new System.Windows.Forms.CheckBox();
			this.cboCommitSolution = new System.Windows.Forms.CheckBox();
			this.cboUpgradeNugetPackages = new System.Windows.Forms.CheckBox();
			this.cboUpdateSolution = new System.Windows.Forms.CheckBox();
			this.cboCleanSolution = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.tlpForm.SuspendLayout();
			this.flpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpForm
			// 
			this.tlpForm.ColumnCount = 4;
			this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 720F));
			this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.tlpForm.Controls.Add(this.flpButtons, 0, 0);
			this.tlpForm.Controls.Add(this.cboShowProjectExecutionInTaskbar, 1, 0);
			this.tlpForm.Controls.Add(this.btnCancel, 2, 0);
			this.tlpForm.Controls.Add(this.btnOK, 3, 0);
			this.tlpForm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpForm.Location = new System.Drawing.Point(0, 0);
			this.tlpForm.Name = "tlpForm";
			this.tlpForm.RowCount = 1;
			this.tlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tlpForm.Size = new System.Drawing.Size(704, 31);
			this.tlpForm.TabIndex = 0;
			// 
			// flpButtons
			// 
			this.flpButtons.AutoSize = true;
			this.flpButtons.Controls.Add(this.cboExecuteProjects);
			this.flpButtons.Controls.Add(this.cboBuildSolution);
			this.flpButtons.Controls.Add(this.cboRestoreNugetPackages);
			this.flpButtons.Controls.Add(this.cboCommitSolution);
			this.flpButtons.Controls.Add(this.cboUpgradeNugetPackages);
			this.flpButtons.Controls.Add(this.cboUpdateSolution);
			this.flpButtons.Controls.Add(this.cboCleanSolution);
			this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flpButtons.Location = new System.Drawing.Point(3, 3);
			this.flpButtons.Name = "flpButtons";
			this.flpButtons.Size = new System.Drawing.Size(544, 25);
			this.flpButtons.TabIndex = 0;
			this.flpButtons.WrapContents = false;
			// 
			// cboShowProjectExecutionInTaskbar
			// 
			this.cboShowProjectExecutionInTaskbar.AutoSize = true;
			this.cboShowProjectExecutionInTaskbar.Location = new System.Drawing.Point(723, 6);
			this.cboShowProjectExecutionInTaskbar.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboShowProjectExecutionInTaskbar.Name = "cboShowProjectExecutionInTaskbar";
			this.cboShowProjectExecutionInTaskbar.Size = new System.Drawing.Size(1, 17);
			this.cboShowProjectExecutionInTaskbar.TabIndex = 4;
			this.cboShowProjectExecutionInTaskbar.Text = "Show Execute Project In Taskbar";
			this.cboShowProjectExecutionInTaskbar.UseVisualStyleBackColor = true;
			// 
			// cboExecuteProjects
			// 
			this.cboExecuteProjects.AutoSize = true;
			this.cboExecuteProjects.Location = new System.Drawing.Point(495, 6);
			this.cboExecuteProjects.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboExecuteProjects.Name = "cboExecuteProjects";
			this.cboExecuteProjects.Size = new System.Drawing.Size(46, 17);
			this.cboExecuteProjects.TabIndex = 10;
			this.cboExecuteProjects.Text = "Run";
			this.cboExecuteProjects.UseVisualStyleBackColor = true;
			// 
			// cboBuildSolution
			// 
			this.cboBuildSolution.AutoSize = true;
			this.cboBuildSolution.Location = new System.Drawing.Point(440, 6);
			this.cboBuildSolution.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboBuildSolution.Name = "cboBuildSolution";
			this.cboBuildSolution.Size = new System.Drawing.Size(49, 17);
			this.cboBuildSolution.TabIndex = 9;
			this.cboBuildSolution.Text = "Build";
			this.cboBuildSolution.UseVisualStyleBackColor = true;
			// 
			// cboRestoreNugetPackages
			// 
			this.cboRestoreNugetPackages.AutoSize = true;
			this.cboRestoreNugetPackages.Location = new System.Drawing.Point(339, 6);
			this.cboRestoreNugetPackages.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboRestoreNugetPackages.Name = "cboRestoreNugetPackages";
			this.cboRestoreNugetPackages.Size = new System.Drawing.Size(95, 17);
			this.cboRestoreNugetPackages.TabIndex = 8;
			this.cboRestoreNugetPackages.Text = "Restore Nuget";
			this.cboRestoreNugetPackages.UseVisualStyleBackColor = true;
			// 
			// cboCommitSolution
			// 
			this.cboCommitSolution.AutoSize = true;
			this.cboCommitSolution.Location = new System.Drawing.Point(234, 6);
			this.cboCommitSolution.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboCommitSolution.Name = "cboCommitSolution";
			this.cboCommitSolution.Size = new System.Drawing.Size(99, 17);
			this.cboCommitSolution.TabIndex = 7;
			this.cboCommitSolution.Text = "Commit Solution";
			this.cboCommitSolution.UseVisualStyleBackColor = true;
			// 
			// cboUpgradeNugetPackages
			// 
			this.cboUpgradeNugetPackages.AutoSize = true;
			this.cboUpgradeNugetPackages.Location = new System.Drawing.Point(129, 6);
			this.cboUpgradeNugetPackages.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboUpgradeNugetPackages.Name = "cboUpgradeNugetPackages";
			this.cboUpgradeNugetPackages.Size = new System.Drawing.Size(99, 17);
			this.cboUpgradeNugetPackages.TabIndex = 6;
			this.cboUpgradeNugetPackages.Text = "Upgrade Nuget";
			this.cboUpgradeNugetPackages.UseVisualStyleBackColor = true;
			// 
			// cboUpdateSolution
			// 
			this.cboUpdateSolution.AutoSize = true;
			this.cboUpdateSolution.Location = new System.Drawing.Point(62, 6);
			this.cboUpdateSolution.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboUpdateSolution.Name = "cboUpdateSolution";
			this.cboUpdateSolution.Size = new System.Drawing.Size(61, 17);
			this.cboUpdateSolution.TabIndex = 5;
			this.cboUpdateSolution.Text = "Update";
			this.cboUpdateSolution.UseVisualStyleBackColor = true;
			// 
			// cboCleanSolution
			// 
			this.cboCleanSolution.AutoSize = true;
			this.cboCleanSolution.Location = new System.Drawing.Point(3, 6);
			this.cboCleanSolution.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.cboCleanSolution.Name = "cboCleanSolution";
			this.cboCleanSolution.Size = new System.Drawing.Size(53, 17);
			this.cboCleanSolution.TabIndex = 4;
			this.cboCleanSolution.Text = "Clean";
			this.cboCleanSolution.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnCancel.Location = new System.Drawing.Point(537, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(79, 25);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnOK.Location = new System.Drawing.Point(622, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(79, 25);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// SolutionOptionsForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(704, 31);
			this.ControlBox = false;
			this.Controls.Add(this.tlpForm);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(720, 70);
			this.MinimizeBox = false;
			this.Name = "SolutionOptionsForm";
			this.ShowInTaskbar = false;
			this.Text = "Solution Options";
			this.tlpForm.ResumeLayout(false);
			this.tlpForm.PerformLayout();
			this.flpButtons.ResumeLayout(false);
			this.flpButtons.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpForm;
		private System.Windows.Forms.FlowLayoutPanel flpButtons;
		private System.Windows.Forms.CheckBox cboCleanSolution;
		private System.Windows.Forms.CheckBox cboUpdateSolution;
		private System.Windows.Forms.CheckBox cboCommitSolution;
		private System.Windows.Forms.CheckBox cboUpgradeNugetPackages;
		private System.Windows.Forms.CheckBox cboRestoreNugetPackages;
		private System.Windows.Forms.CheckBox cboBuildSolution;
		private System.Windows.Forms.CheckBox cboExecuteProjects;
		private System.Windows.Forms.CheckBox cboShowProjectExecutionInTaskbar;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}