#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
		private SolutionDetails GetSolutionDetails(string solution)
		{
			var solutionDetails = new SolutionDetails();

			if (System.IO.Directory.Exists(solution))
			{
				var possibleSolutionFullNames = System.IO.Directory.GetFiles(solution, "*.sln", System.IO.SearchOption.AllDirectories);

				if (possibleSolutionFullNames.Length == 1)
				{
					solutionDetails.SolutionFullName = possibleSolutionFullNames.First();
				}
				else if (possibleSolutionFullNames.Length > 1)
				{
					var possibleSolutionName = System.IO.Path.GetFileName(solution);

					var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

					if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						solutionDetails.SolutionFullName = possibleSolutionFullName;
					}
					else
					{
						throw new Exception(string.Format("Cannot determine which solution to update \"{0}\"", solution));
					}
				}
				else
				{
					throw new Exception(string.Format("Cannot find a solution to update \"{0}\"", solution));
				}
			}

			if (System.IO.File.Exists(solution))
			{
				solutionDetails.SolutionFullName = solution;
			}

			if (string.IsNullOrWhiteSpace(solutionDetails.SolutionFullName))
			{
				return null;
			}

			solutionDetails.Name = System.IO.Path.GetFileNameWithoutExtension(solutionDetails.SolutionFullName);
			solutionDetails.SolutionDirectory = System.IO.Path.GetDirectoryName(solutionDetails.SolutionFullName);
			solutionDetails.RootSourceDirectory  = SourceControlClientApi.GetRootDirectory(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.GetRootDirectoryRequest()
			{
				FullName = solutionDetails.SolutionDirectory,
			}).FullName;

			var solutionDetailsFullName =  System.IO.Path.Combine(solutionDetails.SolutionDirectory, SerializableModels.SolutionDetails.FileName);
			if (System.IO.File.Exists(solutionDetailsFullName))
			{
				using (var stream = System.IO.File.OpenRead(solutionDetailsFullName))
				{
					var storedSolutionDetails = Serialization.Deserialize<SerializableModels.SolutionDetails>(stream, ISI.Extensions.Serialization.SerializationFormat.Json);

					solutionDetails.UpdateNugetPackagesPriority = storedSolutionDetails.UpdateNugetPackagesPriority ?? int.MaxValue;
					solutionDetails.ExecuteBuildScriptTargetAfterUpdateNugetPackages = storedSolutionDetails.ExecuteBuildScriptTargetAfterUpdateNugetPackages;
					solutionDetails.DoNotUpdatePackageIds = storedSolutionDetails.DoNotUpdatePackageIds;
				}
			}

			return solutionDetails;
		}
	}
}