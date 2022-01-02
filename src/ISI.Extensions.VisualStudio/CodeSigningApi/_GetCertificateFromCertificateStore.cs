#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi
	{
		private System.Security.Cryptography.X509Certificates.X509Certificate2 GetCertificateFromCertificateStore(string certificateStoreName, string certificateStoreLocation, string certificateSubjectName, string certificateFingerprint)
		{
			var storeName = ISI.Extensions.Enum<System.Security.Cryptography.X509Certificates.StoreName?>.Parse(certificateStoreName);
			var storeLocation = ISI.Extensions.Enum<System.Security.Cryptography.X509Certificates.StoreLocation?>.Parse(certificateStoreLocation);

			if (storeName.HasValue)
			{
				if (storeLocation.HasValue)
				{
					using (var store = new System.Security.Cryptography.X509Certificates.X509Store(storeName.Value, storeLocation.Value))
					{
						store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.OpenExistingOnly | System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);

						if (!string.IsNullOrWhiteSpace(certificateSubjectName))
						{
							var certificates = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectName, certificateSubjectName, false);

							if (certificates.Count > 0)
							{
								return certificates[0];
							}
						}

						if (!string.IsNullOrWhiteSpace(certificateFingerprint))
						{
							var certificates = store.Certificates.Find(System.Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, certificateFingerprint, false);

							if (certificates.Count > 0)
							{
								return certificates[0];
							}
						}
					}
				}
				else
				{
					throw new Exception(string.Format("StoreLocation: \"{0}\" not recognized", certificateStoreLocation));
				}
			}
			else
			{
				throw new Exception(string.Format("StoreName: \"{0}\" not recognized", certificateStoreName));
			}

			throw new Exception("Certificate not found");
		}
	}
}