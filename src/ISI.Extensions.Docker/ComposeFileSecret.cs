using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileSecret
	{
		public string File { get; set; }
		public bool? External { get; set; }
		public string Name { get; set; }
		public IDictionary<string, string> Labels { get; set; }
	}
}
