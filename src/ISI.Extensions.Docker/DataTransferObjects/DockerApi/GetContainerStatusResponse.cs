using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public class GetContainerStatusResponse
	{
		public string Status { get; set; }

		public bool Errored { get; set; }
	}
}