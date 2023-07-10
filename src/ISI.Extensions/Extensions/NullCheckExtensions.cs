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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Extensions
{
	public enum NullCheckResult
	{
		ReturnDefault,
		ReturnNull,
	}

	public enum NullCheckCollectionResult
	{
		ReturnNull,
		Empty,
	}

	public enum NullCheckDictionaryResult
	{
		ReturnNull,
		Empty,
	}

	[System.Diagnostics.DebuggerStepThrough]
	public static class NullCheckExtensions
	{
		public static IEnumerable<IEnumerable<TValue>> NullCheckedChunk<TValue>(this IEnumerable<TValue> values, int size, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.Empty)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return (Array.Empty<TValue>()).Chunker(size);
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.Chunker(size);
		}

		public static void NullCheckedSetProperty<TObject, TProperty>(this TObject @object, System.Linq.Expressions.Expression<Func<TObject, TProperty>> property, TProperty propertyValue)
		{
			if (@object != null)
			{
				var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

				propertyInfo.SetValue(@object, propertyValue);
			}
		}

		public static bool NullCheckedContains<TValue>(this IEnumerable<TValue> values, TValue value)
		{
			if (values == null)
			{
				return false;
			}

			return values.Contains(value);
		}

		public static bool NullCheckedContains<TValue>(this IEnumerable<TValue> values, TValue value, IEqualityComparer<TValue> comparer)
		{
			if (values == null)
			{
				return false;
			}

			return values.Contains(value, comparer);
		}

		public static int NullCheckedCount<TValue>(this IEnumerable<TValue> values)
		{
			if (values == null)
			{
				return 0;
			}

			return values.Count();
		}

		public static bool NullCheckedAny(this System.Collections.Specialized.NameValueCollection values)
		{
			if (values == null)
			{
				return false;
			}

			return values.HasKeys();
		}

		public static bool NullCheckedAny<TValue>(this IEnumerable<TValue> values)
		{
			if (values == null)
			{
				return false;
			}

			return values.Any();
		}

		public static bool NullCheckedAny<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> predicate)
		{
			if (values == null)
			{
				return false;
			}

			return values.Any(predicate);
		}

		public static bool NullCheckedAll<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> predicate)
		{
			if (values == null)
			{
				return false;
			}

			return values.All(predicate);
		}



		public static TResult IsNull<TValue, TResult>(this TValue value, Func<TValue, TResult> property)
		{
			if (value != null)
			{
				return property(value);
			}

			return default;
		}

		public static TResult IsNull<TValue, TResult>(this TValue value, Func<TValue, TResult> property, TResult defaultValue)
		{
			if (value != null)
			{
				return property(value);
			}

			return defaultValue;
		}

		public static TResult NullCheckedConvert<TValue, TResult>(this TValue value, Func<TValue, TResult> converter)
		{
			if (value == null)
			{
				return default;
			}

			return converter(value);
		}

		public static IEnumerable<TResult> NullCheckedCast<TResult>(this System.Collections.IEnumerable values, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<TResult>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.Cast<TResult>();
		}

		public static TValue NullCheckedFirstOrDefault<TValue>(this IEnumerable<TValue> values)
		{
			if (values == null)
			{
				return default;
			}

			return values.FirstOrDefault();
		}

		public static TValue NullCheckedFirstOrDefault<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> predicate)
		{
			if (values == null)
			{
				return default;
			}

			return values.FirstOrDefault(predicate);
		}

		public static TValue NullCheckedLastOrDefault<TValue>(this IEnumerable<TValue> values)
		{
			if (values == null)
			{
				return default;
			}

			return values.LastOrDefault();
		}

		public static TValue NullCheckedLastOrDefault<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> predicate)
		{
			if (values == null)
			{
				return default;
			}

			return values.LastOrDefault(predicate);
		}

		public static TValue[] ToNullCheckedArray<TValue>(this IEnumerable<TValue> values, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<TValue>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.ToArray();
		}

		public static TResult[] ToNullCheckedArray<TValue, TResult>(this IEnumerable<TValue> values, Func<TValue, TResult> converter, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<TResult>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.Select(converter).ToArray();
		}

		public static TValue[] ToNullCheckedDistinctArray<TValue>(this IEnumerable<TValue> values, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			return ToNullCheckedArray(values?.Distinct(), ifNullReturn);
		}

		public static IList<TValue> ToNullCheckedList<TValue>(this IEnumerable<TValue> values, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return new List<TValue>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.ToList();
		}

		public static IList<TResult> ToNullCheckedList<TValue, TResult>(this IEnumerable<TValue> values, Func<TValue, TResult> converter, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return new List<TResult>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.Select(converter).ToList();
		}

		public static HashSet<TValue> ToNullCheckedHashSet<TValue>(this IEnumerable<TValue> values, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return new();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return new(values);
		}

		public static HashSet<TResult> ToNullCheckedHashSet<TValue, TResult>(this IEnumerable<TValue> values, Func<TValue, TResult> converter, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return new();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return new(values.Select(converter));
		}

		public static TValueCollection ToNullCheckedCollection<TValue, TValueCollection>(this IEnumerable<TValue> values, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
			where TValueCollection : class, ICollection<TValue>, new()
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return default;
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return Activator.CreateInstance(typeof(TValueCollection), values) as TValueCollection;
		}

		public static TResultCollection ToNullCheckedCollection<TValue, TResult, TResultCollection>(this IEnumerable<TValue> values, Func<TValue, TResult> converter, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
			where TResultCollection : class, ICollection<TResult>, new()
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return default;
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return Activator.CreateInstance(typeof(TResultCollection), values.Select(converter)) as TResultCollection;
		}

		public static IDictionary<TKey, TValue> ToNullCheckedDictionary<TValue, TKey>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, IEqualityComparer<TKey> comparer, NullCheckDictionaryResult ifNullReturn = NullCheckDictionaryResult.ReturnNull)
		{
			return values.ToNullCheckedDictionary(keySelector, value => value, comparer, ifNullReturn);
		}

		public static IDictionary<TKey, TElement> ToNullCheckedDictionary<TValue, TKey, TElement>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, Func<TValue, TElement> elementSelector, IEqualityComparer<TKey> comparer, NullCheckDictionaryResult ifNullReturn = NullCheckDictionaryResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckDictionaryResult.ReturnNull:
						return null;
					case NullCheckDictionaryResult.Empty:
						return new Dictionary<TKey, TElement>(comparer);
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.ToDictionary(keySelector, elementSelector, comparer);
		}

		public static IDictionary<TKey, TValue> ToNullCheckedDictionary<TValue, TKey>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, NullCheckDictionaryResult ifNullReturn = NullCheckDictionaryResult.ReturnNull)
		{
			return values.ToNullCheckedDictionary(keySelector, value => value, ifNullReturn);
		}

		public static IDictionary<TKey, TElement> ToNullCheckedDictionary<TValue, TKey, TElement>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, Func<TValue, TElement> elementSelector, NullCheckDictionaryResult ifNullReturn = NullCheckDictionaryResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckDictionaryResult.ReturnNull:
						return null;
					case NullCheckDictionaryResult.Empty:
						return new Dictionary<TKey, TElement>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.ToDictionary(keySelector, elementSelector);
		}

		public static KeyValuePair<TKey, TElement>[] ToNullCheckedKeyValueArray<TValue, TKey, TElement>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, Func<TValue, TElement> elementSelector, NullCheckDictionaryResult ifNullReturn = NullCheckDictionaryResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckDictionaryResult.ReturnNull:
						return null;
					case NullCheckDictionaryResult.Empty:
						return Array.Empty<KeyValuePair<TKey, TElement>>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.Select(value => new KeyValuePair<TKey, TElement>(keySelector(value), elementSelector(value))).ToArray();
		}

		public static bool NullCheckedTryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
		{
			if (dictionary == null)
			{
				value = default;

				return false;
			}

			return dictionary.TryGetValue(key, out value);
		}

		public static TValue NullCheckedGetValueIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary == null)
			{
				return default;
			}

			return dictionary.GetValueIfExists<TKey, TValue>(key);
		}

		public static TValue NullCheckedGetValueIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue keyNotExistsValue)
		{
			if (dictionary == null)
			{
				return keyNotExistsValue;
			}

			return dictionary.GetValueIfExists<TKey, TValue>(key, keyNotExistsValue);
		}

		public static TValue NullCheckedGetValueIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<IDictionary<TKey, TValue>, TKey, TValue> getKeyNotExistsValue)
		{
			if (dictionary == null)
			{
				return getKeyNotExistsValue(dictionary, key);
			}

			return dictionary.GetValueIfExists<TKey, TValue>(key, getKeyNotExistsValue);
		}


		public static IEnumerable<TResult> NullCheckedSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (source == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<TResult>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return source.Select(selector);
		}

		public static IEnumerable<TResult> NullCheckedSelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (source == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<TResult>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return source.SelectMany(selector);
		}

		public static IEnumerable<TValue> NullCheckedWhere<TValue>(this IEnumerable<TValue> source, Func<TValue, bool> predicate, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.Empty)
		{
			if (source == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<TValue>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return source.Where(predicate);
		}

		public static TValue NullCheckedMin<TValue>(this IEnumerable<TValue> values, TValue ifNullReturn = default, TValue ifEmptyReturn = default)
		{
			if (values == null)
			{
				return ifNullReturn;
			}

			if (values.Any())
			{
				return values.Min();
			}

			return ifEmptyReturn;
		}

		public static TValue NullCheckedMin<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> predicate, TValue ifNullReturn = default, TValue ifEmptyReturn = default)
		{
			if (values == null)
			{
				return ifNullReturn;
			}

			var filteredValues = values.Where(predicate);

			if (filteredValues.Any())
			{
				return filteredValues.Min();
			}

			return ifEmptyReturn;
		}

		public static TValue NullCheckedMax<TValue>(this IEnumerable<TValue> values, TValue ifNullReturn = default, TValue ifEmptyReturn = default)
		{
			if (values == null)
			{
				return ifNullReturn;
			}

			if (values.Any())
			{
				return values.Max();
			}

			return ifEmptyReturn;
		}

		public static TValue NullCheckedMax<TValue>(this IEnumerable<TValue> values, Func<TValue, bool> predicate, TValue ifNullReturn = default, TValue ifEmptyReturn = default)
		{
			if (values == null)
			{
				return ifNullReturn;
			}

			var filteredValues = values.Where(predicate);

			if (filteredValues.Any())
			{
				return filteredValues.Max();
			}

			return ifEmptyReturn;
		}


		public static void NullCheckedEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
		{
			if (source != null)
			{
				foreach (var item in source)
				{
					action(item);
				}
			}
		}

		public static IEnumerable<IGrouping<TKey, TValue>> NullCheckedGroup<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<IGrouping<TKey, TValue>>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.GroupBy(keySelector);
		}

		public static IEnumerable<IGrouping<TKey, TValue>> NullCheckedGroup<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, IEqualityComparer<TKey> comparer, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.ReturnNull)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return Array.Empty<IGrouping<TKey, TValue>>();
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.GroupBy(keySelector, comparer);
		}

		public static IEnumerable<TValue> NullCheckedDistinct<TValue>(this IEnumerable<TValue> values)
		{
			return values?.Distinct();
		}

		public static IEnumerable<TValue> NullCheckedDistinct<TValue>(this IEnumerable<TValue> values, IEqualityComparer<TValue> comparer)
		{
			return values?.Distinct(comparer);
		}

		public static IEnumerable<TValue> NullCheckedDistinctByProperty<TValue, TProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property, IComparer<TValue> comparer)
		{
			return values?.DistinctByProperty(property, comparer);
		}

		public static IEnumerable<TValue> NullCheckedDistinctByProperty<TValue, TProperty, TSortByProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property, Func<TValue, TSortByProperty> sortByProperty, IComparer<TSortByProperty> sortByPropertyComparer)
		{
			return values?.DistinctByProperty(property, sortByProperty, sortByPropertyComparer);
		}

		public static IEnumerable<TValue> NullCheckedDistinctByProperty<TValue, TProperty, TSortByProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property, Func<TValue, TSortByProperty> sortByProperty)
		{
			return values?.DistinctByProperty(property, sortByProperty);
		}

		public static IEnumerable<TValue> NullCheckedDistinctByProperty<TValue, TProperty>(this IEnumerable<TValue> values, Func<TValue, TProperty> property)
		{
			return values?.DistinctByProperty(property);
		}


		public static IOrderedEnumerable<TValue> NullCheckedOrderBy<TValue, TKey>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.Empty)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return (Array.Empty<TValue>()).OrderBy(keySelector);
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.OrderBy(keySelector);
		}

		public static IOrderedEnumerable<TValue> NullCheckedOrderBy<TValue, TKey>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, IComparer<TKey> comparer, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.Empty)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return (Array.Empty<TValue>()).OrderBy(keySelector, comparer);
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.OrderBy(keySelector, comparer);
		}

		public static IOrderedEnumerable<TValue> NullCheckedOrderByDescending<TValue, TKey>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.Empty)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return (Array.Empty<TValue>()).OrderByDescending(keySelector);
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.OrderByDescending(keySelector);
		}

		public static IOrderedEnumerable<TValue> NullCheckedOrderByDescending<TValue, TKey>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector, IComparer<TKey> comparer, NullCheckCollectionResult ifNullReturn = NullCheckCollectionResult.Empty)
		{
			if (values == null)
			{
				switch (ifNullReturn)
				{
					case NullCheckCollectionResult.ReturnNull:
						return null;
					case NullCheckCollectionResult.Empty:
						return (Array.Empty<TValue>()).OrderByDescending(keySelector, comparer);
					default:
						throw new ArgumentOutOfRangeException(nameof(ifNullReturn), ifNullReturn, null);
				}
			}

			return values.OrderByDescending(keySelector, comparer);
		}
	}
}
