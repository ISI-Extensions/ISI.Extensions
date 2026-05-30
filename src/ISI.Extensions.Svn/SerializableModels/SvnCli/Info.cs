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

