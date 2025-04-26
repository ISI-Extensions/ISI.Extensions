using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public class CalculateHttpTokenRequest : IRequest
	{
		public HostContext HostContext { get; set; }

		public string Domain { get; set; }

		public string ChallengeToken { get; set; }
	}
}