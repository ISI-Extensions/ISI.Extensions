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
	public class ChangeSetItem : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jenkins.ChangeSetItem>
	{
		public static ChangeSetItem ToSerializable(ISI.Extensions.Jenkins.ChangeSetItem source)
		{
			return source.NullCheckedConvert(value => new ChangeSetItem()
			{
				AffectedPaths = value.AffectedPaths.ToNullCheckedArray(),
				CommitId = value.CommitId,
				TimeStamp = value.TimeStamp,
				Author = value.Author.NullCheckedConvert(UserInformation.ToSerializable),
				Comment = value.Comment,
				Id = value.Id,
				Message = value.Message,
				PathEdits = value.PathEdits.ToNullCheckedArray(FilePathEdit.ToSerializable),
			});
		}

		public ISI.Extensions.Jenkins.ChangeSetItem Export()
		{
			return new()
			{
				AffectedPaths = AffectedPaths.ToNullCheckedArray(),
				CommitId = CommitId,
				TimeStamp = TimeStamp,
				Author = Author.NullCheckedConvert(x => x.Export()),
				Comment = Comment,
				Id = Id,
				Message = Message,
				PathEdits = PathEdits.ToNullCheckedArray(x => x.Export()),
			};
		}

		[DataMember(Name = "affectedPaths", EmitDefaultValue = false)]
		public string[] AffectedPaths { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public string CommitId { get; set; }

		[DataMember(Name = "timeStamp", EmitDefaultValue = false)]
		public string __TimeStamp { get => TimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => TimeStamp = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? TimeStamp { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public UserInformation Author { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "msg", EmitDefaultValue = false)]
		public string Message { get; set; }

		[DataMember(Name = "paths", EmitDefaultValue = false)]
		public FilePathEdit[] PathEdits { get; set; }
	}
}