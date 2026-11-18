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

namespace ISI.Extensions.NameCheap.SerializableModels.DomainsApi
{
	[ISI.Extensions.Serialization.PreferredSerializerXml]
	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	[System.Xml.Serialization.XmlRoot(ElementName = "ApiResponse", Namespace = "http://api.namecheap.com/xml.response", IsNullable = false)]
	public class ListDomainsResponse
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
		public ListDomainsResponseCommandResponse CommandResponse { get; set; }

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
	public class ListDomainsResponseCommandResponse
	{
		[System.Xml.Serialization.XmlArray("DomainGetListResult")]
		[System.Xml.Serialization.XmlArrayItem("Domain", IsNullable = false)]
		public ListDomainsResponseCommandResponseDomain[] Domains { get; set; }

		[System.Xml.Serialization.XmlElement("Paging")]
		public ApiResponsePaging Paging { get; set; }

		[System.Xml.Serialization.XmlAttribute("Type")]
		public string Type { get; set; }
	}

	[System.Serializable]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://api.namecheap.com/xml.response")]
	public class ListDomainsResponseCommandResponseDomain
	{
		[System.Xml.Serialization.XmlAttribute("ID")]
		public string ID { get; set; }

		[System.Xml.Serialization.XmlAttribute("Name")]
		public string Name { get; set; }

		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string User { get; set; }

		[System.Xml.Serialization.XmlAttribute("Created")]
		public string Created { get; set; }

		[System.Xml.Serialization.XmlAttribute("Expires")]
		public string Expires { get; set; }

		[System.Xml.Serialization.XmlAttribute("IsExpired")]
		public bool IsExpired { get; set; }

		[System.Xml.Serialization.XmlAttribute("IsLocked")]
		public bool IsLocked { get; set; }

		[System.Xml.Serialization.XmlAttribute("AutoRenew")]
		public bool AutoRenew { get; set; }

		[System.Xml.Serialization.XmlAttribute("WhoisGuard")]
		public string WhoisGuard { get; set; }

		[System.Xml.Serialization.XmlAttribute("IsPremium")]
		public bool IsPremium { get; set; }

		[System.Xml.Serialization.XmlAttribute("IsOurDNS")]
		public bool IsOurDNS { get; set; }
	}
}