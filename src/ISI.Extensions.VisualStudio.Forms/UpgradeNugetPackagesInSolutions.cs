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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.VisualStudio.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.VisualStudio.Forms
{
	public class UpgradeNugetPackagesInSolutions
	{
		private static ISI.Extensions.VisualStudio.SolutionApi _solutionApi = null;
		protected static ISI.Extensions.VisualStudio.SolutionApi SolutionApi => _solutionApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

		public class SolutionsContext : SolutionsForm.ISolutionsContext
		{
			public IList<Solution> Solutions { get; set; } = new List<Solution>();
		}

		public static System.Windows.Forms.Form CreateForm(IEnumerable<string> selectedItemPaths, bool exitOnClose = false)
		{
			SolutionsForm.ISolutionsContext upgradeNugetPackagesInSolutions(SolutionsForm form, Action<Solution> start)
			{
				var context = new SolutionsContext();

				form.Text = "Upgrade Nuget Packages In Solution(s)";
				form.StartButton.Text = "Upgrade";

				void OnChangedSelection()
				{
					form.StartButton.Enabled = context.Solutions.Any(solution => solution.Selected);
				}

				var excludedPathFilters = SolutionApi.GetDefaultExcludePathFilters();
				var solutionFileNames = new System.Collections.Concurrent.ConcurrentBag<string>();
				var maxCheckDirectoryDepth = SolutionApi.GetMaxCheckDirectoryDepth() - 1;

				Parallel.ForEach(selectedItemPaths, selectedItemPath =>
				{
					if (System.IO.File.Exists(selectedItemPath))
					{
						if (ISI.Extensions.VisualStudio.Solution.IsSolutionFileName(selectedItemPath))
						{
							solutionFileNames.Add(selectedItemPath);
						}
					}
					else if (System.IO.Directory.Exists(selectedItemPath))
					{
						foreach (var solutionFullName in new HashSet<string>(ISI.Extensions.VisualStudio.Solution.EnumerateSolutionFiles(selectedItemPath, excludedPathFilters, maxCheckDirectoryDepth), StringComparer.InvariantCultureIgnoreCase))
						{
							solutionFileNames.Add(solutionFullName);
						}
					}
				});

				var solutions = solutionFileNames.Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(solutionFullName => solutionFullName, StringComparer.InvariantCultureIgnoreCase).ToDictionary(solutionFileName => solutionFileName, solutionFileName => true);

				var selectAll = !solutions.Values.Any(value => value);

				foreach (var solution in solutions)
				{
					context.Solutions.Add(new(solution.Key, form.SolutionsPanel, (context.Solutions.Count % 2 == 1), selectAll || solution.Value, start, null, true, false, OnChangedSelection, form.SetStatus, null));
				}

				form.SolutionsPanel.Controls.AddRange(context.Solutions.Select(solution => solution.Panel).ToArray());

				OnChangedSelection();

				return context;
			};

			void UpdatePreviouslySelectedSolutions(SolutionsForm form)
			{
			};

			void OnCloseForm(SolutionsForm form)
			{

			};

			return new ISI.Extensions.VisualStudio.Forms.SolutionsForm(upgradeNugetPackagesInSolutions, UpdatePreviouslySelectedSolutions, OnCloseForm)
			{
				UpgradeNugetPackages = true,
				ShowUpgradeNugetPackagesCheckBox = true,
				CommitSolution = true,
				RestoreNugetPackages = false,
				BuildSolution = false,
				ShowCommitSolutionCheckBox = true,
				ShowExecuteProjectsCheckBox = false,
				ShowShowProjectExecutionInTaskbarCheckBox = false,
				ExitOnClose = exitOnClose,
			};
		}
	}
}