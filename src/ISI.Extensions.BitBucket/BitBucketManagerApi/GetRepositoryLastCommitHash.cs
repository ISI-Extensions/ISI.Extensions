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
using DTOs = ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi;
using SerializableDTOs = ISI.Extensions.BitBucket.SerializableModels;

namespace ISI.Extensions.BitBucket
{
	public partial class BitBucketManagerApi
	{
		public DTOs.GetRepositoryLastCommitHashResponse GetRepositoryLastCommitHash(DTOs.GetRepositoryLastCommitHashRequest request)
		{
			var response = new DTOs.GetRepositoryLastCommitHashResponse();

			var remoteUri = new UriBuilder("https://bitbucket.org");
			remoteUri.AddDirectoryToPath(request.Workspace);
			remoteUri.AddDirectoryToPath(request.RepositoryKey);

			remoteUri.UserName = "x-token-auth";
			remoteUri.Password = request.BitBucketApiToken;

			var arguments = new List<string>();

			arguments.Add("ls-remote");
			arguments.Add("-h");
			arguments.Add($"\"{remoteUri.Uri}\"");

			var processResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				ProcessExeFullName = "git",
				Arguments = arguments.ToArray(),
			});

			var answers = processResponse.Output.Trim([' ', '\r', '\n']).Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

			if (answers.Length > 1)
			{
				response.CommitHash = answers[0];
				response.Branch = answers[1];
			}

			return response;
		}
	}
}