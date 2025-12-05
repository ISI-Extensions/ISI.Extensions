using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.XenOrchestra.DataTransferObjects.XenOrchestraApi
{
	public class ActivateCertificateRequest : IRequest
	{
		public string XenOrchestraServer { get; set; }
		public string XenOrchestraUserName { get; set; }
		public string XenOrchestraPassword { get; set; }

		public byte[] KeyCertificate { get; set; }
		public byte[] BundleCertificate { get; set; }
	}
}