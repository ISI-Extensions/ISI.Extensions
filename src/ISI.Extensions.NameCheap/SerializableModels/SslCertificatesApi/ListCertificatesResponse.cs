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
	public class ListCertificatesResponse
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
		public ListCertificatesResponseCommandResponse CommandResponse { get; set; }

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
	public class ListCertificatesResponseCommandResponse
	{
		[System.Xml.Serialization.XmlArray("SSLListResult")]
		[System.Xml.Serialization.XmlArrayItem("SSL", IsNullable = false)]
		public ListCertificatesResponseCommandResponseCertificate[] Certificates { get; set; }

		[System.Xml.Serialization.XmlElement("Paging")]
		public ApiResponsePaging Paging { get; set; }

		[System.Xml.Serialization.XmlAttribute("Type")]
		public string Type { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class ListCertificatesResponseCommandResponseCertificate
	{
		[System.Xml.Serialization.XmlAttribute("CertificateID")]
		public string VendorCertificateKey { get; set; }

		[System.Xml.Serialization.XmlAttribute("HostName")]
		public string HostName { get; set; }

		[System.Xml.Serialization.XmlAttribute("SSLType")]
		public string CertificateType { get; set; }

		[System.Xml.Serialization.XmlAttribute("PurchaseDate")]
		public string PurchaseDate { get; set; }

		[System.Xml.Serialization.XmlAttribute("ExpireDate")]
		public string ExpireDate { get; set; }

		[System.Xml.Serialization.XmlAttribute("ActivationExpireDate")]
		public string ActivationExpireDate { get; set; }

		[System.Xml.Serialization.XmlAttribute("IsExpiredYN")]
		public bool IsExpired { get; set; }

		[System.Xml.Serialization.XmlAttribute("Status")]
		public string Status { get; set; }
	}
}