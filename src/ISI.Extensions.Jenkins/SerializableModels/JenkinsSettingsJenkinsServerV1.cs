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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;
using LOCALENTITIES = ISI.Extensions.Jenkins;

namespace ISI.Extensions.Jenkins.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("96b9e8f1-3cdb-4e97-a3c3-dd65f8471534")]
	public class JenkinsSettingsJenkinsServerV1 : IJenkinsSettingsJenkinsServer
	{
		public static IJenkinsSettingsJenkinsServer ToSerializable(LOCALENTITIES.JenkinsSettingsJenkinsServer source)
		{
			return new JenkinsSettingsJenkinsServerV1()
			{
				JenkinsServerUuid = source.JenkinsServerUuid,
				Description = source.Description,
				JenkinsUrl = source.JenkinsUrl,
				UserName = source.UserName,
				ApiToken = source.ApiToken,
				Directories = source.Directories.ToNullCheckedArray(),
			};
		}

		public LOCALENTITIES.JenkinsSettingsJenkinsServer Export()
		{
			return new LOCALENTITIES.JenkinsSettingsJenkinsServer()
			{
				JenkinsServerUuid = JenkinsServerUuid,
				Description = Description,
				JenkinsUrl = JenkinsUrl,
				UserName = UserName,
				ApiToken = ApiToken,
				Directories = Directories.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "jenkinsServerUuid", EmitDefaultValue = false)]
		public Guid JenkinsServerUuid { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string JenkinsUrl { get; set; }

		[DataMember(Name = "userName", EmitDefaultValue = false)]
		public string UserName { get; set; }

		[DataMember(Name = "apiToken", EmitDefaultValue = false)]
		public string ApiToken { get; set; }

		[DataMember(Name = "directories", EmitDefaultValue = false)]
		public string[] Directories { get; set; }
	}
}
