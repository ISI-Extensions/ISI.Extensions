using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Kubernetes.DataTransferObjects.KubernetesApi
{
	public class ApplyResponse
	{
		public string Output { get; set; }

		public bool Errored { get; set; }
	}
}