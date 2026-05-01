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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nginx.Extensions
{
	public static class NginxSettingsExtensions
	{
		private static IDictionary<string, NginxManagerServer> GetNginxManagerServersByDirectory(NginxApi nginxApi)
		{
			var nginxManagerServers = (nginxApi.GetNginxSettings(new())?.NginxSettings?.NginxManagerServers ?? []);

			var nginxManagerServersByDirectory = new Dictionary<string, NginxManagerServer>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var nginxManagerServer in nginxManagerServers)
			{
				foreach (var directory in nginxManagerServer.Directories ?? [])
				{
					nginxManagerServersByDirectory.Add(directory, nginxManagerServer);
				}
			}

			return nginxManagerServersByDirectory;
		}

		public static ISI.Extensions.Nginx.NginxManagerServer FindNginxManagerServerByDirectory(this NginxApi nginxApi, string directory, bool useClosestAncestryDirectory)
		{
			var nginxManagerServersByDirectory = GetNginxManagerServersByDirectory(nginxApi);

			nginxManagerServersByDirectory.TryGetValue(directory, out var nginxManagerServer);

			if (useClosestAncestryDirectory && (nginxManagerServer == null))
			{
				nginxManagerServer = nginxManagerServersByDirectory.Where(js => directory.StartsWith(js.Key, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(js => js.Key.Length).FirstOrDefault().Value;
			}

			return nginxManagerServer;
		}

		public static ISI.Extensions.Nginx.NginxManagerServer[] GetNginxManagerServers(this NginxApi nginxApi)
		{
			var nginxManagerServers = (nginxApi.GetNginxSettings(new())?.NginxSettings?.NginxManagerServers ?? []);

			return nginxManagerServers.OrderBy(nginxManagerServer => nginxManagerServer.Description, StringComparer.InvariantCultureIgnoreCase).ToArray();
		}

		public static ISI.Extensions.Nginx.NginxManagerServer GetNginxManagerServer(this NginxApi nginxApi, Guid nginxManagerServerUuid)
		{
			return (nginxApi.GetNginxSettings(new())?.NginxSettings?.NginxManagerServers ?? []).FirstOrDefault(nginxManagerServer => nginxManagerServer.NginxManagerServerUuid == nginxManagerServerUuid);
		}

		public static int GetMaxCheckDirectoryDepth(this NginxApi nginxApi)
		{
			return nginxApi.GetNginxSettings(new())?.NginxSettings?.MaxCheckDirectoryDepth ?? 5;
		}
	}
}
