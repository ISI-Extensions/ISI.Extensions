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
		public DTOs.TagImageResponse TagImage(DTOs.TagImageRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.TagImageResponse();

			var arguments = new List<string>();

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

			arguments.Add("tag");

			arguments.Add(GetContainerImageReference(request.SourceContainerRegistry, request.SourceContainerRepository, request.SourceContainerImageTag));

			arguments.Add(GetContainerImageReference(request.TargetContainerRegistry, request.TargetContainerRepository, request.TargetContainerImageTag));

			var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = logger,
				ProcessExeFullName = "docker",
				Arguments = arguments.ToArray(),
				EnvironmentVariables = AddDockerContextServerApiVersion(null, request.Host, request.Context),
			});

			response.Output = waitForProcessResponse.Output;

			response.Errored = waitForProcessResponse.Errored;

			return response;
		}
	}
}