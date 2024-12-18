using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public class GetEnvironmentVariablesResponse
	{
		public TempEnvironmentVariablesFiles EnvironmentVariables { get; set; }
	}
}