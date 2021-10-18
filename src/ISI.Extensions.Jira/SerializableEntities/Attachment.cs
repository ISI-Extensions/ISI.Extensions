﻿#region Copyright & License
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

namespace ISI.Extensions.Jira.SerializableEntities
{
	[DataContract]
	public partial class Attachment : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.Attachment>
	{
		public ISI.Extensions.Jira.Attachment Export()
		{
			return new ISI.Extensions.Jira.Attachment()
			{
				IssueId = IssueId,
				AttachmentId = AttachmentId,
				Mimetype = Mimetype,
				FileName = FileName,
				FileSize = FileSize,
			};
		}

		[DataMember(Name = "issueId", EmitDefaultValue = false)]
		public string IssueId { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string AttachmentId { get; set; }

		[DataMember(Name = "mimetype", EmitDefaultValue = false)]
		public string Mimetype { get; set; }

		[DataMember(Name = "fileName", EmitDefaultValue = false)]
		public string FileName { get; set; }

		[DataMember(Name = "fileSize", EmitDefaultValue = false)]
		public long FileSize { get; set; }

	}
}