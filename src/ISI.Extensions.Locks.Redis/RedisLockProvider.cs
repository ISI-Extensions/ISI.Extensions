﻿#region Copyright & License
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

namespace ISI.Extensions.Locks.Redis
{
	public class RedisLockProvider : ILockProvider
	{
		protected ISI.Extensions.Locks.Redis.Configuration RedisConfiguration { get; }

		private static StackExchange.Redis.ConnectionMultiplexer _connection = null;
		protected StackExchange.Redis.ConnectionMultiplexer Connection => _connection ??= StackExchange.Redis.ConnectionMultiplexer.Connect(RedisConfiguration.ConnectionString);

		public RedisLockProvider(
			ISI.Extensions.Locks.Redis.Configuration redisConfiguration)
		{
			RedisConfiguration = redisConfiguration;
		}

		private StackExchange.Redis.IDatabase GetDatabase() => Connection.GetDatabase(RedisConfiguration.Database);

		public ILock GetLock(string key, TimeSpan lockTimeout, TimeSpan lockRetryInterval, TimeSpan lockRetryTimeout)
		{
			var autoResetEvent = new System.Threading.AutoResetEvent(false);

			var now = DateTime.UtcNow;

			var redisLock = new RedisLock(GetDatabase(), key);

			while (true)
			{
				if (redisLock.Database.LockTake(redisLock.Key, redisLock.Token, lockTimeout))
				{
					return redisLock;
				}

				if (DateTime.UtcNow > now + lockRetryTimeout)
				{
					throw new LockException(string.Format("Failed to get lock on \"{0}\"", key));
				}

				autoResetEvent.WaitOne(lockRetryInterval);
			}
		}
	}
}
