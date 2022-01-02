﻿#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Locks
{
	public class LockFactory : ILockFactory
	{
		protected Configuration Configuration { get; }
		protected System.IServiceProvider ServiceProvider { get; }

		public LockFactory(
			Configuration configuration,
			System.IServiceProvider serviceProvider)
		{
			Configuration = configuration;
			ServiceProvider = serviceProvider;
		}

		public ILock GetLock(string key, TimeSpan? lockTimeout = null, TimeSpan? lockRetryInterval = null, TimeSpan? lockRetryTimeout = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new NullReferenceException("key cannot be null or empty");
			}

			var lockProvider = ServiceProvider.GetService<ILockProvider>();

			return lockProvider.GetLock(key, lockTimeout?? Configuration.DefaultLockTimeout, lockRetryInterval ?? Configuration.DefaultRetryInterval, lockRetryTimeout ?? Configuration.DefaultRetryTimeout);
		}

		public void Lock(string key, Action action, TimeSpan? lockTimeout = null, TimeSpan? lockRetryInterval = null, TimeSpan? lockRetryTimeout = null)
		{
			if (action == null)
			{
				throw new NullReferenceException("action cannot be null");
			}

			using (GetLock(key, lockTimeout, lockRetryInterval, lockRetryTimeout))
			{
				action();
			}
		}

		public ILock GetDistributedLock(string key, TimeSpan? lockTimeout = null, TimeSpan? lockRetryInterval = null, TimeSpan? lockRetryTimeout = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new NullReferenceException("key cannot be null or empty");
			}

			var lockProvider = ServiceProvider.GetService<ILockDistributedProvider>();

			return lockProvider.GetLock(key, lockTimeout?? Configuration.DefaultLockTimeout, lockRetryInterval ?? Configuration.DefaultRetryInterval, lockRetryTimeout ?? Configuration.DefaultRetryTimeout);
		}

		public void DistributedLock(string key, Action action, TimeSpan? lockTimeout = null, TimeSpan? lockRetryInterval = null, TimeSpan? lockRetryTimeout = null)
		{
			if (action == null)
			{
				throw new NullReferenceException("action cannot be null");
			}

			using (GetDistributedLock(key, lockTimeout, lockRetryInterval, lockRetryTimeout))
			{
				action();
			}
		}
	}
}
