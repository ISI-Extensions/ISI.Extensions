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

namespace ISI.Extensions.Certificates.Extensions
{
	public static class X509Certificate2Extensions
	{
		public static bool IsPrivateKeyExportable(this System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
		{
			if (certificate?.HasPrivateKey ?? false)
			{
				if (certificate.PrivateKey is System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoServiceProvider)
				{
					return rsaCryptoServiceProvider.CspKeyContainerInfo.Exportable;
				}

				if (certificate.PrivateKey is System.Security.Cryptography.DSACryptoServiceProvider dsaCryptoServiceProvider)
				{
					return dsaCryptoServiceProvider.CspKeyContainerInfo.Exportable;
				}

				if (certificate.PrivateKey is System.Security.Cryptography.RSACng rsaCng)
				{
					return rsaCng.Key.ExportPolicy.HasFlag(System.Security.Cryptography.CngExportPolicies.AllowExport);
				}

				if (certificate.PrivateKey is System.Security.Cryptography.ECDsaCng ecdsaCng)
				{
					return ecdsaCng.Key.ExportPolicy.HasFlag(System.Security.Cryptography.CngExportPolicies.AllowExport);
				}
			}

			return false;
		}

		public static string GetBundlePem(this System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
		{
			var bundlePemBuilder = new StringBuilder();
			using (var chain = new System.Security.Cryptography.X509Certificates.X509Chain())
			{
				if (chain.Build(certificate))
				{
					var chainElements = chain.ChainElements.ToNullCheckedArray(ISI.Extensions.Extensions.NullCheckCollectionResult.Empty);

					for (var chainElementIndex = 0; chainElementIndex < chainElements.Length - 1; chainElementIndex++)
					{
						bundlePemBuilder.AppendLine(chainElements[chainElementIndex].Certificate.ExportCertificatePem());
					}
				}
			}

			return bundlePemBuilder.ToString();
		}
	}
}
