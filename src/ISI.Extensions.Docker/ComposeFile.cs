using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFile
	{
		public string Name { get; set; }
		public string Version { get; set; }
		public ComposeFileService[] Services { get; set; }
		public ComposeFileNetwork[] Networks { get; set; }
		public ComposeFileVolume[] Volumes { get; set; }
		public ComposeFileSecret[] Secrets { get; set; }
		public ComposeFileConfig[] Configs { get; set; }
		public IDictionary<string, object> Extensions { get; set; }
	}
}
