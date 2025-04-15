using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Ssh.DataTransferObjects.SshApi
{
	public class ScpResponse
	{
		public string Output { get; set; }
		public bool Errored { get; set; }
	}
}