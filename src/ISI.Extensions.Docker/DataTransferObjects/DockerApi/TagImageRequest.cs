using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public class TagImageRequest
	{
		public string Host { get; set; }
		public string Context { get; set; }

		public string SourceContainerRegistry { get; set; }
		public string SourceContainerRepository { get; set; }
		public string SourceContainerImageTag { get; set; }

		public string TargetContainerRegistry { get; set; }
		public string TargetContainerRepository { get; set; }
		public string TargetContainerImageTag { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}