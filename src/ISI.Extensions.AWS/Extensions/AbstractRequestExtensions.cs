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
using System.Text;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.AWS.DataTransferObjects;

namespace ISI.Extensions.AWS.Extensions
{
	public static class AbstractRequestExtensions
	{
		public static Amazon.Route53.AmazonRoute53Client GetAmazonRoute53Client(this DTOs.IRequest request, Configuration configuration)
		{
			var awsCredentials = (Amazon.Runtime.AWSCredentials)null;
			var awsRegionEndpoint = (Amazon.RegionEndpoint)null;
			
			if (string.IsNullOrWhiteSpace(request.AmazonAccessKey) && string.IsNullOrWhiteSpace(request.AmazonSecretKey))
			{
				var amazonSecretKey = configuration.AmazonSecretKey;

				amazonSecretKey = (amazonSecretKey.StartsWith("%") && amazonSecretKey.EndsWith("%") ? ISI.Extensions.ConfigurationValueReader.GetValue(amazonSecretKey.Trim('%')) : amazonSecretKey);

				awsCredentials = new Amazon.Runtime.BasicAWSCredentials(configuration.AmazonAccessKey, amazonSecretKey);
			}
			else
			{
				var amazonSecretKey = request.AmazonSecretKey;

				amazonSecretKey = (amazonSecretKey.StartsWith("%") && amazonSecretKey.EndsWith("%") ? ISI.Extensions.ConfigurationValueReader.GetValue(amazonSecretKey.Trim('%')) : amazonSecretKey);

				awsCredentials = new Amazon.Runtime.BasicAWSCredentials(request.AmazonAccessKey, amazonSecretKey);
			}

			if ((awsRegionEndpoint == null) && !string.IsNullOrWhiteSpace(request.RegionEndpoint))
			{
				var regionEndpoint = request.RegionEndpoint;

				regionEndpoint = (regionEndpoint.StartsWith("%") && regionEndpoint.EndsWith("%") ? ISI.Extensions.ConfigurationValueReader.GetValue(regionEndpoint.Trim('%')) : regionEndpoint);

				TryParseRegionEndpoint(regionEndpoint, out awsRegionEndpoint);
			}

			if ((awsRegionEndpoint == null) && !string.IsNullOrWhiteSpace(configuration.RegionEndpoint))
			{
				var regionEndpoint = configuration.RegionEndpoint;

				regionEndpoint = (regionEndpoint.StartsWith("%") && regionEndpoint.EndsWith("%") ? ISI.Extensions.ConfigurationValueReader.GetValue(regionEndpoint.Trim('%')) : regionEndpoint);

				TryParseRegionEndpoint(regionEndpoint, out awsRegionEndpoint);
			}

			return new Amazon.Route53.AmazonRoute53Client(awsCredentials, awsRegionEndpoint ?? Amazon.RegionEndpoint.USEast1);
		}

		private static IDictionary<string, Amazon.RegionEndpoint> _regionEndpointsByName = null;
		public static IDictionary<string, Amazon.RegionEndpoint> RegionEndpointsByName => _regionEndpointsByName ??= GetRegionEndpointsByName();

		private static IDictionary<string, Amazon.RegionEndpoint> GetRegionEndpointsByName()
		{
			var regionEndpointsByName = new Dictionary<string, Amazon.RegionEndpoint>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var regionEndpoint in Amazon.RegionEndpoint.EnumerableAllRegions)
			{
				regionEndpointsByName.Add(regionEndpoint.SystemName, regionEndpoint);
			}

			return regionEndpointsByName;
		}

		public static bool TryParseRegionEndpoint(string regionEndpointKey, out Amazon.RegionEndpoint regionEndpoint)
		{
			if (RegionEndpointsByName.TryGetValue(regionEndpointKey ?? string.Empty, out regionEndpoint))
			{
				return true;
			}

			regionEndpoint = null;
			return false;
		}
	}
}
