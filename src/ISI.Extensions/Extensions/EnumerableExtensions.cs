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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISI.Extensions.Extensions
{
	[System.Diagnostics.DebuggerStepThrough]
	public static partial class EnumerableExtensions
	{
		public static IEnumerable<IEnumerable<TValue>> Chunker<TValue>(this IEnumerable<TValue> values, int chunkSize)
		{
			using (var chunkMasterEnumerator = new ChunkMasterEnumerator<TValue>(values.GetEnumerator(), chunkSize))
			{
				var endOfChunks = false;

				while (!endOfChunks)
				{
					chunkMasterEnumerator.Lock(chunkMasterEnumerator.CatchRemainingChunk);

					if (chunkMasterEnumerator.GetNextChunkEnumerable(out var chunkEnumerable))
					{
						yield return chunkEnumerable;
					}
					else
					{
						endOfChunks = true;
					}
				}
			}
		}

		private class ChunkMasterEnumerator<TValue> : IEnumerator<TValue>
		{
			private System.Threading.Semaphore _semaphore = null;

			private ChunkEnumerator<TValue> _chunkEnumerator = null;

			private readonly IEnumerator<TValue> _enumerator;
			private readonly int _chunkSize;

			private bool _hasNextValue;
			private TValue _nextValue;
			private TValue _currentValue;

			public bool EndOfValues { get; internal set; }

			public ChunkMasterEnumerator(IEnumerator<TValue> enumerator, int chunkSize)
			{
				_enumerator = enumerator;
				_chunkSize = chunkSize;
				_hasNextValue = false;
				EndOfValues = false;
			}

			public void Reset()
			{
				_semaphore?.Release();
				_semaphore = null;
				_enumerator.Reset();
				_hasNextValue = false;
				EndOfValues = false;
			}

			public bool MoveNext()
			{
				if (!EndOfValues)
				{
					if (_hasNextValue)
					{
						_currentValue = _nextValue;
					}

					if (_enumerator.MoveNext())
					{
						_nextValue = _enumerator.Current;

						if (!_hasNextValue)
						{
							_currentValue = _nextValue;

							if (_enumerator.MoveNext())
							{
								_nextValue = _enumerator.Current;
								_hasNextValue = true;
							}
							else
							{
								_hasNextValue = false;
								EndOfValues = true;
							}
						}

						return true;
					}

					EndOfValues = true;

					if (_hasNextValue)
					{
						_currentValue = _nextValue;
						_hasNextValue = false;

						return true;
					}
				}

				return false;
			}

			public TValue Current => _currentValue;

			object IEnumerator.Current => Current;

			internal void CatchRemainingChunk()
			{
				_chunkEnumerator?.CatchRemainingChunk();
			}

			internal void Lock(Action action)
			{
				if (_semaphore == null)
				{
					_semaphore = new(0, 1);
				}
				else
				{
					_semaphore.WaitOne();
				}

				action();

				_semaphore.Release();
			}

			internal bool GetNextChunkEnumerable(out ChunkEnumerable<TValue> chunkEnumerable)
			{
				if (!EndOfValues)
				{
					if (_hasNextValue)
					{
						chunkEnumerable = new(this);

						return true;
					}
					
					if (_enumerator.MoveNext())
					{
						_nextValue = _enumerator.Current;
						_hasNextValue = true;

						chunkEnumerable = new(this);

						return true;
					}
				}

				//EndOfValues = true;
				chunkEnumerable = null;

				return false;
			}

			internal ChunkEnumerator<TValue> GetNextChunkEnumerator()
			{
				return (_chunkEnumerator = new(this, _chunkSize));
			}

			public void Dispose()
			{
				_enumerator?.Dispose();
			}
		}

		private class ChunkEnumerator<TValue> : IEnumerator<TValue>
		{
			private readonly ChunkMasterEnumerator<TValue> _chunkMasterEnumerator;
			private readonly int _chunkSize;
			private int _chunkIndex = 0;

			private IEnumerable<TValue> _enumerable = null;
			private IEnumerator<TValue> _enumerator = null;

			internal ChunkEnumerator(ChunkMasterEnumerator<TValue> chunkMasterEnumerator, int chunkSize)
			{
				_chunkMasterEnumerator = chunkMasterEnumerator;
				_chunkSize = chunkSize;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}

			public TValue Current { get; private set; }

			object IEnumerator.Current => Current;

			public bool MoveNext()
			{
				if (_chunkIndex < _chunkSize)
				{
					if (_enumerable == null)
					{
						if (_chunkMasterEnumerator.EndOfValues)
						{
							return false;
						}

						bool? response = null;

						_chunkMasterEnumerator.Lock(() =>
						{
							if (_enumerator == null)
							{
								if (!_chunkMasterEnumerator.EndOfValues)
								{
									response = _chunkMasterEnumerator.MoveNext();

									if (response.Value)
									{
										Current = _chunkMasterEnumerator.Current;

										_chunkIndex++;
									}
								}
							}
						});

						if (response.HasValue)
						{
							return response.Value;
						}
					}

					if ((_enumerator?.MoveNext()) ?? false)
					{
						Current = _enumerator.Current;

						_chunkIndex++;

						return true;
					}
				}

				return false;
			}

			internal void CatchRemainingChunk()
			{
				if ((_enumerable == null) && !_chunkMasterEnumerator.EndOfValues && (_chunkIndex < _chunkSize))
				{
					var chunkIndex = _chunkIndex;

					var enumerable = new List<TValue>();

					while ((chunkIndex < _chunkSize) && !_chunkMasterEnumerator.EndOfValues)
					{
						if (_chunkMasterEnumerator.MoveNext())
						{
							enumerable.Add(_chunkMasterEnumerator.Current);

							chunkIndex++;
						}
					}

					_enumerable = enumerable;
					_enumerator = _enumerable.GetEnumerator();

					if (!enumerable.Any())
					{
						_chunkMasterEnumerator.EndOfValues = true;
					}
				}
			}

			void IDisposable.Dispose()
			{
				_enumerator?.Dispose();
				_enumerator = null;
			}
		}

		private class ChunkEnumerable<TValue> : IEnumerable<TValue>
		{
			private readonly ChunkMasterEnumerator<TValue> _chunkMasterEnumerator;
			private readonly IEnumerator<TValue> _enumerator;

			internal ChunkEnumerable(ChunkMasterEnumerator<TValue> chunkMasterEnumerator)
			{
				_chunkMasterEnumerator = chunkMasterEnumerator;
				_enumerator = _chunkMasterEnumerator.GetNextChunkEnumerator();
			}

			public IEnumerator<TValue> GetEnumerator() => _enumerator;

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

























		public static IEnumerable<TValue> DistinctByProperty<TValue, TProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property, IComparer<TValue> comparer)
		{
			return values.GroupBy(property).Select(groupedValues => groupedValues.OrderBy(value => value, comparer).First());
		}

		public static IEnumerable<TValue> DistinctByProperty<TValue, TProperty, TSortByProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property, Func<TValue, TSortByProperty> sortByProperty, IComparer<TSortByProperty> sortByPropertyComparer)
		{
			return values.GroupBy(property).Select(groupedValues => groupedValues.OrderBy(sortByProperty, sortByPropertyComparer).First());
		}

		public static IEnumerable<TValue> DistinctByProperty<TValue, TProperty, TSortByProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property, Func<TValue, TSortByProperty> sortByProperty)
		{
			return values.GroupBy(property).Select(groupedValues => groupedValues.OrderBy(sortByProperty).First());
		}

		public static IEnumerable<TValue> DistinctByProperty<TValue, TProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property)
		{
			return values.GroupBy(property).Select(groupedValues => groupedValues.First());
		}

		public static bool Contains(this IEnumerable<string> values, string value, StringComparer stringComparer = null)
		{
			return (values.IndexOf(value, stringComparer) >= 0);
		}

		public static int IndexOf(this IEnumerable<string> values, string value, StringComparer stringComparer = null)
		{
			var result = 0;

			stringComparer ??= StringComparer.OrdinalIgnoreCase;

			foreach (var _value in values)
			{
				if (stringComparer.Equals(_value, value))
				{
					return result;
				}
				result++;
			}

			return -1;
		}

		public static double Median<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> property)
		{
			return values.Select(property).Median();
		}

		public static double Median(this IEnumerable<bool> values)
		{
			var sortedValues = values.OrderBy(value => value).ToArray();

			//get the median
			var size = sortedValues.Length;

			var mid = size / 2;

			var median = (size % 2 != 0) ? ((double)(sortedValues[mid] ? 1 : 0)) : (((double)(sortedValues[mid] ? 1 : 0)) + ((double)(sortedValues[mid - 1] ? 1 : 0))) / 2;

			return median;
		}

		public static double Median<TValue>(this IEnumerable<TValue> values, Func<TValue, double> property)
		{
			return values.Select(property).Median();
		}

		public static double Median(this IEnumerable<double> values)
		{
			var sortedValues = values.OrderBy(value => value).ToArray();

			//get the median
			var size = sortedValues.Length;

			var mid = size / 2;

			var median = (size % 2 != 0) ? sortedValues[mid] : (sortedValues[mid] + sortedValues[mid - 1]) / 2;

			return median;
		}

		public static decimal Median<TValue>(this IEnumerable<TValue> values, Func<TValue, decimal> property)
		{
			return values.Select(property).Median();
		}

		public static decimal Median(this IEnumerable<decimal> values)
		{
			var sortedValues = values.OrderBy(value => value).ToArray();

			//get the median
			var size = sortedValues.Length;

			var mid = size / 2;

			var median = (size % 2 != 0) ? sortedValues[mid] : (sortedValues[mid] + sortedValues[mid - 1]) / 2;

			return median;
		}
	}
}