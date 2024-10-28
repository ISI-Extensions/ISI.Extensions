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

namespace ISI.Extensions.Repository
{
	public interface IRecordManager
	{
		void CreateTable(CreateTableMode createTableMode = CreateTableMode.ErrorIfExists);
		void DropTable();
	}

	public interface IRecordManager<TRecord> : IRecordManager
		where TRecord : class, IRecordManagerRecord, new()
	{
		IAsyncEnumerable<TRecord> InsertRecordsAsync(IEnumerable<TRecord> records, System.Threading.CancellationToken cancellationToken = default);
		Task<TRecord> InsertRecordAsync(TRecord record, System.Threading.CancellationToken cancellationToken = default);

		IAsyncEnumerable<TRecord> UpsertRecordsAsync(IEnumerable<TRecord> records, System.Threading.CancellationToken cancellationToken = default);
		Task<TRecord> UpsertRecordAsync(TRecord record, System.Threading.CancellationToken cancellationToken = default);

		Task<int> UpdateRecordsAsync(IEnumerable<TRecord> records, System.Threading.CancellationToken cancellationToken = default);
		Task<int> UpdateRecordAsync(TRecord record, System.Threading.CancellationToken cancellationToken = default);
		Task<int> UpdateRecordsAsync(IEnumerable<TRecord> records, UpdateRecordFilterColumnCollection<TRecord> updateRecordFilterColumns, System.Threading.CancellationToken cancellationToken = default);
		Task<int> UpdateRecordAsync(TRecord record, UpdateRecordFilterColumnCollection<TRecord> updateRecordFilterColumns, System.Threading.CancellationToken cancellationToken = default);
	}

	public interface IRecordManagerPrimaryKey<TRecord, TRecordPrimaryKey> : IRecordManager<TRecord>
		where TRecord : class, IRecordManagerPrimaryKeyRecord<TRecordPrimaryKey>, new()
	{
		IAsyncEnumerable<TRecord> GetRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, int skip = 0, int take = -1, System.Threading.CancellationToken cancellationToken = default);
		Task<TRecord> GetRecordAsync(TRecordPrimaryKey primaryKeyValue, System.Threading.CancellationToken cancellationToken = default);

		Task<int> UpdateRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, SetRecordColumnCollection<TRecord> setRecordColumns, System.Threading.CancellationToken cancellationToken = default);

		Task<int> DeleteRecordAsync(TRecordPrimaryKey primaryKeyValue, System.Threading.CancellationToken cancellationToken = default);
		Task<int> DeleteRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, System.Threading.CancellationToken cancellationToken = default);
	}

	public interface IRecordManagerPrimaryKeyWithArchive<TRecord, TRecordPrimaryKey> : IRecordManagerPrimaryKey<TRecord, TRecordPrimaryKey>
		where TRecord : class, IRecordManagerPrimaryKeyRecord<TRecordPrimaryKey>, IRecordManagerRecordWithArchive, new()
	{
		IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> GetArchiveRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, int skip = 0, int take = -1, System.Threading.CancellationToken cancellationToken = default);
		IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> GetArchiveRecordsAsync(TRecordPrimaryKey primaryKeyValue, DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, int skip = 0, int take = -1, System.Threading.CancellationToken cancellationToken = default);
	}
}
