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
