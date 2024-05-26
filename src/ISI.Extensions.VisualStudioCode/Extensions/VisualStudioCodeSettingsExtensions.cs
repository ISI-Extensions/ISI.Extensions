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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.VisualStudioCode.Extensions
{
	public static class VisualStudioCodeSettingsExtensions
	{
		public static int GetMaxCheckDirectoryDepth(this SolutionApi solutionApi)
		{
			return solutionApi.GetVisualStudioCodeSettings(new())?.VisualStudioCodeSettings?.MaxCheckDirectoryDepth ?? 5;
		}

		public static string[] GetDefaultExcludePathFilters(this SolutionApi solutionApi)
		{
			return solutionApi.GetVisualStudioCodeSettings(new())?.VisualStudioCodeSettings?.DefaultExcludePathFilters ?? [];
		}

		public static string[] GetPreviouslySelectedSolutionFilterKeys(this SolutionApi solutionApi)
		{
			return solutionApi.GetVisualStudioCodeSettings(new())?.VisualStudioCodeSettings?.PreviouslySelectedSolutionFilterKeys ?? [];
		}

		public static string[] GetRefreshSolutionsExcludePathFilters(this SolutionApi solutionApi)
		{
			return solutionApi.GetVisualStudioCodeSettings(new())?.VisualStudioCodeSettings?.RefreshSolutionsExcludePathFilters ?? [];
		}

		public static string[] GetRefreshSolutionsPreviouslySelectedSolutions(this SolutionApi solutionApi)
		{
			return solutionApi.GetVisualStudioCodeSettings(new())?.VisualStudioCodeSettings?.RefreshSolutionsPreviouslySelectedSolutions ?? [];
		}

		public static void UpdateRefreshSolutionsPreviouslySelectedSolutions(this SolutionApi solutionApi, IEnumerable<string> removeSolutions, IEnumerable<string> addSolutions)
		{
			solutionApi.UpdateVisualStudioCodeSettings(new()
			{
				UpdateSettings = settings =>
				{
					var solutions = new HashSet<string>((settings.RefreshSolutionsPreviouslySelectedSolutions ?? []), StringComparer.InvariantCultureIgnoreCase);

					if (removeSolutions != null)
					{
						foreach (var removeSolution in removeSolutions)
						{
							solutions.RemoveWhere(solution => string.Equals(solution, removeSolution, StringComparison.InvariantCultureIgnoreCase));
						}
					}

					if (addSolutions != null)
					{
						foreach (var addSolution in addSolutions)
						{
							solutions.Add(addSolution);
						}
					}

					if (!settings.RefreshSolutionsPreviouslySelectedSolutions.Equals(solutions, StringComparer.InvariantCultureIgnoreCase, true))
					{
						settings.RefreshSolutionsPreviouslySelectedSolutions = solutions.ToArray();

						return true;
					}

					return false;
				},
			});
		}


		public static void UpdateUpgradeNodeModulesPreviouslySelectedProjectKeys(this SolutionApi solutionApi, IEnumerable<string> removeProjectKeys, IEnumerable<string> addProjectKeys)
		{
			solutionApi.UpdateVisualStudioCodeSettings(new()
			{
				UpdateSettings = settings =>
				{
					var projectKeys = new HashSet<string>((settings.UpgradeNodeModulesPreviouslySelectedProjectKeys ?? []), StringComparer.InvariantCultureIgnoreCase);

					if (removeProjectKeys != null)
					{
						foreach (var removeProjectKey in removeProjectKeys)
						{
							projectKeys.RemoveWhere(projectKey => string.Equals(projectKey, removeProjectKey, StringComparison.InvariantCultureIgnoreCase));
						}
					}

					if (addProjectKeys != null)
					{
						foreach (var addProjectKey in addProjectKeys)
						{
							projectKeys.Add(addProjectKey);
						}
					}

					if (!settings.UpgradeNodeModulesPreviouslySelectedProjectKeys.Equals(projectKeys, StringComparer.InvariantCultureIgnoreCase, true))
					{
						settings.UpgradeNodeModulesPreviouslySelectedProjectKeys = projectKeys.ToArray();

						return true;
					}

					return false;
				},
			});
		}
	}
}
