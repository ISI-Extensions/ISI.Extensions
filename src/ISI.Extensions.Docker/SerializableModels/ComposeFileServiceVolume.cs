using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceVolume
	{
		[YamlMember(Alias = "type", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Type { get; set; }

		[YamlMember(Alias = "target", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Target { get; set; }

		[YamlMember(Alias = "source", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Source { get; set; }

		[YamlMember(Alias = "read_only", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? ReadOnly { get; set; }

		[YamlMember(Alias = "driver", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Driver { get; set; }

		[YamlMember(Alias = "driver_opts", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> DriverOpts { get; set; }

		[YamlMember(Alias = "external", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? External { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }
	}
}
