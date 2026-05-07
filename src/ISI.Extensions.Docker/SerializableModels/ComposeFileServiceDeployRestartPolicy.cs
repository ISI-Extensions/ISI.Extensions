using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeployRestartPolicy
	{
		[YamlMember(Alias = "condition", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Condition { get; set; }

		[YamlMember(Alias = "delay", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Delay { get; set; }

		[YamlMember(Alias = "max_attempts", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? MaxAttempts { get; set; }

		[YamlMember(Alias = "window", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Window { get; set; }
	}
}
