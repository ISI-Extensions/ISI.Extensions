using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceDeployUpdateConfig
	{
		public int? Parallelism { get; set; }
		public string Delay { get; set; }
		public string FailureAction { get; set; }
		public string Monitor { get; set; }
		public string MaxFailureRatio { get; set; }
		public string Order { get; set; }
	}
}
