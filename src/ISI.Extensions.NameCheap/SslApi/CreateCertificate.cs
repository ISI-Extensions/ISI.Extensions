using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.SslApi;

namespace ISI.Extensions.NameCheap
{
	public partial class SslApi
	{
		public DTOs.CreateCertificateResponse CreateCertificate(DTOs.CreateCertificateRequest request)
		{
			var response = new DTOs.CreateCertificateResponse();
			
			
			return response;
		}
	}
}