#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Text;
using System.Runtime.Serialization;

namespace ISI.Extensions.NameCheap.SerializableModels
{
	[DataContract]
	public class SetDnsRecordsRequest
	{
		[DataMember(Name = "authDetails", EmitDefaultValue = false)]
		public SetDnsRecordsRequestAuthDetails AuthDetails { get; set; }

		[DataMember(Name = "request", EmitDefaultValue = false)]
		public SetDnsRecordsRequestDnsRecords DnsRecords { get; set; }
	}

	[DataContract]
	public class SetDnsRecordsRequestAuthDetails
	{
		[DataMember(Name = "ParentUserType", EmitDefaultValue = false)]
		public string ParentUserType { get; set; }

		[DataMember(Name = "ParentUserId", EmitDefaultValue = false)]
		public int ParentUserId { get; set; }

		[DataMember(Name = "UserId", EmitDefaultValue = false)]
		public string UserId { get; set; }

		[DataMember(Name = "UserName", EmitDefaultValue = false)]
		public string UserName { get; set; }

		[DataMember(Name = "ClientIp", EmitDefaultValue = false)]
		public string ClientIp { get; set; }

		[DataMember(Name = "EndUserIp", EmitDefaultValue = false)]
		public string EndUserIp { get; set; }

		[DataMember(Name = "AdminUserName", EmitDefaultValue = false)]
		public string AdminUserName { get; set; }

		[DataMember(Name = "DisableSecurityNotification", EmitDefaultValue = false)]
		public bool DisableSecurityNotification { get; set; }

		[DataMember(Name = "AllowWhenDomainLocked", EmitDefaultValue = false)]
		public bool AllowWhenDomainLocked { get; set; }

		[DataMember(Name = "ProceedWhenDomainLockedFlag", EmitDefaultValue = false)]
		public bool ProceedWhenDomainLockedFlag { get; set; }

		[DataMember(Name = "DefaultChargeForUserName", EmitDefaultValue = false)]
		public string DefaultChargeForUserName { get; set; }

		[DataMember(Name = "Roles", EmitDefaultValue = false)]
		public string[] Roles { get; set; }
	}

	[DataContract]
	public class SetDnsRecordsRequestDnsRecords
	{
		[DataMember(Name = "RequestValues", EmitDefaultValue = false)]
		public SetDnsRecordsRequestDnsRecordKeyValue[] DnsRecordKeyValues { get; set; }

		[DataMember(Name = "SLD", EmitDefaultValue = false)]
		public string SLD { get; set; }

		[DataMember(Name = "TLD", EmitDefaultValue = false)]
		public string TLD { get; set; }
	}

	[DataContract]
	public class SetDnsRecordsRequestDnsRecordKeyValue
	{
		[DataMember(Name = "Key", EmitDefaultValue = false)]
		public string Key { get; set; }

		[DataMember(Name = "Value", EmitDefaultValue = false)]
		public string Value { get; set; }
	}
}