#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using NUnit.Framework;
using System.Runtime.Serialization;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class JsonJwt_Tests
	{
		[DataContract]
		public class NewOrderRequest
		{
			[DataMember(Name = "notBefore", EmitDefaultValue = false)]
			public string __CertificateNotBeforeDateTimeUtc
			{
				get => CertificateNotBeforeDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise);
				set => CertificateNotBeforeDateTimeUtc = value.ToDateTimeUtcNullable();
			}

			[IgnoreDataMember]
			public DateTime? CertificateNotBeforeDateTimeUtc { get; set; }

			[DataMember(Name = "notAfter", EmitDefaultValue = false)]
			public string __CertificateNotAfterDateTimeUtc
			{
				get => CertificateNotAfterDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise);
				set => CertificateNotAfterDateTimeUtc = value.ToDateTimeUtcNullable();
			}

			[IgnoreDataMember]
			public DateTime? CertificateNotAfterDateTimeUtc { get; set; }

			[DataMember(Name = "identifiers", EmitDefaultValue = false)]
			public CertificateIdentifier[] CertificateIdentifiers { get; set; }

			[DataContract]
			public class CertificateIdentifier
			{
				[DataMember(Name = "type", EmitDefaultValue = false)]
				public string IdentifierType { get; set; }

				[DataMember(Name = "value", EmitDefaultValue = false)]
				public string IdentifierValue { get; set; }
			}

			[Test]
			public void Deserialize_Test()
			{
				var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
				var configuration = configurationBuilder.Build();

				var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
					.AddOptions()
					.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

				services.AddAllConfigurations(configuration);

				var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

				var jsonSerializer = new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer();

				var signedPayload = @"eyJhbGciOiJFUzI1NiIsImtpZCI6Imh0dHBzOi8vbG9jYWxob3N0OjE1NjMzL2FjbWUtYWNjb3VudHMvOWRkOTdkMmItODY4My00YzQ0LWI5OGUtM2JkYjE3NzI5ZWVjIiwibm9uY2UiOiJNNzlTTzJKNDJEWUk3TEhPSUNCVE83IiwidXJsIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6MTU2MzMvYWNtZS1vcmRlcnMvY3JlYXRlIn0.eyJpZGVudGlmaWVycyI6W3sidHlwZSI6ImRucyIsInZhbHVlIjoiKi55b3VyLmRvbWFpbi5uYW1lIn1dfQ.pnUwQYhNNW7vIA_FWapKfY0WHrhwpUJ5dDQG7RSO6arO2_48gNkJJwrYFvY2YsE7XC5RmgUwvNCoD03XSqE_Pg";

				var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

				if (jwtSecurityTokenHandler.CanReadToken(signedPayload))
				{
					var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(signedPayload);

					var request = jwtSecurityToken.Payload.Deserialize<NewOrderRequest>(jsonSerializer);
				}
			}
		}
	}
}
