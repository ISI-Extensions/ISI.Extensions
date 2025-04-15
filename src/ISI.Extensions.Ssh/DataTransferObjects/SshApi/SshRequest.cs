using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Ssh.DataTransferObjects.SshApi
{
	public class SshRequest
	{
		public string Destination { get; set; }
		public string Command { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}