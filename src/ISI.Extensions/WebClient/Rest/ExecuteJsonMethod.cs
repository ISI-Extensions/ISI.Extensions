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
		public static System.Net.HttpStatusCode? ExecuteFormRequestJsonMethod(string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteFormRequestJsonMethod(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestJsonMethod(string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			var response = ExecuteFormRequestJsonMethod<IgnoredResponse, UnhandledExceptionResponse>(httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.StatusCode;
		}






		public static TResponse ExecuteFormRequestJsonMethod<TResponse>(string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteFormRequestJsonMethod<TResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestJsonMethod<TResponse>(string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			var responseType = typeof(TResponse);
			
			if (responseType.IsGenericType && (responseType.GetGenericTypeDefinition() == typeof(SerializedResponse<>)))
			{
				responseType = typeof(JsonSerializedResponse<>).MakeGenericType(responseType.GenericTypeArguments);
			}

			var response = ExecuteFormRequestJsonMethod(new RestResponseTypeCollection()
			{
				DefaultRestResponseType = responseType,
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.Response as TResponse;
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestJsonMethod<TResponse, TErrorResponse>(string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteFormRequestJsonMethod<TResponse, TErrorResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestJsonMethod<TResponse, TErrorResponse>(string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			var responseType = typeof(TResponse);
			
			if (responseType.IsGenericType && (responseType.GetGenericTypeDefinition() == typeof(SerializedResponse<>)))
			{
				responseType = typeof(JsonSerializedResponse<>).MakeGenericType(responseType.GenericTypeArguments);
			}

			var errorResponseType = typeof(TErrorResponse);
			
			if (errorResponseType.IsGenericType && (errorResponseType.GetGenericTypeDefinition() == typeof(SerializedResponse<>)))
			{
				errorResponseType = typeof(JsonSerializedResponse<>).MakeGenericType(errorResponseType.GenericTypeArguments);
			}

			var response = ExecuteFormRequestJsonMethod(new RestResponseTypeCollection()
			{
				DefaultRestResponseType = responseType,
				DefaultErrorRestResponseType = errorResponseType,
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return new RestResponse<TResponse, TErrorResponse>(response.StatusCode, (response.IsErrorResponse ? null : response.Response as TResponse), (response.IsErrorResponse ? response.Response as TErrorResponse : null));
		}






		public static IRestResponseWrapper ExecuteFormRequestJsonMethod(RestResponseTypeCollection restResponseTypes, string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteFormRequestJsonMethod(restResponseTypes, httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestJsonMethod(RestResponseTypeCollection restResponseTypes, string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(restResponseTypes, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecuteJsonMethod<TRequest>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteJsonMethod<TRequest>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			var response = ExecuteJsonMethod<TRequest, IgnoredResponse, UnhandledExceptionResponse>(httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.StatusCode;
		}






		public static TResponse ExecuteJsonMethod<TRequest, TResponse>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteJsonMethod<TRequest, TResponse>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			var responseType = typeof(TResponse);
			
			if (responseType.IsGenericType && (responseType.GetGenericTypeDefinition() == typeof(SerializedResponse<>)))
			{
				responseType = typeof(JsonSerializedResponse<>).MakeGenericType(responseType.GenericTypeArguments);
			}

			var response = ExecuteJsonMethod<TRequest>(new RestResponseTypeCollection()
			{
				DefaultRestResponseType = responseType,
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.Response as TResponse;
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			var responseType = typeof(TResponse);
			
			if (responseType.IsGenericType && (responseType.GetGenericTypeDefinition() == typeof(SerializedResponse<>)))
			{
				responseType = typeof(JsonSerializedResponse<>).MakeGenericType(responseType.GenericTypeArguments);
			}

			var errorResponseType = typeof(TErrorResponse);
			
			if (errorResponseType.IsGenericType && (errorResponseType.GetGenericTypeDefinition() == typeof(SerializedResponse<>)))
			{
				errorResponseType = typeof(JsonSerializedResponse<>).MakeGenericType(errorResponseType.GenericTypeArguments);
			}

			var response = ExecuteJsonMethod<TRequest>(new RestResponseTypeCollection()
			{
				DefaultRestResponseType = responseType,
				DefaultErrorRestResponseType = errorResponseType,
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return new RestResponse<TResponse, TErrorResponse>(response.StatusCode, (response.IsErrorResponse ? null : response.Response as TResponse), (response.IsErrorResponse ? response.Response as TErrorResponse : null));
		}






		public static IRestResponseWrapper ExecuteJsonMethod<TRequest>(RestResponseTypeCollection restResponseTypes, string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(restResponseTypes, httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteJsonMethod<TRequest>(RestResponseTypeCollection restResponseTypes, string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(GetContentTypeHeaderValue<TRequest>(ContentTypeJsonHeaderValue), AcceptJsonHeaderValue, typeof(JsonRestResponseWrapper<>), restResponseTypes, httpMethod, uri, headers, request, GetExecuteMethodBodyBuilder<TRequest>(GetExecuteJsonMethodBodyBuilder<TRequest>()), throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}
	}
}
