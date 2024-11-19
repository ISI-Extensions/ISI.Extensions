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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

//sudo service redis-server start

namespace ISI.Extensions.Caching.Redis
{
	public class RedisCacheManager : ISI.Extensions.Caching.ICacheManager
	{
		public Guid CacheManagerInstanceUuid { get; } = Guid.NewGuid();

		public event OnClearCache OnClearCache;

		private const string HmGetScript = (@"return redis.call('HMGET', KEYS[1], unpack(ARGV))");
		private const string AbsoluteExpirationKey = "absexp";
		private const string SlidingExpirationKey = "sldexp";
		private const string DataKey = "data";

		protected ISI.Extensions.Caching.Configuration Configuration { get; }
		protected ISI.Extensions.Caching.Redis.Configuration RedisConfiguration { get; }
		protected IRedisJsonSerializer JsonSerializer { get; }

		private static StackExchange.Redis.ConnectionMultiplexer _connection = null;
		protected StackExchange.Redis.ConnectionMultiplexer Connection => _connection ??= StackExchange.Redis.ConnectionMultiplexer.Connect(RedisConfiguration.ConnectionString);

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

		public RedisCacheManager(
			ISI.Extensions.Caching.Configuration configuration,
			ISI.Extensions.Caching.Redis.Configuration redisConfiguration,
			IRedisJsonSerializer jsonSerializer)
		{
			Configuration = configuration;
			RedisConfiguration = redisConfiguration;
			JsonSerializer = jsonSerializer;

			DefaultCacheEntryExpirationPolicyGetter = itemToCache =>
			{
				if (itemToCache is IHasCacheAbsoluteDateTimeExpiration absoluteTimeExpiration)
				{
					return GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(absoluteTimeExpiration.CacheAbsoluteDateTimeExpirationUtc);
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

		private StackExchange.Redis.IDatabase GetDatabase() => Connection.GetDatabase(RedisConfiguration.Database);

		public void Dispose() => _connection?.Dispose();

		public bool TryGetValue(string cacheKey, out object value) => TryGetValue(cacheKey, out value, true);

		protected bool TryGetValue(string cacheKey, out object value, bool checkCachedItemProxy)
		{
			var database = GetDatabase();

			var results = (StackExchange.Redis.RedisValue[])database.ScriptEvaluate(
				HmGetScript,
				[CacheKeyFormatter(cacheKey)],
				[(StackExchange.Redis.RedisValue)AbsoluteExpirationKey, (StackExchange.Redis.RedisValue)SlidingExpirationKey, (StackExchange.Redis.RedisValue)DataKey]);

			var serializedValue = results[2];

			if (serializedValue.HasValue)
			{
				var serializedParts = ((string)serializedValue).Split(['\n'], 2);
				var type = Type.GetType(serializedParts[0]);

				if (type.IsPrimitive || (type == typeof(string)))
				{
					value = Convert.ChangeType(serializedParts[1], type);
				}
				else
				{
					value = JsonSerializer.Deserialize(type, serializedParts[1]);
				}

				if (checkCachedItemProxy && (value is CachedItemProxy cachedItemProxy))
				{
					return TryGetValue(cachedItemProxy.CacheKey, out value);
				}

				return true;
			}

			value = null;

			return false;
		}

		public Microsoft.Extensions.Caching.Memory.ICacheEntry CreateEntry(string cacheKey) => new RedisCacheEntry(GetDatabase, JsonSerializer, CacheKeyFormatter(cacheKey));

		public void ClearCache()
		{
			OnClearCache?.Invoke(null);
			
			var endpoints = Connection.GetEndPoints();

			var server = Connection.GetServer(endpoints.First());

			server.FlushDatabase(RedisConfiguration.Database);
		}

		public void ClearCache(ClearCacheRequest request)
		{
			if (request.ClearAll)
			{
				ClearCache();
			}
			else
			{
				OnClearCache?.Invoke(request);
			
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

		public void Remove(string cacheKey)
		{
			var database = GetDatabase();

			if (TryGetValue(cacheKey, out var value, false))
			{
				if (value is CachedItemProxy cachedItemProxy)
				{
					Remove(cachedItemProxy.CacheKey);
				}
				else
				{
					database.KeyDelete(CacheKeyFormatter(cacheKey));
				}
			}

			var cachedItemProxiesCacheKey = CachedItemProxies.GetCachedItemProxiesCacheKey(cacheKey);
			if (TryGetValue(cachedItemProxiesCacheKey, out value, false))
			{
				if (value is CachedItemProxies cachedItemProxies)
				{
					foreach (var proxyCacheKey in cachedItemProxies.ProxyCacheKeys)
					{
						database.KeyDelete(CacheKeyFormatter(proxyCacheKey));
					}
				}

				database.KeyDelete(CacheKeyFormatter(cachedItemProxiesCacheKey));
			}
		}

		public void RemoveByCacheKeyPrefix(string cacheKeyPrefix)
		{
			var endpoints = Connection.GetEndPoints();

			var server = Connection.GetServer(endpoints.First());

			var cacheKeys = server.Keys();

			foreach (var cacheKey in cacheKeys.NullCheckedSelect(key => GetRawCacheKey(string.Format("{0}", key))))
			{
				if (cacheKey.StartsWith(cacheKeyPrefix))
				{
					Remove(cacheKey);
				}
			}
		}

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

		public void SetDefaultCacheEntryExpirationPolicyGetter(DefaultCacheEntryExpirationPolicyGetter defaultCacheEntryExpirationPolicyGetter) => DefaultCacheEntryExpirationPolicyGetter = defaultCacheEntryExpirationPolicyGetter;

		public ICacheEntryExpirationPolicy GetDefaultCacheEntryExpirationPolicy(object itemToCache) => DefaultCacheEntryExpirationPolicyGetter(itemToCache);

		public ICacheEntryExpirationPolicy GetNoExpirationCacheEntryExpirationPolicy() => new NoExpirationCacheEntryExpirationPolicy();

		public ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(DateTime dateTime) => new AbsoluteTimeExpirationCacheEntryExpirationPolicy(dateTime);

		public ICacheEntryExpirationPolicy GetAbsoluteTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null) => new AbsoluteTimeExpirationCacheEntryExpirationPolicy(timeSpan ?? Configuration.DefaultAbsoluteExpirationDuration);

		public ICacheEntryExpirationPolicy GetSlidingTimeExpirationCacheEntryExpirationPolicy(TimeSpan? timeSpan = null) => new SlidingTimeExpirationCacheEntryExpirationPolicy(timeSpan ?? Configuration.DefaultSlidingExpirationDuration);

		public virtual TimeSpan GetDefaultAbsoluteExpirationDuration() => Configuration.DefaultAbsoluteExpirationDuration;
		public virtual TimeSpan GetDefaultSlidingExpirationDuration() => Configuration.DefaultSlidingExpirationDuration;
	}
}
