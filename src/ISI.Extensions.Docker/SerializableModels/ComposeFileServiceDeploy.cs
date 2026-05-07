using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeploy
	{
		[YamlMember(Alias = "replicas", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Replicas { get; set; }

		[YamlMember(Alias = "mode", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Mode { get; set; }

		[YamlMember(Alias = "resources", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployResources Resources { get; set; }

		[YamlMember(Alias = "placement", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployPlacement Placement { get; set; }

		[YamlMember(Alias = "update_config", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployUpdateConfig UpdateConfig { get; set; }

		[YamlMember(Alias = "restart_policy", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployRestartPolicy RestartPolicy { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }
	}
}
