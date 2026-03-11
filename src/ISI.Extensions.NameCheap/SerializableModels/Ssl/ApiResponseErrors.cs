using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.NameCheap.SerializableModels.Ssl
{
	[System.Serializable]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class ApiResponseErrors
	{
		[System.Xml.Serialization.XmlElement("Error")]
		public ApiResponseErrorsError Error { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class ApiResponseErrorsError
	{
		[System.Xml.Serialization.XmlAttribute("Number")]
		public byte Number { get; set; }

		[System.Xml.Serialization.XmlText]
		public string Value { get; set; }
	}
}
