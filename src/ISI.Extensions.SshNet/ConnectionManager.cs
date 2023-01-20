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
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.SshNet
{
	public class ConnectionManager
	{
		private static ISI.Extensions.SecureShell.IHostConfigurationManager _hostConfigurationManager = null;
		private static ISI.Extensions.SecureShell.IHostConfigurationManager HostConfigurationManager => _hostConfigurationManager ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.SecureShell.IHostConfigurationManager>();

		public static Renci.SshNet.ConnectionInfo GetConnectionInfo(string server, string userName, string password)
		{
			var port = 22;

			var pieces = server.Split(':');

			server = pieces.First();

			if (pieces.Length > 1)
			{
				port = pieces[1].ToInt();
			}

			var hostConfiguration = HostConfigurationManager.GetHostConfiguration(server, port, userName);

			if (!string.IsNullOrEmpty(hostConfiguration?.PrivateKey))
			{
				using (var privateKeyStream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(hostConfiguration?.PrivateKey)))
				{
					if (string.IsNullOrEmpty(password))
					{
						return new(server, port, userName, new Renci.SshNet.PrivateKeyAuthenticationMethod(userName, new Renci.SshNet.PrivateKeyFile(privateKeyStream)));
					}

					return new(server, port, userName, new Renci.SshNet.PrivateKeyAuthenticationMethod(userName, new Renci.SshNet.PrivateKeyFile(privateKeyStream, password)));
				}
			}

			return new(server, port, userName, new Renci.SshNet.PasswordAuthenticationMethod(userName, password));
		}
	}
}
