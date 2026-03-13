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
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class ApiResponsePaging
	{
		[System.Xml.Serialization.XmlElement("TotalItems")]
		public int TotalItems { get; set; }

		[System.Xml.Serialization.XmlElement("CurrentPage")]
		public int CurrentPage { get; set; }

		[System.Xml.Serialization.XmlElement("PageSize")]
		public int PageSize { get; set; }
	}
}
