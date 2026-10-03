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

namespace ISI.Extensions.TrueNAS.DataTransferObjects.TrueNASApi
{
	public class GetSystemConfigGeneralResponse
	{
		public int SystemConfigGeneralId { get; set; }
		public GetSystemConfigGeneralResponseCertificate Certificate { get; set; }
		public int UiHttpsPort { get; set; }
		public bool UiHttpsRedirect { get; set; }
		public string[] UiHttpsProtocols { get; set; }
		public int UiPort { get; set; }
		public string[] UiAddress { get; set; }
		public string[] UiV6Address { get; set; }
		public bool UiConsoleMessage { get; set; }
		public string UiXFrameOptions { get; set; }
		public string Kbdmap { get; set; }
		public string Timezone { get; set; }
		public bool UsageCollection { get; set; }
		public bool WizardShown { get; set; }
		public bool UsageCollectionIsSet { get; set; }
		public bool DsAuth { get; set; }
	}

	public class GetSystemConfigGeneralResponseCertificate
	{
		public int CertificateId { get; set; }
		public int Type { get; set; }
		public string CertificateName { get; set; }
		public string Certificate { get; set; }
		public string PrivateKey { get; set; }
		public object Csr { get; set; }
		public int RenewDays { get; set; }
		public bool AddToTrustedStore { get; set; }
		public string RootPath { get; set; }
		public string CertificatePath { get; set; }
		public string PrivateKeyPath { get; set; }
		public string CertType { get; set; }
		public bool CertTypeExisting { get; set; }
		public bool CertTypeCsr { get; set; }
		public bool CertTypeCa { get; set; }
		public string[] ChainList { get; set; }
		public int KeyLength { get; set; }
		public string KeyType { get; set; }
		public string Common { get; set; }
		public string[] San { get; set; }
		public string Dn { get; set; }
		public string SubjectNameHash { get; set; }
		public GetSystemConfigGeneralResponseExtensions Extensions { get; set; }
		public string DigestAlgorithm { get; set; }
		public int Lifetime { get; set; }
		public string From { get; set; }
		public string Until { get; set; }
		public string Serial { get; set; }
		public bool Chain { get; set; }
		public string Fingerprint { get; set; }
		public bool Expired { get; set; }
		public bool Parsed { get; set; }
	}

	public class GetSystemConfigGeneralResponseExtensions
	{
		public string AuthorityKeyIdentifier { get; set; }
		public string SubjectKeyIdentifier { get; set; }
		public string KeyUsage { get; set; }
		public string BasicConstraints { get; set; }
		public string ExtendedKeyUsage { get; set; }
		public string CertificatePolicies { get; set; }
		public string AuthorityInfoAccess { get; set; }
		public string SubjectAltName { get; set; }
		public string CtPrecertScts { get; set; }
	}
}