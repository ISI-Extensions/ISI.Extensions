using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeployPlacement
	{
		[YamlMember(Alias = "constraints", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string[] Constraints { get; set; }

		[YamlMember(Alias = "preferences", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public Dictionary<string, string>[] Preferences { get; set; }
	}
}
