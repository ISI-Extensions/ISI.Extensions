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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Ngrok.SerializableModels.ClientApi
{
	[DataContract]
	public class StartTunnelRequest
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string TunnelName { get; set; }

		[DataMember(Name = "proto", EmitDefaultValue = false)]
		public string __TunnelProtocol
		{
			get => TunnelProtocol.GetAbbreviation();
			set => TunnelProtocol = ISI.Extensions.Enum<Ngrok.SerializableModels.ClientApi.TunnelProtocol>.Parse(value);
		}
		[IgnoreDataMember]
		public Ngrok.SerializableModels.ClientApi.TunnelProtocol TunnelProtocol { get; set; }

		[DataMember(Name = "hostname", EmitDefaultValue = false)]
		public string Hostname { get; set; }

		[DataMember(Name = "subdomain", EmitDefaultValue = false)]
		public string Subdomain { get; set; }

		[DataMember(Name = "addr", EmitDefaultValue = false)]
		public string LocalAddress { get; set; }

		[DataMember(Name = "host_header", EmitDefaultValue = false)]
		public string HostHeader { get; set; }

		[DataMember(Name = "remote_addr", EmitDefaultValue = false)]
		public string RemoteAddress { get; set; }

		[DataMember(Name = "inspect", EmitDefaultValue = false)]
		public bool Inspect { get; set; } = true;

		[DataMember(Name = "bind_tls", EmitDefaultValue = false)]
		public string __UseTls
		{
			get => UseTls.GetAbbreviation();
			set => UseTls = ISI.Extensions.Enum<Ngrok.SerializableModels.ClientApi.UseTls>.Parse(value);
		}
		[IgnoreDataMember]
		public Ngrok.SerializableModels.ClientApi.UseTls UseTls { get; set; }
	}
}