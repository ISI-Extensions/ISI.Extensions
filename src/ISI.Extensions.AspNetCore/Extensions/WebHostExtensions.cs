#region Copyright & License
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static class WebHostExtensions
	{
		public static void RunWithShutdownForAppOfflineHtm(this Microsoft.AspNetCore.Hosting.IWebHost host)
		{
			host.RunWithShutdownForAppOfflineHtm(default(System.Threading.CancellationToken), TimeSpan.FromMilliseconds(int.MaxValue));
		}

		public static void RunWithShutdownForAppOfflineHtm(this Microsoft.AspNetCore.Hosting.IWebHost host, System.Threading.CancellationToken cancellationToken)
		{
			host.RunWithShutdownForAppOfflineHtm(cancellationToken, TimeSpan.FromMilliseconds(int.MaxValue));
		}

		public static void RunWithShutdownForAppOfflineHtm(this Microsoft.AspNetCore.Hosting.IWebHost host, TimeSpan timeout)
		{
			host.RunWithShutdownForAppOfflineHtm(default(System.Threading.CancellationToken), (timeout.TotalMilliseconds > int.MaxValue ? TimeSpan.FromMilliseconds(int.MaxValue) : timeout));
		}

		public static void RunWithShutdownForAppOfflineHtm(this Microsoft.AspNetCore.Hosting.IWebHost host, System.Threading.CancellationToken cancellationToken, TimeSpan timeout)
		{
			var webHostCancellationTokenSource = new System.Threading.CancellationTokenSource(timeout);
			var webHostCancellationToken = webHostCancellationTokenSource.Token;

			cancellationToken.Register(() => webHostCancellationTokenSource.Cancel());

			var currentDirectory = System.IO.Directory.GetCurrentDirectory();

			var appOfflineFullName = System.IO.Path.Combine(currentDirectory, "app_offline.htm");

			var fileSystemWatcher = new System.IO.FileSystemWatcher(currentDirectory)
			{
				EnableRaisingEvents = true,
			};

			fileSystemWatcher.Changed += (o, fileSystemEvent) =>
			{
				if (string.Equals(fileSystemEvent.FullPath, appOfflineFullName, StringComparison.InvariantCultureIgnoreCase))
				{
					webHostCancellationTokenSource.Cancel();
				}
			};

			host.RunAsync(webHostCancellationToken).GetAwaiter().GetResult();
		}
	}
}
