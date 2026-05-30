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
using LOCALENTITIES = ISI.Extensions.Nginx;

namespace ISI.Extensions.Nginx.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("3fcddb8e-8f47-442f-972f-08d0ffe59f11")]
	public class NginxManagerServerV1 : INginxManagerServer
	{
		public static INginxManagerServer ToSerializable(LOCALENTITIES.NginxManagerServer source)
		{
			return new NginxManagerServerV1()
			{
				NginxManagerServerUuid = source.NginxManagerServerUuid,
				Description = source.Description,
				NginxManagerApiUrl = source.NginxManagerApiUrl,
				NginxManagerApiKey = source.NginxManagerApiKey,
				Directories = source.Directories.ToNullCheckedArray(),
			};
		}

		public LOCALENTITIES.NginxManagerServer Export()
		{
			return new LOCALENTITIES.NginxManagerServer()
			{
				NginxManagerServerUuid = NginxManagerServerUuid,
				Description = Description,
				NginxManagerApiUrl = NginxManagerApiUrl,
				NginxManagerApiKey = NginxManagerApiKey,
				Directories = Directories.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "nginxManagerServerUuid", EmitDefaultValue = false)]
		public Guid NginxManagerServerUuid { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "nginxManagerApiUrl", EmitDefaultValue = false)]
		public string NginxManagerApiUrl { get; set; }

		[DataMember(Name = "nginxManagerApiKey", EmitDefaultValue = false)]
		public string NginxManagerApiKey { get; set; }

		[DataMember(Name = "directories", EmitDefaultValue = false)]
		public string[] Directories { get; set; }
	}
}