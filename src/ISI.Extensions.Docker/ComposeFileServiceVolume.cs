using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceVolume
	{
		public string Type { get; set; }
		public string Target { get; set; }
		public string Source { get; set; }
		public bool? ReadOnly { get; set; }
		public string Driver { get; set; }
		public Dictionary<string, string> DriverOpts { get; set; }
		public bool? External { get; set; }
		public Dictionary<string, string> Labels { get; set; }
	}
}
