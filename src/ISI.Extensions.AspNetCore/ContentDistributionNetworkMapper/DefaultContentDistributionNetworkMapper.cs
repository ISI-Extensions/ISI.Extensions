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
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore
{
	public class DefaultContentDistributionNetworkMapper : IContentDistributionNetworkMapper
	{
		private static ISI.Extensions.Caching.ICacheManager _cacheManager = null;
		private static ISI.Extensions.Caching.ICacheManager CacheManager => _cacheManager ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Caching.ICacheManager>();

		private static Configuration _configuration = null;
		private static Configuration Configuration => _configuration ??= ISI.Extensions.ServiceLocator.Current.GetService<Configuration>(() => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
		{
			MapToType = typeof(Configuration),
			ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
		});

		private static bool? _useWhenUsingSsl = null;

		private static bool UseWhenUsingSsl
		{
			get
			{
				if (!_useWhenUsingSsl.HasValue)
				{
					switch (Configuration.ContentDistributionNetwork.UseWhenUsingSsl)
					{
						case ISI.Extensions.Configuration.EnabledStatus.Default:
							_useWhenUsingSsl = false;
							break;
						case ISI.Extensions.Configuration.EnabledStatus.Always:
							_useWhenUsingSsl = true;
							break;
						case ISI.Extensions.Configuration.EnabledStatus.Never:
							_useWhenUsingSsl = false;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				return _useWhenUsingSsl.GetValueOrDefault();
			}
		}

		private static Uri _replacementRootUri = null;
		private static Uri ReplacementRootUri => _replacementRootUri ??= (new UriBuilder(Configuration.ContentDistributionNetwork.ReplacementRootUrl)).Uri;

		private static Uri _sslReplacementRootUri = null;

		private static Uri SslReplacementRootUri
		{
			get
			{
				if (_sslReplacementRootUri == null)
				{
					var sslReplacementRootUri = Configuration.ContentDistributionNetwork.SslReplacementRootUrl;

					if (string.IsNullOrEmpty(sslReplacementRootUri))
					{
						_sslReplacementRootUri = (new UriBuilder(Configuration.ContentDistributionNetwork.ReplacementRootUrl)).Uri;
					}
					else
					{
						_sslReplacementRootUri = (new UriBuilder(sslReplacementRootUri)).Uri;
					}
				}

				return _sslReplacementRootUri;
			}
		}

		public bool CanRemap(string url)
		{
			try
			{
				//return ((System.Web.HttpContext.Current != null) && (System.Web.HttpContext.Current.Request != null));
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				return false;
			}

			return false;
		}

		private static string CdnUrlCacheKey(string url)
		{
			//var urlScheme = (url.Contains("://") ? (new UriBuilder(url)).Scheme : System.Web.HttpContext.Current.Request.GetScheme());
			var urlScheme = (url.Contains("://") ? (new UriBuilder(url)).Scheme : "https");

			var pathAndQueryStringParts = url.Split(new string[] { "?" }, StringSplitOptions.None);

			return string.Format("cdn-url:{0}-{1}", urlScheme, pathAndQueryStringParts[0]);
		}

		private static string OriginalUrlCacheKey(string url)
		{
			var pathAndQueryStringParts = url.Split(new string[] { "?" }, StringSplitOptions.None);

			return string.Format("cdn-original-url:{0}", pathAndQueryStringParts[0]);
		}

		public string GetCdnUrl(string url)
		{
			return url;

			//url = GetOriginalUrl(url);

			//return Cache.Get<string>(CdnUrlCacheKey(url), () =>
			//{
			//	var cdnUrl = url;

			//	cdnUrl = System.Web.VirtualPathUtility.ToAbsolute(url);

			//	var urlScheme = (cdnUrl.Contains("://") ? (new UriBuilder(cdnUrl)).Scheme : System.Web.HttpContext.Current.Request.GetScheme());
			//	var urlSchemeIsSsl = string.Equals(urlScheme, ISI.Libraries.Web.Scheme.SslScheme, StringComparison.InvariantCultureIgnoreCase);

			//	var requestUri = new UriBuilder(System.Web.HttpContext.Current.Request.Url)
			//	{
			//		Path = System.Web.VirtualPathUtility.ToAbsolute("~/")
			//	};

			//	if (UseWhenUsingSsl || !urlSchemeIsSsl)
			//	{
			//		var useSsl = ISI.Libraries.Web.Scheme.AllowSslUrlGeneration && urlSchemeIsSsl;

			//		var replacementRootUri = (useSsl ? SslReplacementRootUri : ReplacementRootUri);

			//		if (cdnUrl.StartsWith(requestUri.ToString(), StringComparison.CurrentCultureIgnoreCase))
			//		{
			//			var uri = new UriBuilder(cdnUrl)
			//			{
			//				Query = string.Empty
			//			};

			//			uri.Host = replacementRootUri.Host;
			//			uri.Port = replacementRootUri.Port;

			//			uri.Path = string.Format("{0}{1}", replacementRootUri.AbsolutePath, uri.Path.Substring(requestUri.Path.Length));

			//			cdnUrl = uri.Uri.ToString();
			//		}

			//		if (cdnUrl.StartsWith("/"))
			//		{
			//			var uri = new UriBuilder(requestUri.ToString());

			//			uri.Host = replacementRootUri.Host;
			//			uri.Port = replacementRootUri.Port;

			//			if (replacementRootUri.AbsolutePath.Length > 1)
			//			{
			//				uri.Path = string.Format("{0}{1}", replacementRootUri.AbsolutePath, uri.Path);
			//			}

			//			var pathAndQueryStringParts = cdnUrl.Split(new string[] { "?" }, StringSplitOptions.None);

			//			if (pathAndQueryStringParts.Length > 0)
			//			{
			//				uri.Path = string.Format("{0}{1}", uri.Path, pathAndQueryStringParts[0].Substring(requestUri.Path.Length));
			//			}

			//			cdnUrl = uri.Uri.ToString();
			//		}
			//	}

			//	Cache.AddWithSlidingTimeExpiration(OriginalUrlCacheKey(cdnUrl), url, new TimeSpan(1, 0, 0, 0));

			//	return cdnUrl;
			//}, null, () => Cache.SlidingTimeExpirationCacheItemPolicy(new TimeSpan(1, 0, 0, 0)));
		}

		public string GetOriginalUrl(string url)
		{
			var result = url;

			//var key = OriginalUrlCacheKey(url);

			//if (Cache.ContainsKey(key))
			//{
			//	result = Cache.Get<string>(key);
			//}
			//else if (url.Contains("://"))
			//{
			//	var uri = new UriBuilder(url);

			//	if (string.Equals(uri.Host, ReplacementRootUri.Host, StringComparison.InvariantCultureIgnoreCase) &&
			//	    uri.Path.StartsWith(ReplacementRootUri.AbsolutePath, StringComparison.InvariantCultureIgnoreCase))
			//	{
			//		result = uri.Path;

			//		if (ReplacementRootUri.AbsolutePath.Length > 1)
			//		{
			//			result = result.Substring(ReplacementRootUri.AbsolutePath.Length);
			//		}

			//		if (!result.StartsWith("//"))
			//		{
			//			result = "//" + result;
			//		}

			//		if (!result.StartsWith("~"))
			//		{
			//			result = "~" + result;
			//		}
			//	}
			//}

			return result;
		}
	}
}