using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceUlimit
	{
		[YamlMember(Alias = "soft", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Soft { get; set; }

		[YamlMember(Alias = "hard", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Hard { get; set; }
	}
}
