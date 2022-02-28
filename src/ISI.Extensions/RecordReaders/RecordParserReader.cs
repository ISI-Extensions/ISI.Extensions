#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
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