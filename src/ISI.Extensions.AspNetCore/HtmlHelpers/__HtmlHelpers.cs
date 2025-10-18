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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Caching.Extensions;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class HtmlHelpers
	{
		#region CacheManager
		private static ISI.Extensions.Caching.ICacheManager _cacheManager = null;
		public static ISI.Extensions.Caching.ICacheManager CacheManager => _cacheManager ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Caching.ICacheManager>();
		#endregion

		//public static TimeSpan CacheableDuration = Configuration.Current.DynamicPages.CacheableDuration;

		#region DynamicPagesHelper
		//private static ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.DynamicPagesHelper _dynamicPagesHelper = null;
		//public static ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.DynamicPagesHelper DynamicPagesHelper => _dynamicPagesHelper ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.DynamicPagesHelper>();
		#endregion

		[Flags]
		public enum StylesheetMediaTypes
		{
			[ISI.Extensions.Enum("All", "all")] All = 1,
			[ISI.Extensions.Enum("Braille", "braille")] Braille = 2,
			[ISI.Extensions.Enum("Embossed", "embossed")] Embossed = 4,
			[ISI.Extensions.Enum("Handheld", "handheld")] Handheld = 8,
			[ISI.Extensions.Enum("Print", "print")] Print = 16,
			[ISI.Extensions.Enum("Projection", "projection")] Projection = 32,
			[ISI.Extensions.Enum("Screen", "screen")] Screen = 64,
			[ISI.Extensions.Enum("Speech", "speech")] Speech = 128,
			[ISI.Extensions.Enum("TTY", "tty")] TTY = 256,
			[ISI.Extensions.Enum("TV", "tv")] TV = 512
		}

		internal static string CommaDelimitedList(StylesheetMediaTypes stylesheetMediaTypes)
		{
			return string.Join(", ", from mediaType
															 in ISI.Extensions.Enum<StylesheetMediaTypes>.ToArray()
															 where (stylesheetMediaTypes & mediaType) == mediaType
															 select mediaType.GetAbbreviation());
		}

		public class StylesheetContentUrl : ISI.Extensions.AspNetCore.IContentUrl
		{
			public string Media { get; }
			public ISI.Extensions.AspNetCore.IContentUrl ContentUrl { get; }

			public StylesheetContentUrl(string media, global::ISI.Extensions.AspNetCore.IContentUrl contentUrl)
			{
				Media = media;
				ContentUrl = contentUrl;
			}

			public StylesheetContentUrl(string media, string contentUrl)
			{
				Media = media;
				ContentUrl = new ISI.Extensions.AspNetCore.ContentUrl(contentUrl);
			}

			public StylesheetContentUrl(string url)
			{
				var urlPieces = url.Split(["\t"], StringSplitOptions.None);

				if (urlPieces.Length > 1)
				{
					Media = urlPieces[0];
					ContentUrl = new ISI.Extensions.AspNetCore.ContentUrl(urlPieces[1]);
				}
				else
				{
					ContentUrl = new ISI.Extensions.AspNetCore.ContentUrl(urlPieces[0]);
				}
			}

			public string VirtualPath => ContentUrl.VirtualPath;

			public bool IsExternalContent => ContentUrl.IsExternalContent;

			public string GetUrl(bool considerContentDistributionNetwork = true)
			{
				return ContentUrl.GetUrl(considerContentDistributionNetwork);
			}

			public string GetCacheBusterKey()
			{
				return ContentUrl.GetCacheBusterKey();
			}

			public override string ToString()
			{
				return GetUrl(true);
			}

			public void WriteTo(System.IO.TextWriter writer, System.Text.Encodings.Web.HtmlEncoder encoder)
			{
				writer.Write(ToString());
			}

			public string ToHtmlString()
			{
				return ToString();
			}
		}

		public class ContentUrlCollection : HashSet<ISI.Extensions.AspNetCore.IContentUrl>
		{
			#region ContentUrlComparer
			public class ContentUrlComparer : IEqualityComparer<ISI.Extensions.AspNetCore.IContentUrl>
			{
				private static ContentUrlComparer _instance = null;
				public static ContentUrlComparer Instance => _instance ??= new();

				private ContentUrlComparer()
				{

				}

				public bool Equals(ISI.Extensions.AspNetCore.IContentUrl x, ISI.Extensions.AspNetCore.IContentUrl y)
				{
					var xKey = $"{(x is StylesheetContentUrl ? ((StylesheetContentUrl)x).Media : "-")}\t{x.VirtualPath}";
					var yKey = $"{(y is StylesheetContentUrl ? ((StylesheetContentUrl)y).Media : "-")}\t{y.VirtualPath}";

					return string.Equals(xKey, yKey, StringComparison.InvariantCultureIgnoreCase);
				}

				public int GetHashCode(ISI.Extensions.AspNetCore.IContentUrl obj)
				{
					return obj.GetHashCode();
				}
			}
			#endregion

			public ContentUrlCollection()
				: base(ContentUrlComparer.Instance)
			{

			}
			public ContentUrlCollection(IEnumerable<ISI.Extensions.AspNetCore.IContentUrl> contentUrls)
				: base(contentUrls, ContentUrlComparer.Instance)
			{

			}

			public new void Add(ISI.Extensions.AspNetCore.IContentUrl item)
			{
				if ((item is StylesheetContentUrl) && string.IsNullOrEmpty(((StylesheetContentUrl)item).Media))
				{
					base.Add(((StylesheetContentUrl)item).ContentUrl);
				}
				else
				{
					base.Add(item);
				}
			}

			public string Key()
			{
				return string.Join("+", this.Select(contentUrl => $"{contentUrl.VirtualPath}{(contentUrl.VirtualPath.IndexOf("?") > 0 ? "&" : "?")}{contentUrl.GetCacheBusterKey()}")).ToLower();
			}
		}

		#region GetKeyFromCombinedUrls
		public static string GetKeyFromCombinedUrls(HtmlHelpers.ContentUrlCollection contentUrls)
		{
			var urlCollectionKey = contentUrls.Key();

			//var cacheKey = ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.Constants.KeyFromCombinedUrlsCacheKey(urlCollectionKey);

			//return CacheManager.Get(cacheKey, () =>
			//	{
			//		var key = Areas.DynamicPages.DefinitionCache.PersistenceFactory.Persister.GetDefinitionKey(urlCollectionKey);

			//		if (string.IsNullOrWhiteSpace(key))
			//		{
			//			key = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting);

			//			Areas.DynamicPages.DefinitionCache.PersistenceFactory.Persister.SetDefinition(key, urlCollectionKey);
			//		}

			//		CacheManager.AddWithAbsoluteTimeExpiration(ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.Constants.CombinedUrlsFromKeyCacheKey(key), urlCollectionKey, CacheableDuration);

			//		return key;
			//	}, null, () => CacheManager.AbsoluteTimeExpirationCacheItemPolicy(CacheableDuration));

			return null;
		}
		#endregion
	}
}
