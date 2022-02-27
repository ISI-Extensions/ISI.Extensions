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
using System.Text;
using DTOs = ISI.Extensions.GoDaddy.DataTransferObjects;

namespace ISI.Extensions.GoDaddy.Extensions
{
	public static class AbstractRequestExtensions
	{
		public static ISI.Extensions.WebClient.HeaderCollection GetHeaders(this DTOs.AbstractRequest request, Configuration configuration)
		{
			var headers = new ISI.Extensions.WebClient.HeaderCollection();

			var apiKey = (string.IsNullOrWhiteSpace(request.ApiKey) ? configuration.ApiKey : request.ApiKey);
			var apiSecret = (string.IsNullOrWhiteSpace(request.ApiSecret) ? configuration.ApiSecret : request.ApiSecret);

			var authenticationToken = string.Format("sso-key {0}:{1}", apiKey, apiSecret);

			headers.AddAuthorizationToken(authenticationToken);

			return headers;
		}

		public static string GetUrl(this DTOs.AbstractRequest request, Configuration configuration)
		{
			return (string.IsNullOrWhiteSpace(request.Url) ? configuration.Url : request.Url);
		}
	}
}
