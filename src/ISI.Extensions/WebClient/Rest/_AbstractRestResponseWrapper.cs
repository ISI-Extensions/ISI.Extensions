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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public abstract class AbstractRestResponseWrapper<TResponse> : IRestResponseWrapper, IRestResponseWrapperAddResponse
			where TResponse : class
		{
			public delegate TResponse ExtractResponseDelegate(System.IO.Stream stream, WebRequestDetails webRequestDetails);

			public System.Net.HttpStatusCode StatusCode { get; set; }
			public TResponse Response { get; set; }
			public bool IsErrorResponse { get; set; }
			public System.Exception Exception { get; set; }

			protected ExtractResponseDelegate ExtractResponse { get; }

			protected static readonly ExtractResponseDelegate JsonExtractResponse = (stream, webRequestDetails) =>
			{
				if (webRequestDetails != null)
				{
					webRequestDetails.SetResponseRaw(stream.TextReadToEnd());

					return Serialization.Deserialize<TResponse>(webRequestDetails.ResponseRaw, ISI.Extensions.Serialization.SerializationFormat.Json);
				}

				return Serialization.Deserialize<TResponse>(stream, ISI.Extensions.Serialization.SerializationFormat.Json);
			};

			protected static readonly ExtractResponseDelegate XmlExtractResponse = (stream, webRequestDetails) =>
			{
				if (webRequestDetails != null)
				{
					webRequestDetails.SetResponseRaw(stream.TextReadToEnd());

					return Serialization.Deserialize<TResponse>(webRequestDetails.ResponseRaw, ISI.Extensions.Serialization.SerializationFormat.Xml);
				}

				return Serialization.Deserialize<TResponse>(stream, ISI.Extensions.Serialization.SerializationFormat.Xml);
			};

			public AbstractRestResponseWrapper(ExtractResponseDelegate extractResponse)
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
						var decompressedStream = (System.IO.Stream)null;

						if (typeof(TResponse) == typeof(IgnoredResponse))
						{
							webRequestDetails?.SetStatusCode(StatusCode);
							webRequestDetails?.SetResponseHeaders(webResponse?.Headers);

							Response = new IgnoredResponse() as TResponse;
						}
						else
						{
							if (webResponse.Headers.TryGetValue("Content-Encoding", out var encodingHeaders))
							{
								foreach (var encodingHeader in encodingHeaders ?? [])
								{
									if (string.Equals(encodingHeader, "gzip", StringComparison.InvariantCultureIgnoreCase))
									{
										using (var zipStream = new System.IO.Compression.GZipStream(decompressedStream ?? responseStream, System.IO.Compression.CompressionMode.Decompress))
										{
											var stream = new System.IO.MemoryStream();
											zipStream.CopyTo(stream);

											decompressedStream?.Dispose();
											decompressedStream = stream;
											decompressedStream.Rewind();
											stream = null;
										}
									}
									else if (string.Equals(encodingHeader, "compress", StringComparison.InvariantCultureIgnoreCase))
									{
										throw new Exception("Cannot decompress compress streams");
										//using (var zipStream = new System.IO.Compression.GZipStream(decompressedStream ?? responseStream, System.IO.Compression.CompressionMode.Decompress))
										//{
										//	var stream = new System.IO.MemoryStream();
										//	zipStream.CopyTo(stream);

										//	decompressedStream?.Dispose();
										//	decompressedStream = stream;
										//	decompressedStream.Rewind();
										//	stream = null;
										//}
									}
									else if (string.Equals(encodingHeader, "deflate", StringComparison.InvariantCultureIgnoreCase))
									{
										using (var deflateStream = new System.IO.Compression.DeflateStream(decompressedStream ?? responseStream, System.IO.Compression.CompressionMode.Decompress))
										{
											var stream = new System.IO.MemoryStream();
											deflateStream.CopyTo(stream);

											decompressedStream?.Dispose();
											decompressedStream = stream;
											decompressedStream.Rewind();
											stream = null;
										}
									}
									else
									{
										throw new Exception($"Cannot decompress {encodingHeader} streams");
									}
								}
							}

							webRequestDetails?.SetStatusCode(StatusCode);
							webRequestDetails?.SetResponseHeaders(webResponse?.Headers);

							if (typeof(TResponse).Implements<IRestStreamResponse>())
							{
								Response = Activator.CreateInstance<TResponse>();

								((IRestStreamResponse)Response).StatusCode = StatusCode;
								(decompressedStream ?? responseStream).CopyTo(((IRestStreamResponse)Response).Stream);
								((IRestStreamResponse)Response).ResponseHeaders = new(webResponse?.Headers);
							}
							else if (typeof(TResponse).Implements<IRestContentResponse>())
							{
								Response = Activator.CreateInstance<TResponse>();

								var content = (decompressedStream ?? responseStream).TextReadToEnd();
								webRequestDetails?.SetResponseRaw(content);

								((IRestContentResponse)Response).StatusCode = StatusCode;
								((IRestContentResponse)Response).Content = content;
								((IRestContentResponse)Response).ResponseHeaders = new(webResponse?.Headers);

								if (exception != null)
								{
									exception = new RestException(StatusCode, webRequestDetails, exception);
								}
							}
							else
							{
								Response = ExtractResponse(responseStream, webRequestDetails);
							}
						}

						decompressedStream?.Dispose();
					}
				}
				else
				{
					webRequestDetails?.SetStatusCode(StatusCode);
					webRequestDetails?.SetResponseHeaders(webResponse?.Headers);
					webRequestDetails?.SetException(exception);
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
