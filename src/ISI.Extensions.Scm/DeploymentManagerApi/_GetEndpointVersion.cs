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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.DeploymentManagerApi;

namespace ISI.Extensions.Scm
{
	public partial class DeploymentManagerApi
	{
		private static readonly object _endPointVersionsLock = new();
		private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, decimal> _endPointVersions = new();

		private decimal GetEndpointVersion(string servicesManagerUrl)
		{
			if (!_endPointVersions.TryGetValue(servicesManagerUrl, out var endPointVersion))
			{
				lock (_endPointVersionsLock)
				{
					if (!_endPointVersions.TryGetValue(servicesManagerUrl, out endPointVersion))
					{
						endPointVersion = 1;

						try
						{
							var uri = new UriBuilder(servicesManagerUrl);
							uri.SetPathAndQueryString(string.Empty);

							var restResponse = ISI.Extensions.WebClient.Rest.ExecuteGet<ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, new(), false);

							var version = (string)null;
							if((restResponse?.ResponseHeaders?.TryGetValue(HeaderKey.ServicesManagerVersion, out version)).GetValueOrDefault())
							{
								var servicesManagerVersion = new Version(version);

								endPointVersion = (decimal)servicesManagerVersion.Major + (((decimal)servicesManagerVersion.Minor) * (decimal).1);
							}
						}
						catch
						{
						}

						_endPointVersions.TryAdd(servicesManagerUrl, endPointVersion);
					}
				}
			}
			
			return endPointVersion;
		}
	}
}