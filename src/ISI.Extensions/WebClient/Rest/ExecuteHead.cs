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
		private const string HeadHttpMethod = System.Net.WebRequestMethods.Http.Head;

		public static System.Net.HttpStatusCode? ExecuteHead(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteHead(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteHead<TResponse>(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteMethod<NoRequest, TResponse>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteHead<TResponse>(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteMethod<NoRequest, TResponse>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteHead<TResponse, TErrorResponse>(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<NoRequest, TResponse, TErrorResponse>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteHead<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<NoRequest, TResponse, TErrorResponse>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteHead(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(restResponseTypes, HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteHead(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(restResponseTypes, HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecuteJsonHead(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<NoRequest>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteJsonHead(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<NoRequest>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteJsonHead<TResponse>(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteJsonMethod<NoRequest, TResponse>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteJsonHead<TResponse>(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteJsonMethod<NoRequest, TResponse>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonHead<TResponse, TErrorResponse>(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<NoRequest, TResponse, TErrorResponse>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonHead<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<NoRequest, TResponse, TErrorResponse>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteJsonHead(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<NoRequest>(restResponseTypes, HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteJsonHead(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<NoRequest>(restResponseTypes, HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}





		public static System.Net.HttpStatusCode? ExecuteXmlHead(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<NoRequest>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteXmlHead(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<NoRequest>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteXmlHead<TResponse>(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteXmlMethod<NoRequest, TResponse>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteXmlHead<TResponse>(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteXmlMethod<NoRequest, TResponse>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteXmlHead<TResponse, TErrorResponse>(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<NoRequest, TResponse, TErrorResponse>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteXmlHead<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<NoRequest, TResponse, TErrorResponse>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteXmlHead<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(restResponseTypes, HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteXmlHead<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(restResponseTypes, HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static string ExecuteTextHead(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return (ExecuteTextMethod<NoRequest>(HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}

		public static string ExecuteTextHead(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return (ExecuteTextMethod<NoRequest>(HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}






		public static string ExecuteTextHead<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return (ExecuteTextMethod<TRequest>(restResponseTypes, HeadHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}

		public static string ExecuteTextHead<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return (ExecuteTextMethod<TRequest>(restResponseTypes, HeadHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}
	}
}
