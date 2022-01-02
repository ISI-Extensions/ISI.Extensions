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
 
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.GetAssemblyVersionFilesResponse GetAssemblyVersionFiles(DTOs.GetAssemblyVersionFilesRequest request)
		{
			var response = new DTOs.GetAssemblyVersionFilesResponse();

			var solutionDetails = GetSolutionDetails(new DTOs.GetSolutionDetailsRequest()
			{
				Solution = request.Solution,
			}).SolutionDetails;

			response.AssemblyVersionFiles = new ISI.Extensions.VisualStudio.AssemblyVersionFileDictionary();

			void addVersionFile(string assemblyGroupName, string assemblyInfoFullName)
			{
				//Console.WriteLine("addVersionFile(\"{0}\", \"{1}\")", assemblyGroupName, assemblyInfoFullName);

				var assemblyVersion = GetAssemblyVersion(CodeGenerationApi.ParseAssemblyInfoFile(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeGenerationApi.ParseAssemblyInfoFileRequest()
				{
					AssemblyInfoFullName = assemblyInfoFullName,
				}).Version, request.BuildRevision);

				response.AssemblyVersionFiles.Add(new ISI.Extensions.VisualStudio.AssemblyVersionFile()
				{
					FullName = assemblyInfoFullName,
					AssemblyGroupName = assemblyGroupName,
					AssemblyVersion = assemblyVersion,
					AssemblyFileContent = System.IO.File.ReadAllText(assemblyInfoFullName),
				});
			}

			if (!string.IsNullOrWhiteSpace(request.RootAssemblyVersionKey))
			{
				var assemblyVersionFile = System.IO.Path.Combine(solutionDetails.SolutionDirectory, string.Format("{0}.Version.cs", request.RootAssemblyVersionKey));

				if (System.IO.File.Exists(assemblyVersionFile))
				{
					addVersionFile(request.RootAssemblyVersionKey, assemblyVersionFile);
				}
			}

			foreach (var projectPath in solutionDetails.ProjectDetailsSet.Select(projectDetails => projectDetails.ProjectDirectory))
			{
				var assemblyGroupDirectory = System.IO.Path.GetDirectoryName(projectPath);
				var assemblyGroupName = System.IO.Path.GetFileName(assemblyGroupDirectory);

				if (!response.AssemblyVersionFiles.ContainsKey(assemblyGroupName))
				{
					var assemblyVersionFile = System.IO.Path.Combine(assemblyGroupDirectory, string.Format("{0}.Version.cs", assemblyGroupName));
					if (System.IO.File.Exists(assemblyVersionFile))
					{
						addVersionFile(assemblyGroupName, assemblyVersionFile);
					}
				}
			}

			return response;
		}
	}
}