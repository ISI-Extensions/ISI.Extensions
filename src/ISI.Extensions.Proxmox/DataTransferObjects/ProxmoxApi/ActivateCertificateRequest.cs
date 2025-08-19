using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Proxmox.DataTransferObjects.ProxmoxApi
{
	public class ActivateCertificateRequest : IRequest
	{
		public string ProxmoxServer { get; set; }
		public string ProxmoxUserName { get; set; }
		public string ProxmoxPassword { get; set; }

		public byte[] KeyCertificate { get; set; }
		public byte[] BundleCertificate { get; set; }
	}
}