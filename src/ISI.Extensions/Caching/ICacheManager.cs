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
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.Caching
{
	public delegate string GenerateCacheKey<TKey>(TKey key);

	public delegate string[] GenerateCacheKeys<TItem>(TItem item);

	public delegate TItem GetItem<TItem>();
	public delegate Task<TItem> GetItemAsync<TItem>();

	public delegate TItem GetItem<TKey, TItem>(TKey key);
	public delegate Task<TItem> GetItemAsync<TKey, TItem>(TKey key);

	public delegate IDictionary<TKey, TItem> GetItems<TKey, TItem>(IEnumerable<TKey> keys);
	public delegate Task<IDictionary<TKey, TItem>> GetItemsAsync<TKey, TItem>(IEnumerable<TKey> keys);

	public delegate ICacheEntryExpirationPolicy DefaultCacheEntryExpirationPolicyGetter(object itemToCache);

	public interface ICacheManager : IDisposable
	{
		Guid CacheManagerInstanceUuid { get; }
		
		bool TryGetValue(string cacheKey, out object value);
		Microsoft.Extensions.Caching.Memory.ICacheEntry CreateEntry(string key);
		
		void ClearCache();
		void ClearCache(ISI.Extensions.Caching.ClearCacheRequest clearCacheRequest);
		void Remove(string key);
		void RemoveByCacheKeyPrefix(string cacheKeyPrefix);
		void Remove(IEnumerable<ClearCacheRequestCacheKeyWithCacheKeyInstanceUuid> cacheKeysWithCacheKeyInstanceUuid);

		void SetDefaultCacheEntryExpirationPolicyGetter(DefaultCacheEntryExpirationPolicyGetter defaultCacheEntryExpirationPolicyGetter);
		ISI.Extensions.Caching.ICacheEntryExpirationPolicy GetDefaultCacheEntryExpirationPolicy(object itemToCache);
		ISI.Extensions.Caching.ICacheEntryExpirationPolicy GetNoExpirationCacheEntryExpirationPolicy();
		ISI.Extensions.Caching.ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(DateTime dateTime);
		ISI.Extensions.Caching.ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null);
		ISI.Extensions.Caching.ICacheEntryExpirationPolicy GetSlidingTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null);

		TimeSpan GetDefaultAbsoluteExpirationDuration();
		TimeSpan GetDefaultSlidingExpirationDuration();
	}
}
