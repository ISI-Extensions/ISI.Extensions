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

namespace ISI.Extensions
{
	public class Ini
	{
		public class IniSectionKeyValues : IEnumerable<KeyValuePair<string, string>>
		{
			private readonly IniSections _iniSections;
			private readonly string _sectionKey;
			private readonly IEqualityComparer<string> _comparer;
			private readonly IDictionary<string, string> _values;

			public IniSectionKeyValues(IniSections iniSections, string sectionKey, IEqualityComparer<string> comparer)
			{
				_iniSections = iniSections;
				_sectionKey = sectionKey;
				_comparer = comparer;
				_values = new Dictionary<string, string>(comparer);
			}

			public string this[string key]
			{
				get
				{
					if (!_values.ContainsKey(key))
					{
						_values.Add(key, string.Empty);
					}

					var value = _values[key];

					if (_iniSections.UseDemaskedValue)
					{
						value = ISI.Extensions.ConfigurationValueReader.GetValue(value);
					}

					return value;
				}
				set
				{
					if (!_values.ContainsKey(key))
					{
						_values.Add(key, string.Empty);
					}

					var hasChange = !_comparer.Equals(_values[key], value);

					_values[key] = value;

					if(hasChange)
					{
						_iniSections.OnChange?.Invoke(_sectionKey, key, value);
					}
				}
			}

			IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
			{
				return _values.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value)).GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return _values.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value)).GetEnumerator();
			}
		}

		public class IniSections : IEnumerable<KeyValuePair<string, IniSectionKeyValues>>
		{
			public delegate void OnChangeEvent(string section, string key, string value);

			private readonly IEqualityComparer<string> _comparer;
			private readonly IDictionary<string, IniSectionKeyValues> _sections;

			public OnChangeEvent OnChange { get; set; }
			public bool UseDemaskedValue { get; set; } = false;

			public IniSections()
				: this(StringComparer.InvariantCultureIgnoreCase)
			{
			}
			public IniSections(string content, IEqualityComparer<string> comparer)
				: this(comparer)
			{
				Parse(content);
			}
			public IniSections(IEqualityComparer<string> comparer)
			{
				_comparer = comparer;
				_sections = new Dictionary<string, IniSectionKeyValues>(_comparer);
			}
			public IniSections(string content)
				: this()
			{
				Parse(content);
			}

			public IniSectionKeyValues this[string key]
			{
				get
				{
					if (!_sections.ContainsKey(key))
					{
						_sections.Add(key, new IniSectionKeyValues(this, key, _comparer));
					}

					return _sections[key];
				}
			}

			protected void Parse(string content)
			{
				var onChange = OnChange;
				OnChange = null;

				var lines = content.Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

				IniSectionKeyValues currentSection = null;

				foreach (var line in lines)
				{
					if (line.TrimStart().StartsWith(";"))
					{
						
					}
					else if (line.TrimStart().StartsWith("["))
					{
						var sectionKey = line.Split(new char[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
						currentSection = this[sectionKey];
					}
					else if(line.Contains("="))
					{
						if (currentSection != null)
						{
							var keyValue = line.Split(new char[] {'='}, StringSplitOptions.None, s => s.Trim());
							if (keyValue.Length > 0)
							{
								currentSection[keyValue[0]] = (keyValue.Length > 1 ? keyValue[1] : string.Empty);
							}
						}
					}
				}

				OnChange = onChange;
			}

			IEnumerator<KeyValuePair<string, IniSectionKeyValues>> IEnumerable<KeyValuePair<string, IniSectionKeyValues>>.GetEnumerator()
			{
				return _sections.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return _sections.GetEnumerator();
			}
		}
	}
}
