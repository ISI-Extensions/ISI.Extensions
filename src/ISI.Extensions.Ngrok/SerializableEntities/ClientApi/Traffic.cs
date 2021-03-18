#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Ngrok.SerializableEntities.ClientApi
{
	[DataContract]
	public class Traffic : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Ngrok.Traffic>
	{
		public ISI.Extensions.Ngrok.Traffic Export()
		{
			return new ISI.Extensions.Ngrok.Traffic()
			{
				TrafficUri = TrafficUri,
				TrafficKey = TrafficKey,
				TunnelName = TunnelName,
				LocalExternalGatewayAddress = LocalExternalGatewayAddress,
				StartDateTime = StartDateTime.ToDateTime(),
				Duration = Duration,
				Request = Request.NullCheckedConvert(request => request.Export()),
				Response = Response.NullCheckedConvert(response => response.Export()),
			};
		}

		[DataMember(Name = "uri", EmitDefaultValue = false)]
		public string TrafficUri { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string TrafficKey { get; set; }

		[DataMember(Name = "tunnel_Name", EmitDefaultValue = false)]
		public string TunnelName { get; set; }

		[DataMember(Name = "remote_Addr", EmitDefaultValue = false)]
		public string LocalExternalGatewayAddress { get; set; }

		[DataMember(Name = "start", EmitDefaultValue = false)]
		public string StartDateTime { get; set; }

		[DataMember(Name = "duration", EmitDefaultValue = false)]
		public long Duration { get; set; }

		[DataMember(Name = "request", EmitDefaultValue = false)]
		public TrafficRequest Request { get; set; }

		[DataMember(Name = "response", EmitDefaultValue = false)]
		public TrafficResponse Response { get; set; }
	}
}