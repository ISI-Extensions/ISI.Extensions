using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceHealthCheck
	{
		[YamlMember(Alias = "test", ScalarStyle = YamlDotNet.Core.ScalarStyle.Folded, DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Test { get; set; }

		[YamlMember(Alias = "interval")]
		public string Interval { get; set; }

		[YamlMember(Alias = "timeout")]
		public string Timeout { get; set; }

		[YamlMember(Alias = "retries", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Retries { get; set; }

		[YamlMember(Alias = "start_period")]
		public string StartPeriod { get; set; }
	}
}
