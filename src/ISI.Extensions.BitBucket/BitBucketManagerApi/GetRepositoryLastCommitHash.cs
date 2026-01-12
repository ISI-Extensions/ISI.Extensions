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