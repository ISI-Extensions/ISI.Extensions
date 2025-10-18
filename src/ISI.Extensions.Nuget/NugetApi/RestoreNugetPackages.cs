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
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.RestoreNugetPackagesResponse RestoreNugetPackages(DTOs.IRestoreNugetPackagesRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.RestoreNugetPackagesResponse();

			var usePackagesDirectory = (request as DTOs.RestoreNugetPackagesRequest)?.UsePackagesDirectory ?? (request as DTOs.RestoreNugetPackagesUsingSolutionDetailsRequest)?.SolutionDetails?.UsePackagesDirectory ?? false;

			var solutionFullName = (string)null;
			var solutionDirectory = (string)null;
			var targets = new List<string>();

			switch (request)
			{
				case DTOs.RestoreNugetPackagesRequest restoreNugetPackagesRequest:
					{
						if (!string.IsNullOrWhiteSpace(restoreNugetPackagesRequest.Solution) && System.IO.Directory.Exists(restoreNugetPackagesRequest.Solution))
						{
							var possibleSolutionFullNames = ISI.Extensions.VisualStudio.Solution.FindSolutionFullNames(restoreNugetPackagesRequest.Solution, System.IO.SearchOption.AllDirectories).ToArray();

							if (possibleSolutionFullNames.Length == 1)
							{
								restoreNugetPackagesRequest.Solution = possibleSolutionFullNames.First();
							}
							else if (possibleSolutionFullNames.Length > 1)
							{
								var possibleSolutionName = System.IO.Path.GetFileName(restoreNugetPackagesRequest.Solution);

								var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

								if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
								{
									restoreNugetPackagesRequest.Solution = possibleSolutionFullName;
								}
								else
								{
									throw new($"Cannot determine which solution to restore for \"{restoreNugetPackagesRequest.Solution}\"");
								}
							}
							else
							{
								throw new($"Cannot find a solution to restore for \"{restoreNugetPackagesRequest.Solution}\"");
							}
						}

						solutionFullName = restoreNugetPackagesRequest.Solution;
						solutionDirectory = System.IO.Path.GetDirectoryName(restoreNugetPackagesRequest.Solution);
						targets.Add(restoreNugetPackagesRequest.Solution);
					}
					break;

				case DTOs.RestoreNugetPackagesUsingSolutionDetailsRequest restoreNugetPackagesUsingSolutionDetailsRequest:
					solutionFullName = restoreNugetPackagesUsingSolutionDetailsRequest.SolutionDetails.SolutionFullName;
					solutionDirectory = restoreNugetPackagesUsingSolutionDetailsRequest.SolutionDetails.SolutionDirectory;
					targets.AddRange(restoreNugetPackagesUsingSolutionDetailsRequest.SolutionDetails.ProjectDetailsSet.Select(projectDetails => projectDetails.ProjectFullName));
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}

			logger.LogInformation($"Solution: {solutionFullName}");

			var nugetExeFullName = GetNugetExeFullName(new()).NugetExeFullName;

			response.Success = targets.Any();

			foreach (var target in targets)
			{
				if (solutionFullName.EndsWith(ISI.Extensions.VisualStudio.Solution.SolutionExtensionX, StringComparison.InvariantCultureIgnoreCase))
				{
					var arguments = new List<string>();
					arguments.Add("restore");
					arguments.Add($"\"{System.IO.Path.GetDirectoryName(target)}\"");

					response.Success &= !ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = "dotnet",
						Arguments = arguments,
					}).Errored;
				}
				else
				{
					var arguments = new List<string>();
					arguments.Add("restore");
					arguments.Add($"\"{target}\"");
					if (usePackagesDirectory)
					{
						arguments.Add($"-PackagesDirectory \"{System.IO.Path.Combine(solutionDirectory, "packages")}\"");
					}

					//arguments.Add("-NoHttpCache");
					arguments.Add("-NonInteractive");
					if (!string.IsNullOrWhiteSpace(request.MSBuildExe) && System.IO.File.Exists(request.MSBuildExe))
					{
						arguments.Add($"-MSBuildPath \"{System.IO.Path.GetDirectoryName(request.MSBuildExe)}\"");
					}

					response.Success &= !ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = nugetExeFullName,
						Arguments = arguments,
					}).Errored;
				}
			}

			return response;
		}
	}
}