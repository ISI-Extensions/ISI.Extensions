using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public class GetCertificateRequest : IRequest
	{
		public HostContext HostContext { get; set; }

		public string GetCertificatesUrl { get; set; }

		public bool GetPrivateKey { get; set; }

		public bool GetPrivateKeyPassword { get; set; }

		public bool GetPfxCertificate { get; set; }

		public bool GetPfxCertificatePassword { get; set; }
	}
}