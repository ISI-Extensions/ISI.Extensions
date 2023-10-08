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

namespace ISI.Extensions.Parsers
{
	public class DelimitedRecordParser<TRecord> : IRecordParser<TRecord>
		where TRecord : class, new()
	{
		private ISI.Extensions.Parsers.ITextParserContext _textParserContext = null;
		protected ISI.Extensions.Parsers.ITextParserContext TextParserContext => _textParserContext ??= TextParser.CreateTextParserContext();
		protected ISI.Extensions.Parsers.ITextParser TextParser { get; }

		private string LastUnparsedHeader { get; set; } = null;
		private string LastUnparsedSource { get; set; } = null;

		protected ISI.Extensions.Columns.ColumnCollection<TRecord> Columns { get; }
		protected IDictionary<string, int> ColumnLookUp { get; }
		private int[] _columnIndexes;
		protected int[] ColumnIndexes => _columnIndexes;

		protected OnRead<TRecord>[] OnReads { get; }

		public DelimitedRecordParser(ISI.Extensions.Parsers.ITextParser textParser, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<OnRead<TRecord>> onReads)
		{
			TextParser = textParser;

			Columns = new ISI.Extensions.Columns.ColumnCollection<TRecord>(columns ?? ISI.Extensions.Columns.ColumnCollection<TRecord>.GetDefault());

			ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
			for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
			{
				foreach (var columnName in Columns[columnIndex].ColumnNames)
				{
					if (!string.IsNullOrEmpty(columnName))
					{
						ColumnLookUp.Add(columnName, columnIndex);
					}
				}
			}
			_columnIndexes = System.Linq.Enumerable.Range(0, Columns.Count - 1).ToArray();

			OnReads = onReads.ToNullCheckedArray(NullCheckCollectionResult.Empty);
		}

		protected IRecordParserResponse<TRecord> ProcessSource(object context, ITextParserResponse textParserResponse)
		{
			LastUnparsedSource = textParserResponse.Source;

			var values = textParserResponse.Record.Cast<object>().ToArray();

			foreach (var onRead in OnReads)
			{
				onRead(context, LastUnparsedSource, ColumnLookUp, Columns, ref _columnIndexes, ref values);
			}

			if (context is Parsers.IRecordHeader recordHeader)
			{
				LastUnparsedHeader = recordHeader.Header;
			}

			if (values.NullCheckedAny())
			{
				var record = new TRecord();

				for (var valueIndex = 0; ((valueIndex < values.Length) && (valueIndex < ColumnIndexes.Length)); valueIndex++)
				{
					var columnIndex = ColumnIndexes[valueIndex];
					if (columnIndex >= 0)
					{
						Columns[columnIndex].SetValue(record, Columns[columnIndex].TransformValue(values[valueIndex]));
					}
				}

				if (record is ISI.Extensions.DataReader.IAcceptsRawValues acceptsRawValues)
				{
					acceptsRawValues.SetRawValues(LastUnparsedSource);
				}

				return new RecordParserResponse<TRecord>(true, LastUnparsedSource, record);
			}

			return null;
		}

		public IRecordParserResponse<TRecord> Read(object context, string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				using (var memoryStream = new System.IO.MemoryStream())
				{
					memoryStream.TextWrite(source);

					memoryStream.Rewind();

					using (var streamReader = new System.IO.StreamReader(memoryStream))
					{
						return Read(context, streamReader);
					}
				}
			}

			return new RecordParserResponse<TRecord>(false, null, null);
		}

		public IRecordParserResponse<TRecord> Read(object context, System.IO.StreamReader stream)
		{
			var textParserResponse = TextParser.Read(TextParserContext, stream);

			if (textParserResponse.Success)
			{
				return ProcessSource(context, textParserResponse);
			}

			return new RecordParserResponse<TRecord>(false, null, null);
		}

		string IRecordParser<TRecord>.BuildHeaderRecord()
		{
			return TextParser.GetUnparsed(ColumnIndexes.Where(columnIndex => columnIndex >= 0).Select(columnIndex => Columns[columnIndex].ColumnNames.FirstOrDefault()));
		}

		string IRecordParser<TRecord>.GetUnparsedRecord(TRecord record)
		{
			return TextParser.GetUnparsed(ColumnIndexes.Where(columnIndex => columnIndex >= 0).Select(columnIndex => string.Format("{0}", Columns[columnIndex].GetValue(record))));
		}

		string IRecordParser<TRecord>.GetLastUnparsedHeader() => LastUnparsedHeader;
		string IRecordParser<TRecord>.GetLastUnparsedSource() => LastUnparsedSource;
	}
}