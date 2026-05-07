using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceDeployPlacement
	{
		public string[] Constraints { get; set; }
		public IDictionary<string, string>[] Preferences { get; set; }
	}
}
