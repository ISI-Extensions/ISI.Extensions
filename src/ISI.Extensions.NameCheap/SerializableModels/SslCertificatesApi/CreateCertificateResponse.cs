using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.NameCheap.SerializableModels.SslCertificatesApi
{
	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	[System.Xml.Serialization.XmlRoot(Namespace = "http://api.namecheap.com/xml.response", IsNullable = false)]
	public class CreateCertificateResponse
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
		public CreateCertificateResponseCommandResponse CommandResponse { get; set; }

		[System.Xml.Serialization.XmlElement("Server")]
		public string Server { get; set; }

		[System.Xml.Serialization.XmlElement("GMTTimeDifference")]
		public string GMTTimeDifference { get; set; }

		[System.Xml.Serialization.XmlElement("ExecutionTime")]
		public decimal ExecutionTime { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	public class CreateCertificateResponseCommandResponse
	{
		[System.Xml.Serialization.XmlElement("SSLCreateResult")]
		public CreateCertificateResponseCommandResponseSslCreateResult SslCreateResult { get; set; }

		[System.Xml.Serialization.XmlAttribute("Type")]
		public string Type { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	public class CreateCertificateResponseCommandResponseSslCreateResult
	{
		[System.Xml.Serialization.XmlElement("SSLCertificate")]
		public CreateCertificateResponseCommandResponseSSLCreateResultSSLCertificate SslCertificate { get; set; }

		[System.Xml.Serialization.XmlAttribute("IsSuccess")]
		public bool IsSuccess { get; set; }

		[System.Xml.Serialization.XmlAttribute("OrderId")]
		public string OrderId { get; set; }

		[System.Xml.Serialization.XmlAttribute("TransactionId")]
		public string TransactionId { get; set; }

		[System.Xml.Serialization.XmlAttribute("ChargedAmount")]
		public decimal ChargedAmount { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	public class CreateCertificateResponseCommandResponseSSLCreateResultSSLCertificate
	{
		[System.Xml.Serialization.XmlAttribute("CertificateID")]
		public string CertificateId { get; set; }

		[System.Xml.Serialization.XmlAttribute("Created")]
		public string Created { get; set; }

		[System.Xml.Serialization.XmlAttribute("SSLType")]
		public string SslType { get; set; }

		[System.Xml.Serialization.XmlAttribute("Years")]
		public int Years { get; set; }

		[System.Xml.Serialization.XmlAttribute("Status")]
		public string Status { get; set; }
	}
}