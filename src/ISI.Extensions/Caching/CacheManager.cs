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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Caching
{
	public class CacheManager<TMemoryCache> : AbstractCacheManager
		where TMemoryCache : Microsoft.Extensions.Caching.Memory.IMemoryCache
	{
		protected Microsoft.Extensions.Caching.Memory.IMemoryCache MemoryCache { get; }

		//private object _innerMemoryCache = null;
		//private object InnerMemoryCache => _innerMemoryCache ??= MemoryCache.GetType().GetProperty("EntriesCollection", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?.GetValue(MemoryCache);

		//private System.Reflection.MethodInfo _innerMemoryCacheClearMethodInfo = null;
		//private System.Reflection.MethodInfo InnerMemoryCacheClearMethodInfo => _innerMemoryCacheClearMethodInfo ??= InnerMemoryCache.GetType().GetMethod("Clear", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

		//private System.Reflection.MethodInfo _innerMemoryCacheKeysMethodInfo = null;
		//private System.Reflection.MethodInfo InnerMemoryCacheKeysMethodInfo => _innerMemoryCacheKeysMethodInfo ??= InnerMemoryCache.GetType().GetMethod("Keys", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

		public CacheManager(
			ISI.Extensions.Caching.Configuration configuration,
			TMemoryCache memoryCache)
		: base(configuration)
		{
			MemoryCache = memoryCache;
		}

		public override void Dispose() => MemoryCache?.Dispose();

		public override bool TryGetValue(string cacheKey, out object value)
		{
			if (MemoryCache.TryGetValue(CacheKeyFormatter(cacheKey), out value))
			{
				if (value is CachedItemProxy cachedItemProxy)
				{
					return TryGetValue(cachedItemProxy.CacheKey, out value);
				}

				return true;
			}

			return false;
		}

		public override Microsoft.Extensions.Caching.Memory.ICacheEntry CreateEntry(string cacheKey) => MemoryCache.CreateEntry(CacheKeyFormatter(cacheKey));

		public override void ClearCache()
		{
			InvokeOnClearCache(null);

			if (MemoryCache is Microsoft.Extensions.Caching.Memory.MemoryCache memoryCache)
			{
				memoryCache.Clear();
			}
		}

		public override void RemoveByCacheKeyPrefix(string cacheKeyPrefix)
		{
			if (MemoryCache is Microsoft.Extensions.Caching.Memory.MemoryCache memoryCache)
			{
				var cacheKeys = memoryCache.Keys;

				foreach (var cacheKey in cacheKeys.NullCheckedSelect(key => GetRawCacheKey(string.Format("{0}", key))))
				{
					if (cacheKey.StartsWith(cacheKeyPrefix))
					{
						Remove(cacheKey);
					}
				}
			}
		}

		public override void Remove(string cacheKey)
		{
			if (MemoryCache.TryGetValue(CacheKeyFormatter(cacheKey), out var value))
			{
				if (value is CachedItemProxy cachedItemProxy)
				{
					Remove(cachedItemProxy.CacheKey);
				}
				else
				{
					MemoryCache.Remove(CacheKeyFormatter(cacheKey));
				}
			}

			var cachedItemProxiesCacheKey = CachedItemProxies.GetCachedItemProxiesCacheKey(cacheKey);
			if (MemoryCache.TryGetValue(CacheKeyFormatter(cachedItemProxiesCacheKey), out value))
			{
				if (value is CachedItemProxies cachedItemProxies)
				{
					foreach (var proxyCacheKey in cachedItemProxies.ProxyCacheKeys)
					{
						if (MemoryCache.TryGetValue(CacheKeyFormatter(proxyCacheKey), out var _))
						{
							MemoryCache.Remove(CacheKeyFormatter(proxyCacheKey));
						}
					}
				}

				MemoryCache.Remove(CacheKeyFormatter(cachedItemProxiesCacheKey));
			}
		}
	}
}
