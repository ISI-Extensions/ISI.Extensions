using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceBuild
	{
		[YamlMember(Alias = "context", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Context { get; set; }

		[YamlMember(Alias = "dockerfile", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Dockerfile { get; set; }

		[YamlMember(Alias = "args", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Args { get; set; }

		[YamlMember(Alias = "target", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Target { get; set; }

		[YamlMember(Alias = "cache_from", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] CacheFrom { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }
	}
}
