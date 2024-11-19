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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using EnterpriseCacheManagerApiDTOs = ISI.Extensions.Caching.DataTransferObjects.EnterpriseCacheManagerApi;

namespace ISI.Extensions.Caching
{
	public class ClearCacheRequest
	{
		private HashSet<string> _cacheKeyScopes = null;
		public HashSet<string> CacheKeyScopes
		{
			get => _cacheKeyScopes ??= [];
			set => _cacheKeyScopes = [..value ?? []];
		}

		public bool ClearAll { get; set; } = false;

		private HashSet<string> _cacheKeys = null;
		public HashSet<string> CacheKeys
		{
			get => _cacheKeys ??= [];
			set => _cacheKeys = [..value ?? []];
		}

		private HashSet<ClearCacheRequestCacheKeyWithCacheKeyInstanceUuid> _cacheKeysWithCacheKeyInstanceUuid = null;
		public HashSet<ClearCacheRequestCacheKeyWithCacheKeyInstanceUuid> CacheKeysWithCacheKeyInstanceUuid
		{
			get => _cacheKeysWithCacheKeyInstanceUuid ??= new(new ClearCacheRequestCacheKeyWithCacheKeyInstanceUuidEqualityComparer());
			set => _cacheKeysWithCacheKeyInstanceUuid = new(value ?? [], new ClearCacheRequestCacheKeyWithCacheKeyInstanceUuidEqualityComparer());
		}
		
		private HashSet<string> _cacheKeyPrefixes = null;
		public HashSet<string> CacheKeyPrefixes
		{
			get => _cacheKeyPrefixes ??= [];
			set => _cacheKeyPrefixes = [..value ?? []];
		}

		public ClearCacheRequest()
		{

		}
		internal ClearCacheRequest(string cacheKeyScope)
			: this(cacheKeyScope, null)
		{
		}
		internal ClearCacheRequest(EnterpriseCacheManagerApiDTOs.ClearCacheRequest item)
			: this(null, item)
		{
		}
		internal ClearCacheRequest(string cacheKeyScope, EnterpriseCacheManagerApiDTOs.ClearCacheRequest item)
		{
			if (!string.IsNullOrWhiteSpace(cacheKeyScope))
			{
				CacheKeyScopes.Add(cacheKeyScope);
			}

			if (item != null)
			{
				Import(item);
			}
		}

		internal void Import(EnterpriseCacheManagerApiDTOs.ClearCacheRequest item)
		{
			ClearAll |= item.ClearAll;

			CacheKeys.UnionWith(item.CacheKeys ?? Array.Empty<string>());

			if (item.ItemsWithCacheKey != null)
			{
				foreach (var itemWithCacheKey in item.ItemsWithCacheKey)
				{
					if (itemWithCacheKey is ISI.Extensions.Caching.IHasCacheKeyWithInstanceUuid itemWithCacheKeyAndInstanceUuid)
					{
						CacheKeysWithCacheKeyInstanceUuid.Add(new()
						{
							CacheKey = itemWithCacheKeyAndInstanceUuid.CacheKey,
							CacheKeyInstanceUuid = itemWithCacheKeyAndInstanceUuid.CacheKeyInstanceUuid,
						});
					}
					else
					{
						CacheKeys.Add(itemWithCacheKey.CacheKey);
					}
				}
			}

			CacheKeyPrefixes.UnionWith(item.CacheKeyPrefixes ?? Array.Empty<string>());
		}

		public void Clear()
		{
			CacheKeyScopes.Clear();
			ClearAll = false;
			CacheKeys.Clear();
			CacheKeysWithCacheKeyInstanceUuid.Clear();
			CacheKeyPrefixes.Clear();
		}

		public bool HasValue()
		{
			return ClearAll || CacheKeys.Any() || CacheKeysWithCacheKeyInstanceUuid.Any() || CacheKeyPrefixes.Any();
		}

		internal EnterpriseCacheManagerApiDTOs.ClearCacheRequest ToClearCacheRequest()
		{
			return new()
			{
				CacheKeyScopes = CacheKeyScopes,
				ClearAll = ClearAll,
				CacheKeys = CacheKeys,
				ItemsWithCacheKey = CacheKeysWithCacheKeyInstanceUuid,
				CacheKeyPrefixes = CacheKeyPrefixes,
			};
		}
	}
}
