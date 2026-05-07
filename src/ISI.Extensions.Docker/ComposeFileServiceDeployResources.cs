using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceDeployResources
	{
		public ComposeFileServiceDeployResourcesResourceSpec Limits { get; set; }
		public ComposeFileServiceDeployResourcesResourceSpec Reservations { get; set; }
	}
}
