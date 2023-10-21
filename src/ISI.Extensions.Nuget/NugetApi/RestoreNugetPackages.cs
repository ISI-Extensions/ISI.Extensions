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
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.RestoreNugetPackagesResponse RestoreNugetPackages(DTOs.RestoreNugetPackagesRequest request)
		{
			var response = new DTOs.RestoreNugetPackagesResponse();

			var target = request.PackagesConfigFileName;

			if (!string.IsNullOrWhiteSpace(request.Solution) && System.IO.Directory.Exists(request.Solution))
			{
				var possibleSolutionFullNames = System.IO.Directory.GetFiles(request.Solution, "*.sln", System.IO.SearchOption.AllDirectories);

				if (possibleSolutionFullNames.Length == 1)
				{
					request.Solution = possibleSolutionFullNames.First();
				}
				else if (possibleSolutionFullNames.Length > 1)
				{
					var possibleSolutionName = System.IO.Path.GetFileName(request.Solution);

					var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

					if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						request.Solution = possibleSolutionFullName;
					}
					else
					{
						throw new(string.Format("Cannot determine which solution to update \"{0}\"", request.Solution));
					}
				}
				else
				{
					throw new(string.Format("Cannot find a solution to update \"{0}\"", request.Solution));
				}
			}

			var solutionDirectory = System.IO.Path.GetDirectoryName(request.Solution);

			if (string.IsNullOrWhiteSpace(target))
			{
				target = request.Solution;
			}

			var arguments = string.Format("restore \"{0}\" -PackagesDirectory \"{1}\" -NonInteractive -MSBuildPath \"{2}\"", target, System.IO.Path.Combine(solutionDirectory, "packages"), System.IO.Path.GetDirectoryName(request.MSBuildExe));

			var nugetExeFullName = GetNugetExeFullName(new()).NugetExeFullName;

			response.Success = !ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = new AddToLogLogger(request.AddToLog, Logger),
				ProcessExeFullName = nugetExeFullName,
				Arguments = new[] { arguments },
			}).Errored;

			return response;
		}
	}
}