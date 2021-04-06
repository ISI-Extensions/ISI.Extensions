#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Scm
{
	public partial class DeploymentManagerApi
	{
		private bool Watch(string servicesManagerUrl, string password, string statusTrackerKey)
		{
			var success = false;

			using (var managerClient = ISI.Extensions.Scm.ServiceReferences.ServicesManager.ManagerClient.GetClient(servicesManagerUrl))
			{
				managerClient.Endpoint.Binding.OpenTimeout = TimeSpan.FromMinutes(60);
				managerClient.Endpoint.Binding.SendTimeout = TimeSpan.FromMinutes(60);
				managerClient.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMinutes(60);
				managerClient.Endpoint.Binding.CloseTimeout = TimeSpan.FromMinutes(60);

				var logIndex = 0;

				var isFinished = false;
				while (!isFinished)
				{
					var maxTries = 3;
					while (maxTries > 0)
					{
						try
						{
							System.Threading.Thread.Sleep(5000);

							var response = managerClient.GetStatus(password, statusTrackerKey);

							if (response != null)
							{
								success = response.Success;
								isFinished = !response.Active;

								while (logIndex < response.Log.Count)
								{
									Logger.LogInformation(string.Format("{0}", response.Log[logIndex++].Description));
								}
							}
							else
							{
								isFinished = true;
							}

							maxTries = 0;
						}
#pragma warning disable CS0168 // Variable is declared but never used
						catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
						{
							if (maxTries > 0)
							{
								System.Threading.Thread.Sleep(20000);
								maxTries--;
							}
							else
							{
								throw;
							}
						}
					}

				}

				Logger.Log((success ? LogLevel.Information : LogLevel.Error), string.Format("  Success '{0}'.", (success ? "True" : "False")));
			}

			return success;
		}
	}
}