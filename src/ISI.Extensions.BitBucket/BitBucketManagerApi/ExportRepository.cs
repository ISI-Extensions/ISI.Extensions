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
using Microsoft.Extensions.Logging;
using SharpCompress.Writers;
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
						switch (request.ExportFormat)
						{
							case ISI.Extensions.Git.ExportFormat.Bundle:
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
								break;

							case ISI.Extensions.Git.ExportFormat.TarGz:
								var tarGzFullName = System.IO.Path.Combine(tempDirectory.FullName, $"{request.RepositoryKey}.tar.gz");

								Logger.LogInformation($"tarGzFullName: {tarGzFullName}");

								using (var compressedFileStream = System.IO.File.Create(tarGzFullName))
								{
									using (var writer = SharpCompress.Writers.WriterFactory.OpenWriter(compressedFileStream, SharpCompress.Common.ArchiveType.Tar, new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.GZip)))
									{
										writer.WriteAll(repositoryDirectory, "*", System.IO.SearchOption.AllDirectories);
									}
								}

								using (var dumpStream = System.IO.File.OpenRead(tarGzFullName))
								{
									dumpStream.CopyTo(request.DownloadStream);
								}
								break;

							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}
			}

			return response;
		}
	}
}