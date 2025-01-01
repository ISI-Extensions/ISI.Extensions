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
	public class NginxFileConfigurationValueReader : IConfigurationValueReader
	{
		public const string Prefix = "nginx";

		string IConfigurationValueReader.Prefix => Prefix;

		private static readonly object _valueByKeyByKeyLock = new();
		private readonly IDictionary<string, IDictionary<string, string>> _valueByKeyByKey = new Dictionary<string, IDictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);

		private readonly System.Text.RegularExpressions.Regex _serverProperties = new(@"(?:\s*listen\s*(?<port>\d+)(?:\s*(?<scheme>[a-zA-Z]+))?)");

		public string GetValue(ParsedValue parsedValue)
		{
			if (!_valueByKeyByKey.TryGetValue(parsedValue.Key, out var valueByKey))
			{
				lock (_valueByKeyByKeyLock)
				{
					if (!_valueByKeyByKey.TryGetValue(parsedValue.Key, out valueByKey))
					{
						var fileName = ISI.Extensions.IO.Path.GetFileNameDeMasked(parsedValue.Key);

						if (System.IO.File.Exists(fileName))
						{
							valueByKey = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

							var lines = System.IO.File.ReadLines(fileName).ToNullCheckedArray(line => line.Replace("\t", " "));

							var scheme = (string)null;
							var host = (string)null;
							var port = 0;
							var location = "/";
							foreach (var line in lines)
							{
								if (line.IndexOf(" listen ", StringComparison.InvariantCultureIgnoreCase) >= 0)
								{
									var serverProperties = _serverProperties.Match(line);

									scheme = (string.Equals(serverProperties.Groups["scheme"]?.Value ?? string.Empty, "ssl", StringComparison.InvariantCultureIgnoreCase) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
									port = (serverProperties.Groups["port"]?.Value ?? (string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase) ? "443" : "80")).ToInt();
								}

								if (line.IndexOf(" server_name ", StringComparison.InvariantCultureIgnoreCase) >= 0)
								{
									host = line.Replace(" server_name ", string.Empty, StringComparer.InvariantCultureIgnoreCase).Trim(' ', ';');
								}

								if (line.IndexOf(" location ", StringComparison.InvariantCultureIgnoreCase) >= 0)
								{
									location = line.Replace(" location ", string.Empty, StringComparer.InvariantCultureIgnoreCase).Trim(' ', ';', '{');
								}

								if (line.IndexOf(" proxy_pass ", StringComparison.InvariantCultureIgnoreCase) >= 0)
								{
									var proxyHost = line.Replace(" proxy_pass ", string.Empty, StringComparer.InvariantCultureIgnoreCase).Trim(' ', ';');

									if (!string.IsNullOrWhiteSpace(scheme) && !string.IsNullOrWhiteSpace(host))
									{
										var uri = new UriBuilder(scheme, host, port)
										{
											Path = location,
										};

										foreach (var key in new[] { uri.ToString(), uri.ToString().Trim('/'), uri.Uri.ToString(), uri.Uri.ToString().Trim('/') })
										{
											if (!valueByKey.ContainsKey(key))
											{
												valueByKey.Add(key, proxyHost);
											}
										}
									}

								}
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
				var keyIndex = parsedValue.KeyIndex.Replace("{MachineName}", System.Environment.MachineName.ToLower(), StringComparer.InvariantCultureIgnoreCase);

				if (valueByKey.TryGetValue(keyIndex, out var value))
				{
					return value;
				}
			}

			return parsedValue.DefaultValue;
		}
	}
}
