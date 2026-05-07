using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceServiceDependency
	{
		[YamlMember(Alias = "condition", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Condition { get; set; }
	}
}
