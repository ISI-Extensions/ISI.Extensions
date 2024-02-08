using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public class GetCertificateResponse
	{
		public byte[] CertificatePfx { get; set; }
		public string CertificatePem { get; set; }
		public string CertificatePassword { get; set; }
	}
}