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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.SpreadSheets
{
	public class WorkbookValuesDataReader : ISI.Extensions.DataReader.IDataReader
	{
		protected ISI.Extensions.SpreadSheets.IWorkbook Workbook { get; }

		protected ISI.Extensions.Columns.IColumn[] Columns { get; }
		protected IDictionary<string, int> ColumnLookUp { get; }

		protected ISI.Extensions.DataReader.TransformRecord TransformRecord { get; }

		public WorkbookValuesDataReader(string fileName, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this(columns, transformRecord)
		{
			var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

			Workbook = spreadSheetHelper.Open(fileName);
		}

		public WorkbookValuesDataReader(System.IO.Stream stream, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this(columns, transformRecord)
		{
			var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

			Workbook = spreadSheetHelper.Open(stream);
		}

		public WorkbookValuesDataReader(ISI.Extensions.SpreadSheets.IWorkbook workbook, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this(columns, transformRecord)
		{
			Workbook = workbook;
		}

		private WorkbookValuesDataReader(IEnumerable<ISI.Extensions.Columns.IColumn> columns, ISI.Extensions.DataReader.TransformRecord transformRecord)
		{
			Columns = columns.ToNullCheckedArray(ISI.Extensions.Extensions.NullCheckCollectionResult.Empty);

			{
				ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
				var columnIndex = 0;
				foreach (var column in Columns)
				{
					if (!string.IsNullOrEmpty(column.ColumnName))
					{
						ColumnLookUp.Add(column.ColumnName, columnIndex);
					}
					columnIndex++;
				}
			}

			TransformRecord = transformRecord;
		}

		private IEnumerator<WorksheetValuesDataReader> _worksheetValuesDataReaderEnumerator { get; set; } = null;

		int System.Data.IDataReader.Depth => _worksheetValuesDataReaderEnumerator?.Current?.Depth ?? 0;
		bool System.Data.IDataReader.IsClosed => (_worksheetValuesDataReaderEnumerator?.Current?.IsClosed).GetValueOrDefault(true);
		int System.Data.IDataReader.RecordsAffected => _worksheetValuesDataReaderEnumerator?.Current?.RecordsAffected ?? 0;
		int System.Data.IDataRecord.FieldCount => _worksheetValuesDataReaderEnumerator?.Current?.FieldCount ?? 0;

		private bool NextResult()
		{
			_worksheetValuesDataReaderEnumerator ??= Workbook.Worksheets.Select(worksheet => new WorksheetValuesDataReader(worksheet.Cells.Rows, Columns, TransformRecord)).GetEnumerator();

			return _worksheetValuesDataReaderEnumerator.MoveNext();
		}
		bool System.Data.IDataReader.NextResult() => NextResult();

		bool System.Data.IDataReader.Read()
		{
			if (_worksheetValuesDataReaderEnumerator == null)
			{
				if (!NextResult())
				{
					return false;
				}
			}

			return _worksheetValuesDataReaderEnumerator.Current.Read();
		}


		void System.Data.IDataReader.Close() => _worksheetValuesDataReaderEnumerator.Current.Close();
		System.Data.DataTable System.Data.IDataReader.GetSchemaTable() => _worksheetValuesDataReaderEnumerator.Current.GetSchemaTable();

		public object this[int i] => _worksheetValuesDataReaderEnumerator.Current[i];
		public object this[string name] => _worksheetValuesDataReaderEnumerator.Current[name];

		string System.Data.IDataRecord.GetName(int i) => _worksheetValuesDataReaderEnumerator.Current.GetName(i);
		string System.Data.IDataRecord.GetDataTypeName(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDataTypeName(i);
		Type System.Data.IDataRecord.GetFieldType(int i) => _worksheetValuesDataReaderEnumerator.Current.GetFieldType(i);
		System.Data.IDataReader System.Data.IDataRecord.GetData(int i) => _worksheetValuesDataReaderEnumerator.Current.GetData(i);
		bool System.Data.IDataRecord.IsDBNull(int i) => _worksheetValuesDataReaderEnumerator.Current.IsDBNull(i);
		IDictionary<string, object> ISI.Extensions.DataReader.IDataReader.GetValuesDictionary() => _worksheetValuesDataReaderEnumerator.Current.GetValuesDictionary();
		object System.Data.IDataRecord.GetValue(int i) => _worksheetValuesDataReaderEnumerator.Current.GetValue(i);
		int System.Data.IDataRecord.GetValues(object[] values) => _worksheetValuesDataReaderEnumerator.Current.GetValues(values);
		int System.Data.IDataRecord.GetOrdinal(string name) => _worksheetValuesDataReaderEnumerator.Current.GetOrdinal(name);
		bool System.Data.IDataRecord.GetBoolean(int i) => _worksheetValuesDataReaderEnumerator.Current.GetBoolean(i);
		byte System.Data.IDataRecord.GetByte(int i) => _worksheetValuesDataReaderEnumerator.Current.GetByte(i);
		long System.Data.IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => _worksheetValuesDataReaderEnumerator.Current.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		char System.Data.IDataRecord.GetChar(int i) => _worksheetValuesDataReaderEnumerator.Current.GetChar(i);
		long System.Data.IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => _worksheetValuesDataReaderEnumerator.Current.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		Guid System.Data.IDataRecord.GetGuid(int i) => _worksheetValuesDataReaderEnumerator.Current.GetGuid(i);
		short System.Data.IDataRecord.GetInt16(int i) => _worksheetValuesDataReaderEnumerator.Current.GetInt16(i);
		int System.Data.IDataRecord.GetInt32(int i) => _worksheetValuesDataReaderEnumerator.Current.GetInt32(i);
		long System.Data.IDataRecord.GetInt64(int i) => _worksheetValuesDataReaderEnumerator.Current.GetInt64(i);
		float System.Data.IDataRecord.GetFloat(int i) => _worksheetValuesDataReaderEnumerator.Current.GetFloat(i);
		double System.Data.IDataRecord.GetDouble(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDouble(i);
		string System.Data.IDataRecord.GetString(int i) => _worksheetValuesDataReaderEnumerator.Current.GetString(i);
		decimal System.Data.IDataRecord.GetDecimal(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDecimal(i);
		DateTime System.Data.IDataRecord.GetDateTime(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDateTime(i);
		TEnum ISI.Extensions.DataReader.IDataReader.GetEnum<TEnum>(int i, TEnum defaultValue = default) => _worksheetValuesDataReaderEnumerator.Current.GetEnum<TEnum>(i, defaultValue);
		bool? ISI.Extensions.DataReader.IDataReader.GetBooleanNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetBooleanNullable(i);
		DateTime ISI.Extensions.DataReader.IDataReader.GetDateTimeUtc(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDateTimeUtc(i);
		DateTime? ISI.Extensions.DataReader.IDataReader.GetDateTimeUtcNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDateTimeUtcNullable(i);
		DateTime? ISI.Extensions.DataReader.IDataReader.GetDateTimeNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDateTimeNullable(i);
		TimeSpan ISI.Extensions.DataReader.IDataReader.GetTimeSpan(int i) => _worksheetValuesDataReaderEnumerator.Current.GetTimeSpan(i);
		TimeSpan? ISI.Extensions.DataReader.IDataReader.GetTimeSpanNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetTimeSpanNullable(i);
		decimal? ISI.Extensions.DataReader.IDataReader.GetDecimalNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDecimalNullable(i);
		double? ISI.Extensions.DataReader.IDataReader.GetDoubleNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetDoubleNullable(i);
		Guid? ISI.Extensions.DataReader.IDataReader.GetGuidNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetGuidNullable(i);
		int ISI.Extensions.DataReader.IDataReader.GetInt(int i) => _worksheetValuesDataReaderEnumerator.Current.GetInt(i);
		int? ISI.Extensions.DataReader.IDataReader.GetIntNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetIntNullable(i);
		long ISI.Extensions.DataReader.IDataReader.GetLong(int i) => _worksheetValuesDataReaderEnumerator.Current.GetLong(i);
		long? ISI.Extensions.DataReader.IDataReader.GetLongNullable(int i) => _worksheetValuesDataReaderEnumerator.Current.GetLongNullable(i);

		public void Dispose()
		{
			Workbook?.Dispose();
		}
	}
}
