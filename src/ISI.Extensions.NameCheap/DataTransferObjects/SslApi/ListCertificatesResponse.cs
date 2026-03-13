using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.NameCheap.DataTransferObjects.SslApi
{
	public class ListCertificatesResponse
	{
		public IEnumerable<ListCertificatesResponseCertificate> Certificates { get; set; }
	}

	public class ListCertificatesResponseCertificate
	{
		public string VendorCertificateKey { get; set; }
		public string HostName { get; set; }
		public NameCheapSslCertificateType CertificateType { get; set; }
		public DateTime? PurchaseDate { get; set; }
		public DateTime? ExpireDate { get; set; }
		public DateTime? ActivationExpireDate { get; set; }
		public bool IsExpired { get; set; }
		public string Status { get; set; }
	}
}