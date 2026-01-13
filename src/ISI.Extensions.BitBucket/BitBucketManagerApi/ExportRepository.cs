using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi;
using SerializableDTOs = ISI.Extensions.BitBucket.SerializableModels;

namespace ISI.Extensions.BitBucket
{
	public partial class BitBucketManagerApi
	{
		public DTOs.ExportRepositoryResponse ExportRepository(DTOs.ExportRepositoryRequest request)
		{
			var response = new DTOs.ExportRepositoryResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				//tempDirectory.DeleteDirectory = false;

				var repositoryDirectory = System.IO.Path.Combine(tempDirectory.FullName, request.RepositoryKey);

				Logger.LogInformation($"repositoryDirectory: {repositoryDirectory}");

				var createRepositoryResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = logger,
					ProcessExeFullName = "git",
					Arguments = ["init", "--bare", repositoryDirectory],
				});

				if (createRepositoryResponse.ExitCode == 0)
				{
					var remoteUri = new UriBuilder("https://bitbucket.org");
					remoteUri.AddDirectoryToPath(request.Workspace);
					remoteUri.AddDirectoryToPath(request.RepositoryKey);

					remoteUri.UserName = "x-token-auth";
					remoteUri.Password = request.BitBucketApiToken;

					var arguments = new List<string>();

					arguments.Add("-C");
					arguments.Add($"\"{repositoryDirectory}\"");
					arguments.Add("fetch");
					arguments.Add("--force");
					arguments.Add("--prune");
					arguments.Add($"\"{remoteUri.Uri}\"");
					arguments.Add("refs/heads/*:refs/heads/*");
					arguments.Add("refs/tags/*:refs/tags/*");

					var pullRepositoryResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = "git",
						Arguments = arguments.ToArray(),
					});

					if (pullRepositoryResponse.ExitCode == 0)
					{
						var bundleFullName = System.IO.Path.Combine(tempDirectory.FullName, $"{request.RepositoryKey}.bundle");

						Logger.LogInformation($"bundleFullName: {bundleFullName}");

						var bundleRepositoryResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
						{
							Logger = logger,
							ProcessExeFullName = "git",
							Arguments = ["-C", repositoryDirectory, "bundle", "create", $"\"{bundleFullName}\"", "--all"],
						});

						if (bundleRepositoryResponse.ExitCode == 0)
						{
							using (var dumpStream = System.IO.File.OpenRead(bundleFullName))
							{
								dumpStream.CopyTo(request.DownloadStream);
							}
						}
					}
				}
			}

			return response;
		}
	}
}