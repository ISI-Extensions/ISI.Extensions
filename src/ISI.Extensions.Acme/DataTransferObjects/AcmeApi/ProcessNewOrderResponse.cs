using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public class ProcessNewOrderResponse
	{
		public string PrivateKeyPem { get; set; }
		public string CertificatePem { get; set; }
	}
}