using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceDeployRestartPolicy
	{
		public string Condition { get; set; }
		public string Delay { get; set; }
		public int? MaxAttempts { get; set; }
		public string Window { get; set; }
	}
}
