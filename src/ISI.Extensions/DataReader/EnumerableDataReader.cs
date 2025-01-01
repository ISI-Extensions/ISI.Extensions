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

namespace ISI.Extensions.DataReader
{
	public partial class EnumerableDataReader<TRecord> : AbstractDataReader
		where TRecord : class, new()
	{
		protected IEnumerable<IEnumerable<TRecord>> RecordSets { get; }
		protected IEnumerator<IEnumerable<TRecord>> RecordSetsEnumerator { get; private set; }
		protected IEnumerator<TRecord> RecordsEnumerator { get; private set; }

		protected ISI.Extensions.Columns.IColumn<TRecord>[] Columns { get; }
		protected IDictionary<string, int> ColumnLookUp { get; }

		protected ISI.Extensions.DataReader.TransformRecord TransformRecord { get; }

		public EnumerableDataReader(IEnumerable<TRecord> records, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this([records], null, transformRecord)
		{
		}

		public EnumerableDataReader(IEnumerable<TRecord> records, ISI.Extensions.Columns.ColumnCollection<TRecord> columns, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this([records], columns, transformRecord)
		{
		}
		
		public EnumerableDataReader(IEnumerable<IEnumerable<TRecord>> recordSets, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this(recordSets, null, transformRecord)
		{
		}

		public EnumerableDataReader(IEnumerable<IEnumerable<TRecord>> recordSets, ISI.Extensions.Columns.ColumnCollection<TRecord> columns, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
		{
			RecordSets = recordSets;
			Columns = (columns.NullCheckedAny() ? columns : ISI.Extensions.Columns.ColumnCollection<TRecord>.GetDefault()).ToArray();

			{
				ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
				var columnIndex = 0;
				foreach (var column in Columns)
				{
					if (!string.IsNullOrEmpty(column.ColumnName))
					{
						if (!ColumnLookUp.ContainsKey(column.ColumnName))
						{
							ColumnLookUp.Add(column.ColumnName, columnIndex);
						}
					}
					columnIndex++;
				}
			}

			FieldCount = -1;

			TransformRecord = transformRecord;

			RecordSetsEnumerator = null;
			RecordsEnumerator = null;
		}

		public override string GetName(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			return Columns[columnIndex].ColumnName;
		}

		public override System.Data.DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public override int GetOrdinal(string name)
		{
			return ColumnLookUp[name];
		}

		public override bool NextResult()
		{
			Close();

			return false;
		}

		public override bool Read()
		{
			Values = null;

			RecordSetsEnumerator ??= RecordSets.GetEnumerator();

			if (RecordsEnumerator == null)
			{
				if (RecordSetsEnumerator.MoveNext())
				{
					RecordsEnumerator = RecordSetsEnumerator.Current.GetEnumerator();
				}
			}

			bool? result = null;

			if (RecordsEnumerator != null)
			{
				var endOfEnumerable = false;

				while (!result.HasValue)
				{
					endOfEnumerable = !RecordsEnumerator.MoveNext();

					if (endOfEnumerable)
					{
						endOfEnumerable = !RecordSetsEnumerator.MoveNext();

						if (!endOfEnumerable)
						{
							RecordsEnumerator = RecordSetsEnumerator.Current.GetEnumerator();

							endOfEnumerable = !RecordsEnumerator.MoveNext();
						}
					}

					if (endOfEnumerable)
					{
						result = false;
					}

					if (!endOfEnumerable)
					{
						var record = RecordsEnumerator.Current;

						Source = record;
						Values = new object[Columns.Length];

						for (var columnIndex = 0; columnIndex < Columns.Length; columnIndex++)
						{
							Values[columnIndex] = Columns[columnIndex].GetValue(record);
						}

						TransformRecord?.Invoke(Depth, Columns, Source, ref Values);
						
						FieldCount = Values.NullCheckedCount();

						if (Values != null)
						{
							result = true;
						}
					}
				}

				if (endOfEnumerable || result.GetValueOrDefault())
				{
					Depth++;
				}
			}

			return result.GetValueOrDefault();
		}

		public override void Close()
		{
			RecordsEnumerator?.Dispose();
			RecordsEnumerator = null;
		}
	}
}
