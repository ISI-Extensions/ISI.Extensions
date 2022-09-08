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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.WebClient
{
	public class Header
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}

	public delegate void OnHasContent(HeaderCollection headers, string content);

	public class HeaderCollection : List<Header>
	{
		internal class Keys
		{
			public const string Authorization = nameof(Authorization);
			public const string UserAgent = "User-Agent";
			public const string ContentType = "Content-Type";
			public const string Accept = nameof(Accept);
			public const string Expect = nameof(Expect);
			public const string Referer = nameof(Referer);
			public const string AuthorizationToken = nameof(AuthorizationToken);
			public const string Basic = "Basic ";
			public const string Bearer = "Bearer ";
			public const string ContentDisposition = "Content-Disposition";
		}

		public event OnHasContent OnHasContent = null;

		public HeaderCollection()
		{

		}
		public HeaderCollection(IEnumerable<Header> headers)
		{
			AddRange(headers);
		}
		public HeaderCollection(System.Net.WebHeaderCollection headers)
		{
			if (headers != null)
			{
				foreach (var key in headers.AllKeys)
				{
					Add(key, headers[key]);
				}
			}
		}

		public void ProcessContent(Func<string> getContent)
		{
			OnHasContent?.Invoke(this, getContent());
		}

		public new void AddRange(IEnumerable<Header> headers)
		{
			foreach (var header in headers)
			{
				Add(header.Key, header.Value);
			}
		}

		public Header Add(string key, string value)
		{
			var header = new Header()
			{
				Key = key,
				Value = value
			};

			this.Add(header);

			return header;
		}

		public Header AddAuthorizationToken(string authenticationToken)
		{
			var header = new Header()
			{
				Key = Keys.AuthorizationToken,
				Value = authenticationToken
			};

			this.Add(header);

			return header;
		}

		public Header AddBasicAuthentication(string userName, string password)
		{
			var authenticationToken = string.Format("{0}{1}", Keys.Basic, Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", userName, password))));

			var header = new Header()
			{
				Key = Keys.Authorization,
				Value = authenticationToken
			};

			this.Add(header);

			return header;
		}

		public Header AddBearerAuthentication(string token)
		{
			var authenticationToken = string.Format("{0}{1}", Keys.Bearer, token);

			var header = new Header()
			{
				Key = Keys.Authorization,
				Value = authenticationToken
			};

			this.Add(header);

			return header;
		}

		public string UserAgent
		{
			get => this.FirstOrDefault(header => string.Equals(header.Key, Keys.UserAgent, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
			set
			{
				this.RemoveAll(header => string.Equals(header.Key, Keys.UserAgent, StringComparison.InvariantCultureIgnoreCase));
				Add(Keys.UserAgent, value);
			}
		}

		private string _preferredContentType = null;
		public string PreferredContentType => _preferredContentType;
		public string ContentType
		{
			get => this.FirstOrDefault(header => string.Equals(header.Key, Keys.ContentType, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
			set
			{
				this.RemoveAll(header => string.Equals(header.Key, Keys.ContentType, StringComparison.InvariantCultureIgnoreCase));
				Add(Keys.ContentType, value);
				_preferredContentType = value;
			}
		}

		private string _preferredAccept = null;
		public string PreferredAccept => _preferredAccept;
		public string Accept
		{
			get => this.FirstOrDefault(header => string.Equals(header.Key, Keys.Accept, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
			set
			{
				this.RemoveAll(header => string.Equals(header.Key, Keys.Accept, StringComparison.InvariantCultureIgnoreCase));
				Add(Keys.Accept, value);
				_preferredAccept = value;
			}
		}

		public string Expect
		{
			get => this.FirstOrDefault(header => string.Equals(header.Key, Keys.Expect, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
			set
			{
				this.RemoveAll(header => string.Equals(header.Key, Keys.Expect, StringComparison.InvariantCultureIgnoreCase));
				Add(Keys.Expect, value);
			}
		}

		public string Referer
		{
			get => this.FirstOrDefault(header => string.Equals(header.Key, Keys.Referer, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
			set
			{
				this.RemoveAll(header => string.Equals(header.Key, Keys.Referer, StringComparison.InvariantCultureIgnoreCase));
				Add(Keys.Referer, value);
			}
		}

		public bool TryGetValue(string key, out string value)
		{
			var header = this.FirstOrDefault(header => string.Equals(header.Key, key, StringComparison.InvariantCultureIgnoreCase));

			if (header == null)
			{
				value = null;
				return false;
			}

			value = header.Value;
			return true;
		}

		public void ApplyToWebRequest(System.Net.HttpWebRequest request)
		{
			foreach (var header in this.Where(h => !string.IsNullOrEmpty(h.Key)))
			{
				if (string.Equals(header.Key, Keys.UserAgent, StringComparison.InvariantCultureIgnoreCase))
				{
					request.UserAgent = header.Value;
				}
				else if (string.Equals(header.Key, Keys.ContentType, StringComparison.InvariantCultureIgnoreCase))
				{
					request.ContentType = header.Value;
				}
				else if (string.Equals(header.Key, Keys.Accept, StringComparison.InvariantCultureIgnoreCase))
				{
					request.Accept = header.Value;
				}
				else if (string.Equals(header.Key, Keys.Expect, StringComparison.InvariantCultureIgnoreCase))
				{
					request.Expect = header.Value;
				}
				else if (string.Equals(header.Key, Keys.Referer, StringComparison.InvariantCultureIgnoreCase))
				{
					request.Referer = header.Value;
				}
				else if (string.Equals(header.Key, Keys.AuthorizationToken, StringComparison.InvariantCultureIgnoreCase))
				{
					request.Headers[Keys.Authorization] = header.Value;
				}
				else if (string.Equals(header.Key, Keys.Authorization, StringComparison.InvariantCultureIgnoreCase))
				{
					if (header.Value.StartsWith(Keys.Basic, StringComparison.InvariantCultureIgnoreCase))
					{
						request.Headers[Keys.Authorization] = header.Value;
					}
					else if (header.Value.StartsWith(Keys.Bearer, StringComparison.InvariantCultureIgnoreCase))
					{
						request.Headers[Keys.Authorization] = header.Value;
					}
					else
					{
						var values = header.Value.Split(new[] { ' ', ':' });

						if (values.Length >= 2)
						{
							request.Credentials = new System.Net.NetworkCredential(values[0], values[1]);
							request.PreAuthenticate = true;
						}
						else
						{
							var authorization = Encoding.Default.GetString(Convert.FromBase64String(header.Value));

							values = authorization.Split(new[] { ' ', ':' });

							if (values.Length >= 2)
							{
								request.Credentials = new System.Net.CredentialCache();
								((System.Net.CredentialCache)request.Credentials).Add(request.RequestUri, Keys.Basic.Trim(), new System.Net.NetworkCredential(values[0], values[1]));
								request.PreAuthenticate = true;
							}
							else
							{
								request.Headers[Keys.Authorization] = header.Value;
							}
						}
					}
				}
				else
				{
					request.Headers[header.Key] = header.Value;
				}
			}
		}
	}
}