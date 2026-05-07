using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Docker.DataTransferObjects.DockerApi
{
	public interface IDeserializeComposeFileRequest
	{
	}

	public class DeserializeComposeFileRequest : IDeserializeComposeFileRequest
	{
		public string ComposeFileFullName { get; set; }
	}

	public class DeserializeComposeFileStreamRequest : IDeserializeComposeFileRequest
	{
		public System.IO.Stream ComposeFileStream { get; set; }
	}
}