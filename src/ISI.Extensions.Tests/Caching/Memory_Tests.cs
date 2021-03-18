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
	public class Memory_Tests
	{
		protected Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
		protected System.IServiceProvider ServiceProvider { get; set; }
		protected ISI.Extensions.Caching.ICacheManager CacheManager { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			Configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(Configuration)
				.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache>(provider => new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()))
				.AddSingleton<ISI.Extensions.Caching.ICacheManager, ISI.Extensions.Caching.CacheManager<Microsoft.Extensions.Caching.Memory.IMemoryCache>>();

			Configuration.AddAllConfigurations(services);

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(Configuration);

			CacheManager = ServiceProvider.GetService<ISI.Extensions.Caching.ICacheManager>();
		}

		[Test]
		public void Simple_Test()
		{
			var key = "HelloWorldKey";
			var value = "Hello World";

			CacheManager.Set(key, value);

			if (CacheManager.TryGetValue(key, out var cachedValue))
			{
				Console.WriteLine(cachedValue);
			}
		}

		[Test]
		public void GetOrCreate_keys_getCacheKey_getItem_getDefaultValue_getCacheEntryExpirationPolicy_forceRefreshCache_Test()
		{
			Func<string, string> getCacheKey = key => string.Format("CacheKey:{0}", key);
			Func<string, string> getItem = key => string.Format("Item:{0}", key);
			Func<string, string> getDefaultValue = key => string.Format("Default-Item:{0}", key);

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

			var items = CacheManager.GetOrCreate(keys, getCacheKey, getItemCheckCachedKeys, getDefaultValue);

			foreach (var item in items)
			{
				Console.WriteLine("{0} => {1}", item.Key, item.Value);
			}
		}

		[Test]
		public void GetOrCreate_keys_getCacheKey_getItems_getDefaultValues_getCacheEntryExpirationPolicy_forceRefreshCache_Test()
		{
			Func<string, string> getCacheKey = key => string.Format("CacheKey:{0}", key);
			Func<string, string> getItem = key => string.Format("Item:{0}", key);
			Func<string, string> getDefaultValue = key => string.Format("Default-Item:{0}", key);

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

			Func<IEnumerable<string>, IDictionary<string, string>> getItems = keys => keys.ToDictionary(key => key, getItemCheckCachedKeys);
			Func<IEnumerable<string>, IDictionary<string, string>> getDefaultValues = keys => keys.ToDictionary(key => key, getDefaultValue);

			var items = CacheManager.GetOrCreate(keys, getCacheKey, getItems, getDefaultValues);

			foreach (var item in items)
			{
				Console.WriteLine("{0} => {1}", item.Key, item.Value);
			}
		}

		[Test]
		public void Cache_Tagged_Objects_Tests()
		{
			Func<Guid, string> getCacheKey = key => string.Format("CacheKey:{0:D}", key);

			var testObjects = new List<TestObject>();
			for (int i = 0; i < 20; i++)
			{
				var testObjectUuid = Guid.NewGuid();

				testObjects.Add(new TestObject()
				{
					TestObjectUuid = testObjectUuid,
					Description = string.Format("Description for {0:D}", testObjectUuid),
					CacheKey = getCacheKey(testObjectUuid),
					CacheAbsoluteDateTimeExpiration = DateTime.UtcNow.AddMinutes(5),
				});
			}

			CacheManager.AddRange(testObjects);

			foreach (var testObject in testObjects)
			{
				if (CacheManager.TryGetValue<TestObject>(getCacheKey(testObject.TestObjectUuid), out var cachedTestObject))
				{
					System.Console.WriteLine("{0:D} => {1}", cachedTestObject.TestObjectUuid, cachedTestObject.Description);
				}
			}
		}



		public class TestObject : ISI.Extensions.Caching.IHasCacheKeyWithAbsoluteTimeExpiration
		{
			public Guid TestObjectUuid { get; set; }
			public string Description { get; set; }

			public string CacheKey { get; set; }
			public DateTime CacheAbsoluteDateTimeExpiration { get; set; }
		}
	}
}
