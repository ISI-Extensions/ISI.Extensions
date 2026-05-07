using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public interface IYamlMetaData
	{
		string RawValue { get; }
		bool IsYaml { get; }
		int? StartIndex { get; }
		int? EndIndex { get; }
	}
}
