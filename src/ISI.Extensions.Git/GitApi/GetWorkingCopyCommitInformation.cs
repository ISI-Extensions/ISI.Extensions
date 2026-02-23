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
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Git.DataTransferObjects.GitApi;
using SourceControlClientApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Git
{
	public partial class GitApi
	{
		public DTOs.GetWorkingCopyCommitInformationResponse GetWorkingCopyCommitInformation(DTOs.GetWorkingCopyCommitInformationRequest request)
		{
			var response = new DTOs.GetWorkingCopyCommitInformationResponse();

			if (GitIsInstalled)
			{
				var rootFullName = (string)null;

				{
					var arguments = new List<string>();

					arguments.Add("rev-parse");
					arguments.Add("--show-toplevel");

					var gitResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = new NullLogger(),
						ProcessExeFullName = "git",
						Arguments = arguments.ToArray(),
						WorkingDirectory = System.IO.Path.GetFullPath(request.FullName),
					});

					rootFullName = (gitResponse.Output ?? string.Empty).Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(line => !string.IsNullOrWhiteSpace(line));
				}
				
				{
					var arguments = new List<string>();

					arguments.Add("log");
					arguments.Add("-1");
					arguments.Add("--pretty=\"{'commit': '%H', 'author': '%an', 'email': '%ae', 'date': '%ai', 'message': '%s'}\"");
					arguments.Add("--");
					arguments.Add($"\"{request.FullName}\"");

					var workingDirectory = System.IO.Directory.Exists(request.FullName) ? request.FullName : System.IO.Path.GetDirectoryName(request.FullName);

					var gitResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = new NullLogger(),
						ProcessExeFullName = "git",
						Arguments = arguments.ToArray(),
						WorkingDirectory = workingDirectory,
					});

					var serializableWorkingCopyCommitInformation = JsonSerializer.Deserialize<SerializableModels.WorkingCopyCommitInformation>(gitResponse.Output);

					response.WorkingCopyCommitInformation = new()
					{
						Path = ISI.Extensions.IO.Path.GetRelativePath(rootFullName, request.FullName),
						CommitKey = serializableWorkingCopyCommitInformation?.CommitKey,
						Author = serializableWorkingCopyCommitInformation?.Author,
						AuthorEmail = serializableWorkingCopyCommitInformation?.AuthorEmail,
						CommitDateTimeUtc = (serializableWorkingCopyCommitInformation?.CommitDateTimeUtc).ToDateTimeUtc(),
						Message = serializableWorkingCopyCommitInformation?.Message,
					};
				}
			}

			return response;
		}
	}
}