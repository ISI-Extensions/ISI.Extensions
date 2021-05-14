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
using DTOs = ISI.Extensions.Git.DataTransferObjects.GitApi;
using SourceControlClientApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Git
{
	[SourceControlClientApi]
	public partial class GitApi : ISI.Extensions.Scm.ISourceControlClientApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public GitApi()
			: this(null)
		{
		}

		public GitApi(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger ?? new ConsoleLogger();
		}
		
		private const string SccDirectoryName = ".git";
		bool ISI.Extensions.Scm.ISourceControlClientApi.IsSccDirectory(string directoryName) => string.Equals(System.IO.Path.GetFileName(directoryName), SccDirectoryName, StringComparison.InvariantCultureIgnoreCase);
		bool ISI.Extensions.Scm.ISourceControlClientApi.UsesScc(string path)
		{
			var usesGit = false;

			while (!System.IO.Directory.Exists(path))
			{
				path = System.IO.Path.GetDirectoryName(path);
			}

			while (!usesGit && !string.IsNullOrEmpty(path))
			{
				usesGit = System.IO.Directory.Exists(System.IO.Path.Combine(path, SccDirectoryName));

				path = System.IO.Path.GetDirectoryName(path);
			}

			return usesGit;
		}

		SourceControlClientApiDTOs.UpdateWorkingCopyResponse ISI.Extensions.Scm.ISourceControlClientApi.UpdateWorkingCopy(SourceControlClientApiDTOs.UpdateWorkingCopyRequest request)
		{
			var response = new SourceControlClientApiDTOs.UpdateWorkingCopyResponse();

			var apiResponse = Update(new DTOs.UpdateRequest()
			{
				FullName = request.FullName,
				IncludeSubModules = request.IncludeExternals,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.CommitWorkingCopyResponse ISI.Extensions.Scm.ISourceControlClientApi.CommitWorkingCopy(SourceControlClientApiDTOs.CommitWorkingCopyRequest request)
		{
			var response = new SourceControlClientApiDTOs.CommitWorkingCopyResponse();

			var apiResponse = Commit(new DTOs.CommitRequest()
			{
				FullName = request.FullName,
				LogMessage = request.LogMessage,
			});

			response.Success = apiResponse.Success;

			return response;
		}
	}
}