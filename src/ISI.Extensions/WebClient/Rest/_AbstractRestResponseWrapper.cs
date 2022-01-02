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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public abstract class AbstractRestResponseWrapper<TResponse> : IRestResponseWrapper, IRestResponseWrapperAddResponse
			where TResponse : class
		{
			public System.Net.HttpStatusCode StatusCode { get; set; }
			public TResponse Response { get; set; }
			public bool IsErrorResponse { get; set; }
			public System.Exception Exception { get; set; }

			protected Func<System.IO.Stream, WebRequestDetails, TResponse> ExtractResponse { get; }

			protected static readonly Func<System.IO.Stream, WebRequestDetails, TResponse> JsonExtractResponse = (stream, webRequestDetails) =>
			{
				if (webRequestDetails != null)
				{
					webRequestDetails.SetResponseRaw(stream.TextReadToEnd());

					return Serialization.Deserialize<TResponse>(webRequestDetails.ResponseRaw, ISI.Extensions.Serialization.SerializationFormat.Json);
				}

				return Serialization.Deserialize<TResponse>(stream, ISI.Extensions.Serialization.SerializationFormat.Json);
			};

			protected static readonly Func<System.IO.Stream, WebRequestDetails, TResponse> XmlExtractResponse = (stream, webRequestDetails) =>
			{
				if (webRequestDetails != null)
				{
					webRequestDetails.SetResponseRaw(stream.TextReadToEnd());

					return Serialization.Deserialize<TResponse>(webRequestDetails.ResponseRaw, ISI.Extensions.Serialization.SerializationFormat.Xml);
				}

				return Serialization.Deserialize<TResponse>(stream, ISI.Extensions.Serialization.SerializationFormat.Xml);
			};

			public AbstractRestResponseWrapper(Func<System.IO.Stream, WebRequestDetails, TResponse> extractResponse)
			{
				if (extractResponse == null)
				{
					var serializer = Serialization.GetSerializer(typeof(TResponse));
					switch (serializer.SerializationFormat)
					{
						case ISI.Extensions.Serialization.SerializationFormat.Json:
							extractResponse = JsonExtractResponse;
							break;
						case ISI.Extensions.Serialization.SerializationFormat.Xml:
							extractResponse = XmlExtractResponse;
							break;
					}
				}

				ExtractResponse = extractResponse;
			}

			void IRestResponseWrapperAddResponse.AddResponse(System.Net.WebResponse webResponse, WebRequestDetails webRequestDetails, bool isErrorResponse, System.Exception exception)
			{
				if (webResponse != null)
				{
					using (var responseStream = webResponse.GetResponseStream())
					{
						if (typeof(TResponse) == typeof(IgnoredResponse))
						{
							Response = new IgnoredResponse() as TResponse;
						}
						else if (typeof(TResponse).Implements<IRestStreamResponse>())
						{
							Response = Activator.CreateInstance<TResponse>();

							((IRestStreamResponse)Response).StatusCode = StatusCode;
							responseStream.CopyTo(((IRestStreamResponse)Response).Stream);
						}
						else if (typeof(TResponse).Implements<IRestContentResponse>())
						{
							Response = Activator.CreateInstance<TResponse>();

							((IRestContentResponse)Response).StatusCode = StatusCode;
							((IRestContentResponse)Response).Content = responseStream.TextReadToEnd();
							webRequestDetails?.SetResponseRaw(((IRestContentResponse)Response).Content);
						}
						else
						{
							Response = ExtractResponse(responseStream, webRequestDetails);
						}
					}
				}

				IsErrorResponse = isErrorResponse;

				if (Response is IRestExceptionResponse restExceptionResponse)
				{
					restExceptionResponse.Exception = exception;
				}

				Exception = exception;
			}

			object IRestResponseWrapper.Response => Response;

			TRestResponseWrapperResponse IRestResponseWrapper.GetResponse<TRestResponseWrapperResponse>() => Response as TRestResponseWrapperResponse;
		}
	}
}
