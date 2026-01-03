#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
		public static System.Net.HttpStatusCode? ExecuteFormRequestMethod(string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteFormRequestMethod(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteFormRequestMethod(string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			var response = ExecuteFormRequestMethod<IgnoredResponse, UnhandledExceptionResponse>(httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.StatusCode;
		}






		public static TResponse ExecuteFormRequestMethod<TResponse>(string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			return ExecuteFormRequestMethod<TResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteFormRequestMethod<TResponse>(string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
		{
			var response = ExecuteFormRequestMethod(new()
			{
				DefaultRestResponseType = typeof(TResponse)
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.Response as TResponse;
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestMethod<TResponse, TErrorResponse>(string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteFormRequestMethod<TResponse, TErrorResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteFormRequestMethod<TResponse, TErrorResponse>(string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TResponse : class
			where TErrorResponse : class
		{
			var response = ExecuteFormRequestMethod(new()
			{
				DefaultRestResponseType = typeof(TResponse),
				DefaultErrorRestResponseType = typeof(TErrorResponse),
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return new(response.StatusCode, (response.IsErrorResponse ? null : response.Response as TResponse), (response.IsErrorResponse ? response.Response as TErrorResponse : null));
		}






		public static IRestResponseWrapper ExecuteFormRequestMethod(RestResponseTypeCollection restResponseTypes, string httpMethod, string url, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteFormRequestMethod(restResponseTypes, httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteFormRequestMethod(RestResponseTypeCollection restResponseTypes, string httpMethod, Uri uri, HeaderCollection headers, FormDataCollection request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
		{
			return ExecuteMethod(GetContentTypeHeaderValue<FormDataCollection>(), AcceptAllHeaderValue, typeof(RestResponseWrapper<>), restResponseTypes, httpMethod, uri, headers, request, GetExecuteMethodBodyBuilder<FormDataCollection>(), throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}






		public static System.Net.HttpStatusCode? ExecuteMethod<TRequest>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static System.Net.HttpStatusCode? ExecuteMethod<TRequest>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			var response = ExecuteMethod<TRequest, IgnoredResponse, UnhandledExceptionResponse>(httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.StatusCode;
		}






		public static TResponse ExecuteMethod<TRequest, TResponse>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			return ExecuteMethod<TRequest, TResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static TResponse ExecuteMethod<TRequest, TResponse>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
		{
			var response = ExecuteMethod<TRequest>(new()
			{
				DefaultRestResponseType = typeof(TResponse)
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return response.Response as TResponse;
		}






		public static RestResponse<TResponse, TErrorResponse> ExecuteMethod<TRequest, TResponse, TErrorResponse>(string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			return ExecuteMethod<TRequest, TResponse, TErrorResponse>(httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static RestResponse<TResponse, TErrorResponse> ExecuteMethod<TRequest, TResponse, TErrorResponse>(string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
			where TResponse : class
			where TErrorResponse : class
		{
			var response = ExecuteMethod<TRequest>(new()
			{
				DefaultRestResponseType = typeof(TResponse),
				DefaultErrorRestResponseType = typeof(TErrorResponse),
			}, httpMethod, uri, headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);

			return new(response.StatusCode, (response.IsErrorResponse ? null : response.Response as TResponse), (response.IsErrorResponse ? response.Response as TErrorResponse : null));
		}






		public static IRestResponseWrapper ExecuteMethod<TRequest>(RestResponseTypeCollection restResponseTypes, string httpMethod, string url, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(restResponseTypes, httpMethod, new Uri(url), headers, request, throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}

		public static IRestResponseWrapper ExecuteMethod<TRequest>(RestResponseTypeCollection restResponseTypes, string httpMethod, Uri uri, HeaderCollection headers, TRequest request, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes = null, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = null, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates = null, System.Net.CookieContainer cookieContainer = null)
			where TRequest : class
		{
			return ExecuteMethod<TRequest>(GetContentTypeHeaderValue<TRequest>(), AcceptAllHeaderValue, typeof(RestResponseWrapper<>), restResponseTypes, httpMethod, uri, headers, request, GetExecuteMethodBodyBuilder<TRequest>(), throwUnhandledException, overRideSecurityProtocolTypes, serverCertificateValidationCallback, clientCertificates, cookieContainer);
		}


		private static System.Net.HttpWebRequest CreateWebRequest(string contentType, string acceptTypes, string httpMethod, Uri uri, HeaderCollection headers, System.Net.CookieContainer cookieContainer, IProxyWrapper proxyWrapper, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates)
		{
			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(proxyWrapper?.Uri ?? uri);
			webRequest.CookieContainer = cookieContainer;
			webRequest.AllowAutoRedirect = false;

			//webRequest.Timeout = Int32.MaxValue;

			if (headers.NullCheckedAny())
			{
				headers.ApplyToWebRequest(webRequest);
			}

			webRequest.Proxy = proxyWrapper?.Proxy ?? System.Net.WebRequest.DefaultWebProxy;
			webRequest.Method = httpMethod;
			webRequest.ContentType = headers?.PreferredContentType ?? webRequest.ContentType;
			if (!string.Equals(httpMethod, System.Net.WebRequestMethods.Http.Get))
			{
				webRequest.ContentType = headers?.PreferredContentType ?? contentType;
			}
			webRequest.Accept = headers?.PreferredAccept ?? acceptTypes;

			if (serverCertificateValidationCallback != null)
			{
				webRequest.ServerCertificateValidationCallback += serverCertificateValidationCallback;
			}
			else if (RestConfiguration?.IgnoreServerCertificateValidationForSubjectsContaining?.NullCheckedAny() ?? false)
			{
				webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
				{
					if (errors == System.Net.Security.SslPolicyErrors.None)
					{
						return true;
					}

					foreach (var subject in RestConfiguration.IgnoreServerCertificateValidationForSubjectsContaining)
					{
						if (certificate.Subject.IndexOf(subject, StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							return true;
						}

						Console.WriteLine(certificate.Subject);
					}

					return false;
				};
			}

			if (clientCertificates != null)
			{
				webRequest.ClientCertificates = clientCertificates;
			}

			return webRequest;
		}


		public static IRestResponseWrapper ExecuteMethod<TRequest>(string contentType, string acceptTypes, Type restResponseWrapperType, RestResponseTypeCollection restResponseTypes, string httpMethod, Uri uri, HeaderCollection headers, TRequest request, ExecuteMethodBuildBody<TRequest> bodyBuilder, bool throwUnhandledException, System.Security.Authentication.SslProtocols? overRideSecurityProtocolTypes, System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Net.CookieContainer cookieContainer)
			where TRequest : class
		{
			var webRequestDetails = _EventHandler?.WebRequestDetails ?? new WebRequestDetails();

			webRequestDetails.Clear();

			webRequestDetails.SetHttpMethod(httpMethod);
			webRequestDetails.SetUri(uri);

			IRestResponseWrapper response = null;

			IProxyWrapper proxyWrapper = null;

			if (overRideSecurityProtocolTypes.HasValue && string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase))
			{
				proxyWrapper = new SslProtocolWebProxy(uri, overRideSecurityProtocolTypes.Value, clientCertificates);
				clientCertificates = null;
			}
			
			proxyWrapper ??= ProxyWrapper;

			System.IO.Stream requestStream = null;

			try
			{
				headers?.ProcessContent(() =>
				{
					bodyBuilder?.Invoke(request, null, webRequestDetails);

					return webRequestDetails.BodyRaw;
				});

				cookieContainer ??= new();
				var webRequest = CreateWebRequest(contentType, acceptTypes, httpMethod, uri, headers, cookieContainer, proxyWrapper, serverCertificateValidationCallback, clientCertificates);

				webRequestDetails.SetHeaders(webRequest.Headers);

				if (!string.Equals(httpMethod, System.Net.WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase) && (request != null))
				{
					bodyBuilder?.Invoke(request, webRequest, webRequestDetails);
				}

				_EventHandler?.Execute();

				var webResponse = webRequest.GetResponse();

				var statusCode = ((System.Net.HttpWebResponse)webResponse).StatusCode;
				while ((statusCode == System.Net.HttpStatusCode.Found) || (statusCode == System.Net.HttpStatusCode.Moved) || (statusCode == System.Net.HttpStatusCode.TemporaryRedirect))
				{
					var location = webResponse.Headers["Location"];

					var redirectUri = (location.StartsWith("/") ? (new UriBuilder(uri) { Path = location, }).Uri : new(location));

					if (uri == redirectUri)
					{
						statusCode = System.Net.HttpStatusCode.OK;
					}
					else
					{
						webResponse?.Dispose();
						webResponse = null;

						uri = redirectUri;

						webRequest = CreateWebRequest(contentType, acceptTypes, httpMethod, uri, headers, cookieContainer, proxyWrapper, serverCertificateValidationCallback, clientCertificates);

						if (!string.Equals(httpMethod, System.Net.WebRequestMethods.Http.Get) && (request != null))
						{
							bodyBuilder?.Invoke(request, webRequest, null);
						}

						webResponse = webRequest.GetResponse();

						statusCode = ((System.Net.HttpWebResponse)webResponse).StatusCode;
					}
				}

				response = restResponseTypes.GetRestResponse(restResponseWrapperType, webResponse, webRequestDetails, false);

				webResponse?.Dispose();
				webResponse = null;
			}
			catch (System.Net.WebException exception)
			{
				using (var webResponse = exception.Response as System.Net.HttpWebResponse)
				{
					response = restResponseTypes.GetRestResponse(restResponseWrapperType, webResponse, webRequestDetails, true, exception);
				}
			}
			catch (Exception exception)
			{
				response = restResponseTypes.GetRestResponse(restResponseWrapperType, null, webRequestDetails, true, exception);
			}

			requestStream?.Close();
			requestStream?.Dispose();
			requestStream = null;

			(proxyWrapper as IDisposable)?.Dispose();
			proxyWrapper = null;

			_EventHandler?.Executed();

			if (throwUnhandledException)
			{
				var unhandledExceptionResponse = response.Response as UnhandledExceptionResponse;
				if (unhandledExceptionResponse?.Exception != null)
				{
					throw unhandledExceptionResponse.Exception;
				}

				if ((response is IRestExceptionResponse restExceptionResponse) && (restExceptionResponse?.Exception != null))
				{
					throw restExceptionResponse.Exception;
				}
			}

			return response;
		}
	}
}
