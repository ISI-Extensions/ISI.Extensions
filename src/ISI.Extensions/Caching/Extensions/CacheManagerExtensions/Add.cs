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
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ISI.Extensions.Caching.Extensions
{
	public static partial class CacheManagerExtensions
	{
		public static void Add<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, string cacheKey, TItem item, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null)
		{
			using (var cacheEntry = cacheManager.CreateEntry(cacheKey))
			{
				cacheEntry.Value = item;

				(getCacheEntryExpirationPolicy?.Invoke() ?? cacheManager.GetDefaultCacheEntryExpirationPolicy(item))?.SetCacheEntryExpiration(cacheEntry);
			}

			if (item is ISI.Extensions.Caching.IHasProxyCacheKeys hasProxyCacheKeys)
			{
				var proxyCacheKeys = new HashSet<string>(StringComparer.Ordinal);
				proxyCacheKeys.UnionWith(hasProxyCacheKeys.ProxyCacheKeys ?? Array.Empty<string>());
				proxyCacheKeys.Remove(cacheKey);

				if (proxyCacheKeys.Any())
				{
					foreach (var proxyCacheKey in proxyCacheKeys)
					{
						Add(cacheManager, proxyCacheKey, new CachedItemProxy()
						{
							CacheKey = cacheKey,
						}, getCacheEntryExpirationPolicy);
					}

					Add(cacheManager, CachedItemProxies.GetCachedItemProxiesCacheKey(cacheKey), new CachedItemProxies()
					{
						ProxyCacheKeys = proxyCacheKeys.ToArray(),
					}, getCacheEntryExpirationPolicy);
				}
			}
		}

		public static void Add<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, TItem item)
			where TItem : ISI.Extensions.Caching.IHasCacheKey
		{
			Add(cacheManager, item.CacheKey, item);
		}

		public static void AddCacheKeyProxies<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, string cacheKey, TItem item, GenerateCacheKeys<TItem> generateProxyCacheKeys, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null)
		{
			var proxyCacheKeys = new HashSet<string>(StringComparer.Ordinal);
			proxyCacheKeys.UnionWith(generateProxyCacheKeys(item));
			proxyCacheKeys.Remove(cacheKey);

			if (proxyCacheKeys.Any())
			{
				foreach (var proxyCacheKey in proxyCacheKeys)
				{
					Add(cacheManager, proxyCacheKey, new CachedItemProxy()
					{
						CacheKey = cacheKey,
					}, getCacheEntryExpirationPolicy);
				}

				Add(cacheManager, CachedItemProxies.GetCachedItemProxiesCacheKey(cacheKey), new CachedItemProxies()
				{
					ProxyCacheKeys = proxyCacheKeys.ToArray(),
				}, getCacheEntryExpirationPolicy);
			}
		}
	}
}