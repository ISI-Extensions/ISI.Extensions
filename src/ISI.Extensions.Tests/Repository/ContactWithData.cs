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

namespace ISI.Extensions.Tests.Repository
{
	[ISI.Extensions.Repository.Record(Schema = "Contacts", TableName = "ContactWithData")]
	public class ContactWithData : ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>, ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime
	{
		[ISI.Extensions.Repository.PrimaryKey]
		[ISI.Extensions.Repository.RecordProperty(ColumnName = "ContactUuid")]
		public Guid ContactUuid { get; set; }

		[ISI.Extensions.Repository.RecordProperty(ColumnName = "FirstName", PropertySize = 255)]
		public string FirstName { get; set; }

		[ISI.Extensions.Repository.RecordProperty(ColumnName = "LastName", PropertySize = 255)]
		public string LastName { get; set; }

		[ISI.Extensions.Repository.RecordProperty(ColumnName = "TimeStamp")]
		public DateTime TimeStamp { get; set; }

		[ISI.Extensions.Repository.RecordProperty(ColumnName = "ContactData")]
		public IContactData ContactData { get; set; }
		
		Guid ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>.PrimaryKey => ContactUuid;

		[ISI.Extensions.Repository.IgnoreRecordProperty]
		DateTime ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime.ArchiveDateTimeUtc => TimeStamp;
	}
}
