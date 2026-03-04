using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Docker.DataTransferObjects.DockerApi;
using SERIALIZABLEMODELS = ISI.Extensions.Docker.SerializableModels;

namespace ISI.Extensions.Docker
{
	public partial class DockerApi
	{
		public DTOs.CopyFromImageResponse CopyFromImage(DTOs.CopyFromImageRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.CopyFromImageResponse();

			using (var tempEnvironmentFiles = new TempEnvironmentVariablesFiles(request.AppDirectory, request.EnvironmentFileFullNames, request.EnvironmentFileContents, request.EnvironmentVariables))
			{
				var containerName = $"tmp{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36)}";

				var containerRegistry = request.ContainerRegistry ?? string.Empty;
				var containerRepository = request.ContainerRepository;
				var containerImageTag = (string.IsNullOrWhiteSpace(request.ContainerImageTag) ? "latest" : request.ContainerImageTag);
				containerImageTag = GetContainerImageReference(containerRegistry, containerRepository, containerImageTag);

				{
					var arguments = new List<string>();

					arguments.AddRange(GetHostContext(request));

					arguments.Add("run");

					arguments.Add($"--name {containerName}");

					arguments.Add(containerImageTag);

					var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = "docker",
						Arguments = arguments.ToArray(),
						WorkingDirectory = request.AppDirectory,
						EnvironmentVariables = AddDockerContextServerApiVersion(null, request),
					});

					if (waitForProcessResponse.Errored)
					{
						throw new Exception(waitForProcessResponse.Output);
					}
				}

				foreach (var copyFile in request.CopyFiles)
				{
					var arguments = new List<string>();

					arguments.AddRange(GetHostContext(request));

					arguments.Add("cp");

					arguments.Add($"{containerName}:{copyFile.Source}");

					arguments.Add((string.IsNullOrWhiteSpace(copyFile.Target) ? "." : copyFile.Target));

					var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = "docker",
						Arguments = arguments.ToArray(),
						WorkingDirectory = request.AppDirectory,
						EnvironmentVariables = AddDockerContextServerApiVersion(null, request),
					});
				}

				{
					var arguments = new List<string>();

					arguments.AddRange(GetHostContext(request));

					arguments.Add("rm");

					arguments.Add(containerName);

					var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						ProcessExeFullName = "docker",
						Arguments = arguments.ToArray(),
						WorkingDirectory = request.AppDirectory,
						EnvironmentVariables = AddDockerContextServerApiVersion(null, request),
					});

					if (waitForProcessResponse.Errored)
					{
						throw new Exception(waitForProcessResponse.Output);
					}
				}
			}

			return response;
		}
	}
}