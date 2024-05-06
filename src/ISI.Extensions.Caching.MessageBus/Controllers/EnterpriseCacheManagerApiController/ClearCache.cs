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
using MESSAGEBUS = ISI.Caching.MessageBus.v1;

namespace ISI.Extensions.Caching.MessageBus.Controllers
{
	public partial class EnterpriseCacheManagerApiController
	{
		private static readonly System.Collections.Concurrent.ConcurrentQueue<ISI.Extensions.Caching.ClearCacheRequest> _clearCacheRequests = new();

		public void ClearCache(MESSAGEBUS.ClearCacheRequest request)
		{
			if (request.SourceCacheManagerInstanceUuid != CacheManager.CacheManagerInstanceUuid)
			{
				var clearCacheRequest = request.NullCheckedConvert(source => new ISI.Extensions.Caching.ClearCacheRequest()
				{
					CacheKeyScopes = new(source.CacheKeyScopes ?? []),
					ClearAll = source.ClearAll,
					CacheKeys = new(source.CacheKeys ?? []),
					CacheKeysWithCacheKeyInstanceUuid = new(source.CacheKeysWithCacheKeyInstanceUuid.ToNullCheckedArray(cacheKeyWithCacheKeyInstanceUuid => new ISI.Extensions.Caching.ClearCacheRequestCacheKeyWithCacheKeyInstanceUuid()
					{
						CacheKey = cacheKeyWithCacheKeyInstanceUuid.CacheKey,
						CacheKeyInstanceUuid = cacheKeyWithCacheKeyInstanceUuid.CacheKeyInstanceUuid,
					}) ?? [], new ISI.Extensions.Caching.ClearCacheRequestCacheKeyWithCacheKeyInstanceUuidEqualityComparer()),
					CacheKeyPrefixes = new(source.CacheKeyPrefixes ?? []),
				});

				if (Configuration.EnterpriseCacheManagerApi.EnqueueIncomingClearRequests)
				{
					_clearCacheRequests.Enqueue(clearCacheRequest);
				}
				else
				{
					ClearCache(clearCacheRequest);
				}
			}
		}

		public void ProcessClearCacheQueue()
		{
			//var doGarbageCollection = false;
			while (_clearCacheRequests.TryDequeue(out var clearCacheRequest))
			{
				//doGarbageCollection = true;
				ClearCache(clearCacheRequest);
			}

			//if (doGarbageCollection)
			//{
			//	GC.Collect();
			//}
		}

		private void ClearCache(ISI.Extensions.Caching.ClearCacheRequest clearCacheRequest)
		{
			var process = true;

			if (EnterpriseCacheClients.Any())
			{
				foreach (var enterpriseCacheClient in EnterpriseCacheClients)
				{
					if (clearCacheRequest.HasValue())
					{
						enterpriseCacheClient.ClearCache(clearCacheRequest);
					}
				}
			}

			if (clearCacheRequest.HasValue())
			{
				if (clearCacheRequest.CacheKeyScopes.NullCheckedAny())
				{
					process = clearCacheRequest.CacheKeyScopes.Any(cacheKeyScope => CacheKeyScopes.Contains(cacheKeyScope));
				}

				if (process)
				{
					CacheManager.ClearCache(clearCacheRequest);
				}
			}
		}
	}
}

