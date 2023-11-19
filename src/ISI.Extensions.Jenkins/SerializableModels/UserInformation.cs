#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public class UserInformation : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jenkins.UserInformation>
	{
		public static UserInformation ToSerializable(ISI.Extensions.Jenkins.UserInformation source)
		{
			return source.NullCheckedConvert(value => new UserInformation()
			{
				Url = value.Url,
				FullName = value.FullName,
				Id = value.Id,
				Description = value.Description,
				Properties = value.Properties.ToNullCheckedArray(UserProperty.ToSerializable),
			});
		}

		public ISI.Extensions.Jenkins.UserInformation Export()
		{
			return new()
			{
				Url = Url,
				FullName = FullName,
				Id = Id,
				Description = Description,
				Properties = Properties.ToNullCheckedArray(x => x.Export()),
			};
		}

		[DataMember(Name = "absoluteUrl", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "fullName", EmitDefaultValue = false)]
		public string FullName { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "property", EmitDefaultValue = false)]
		public UserProperty[] Properties { get; set; }

		public string RestJsonUrl => this.Url + "api/json";
	}
}