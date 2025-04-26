using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public class CalculateHttpTokenResponse
	{
		public string Domain { get; set; }
		public string Url { get; set; }

		public string HttpToken { get; internal set; }
	}
}