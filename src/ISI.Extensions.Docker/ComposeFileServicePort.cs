using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServicePort
	{
		public int Target { get; set; }
		public int Published { get; set; }
		public string Protocol { get; set; }
		public string Mode { get; set; }

		public string RawValue { get; set; }
		public bool IsYaml { get; set; }
	}
}
