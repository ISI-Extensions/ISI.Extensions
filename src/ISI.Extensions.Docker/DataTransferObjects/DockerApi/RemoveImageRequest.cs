using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public class RemoveImageRequest
	{
		public string AppDirectory { get; set; }

		public string Context { get; set; }

		public string ContainerImageTag { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}