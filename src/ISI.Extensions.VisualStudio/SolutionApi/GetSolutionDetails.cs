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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.GetSolutionDetailsResponse GetSolutionDetails(DTOs.GetSolutionDetailsRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.GetSolutionDetailsResponse();

			var solutionFullName = GetSolutionFullName(new()
			{
				Solution = request.Solution,
				ThrowErrorIfNoSolutionFound = false,
				AddToLog = request.AddToLog,
			}).SolutionFullName;

			if (!string.IsNullOrWhiteSpace(solutionFullName))
			{
				response.SolutionDetails = new()
				{
					SolutionFullName = solutionFullName,
				};

				response.SolutionDetails.SolutionName = System.IO.Path.GetFileNameWithoutExtension(response.SolutionDetails.SolutionFullName);
				response.SolutionDetails.SolutionDirectory = System.IO.Path.GetDirectoryName(response.SolutionDetails.SolutionFullName);
				response.SolutionDetails.RootSourceDirectory = SourceControlClientApi.GetRootDirectory(new()
				{
					FullName = response.SolutionDetails.SolutionDirectory,
				}).FullName;

				var solutionPreferences = GetSolutionPreferences(new()
				{
					SolutionDirectory = response.SolutionDetails.SolutionDirectory,
				}).SolutionPreferences;

				if (solutionPreferences != null)
				{
					response.SolutionDetails.UpgradeNugetPackagesPriority = solutionPreferences.UpgradeNugetPackagesPriority ?? int.MaxValue;
					response.SolutionDetails.ExecuteBuildScriptTargetAfterUpgradeNugetPackages = solutionPreferences.ExecuteBuildScriptTargetAfterUpgradeNugetPackages;
					response.SolutionDetails.DoNotUpgradePackages = solutionPreferences.DoNotUpgradePackages;
				}

				if (!string.IsNullOrWhiteSpace(response.SolutionDetails.SolutionFullName))
				{
					response.SolutionDetails.ProjectDetailsSet = ProjectFileNamesFromSolutionFullName(response.SolutionDetails.SolutionFullName, true)
						.ToNullCheckedArray(projectFullName => new ProjectDetails()
						{
							ProjectName = System.IO.Path.GetFileNameWithoutExtension(projectFullName),
							ProjectDirectory = System.IO.Path.GetDirectoryName(projectFullName),
							ProjectFullName = projectFullName,
						});

					response.SolutionDetails.UsePackagesDirectory = false;
					for (var projectIndex = 0; (!response.SolutionDetails.UsePackagesDirectory && (projectIndex < response.SolutionDetails.ProjectDetailsSet.Length)); projectIndex++)
					{
						if (System.IO.File.Exists(response.SolutionDetails.ProjectDetailsSet[projectIndex].ProjectFullName))
						{
							if (System.IO.File.ReadAllText(response.SolutionDetails.ProjectDetailsSet[projectIndex].ProjectFullName).IndexOf("<HintPath>", StringComparison.InvariantCultureIgnoreCase) >= 0)
							{
								response.SolutionDetails.UsePackagesDirectory = true;
							}
						}
					}

					var solutionFilterDetailsSet = new List<SolutionFilterDetails>();

					foreach (var solutionFilterFullName in System.IO.Directory.EnumerateFiles(response.SolutionDetails.SolutionDirectory, "*.slnf", System.IO.SearchOption.TopDirectoryOnly))
					{
						solutionFilterDetailsSet.Add(new()
						{
							SolutionFilterName = System.IO.Path.GetFileNameWithoutExtension(solutionFilterFullName),
							SolutionFilterDirectory = response.SolutionDetails.SolutionDirectory,
							SolutionFilterFullName = solutionFilterFullName,
						});
					}

					response.SolutionDetails.SolutionFilterDetailsSet = solutionFilterDetailsSet.ToArray();
				}
			}

			return response;
		}
	}
}