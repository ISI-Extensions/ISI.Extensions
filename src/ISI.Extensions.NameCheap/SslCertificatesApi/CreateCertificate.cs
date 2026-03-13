using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.NameCheap.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.SslApi;

namespace ISI.Extensions.NameCheap
{
	public partial class SslCertificatesApi
	{
		public DTOs.CreateCertificateResponse CreateCertificate(DTOs.CreateCertificateRequest request)
		{
			var response = new DTOs.CreateCertificateResponse();
			
			
			return response;
		}
	}
}