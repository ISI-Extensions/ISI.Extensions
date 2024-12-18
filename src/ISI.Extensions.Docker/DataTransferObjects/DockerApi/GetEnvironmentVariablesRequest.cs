using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public class GetEnvironmentVariablesRequest
	{
		public string ComposeDirectory { get; set; }

		public string[] EnvironmentFileFullNames { get; set; }
		public InvariantCultureIgnoreCaseStringDictionary<string> EnvironmentVariables { get; set; }
	}
}