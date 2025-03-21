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
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ISI.Extensions.Caching.Extensions
{
	public static partial class CacheManagerExtensions
	{
		public static IDictionary<TKey, TItem> GetOrCreate<TKey, TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, IEnumerable<TKey> cacheKeys, GenerateCacheKey<TKey> getCacheKey, GetItems<TKey, TItem> getItems, GetItems<TKey, TItem> getDefaultValues)
		{
			return GetOrCreate(cacheManager, cacheKeys, getCacheKey, getItems, getDefaultValues, null, null, false);
		}

		public static IDictionary<TKey, TItem> GetOrCreate<TKey, TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, IEnumerable<TKey> cacheKeys, GenerateCacheKey<TKey> getCacheKey, GetItems<TKey, TItem> getItems, GetItems<TKey, TItem> getDefaultValues, GenerateCacheKeys<TItem> generateProxyCacheKeys, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy, bool forceRefreshCache)
		{
			var result = new Dictionary<TKey, TItem>();

			var neededCacheKeys = cacheKeys.Distinct().ToDictionary(key => getCacheKey(key), key => key);

			#region Get from Cache
			if (!forceRefreshCache)
			{
				var foundCacheKeys = new List<string>();

				foreach (var neededCacheKey in neededCacheKeys)
				{
					if (cacheManager.TryGetValue<TItem>(neededCacheKey.Key, out var item))
					{
						result.Add(neededCacheKey.Value, item);
						foundCacheKeys.Add(neededCacheKey.Key);
					}
				}

				foreach (var foundCacheKey in foundCacheKeys)
				{
					neededCacheKeys.Remove(foundCacheKey);
				}
			}
			#endregion

			#region Use getItems
			if (neededCacheKeys.Any())
			{
				var foundCacheKeys = new List<string>();

				var items = getItems(neededCacheKeys.Select(neededKey => neededKey.Value));

				foreach (var item in items.Where(i => i.Value != null))
				{
					result.Add(item.Key, item.Value);

					var cacheKey = getCacheKey(item.Key);
					Add(cacheManager, cacheKey, item.Value, getCacheEntryExpirationPolicy);
					foundCacheKeys.Add(cacheKey);

					if (generateProxyCacheKeys != null)
					{
						AddCacheKeyProxies(cacheManager, cacheKey, item.Value, generateProxyCacheKeys);
					}
				}

				foreach (var foundFormattedCacheKey in foundCacheKeys)
				{
					neededCacheKeys.Remove(foundFormattedCacheKey);
				}
			}
			#endregion

			#region Use getDefaultValues
			if (neededCacheKeys.Any() && (getDefaultValues != null))
			{
				var items = getDefaultValues(neededCacheKeys.Select(neededFormattedCacheKey => neededFormattedCacheKey.Value));

				foreach (var item in items)
				{
					result.Add(item.Key, item.Value);

					var cacheKey = getCacheKey(item.Key);
					Add(cacheManager, cacheKey, item.Value, getCacheEntryExpirationPolicy);

					if (generateProxyCacheKeys != null)
					{
						AddCacheKeyProxies(cacheManager, cacheKey, item.Value, generateProxyCacheKeys);
					}
				}
			}
			#endregion

			return result;
		}

		public static TItem GetOrCreate<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, string cacheKey, GetItem<TItem> getItem, GetItem<TItem> getDefaultValue, GenerateCacheKeys<TItem> generateProxyCacheKeys, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy, bool forceRefreshCache)
		{
			if (!forceRefreshCache)
			{
				if (cacheManager.TryGetValue<TItem>(cacheKey, out var cachedValue))
				{
					return cachedValue;
				}
			}

			cacheManager.Remove(cacheKey);

			if (getItem == null)
			{
				if (getDefaultValue == null)
				{
					return default;
				}

				return getDefaultValue();
			}

			var value = (TItem)getItem();

			Add(cacheManager, cacheKey, value, getCacheEntryExpirationPolicy);

			if (generateProxyCacheKeys != null)
			{
				AddCacheKeyProxies(cacheManager, cacheKey, value, generateProxyCacheKeys);
			}

			return value;
		}

		public static TItem GetOrCreate<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, string cacheKey, Func<ICacheEntry, TItem> getItem)
		{
			if (!cacheManager.TryGetValue(cacheKey, out var item))
			{
				using (var entry = cacheManager.CreateEntry(cacheKey))
				{
					item = getItem(entry);

					entry.SetValue(item);
				}

				if (item is ISI.Extensions.Caching.IHasProxyCacheKeys hasProxyCacheKeys)
				{
					var proxyCacheKeys = new HashSet<string>(StringComparer.Ordinal);
					proxyCacheKeys.UnionWith(hasProxyCacheKeys.ProxyCacheKeys ?? []);
					proxyCacheKeys.Remove(cacheKey);

					if (proxyCacheKeys.Any())
					{
						foreach (var proxyCacheKey in proxyCacheKeys)
						{
							Add(cacheManager, proxyCacheKey, new CachedItemProxy()
							{
								CacheKey = cacheKey,
							}, () => cacheManager.GetDefaultCacheEntryExpirationPolicy(item));
						}

						Add(cacheManager, CachedItemProxies.GetCachedItemProxiesCacheKey(cacheKey), new CachedItemProxies()
						{
							ProxyCacheKeys = proxyCacheKeys.ToArray(),
						}, () => cacheManager.GetDefaultCacheEntryExpirationPolicy(item));
					}
				}
			}

			return (TItem)item;
		}

	}
}