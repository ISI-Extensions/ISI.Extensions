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
		protected string[] AddConnection(DTOs.IRequestConnection request)
		{
			var arguments = new List<string>();

			if (!string.IsNullOrWhiteSpace(request.Cluster))
			{
				arguments.Add($"--cluster={request.Cluster}");
			}
			else if (!string.IsNullOrWhiteSpace(request.Context))
			{
				arguments.Add($"--context={request.Context}");
			}
			else if (!string.IsNullOrWhiteSpace(request.KubeConfig))
			{
				arguments.Add($"--kubeconfig={request.KubeConfig}");
			}

			return arguments.ToArray();
		}
	}
}