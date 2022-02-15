using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;
using System;

namespace ISI.Extensions.RecordReaders
{
	public class RecordParserReader<TRecord> : IEnumerable<TRecord>
		where TRecord : class, new()
	{
		protected class RecordParserReaderEnumerator : IEnumerator<TRecord>
		{
			public ISI.Extensions.Parsers.IRecordDepth Context { get; private set; }

			protected System.IO.Stream SourceStream { get; }
			protected ISI.Extensions.Parsers.IRecordParser<TRecord> RecordParser { get; }

			protected System.IO.StreamReader StreamReader { get; private set; }

			public RecordParserReaderEnumerator(System.IO.Stream sourceStream, ISI.Extensions.Parsers.IRecordParser<TRecord> recordParser)
			{
				SourceStream = sourceStream;
				RecordParser = recordParser;

				Reset();
			}

			public void Reset()
			{
				Context = new ISI.Extensions.Parsers.RecordContext()
				{
					Depth = 0,
				};
				SourceStream.Rewind();
				StreamReader = null;
				Current = null;
			}

			public bool MoveNext()
			{
				var result = false;

				if (StreamReader == null)
				{
					StreamReader = new System.IO.StreamReader(SourceStream);
				}

				while (!result && !StreamReader.EndOfStream)
				{
					var parsedResult = RecordParser.Read(Context, StreamReader);

					if ((parsedResult?.Success).GetValueOrDefault())
					{
						Current = parsedResult.Record;

						Context.Depth++;

						result = true;
					}
				}

				return result;
			}

			public TRecord Current { get; private set; }

			object System.Collections.IEnumerator.Current => Current;

			public void Dispose()
			{
				StreamReader = null;
			}
		}

		protected System.IO.Stream SourceStream { get; }
		protected ISI.Extensions.Parsers.IRecordParser<TRecord> RecordParser { get; }

		public RecordParserReader(System.IO.Stream sourceStream, ISI.Extensions.Parsers.IRecordParser<TRecord> recordParser)
		{
			SourceStream = sourceStream;
			RecordParser = recordParser;
		}

		public IEnumerator<TRecord> GetEnumerator()
		{
			return new RecordParserReaderEnumerator(SourceStream, RecordParser);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
