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
using DTOs = ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi;

namespace ISI.Extensions.Jenkins
{
	public partial class JenkinsApi
	{
		public DTOs.IsJobEnabledResponse IsJobEnabled(DTOs.IsJobEnabledRequest request)
		{
			var response = new DTOs.IsJobEnabledResponse();
			
			var jobStatusXml = GetJobStatusXml(new()
			{
				JenkinsUrl = request.JenkinsUrl,
				UserName = request.UserName,
				ApiToken = request.ApiToken,
				SslProtocols = request.SslProtocols,
				JobId = request.JobId,
			})?.StatusXml ?? string.Empty;

			if (!string.IsNullOrWhiteSpace(jobStatusXml))
			{
				response.IsEnabled = true;

				const string disabledToken = "<disabled>";
				var disabledIndex = jobStatusXml.IndexOf(disabledToken, StringComparison.InvariantCultureIgnoreCase);
				if (disabledIndex >= 0)
				{
					response.IsEnabled = !jobStatusXml.Substring(disabledIndex + disabledToken.Length).Split(new[] {'<'}).NullCheckedFirstOrDefault().ToBoolean();
				}
			}

			return response;
		}
	}
}