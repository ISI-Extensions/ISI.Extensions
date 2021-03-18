#region Copyright & License
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.DataReader
{
	public class ParsedStringDataReader : AbstractDataReader
	{
		protected System.IO.Stream Stream { get; }

		protected ISI.Extensions.Parsers.ITextParser StringParser { get; }

		protected ISI.Extensions.DataReader.IColumnInfo[] Columns { get; }
		protected IDictionary<string, int> ColumnLookUp { get; }

		protected ISI.Extensions.DataReader.TransformRecord TransformRecord { get; }

		protected System.IO.StreamReader StreamReader { get; private set; }

		public ParsedStringDataReader(System.IO.Stream stream, ISI.Extensions.Parsers.ITextParser stringParser, IEnumerable<ISI.Extensions.DataReader.IColumnInfo> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
		{
			Stream = stream;
			StringParser = stringParser;

			Columns = columns.ToNullCheckedArray(NullCheckCollectionResult.Empty);

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

			Depth = 0;
			StreamReader = null;

			FieldCount = Columns.Length;
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
			var result = false;

			if (StreamReader == null)
			{
				StreamReader = new System.IO.StreamReader(Stream);
			}

			while (!result && !StreamReader.EndOfStream)
			{
				var line = StreamReader.ReadLine();

				if (!string.IsNullOrWhiteSpace(line))
				{
					var parserResult = StringParser.Read(line);

					if (parserResult.Success)
					{
						Values = new object[parserResult.Record.Length];

						for (var columnIndex = 0; columnIndex < parserResult.Record.Length; columnIndex++)
						{
							Values[columnIndex] = parserResult.Record[columnIndex];
						}

						TransformRecord?.Invoke(Depth, Columns, ref Values);

						if (Values != null)
						{
							result = true;
						}
					}
				}
			}

			if (StreamReader.EndOfStream || (Values != null))
			{
				Depth++;
			}

			return result;
		}

		public override void Close()
		{

		}
	}
}
