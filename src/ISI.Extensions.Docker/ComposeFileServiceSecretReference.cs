using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceSecretReference
	{
		public string Source { get; set; }
		public string Target { get; set; }
		public int? Uid { get; set; }
		public int? Gid { get; set; }
		public int? Mode { get; set; }
	}
}
