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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.ConfigurationValueReaders
{
	public class EmbeddedFileConfigurationValueReader : IConfigurationValueReader
	{
		public const string Prefix = "embedded";

		string IConfigurationValueReader.Prefix => Prefix;

		private readonly IDictionary<string, string> _valueByKey = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
		private static readonly object _valueByKeyByKeyLock = new();
		private readonly IDictionary<string, IDictionary<string, string>> _valueByKeyByKey = new Dictionary<string, IDictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);

		public string GetValue(ParsedValue parsedValue)
		{
			var virtualFileVolumesFileProvider = new ISI.Extensions.VirtualFileVolumesFileProvider();

			if (string.IsNullOrWhiteSpace(parsedValue.KeyIndex))
			{
				if (_valueByKey.TryGetValue(parsedValue.Key, out var value))
				{
					return value;
				}

				var fileInfo = virtualFileVolumesFileProvider.GetFileInfo(parsedValue.Key);

				if (fileInfo?.Exists ?? false)
				{
					value = fileInfo.CreateReadStream().TextReadToEnd();
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
							var fileInfo = virtualFileVolumesFileProvider.GetFileInfo(parsedValue.Key);

							if (fileInfo?.Exists ?? false)
							{
								var valuesByKey = fileInfo.CreateReadStream().TextReadToEnd().Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Select(line =>
									{
										var pieces = line.Split([':', '\t'], 2, StringSplitOptions.RemoveEmptyEntries);

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
										throw new($"key: \"{keyValuePair.Key}\" already exists with value of \"{valueByKey[keyValuePair.Key]}\" cannot add value \"{keyValuePair.Value}\"");
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
