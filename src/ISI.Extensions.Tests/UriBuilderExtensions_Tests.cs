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
 
using ISI.Extensions.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class UriBuilderExtensions_Tests
	{
		[Test]
		public void SetPathAndQueryString_Test()
		{
			var uriBuilder = new UriBuilder("localhost:5001");
			uriBuilder.SetPathAndQueryString("/nuget/v2/package");

		}

		[Test]
		public void WarmUpWebService_Test()
		{
			var log = new ISI.Extensions.TextWriterLogger(TestContext.Progress);

			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			System.Net.ServicePointManager.Expect100Continue = true;
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;

			var webServiceUrls = new[]
			{
					(new UriBuilder(settings.Scm.WebServiceUrl) {Query = string.Empty}).Uri.ToString(),
					(new UriBuilder(settings.Scm.WebServiceUrl) {Path = string.Empty, Query = string.Empty}).Uri.ToString(),
				};

			foreach (var webServiceUrl in webServiceUrls)
			{
				log.Log(LogLevel.Information, "Warming up: {0}", webServiceUrl);

				var tryAttemptsLeft = 3;
				while (tryAttemptsLeft > 0)
				{
					try
					{
						var warmUpUrl = webServiceUrl;

						var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(warmUpUrl);
						httpWebRequest.Method = System.Net.WebRequestMethods.Http.Get;

						using (var httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
						{
							using (var stream = new System.IO.StreamReader(httpWebResponse.GetResponseStream()))
							{
								var result = stream.ReadToEnd();
							}
						}

						tryAttemptsLeft = 0;
					}
					catch (Exception exception)
					{
						log.Log(LogLevel.Information, exception.ErrorMessageFormatted());

						tryAttemptsLeft--;
						if (tryAttemptsLeft < 0)
						{
							throw;
						}

						System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
					}
				}
			}
		}
	}
}
