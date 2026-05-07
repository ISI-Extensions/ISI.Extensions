using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileVolume
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string Target { get; set; }
		public string Source { get; set; }
		public bool? ReadOnly { get; set; }
		public string Driver { get; set; }
		public IDictionary<string, string> DriverOpts { get; set; }
		public bool? External { get; set; }
		public IDictionary<string, string> Labels { get; set; }
	}
}
