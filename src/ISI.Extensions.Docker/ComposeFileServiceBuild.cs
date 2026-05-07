using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceBuild
	{
		public string Context { get; set; }
		public string Dockerfile { get; set; }
		public IDictionary<string, string> Args { get; set; }
		public string Target { get; set; }
		public string[] CacheFrom { get; set; }
		public IDictionary<string, string> Labels { get; set; }
	}
}
