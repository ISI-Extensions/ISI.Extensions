using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileNetwork
	{
		[YamlMember(Alias = "name", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Name { get; set; }

		[YamlMember(Alias = "driver", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Driver { get; set; }

		[YamlMember(Alias = "driver_opts", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> DriverOpts { get; set; }

		[YamlMember(Alias = "external", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? External { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }

		[YamlMember(Alias = "ipam", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileNetworkIpam Ipam { get; set; }

		[YamlMember(Alias = "attachable", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? Attachable { get; set; }

		[YamlMember(Alias = "ingress", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? Ingress { get; set; }

		[YamlMember(Alias = "internal", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? Internal { get; set; }
	}
}
