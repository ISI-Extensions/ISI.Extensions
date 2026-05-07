using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeployUpdateConfig
	{
		[YamlMember(Alias = "parallelism", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Parallelism { get; set; }

		[YamlMember(Alias = "delay", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Delay { get; set; }

		[YamlMember(Alias = "failure_action", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string FailureAction { get; set; }

		[YamlMember(Alias = "monitor", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Monitor { get; set; }

		[YamlMember(Alias = "max_failure_ratio", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string MaxFailureRatio { get; set; }

		[YamlMember(Alias = "order", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Order { get; set; }
	}
}
