using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Svn.SerializableModels.SvnCli
{
	[ISI.Extensions.Serialization.PreferredSerializerXml]
	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	[System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false, ElementName = "log")]
	public class Log
	{
		[System.Xml.Serialization.XmlElement("logentry")]
		public LogLogEntry LogEntry { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class LogLogEntry
	{
		[System.Xml.Serialization.XmlElement("author")]
		public string Author { get; set; }

		[System.Xml.Serialization.XmlElement("date")]
		public System.DateTime MessageDateTimeUtc { get; set; }

		[System.Xml.Serialization.XmlElement("msg")]
		public string Message { get; set; }

		[System.Xml.Serialization.XmlAttribute("revision")]
		public uint Revision { get; set; }
	}
}
