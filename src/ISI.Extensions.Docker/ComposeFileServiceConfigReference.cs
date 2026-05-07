using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceConfigReference
	{
		public string Source { get; set; }
		public string Target { get; set; }
		public string Uid { get; set; }
		public string Gid { get; set; }
		public string UnixFileMode { get; set; }
	}
}
