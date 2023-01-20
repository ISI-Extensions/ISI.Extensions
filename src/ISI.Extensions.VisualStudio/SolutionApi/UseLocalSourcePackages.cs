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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.UseLocalSourcePackagesResponse UseLocalSourcePackages(DTOs.UseLocalSourcePackagesRequest request)
		{
			var response = new DTOs.UseLocalSourcePackagesResponse();

			request.Progress ??= (project, index, count) => Logger.LogInformation(project);
			request.AddProject ??= _ => { };

			var solutionFullName = GetClosestSolutionFullName(new DTOs.GetClosestSolutionFullNameRequest()
			{
				FileName = request.SolutionItem,
			}).ClosestSolutionFullName;

			if (string.IsNullOrWhiteSpace(solutionFullName))
			{
				throw new Exception(string.Format("Solution not found for \"{0}\"", request.SolutionItem));
			}

			var projectDetailsSet = new List<ProjectDetails>();

			if (solutionFullName.StartsWith(request.SolutionItem, StringComparison.InvariantCultureIgnoreCase))
			{
				var solutionDetails = GetSolutionDetails(new DTOs.GetSolutionDetailsRequest()
				{
					Solution = solutionFullName,
				}).SolutionDetails;

				projectDetailsSet.AddRange(solutionDetails.ProjectDetailsSet);
			}
			else
			{
				var projectFullName = ProjectApi.GetClosestProjectFullName(new ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi.GetClosestProjectFullNameRequest()
				{
					FileName = request.SolutionItem,
				}).ClosestProjectFullName;

				if (!string.IsNullOrWhiteSpace(projectFullName))
				{
					projectDetailsSet.Add(ProjectApi.GetProjectDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi.GetProjectDetailsRequest()
					{
						Project = projectFullName,
					}).ProjectDetails);
				}
			}

			if (projectDetailsSet.Any())
			{
				var localProjectDetailsSet = GetLocalProjectDetails(new DTOs.GetLocalProjectDetailsRequest()
				{
					LocalDirectory = System.IO.Path.GetDirectoryName(solutionFullName),
				}).ProjectDetailsSet;

				var localProjects = localProjectDetailsSet.ToDictionary(projectDetails => projectDetails.ProjectName, projectDetails => projectDetails, StringComparer.InvariantCultureIgnoreCase);

				bool tryGetLocalProject(string projectName, out string projectFullName)
				{
					if (localProjects.TryGetValue(projectName, out var projectDetails))
					{
						projectFullName = projectDetails.ProjectFullName;
						return true;
					}

					projectFullName = null;
					return false;
				}

				var addedLocalProjectFullNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

				void addLocalProject(string projectFullName)
				{
					if (!addedLocalProjectFullNames.Contains(projectFullName))
					{
						request.AddProject(projectFullName);
						addedLocalProjectFullNames.Add(projectFullName);
					}
				}

				var sortedProjectDetailsSet = projectDetailsSet.OrderBy(projectDetails => projectDetails.ProjectName, StringComparer.InvariantCultureIgnoreCase).ToArray();

				for (var projectDetailsIndex = 0; projectDetailsIndex < sortedProjectDetailsSet.Length; projectDetailsIndex++)
				{
					var projectDetails = sortedProjectDetailsSet[projectDetailsIndex];

					request.Progress(projectDetails.ProjectName, projectDetailsIndex, sortedProjectDetailsSet.Length);

					NugetApi.UseLocalSource(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UseLocalSourceRequest()
					{
						ProjectFullName = projectDetails.ProjectFullName,
						TryGetLocalProject = tryGetLocalProject,
						AddLocalProjectToSolution = addLocalProject,
					});
				}
			}

			return response;
		}
	}
}