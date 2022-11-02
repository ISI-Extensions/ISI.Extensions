#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.VisualStudio.Forms
{
	public class RunMigrationTools
	{
		private static ISI.Extensions.VisualStudio.VisualStudioSettings _visualStudioSettings = null;
		protected static ISI.Extensions.VisualStudio.VisualStudioSettings VisualStudioSettings => _visualStudioSettings ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.VisualStudioSettings>();
		
		private static ISI.Extensions.VisualStudio.SolutionApi _solutionApi = null;
		protected static ISI.Extensions.VisualStudio.SolutionApi SolutionApi => _solutionApi ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

		public class SolutionsContext : SolutionsForm.ISolutionsContext
		{
			public IList<Solution> Solutions { get; } = new List<Solution>();
		}

		public static System.Windows.Forms.Form CreateForm(IEnumerable<string> selectedItemPaths, bool exitOnClose = false)
		{
			SolutionsForm.ISolutionsContext BuildSolutions(SolutionsForm form, Action<Solution> start)
			{
				var context = new SolutionsContext();

				form.Text = "Run MigrationTool(s)";
				form.StartButton.Text = "Run";

				void OnChangedSelection()
				{
					form.StartButton.Enabled = context.Solutions.Any(solution => solution.Selected);
				}

				var excludedPathFilters = VisualStudioSettings.GetRunMigrationToolsExcludePathFilters();
				var projectFileNames = new System.Collections.Concurrent.ConcurrentBag<string>();
				var maxCheckDirectoryDepth = VisualStudioSettings.GetMaxCheckDirectoryDepth() - 1;

				Parallel.ForEach(selectedItemPaths, selectedItemPath =>
				{
					if (System.IO.File.Exists(selectedItemPath))
					{
						if (selectedItemPath.EndsWith(".MigrationTool.csproj", StringComparison.InvariantCultureIgnoreCase))
						{
							projectFileNames.Add(selectedItemPath);
						}
						else if (selectedItemPath.EndsWith(".MigrationTools.csproj", StringComparison.InvariantCultureIgnoreCase))
						{
							projectFileNames.Add(selectedItemPath);
						}
					}
					else if (System.IO.Directory.Exists(selectedItemPath))
					{
						foreach (var projectFileName in ISI.Extensions.IO.Path.EnumerateFiles(selectedItemPath, "*.csproj", excludedPathFilters, maxCheckDirectoryDepth))
						{
							if (projectFileName.EndsWith(".MigrationTool.csproj", StringComparison.InvariantCultureIgnoreCase) || projectFileName.EndsWith(".MigrationTools.csproj", StringComparison.InvariantCultureIgnoreCase))
							{
								projectFileNames.Add(projectFileName);
							}
						}
					}
				});

				var previouslySelectedProjectKeys = new HashSet<string>(VisualStudioSettings.GetRunMigrationToolsPreviouslySelectedProjectKeys(), StringComparer.InvariantCultureIgnoreCase);

				var projectKeys = projectFileNames.Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(projectFileName => projectFileName, StringComparer.InvariantCultureIgnoreCase).Select(projectFileName =>
				{
					var solutionFullName = SolutionApi.GetClosestSolutionFullName(new()
					{
						FileName = projectFileName,
					}).ClosestSolutionFullName;

					var projectKey = new ProjectKey(solutionFullName, projectFileName);

					projectKey.Selected = previouslySelectedProjectKeys.Contains(projectKey.Value);

					return projectKey;
				});

				var selectAll = !projectKeys.Any(projectKey => projectKey.Selected);

				foreach (var solutionGroupedProjectKeys in projectKeys.GroupBy(projectKey => projectKey.SolutionFullName, StringComparer.InvariantCultureIgnoreCase).OrderBy(solutionGroupedProjectKey => solutionGroupedProjectKey.Key, StringComparer.InvariantCultureIgnoreCase))
				{
					context.Solutions.Add(new(solutionGroupedProjectKeys.Key, form.SolutionsPanel, (context.Solutions.Count % 2 == 1), selectAll || solutionGroupedProjectKeys.Any(projectKey => projectKey.Selected), start, solutionGroupedProjectKeys, false, true, OnChangedSelection));
				}

				//form.SolutionsPanel.Controls.AddRange(context.Solutions.OrderBy(solution => solution.Caption, StringComparer.InvariantCultureIgnoreCase).Select(solution => solution.Panel).ToArray());
				form.SolutionsPanel.Controls.AddRange(context.Solutions.Select(solution => solution.Panel).ToArray());

				OnChangedSelection();

				return context;
			};

			void UpdatePreviouslySelectedSolutions(SolutionsForm form)
			{
				var removeProjectKeys = form.SolutionsContext.Solutions.SelectMany(solution => solution.SolutionProjects.Select(project => project.ProjectKey));
				var addProjectKeys = form.SolutionsContext.Solutions.Where(solution => solution.Selected).SelectMany(solution => solution.SolutionProjects.Where(projectKey => projectKey.Selected).Select(project => project.ProjectKey));

				VisualStudioSettings.UpdateRunMigrationToolsPreviouslySelectedProjectKeys(removeProjectKeys.Select(projectKey => projectKey.Value), addProjectKeys.Select(projectKey => projectKey.Value));
			};

			void OnCloseForm(SolutionsForm form)
			{

			};

			return new ISI.Extensions.VisualStudio.Forms.SolutionsForm(BuildSolutions, UpdatePreviouslySelectedSolutions, OnCloseForm)
			{
				ShowExecuteProjectsCheckBox = true,
				ShowShowProjectExecutionInTaskbarCheckBox = false,
				ExitOnClose = exitOnClose,
			};
		}
	}
}