#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Jira.SerializableModels
{
	[DataContract]
	public class History : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.History>
	{
		public ISI.Extensions.Jira.History Export()
		{
			return new()
			{
				HistoryId = HistoryId,
				Author = Author?.Export(),
				Created = Created,
				Items = Items.ToNullCheckedArray(item => item?.Export()),
				HistoryMetadata = HistoryMetadata?.Export(),
			};
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string HistoryId { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public User Author { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string __Created { get =>string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffzz}00", (Created.Kind == DateTimeKind.Local ? Created : Created.ToLocalTime())); set => Created = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Created { get; set; }

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public HistoryItem[] Items { get; set; }

		[DataMember(Name = "historyMetadata", EmitDefaultValue = false)]
		public HistoryMetadata HistoryMetadata { get; set; }
	}
}