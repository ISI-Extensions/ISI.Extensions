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

namespace ISI.Extensions.GoDaddy.SerializableModels.DomainsApiV3
{
	[DataContract]
	public class ListDomainsResponse
	{
		[DataMember(Name = "items", EmitDefaultValue = false)]
		public ListDomainsResponseItem[] Domains { get; set; }

		[DataMember(Name = "links", EmitDefaultValue = false)]
		public ResponseLink[] Links { get; set; }
	}

	[DataContract]
	public class ListDomainsResponseItem
	{
		[DataMember(Name = "domain", EmitDefaultValue = false)]
		public string Domain { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string Status { get; set; }

		[DataMember(Name = "expiresAt", EmitDefaultValue = false)]
		public string __ExpiresDateTime { get => Expires.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => Expires = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Expires { get; set; }

		[DataMember(Name = "createdAt", EmitDefaultValue = false)]
		public string __CreatedDateTime { get => CreatedDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CreatedDateTime = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CreatedDateTime { get; set; }

		[DataMember(Name = "autoRenew", EmitDefaultValue = false)]
		public bool AutoRenew { get; set; }

		[DataMember(Name = "privacy", EmitDefaultValue = false)]
		public bool Privacy { get; set; }

		[DataMember(Name = "transferLock", EmitDefaultValue = false)]
		public bool TransferLock { get; set; }

		[DataMember(Name = "nameServers", EmitDefaultValue = false)]
		public string[] NameServers { get; set; }

		[DataMember(Name = "links", EmitDefaultValue = false)]
		public ResponseLink[] Links { get; set; }
	}
}