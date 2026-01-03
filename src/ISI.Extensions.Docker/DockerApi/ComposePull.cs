#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Docker.DataTransferObjects.DockerApi;
using SERIALIZABLEMODELS = ISI.Extensions.Docker.SerializableModels;

namespace ISI.Extensions.Docker
{
	public partial class DockerApi
	{
		public DTOs.ComposePullResponse ComposePull(DTOs.ComposePullRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.ComposePullResponse();

			var arguments = new List<string>();

			using (var tempEnvironmentFiles = new TempEnvironmentVariablesFiles(request.ComposeDirectory, request.EnvironmentFileFullNames, request.EnvironmentFileContents, request.EnvironmentVariables))
			{
				request.OnComposePullStart?.Invoke(tempEnvironmentFiles.EnvironmentVariables.TryGetValue);

				if (!string.IsNullOrWhiteSpace(request.Host))
				{
					arguments.Add($"--host {request.Host}");
				}
				else if (!string.IsNullOrWhiteSpace(request.Context))
				{
					if (!DockerContexts.ContainsKey(request.Context))
					{
						throw new Exception($"Context \"{request.Context}\" not found");
					}

					arguments.Add($"--context {request.Context}");
				}

				if (!UseDockerDashCompose)
				{
					arguments.Add("compose");
				}

				if (!string.IsNullOrWhiteSpace(request.ComposeFile))
				{
					arguments.Add($"--file {request.ComposeFile}");
				}

				if (!string.IsNullOrWhiteSpace(request.ProjectName))
				{
					arguments.Add($"--project-name {request.ProjectName}");
				}

				arguments.Add("--ansi never");
				arguments.Add("--progress plain");

				arguments.AddRange(tempEnvironmentFiles.GetDockerComposeArguments());

				arguments.Add("pull");

				logger.LogInformation($"{(UseDockerDashCompose ? "docker-compose" : "docker")} {string.Join(" ", arguments)}");

				var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = logger,
					ProcessExeFullName = (UseDockerDashCompose ? "docker-compose" : "docker"),
					Arguments = arguments.ToArray(),
					WorkingDirectory = request.ComposeDirectory,
					EnvironmentVariables = AddDockerContextServerApiVersion(null, request.Host, request.Context),
				});

				response.Output = waitForProcessResponse.Output;

				response.Errored = waitForProcessResponse.Errored;
				
				request.OnComposePullFinish?.Invoke(tempEnvironmentFiles.EnvironmentVariables.TryGetValue, response.Errored);

				if (response.Errored)
				{
					throw new Exception($"Error pulling\n{waitForProcessResponse.Output}");
				}
			}

			return response;
		}
	}
}