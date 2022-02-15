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
	public class SortedRecordParserReader<TRecord> : IEnumerable<TRecord>
		where TRecord : class, new()
	{
		public interface IColumnSortDefinition
		{
			int CompareColumn(TRecord x, TRecord y);
		}

		public class ColumnSortDefinition<TProperty> : IColumnSortDefinition
		{
			public Func<TRecord, TProperty> GetValue { get; }
			public Func<TProperty, TProperty, int> Comparer { get; }
			public bool SortAscending { get; }

			private readonly Comparer<TProperty> _defaultComparer;

			public ColumnSortDefinition(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> expression, Func<TProperty, TProperty, int> comparer = null, bool sortAscending = true)
			{
				GetValue = expression.Compile();
				SortAscending = sortAscending;

				if (comparer == null)
				{
					_defaultComparer = Comparer<TProperty>.Default;
					comparer = ((x, y) => _defaultComparer.Compare(x, y));
				}

				Comparer = comparer;
			}

			public int CompareColumn(TRecord x, TRecord y)
			{
				if (SortAscending)
				{
					return Comparer(GetValue(x), GetValue(y));
				}

				return Comparer(GetValue(x), GetValue(y)) * -1;
			}
		}

		public class ColumnSortDefinitionCollection : List<IColumnSortDefinition>, IComparer<TRecord>
		{
			public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> expression)
			{
				Add(new ColumnSortDefinition<TProperty>(expression, null, true));
			}
			public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> expression, Func<TProperty, TProperty, int> comparer)
			{
				Add(new ColumnSortDefinition<TProperty>(expression, comparer, true));
			}
			public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> expression, bool sortAscending)
			{
				Add(new ColumnSortDefinition<TProperty>(expression, null, sortAscending));
			}
			public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> expression, Func<TProperty, TProperty, int> comparer, bool sortAscending)
			{
				Add(new ColumnSortDefinition<TProperty>(expression, comparer, sortAscending));
			}

			public int Compare(TRecord x, TRecord y)
			{
				var result = 0;

				for (var columnIndex = 0; ((columnIndex < Count) && (result == 0)); columnIndex++)
				{
					result = this[columnIndex].CompareColumn(x, y);
				}

				return result;
			}
		}

		public class SortedRecordParserReaderEnumerator : IEnumerator<TRecord>
		{
			protected class ParsedRecordComparer : IComparer<ISI.Extensions.Parsers.IRecordParserResponse<TRecord>>
			{
				protected IComparer<TRecord> Comparer { get; }

				public ParsedRecordComparer(IComparer<TRecord> comparer)
				{
					Comparer = comparer;
				}

				public int Compare(ISI.Extensions.Parsers.IRecordParserResponse<TRecord> x, ISI.Extensions.Parsers.IRecordParserResponse<TRecord> y)
				{
					return Comparer.Compare(x.Record, y.Record);
				}
			}

			protected class ParsedStringRecordComparer : IComparer<(ISI.Extensions.Parsers.IRecordParserResponse<TRecord> RecordParserResponse, System.IO.StreamReader StreamReader)>
			{
				protected IComparer<ISI.Extensions.Parsers.IRecordParserResponse<TRecord>> Comparer { get; }

				public ParsedStringRecordComparer(IComparer<ISI.Extensions.Parsers.IRecordParserResponse<TRecord>> comparer)
				{
					Comparer = comparer;
				}

				public int Compare((ISI.Extensions.Parsers.IRecordParserResponse<TRecord> RecordParserResponse, System.IO.StreamReader StreamReader) x, (ISI.Extensions.Parsers.IRecordParserResponse<TRecord> RecordParserResponse, System.IO.StreamReader StreamReader) y)
				{
					return Comparer.Compare(x.RecordParserResponse, y.RecordParserResponse);
				}
			}

			public int MaxDegreeOfParallelismForBucketCreation { get; }
			public int MaxBucketSize { get; }
			protected System.IO.Stream SourceStream { get; }
			protected ISI.Extensions.Parsers.IRecordParser<TRecord> RecordParser { get; }
			protected ColumnSortDefinitionCollection ColumnSortDefinitions { get; }
			protected IComparer<ISI.Extensions.Parsers.IRecordParserResponse<TRecord>> RecordComparer { get; }
			protected IComparer<(ISI.Extensions.Parsers.IRecordParserResponse<TRecord> RecordParserResponse, System.IO.StreamReader StreamReader)> BucketComparer { get; }

			protected List<System.IO.Stream> BucketStreams { get; private set; }
			protected List<(ISI.Extensions.Parsers.IRecordParserResponse<TRecord> RecordParserResponse, System.IO.StreamReader StreamReader)> SortedQueue { get; private set; }

			public SortedRecordParserReaderEnumerator(System.IO.Stream sourceStream, ISI.Extensions.Parsers.IRecordParser<TRecord> recordParser, ColumnSortDefinitionCollection columnSortDefinitions, int maxDegreeOfParallelismForBucketCreation, int maxBucketSize)
			{
				MaxDegreeOfParallelismForBucketCreation = maxDegreeOfParallelismForBucketCreation;
				MaxBucketSize = maxBucketSize;
				SourceStream = sourceStream;
				RecordParser = recordParser;
				ColumnSortDefinitions = columnSortDefinitions;
				RecordComparer = new ParsedRecordComparer(ColumnSortDefinitions);
				BucketComparer = new ParsedStringRecordComparer(RecordComparer);

				BucketStreams = null;
				SortedQueue = null;
			}

			public void Reset()
			{
				Current = null;

				if (SortedQueue != null)
				{
					foreach (var queueItem in SortedQueue)
					{
						queueItem.StreamReader.BaseStream.Dispose();
						queueItem.StreamReader.Dispose();
					}
					SortedQueue = null;
				}

				if (BucketStreams != null)
				{
					foreach (var bucketStream in BucketStreams)
					{
						bucketStream?.Dispose();
					}
					BucketStreams = null;
				}
			}

			private void AddToSortedQueue(ISI.Extensions.Parsers.IRecordParserResponse<TRecord> parsedRecord, System.IO.StreamReader reader)
			{
				var item = (RecordParserResponse: parsedRecord, StreamReader: reader);

				if (!SortedQueue.Any() || (BucketComparer.Compare(SortedQueue[SortedQueue.Count - 1], item) <= 0))
				{
					SortedQueue.Add(item);
				}
				else if (BucketComparer.Compare(SortedQueue[0], item) >= 0)
				{
					SortedQueue.Insert(0, item);
				}
				else
				{
					var index = SortedQueue.BinarySearch(item, BucketComparer);
					if (index < 0)
					{
						index = ~index;
					}
					SortedQueue.Insert(index, item);
				}
			}

			private void ReadLines(System.IO.StreamReader streamReader, object streamReaderLock, int maxLines, ref string[] lines)
			{
				var lineIndex = 0;

				lock (streamReaderLock)
				{
					while (!streamReader.EndOfStream && (lineIndex < maxLines))
					{
						var line = streamReader.ReadLine();
						if (!string.IsNullOrEmpty(line))
						{
							lines[lineIndex++] = line;
						}
					}
				}

				if (lineIndex < maxLines)
				{
					Array.Resize(ref lines, lineIndex);
				}
			}

			public bool MoveNext()
			{
				var result = false;

				if ((BucketStreams == null) || (SortedQueue == null))
				{
					BucketStreams = new List<System.IO.Stream>();

					#region Create buckets
					{
						var bucketLock = new object();

						Action<ISI.Extensions.Parsers.IRecordParserResponse<TRecord>[], int> addBucket = (bucketRecords, recordCount) =>
						{
							if (bucketRecords.Any())
							{
								if (bucketRecords.Length > recordCount)
								{
									Array.Resize(ref bucketRecords, recordCount);
								}

								Array.Sort(bucketRecords, RecordComparer);

								System.IO.Stream bucketStream = new ISI.Extensions.Stream.TempFileStream();
								var streamWriter = new System.IO.StreamWriter(bucketStream);

								foreach (var bucketRecord in bucketRecords)
								{
									streamWriter.WriteLine(bucketRecord.Source);
								}

								lock (bucketLock)
								{
									BucketStreams.Add(bucketStream);
								}

								streamWriter = null;
								bucketStream = null;
							}
						};

						{
							var streamReaderLock = new object();
							var streamReader = new System.IO.StreamReader(SourceStream);

							const int maxLines = 500;

							var maxBucketSize = MaxBucketSize - maxLines;
							System.Threading.Tasks.Parallel.For(0, MaxDegreeOfParallelismForBucketCreation, (threadIndex, parallelState) =>
								{
									var recordCount = 0;
									var records = new ISI.Extensions.Parsers.IRecordParserResponse<TRecord>[MaxBucketSize];

									var lines = new string[maxLines];

									while (lines.Any())
									{
										ReadLines(streamReader, streamReaderLock, maxLines, ref lines);

										foreach (var line in lines)
										{
											var parsedResult = RecordParser.Read(null, line);
											if (parsedResult.Success)
											{
												records[recordCount++] = parsedResult;

												if (recordCount >= maxBucketSize)
												{
													addBucket(records, recordCount);
													recordCount = 0;
												}
											}
										}
									}

									addBucket(records, recordCount);
								});

							streamReader = null;
						}
					}
					#endregion

					SortedQueue = new List<(ISI.Extensions.Parsers.IRecordParserResponse<TRecord> RecordParserResponse, System.IO.StreamReader StreamReader)>();

					#region Seed Sorted Queue
					{
						foreach (var bucketStream in BucketStreams)
						{
							bucketStream.Rewind();

							var streamReader = new System.IO.StreamReader(bucketStream);

							var parsedResult = RecordParser.Read(null, streamReader);
							if (parsedResult.Success)
							{
								AddToSortedQueue(parsedResult, streamReader);
							}

							streamReader = null;
						}
					}
					#endregion
				}

				if (SortedQueue.Any())
				{
					result = true;

					var queueItem = SortedQueue.FirstOrDefault();

					Current = queueItem.RecordParserResponse.Record;

					SortedQueue.RemoveAt(0);

					var parsedResult = RecordParser.Read(null, queueItem.StreamReader);
					if (parsedResult.Success)
					{
						AddToSortedQueue(parsedResult, queueItem.StreamReader);
					}
					else
					{
						queueItem.StreamReader.BaseStream.Dispose();
						queueItem.StreamReader.Dispose();
					}
				}

				return result;
			}

			public TRecord Current { get; private set; }

			object System.Collections.IEnumerator.Current => Current;

			public void Dispose()
			{
				Reset();
			}
		}

		public int MaxDegreeOfParallelismForBucketCreation { get; set; }
		public int MaxBucketSize { get; set; }
		protected System.IO.Stream SourceStream { get; }
		protected ISI.Extensions.Parsers.IRecordParser<TRecord> RecordParser { get; }
		protected ColumnSortDefinitionCollection ColumnSortDefinitions { get; }

		public SortedRecordParserReader(System.IO.Stream sourceStream, ISI.Extensions.Parsers.IRecordParser<TRecord> recordParser, ColumnSortDefinitionCollection columnSortDefinitions)
		{
			MaxDegreeOfParallelismForBucketCreation = Environment.ProcessorCount;
			MaxBucketSize = 2000000;
			SourceStream = sourceStream;
			RecordParser = recordParser;
			ColumnSortDefinitions = columnSortDefinitions;
		}

		public IEnumerator<TRecord> GetEnumerator()
		{
			return new SortedRecordParserReaderEnumerator(SourceStream, RecordParser, ColumnSortDefinitions, MaxDegreeOfParallelismForBucketCreation, MaxBucketSize);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}