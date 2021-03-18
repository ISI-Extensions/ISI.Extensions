#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		[ThreadStatic]
		internal static EventHandler _EventHandler = null;

		public delegate void OnExecute(Func<string> getCurlCommand);
		public delegate void OnExecuted(Func<string> getResponse);

		public static EventHandler GetEventHandler()
		{
			return _EventHandler ??= new EventHandler(() => { _EventHandler = null; });
		}

		public class WebRequestDetails
		{
			public string HttpMethod { get; private set; } = null;
			public Uri Uri { get; private set; } = null;
			public System.Net.WebHeaderCollection Headers { get; private set; } = null;
			public FormDataCollection FormData { get; private set; } = null;
			public string BodyRaw { get; private set; } = null;

			public string ResponseRaw { get; private set; } = null;

			internal void SetHttpMethod(string httpMethod)
			{
				HttpMethod = httpMethod;
			}
			internal void SetUri(Uri uri)
			{
				Uri = uri;
			}
			internal void SetHeaders(System.Net.WebHeaderCollection headers)
			{
				Headers = headers;
			}
			internal void SetFormData(FormDataCollection formData)
			{
				FormData = formData;
			}
			internal void SetBodyRaw(string bodyRaw)
			{
				BodyRaw = bodyRaw;
			}

			internal void SetResponseRaw(string responseRaw)
			{
				ResponseRaw = responseRaw;
			}

			internal string GetCurlCommand()
			{
				var cmd = new StringBuilder();

				cmd.Append("curl ");

				if (!string.Equals(HttpMethod, System.Net.WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase))
				{
					cmd.Append(string.Format("-X {0} ", HttpMethod.ToUpper()));
				}

				cmd.AppendLine(string.Format("{0} \\", Uri.ToString()));

				if (Headers != null)
				{
					foreach (var headerKey in Headers.AllKeys)
					{
						cmd.AppendLine(string.Format("-H \"{0}: {1}\" \\", headerKey, Headers[headerKey]));
					}
				}

				if (FormData != null)
				{
					foreach (var keyValueFormData in FormData.OfType<FormData>())
					{
						if (keyValueFormData.Values.Length == 0)
						{
							cmd.AppendLine(string.Format("-d {0} \\", keyValueFormData.Key));
						}
						else if (keyValueFormData.Values.Length == 1)
						{
							cmd.AppendLine(string.Format("-d {0}={1} \\", keyValueFormData.Key, keyValueFormData.Value));
						}
						else if (keyValueFormData.Values.Length > 1)
						{
							foreach (var value in keyValueFormData.Values)
							{
								cmd.AppendLine(string.Format("-d {0}[]={1} \\", keyValueFormData.Key, value));
							}
						}
					}
				}

				if (!string.IsNullOrWhiteSpace(BodyRaw))
				{
					cmd.AppendLine(string.Format("-d \"{0}\" \\", BodyRaw.Replace("\"", "\"\"")));
				}

				return cmd.ToString().TrimEnd(' ', '\\', '\r', '\n');
			}

			internal string GetResponse()
			{
				return ResponseRaw;
			}
		}

		public class EventHandler : IDisposable
		{
			public WebRequestDetails WebRequestDetails { get; }

			public event OnExecute OnExecute = null;
			public event OnExecuted OnExecuted = null;

			private readonly Action _closeEventHandler;

			public EventHandler(Action closeEventHandler)
			{
				_closeEventHandler = closeEventHandler;
				WebRequestDetails = new WebRequestDetails();
			}

			internal void Execute()
			{
				OnExecute?.Invoke(WebRequestDetails.GetCurlCommand);
			}

			internal void Executed()
			{
				OnExecuted?.Invoke(WebRequestDetails.GetResponse);
			}

			public void Dispose()
			{
				_closeEventHandler?.Invoke();
			}
		}
	}
}