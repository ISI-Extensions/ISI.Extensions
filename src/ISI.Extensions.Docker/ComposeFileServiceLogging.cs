using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceLogging
	{
		public string Driver { get; set; }
		public IDictionary<string, string> Options { get; set; }
	}
}
