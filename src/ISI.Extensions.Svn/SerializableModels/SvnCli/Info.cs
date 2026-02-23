using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Svn.SerializableModels.SvnCli
{
	[ISI.Extensions.Serialization.PreferredSerializerXml]
	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	[System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false, ElementName = "info")]
	public class Info
	{
		[System.Xml.Serialization.XmlElement("entry")]
		public InfoEntry[] Entries { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class InfoEntry
	{
		[System.Xml.Serialization.XmlElement("url")]
		public string Url { get; set; }

		[System.Xml.Serialization.XmlElement("relative-url")]
		public string RelativeUrl { get; set; }

		[System.Xml.Serialization.XmlElement("repository")]
		public InfoEntryRepository Repository { get; set; }

		[System.Xml.Serialization.XmlElement("wc-info")]
		public InfoEntryWorkingCopyInfo WorkingCopyInfo { get; set; }

		[System.Xml.Serialization.XmlElement("commit")]
		public InfoEntryCommit Commit { get; set; }

		[System.Xml.Serialization.XmlAttribute("path")]
		public string Path { get; set; }

		[System.Xml.Serialization.XmlAttribute("revision")]
		public string Revision { get; set; }

		[System.Xml.Serialization.XmlAttribute("kind")]
		public string Kind { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class InfoEntryRepository
	{
		[System.Xml.Serialization.XmlElement("root")]
		public string RepositoryRoot { get; set; }

		[System.Xml.Serialization.XmlElement("uuid")]
		public string RepositoryUuid { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class InfoEntryWorkingCopyInfo
	{
		[System.Xml.Serialization.XmlElement("wcroot-abspath")]
		public string WorkingCopyRootAbsolutePath { get; set; }

		[System.Xml.Serialization.XmlElement("schedule")]
		public string Schedule { get; set; }

		[System.Xml.Serialization.XmlElement("depth")]
		public string Depth { get; set; }

		[System.Xml.Serialization.XmlElement("text-updated")]
		public System.DateTime? TextUpdated { get; set; }

		[System.Xml.Serialization.XmlElement("checksum")]
		public string CheckSum { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true)]
	public class InfoEntryCommit
	{
		[System.Xml.Serialization.XmlElement("author")]
		public string Author { get; set; }

		[System.Xml.Serialization.XmlElement("date")]
		public System.DateTime CommitDateTimeUtc { get; set; }

		[System.Xml.Serialization.XmlAttribute("revision")]
		public string Revision { get; set; }
	}
}

