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
using System.Threading.Tasks;

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public class RestResponseTypeCollection : Dictionary<System.Net.HttpStatusCode, Type>
		{
			public Type DefaultRestResponseType { get; set; } = typeof(IgnoredResponse);
			public Type DefaultErrorRestResponseType { get; set; } = typeof(UnhandledExceptionResponse);

			public RestResponseTypeCollection()
			{

			}
			public RestResponseTypeCollection(IEnumerable<KeyValuePair<System.Net.HttpStatusCode, Type>> restResponseTypes)
			{
				foreach (var restResponseType in restResponseTypes)
				{
					Add(restResponseType.Key, restResponseType.Value);
				}
			}

			public void Add(int statusCode, Type restResponseType)
			{
				Add((System.Net.HttpStatusCode)statusCode, restResponseType);
			}

			public void Add<TResponse>(int statusCode)
			{
				Add((System.Net.HttpStatusCode)statusCode, typeof(TResponse));
			}

			public void Add<TResponse>(System.Net.HttpStatusCode statusCode)
			{
				Add(statusCode, typeof(TResponse));
			}

			public IRestResponseWrapper GetRestResponse(Type restResponseWrapperType, System.Net.WebResponse webResponse, WebRequestDetails webRequestDetails, bool isErrorResponse, System.Exception exception = null)
			{
				var statusCode = (webResponse as System.Net.HttpWebResponse)?.StatusCode;

				Type restResponseType = null;
				if (statusCode.HasValue)
				{
					if (!TryGetValue(statusCode.Value, out restResponseType))
					{
						restResponseType = null;
					}
				}

				if (restResponseType == null)
				{
					restResponseType = (exception == null ? DefaultRestResponseType : DefaultErrorRestResponseType);
				}

				var restResponseWrapper = Activator.CreateInstance(restResponseWrapperType.MakeGenericType(restResponseType)) as IRestResponseWrapper;

				if (restResponseWrapper is IRestResponseWrapperAddResponse restResponseWrapperAddResponse)
				{
					restResponseWrapperAddResponse.StatusCode = statusCode.GetValueOrDefault();
					restResponseWrapperAddResponse.AddResponse(webResponse, webRequestDetails, isErrorResponse, exception);
				}

				return restResponseWrapper;
			}
		}
	}
}
