using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileNetworkIpam
	{
		[YamlMember(Alias = "driver", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Driver { get; set; }

		[YamlMember(Alias = "config", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string>[] Config { get; set; }

		[YamlMember(Alias = "options", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Options { get; set; }
	}
}
