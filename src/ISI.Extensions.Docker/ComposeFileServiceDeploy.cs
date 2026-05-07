using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceDeploy
	{
		public int? Replicas { get; set; }
		public string Mode { get; set; }
		public ComposeFileServiceDeployResources Resources { get; set; }
		public ComposeFileServiceDeployPlacement Placement { get; set; }
		public ComposeFileServiceDeployUpdateConfig UpdateConfig { get; set; }
		public ComposeFileServiceDeployRestartPolicy RestartPolicy { get; set; }
		public IDictionary<string, string> Labels { get; set; }
	}
}
