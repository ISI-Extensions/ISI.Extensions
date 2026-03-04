using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public class CopyFromImageRequest : IRequestHostContext
	{
		public string AppDirectory { get; set; }

		public string Host { get; set; }
		public string Context { get; set; }

		public string[] EnvironmentFileFullNames { get; set; }
		public string[] EnvironmentFileContents { get; set; }
		public InvariantCultureIgnoreCaseStringDictionary<string> EnvironmentVariables { get; set; }

		public string ContainerRegistry { get; set; }
		public string ContainerRepository { get; set; }
		public string ContainerImageTag { get; set; }

		public IEnumerable<(string Source, string Target)> CopyFiles { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}