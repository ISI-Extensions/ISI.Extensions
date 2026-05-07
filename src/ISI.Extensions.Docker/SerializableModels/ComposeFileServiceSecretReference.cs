using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceSecretReference
	{
		[YamlMember(Alias = "source", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Source { get; set; }

		[YamlMember(Alias = "target", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Target { get; set; }

		[YamlMember(Alias = "uid", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Uid { get; set; }

		[YamlMember(Alias = "gid", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Gid { get; set; }

		[YamlMember(Alias = "mode", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Mode { get; set; }
	}
}
