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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions
{
	public class RecordParserFactory<TRecord>
		where TRecord : class, new()
	{
		public static ISI.Extensions.Parsers.OnRead<TRecord> GetHeadersOnFirstLineHandler()
		{
			return (object context, string source, IDictionary<string, int> columnLookup, ISI.Extensions.Columns.IColumnCollection<TRecord> columns, ref int[] columnIndexes, ref object[] values) =>
			{
				if ((context is Parsers.IRecordDepth recordDepthContext) && (recordDepthContext.Depth == 0))
				{
					if (context is Parsers.IRecordHeader recordHeader)
					{
						recordHeader.Header = source;
					}

					columnIndexes = System.Linq.Enumerable.Range(0, values.Length).Select(columnIndex => -1).ToArray();

					for (var valueIndex = 0; valueIndex < values.Length; valueIndex++)
					{
						var value = (values[valueIndex] is string ? (string)values[valueIndex] : $"{values[valueIndex]}").Trim();

						if (columnLookup.TryGetValue(value, out var columnIndex))
						{
							columnIndexes[valueIndex] = columnIndex;
						}
					}

					recordDepthContext.Depth++;

					values = null;
				}
			};
		}

		public static ISI.Extensions.Parsers.OnRead<TRecord> GetHeaderPrefixHandler(string headerPrefix)
		{
			return (object context, string source, IDictionary<string, int> columnLookup, ISI.Extensions.Columns.IColumnCollection<TRecord> columns, ref int[] columnIndexes, ref object[] values) =>
			{
				var headerColumnValue = (values[0] is string ? (string)values[0] : $"{values[0]}");

				if (headerColumnValue.StartsWith(headerPrefix))
				{
					if (context is Parsers.IRecordHeader recordHeader)
					{
						recordHeader.Header = source;
					}

					values[0] = headerColumnValue.Substring(headerPrefix.Length);

					columnIndexes = System.Linq.Enumerable.Range(0, values.Length).Select(columnIndex => -1).ToArray();

					for (var valueIndex = 0; valueIndex < values.Length; valueIndex++)
					{
						var value = (values[valueIndex] is string ? (string)values[valueIndex] : $"{values[valueIndex]}");

						if (columnLookup.TryGetValue(value, out var columnIndex))
						{
							columnIndexes[valueIndex] = columnIndex;
						}
					}

					values = null;
				}
			};
		}


		public static ISI.Extensions.Parsers.IRecordParser<TRecord> GetRecordParser(char textDelimiter, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
		{
			return new ISI.Extensions.Parsers.DelimitedRecordParser<TRecord>(TextParserFactory.GetTextParser( textDelimiter), columns, onReads);
		}

		public static ISI.Extensions.Parsers.IRecordParser<TRecord> GetRecordParser(TextParserFactory.TextDelimiter textDelimiter, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
		{
			return new ISI.Extensions.Parsers.DelimitedRecordParser<TRecord>(TextParserFactory.GetTextParser(textDelimiter), columns, onReads);
		}

		public static ISI.Extensions.Parsers.IRecordParser<TRecord> GetRecordParserByFileName(string fileName, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
		{
			return GetRecordParser(TextParserFactory.GetTextDelimiterByFileName(fileName), columns, onReads);
		}
	}
}
