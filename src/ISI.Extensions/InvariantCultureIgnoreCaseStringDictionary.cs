#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Text;

namespace ISI.Extensions
{
	public class InvariantCultureIgnoreCaseStringDictionary<TValue> : IDictionary<string, TValue>
	{
		private readonly IDictionary<string, TValue> _dictionary = new Dictionary<string, TValue>(StringComparer.InvariantCultureIgnoreCase);

		public InvariantCultureIgnoreCaseStringDictionary()
		{

		}

		public InvariantCultureIgnoreCaseStringDictionary(IEnumerable<KeyValuePair<string, TValue>> values)
		{
			if (values != null)
			{
				foreach (var keyValue in values)
				{
					_dictionary.Add(keyValue);
				}
			}
		}

		public void Clear() => _dictionary.Clear();
		
		public bool Remove(KeyValuePair<string, TValue> item) => _dictionary.Remove(item);

		public void Add(KeyValuePair<string, TValue> item) => _dictionary.Add(item);
		public void Add(string key, TValue value) => _dictionary.Add(key, value);
		
		public bool Contains(KeyValuePair<string, TValue> item) => _dictionary.Contains(item);
		public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
		
		public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

		public bool Remove(string key) => _dictionary.Remove(key);
		
		public bool TryGetValue(string key, out TValue value) => _dictionary.TryGetValue(key, out value);

		public TValue this[string key]
		{
			get => _dictionary[key];
			set => _dictionary[key] = value;
		}

		public ICollection<string> Keys { get; }
		public ICollection<TValue> Values { get; }

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _dictionary.Count;
		public bool IsReadOnly => _dictionary.IsReadOnly;

		public IDictionary<string, TValue> Clone()
		{
			var clone = new Dictionary<string, TValue>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var keyValue in _dictionary)
			{
				clone.Add(keyValue.Key, keyValue.Value);
			}

			return clone;
		}

		public static implicit operator InvariantCultureIgnoreCaseStringDictionary<TValue>(Dictionary<string, TValue> values) => new(values);
		public static implicit operator InvariantCultureIgnoreCaseStringDictionary<TValue>(KeyValuePair<string, TValue>[] values) => new(values);
	}
}
