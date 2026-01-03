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
using DTOs = ISI.Extensions.Git.DataTransferObjects.BackupHelper;

namespace ISI.Extensions.Git
{
	public partial class BackupHelper
	{
		public virtual DTOs.DumpRepositoryResponse DumpRepository(DTOs.IDumpRepositoryRequest request)
		{
			var response = new DTOs.DumpRepositoryResponse();

			request.StatusTracker ??= new ISI.Extensions.StatusTracker();
			var logger = new ISI.Extensions.StatusTrackers.StatusTrackerLogger(request.StatusTracker, Logger);

			request.ExecutedDateTimeUtc ??= DateTimeStamper.CurrentDateTime();

			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				var repositoryDirectory = System.IO.Path.Combine(tempDirectory.FullName, request.RepositoryKey);

				var createRepositoryResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = logger,
					ProcessExeFullName = "git",
					Arguments = ["init", "--bare", repositoryDirectory],
				});

				if (createRepositoryResponse.ExitCode == 0)
				{
					var remoteLocation = string.Empty;
					var remoteLocationIsUrl = false;
					if (Uri.TryCreate(RepositoriesPath, UriKind.Absolute, out var repositoriesUri))
					{
						remoteLocationIsUrl = true;
						var remoteUri = new UriBuilder(repositoriesUri);
						remoteUri.AddDirectoryToPath(request.RepositoryKey);
						remoteLocation = remoteUri.Uri.ToString();
					}
					else
					{
						remoteLocation = $"\"{System.IO.Path.Combine(RepositoriesPath, request.RepositoryKey)}\"";
					}

					var batchFullName = System.IO.Path.Combine(tempDirectory.FullName, "fetch-repository.bat");
					var batchEnvironmentVariables = new InvariantCultureIgnoreCaseStringDictionary<string>();
					var batchCommands = new StringBuilder();

					var userName = (string.IsNullOrWhiteSpace(UserName) ? Password : UserName); //Incase on apitoken only and it was passed in as a password
					var password = (!string.IsNullOrWhiteSpace(UserName) ? Password : null);

					var gitUrl = string.Empty;
					var userNameEnvironmentName = $"U{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36).ToUpper()}";
					var passwordEnvironmentName = $"P{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36).ToUpper()}";
					if (remoteLocationIsUrl && !string.IsNullOrWhiteSpace(userName))
					{
						batchEnvironmentVariables.Add(userNameEnvironmentName, userName);
						if (!string.IsNullOrWhiteSpace(password))
						{
							batchEnvironmentVariables.Add(passwordEnvironmentName, password);
						}

						var tempUserName = $"u{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}";

						var remoteUri = new UriBuilder(remoteLocation);
						remoteUri.UserName = tempUserName;
						remoteLocation = remoteUri.Uri.ToString();

						remoteUri.UserName = $"${userNameEnvironmentName}";
						if (!string.IsNullOrWhiteSpace(password))
						{
							remoteUri.Password = $"${passwordEnvironmentName}";
						}

						gitUrl = remoteUri.Uri.ToString();

						batchCommands.AppendLine($"git config --global url.\"{gitUrl}\".insteadOf \"{remoteLocation}\"");
						batchCommands.AppendLine();
					}

					batchCommands.AppendLine($"git -C \"{repositoryDirectory}\" fetch --force --prune \"{remoteLocation}\" refs/heads/*:refs/heads/* refs/tags/*:refs/tags/*");
					batchCommands.AppendLine();

					if (!string.IsNullOrWhiteSpace(gitUrl))
					{
						batchCommands.AppendLine($"git config --global --remove-section url.\"{gitUrl}\"");
					}

					System.IO.File.WriteAllText(batchFullName, batchCommands.ToString());

					var pullRepositoryResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = batchFullName,
						EnvironmentVariables = batchEnvironmentVariables,
					});

					if (pullRepositoryResponse.ExitCode == 0)
					{
						var bundleFullName = $"\"{System.IO.Path.Combine(tempDirectory.FullName, $"{request.RepositoryKey}.bundle")}\"";

						if (request is DTOs.DumpRepositoryFileNameRequest dumpRepositoryFileNameRequest)
						{
							bundleFullName = dumpRepositoryFileNameRequest.DumpFullName;
						}

						var bundleRepositoryResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
						{
							Logger = logger,
							ProcessExeFullName = "git",
							Arguments = ["-C", repositoryDirectory, "bundle", "create", bundleFullName, "--all"],
						});

						if (bundleRepositoryResponse.ExitCode == 0)
						{
							if (request is DTOs.DumpRepositoryStreamRequest dumpRepositoryStreamRequest)
							{
								using (var dumpStream = System.IO.File.OpenRead(bundleFullName))
								{
									dumpStream.CopyTo(dumpRepositoryStreamRequest.Stream);
								}
							}

							response.Success = true;
						}
					}
				}
			}


			/*
			var processStartInfo = new System.Diagnostics.ProcessStartInfo();

			processStartInfo.CreateNoWindow = true;
			//processStartInfo.FileName = "git";
			//processStartInfo.Arguments = string.Format("bundle create \"{0}\" --all", request.DumpFullName);
			//7z a -r E:\gogs-backups\isi.extensions.cake.git.zip E:\gogs-repositories\isi\isi.extensions.cake.git\*
			processStartInfo.FileName = "7z";
			processStartInfo.Arguments = string.Format("a -r \"{0}\" .\\*", request.DumpFullName);
			processStartInfo.WorkingDirectory = System.IO.Path.Combine(RepositoriesPath, request.RepositoryKey.Replace("+", "\\"));

			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

			//request.statusTracker.AddToLog(string.Format("    Working Directory: \"{0}\"", processStartInfo.WorkingDirectory));
			//request.statusTracker.AddToLog(string.Format("    Command: {0} {1}", processStartInfo.FileName, processStartInfo.Arguments));

			using (var process = System.Diagnostics.Process.Start(processStartInfo))
			{
				process.OutputDataReceived += (sender, args) =>
				{
					if (!string.IsNullOrWhiteSpace(args.Data))
					{
						request.StatusTracker.AddToLog(args.Data);
					}
				};

				process.BeginOutputReadLine();

				process.ErrorDataReceived += (sender, args) =>
				{
					if (!string.IsNullOrWhiteSpace(args.Data))
					{
						request.StatusTracker.AddToLog(string.Format("Error: {0}", args.Data));
					}
				};

				process.BeginErrorReadLine();

				process.WaitForExit();
			}
			*/

			return response;
		}
	}
}