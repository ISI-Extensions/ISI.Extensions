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

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public delegate void ExecuteMethodBuildBody<TRequest>(TRequest request, System.Net.HttpWebRequest webRequest, WebRequestDetails webRequestDetails)
			where TRequest : class;

		private static ExecuteMethodBuildBody<TRequest> GetExecuteMethodBodyBuilder<TRequest>(ExecuteMethodBuildBody<TRequest> defaultBodyBuilder = null)
			where TRequest : class
		{
			if (typeof(TRequest) == typeof(FormDataCollection))
			{
				return (request, webRequest, webRequestDetails) =>
				{
					if (request is FormDataCollection formData)
					{
						formData.WriteEncodedRequest(webRequest, webRequestDetails);
					}
				};
			}

			if (typeof(TRequest) == typeof(TextRequest))
			{
				return (request, webRequest, webRequestDetails) =>
				{
					if (request is TextRequest textRequest)
					{
						var encodedRequest = textRequest.GetEncodedRequest();

						webRequest?.GetRequestStream()?.Write(encodedRequest, 0, encodedRequest.Length);

						webRequestDetails?.SetBodyRaw(textRequest.Content);
					}
				};
			}

			if (defaultBodyBuilder != null)
			{
				return defaultBodyBuilder;
			}

			var serializer = Serialization.GetSerializer(typeof(TRequest));
			switch (serializer.SerializationFormat)
			{
				case ISI.Extensions.Serialization.SerializationFormat.Json:
					return GetExecuteJsonMethodBodyBuilder<TRequest>();
				case ISI.Extensions.Serialization.SerializationFormat.Xml:
					return GetExecuteXmlMethodBodyBuilder<TRequest>();
			}

			return null;
		}

		private static ExecuteMethodBuildBody<TRequest> GetExecuteJsonMethodBodyBuilder<TRequest>()
			where TRequest : class
		{
			return (request, webRequest, webRequestDetails) =>
			{
				if (request != null)
				{
					var content = (string.IsNullOrWhiteSpace(webRequestDetails?.BodyRaw) ? Serialization.Serialize(request, ISI.Extensions.Serialization.SerializationFormat.Json).SerializedValue : webRequestDetails.BodyRaw);

					var encodedRequest = (new ASCIIEncoding()).GetBytes(content);

					webRequest?.GetRequestStream()?.Write(encodedRequest, 0, encodedRequest.Length);

					webRequestDetails?.SetBodyRaw(content);
				}
			};
		}

		private static ExecuteMethodBuildBody<TRequest> GetExecuteXmlMethodBodyBuilder<TRequest>()
			where TRequest : class
		{
			return (request, webRequest, webRequestDetails) =>
			{
				if (request != null)
				{
					var content = (string.IsNullOrWhiteSpace(webRequestDetails?.BodyRaw) ? Serialization.Serialize(request, ISI.Extensions.Serialization.SerializationFormat.Xml).SerializedValue : webRequestDetails.BodyRaw);

					var encodedRequest = (new ASCIIEncoding()).GetBytes(content);

					webRequest?.GetRequestStream()?.Write(encodedRequest, 0, encodedRequest.Length);

					webRequestDetails?.SetBodyRaw(content);
				}
			};
		}
	}
}