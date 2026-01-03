#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Tests.Caching
{
	[TestFixture]
	public class CacheKey_Tests
	{
		protected Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
		protected System.IServiceProvider ServiceProvider { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			Configuration = builder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(Configuration)
				.AddSingleton<ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>();
			
			Configuration.AddAllConfigurations(services);

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(Configuration);
		}

		[Test]
		public void GetCacheKeyFormat_Test()
		{
			var key = ISI.Extensions.Caching.CacheKeyGenerators.GetCacheKeyFormat<CacheKeyProxyTestObject>(2);
		}

		public class CacheKeyProxyTestObject
		{
			public Guid TestObjectUuid { get; set; }
			public string Description { get; set; }
		}

	}
}
