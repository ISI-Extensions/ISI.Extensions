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

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public static IRestResponseWrapper ExecuteTextMethod(string httpMethod, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteTextMethod(httpMethod, new Uri(url), headers, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteTextMethod(string httpMethod, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			var response = ExecuteTextMethod<TextRequest>(new()
			{
				DefaultRestResponseType = typeof(TextResponse)
			}, httpMethod, uri, headers, new(), throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response;
		}






		public static IRestResponseWrapper ExecuteTextMethod(string httpMethod, string url, HeaderCollection headers, string request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteTextMethod(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteTextMethod(string httpMethod, Uri uri, HeaderCollection headers, string request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			var response = ExecuteTextMethod<TextRequest>(new()
			{
				DefaultRestResponseType = typeof(TextResponse)
			}, httpMethod, uri, headers, new()
			{
				Content = request
			}, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response;
		}






		public static IRestResponseWrapper ExecuteTextMethod<TRequest>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteTextMethod<TRequest>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteTextMethod<TRequest>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			var response = ExecuteTextMethod<TRequest>(new()
			{
				DefaultRestResponseType = typeof(TextResponse)
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response;
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteTextMethod<TRequest, TResponse, TErrorResponse>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteTextMethod<TRequest, TResponse, TErrorResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteTextMethod<TRequest, TResponse, TErrorResponse>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			var response = ExecuteTextMethod<TRequest>(new()
			{
				DefaultRestResponseType = typeof(TResponse),
				DefaultErrorRestResponseType = typeof(TErrorResponse),
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return new(response.StatusCode, (response.IsErrorResponse ? null : response.Response as TResponse), (response.IsErrorResponse ? response.Response as TErrorResponse : null));
		}






		public static IRestResponseWrapper ExecuteTextMethod<TRequest>(RestResponseTypeCollection restResponseTypes, string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteTextMethod<TRequest>(restResponseTypes, httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteTextMethod<TRequest>(RestResponseTypeCollection restResponseTypes, string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(GetContentTypeHeaderValue<TRequest>(ContentTypeTextHeaderValue), AcceptTextHeaderValue, typeof(TextRestResponseWrapper<>), restResponseTypes, httpMethod, uri, headers, request, GetExecuteMethodBodyBuilder<TRequest>() ?? GetExecuteJsonMethodBodyBuilder<TRequest>(), throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}
	}
}
