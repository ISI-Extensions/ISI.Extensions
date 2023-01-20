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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.ScmApi;

namespace ISI.Extensions.Scm
{
	public partial class ScmApi
	{
		public DTOs.GetAuthenticationTokenResponse GetAuthenticationToken(DTOs.GetAuthenticationTokenRequest request)
		{
			var response = new DTOs.GetAuthenticationTokenResponse();

			Logger.LogInformation(string.Format("GetAuthenticationToken, ScmManagementUrl: {0}", request.ScmManagementUrl));

			var tryAttemptsLeft = request.MaxTries;
			while (tryAttemptsLeft > 0)
			{
				try
				{
					using (var remoteManagementClient = ISI.Extensions.Scm.ServiceReferences.Scm.RemoteManagementClient.GetClient(request.ScmManagementUrl))
					{
						response.AuthenticationToken = remoteManagementClient.GetAuthenticationTokenAsync(request.UserName, request.Password).GetAwaiter().GetResult();
					}

					tryAttemptsLeft = 0;
				}
				catch (Exception exception)
				{
					tryAttemptsLeft--;
					if (tryAttemptsLeft < 0)
					{
						Logger.LogError("Error getting authentication token");
						throw;
					}

					Logger.LogError(string.Format("Error getting authentication token, Sleeping for {0} seconds", request.ExceptionSleepForInSeconds));

					System.Threading.Thread.Sleep(TimeSpan.FromSeconds(request.ExceptionSleepForInSeconds));
				}
			}

			return response;
		}
	}
}