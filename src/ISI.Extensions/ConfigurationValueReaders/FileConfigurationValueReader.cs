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

namespace ISI.Extensions.ConfigurationValueReaders
{
	public class FileConfigurationValueReader : IConfigurationValueReader
	{
		public const string Prefix = "file";

		string IConfigurationValueReader.Prefix => Prefix;

		private readonly IDictionary<string, string> _valueByKey = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
		private static readonly object _valueByKeyByKeyLock = new();
		private readonly IDictionary<string, IDictionary<string, string>> _valueByKeyByKey = new Dictionary<string, IDictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);

		private string CheckForRedirectFileName(string fileName)
		{
			var redirectFileName = string.Format("{0}.redirect", fileName);

			if (System.IO.File.Exists(redirectFileName))
			{
				var redirectedFileName = System.IO.File.ReadAllText(redirectFileName);

				if (System.IO.File.Exists(redirectedFileName))
				{
					fileName = redirectedFileName;
				}
			}

			return fileName;
		}

		public string GetValue(ParsedValue parsedValue)
		{
			if (string.IsNullOrWhiteSpace(parsedValue.KeyIndex))
			{
				if (_valueByKey.TryGetValue(parsedValue.Key, out var value))
				{
					return value;
				}

				var fileName = CheckForRedirectFileName(ISI.Extensions.IO.Path.GetFileNameDeMasked(parsedValue.Key));

				if (System.IO.File.Exists(fileName))
				{
					value = System.IO.File.ReadAllText(fileName);
				}

				if (!_valueByKey.ContainsKey(parsedValue.Key))
				{
					_valueByKey.Add(parsedValue.Key, value);
				}

				if (!string.IsNullOrWhiteSpace(value))
				{
					return value;
				}
			}
			else
			{
				if (!_valueByKeyByKey.TryGetValue(parsedValue.Key, out var valueByKey))
				{
					lock (_valueByKeyByKeyLock)
					{
						if (!_valueByKeyByKey.TryGetValue(parsedValue.Key, out valueByKey))
						{
							var fileName = CheckForRedirectFileName(ISI.Extensions.IO.Path.GetFileNameDeMasked(parsedValue.Key));

							if (System.IO.File.Exists(fileName))
							{
								var valuesByKey = System.IO.File.ReadAllText(fileName).Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(line =>
								{
									var pieces = line.Split(new[] {':', '\t'}, 2, StringSplitOptions.RemoveEmptyEntries);

									if (pieces.Length == 1)
									{
										return new(pieces[0].Trim(), null);
									}

									return new KeyValuePair<string, string>(pieces[0].Trim(), pieces[1].Trim());
								});

								valueByKey = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

								foreach (var keyValuePair in valuesByKey)
								{
									if (valueByKey.ContainsKey(keyValuePair.Key))
									{
										throw new(string.Format("key: \"{0}\" already exists with value of \"{1}\" cannot add value \"{2}\"", keyValuePair.Key, valueByKey[keyValuePair.Key], keyValuePair.Value));
									}

									valueByKey.Add(keyValuePair);
								}
							}

							if (!_valueByKeyByKey.ContainsKey(parsedValue.Key))
							{
								_valueByKeyByKey.Add(parsedValue.Key, valueByKey);
							}
						}
					}
				}

				if (valueByKey != null)
				{
					if (valueByKey.TryGetValue(parsedValue.KeyIndex, out var value))
					{
						return value;
					}
				}
			}

			return parsedValue.DefaultValue;
		}
	}
}
