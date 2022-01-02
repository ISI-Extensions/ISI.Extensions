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

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		private const string PutHttpMethod = System.Net.WebRequestMethods.Http.Put;

		public static System.Net.HttpStatusCode? ExecutePut(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(PutHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecutePut(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<NoRequest>(PutHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecutePut<TRequest>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecutePut<TRequest>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecutePut<TRequest, TResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteMethod<TRequest, TResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecutePut<TRequest, TResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteMethod<TRequest, TResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecutePut<TRequest, TResponse, TErrorResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<TRequest, TResponse, TErrorResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecutePut<TRequest, TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<TRequest, TResponse, TErrorResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecutePut<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(restResponseTypes, PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecutePut<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(restResponseTypes, PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}







		public static System.Net.HttpStatusCode? ExecuteJsonPut<TRequest>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteJsonPut<TRequest>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteJsonPut<TRequest, TResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteJsonPut<TRequest, TResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonPut<TRequest, TResponse, TErrorResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteJsonPut<TRequest, TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<TRequest, TResponse, TErrorResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteJsonPut<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(restResponseTypes, PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteJsonPut<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteJsonMethod<TRequest>(restResponseTypes, PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}





		public static System.Net.HttpStatusCode? ExecuteXmlPut<TRequest>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteXmlPut<TRequest>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteXmlPut<TRequest, TResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteXmlPut<TRequest, TResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteXmlPut<TRequest, TResponse, TErrorResponse>(string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse, TErrorResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteXmlPut<TRequest, TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<TRequest, TResponse, TErrorResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteXmlPut<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(restResponseTypes, PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteXmlPut<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteXmlMethod<TRequest>(restResponseTypes, PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}







		public static System.Net.HttpStatusCode? ExecuteFormRequestPut(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestPut(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteFormRequestPut<TResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestPut<TResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestPut<TResponse, TErrorResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse, TErrorResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestPut<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<FormDataCollection, TResponse, TErrorResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteFormRequestPut(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(restResponseTypes, PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestPut(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod<FormDataCollection>(restResponseTypes, PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecuteFormRequestJsonPut(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestJsonPut(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteFormRequestJsonPut<TResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestJsonPut<TResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestJsonPut<TResponse, TErrorResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse, TErrorResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestJsonPut<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteJsonMethod<FormDataCollection, TResponse, TErrorResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteFormRequestJsonPut(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(restResponseTypes, PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestJsonPut(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteJsonMethod<FormDataCollection>(restResponseTypes, PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}





		public static System.Net.HttpStatusCode? ExecuteFormRequestXmlPut(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestXmlPut(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static TResponse ExecuteFormRequestXmlPut<TResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestXmlPut<TResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestXmlPut<FormDataCollection, TResponse, TErrorResponse>(string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where FormDataCollection : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse, TErrorResponse>(PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestXmlPut<TResponse, TErrorResponse>(Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteXmlMethod<FormDataCollection, TResponse, TErrorResponse>(PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static IRestResponseWrapper ExecuteFormRequestXmlPut(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(restResponseTypes, PutHttpMethod, url, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestXmlPut(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteXmlMethod<FormDataCollection>(restResponseTypes, PutHttpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static string ExecuteTextPut(string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return (ExecuteTextMethod<NoRequest>(PutHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}

		public static string ExecuteTextPut(Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return (ExecuteTextMethod<NoRequest>(PutHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}






		public static string ExecuteTextPut<TRequest>(RestResponseTypeCollection restResponseTypes, string url, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return (ExecuteTextMethod<TRequest>(restResponseTypes, PutHttpMethod, url, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}

		public static string ExecuteTextPut<TRequest>(RestResponseTypeCollection restResponseTypes, Uri uri, HeaderCollection headers, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return (ExecuteTextMethod<TRequest>(restResponseTypes, PutHttpMethod, uri, headers, null, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback).Response as TextResponse)?.Content;
		}
	}
}
