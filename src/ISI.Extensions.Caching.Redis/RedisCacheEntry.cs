#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Caching.Redis
{
	public class RedisCacheEntry : Microsoft.Extensions.Caching.Memory.ICacheEntry
	{
		// KEYS[1] = = key
		// ARGV[1] = absolute-expiration - ticks as long (-1 for none)
		// ARGV[2] = sliding-expiration - ticks as long (-1 for none)
		// ARGV[3] = relative-expiration (long, in seconds, -1 for none) - Min(absolute-expiration - Now, sliding-expiration)
		// ARGV[4] = data - byte[]
		// this order should not change LUA script depends on it
		private const string SetScript = (@"
								redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
								if ARGV[3] ~= '-1' then
									redis.call('EXPIRE', KEYS[1], ARGV[3])
								end
								return 1");
		private const long NotPresent = -1;

		protected Func<StackExchange.Redis.IDatabase> GetDatabase { get; }
		protected IRedisJsonSerializer JsonSerializer { get; }

		public object Key { get; }
		public object Value { get; set; }
		public DateTimeOffset? AbsoluteExpiration { get; set; }
		public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
		public TimeSpan? SlidingExpiration { get; set; }
		public IList<Microsoft.Extensions.Primitives.IChangeToken> ExpirationTokens { get; } = new List<Microsoft.Extensions.Primitives.IChangeToken>();
		public IList<Microsoft.Extensions.Caching.Memory.PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<Microsoft.Extensions.Caching.Memory.PostEvictionCallbackRegistration>();
		public Microsoft.Extensions.Caching.Memory.CacheItemPriority Priority { get; set; }
		public long? Size { get; set; }

		public RedisCacheEntry(
			Func<StackExchange.Redis.IDatabase> getDatabase,
			IRedisJsonSerializer jsonSerializer,
			string key)
		{
			GetDatabase = getDatabase;
			JsonSerializer = jsonSerializer;
			Key = key;
		}

		public void Dispose()
		{
			var type = Value.GetType();

			var serializedValue = $"{type.AssemblyQualifiedNameWithoutVersion()}\n{(type.IsPrimitive || (type == typeof(string)) ? $"{Value}" : JsonSerializer.Serialize(type, Value))}";

			var database = GetDatabase();

			database.ScriptEvaluate(SetScript, [(string)Key],
			[
				AbsoluteExpiration?.Ticks ?? NotPresent,
					SlidingExpiration?.Ticks ?? NotPresent,
					NotPresent, //GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
					serializedValue
			]);
		}
	}
}
