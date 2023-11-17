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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Nuget.Forms
{
	public class UpdateNugetPackages
	{
		private static ISI.Extensions.Nuget.NugetApi _nugetApi = null;
		protected static ISI.Extensions.Nuget.NugetApi NugetApi => _nugetApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.NugetApi>();

		private static ISI.Extensions.VisualStudio.VisualStudioSettings _visualStudioSettings = null;
		protected static ISI.Extensions.VisualStudio.VisualStudioSettings VisualStudioSettings => _visualStudioSettings ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.VisualStudioSettings>();

		public class SolutionsContext : SolutionsForm.ISolutionsContext
		{
			public IList<Solution> Solutions { get; } = new List<Solution>();
		}

		public static System.Windows.Forms.Form CreateForm(IEnumerable<string> selectedItemPaths, bool exitOnClose = false)
		{
			SolutionsForm.ISolutionsContext updateNugetPackages(SolutionsForm form, Action<Solution> start)
			{
				var context = new SolutionsContext();

				form.Text = "Update Nuget Packages";
				form.StartButton.Text = "Update";

				void OnChangedSelection()
				{
					form.StartButton.Enabled = context.Solutions.Any(solution => solution.Selected);
				}

				var excludedPathFilters = VisualStudioSettings.GetRefreshSolutionsExcludePathFilters();
				var solutionFileNames = new System.Collections.Concurrent.ConcurrentBag<string>();
				var maxCheckDirectoryDepth = VisualStudioSettings.GetMaxCheckDirectoryDepth() - 1;

				Parallel.ForEach(selectedItemPaths, selectedItemPath =>
				{
					if (System.IO.File.Exists(selectedItemPath))
					{
						if (selectedItemPath.EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase))
						{
							solutionFileNames.Add(selectedItemPath);
						}
					}
					else if (System.IO.Directory.Exists(selectedItemPath))
					{
						foreach (var solutionFullName in ISI.Extensions.IO.Path.EnumerateFiles(selectedItemPath, "*.sln", excludedPathFilters, maxCheckDirectoryDepth))
						{
							solutionFileNames.Add(solutionFullName);
						}
					}
				});

				var solutions = solutionFileNames.Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(solutionFullName => solutionFullName, StringComparer.InvariantCultureIgnoreCase).ToDictionary(solutionFileName => solutionFileName, solutionFileName => true);

				var selectAll = !solutions.Values.Any(value => value);

				var nugetPackageKeys = new NugetPackageKeyDictionary();
				foreach (var nugetSettingsNugetPackageKey in NugetApi.GetNugetSettings(new ())?.NugetSettings?.UpdateNugetPackages?.NugetSettingsNugetPackageKeys ?? Array.Empty<NugetSettingsNugetPackageKey>())
				{
					nugetPackageKeys.TryAdd(NugetApi.GetNugetPackageKey(new()
					{
						PackageId = nugetSettingsNugetPackageKey.PackageId,
						PackageVersion = nugetSettingsNugetPackageKey.PackageVersion,
					}).NugetPackageKey);
				}


				foreach (var solution in solutions)
				{
					context.Solutions.Add(new(solution.Key, form.SolutionsPanel, (context.Solutions.Count % 2 == 1), selectAll || solution.Value, start, OnChangedSelection, null, nugetPackageKeys));
				}

				form.SolutionsPanel.Controls.AddRange(context.Solutions.Select(solution => solution.Panel).ToArray());

				OnChangedSelection();

				return context;
			};

			void OnCloseForm(SolutionsForm form)
			{

			};

			return new ISI.Extensions.Nuget.Forms.SolutionsForm(updateNugetPackages, OnCloseForm)
			{
				ExitOnClose = exitOnClose,
			};
		}
	}
}