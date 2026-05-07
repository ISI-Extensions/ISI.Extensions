using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeployResourcesResourceSpec
	{
		[YamlMember(Alias = "cpus", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Cpus { get; set; }

		[YamlMember(Alias = "memory", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Memory { get; set; }
	}
}
