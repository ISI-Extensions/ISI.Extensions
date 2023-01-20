#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.ConfigurationHelper.Extensions
{
	public static partial class ConfigurationBuilderExtensions
	{
		public delegate string GetClassicConnectionStringsSectionFilePath(string environment);

		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFiles(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, string[] environments, GetClassicConnectionStringsSectionFilePath getPath)
		{
			return AddClassicConnectionStringsSectionFiles(configurationBuilder, null, environments, getPath, false);
		}
		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFiles(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, Microsoft.Extensions.FileProviders.IFileProvider provider, string[] environments, GetClassicConnectionStringsSectionFilePath getPath)
		{
			return AddClassicConnectionStringsSectionFiles(configurationBuilder, provider, environments, getPath, false);
		}
		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFiles(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, string[] environments, GetClassicConnectionStringsSectionFilePath getPath, bool reloadOnChange)
		{
			return AddClassicConnectionStringsSectionFiles(configurationBuilder, null, environments, getPath, false);
		}
		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFiles(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, Microsoft.Extensions.FileProviders.IFileProvider provider, string[] environments, GetClassicConnectionStringsSectionFilePath getPath, bool reloadOnChange)
		{
			var index = environments.Length;

			while (index-- > 0)
			{
				configurationBuilder.AddClassicConnectionStringsSectionFile(provider, getPath(environments[index]), true, reloadOnChange);
			}

			return configurationBuilder;
		}

		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFile(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, string path)
		{
			return configurationBuilder.AddClassicConnectionStringsSectionFile(null, path, false, false);
		}

		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFile(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, string path, bool optional)
		{
			return configurationBuilder.AddClassicConnectionStringsSectionFile(null, path, optional, false);
		}

		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFile(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, Microsoft.Extensions.FileProviders.IFileProvider provider, string path, bool optional, bool reloadOnChange)
		{
			if (configurationBuilder == null)
			{
				throw new ArgumentNullException(nameof(configurationBuilder));
			}

			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("InvalidFilePath: \"{0}\"", nameof(path));
			}

			return configurationBuilder.AddClassicConnectionStringsSectionFile(source =>
			{
				source.FileProvider = provider;
				source.Path = path;
				source.Optional = optional;
				source.ReloadOnChange = reloadOnChange;
				source.ResolveFileProvider();
			});
		}

		public static Microsoft.Extensions.Configuration.IConfigurationBuilder AddClassicConnectionStringsSectionFile(this Microsoft.Extensions.Configuration.IConfigurationBuilder builder, Action<ClassicConnectionStringsSectionConfigurationSource> configureSource)
		{
			return builder.Add(configureSource);
		}
	}
}