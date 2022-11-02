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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Ebcdic
{
	public class EbcdicRecordReader<TIRecord> : IEnumerable<TIRecord>, IDisposable
		where TIRecord : class
	{
		public class EbcdicRecordReaderEnumerator : IEnumerator<TIRecord>
		{
			protected System.IO.Stream SourceStream { get; }
			protected RecordDefinitionCollection<TIRecord>.IRecordDefinition[] RecordDefinitions { get; }
			public int RecordSize { get; }
			protected Func<TIRecord, bool> Filter { get; }

			public EbcdicRecordReaderEnumerator(System.IO.Stream sourceStream, IEnumerable<RecordDefinitionCollection<TIRecord>.IRecordDefinition> recordDefinitions, Func<TIRecord, bool> filter)
			{
				SourceStream = sourceStream;
				RecordDefinitions = recordDefinitions.ToArray();
				RecordSize = RecordDefinitions.FirstOrDefault().RecordSize;
				Filter = filter;

				if (RecordDefinitions.Any(recordDefinition => recordDefinition.RecordSize != RecordSize))
				{
					throw new(string.Format("All Record Definition must be the same size:\n{0}  ", string.Join("\n  ", recordDefinitions.Select(recordDefinition => string.Format("{0} => {1}", recordDefinition.RecordSize, recordDefinition.RecordType.Name)))));
				}

				Current = null;
			}

			public void Reset()
			{
				SourceStream.Rewind();
				Current = null;
			}

			public bool MoveNext()
			{
				Current = null;

				var recordBuffer = new byte[RecordSize];

				while (Current == null)
				{
					var readLength = SourceStream.Read(recordBuffer, 0, RecordSize);

					if (readLength < RecordSize)
					{
						return false;
					}

					foreach (var recordDefinition in RecordDefinitions)
					{
						if (recordDefinition.IsRecordType(recordBuffer))
						{
							var record = recordDefinition.GetRecord(recordBuffer);

							if (Filter(record))
							{
								Current = record;

								return true;
							}
						}
					}
				}

				return true; //Will never get executed, just here for compiler
			}


			public TIRecord Current { get; protected set; }

			object System.Collections.IEnumerator.Current => Current;

			public void Dispose()
			{
				Current = null;
			}
		}

		protected System.IO.Stream SourceStream { get; }
		protected RecordDefinitionCollection<TIRecord> RecordDefinitions { get; }
		protected Func<TIRecord, bool> Filter { get; }

		public EbcdicRecordReader(System.IO.Stream sourceStream, RecordDefinitionCollection<TIRecord> recordDefinitions, Func<TIRecord, bool> filter = null)
		{
			SourceStream = sourceStream;
			RecordDefinitions = recordDefinitions;
			Filter = filter ?? (record => true);
		}

		public IEnumerator<TIRecord> GetEnumerator()
		{
			return new EbcdicRecordReaderEnumerator(SourceStream, RecordDefinitions, Filter);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			((IDisposable)SourceStream).Dispose();
		}
	}
}
