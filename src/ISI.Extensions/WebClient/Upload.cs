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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.WebClient
{
	public class Upload
	{
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


		public static void UploadFileByChunk(string url, HeaderCollection headers, System.IO.Stream stream, string fileName, int maxFileSegmentSize = 1000000, int maxTries = 3)
		{
			UploadFileByChunk(new Uri(url), headers, stream, fileName, maxFileSegmentSize, maxTries);
		}

		public static void UploadFileByChunk(Uri uri, HeaderCollection headers, System.IO.Stream stream, string fileName, int maxFileSegmentSize = 1000000, int maxTries = 3)
		{
			var fileSegments = new Queue<byte[]>();

			using (var memoryStream = new System.IO.MemoryStream())
			{
				stream.Rewind();

				const int chunkSize = 1427;
				var buffer = new byte[chunkSize];
				var readBlocks = 0;

				readBlocks = stream.Read(buffer, 0, chunkSize);

				while (readBlocks > 0)
				{
					memoryStream.Write(buffer, 0, readBlocks);
					readBlocks = (readBlocks >= chunkSize ? stream.Read(buffer, 0, chunkSize) : 0);

					memoryStream.Flush();

					if ((memoryStream.Position > 0) && ((readBlocks == 0) || (memoryStream.Position + chunkSize > maxFileSegmentSize)))
					{
						memoryStream.Position = 0;
						fileSegments.Enqueue(memoryStream.ToArray());
						memoryStream.SetLength(0);
					}
				}
			}

			while (fileSegments.Any())
			{
				var data = fileSegments.Dequeue();

				var tryAttemptsLeft = maxTries;
				while (tryAttemptsLeft > 0)
				{
					try
					{
						var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);

						headers?.ApplyToWebRequest(webRequest);

						webRequest.Method = System.Net.WebRequestMethods.Http.Post;
						webRequest.ContentType = "application/x-www-form-urlencoded";

						var content = new StringBuilder();

						content.AppendFormat("{0}={1}&", "finalSegment", System.Web.HttpUtility.UrlEncode((fileSegments.Any() ? "false" : "true")));

						content.AppendFormat("{0}={1}&", "file", System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(data)));

						var encodedRequest = (new ASCIIEncoding()).GetBytes(content.ToString());

						using (var requestStream = webRequest.GetRequestStream())
						{
							requestStream.Write(encodedRequest, 0, encodedRequest.Length);

							using (var response = webRequest.GetResponse())
							{
								if ((response is System.Net.HttpWebResponse webResponse) && (webResponse.StatusCode != System.Net.HttpStatusCode.OK))
								{
									var encoding = Encoding.GetEncoding(webResponse.CharacterSet);

									using (var responseStream = new System.IO.StreamReader(webResponse.GetResponseStream(), encoding))
									{
										throw new(string.Format("{0}: {1}\n{2}", webResponse.StatusCode, webResponse.StatusDescription, responseStream.ReadToEnd()));
									}
								}
							}

							requestStream.Close();
						}

						tryAttemptsLeft = 0;
					}
#pragma warning disable CS0168 // Variable is declared but never used
					catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
					{
						tryAttemptsLeft--;
						if (tryAttemptsLeft < 0)
						{
							throw;
						}

						System.Threading.Thread.Sleep(5000);
					}
				}
			}
		}

		public static (System.Net.HttpStatusCode HttpStatusCode, string Body) UploadFile(string url, HeaderCollection headers, System.IO.Stream stream, string fileName, string fileFieldName = "uploadFile", System.Collections.Specialized.NameValueCollection formValues = null)
		{
			return UploadFile(new Uri(url), headers, stream, fileName, fileFieldName, formValues);
		}

		public static (System.Net.HttpStatusCode HttpStatusCode, string Body) UploadFile(Uri uri, HeaderCollection headers, System.IO.Stream stream, string fileName, string fileFieldName = "uploadFile", System.Collections.Specialized.NameValueCollection formValues = null)
		{
			return UploadFiles(uri, headers, new[] { (Stream: stream, FileName: fileName, FileFieldName: fileFieldName) }, formValues);
		}

		public static (System.Net.HttpStatusCode HttpStatusCode, string Body) UploadFiles(string url, HeaderCollection headers, IEnumerable<(System.IO.Stream Stream, string FileName, string FileFieldName)> files, System.Collections.Specialized.NameValueCollection formValues = null)
		{
			return UploadFiles(new Uri(url), headers, files, formValues);
		}

		public static (System.Net.HttpStatusCode HttpStatusCode, string Body) UploadFiles(Uri uri, HeaderCollection headers, IEnumerable<(System.IO.Stream Stream, string FileName, string FileFieldName)> files, System.Collections.Specialized.NameValueCollection formValues = null)
		{
			var boundary = string.Format("---------------------------{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36));
			var boundarybytes = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}--{1}{0}", Environment.NewLine, boundary));

			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);

			headers?.ApplyToWebRequest(webRequest);

			if (webRequest.Headers["Content-Type"] != null)
			{
				webRequest.Headers.Remove("Content-Type");
			}

			webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

			webRequest.Method = System.Net.WebRequestMethods.Http.Post;

			var formItems = new List<byte[]>();
			long requestLen = 0;

			if (formValues != null)
			{
				foreach (var key in formValues.AllKeys)
				{
					var itemString = string.Format("Content-Disposition: form-data; name=\"{1}\"{0}{0}{2}", Environment.NewLine, key, formValues[key]);
					var itemBytes = System.Text.Encoding.UTF8.GetBytes(itemString);

					requestLen += itemBytes.Length;
					requestLen += boundarybytes.Length;

					formItems.Add(itemBytes);
				}
			}

			string getFileHeader((System.IO.Stream Stream, string FileName, string FileFieldName) file)
			{
				return string.Format("Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"{0}Content-Type: {3}{0}{0}", Environment.NewLine, file.FileFieldName, file.FileName, ISI.Extensions.MimeType.GetMimeType(file.FileName));
			}

			foreach (var file in files)
			{
				file.Stream.Rewind();

				var fileHeader = getFileHeader(file);
				var fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(fileHeader);

				requestLen += boundarybytes.Length + fileHeaderBytes.Length + file.Stream.Length;
			}


			var fileTrailer = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}--{1}--{0}", Environment.NewLine, boundary));
			requestLen += fileTrailer.Length;

			webRequest.ContentLength = requestLen;

			//webRequest.Expect = string.Empty;
			//webRequest.Timeout = 1000 * 5;
			//webRequest.ReadWriteTimeout = 1000 * 5;

			using (var requestStream = webRequest.GetRequestStream())
			{
				foreach (var item in formItems)
				{
					requestStream.Write(boundarybytes, 0, boundarybytes.Length);
					requestStream.Write(item, 0, item.Length);
				}

				foreach (var file in files)
				{
					requestStream.Write(boundarybytes, 0, boundarybytes.Length);

					var fileHeader = getFileHeader(file);
					var fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(fileHeader);

					requestStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

					var chunkSize = 1427; // any larger will cause an SSL request to fail
					file.Stream.CopyTo(requestStream, chunkSize: chunkSize);
				}

				requestStream.Write(fileTrailer, 0, fileTrailer.Length);

				requestStream.Flush();
			}

			using (var response = webRequest.GetResponse())
			{
				if (response is System.Net.HttpWebResponse httpWebResponse)
				{
					var encoding = Encoding.GetEncoding(httpWebResponse?.CharacterSet ?? Encoding.Default.WebName);

					using (var responseStream = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), encoding))
					{
						var body = responseStream.ReadToEnd();

						if (httpWebResponse.StatusCode != System.Net.HttpStatusCode.OK)
						{
							throw new(string.Format("{0}: {1}\n{2}", httpWebResponse.StatusCode, httpWebResponse.StatusDescription, body));
						}

						return (HttpStatusCode: httpWebResponse.StatusCode, Body: body);
					}
				}

				return (HttpStatusCode: System.Net.HttpStatusCode.Ambiguous, Body: string.Empty);
			}
		}

		public static TResponse UploadFile<TResponse>(string url, HeaderCollection headers, System.IO.Stream stream, string fileName, string fileFieldName = "uploadFile", System.Collections.Specialized.NameValueCollection formValues = null)
			where TResponse : class
		{
			return UploadFile<TResponse>(new Uri(url), headers, stream, fileName, fileFieldName, formValues);
		}

		public static TResponse UploadFile<TResponse>(Uri uri, HeaderCollection headers, System.IO.Stream stream, string fileName, string fileFieldName = "uploadFile", System.Collections.Specialized.NameValueCollection formValues = null)
			where TResponse : class
		{
			stream.Rewind();

			var boundary = string.Format("---------------------------{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36));
			var boundarybytes = System.Text.Encoding.UTF8.GetBytes(string.Format("\r\n--{0}\r\n", boundary));

			var formItems = new List<byte[]>();
			long requestLen = 0;

			if (formValues != null)
			{
				foreach (var key in formValues.AllKeys)
				{
					var itemString = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", key, formValues[key]);
					var itemBytes = System.Text.Encoding.UTF8.GetBytes(itemString);

					requestLen += itemBytes.Length;
					requestLen += boundarybytes.Length;

					formItems.Add(itemBytes);
				}
			}


			var fileHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", fileFieldName, fileName, ISI.Extensions.MimeType.GetMimeType(fileName));
			var fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(fileHeader);
			var fileTrailer = System.Text.Encoding.UTF8.GetBytes(string.Format("\r\n--{0}--\r\n", boundary));

			requestLen += boundarybytes.Length + fileHeaderBytes.Length + fileTrailer.Length + stream.Length;

			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);

			headers?.ApplyToWebRequest(webRequest);

			if (webRequest.Headers["Content-Type"] != null)
			{
				webRequest.Headers.Remove("Content-Type");
			}

			webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

			webRequest.Method = System.Net.WebRequestMethods.Http.Post;
			webRequest.ContentLength = requestLen;

			//webRequest.Expect = string.Empty;
			//webRequest.Timeout = 1000 * 5;
			//webRequest.ReadWriteTimeout = 1000 * 5;

			using (var requestStream = webRequest.GetRequestStream())
			{
				foreach (var item in formItems)
				{
					requestStream.Write(boundarybytes, 0, boundarybytes.Length);
					requestStream.Write(item, 0, item.Length);
				}

				requestStream.Write(boundarybytes, 0, boundarybytes.Length);
				requestStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

				var chunkSize = 1427; // any larger will cause an SSL request to fail
				stream.CopyTo(requestStream, chunkSize: chunkSize);

				requestStream.Write(fileTrailer, 0, fileTrailer.Length);

				requestStream.Flush();
			}

			using (var response = webRequest.GetResponse())
			{
				if (response is System.Net.HttpWebResponse httpWebResponse)
				{
					var encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);

					using (var responseStream = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), encoding))
					{
						var body = responseStream.ReadToEnd();

						if (httpWebResponse.StatusCode != System.Net.HttpStatusCode.OK)
						{
							throw new(string.Format("{0}: {1}\n{2}", httpWebResponse.StatusCode, httpWebResponse.StatusDescription, body));
						}

						return Serialization.Deserialize<TResponse>(body, ISI.Extensions.Serialization.SerializationFormat.Json);
					}
				}

				return null;
			}
		}
	}
}