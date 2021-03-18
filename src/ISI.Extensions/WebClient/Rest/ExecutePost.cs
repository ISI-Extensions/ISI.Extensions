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
		private const string PostHttpMethod = System.Net.WebRequestMethods.Http.Post;

		public static System.Net.HttpStatusCode? ExecutePost(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(PostHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecutePost(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(PostHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecutePost<TRequest>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecutePost<TRequest>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecutePost<TRequest, TResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteMethod<TRequest, TResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecutePost<TRequest, TResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteMethod<TRequest, TResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecutePost<TRequest, TResponse, TErrorResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<TRequest, TResponse, TErrorResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecutePost<TRequest, TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<TRequest, TResponse, TErrorResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecutePost<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(restResponseTypes, PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecutePost<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(restResponseTypes, PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecutePost(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(restResponseTypes, PostHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecutePost(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(restResponseTypes, PostHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecuteJsonPost<TRequest>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteJsonPost<TRequest>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteJsonPost<TRequest, TResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteJsonPost<TRequest, TResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonPost<TRequest, TResponse, TErrorResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonPost<TRequest, TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteJsonPost<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(restResponseTypes, PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteJsonPost<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(restResponseTypes, PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}





		public static IRestResponseWrapper ExecuteJsonPost(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<NoRequest>(restResponseTypes, PostHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteJsonPost(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<NoRequest>(restResponseTypes, PostHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}





		public static System.Net.HttpStatusCode? ExecuteXmlPost<TRequest>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteXmlPost<TRequest>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteXmlPost<TRequest, TResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteXmlPost<TRequest, TResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteXmlPost<TRequest, TResponse, TErrorResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse, TErrorResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteXmlPost<TRequest, TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse, TErrorResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteXmlPost<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(restResponseTypes, PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteXmlPost<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(restResponseTypes, PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}







		public static System.Net.HttpStatusCode? ExecuteFormRequestPost(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestPost(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteFormRequestPost<TResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestPost<TResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestPost<TResponse, TErrorResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse, TErrorResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestPost<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse, TErrorResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteFormRequestPost(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(restResponseTypes, PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestPost(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(restResponseTypes, PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecuteFormRequestJsonPost(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestJsonPost(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteFormRequestJsonPost<TResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestJsonPost<TResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestJsonPost<TResponse, TErrorResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse, TErrorResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestJsonPost<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse, TErrorResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteFormRequestJsonPost(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(restResponseTypes, PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestJsonPost(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(restResponseTypes, PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}





		public static System.Net.HttpStatusCode? ExecuteFormRequestXmlPost(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestXmlPost(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteFormRequestXmlPost<TResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestXmlPost<TResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestXmlPost<FormDataCollection, TResponse, TErrorResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where FormDataCollection : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse, TErrorResponse>(PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestXmlPost<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse, TErrorResponse>(PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteFormRequestXmlPost(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(restResponseTypes, PostHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestXmlPost(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(restResponseTypes, PostHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static string ExecuteTextPost(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return (ExecuteTextMethod<NoRequest>(PostHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}

		public static string ExecuteTextPost(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return (ExecuteTextMethod<NoRequest>(PostHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}






		public static string ExecuteTextPost<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return (ExecuteTextMethod<TRequest>(restResponseTypes, PostHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}

		public static string ExecuteTextPost<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return (ExecuteTextMethod<TRequest>(restResponseTypes, PostHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}
	}
}
