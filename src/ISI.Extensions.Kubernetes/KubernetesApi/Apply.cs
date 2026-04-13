using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Kubernetes.DataTransferObjects.KubernetesApi;

namespace ISI.Extensions.Kubernetes
{
	public partial class KubernetesApi
	{
		public DTOs.ApplyResponse Apply(DTOs.ApplyRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.ApplyResponse();

			var arguments = new List<string>();

			arguments.AddRange(AddConnection(request));

			arguments.Add("apply");

			foreach (var configFileName in request.ConfigFileNames)
			{
				arguments.Add($"--filename={configFileName}");
			}

			var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = logger,
				ProcessExeFullName = "kubectl",
				Arguments = arguments.ToArray(),
			});

			response.Output = waitForProcessResponse.Output;

			response.Errored = waitForProcessResponse.Errored;

			return response;
		}
	}
}