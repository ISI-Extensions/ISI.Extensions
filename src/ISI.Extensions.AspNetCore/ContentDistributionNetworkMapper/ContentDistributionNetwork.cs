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
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore
{
	public class ContentDistributionNetwork
	{
		private static IContentDistributionNetworkMapper _current = null;
		private static IContentDistributionNetworkMapper Current=> _current ??= ISI.Extensions.ServiceLocator.Current.GetService<IContentDistributionNetworkMapper>(() => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
		{
			MapToType = typeof(DefaultContentDistributionNetworkMapper),
			ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
		}); 

		private static Configuration _configuration = null;
		private static Configuration Configuration=> _configuration ??= ISI.Extensions.ServiceLocator.Current.GetService<Configuration>(() => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
		{
			MapToType = typeof(Configuration),
			ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
		}); 

		private static bool? _useContentDistributionNetwork = null;
		private static bool UseContentDistributionNetwork
		{
			get
			{
				if (!_useContentDistributionNetwork.HasValue)
				{
					switch (Configuration.ContentDistributionNetwork.UseContentDistributionNetwork)
					{
						case ISI.Extensions.Configuration.EnabledStatus.Default:
							_useContentDistributionNetwork = false;
							break;
						case ISI.Extensions.Configuration.EnabledStatus.Always:
							_useContentDistributionNetwork = true;
							break;
						case ISI.Extensions.Configuration.EnabledStatus.Never:
							_useContentDistributionNetwork = false;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					_useContentDistributionNetwork &= !string.IsNullOrWhiteSpace(Configuration.ContentDistributionNetwork.ReplacementRootUrl);

					_useContentDistributionNetwork &= !string.Equals(Configuration.ContentDistributionNetwork.ReplacementRootUrl, Configuration.ContentDistributionNetworkConfiguration.DefaultReplacementRootUrl);
				}

				return _useContentDistributionNetwork.GetValueOrDefault();
			}
		}

		public static string GetUrl(string url)
		{
			var result = url;

			if (UseContentDistributionNetwork && Current.CanRemap(url))
			{
				result = Current.GetCdnUrl(url);
			}

			return result;
		}

		public static string GetOriginalUrl(string url)
		{
			return Current.GetOriginalUrl(url);
		}
	}
}
