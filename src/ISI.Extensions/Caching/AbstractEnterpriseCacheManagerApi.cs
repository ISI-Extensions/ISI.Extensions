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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Caching.DataTransferObjects.EnterpriseCacheManagerApi;

namespace ISI.Extensions.Caching
{
	public abstract class AbstractEnterpriseCacheManagerApi : IEnterpriseCacheManagerApi
	{
		[ThreadStatic]
		public static ClearCacheLock ThreadClearCacheLock = null;

		public static ClearCacheLock ClearCacheLock = null;


		public DTOs.ClearCacheResponse ClearCache(DTOs.ClearCacheRequest request)
		{
			var response = new DTOs.ClearCacheResponse();

			if (request.ItemsWithCacheKey.NullCheckedAny())
			{
				var itemsWithCacheKey = request.ItemsWithCacheKey.ToList();

				for (int index = 0; index < itemsWithCacheKey.Count; index++)
				{
					if (itemsWithCacheKey[index] is ISI.Extensions.Caching.IHasCacheKeyCollection cacheKeys)
					{
						itemsWithCacheKey.AddRange(cacheKeys.CacheableItems);
					}
				}

				request.ItemsWithCacheKey = itemsWithCacheKey;
			}

			if (ClearCacheLock != null)
			{
				response.ClearCacheLockFilterResponse = ClearCacheLock.CheckFilter(request);
			}
			else if (ThreadClearCacheLock != null)
			{
				response.ClearCacheLockFilterResponse = ThreadClearCacheLock.CheckFilter(request);
			}

			if (response.ClearCacheLockFilterResponse == DTOs.ClearCacheLockFilterResponse.ProcessNow)
			{
				response = ProcessClearCache(new ClearCacheRequest(request.CacheKeyScopes.NullCheckedFirstOrDefault(), request));
			}

			return response;
		}

		protected abstract DTOs.ClearCacheResponse ProcessClearCache(ClearCacheRequest request);

		public ClearCacheLock GetClearCacheLock(ClearCacheLockFilter clearCacheFilter)
		{
			if ((ThreadClearCacheLock ?? ClearCacheLock) != null)
			{
				throw new Exception("There can only be one ClearCacheLock at a time");
			}

			return (ClearCacheLock = new ClearCacheLock(this, () => ClearCacheLock = null, clearCacheFilter));
		}
		public ClearCacheLock GetClearCacheLock(DTOs.ClearCacheLockFilterResponse clearCacheFilter)
		{
			return GetClearCacheLock(item => clearCacheFilter);
		}
		public ClearCacheLock GetClearCacheLock()
		{
			return GetClearCacheLock(item => DTOs.ClearCacheLockFilterResponse.Ignore);
		}

		public ClearCacheLock GetClearCacheThreadLock(ClearCacheLockFilter clearCacheFilter)
		{
			if ((ThreadClearCacheLock ?? ClearCacheLock) != null)
			{
				throw new Exception("There can only be one ClearCacheLock at a time");
			}

			return (ThreadClearCacheLock = new ClearCacheLock(this, () => ClearCacheLock = null, clearCacheFilter));
		}
		public ClearCacheLock GetClearCacheThreadLock(DTOs.ClearCacheLockFilterResponse clearCacheFilter)
		{
			return GetClearCacheThreadLock(item => clearCacheFilter);
		}
		public ClearCacheLock GetClearCacheThreadLock()
		{
			return GetClearCacheThreadLock(item => DTOs.ClearCacheLockFilterResponse.Ignore);
		}
	}
}
