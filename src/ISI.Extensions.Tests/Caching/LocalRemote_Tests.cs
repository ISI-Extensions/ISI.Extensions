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
using ISI.Extensions.Caching.Extensions;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ISI.Extensions.Tests.Caching
{
	[TestFixture]
	public class LocalRemote_Tests
	{
		protected Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
		protected System.IServiceProvider ServiceProvider { get; set; }

		protected ISI.Extensions.Caching.ICacheManager CacheManager { get; set; }
		protected ISI.Extensions.Caching.ICacheManager MemoryCacheManager { get; set; }
		protected ISI.Extensions.Caching.ICacheManager RedisCacheManager { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			Configuration = builder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(Configuration)
				.AddSingleton<ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Caching.Redis.IRedisJsonSerializer, ISI.Extensions.Caching.Redis.RedisJsonSerializer<ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>>()

				.AddSingleton<Microsoft.Extensions.Caching.Memory.MemoryCache>(provider => new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()))
				.AddSingleton<ISI.Extensions.Caching.CacheManager<Microsoft.Extensions.Caching.Memory.MemoryCache>>()

				.AddSingleton<ISI.Extensions.Caching.Redis.RedisCacheManager>()

				.AddSingleton<ISI.Extensions.Caching.ICacheManager, ISI.Extensions.Caching.LocalRemoteCacheManager<ISI.Extensions.Caching.CacheManager<Microsoft.Extensions.Caching.Memory.MemoryCache>, ISI.Extensions.Caching.Redis.RedisCacheManager>>();

			Configuration.AddAllConfigurations(services);

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(Configuration);


			CacheManager = ServiceProvider.GetService<ISI.Extensions.Caching.ICacheManager>();
			MemoryCacheManager = ServiceProvider.GetService<ISI.Extensions.Caching.CacheManager<Microsoft.Extensions.Caching.Memory.MemoryCache>>();
			RedisCacheManager = ServiceProvider.GetService<ISI.Extensions.Caching.Redis.RedisCacheManager>();
		}

		[Test]
		public void Simple_Test()
		{
			var key = "HelloWorldKey";
			var value = "Hello World";

			using (var cacheEntry = CacheManager.CreateEntry(key))
			{
				cacheEntry.Value = value;
				//cacheEntry.SetAbsoluteExpiration()
			}

			//cache.Set(key, value);

			if (CacheManager.TryGetValue(key, out var cachedValue))
			{
				Console.WriteLine(cachedValue);
			}
		}

		[Test]
		public void Remote_Test()
		{
			var key = "HelloWorldKeyRemote";
			var value = "Hello World Remote";

			using (var cacheEntry = RedisCacheManager.CreateEntry(key))
			{
				cacheEntry.Value = value;
				//cacheEntry.SetAbsoluteExpiration()
			}

			//cache.Set(key, value);

			{
				if (MemoryCacheManager.TryGetValue(key, out var cachedValue))
				{
					Console.WriteLine(cachedValue);
				}
			}

			{
				if (CacheManager.TryGetValue(key, out var cachedValue))
				{
					Console.WriteLine(cachedValue);
				}
			}

			{
				if (MemoryCacheManager.TryGetValue(key, out var cachedValue))
				{
					Console.WriteLine(cachedValue);
				}
			}
		}

		[Test]
		public void GetOrCreate_keys_getCacheKey_getItem_getDefaultValue_getCacheEntryExpirationPolicy_forceRefreshCache_Test()
		{
			ISI.Extensions.Caching.GenerateCacheKey<string> getCacheKey = key => string.Format("CacheKey:{0}", key);
			ISI.Extensions.Caching.GetItem<string, string> getItem = key => string.Format("Item:{0}", key);
			ISI.Extensions.Caching.GetItem<string, string> getDefaultValue = key => string.Format("Default-Item:{0}", key);

			var cachedKeys = new HashSet<string>();
			for (int i = 20; i < 40; i++)
			{
				var key = string.Format("{0}", i);

				CacheManager.Add(getCacheKey(key), getItem(key));

				cachedKeys.Add(key);
			}

			var keys = new HashSet<string>();
			for (int i = 0; i < 60; i++)
			{
				var key = string.Format("{0}", i);

				keys.Add(key);
			}

			ISI.Extensions.Caching.GetItem<string, string> getItemCheckCachedKeys = key =>
			{
				if (cachedKeys.Contains(key))
				{
					throw new Exception("Should not have asked for this item");
				}

				if (int.TryParse(key, out var value) && (value <= 50))
				{
					return string.Format("Got-Item:{0}", key);
				}

				return null;
			};

			var items = CacheManager.GetOrCreate(
				keys,
				getCacheKey,
				neededCacheKeys => neededCacheKeys.ToDictionary(key => key, key => getItemCheckCachedKeys(key)),
				neededCacheKeys => neededCacheKeys.ToDictionary(key => key, key => getDefaultValue(key)));

			foreach (var item in items)
			{
				Console.WriteLine("{0} => {1}", item.Key, item.Value);
			}
		}

		[Test]
		public void GetOrCreate_keys_getCacheKey_getItems_getDefaultValues_getCacheEntryExpirationPolicy_forceRefreshCache_Test()
		{
			ISI.Extensions.Caching.GenerateCacheKey<string> getCacheKey = key => string.Format("CacheKey:{0}", key);
			ISI.Extensions.Caching.GetItem<string, string> getItem = key => string.Format("Item:{0}", key);
			ISI.Extensions.Caching.GetItem<string, string> getDefaultValue = key => string.Format("Default-Item:{0}", key);

			var cachedKeys = new HashSet<string>();
			for (int i = 20; i < 40; i++)
			{
				var key = string.Format("{0}", i);

				CacheManager.Add(getCacheKey(key), getItem(key));

				cachedKeys.Add(key);
			}

			var keys = new HashSet<string>();
			for (int i = 0; i < 10; i++)
			{
				var key = string.Format("{0}", i);

				keys.Add(key);
			}

			Func<string, string> getItemCheckCachedKeys = key =>
			{
				if (cachedKeys.Contains(key))
				{
					throw new Exception("Should not have asked for this item");
				}

				if (int.TryParse(key, out var value) && (value <= 50))
				{
					return string.Format("Got-Item:{0}", key);
				}

				return null;
			};

			ISI.Extensions.Caching.GetItems<string, string> getItems = keys => keys.ToDictionary(key => key, getItemCheckCachedKeys);
			ISI.Extensions.Caching.GetItems<string, string> getDefaultValues = keys => keys.ToDictionary(key => key, key => getDefaultValue(key));

			var items = CacheManager.GetOrCreate(keys, getCacheKey, getItems, getDefaultValues);

			foreach (var item in items)
			{
				Console.WriteLine("{0} => {1}", item.Key, item.Value);
			}
		}
	}
}
