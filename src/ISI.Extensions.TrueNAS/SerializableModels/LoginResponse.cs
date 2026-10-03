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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.TrueNAS.SerializableModels
{
	[DataContract]
	public class LoginResponse
	{
		[DataMember(Name = "response_type", EmitDefaultValue = false)]
		public string ResponseType { get; set; }

		[DataMember(Name = "user_info", EmitDefaultValue = false)]
		public LoginResponseUserInfo UserInfo { get; set; }

		[DataMember(Name = "authenticator", EmitDefaultValue = false)]
		public string Authenticator { get; set; }
	}

	[DataContract]
	public class LoginResponseUserInfo
	{
		[DataMember(Name = "pw_name", EmitDefaultValue = false)]
		public string PwName { get; set; }

		[DataMember(Name = "pw_gecos", EmitDefaultValue = false)]
		public string PwGecos { get; set; }

		[DataMember(Name = "pw_dir", EmitDefaultValue = false)]
		public string PwDir { get; set; }

		[DataMember(Name = "pw_shell", EmitDefaultValue = false)]
		public string PwShell { get; set; }

		[DataMember(Name = "pw_uid", EmitDefaultValue = false)]
		public int? PwUid { get; set; }

		[DataMember(Name = "pw_gid", EmitDefaultValue = false)]
		public int? PwGid { get; set; }

		[DataMember(Name = "grouplist", EmitDefaultValue = false)]
		public int[] GroupList { get; set; }

		[DataMember(Name = "sid", EmitDefaultValue = false)]
		public string Sid { get; set; }

		[DataMember(Name = "source", EmitDefaultValue = false)]
		public string Source { get; set; }

		[DataMember(Name = "local", EmitDefaultValue = false)]
		public bool? Local { get; set; }

		[DataMember(Name = "attributes", EmitDefaultValue = false)]
		public object Attributes { get; set; }

		[DataMember(Name = "two_factor_config", EmitDefaultValue = false)]
		public object TwoFactorConfig { get; set; }

		[DataMember(Name = "privilege", EmitDefaultValue = false)]
		public object Privilege { get; set; }

		[DataMember(Name = "account_attributes", EmitDefaultValue = false)]
		public string[] AccountAttributes { get; set; }
	}
}