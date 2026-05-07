using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceHealthCheck
	{
		public string[] Test { get; set; }
		public string Interval { get; set; }
		public string Timeout { get; set; }
		public int? Retries { get; set; }
		public string StartPeriod { get; set; }
	}
}
