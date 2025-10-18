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

namespace ISI.Extensions.MessageBus
{
	public class MessageBusMessageHeader
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}

	public delegate void OnHasContent(MessageBusMessageHeaderCollection headers, string content);

	public class MessageBusMessageHeaderCollection : List<MessageBusMessageHeader>
	{
		public class Keys
		{
			public const string OperationKey = nameof(OperationKey);
			public const string Authorization = nameof(Authorization);
			public const string AuthorizationToken = nameof(AuthorizationToken);
			public const string Basic = "Basic ";
			public const string Bearer = "Bearer ";
		}

		public event OnHasContent OnHasContent = null;

		public MessageBusMessageHeaderCollection()
		{

		}
		public MessageBusMessageHeaderCollection(IEnumerable<MessageBusMessageHeader> headers)
		{
			AddRange(headers);
		}

		public void ProcessContent(Func<string> getContent)
		{
			OnHasContent?.Invoke(this, getContent());
		}

		public new void AddRange(IEnumerable<MessageBusMessageHeader> headers)
		{
			foreach (var header in headers)
			{
				Add(header.Key, header.Value);
			}
		}

		public MessageBusMessageHeader Add(string key, string value)
		{
			var header = new MessageBusMessageHeader()
			{
				Key = key,
				Value = value
			};

			this.Add(header);

			return header;
		}

		public MessageBusMessageHeader AddAuthorizationToken(string authenticationToken)
		{
			var header = new MessageBusMessageHeader()
			{
				Key = Keys.AuthorizationToken,
				Value = authenticationToken
			};

			this.Add(header);

			return header;
		}

		public MessageBusMessageHeader AddBasicAuthentication(string userName, string password)
		{
			var authenticationToken = $"{Keys.Basic}{Convert.ToBase64String(Encoding.Default.GetBytes($"{userName}:{password}"))}";

			var header = new MessageBusMessageHeader()
			{
				Key = Keys.Authorization,
				Value = authenticationToken
			};

			this.Add(header);

			return header;
		}

		public MessageBusMessageHeader AddBearerAuthentication(string token)
		{
			var authenticationToken = $"{Keys.Bearer}{token}";

			var header = new MessageBusMessageHeader()
			{
				Key = Keys.Authorization,
				Value = authenticationToken
			};

			this.Add(header);

			return header;
		}

		public string OperationKey
		{
			get => this.FirstOrDefault(header => string.Equals(header.Key, Keys.OperationKey, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
			set
			{
				this.RemoveAll(header => string.Equals(header.Key, Keys.OperationKey, StringComparison.InvariantCultureIgnoreCase));
				Add(Keys.OperationKey, value);
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
	}
}