using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.PfSense.DataTransferObjects.PfSenseApi
{
	public class ActivateCertificateRequest : IRequest
	{
		public string PfSenseServer { get; set; }
		public string PfSenseUserName { get; set; }
		public string PfSensePassword { get; set; }

		public string CertificateName { get; set; }
		public byte[] KeyCertificate { get; set; }
		public byte[] BundleCertificate { get; set; }
	}
}