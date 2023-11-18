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
using ISI.Extensions.JsonSerialization.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using SerializableDTOs = ISI.Extensions.VisualStudio.SerializableModels;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public const string SolutionDetailsFileName = ".solutionDetails.json";
		public const string SolutionPreferencesFileName = ".solutionPreferences.json";

		public DTOs.GetSolutionDetailsResponse GetSolutionDetails(DTOs.GetSolutionDetailsRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.GetSolutionDetailsResponse();

			response.SolutionDetails = new();

			if (System.IO.Directory.Exists(request.Solution))
			{
				var possibleSolutionFullNames = System.IO.Directory.GetFiles(request.Solution, "*.sln", System.IO.SearchOption.AllDirectories);

				if (possibleSolutionFullNames.Length == 1)
				{
					response.SolutionDetails.SolutionFullName = possibleSolutionFullNames.First();
				}
				else if (possibleSolutionFullNames.Length > 1)
				{
					var possibleSolutionName = System.IO.Path.GetFileName(request.Solution);

					var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

					if (string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName).Replace(" ", string.Empty).Replace(".", string.Empty), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));
					}

					if (string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						possibleSolutionFullName = possibleSolutionFullNames.OrderBy(possibleSolutionFullName => possibleSolutionFullName.Split(new[] { '/', '\\' }).Length).FirstOrDefault();
					}

					if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						response.SolutionDetails.SolutionFullName = possibleSolutionFullName;
					}
					else
					{
						var message = string.Format("Cannot determine which solution to update \"{0}\"", request.Solution);

						logger.LogError(message);

						throw new(message);
					}
				}
				else
				{
					var message = string.Format("Cannot find a solution to update \"{0}\"", request.Solution);

					logger.LogError(message);

					throw new(message);
				}
			}

			if (System.IO.File.Exists(request.Solution))
			{
				response.SolutionDetails.SolutionFullName = request.Solution;
			}

			if (string.IsNullOrWhiteSpace(response.SolutionDetails.SolutionFullName))
			{
				return null;
			}

			response.SolutionDetails.SolutionName = System.IO.Path.GetFileNameWithoutExtension(response.SolutionDetails.SolutionFullName);
			response.SolutionDetails.SolutionDirectory = System.IO.Path.GetDirectoryName(response.SolutionDetails.SolutionFullName);
			response.SolutionDetails.RootSourceDirectory = SourceControlClientApi.GetRootDirectory(new()
			{
				FullName = response.SolutionDetails.SolutionDirectory,
			}).FullName;

			var solutionPreferencesFullName = System.IO.Path.Combine(response.SolutionDetails.SolutionDirectory, SolutionPreferencesFileName);
			if (!System.IO.File.Exists(solutionPreferencesFullName))
			{
				solutionPreferencesFullName = System.IO.Path.Combine(response.SolutionDetails.SolutionDirectory, SolutionDetailsFileName);
			}

			if (System.IO.File.Exists(solutionPreferencesFullName))
			{
				using (var stream = System.IO.File.OpenRead(solutionPreferencesFullName))
				{
					var storedSolutionDetails = JsonSerializer.Deserialize<SerializableDTOs.ISolutionPreferences>(stream)?.Export();

					response.SolutionDetails.UpgradeNugetPackagesPriority = storedSolutionDetails.UpgradeNugetPackagesPriority ?? int.MaxValue;
					response.SolutionDetails.ExecuteBuildScriptTargetAfterUpdateNugetPackages = storedSolutionDetails.ExecuteBuildScriptTargetAfterUpdateNugetPackages;
					response.SolutionDetails.DoNotUpdatePackages = storedSolutionDetails.DoNotUpgradePackages;
				}
			}

			if (!string.IsNullOrWhiteSpace(response.SolutionDetails.SolutionFullName))
			{
				var projectDetailsSet = new List<ProjectDetails>();

				var solutionLines = System.IO.File.ReadAllLines(response.SolutionDetails.SolutionFullName);

				foreach (var solutionLine in solutionLines)
				{
					if (solutionLine.Trim().StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
					{
						var pieces = solutionLine.Split(new[] { '=' }).ToList();

						pieces = pieces[1].Split(new[] { '"' }).Select(piece => piece.Trim()).ToList();

						pieces.RemoveAll(piece => string.Equals(piece, ","));
						pieces.RemoveAll(string.IsNullOrWhiteSpace);

						if (pieces[1].EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
						{
							var projFullName = System.IO.Path.Combine(response.SolutionDetails.SolutionDirectory, pieces[1]);

							projectDetailsSet.Add(new()
							{
								ProjectName = System.IO.Path.GetFileNameWithoutExtension(projFullName),
								ProjectDirectory = System.IO.Path.GetDirectoryName(projFullName),
								ProjectFullName = projFullName,
							});
						}
					}
				}

				response.SolutionDetails.ProjectDetailsSet = projectDetailsSet.ToArray();

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

			return response;
		}
	}
}