using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Kubernetes.DataTransferObjects.KubernetesApi
{
	public class ApplyRequest : IRequestConnection
	{
		public string Cluster { get; set; }
		public string Context { get; set; }
		public string KubeConfig { get; set; }

		public IEnumerable<string> ConfigFileNames { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}