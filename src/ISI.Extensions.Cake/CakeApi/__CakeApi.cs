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
using DTOs = ISI.Extensions.Cake.DataTransferObjects.CakeApi;

namespace ISI.Extensions.Cake
{
	[BuildScriptApi]
	public partial class CakeApi : ISI.Extensions.Scm.IBuildScriptApi
	{
		public const string BuildScriptTypeUuid = "d23cb050-1bc5-4891-a8aa-b4f4da2a52aa";
		public const string BuildScriptFileName = "build.cake";

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public CakeApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}

		Guid ISI.Extensions.Scm.IBuildScriptApi.BuildScriptTypeUuid => BuildScriptTypeUuid.ToGuid();

		bool ISI.Extensions.Scm.IBuildScriptApi.TryGetBuildScript(string solutionDirectory, out string buildScriptFullName)
		{
			try
			{
				if (string.Equals(System.IO.Path.GetFileName(solutionDirectory), BuildScriptFileName, StringComparison.CurrentCultureIgnoreCase))
				{
					buildScriptFullName = solutionDirectory;

					return true;
				}

				if (System.IO.File.Exists(solutionDirectory))
				{
					solutionDirectory = System.IO.Path.GetDirectoryName(solutionDirectory);
				}

				var possibleBuildScriptFullNames = System.IO.Directory.GetFiles(solutionDirectory, BuildScriptFileName, System.IO.SearchOption.AllDirectories).OrderBy(fileName => fileName.Length);

				buildScriptFullName = possibleBuildScriptFullNames.NullCheckedFirstOrDefault();

				return !string.IsNullOrWhiteSpace(buildScriptFullName);
			}
			catch
			{
				buildScriptFullName = null;

				return false;
			}
		}

		ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.IsBuildScriptFileResponse ISI.Extensions.Scm.IBuildScriptApi.IsBuildScriptFile(ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.IsBuildScriptFileRequest request)
		{
			var response = new ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.IsBuildScriptFileResponse();

			response.IsBuildFile = IsBuildScriptFile(new DTOs.IsBuildScriptFileRequest()
			{
				BuildScriptFullName = request.BuildScriptFullName,
			}).IsBuildFile;

			return response;
		}

		ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.GetTargetKeysFromBuildScriptResponse ISI.Extensions.Scm.IBuildScriptApi.GetTargetKeysFromBuildScript(ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.GetTargetKeysFromBuildScriptRequest request)
		{
			var response = new ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.GetTargetKeysFromBuildScriptResponse();

			response.Targets = GetTargetKeysFromBuildScript(new DTOs.GetTargetKeysFromBuildScriptRequest()
			{
				BuildScriptFullName = request.BuildScriptFullName,
			}).Targets;

			return response;
		}

		ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.ExecuteBuildTargetResponse ISI.Extensions.Scm.IBuildScriptApi.ExecuteBuildTarget(ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.ExecuteBuildTargetRequest request)
		{
			var response = new ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.ExecuteBuildTargetResponse();

			var executeBuildTargetResponse = ExecuteBuildTarget(new DTOs.ExecuteBuildTargetRequest()
			{
				BuildScriptFullName = request.BuildScriptFullName,
				Target = request.Target,
				Parameters = request.Parameters,
				AddToLog = request.AddToLog,
			});

			response.ExecutionOutputLog = executeBuildTargetResponse?.ExecutionOutputLog;
			response.Success = executeBuildTargetResponse?.Success ?? false;

			return response;
		}
	}
}