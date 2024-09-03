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
 
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class Configuration_Tests
	{
		[Test]
		public void ApplyConfigurationValueReaders_Test()
		{
			System.Environment.SetEnvironmentVariable("ENV_STRING_ARRAY:1", "RonWasHere");

			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();

			var configurationsPath = string.Format("Configuration{0}", System.IO.Path.DirectorySeparatorChar);

			var activeEnvironment = configurationBuilder.GetActiveEnvironmentConfiguration($"{configurationsPath}isi.extensions.environmentsConfig.json");

			//configurationBuilder.SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
			configurationBuilder.AddJsonFile("appsettings.json", optional: false);
			configurationBuilder.AddJsonFiles(activeEnvironment.ActiveEnvironments, environment => $"appsettings.{environment}.json");
			//configurationBuilder.AddDataPathJsonFile(System.IO.Path.Combine("ISI.Extensions.Tests", "appsettings.json"));
			
			configurationBuilder.AddEnvironmentConfiguration(false);

			var configurationRoot = configurationBuilder.Build().ApplyConfigurationValueReaders();

			var configurationTest = configurationRoot.GetConfiguration<ISI.Extensions.Tests.Configuration>();

			foreach (var keyValuePair in configurationRoot.AsEnumerable().OrderBy(keyValuePair => keyValuePair.Key, StringComparer.InvariantCultureIgnoreCase))
			{
				System.Console.WriteLine($"  Config \"{keyValuePair.Key}\" => \"{keyValuePair.Value}\"");
			}
		}

		[Test]
		public void NginxFileConfigurationValueReader_Test()
		{
			var valueReader = new ISI.Extensions.ConfigurationValueReaders.NginxFileConfigurationValueReader();

			var xxx = valueReader.GetValue(new ISI.Extensions.ConfigurationValueReaders.ParsedValue(@"nginx:E:\Data\Nginx\sites-enabled\qa-rewardhub.xxxx.com.conf[http://qa-rewardhub.xxxx.com:14080]"));
			var yyy = valueReader.GetValue(new ISI.Extensions.ConfigurationValueReaders.ParsedValue(@"nginx:E:\Data\Nginx\sites-enabled\qa-rewardhub.xxx.com.conf[http://qa-rewardhub.xxx.com:14080/uuuuu]"));
		}
	}
}