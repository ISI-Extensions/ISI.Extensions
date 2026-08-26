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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Certificates
{
	public class CertificateSigningRequestParameters
	{
		public int KeySize { get; set; } = 4096;

		public string CountryName { get; set; }

		public string State { get; set; }

		public string Locality { get; set; }

		public string Organization { get; set; }

		public string OrganizationUnit { get; set; }

		public string CommonName { get; set; }

		public System.Net.IPAddress[] IpAddresses { get; set; }

		public string EmailAddress { get; set; }

		public CertificateSigningRequestParameters Clone()
		{
			return new CertificateSigningRequestParameters()
			{
				KeySize = KeySize,
				CountryName = CountryName,
				State = State,
				Locality = Locality,
				Organization = Organization,
				OrganizationUnit = OrganizationUnit,
				CommonName = CommonName,
				IpAddresses = IpAddresses.ToNullCheckedArray(ipAddress => System.Net.IPAddress.Parse(ipAddress.ToString())),
				EmailAddress = EmailAddress,
			};
		}

		public static CertificateSigningRequestParameters Parse(string pem)
		{
			var certificateRequest = System.Security.Cryptography.X509Certificates.CertificateRequest.LoadSigningRequestPem(pem, System.Security.Cryptography.HashAlgorithmName.SHA256);

			var certificateSigningRequestParameters = new CertificateSigningRequestParameters();

			var subject = certificateRequest.SubjectName.Name;
			while (!string.IsNullOrWhiteSpace(subject))
			{
				var pieces = subject.Split('=', 2);
				var key = pieces[0].Trim();
				subject = pieces[1].Trim();

				pieces = (subject.StartsWith("[") ? subject.TrimStart('[').Split(']', 2) : subject.Split(',', 2));
				var value = pieces[0].TrimEnd(']').Trim();

				subject = pieces[1].Trim();

				if (string.Equals(key, "C", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.CountryName = value;
				}
				else if(string.Equals(key, "S", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.State = value;
				}
				else if(string.Equals(key, "ST", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.State = value;
				}
				else if(string.Equals(key, "L", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.Locality = value;
				}
				else if (string.Equals(key, "C", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.CountryName = value;
				}
				else if (string.Equals(key, "O", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.Organization = value;
				}
				else if (string.Equals(key, "OU", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.OrganizationUnit = value;
				}
				else if (string.Equals(key, "CN", StringComparison.InvariantCultureIgnoreCase))
				{
					certificateSigningRequestParameters.CommonName = value;
				}
			}

			return certificateSigningRequestParameters;
		}
	}
}
