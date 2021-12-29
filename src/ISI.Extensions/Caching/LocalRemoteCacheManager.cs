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
using System.Text;
using ISI.Extensions.Caching.Extensions;

namespace ISI.Extensions.Caching
{
	public class LocalRemoteCacheManager<TLocalCacheManager, TRemoteCacheManager> : ICacheManager
		where TLocalCacheManager : ICacheManager
		where TRemoteCacheManager : ICacheManager
	{
		public Guid CacheManagerInstanceUuid { get; } = Guid.NewGuid();
		
		protected ICacheManager LocalCacheManager { get; }
		protected ICacheManager RemoteCacheManager { get; }

		public LocalRemoteCacheManager(
			TLocalCacheManager localCacheManager,
			TRemoteCacheManager remoteCacheManager)
		{
			LocalCacheManager = localCacheManager;
			RemoteCacheManager = remoteCacheManager;
		}

		public void Dispose()
		{
			LocalCacheManager?.Dispose();
			RemoteCacheManager?.Dispose();
		}

		public bool TryGetValue(string cacheKey, out object value)
		{
			if(LocalCacheManager.TryGetValue(cacheKey, out value))
			{
				return true;
			}

			if(RemoteCacheManager.TryGetValue(cacheKey, out value))
			{
				LocalCacheManager.Add(cacheKey, value);
				return true;
			}

			return false;
		}

		public Microsoft.Extensions.Caching.Memory.ICacheEntry CreateEntry(string cacheKey) => new LocalRemoteCacheEntry(LocalCacheManager, RemoteCacheManager, cacheKey);

		public void ClearCache()
		{
			LocalCacheManager?.ClearCache();
			RemoteCacheManager?.ClearCache();
		}

		public void ClearCache(ClearCacheRequest clearCacheRequest)
		{
			LocalCacheManager?.ClearCache(clearCacheRequest);
			RemoteCacheManager?.ClearCache(clearCacheRequest);
		}

		public void Remove(string cacheKey)
		{
			foreach (var cacheManager in new[] {LocalCacheManager, RemoteCacheManager})
			{
				cacheManager.Remove(cacheKey);
			}
		}

		public void RemoveByCacheKeyPrefix(string cacheKeyPrefix)
		{
			LocalCacheManager?.RemoveByCacheKeyPrefix(cacheKeyPrefix);
			RemoteCacheManager?.RemoveByCacheKeyPrefix(cacheKeyPrefix);
		}

		public void Remove(IEnumerable<ClearCacheRequestCacheKeyWithCacheKeyInstanceUuid> cacheKeysWithCacheKeyInstanceUuid)
		{
			LocalCacheManager?.Remove(cacheKeysWithCacheKeyInstanceUuid);
			RemoteCacheManager?.Remove(cacheKeysWithCacheKeyInstanceUuid);
		}

		public void SetDefaultCacheEntryExpirationPolicyGetter(DefaultCacheEntryExpirationPolicyGetter defaultCacheEntryExpirationPolicyGetter) => LocalCacheManager.SetDefaultCacheEntryExpirationPolicyGetter(defaultCacheEntryExpirationPolicyGetter);

		public ICacheEntryExpirationPolicy GetDefaultCacheEntryExpirationPolicy(object itemToCache) => LocalCacheManager.GetDefaultCacheEntryExpirationPolicy(itemToCache);

		public ICacheEntryExpirationPolicy GetNoExpirationCacheEntryExpirationPolicy() => LocalCacheManager.GetNoExpirationCacheEntryExpirationPolicy();

		public ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(DateTime dateTime) => LocalCacheManager.GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(dateTime);

		public ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null) => LocalCacheManager.GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(timeSpan);

		public ICacheEntryExpirationPolicy GetSlidingTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null) => LocalCacheManager.GetSlidingTimeExpirationCacheEntryExpirationPolicy(timeSpan);

		public virtual TimeSpan GetDefaultAbsoluteExpirationDuration() => LocalCacheManager.GetDefaultAbsoluteExpirationDuration();
		public virtual TimeSpan GetDefaultSlidingExpirationDuration() => LocalCacheManager.GetDefaultSlidingExpirationDuration();
	}
}
