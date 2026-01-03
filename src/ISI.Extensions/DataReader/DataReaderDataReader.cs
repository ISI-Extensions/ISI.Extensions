#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.DataReader
{
	public class DataReaderDataReader : AbstractDataReader
	{
		protected System.Data.IDataReader DataReader { get; }

		protected ISI.Extensions.Columns.IColumn[] Columns { get; set; }
		protected IDictionary<string, int> ColumnLookUp { get; }

		protected ISI.Extensions.DataReader.TransformRecord TransformRecord { get; }

		private bool _isFirstRecord { get; set; }
		private bool _isEmpty { get; set; }

		public DataReaderDataReader(System.Data.IDataReader dataReader, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
		{
			DataReader = dataReader;

			ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

			TransformRecord = transformRecord;

			Depth = 0;

			_isEmpty = !DataReader.Read();
			if (!_isEmpty)
			{
				_isFirstRecord = true;

				UpdateColumns();
			}
		}

		protected void UpdateColumns()
		{
			FieldCount = DataReader.FieldCount;

			Columns = new ISI.Extensions.Columns.IColumn[FieldCount];

			ColumnLookUp.Clear();
			for (var columnIndex = 0; columnIndex < FieldCount; columnIndex++)
			{
				var columnName = DataReader.GetName(columnIndex);

				Columns[columnIndex] = new ISI.Extensions.Columns.Column(columnName, record => DataReader.IsDBNull(columnIndex), record => DataReader.GetValue(columnIndex));

				ColumnLookUp.Add(columnName, columnIndex);
			}
		}

		public override System.Data.DataTable GetSchemaTable() => DataReader.GetSchemaTable();

		public override int GetOrdinal(string name) => DataReader.GetOrdinal(name);

		public override bool NextResult()
		{
			Depth = 0;

			if (DataReader.NextResult())
			{
				Columns = null;

				return true;
			}

			return false;
		}

		public override bool Read()
		{
			var hasRow = false;

			if (!_isEmpty)
			{
				while (!hasRow && (_isFirstRecord || DataReader.Read()))
				{
					Depth++;

					_isFirstRecord = false;

					Source = new object[FieldCount];

					DataReader.GetValues((object[])Source);

					Values = new object[FieldCount];

					for (var columnIndex = 0; columnIndex < FieldCount; columnIndex++)
					{
						Values[columnIndex] = DataReader.GetValue(columnIndex);
					}

					TransformRecord?.Invoke(Depth, Columns, Source, ref Values);

					if (Values.NullCheckedAny())
					{
						hasRow = true;
					}
				}
			}

			return hasRow;
		}

		public override void Close() => DataReader.Close();
	}
}
