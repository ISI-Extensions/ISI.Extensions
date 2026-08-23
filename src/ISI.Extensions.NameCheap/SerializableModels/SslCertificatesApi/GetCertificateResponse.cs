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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.NameCheap.SerializableModels.SslCertificatesApi
{
	[ISI.Extensions.Serialization.PreferredSerializerXml]
	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	[System.Xml.Serialization.XmlRoot(ElementName = "ApiResponse", Namespace = "http://api.namecheap.com/xml.response", IsNullable = false)]
	public class GetCertificateResponse
	{
		[System.Xml.Serialization.XmlAttribute("Status")]
		public string Status { get; set; }

		[System.Xml.Serialization.XmlElement("Errors")]
		public ApiResponseErrors Errors { get; set; }

		[System.Xml.Serialization.XmlElement("Warnings")]
		public ApiResponseWarnings Warnings { get; set; }

		[System.Xml.Serialization.XmlElement("RequestedCommand")]
		public string RequestedCommand { get; set; }

		[System.Xml.Serialization.XmlElement("CommandResponse")]
		public GetCertificateResponseCommandResponse CommandResponse { get; set; }

		[System.Xml.Serialization.XmlElement("Server")]
		public string Server { get; set; }

		[System.Xml.Serialization.XmlElement("GMTTimeDifference")]
		public string GMTTimeDifference { get; set; }

		[System.Xml.Serialization.XmlElement("ExecutionTime")]
		public decimal ExecutionTime { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class GetCertificateResponseCommandResponse
	{
		[System.Xml.Serialization.XmlElement("SSLGetInfoResult")]
		public GetCertificateResponseCommandResponseGetInfoResult SSLGetInfoResult { get; set; }

		[System.Xml.Serialization.XmlAttribute("Type")]
		public string Type { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class GetCertificateResponseCommandResponseGetInfoResult
	{
		[System.Xml.Serialization.XmlElement("CertificateDetails")]
		public GetCertificateResponseCommandResponseGetInfoResultCertificateDetails CertificateDetails { get; set; }

		[System.Xml.Serialization.XmlElement("Provider")]
		public GetCertificateResponseCommandResponseGetInfoResultProvider Provider { get; set; }

		[System.Xml.Serialization.XmlAttribute("Status")]
		public string Status { get; set; }

		[System.Xml.Serialization.XmlAttribute("StatusDescription")]
		public string StatusDescription { get; set; }

		[System.Xml.Serialization.XmlAttribute("Type")]
		public string Type { get; set; }

		[System.Xml.Serialization.XmlAttribute("IssuedOn")]
		public string IssuedOn { get; set; }

		[System.Xml.Serialization.XmlAttribute("Years")]
		public string Years { get; set; }

		[System.Xml.Serialization.XmlAttribute("Expires")]
		public string Expires { get; set; }

		[System.Xml.Serialization.XmlAttribute("ActivationExpireDate")]
		public string ActivationExpireDate { get; set; }

		[System.Xml.Serialization.XmlAttribute("OrderId")]
		public int OrderId { get; set; }

		[System.Xml.Serialization.XmlAttribute("ReplacedBy")]
		public int ReplacedBy { get; set; }

		[System.Xml.Serialization.XmlAttribute("validationType")]
		public string ValidationType { get; set; }

		[System.Xml.Serialization.XmlAttribute("SANSCount")]
		public byte SANSCount { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class GetCertificateResponseCommandResponseGetInfoResultCertificateDetails
	{
		[System.Xml.Serialization.XmlElement("CSR")]
		public string CSR { get; set; }

		[System.Xml.Serialization.XmlElement("ApproverEmail")]
		public string ApproverEmail { get; set; }

		[System.Xml.Serialization.XmlElement("CommonName")]
		public string CommonName { get; set; }

		[System.Xml.Serialization.XmlElement("AdministratorName")]
		public string AdministratorName { get; set; }

		[System.Xml.Serialization.XmlElement("AdministratorEmail")]
		public string AdministratorEmail { get; set; }

		[System.Xml.Serialization.XmlElement("Certificates")]
		public GetCertificateResponseCommandResponseGetInfoResultCertificateDetailsCertificates Certificates { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class GetCertificateResponseCommandResponseGetInfoResultCertificateDetailsCertificates
	{
		[System.Xml.Serialization.XmlElement("Certificate")]
		public string Certificate { get; set; }

		[System.Xml.Serialization.XmlArray("CaCertificates")]
		[System.Xml.Serialization.XmlArrayItem("Certificate", IsNullable = false)]
		public GetCertificateResponseCommandResponseGetInfoResultCertificateDetailsCertificatesCertificate[] CaCertificates { get; set; }

		[System.Xml.Serialization.XmlAttribute("CertificateReturned")]
		public bool CertificateReturned { get; set; }

		[System.Xml.Serialization.XmlAttribute("ReturnType")]
		public string ReturnType { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class GetCertificateResponseCommandResponseGetInfoResultCertificateDetailsCertificatesCertificate
	{
		[System.Xml.Serialization.XmlElement("Certificate")]
		public string Certificate { get; set; }

		[System.Xml.Serialization.XmlAttribute("Type")]
		public string Type { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class GetCertificateResponseCommandResponseGetInfoResultProvider
	{
		[System.Xml.Serialization.XmlElement("OrderID")]
		public int OrderId { get; set; }

		[System.Xml.Serialization.XmlElement("Name")]
		public string Name { get; set; }
	}
}