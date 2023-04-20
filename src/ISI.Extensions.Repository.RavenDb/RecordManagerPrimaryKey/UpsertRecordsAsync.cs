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

namespace ISI.Extensions.Repository.RavenDb
{
	public abstract partial class RecordManagerPrimaryKey<TRecord, TRecordPrimaryKey>
	{
		public override async Task<IEnumerable<TRecord>> UpsertRecordsAsync(IEnumerable<TRecord> records, Action<TRecord> updateRecordProperties)
		{
			var result = records.ToArray();

			var primaryKeyPropertyDescription = RecordDescription.GetRecordDescription<TRecord>().PrimaryKeyPropertyDescriptions.FirstOrDefault();

			if (typeof(TRecordPrimaryKey) == typeof(Guid))
			{
				foreach (var record in result)
				{
					var primaryKeyValue = (Guid) primaryKeyPropertyDescription.GetValue(record);

					if (primaryKeyValue == Guid.Empty)
					{
						primaryKeyPropertyDescription.SetValue(record, Guid.NewGuid());
					}
				}
			}

			if (typeof(TRecordPrimaryKey) == typeof(Guid?))
			{
				foreach (var record in result)
				{
					var primaryKeyValue = (Guid?) primaryKeyPropertyDescription.GetValue(record);

					if (!primaryKeyValue.HasValue || (primaryKeyValue == Guid.Empty))
					{
						primaryKeyPropertyDescription.SetValue(record, Guid.NewGuid());
					}
				}
			}

			using (var session = Store.OpenSession())
			{
				foreach (var record in result)
				{
					session.Store(record, GetRavenKey((TRecordPrimaryKey) primaryKeyPropertyDescription.GetValue(record)));
				}

				session.SaveChanges();
			}

			return result;
		}
	}
}