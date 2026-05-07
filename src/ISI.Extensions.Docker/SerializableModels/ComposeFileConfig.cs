using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileConfig
	{
		[YamlMember(Alias = "name", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Name { get; set; }

		[YamlMember(Alias = "file", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string File { get; set; }

		[YamlMember(Alias = "external", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? External { get; set; }

		[YamlMember(Alias = "content", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Content { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }
	}
}
