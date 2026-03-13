using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.NameCheap.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.SslApi;
using SerializableModels = ISI.Extensions.NameCheap.SerializableModels.SslCertificatesApi;

namespace ISI.Extensions.NameCheap
{
	public partial class SslCertificatesApi
	{
		public DTOs.ListCertificatesResponse ListCertificates(DTOs.ListCertificatesRequest request)
		{
			var response = new DTOs.ListCertificatesResponse();

			var page = 1;

			var certificates = new List<DTOs.ListCertificatesResponseCertificate>();

			while (page > 0)
			{
				var uri = request.GetUrl(Configuration);
				uri.SetUserNameClientIp(request, IpifyApi, Configuration);
				uri.AddQueryStringParameter("Command", "namecheap.ssl.getList");
				uri.AddQueryStringParameter("Page", page);
				uri.AddQueryStringParameter("ListType", request.ListType.GetAbbreviation());
				if (!string.IsNullOrWhiteSpace(request.SearchTerm))
				{
					uri.AddQueryStringParameter("SearchTerm", request.SearchTerm);
				}

				var serviceResponse = ISI.Extensions.WebClient.Rest.ExecuteXmlGet<SerializableModels.SslCertificatesApi.ListCertificatesResponse>(uri.Uri, null, true);

				certificates.AddRange(serviceResponse?.CommandResponse?.Certificates?.ToNullCheckedArray(certificate => new DTOs.ListCertificatesResponseCertificate()
				{
					VendorCertificateKey = certificate.VendorCertificateKey,
					HostName = certificate.HostName,
					CertificateType = ISI.Extensions.Enum<NameCheapSslCertificateType>.Parse(certificate.CertificateType),
					PurchaseDate = certificate.PurchaseDate.ToDateTimeNullable(),
					ExpireDate = certificate.ExpireDate.ToDateTimeNullable(),
					ActivationExpireDate = certificate.ActivationExpireDate.ToDateTimeNullable(),
					IsExpired = certificate.IsExpired,
					Status = certificate.Status,
				}) ?? []);


				if (certificates.Count < (serviceResponse?.CommandResponse?.Paging?.TotalItems ?? 0))
				{
					page++;
				}
				else
				{
					page = 0;
				}
			}

			response.Certificates = certificates.ToArray();

			return response;
		}
	}
}