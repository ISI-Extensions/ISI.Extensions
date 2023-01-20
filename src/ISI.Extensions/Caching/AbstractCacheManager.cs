#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public abstract class AbstractCacheManager : ICacheManager
	{
		public Guid CacheManagerInstanceUuid { get; } = Guid.NewGuid();

		protected ISI.Extensions.Caching.Configuration Configuration { get; }

		protected string CacheKeyPrefix => Configuration.CacheKeyPrefix;

		private bool? _hasCacheKeyPrefix = null;
		protected bool HasCacheKeyPrefix => _hasCacheKeyPrefix ??= !string.IsNullOrEmpty(CacheKeyPrefix);

		private int? _cacheKeyPrefixLength = null;
		protected int CacheKeyPrefixLength => _cacheKeyPrefixLength ??= (CacheKeyPrefix ?? string.Empty).Length;

		protected string CacheKeyFormatter(string rawCacheKey)
		{
			if (HasCacheKeyPrefix)
			{
				return string.Format("{0}{1}", CacheKeyPrefix, rawCacheKey);
			}

			return rawCacheKey;
		}

		protected string GetRawCacheKey(string cacheKey)
		{
			if (HasCacheKeyPrefix && (cacheKey.Length > CacheKeyPrefixLength))
			{
				return cacheKey.Substring(CacheKeyPrefixLength);
			}

			return cacheKey;
		}

		protected DefaultCacheEntryExpirationPolicyGetter DefaultCacheEntryExpirationPolicyGetter { get; set; }

		public AbstractCacheManager(
			ISI.Extensions.Caching.Configuration configuration)
		{
			Configuration = configuration;

			DefaultCacheEntryExpirationPolicyGetter = itemToCache =>
			{
				if (itemToCache is IHasCacheAbsoluteDateTimeExpiration absoluteTimeExpiration)
				{
					return GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(absoluteTimeExpiration.CacheAbsoluteDateTimeExpiration);
				}

				if (itemToCache is IHasCacheSlidingTimeExpiration slidingTimeExpiration)
				{
					return GetSlidingTimeExpirationCacheEntryExpirationPolicy(slidingTimeExpiration.CacheSlidingTimeExpiration);
				}

				if (itemToCache is IHasCacheTimeToLive timeToLive)
				{
					return GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(TimeSpan.FromSeconds(timeToLive.CacheTimeToLiveInSeconds));
				}

				return GetAbsoluteTimeExpirationCacheEntryExpirationPolicy();
			};
		}

		public abstract void Dispose();

		public abstract bool TryGetValue(string cacheKey, out object value);

		public abstract Microsoft.Extensions.Caching.Memory.ICacheEntry CreateEntry(string cacheKey);

		public abstract void ClearCache();

		public virtual void ClearCache(ClearCacheRequest request)
		{
			if (request.ClearAll)
			{
				ClearCache();
			}
			else
			{
				foreach (var cacheKey in request.CacheKeys.ToNullCheckedArray(NullCheckCollectionResult.Empty))
				{
					Remove(cacheKey);
				}

				if (request.CacheKeysWithCacheKeyInstanceUuid.NullCheckedAny())
				{
					Remove(request.CacheKeysWithCacheKeyInstanceUuid);
				}

				foreach (var cacheKeyPrefix in request.CacheKeyPrefixes.ToNullCheckedArray(NullCheckCollectionResult.Empty))
				{
					RemoveByCacheKeyPrefix(cacheKeyPrefix);
				}
			}
		}

		public abstract void Remove(string cacheKey);

		public abstract void RemoveByCacheKeyPrefix(string cacheKeyPrefix);

		public virtual void Remove(IEnumerable<ClearCacheRequestCacheKeyWithCacheKeyInstanceUuid> cacheKeysWithCacheKeyInstanceUuid)
		{
			foreach (var cacheKeyWithCacheKeyInstanceUuid in cacheKeysWithCacheKeyInstanceUuid)
			{
				if (TryGetValue(cacheKeyWithCacheKeyInstanceUuid.CacheKey, out var item))
				{
					if (item is ISI.Extensions.Caching.IHasCacheKeyInstanceUuid hasCacheKeyInstanceUuid)
					{
						if (hasCacheKeyInstanceUuid.CacheKeyInstanceUuid == cacheKeyWithCacheKeyInstanceUuid.CacheKeyInstanceUuid)
						{
							Remove(cacheKeyWithCacheKeyInstanceUuid.CacheKey);
						}
					}
					else
					{
						Remove(cacheKeyWithCacheKeyInstanceUuid.CacheKey);
					}
				}
			}
		}


		public virtual void SetDefaultCacheEntryExpirationPolicyGetter(DefaultCacheEntryExpirationPolicyGetter defaultCacheEntryExpirationPolicyGetter) => DefaultCacheEntryExpirationPolicyGetter = defaultCacheEntryExpirationPolicyGetter;

		public virtual ICacheEntryExpirationPolicy GetDefaultCacheEntryExpirationPolicy(object itemToCache) => DefaultCacheEntryExpirationPolicyGetter(itemToCache);

		public virtual ICacheEntryExpirationPolicy GetNoExpirationCacheEntryExpirationPolicy() => new NoExpirationCacheEntryExpirationPolicy();

		public virtual ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(DateTime dateTime) => new AbsoluteDateTimeExpirationCacheEntryExpirationPolicy(dateTime);

		public virtual ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null) => new AbsoluteDateTimeExpirationCacheEntryExpirationPolicy(timeSpan ?? Configuration.DefaultAbsoluteExpirationDuration);

		public virtual ICacheEntryExpirationPolicy GetSlidingTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null) => new SlidingTimeExpirationCacheEntryExpirationPolicy(timeSpan ?? Configuration.DefaultSlidingExpirationDuration);

		public virtual TimeSpan GetDefaultAbsoluteExpirationDuration() => Configuration.DefaultAbsoluteExpirationDuration;
		public virtual TimeSpan GetDefaultSlidingExpirationDuration() => Configuration.DefaultSlidingExpirationDuration;
	}
}
