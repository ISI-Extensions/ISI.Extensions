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
using ISI.Extensions.Nuget.Forms.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Nuget.Forms
{
	public partial class SelectNugetPackagesForm : Form
	{
		private static ISI.Extensions.Nuget.NugetApi _nugetApi = null;
		protected ISI.Extensions.Nuget.NugetApi NugetApi => _nugetApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.NugetApi>();

		public IList<NugetPackage> NugetPackages { get; } = new List<NugetPackage>();

		private static HashSet<string> _previousPackageIds { get; } = new(StringComparer.CurrentCultureIgnoreCase);
		private static HashSet<string> _previouslySelectedPackageIds { get; } = new(StringComparer.CurrentCultureIgnoreCase);

		public SelectNugetPackagesForm(IEnumerable<NugetPackageKey> nugetPackageKeys)
		{
			InitializeComponent();

			ISI.Extensions.WinForms.ThemeHelper.SyncTheme(this);

			NugetApi.ApplyFormSize(nameof(SelectNugetPackagesForm), this);

			CloseButton.Visible = false;
			OkButton.Visible = false;

			ForeColor = DefaultForeColor;

			Icon = new(ISI.Extensions.T4Resources.Artwork.GetLantern_icoStream());
			ControlBox = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ShowIcon = true;

			Shown += (shownSender, shownArgs) =>
			{
				var allSelected = !_previousPackageIds.Any();

				foreach (var nugetPackageKey in nugetPackageKeys)
				{
					NugetPackages.Add(new(nugetPackageKey, NugetPackagesPanel, (NugetPackages.Count % 2 == 1), allSelected || !_previousPackageIds.Contains(nugetPackageKey.Package) || _previouslySelectedPackageIds.Contains(nugetPackageKey.Package)));
				}

				NugetPackagesPanel.Resize += (resizeSender, resizeArgs) =>
				{
					var parentControl = (Control)resizeSender;

					foreach (var nugetPackage in NugetPackages)
					{
						if (nugetPackage.Panel.Size.Width != parentControl.Size.Width)
						{
							nugetPackage.Panel.Size = new(parentControl.Size.Width, nugetPackage.Panel.Size.Height);
						}
					}
				};

				OkButton.Visible = true;
				CloseButton.Visible = true;
			};

			OkButton.Click += (clickSender, clickEventArgs) =>
			{
				foreach (var nugetPackage in NugetPackages)
				{
					_previousPackageIds.Add(nugetPackage.NugetPackageKey.Package);

					if (nugetPackage.Selected)
					{
						_previouslySelectedPackageIds.Add(nugetPackage.NugetPackageKey.Package);
					}
					else
					{
						_previouslySelectedPackageIds.Remove(nugetPackage.NugetPackageKey.Package);
					}
				}

				NugetApi.RecordFormSize(this);

				if (this.Modal)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
				}
				else
				{
					this.Close();
				}
			};

			CloseButton.Click += (clickSender, clickEventArgs) =>
			{
				if (this.Modal)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				}
				else
				{
					this.Close();
				}
			};
		}
	}
}
