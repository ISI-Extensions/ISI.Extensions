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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public static IProxyWrapper ProxyWrapper { get; set; } = null;

		private static ISI.Extensions.Serialization.ISerialization _serialization = null;
		protected static ISI.Extensions.Serialization.ISerialization Serialization => _serialization ??= GetSerialization();

		private static ISI.Extensions.Serialization.ISerialization GetSerialization()
		{
			var serialization = ISI.Extensions.ServiceLocator.Current?.GetService<ISI.Extensions.Serialization.ISerialization>();

			if (serialization == null)
			{
				throw new("ISI.Extensions.Serialization.ISerialization is null");
			}

			return serialization;
		}

		private static Configuration _restConfiguration = null;
		public static Configuration RestConfiguration => _restConfiguration ??= ISI.Extensions.ServiceLocator.Current?.GetService<Configuration>() ?? new();

		public static void AddIgnoreServerCertificateValidationForSubjectsContaining(string subject)
		{
			_restConfiguration ??= new();

			if (!string.IsNullOrWhiteSpace(subject))
			{
				var ignoreServerCertificateValidationForSubjectsContaining = new HashSet<string>(RestConfiguration.IgnoreServerCertificateValidationForSubjectsContaining ?? [], StringComparer.InvariantCultureIgnoreCase);

				ignoreServerCertificateValidationForSubjectsContaining.Add(subject);

				RestConfiguration.IgnoreServerCertificateValidationForSubjectsContaining = ignoreServerCertificateValidationForSubjectsContaining.ToArray();
			}
		}

		public const string MethodHeaderKey = "X-HTTP-METHOD";

		public const string AcceptAllHeaderValue = "*/*";
		public const string AcceptTextHeaderValue = ISI.Extensions.MimeTypes.PlainText;
		public const string AcceptXmlHeaderValue = ISI.Extensions.MimeTypes.Xml;
		public const string AcceptTextXmlHeaderValue = ISI.Extensions.MimeTypes.TextXml;
		public const string AcceptJsonHeaderValue = ISI.Extensions.MimeTypes.Json;

		public const string ContentTypeApplicationFormUrlEncodedHeaderValue = "application/x-www-form-urlencoded";
		public const string ContentTypeTextHeaderValue = ISI.Extensions.MimeTypes.PlainText;
		public const string ContentTypeXmlHeaderValue = ISI.Extensions.MimeTypes.Xml;
		public const string ContentTypeJsonHeaderValue = ISI.Extensions.MimeTypes.Json;
		public const string ContentTypeJavascriptHeaderValue = "text/x-javascript";
	}
}