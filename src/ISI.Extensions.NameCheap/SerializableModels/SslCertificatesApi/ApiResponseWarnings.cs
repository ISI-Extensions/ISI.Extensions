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
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class ApiResponseWarnings
	{
		[System.Xml.Serialization.XmlElement("Warning")]
		public ApiResponseWarningsWarning Warning { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class ApiResponseWarningsWarning
	{
		[System.Xml.Serialization.XmlAttribute("Number")]
		public byte Number { get; set; }

		[System.Xml.Serialization.XmlText]
		public string Value { get; set; }
	}
}
