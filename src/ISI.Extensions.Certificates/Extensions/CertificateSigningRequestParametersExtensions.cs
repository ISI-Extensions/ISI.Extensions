#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Security.Cryptography.X509Certificates;

namespace ISI.Extensions.Certificates.Extensions
{
	public static class CertificateSigningRequestParametersExtensions
	{
		public static string GetSubjectName(this CertificateSigningRequestParameters certificateSigningRequestParameters, string caName = null)
		{
			var parameters = new Dictionary<string, string>();

			void addParameter(string key, string value)
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					parameters.Add(key, value.IndexOf(",", StringComparison.InvariantCultureIgnoreCase) >= 0 ? $"[{value}]" : value);
				}
			}

			if (string.Equals(certificateSigningRequestParameters.CountryName ?? string.Empty, "United States", StringComparison.InvariantCultureIgnoreCase))
			{
				certificateSigningRequestParameters.CountryName = "US";
			}

			addParameter("C", certificateSigningRequestParameters.CountryName);
			addParameter("S", certificateSigningRequestParameters.State);
			addParameter("L", certificateSigningRequestParameters.Locality);
			addParameter("O", certificateSigningRequestParameters.Organization);
			addParameter("OU", certificateSigningRequestParameters.OrganizationUnit);
			addParameter("CN", (string.IsNullOrWhiteSpace(caName) ? certificateSigningRequestParameters.CommonName : caName));

			return string.Join(", ", parameters.Select(parameter => $"{parameter.Key}={parameter.Value}"));
		}

		public static (CertificateType CertificateType, string Certificate)[] ProcessCertificateSigningRequest(this CertificateSigningRequestParameters certificateSigningRequestParameters)
		{
			var certificates = new List<(CertificateType CertificateType, string Certificate)>();

			using (var certificateSigningKey = System.Security.Cryptography.RSA.Create(certificateSigningRequestParameters.KeySize))
			{
				var certificateSigningRequest = new System.Security.Cryptography.X509Certificates.CertificateRequest(
					certificateSigningRequestParameters.GetSubjectName(),
					certificateSigningKey,
					System.Security.Cryptography.HashAlgorithmName.SHA256,
					System.Security.Cryptography.RSASignaturePadding.Pkcs1);

				var subjectAlternativeNameBuilder = new System.Security.Cryptography.X509Certificates.SubjectAlternativeNameBuilder();
				subjectAlternativeNameBuilder.AddDnsName(certificateSigningRequestParameters.CommonName);

				if (certificateSigningRequestParameters.CommonName.StartsWith("*."))
				{
					subjectAlternativeNameBuilder.AddDnsName(certificateSigningRequestParameters.CommonName.TrimStart("*."));
				}

				if (certificateSigningRequestParameters.IpAddresses.NullCheckedAny())
				{
					foreach (var ipAddress in certificateSigningRequestParameters.IpAddresses)
					{
						subjectAlternativeNameBuilder.AddIpAddress(ipAddress);
					}
				}

				certificateSigningRequest.CertificateExtensions.Add(subjectAlternativeNameBuilder.Build());

				certificates.Add((CertificateType: ISI.Extensions.Certificates.CertificateType.CertificateSigningRequest, Certificate: certificateSigningRequest.CreateSigningRequestPem()));
				certificates.Add((CertificateType: ISI.Extensions.Certificates.CertificateType.Key, Certificate: certificateSigningKey.ExportRSAPrivateKeyPem()));
			}

			return certificates.ToArray();
		}
	}
}
