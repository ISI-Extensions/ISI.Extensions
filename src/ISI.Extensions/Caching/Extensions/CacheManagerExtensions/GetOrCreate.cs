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
		public static IDictionary<TKey, TItem> GetOrCreate<TKey, TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, IEnumerable<TKey> keys, Func<TKey, string> getCacheKey, Func<TKey, TItem> getItem, Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getDefaultValues, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null, bool forceRefreshCache = false)
		{
			Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getItems = itemKeys => { return itemKeys.ToDictionary(itemKey => itemKey, getItem).Where(keyValue => keyValue.Value != null).ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value); };

			return GetOrCreate(cacheManager, keys, getCacheKey, getItems, getDefaultValues, getCacheEntryExpirationPolicy, forceRefreshCache);
		}

		public static IDictionary<TKey, TItem> GetOrCreate<TKey, TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, IEnumerable<TKey> keys, Func<TKey, string> getCacheKey, Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getItems, Func<TKey, TItem> getDefaultValue, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null, bool forceRefreshCache = false)
		{
			Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getDefaultValues = itemKeys => { return itemKeys.ToDictionary(itemKey => itemKey, getDefaultValue).Where(keyValue => keyValue.Value != null).ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value); };

			return GetOrCreate(cacheManager, keys, getCacheKey, getItems, getDefaultValues, getCacheEntryExpirationPolicy, forceRefreshCache);
		}

		public static IDictionary<TKey, TItem> GetOrCreate<TKey, TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, IEnumerable<TKey> keys, Func<TKey, string> getCacheKey, Func<TKey, TItem> getItem, Func<TKey, TItem> getDefaultValue, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null, bool forceRefreshCache = false)
		{
			Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getItems = itemKeys => { return itemKeys.ToDictionary(itemKey => itemKey, getItem).Where(keyValue => keyValue.Value != null).ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value); };

			Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getDefaultValues = itemKeys => { return itemKeys.ToDictionary(itemKey => itemKey, getDefaultValue).Where(keyValue => keyValue.Value != null).ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value); };

			return GetOrCreate(cacheManager, keys, getCacheKey, getItems, getDefaultValues, getCacheEntryExpirationPolicy, forceRefreshCache);
		}

		public static IDictionary<TKey, TItem> GetOrCreate<TKey, TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, IEnumerable<TKey> keys, Func<TKey, string> getCacheKey, Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getItems, Func<IEnumerable<TKey>, IDictionary<TKey, TItem>> getDefaultValues, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null, bool forceRefreshCache = false)
		{
			var result = new Dictionary<TKey, TItem>();

			var neededFormattedCacheKeys = keys.Distinct().ToDictionary(key => getCacheKey(key), key => key);

			#region Get from Cache
			if (!forceRefreshCache)
			{
				var foundFormattedCacheKeys = new List<string>();

				foreach (var neededFormattedCacheKey in neededFormattedCacheKeys)
				{
					if (cacheManager.TryGetValue<TItem>(neededFormattedCacheKey.Key, out var item))
					{
						result.Add(neededFormattedCacheKey.Value, item);
						foundFormattedCacheKeys.Add(neededFormattedCacheKey.Key);
					}
				}

				foreach (var foundFormattedCacheKey in foundFormattedCacheKeys)
				{
					neededFormattedCacheKeys.Remove(foundFormattedCacheKey);
				}
			}
			#endregion

			#region Use getItems
			if (neededFormattedCacheKeys.Any())
			{
				var foundFormattedCacheKeys = new List<string>();

				var items = getItems(neededFormattedCacheKeys.Select(neededKey => neededKey.Value));

				foreach (var item in items.Where(i => i.Value != null))
				{
					result.Add(item.Key, item.Value);

					var formattedCacheKey = getCacheKey(item.Key);
					Add(cacheManager, formattedCacheKey, item.Value, getCacheEntryExpirationPolicy);
					foundFormattedCacheKeys.Add(formattedCacheKey);
				}

				foreach (var foundFormattedCacheKey in foundFormattedCacheKeys)
				{
					neededFormattedCacheKeys.Remove(foundFormattedCacheKey);
				}
			}
			#endregion

			#region Use getDefaultValues
			if (neededFormattedCacheKeys.Any() && (getDefaultValues != null))
			{
				var items = getDefaultValues(neededFormattedCacheKeys.Select(neededFormattedCacheKey => neededFormattedCacheKey.Value));

				foreach (var item in items)
				{
					result.Add(item.Key, item.Value);
					Add(cacheManager, getCacheKey(item.Key), item.Value, getCacheEntryExpirationPolicy);
				}
			}
			#endregion

			return result;
		}

		public static TItem GetOrCreate<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, Func<string> getCacheKey, Func<TItem> getItem = null, Func<TItem> getDefaultValue = null, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null, bool forceRefreshCache = false)
		{
			return GetOrCreate(cacheManager, getCacheKey(), getItem, getDefaultValue, getCacheEntryExpirationPolicy, forceRefreshCache);
		}

		public static TItem GetOrCreate<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, string cacheKey, Func<TItem> getItem = null, Func<TItem> getDefaultValue = null, Func<ISI.Extensions.Caching.ICacheEntryExpirationPolicy> getCacheEntryExpirationPolicy = null, bool forceRefreshCache = false)
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

			return value;
		}

		public static TItem GetOrCreate<TItem>(this ISI.Extensions.Caching.ICacheManager cacheManager, string cacheKey, Func<ICacheEntry, TItem> getItem)
		{

			if (!cacheManager.TryGetValue(cacheKey, out var cachedItem))
			{
				using (var entry = cacheManager.CreateEntry(cacheKey))
				{
					cachedItem = getItem(entry);

					entry.SetValue(cachedItem);
				}
			}
			return (TItem)cachedItem;
		}

	}
}