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
	public class SimpleKeyValueStorage : ISI.Extensions.KeyValueStorage.IKeyValueStorageReader
	{
		protected string FullName { get; }

		private IDictionary<string, string> _values;
		protected IDictionary<string, string> Values => _values ??= GetValues();

		protected IDictionary<string, string> GetValues()
		{
			var values = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

			if (System.IO.File.Exists(FullName))
			{
				var lines = System.IO.File.ReadAllLines(FullName).ToList();

				lines.RemoveAll(string.IsNullOrWhiteSpace);

				foreach (var keyValue in lines.Select(line => line.Split([':', '\t'], 2)))
				{
					var key = keyValue.First().Trim();

					var value = string.Empty;
					if (keyValue.Length > 1)
					{
						value = keyValue[1].Trim();
					}

					if (values.ContainsKey(key))
					{
						throw new($"key: \"{key}\" already exists with value of \"{values[key]}\" cannot add value \"{value}\"");
					}

					values.Add(key, value);
				}
			}

			return values;
		}

		public SimpleKeyValueStorage(string fullName)
		{
			FullName = fullName;
		}

		public string GetValue(string key, string defaultValue = null)
		{
			if (Values.TryGetValue(key, out var value))
			{
				return value;
			}

			return defaultValue;
		}

		public bool TryGetValue(string key, out string value) => Values.TryGetValue(key, out value);

		public IEnumerable<string> Keys => Values.Keys;

		public void Save()
		{
			if (System.IO.File.Exists(FullName))
			{
				System.IO.File.Delete(FullName);
			}

			System.IO.File.WriteAllText(FullName, string.Join("\n", Values.Select(keyValue => $"{keyValue.Key}\t{keyValue.Value}")));
		}
	}
}
