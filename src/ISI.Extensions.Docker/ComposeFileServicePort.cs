using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServicePort : IYamlMetaData
	{
		public int Target { get; set; }
		public int Published { get; set; }
		public string Protocol { get; set; }
		public string Mode { get; set; }

		internal string RawValue { get; set; }
		string IYamlMetaData.RawValue => RawValue;

		internal bool IsYaml { get; set; }
		bool IYamlMetaData.IsYaml => IsYaml;

		internal int? StartIndex { get; set; }
		int? IYamlMetaData.StartIndex => StartIndex;

		internal int? EndIndex { get; set; }
		int? IYamlMetaData.EndIndex => EndIndex;
	}
}
