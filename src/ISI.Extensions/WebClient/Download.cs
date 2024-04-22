#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
	public class Download
	{
		public class DownloadFileResponse
		{
			public string FileName { get; set; }
			public System.Collections.Specialized.NameValueCollection Headers { get; }

			public DownloadFileResponse()
			{
				Headers = new();
			}
		}

		public class DownloadFileResponse<TStream> : DownloadFileResponse, IDisposable
			where TStream : System.IO.Stream, new()
		{
			public System.IO.Stream Stream { get; set; }

			public DownloadFileResponse()
				: base()
			{
				Stream = new TStream();
			}

			public void Dispose()
			{
				Stream?.Dispose();
				Stream = null;
			}
		}

		public static DownloadFileResponse<TStream> DownloadFile<TStream>(string url, HeaderCollection headers, int? bufferSize = null, System.Net.CookieContainer cookieContainer = null)
			where TStream : System.IO.Stream, new()
		{
			return DownloadFile<TStream>(new Uri(url), headers, bufferSize, cookieContainer);
		}

		public static DownloadFileResponse<TStream> DownloadFile<TStream>(Uri uri, HeaderCollection headers, int? bufferSize = null, System.Net.CookieContainer cookieContainer = null)
			where TStream : System.IO.Stream, new()
		{
			var response = new DownloadFileResponse<TStream>();

			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
			webRequest.CookieContainer = cookieContainer;

			headers?.ApplyToWebRequest(webRequest);

			try
			{
				using (var webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
				{
					if (webResponse.Headers.TryGetValue(ISI.Extensions.WebClient.HeaderCollection.Keys.ContentDisposition, out var headerValues))
					{
						response.FileName = new System.Net.Mime.ContentDisposition(headerValues.First()).FileName;
					}

					foreach (var key in webResponse.Headers.AllKeys)
					{
						response.Headers.Add(key, webResponse.Headers[key]);
					}

					using (var webResponseStream = webResponse.GetResponseStream())
					{
						if (webResponseStream != null)
						{
							if (bufferSize.HasValue)
							{
								webResponseStream.CopyTo(response.Stream, bufferSize.Value);
							}
							else
							{
								webResponseStream.CopyTo(response.Stream);
							}
							response.Stream.Rewind();
						}
					}
				}
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
			{

				throw;
			}

			return response;
		}

		public static DownloadFileResponse DownloadFile(string url, HeaderCollection headers, System.IO.Stream toStream, int? bufferSize = null, System.Net.CookieContainer cookieContainer = null)
		{
			return DownloadFile(new Uri(url), headers, toStream, bufferSize, cookieContainer);
		}

		public static DownloadFileResponse DownloadFile(Uri uri, HeaderCollection headers, System.IO.Stream toStream, int? bufferSize = null, System.Net.CookieContainer cookieContainer = null)
		{
			var response = new DownloadFileResponse();

			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
			webRequest.CookieContainer = cookieContainer;

			headers?.ApplyToWebRequest(webRequest);

			try
			{
				using (var webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
				{
					if (webResponse.Headers.TryGetValue(ISI.Extensions.WebClient.HeaderCollection.Keys.ContentDisposition, out var headerValues))
					{
						response.FileName = new System.Net.Mime.ContentDisposition(headerValues.First()).FileName;
					}

					foreach (var key in webResponse.Headers.AllKeys)
					{
						response.Headers.Add(key, webResponse.Headers[key]);
					}

					using (var webResponseStream = webResponse.GetResponseStream())
					{
						if (webResponseStream != null)
						{
							if (bufferSize.HasValue)
							{
								webResponseStream.CopyTo(toStream, bufferSize.Value);
							}
							else
							{
								webResponseStream.CopyTo(toStream);
							}
							toStream.Rewind();
						}
					}
				}
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
			{

				throw;
			}

			return response;
		}

		public delegate System.IO.Stream DownloadFileGetStreamDelegate(string fileName);

		public static DownloadFileResponse DownloadFile(string url, HeaderCollection headers, DownloadFileGetStreamDelegate getStream, int? bufferSize = null, System.Net.CookieContainer cookieContainer = null)
		{
			return DownloadFile(new Uri(url), headers, getStream, bufferSize, cookieContainer);
		}

		public static DownloadFileResponse DownloadFile(Uri uri, HeaderCollection headers, DownloadFileGetStreamDelegate getStream, int? bufferSize = null, System.Net.CookieContainer cookieContainer = null)
		{
			var response = new DownloadFileResponse();

			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
			webRequest.CookieContainer = cookieContainer;

			headers?.ApplyToWebRequest(webRequest);

			try
			{
				using (var webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
				{
					if (webResponse.Headers.TryGetValue(ISI.Extensions.WebClient.HeaderCollection.Keys.ContentDisposition, out var headerValues))
					{
						response.FileName = new System.Net.Mime.ContentDisposition(headerValues.First()).FileName;
					}

					foreach (var key in webResponse.Headers.AllKeys)
					{
						response.Headers.Add(key, webResponse.Headers[key]);
					}

					var toStream = getStream(response.FileName);

					using (var webResponseStream = webResponse.GetResponseStream())
					{
						if (webResponseStream != null)
						{
							if (bufferSize.HasValue)
							{
								webResponseStream.CopyTo(toStream, bufferSize.Value);
							}
							else
							{
								webResponseStream.CopyTo(toStream);
							}

							toStream.Rewind();
						}
					}
				}
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
			{

				throw;
			}

			return response;
		}
	}
}
