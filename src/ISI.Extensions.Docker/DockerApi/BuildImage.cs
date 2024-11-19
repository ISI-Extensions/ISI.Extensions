#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
		public DTOs.BuildImageResponse BuildImage(DTOs.BuildImageRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.BuildImageResponse();

			var arguments = new List<string>();

			using (var tempEnvironmentFiles = new TempEnvironmentFiles(request.AppDirectory, request.EnvironmentFileFullNames, request.EnvironmentVariables))
			{
				request.OnBuildStart?.Invoke(tempEnvironmentFiles.EnvironmentVariables.TryGetValue);

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

				arguments.Add("build");

				arguments.AddRange(tempEnvironmentFiles.GetDockerComposeArguments());

				if (request.BuildArguments.NullCheckedAny())
				{
					foreach (var buildArgument in request.BuildArguments)
					{
						arguments.Add($"--build-arg \"{buildArgument.Key}={buildArgument.Value}\"");
					}
				}

				var containerRegistry = request.ContainerRegistry ?? string.Empty;

				var containerRepository = request.ContainerRepository;
				var containerImageTags = (request.ContainerImageTags.NullCheckedAny() ? request.ContainerImageTags : [" latest"]);

				if (string.IsNullOrWhiteSpace(containerRegistry))
				{
					for (var index = 0; index < containerImageTags.Length; index++)
					{
						containerImageTags[index] = $"{containerRepository}:{containerImageTags[index]}";
					}
				}
				else
				{
					for (var index = 0; index < containerImageTags.Length; index++)
					{
						containerImageTags[index] = $"{containerRegistry}/{containerRepository}:{containerImageTags[index]}";
					}
				}

				foreach (var containerImageTag in containerImageTags)
				{
					arguments.Add($"--tag {containerImageTag}");
				}

				if (string.IsNullOrWhiteSpace(request.DockerFileFullName))
				{
					arguments.Add(".");
				}
				else
				{
					arguments.Add($"\"{request.DockerFileFullName}\"");
				}

				logger.LogInformation($"docker {string.Join(" ", arguments)}");

				var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = logger,
					ProcessExeFullName = "docker",
					Arguments = arguments.ToArray(),
					WorkingDirectory = request.AppDirectory,
					EnvironmentVariables = AddDockerContextServerApiVersion(null, request.Host, request.Context),
				});

				response.Output = waitForProcessResponse.Output;

				response.Errored = waitForProcessResponse.Errored;

				if (!response.Errored)
				{
					if (!string.IsNullOrWhiteSpace(containerRegistry) && containerImageTags.NullCheckedAny())
					{
						var pushImageResponse = PushImages(new()
						{
							Host = request.Host,
							Context = request.Context,
							ContainerImageTags = request.ContainerImageTags,
							RemoveImage = true,
							AddToLog = request.AddToLog,
						});

						response.Output += "\n" + pushImageResponse.Output;

						response.Errored |= pushImageResponse.Errored;
					}
				}

				request.OnBuildFinish?.Invoke(tempEnvironmentFiles.EnvironmentVariables.TryGetValue, response.Errored);
			}

			return response;
		}
	}
}