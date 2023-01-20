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
using Microsoft.Extensions.Caching.Memory;

namespace ISI.Extensions.Caching
{
	public class LocalRemoteCacheEntry : Microsoft.Extensions.Caching.Memory.ICacheEntry
	{
		object Microsoft.Extensions.Caching.Memory.ICacheEntry.Key => CacheKey;

		public string CacheKey { get; }
		public object Value { get; set; }
		public DateTimeOffset? AbsoluteExpiration { get; set; }
		public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
		public TimeSpan? SlidingExpiration { get; set; }
		public IList<Microsoft.Extensions.Primitives.IChangeToken> ExpirationTokens { get; } = new List<Microsoft.Extensions.Primitives.IChangeToken>();
		public IList<Microsoft.Extensions.Caching.Memory.PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<Microsoft.Extensions.Caching.Memory.PostEvictionCallbackRegistration>();
		public Microsoft.Extensions.Caching.Memory.CacheItemPriority Priority { get; set; }
		public long? Size { get; set; }

		protected ICacheManager LocalCacheManager { get; }
		protected ICacheManager RemoteCacheManager { get; }

		public LocalRemoteCacheEntry(
			ICacheManager localCacheManager,
			ICacheManager remoteCacheManager,
			string cacheKey)
		{
			LocalCacheManager = localCacheManager;
			RemoteCacheManager = remoteCacheManager;
			CacheKey = cacheKey;
		}

		public void Dispose()
		{
			foreach (var cacheManager in new [] { LocalCacheManager, RemoteCacheManager })
			{
				using (var cacheEntry = cacheManager.CreateEntry(CacheKey))
				{
					cacheEntry.SetValue(Value);

					if(AbsoluteExpiration.HasValue)
					{
						cacheEntry.SetAbsoluteExpiration(AbsoluteExpiration.Value);
					}

					if(AbsoluteExpirationRelativeToNow.HasValue)
					{
						cacheEntry.SetAbsoluteExpiration(AbsoluteExpirationRelativeToNow.Value);
					}

					if(SlidingExpiration.HasValue)
					{
						cacheEntry.SetSlidingExpiration(SlidingExpiration.Value);
					}

					foreach (var expirationToken in ExpirationTokens)
					{
						cacheEntry.AddExpirationToken(expirationToken);
					}

					foreach (var postEvictionCallback in PostEvictionCallbacks)
					{
						cacheEntry.PostEvictionCallbacks.Add(postEvictionCallback);
					}

					cacheEntry.SetPriority(Priority);
				}
			}
		}
	}
}
