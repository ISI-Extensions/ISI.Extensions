﻿#region Copyright & License
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
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.VisualStudioCode.Forms
{
	public partial class SolutionOptionsForm : Form
	{
		public bool ShowCleanSolutionCheckBox { get; set; } = true;
		public bool ShowUpdateSolutionCheckBox { get; set; } = true;
		public bool ShowCommitSolutionCheckBox { get; set; } = false;
		public bool ShowUpgradeNodeModulesCheckBox { get; set; } = false;
		public bool ShowInstallNodeModulesCheckBox { get; set; } = true;

		public bool CleanSolution => cboCleanSolution.Checked;
		public bool UpdateSolution => cboUpdateSolution.Checked;
		public bool CommitSolution => cboCommitSolution.Checked;
		public bool UpgradeNodeModules => cboUpgradeNodeModules.Checked;
		public bool InstallNodeModules => cboInstallNodeModules.Checked;

		public SolutionOptionsForm(bool cleanSolution, bool updateSolution, bool upgradeNodeModules, bool commitSolution, bool installNodeModules)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			cboCleanSolution.Checked = cleanSolution;
			cboUpdateSolution.Checked = updateSolution;
			cboCommitSolution.Checked = upgradeNodeModules && commitSolution;
			cboUpgradeNodeModules.Checked = upgradeNodeModules;
			cboInstallNodeModules.Checked = installNodeModules;

			btnOK.Click += (clickSender, clickEventArgs) =>
			{
				if (this.Modal)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				else
				{
					this.Close();
				}
			};

			btnCancel.Click += (clickSender, clickEventArgs) =>
			{
				if (!this.Modal)
				{
					this.Close();
				}
			};

			Shown += (shownSender, shownArgs) =>
			{
				cboCleanSolution.Visible = ShowCleanSolutionCheckBox;
				cboUpdateSolution.Visible = ShowUpdateSolutionCheckBox;
				cboInstallNodeModules.Visible = ShowInstallNodeModulesCheckBox;
				cboCommitSolution.Visible = ShowUpgradeNodeModulesCheckBox && ShowCommitSolutionCheckBox;
				cboUpgradeNodeModules.Visible = ShowUpgradeNodeModulesCheckBox;
			};
		}
	}
}
