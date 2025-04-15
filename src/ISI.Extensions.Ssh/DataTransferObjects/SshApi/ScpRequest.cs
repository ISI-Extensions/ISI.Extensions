using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Ssh.DataTransferObjects.SshApi
{
	public class ScpRequest
	{
		public string Source { get; set; }
		public string Target { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}