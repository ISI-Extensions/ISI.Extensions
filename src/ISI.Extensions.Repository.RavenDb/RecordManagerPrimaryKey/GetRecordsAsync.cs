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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository.RavenDb
{
	public abstract partial class RecordManagerPrimaryKey<TRecord, TRecordPrimaryKey>
	{
		public virtual async Task<IEnumerable<TRecord>> GetRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, int skip = 0, int take = -1, System.Threading.CancellationToken cancellationToken = default)
		{
			var records = new List<TRecord>();

			using (var session = Store.OpenSession())
			{
				var sources = session.Load<object>(primaryKeyValues.Select(GetRavenKey));

				foreach (var source in sources)
				{
					var record = ToRecord(source);

					if (record != null)
					{
						records.Add(record);
					}
				}
			}

			return records.ToArray();
		}

		public virtual async Task<TRecord> GetRecordAsync(TRecordPrimaryKey primaryKeyValue, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var session = Store.OpenSession())
			{
				return session.Load<object>(GetRavenKey(primaryKeyValue)).NullCheckedConvert(ToRecord);
			}
		}
	}
}