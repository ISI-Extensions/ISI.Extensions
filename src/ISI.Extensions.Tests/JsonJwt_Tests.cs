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
using ISI.Extensions.JsonSerialization.Extensions;
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

			[DataContract]
			public class NewAccountRequest
			{
				[DataMember(Name = "contact", EmitDefaultValue = false)]
				public string[] Contacts { get; set; }

				[DataMember(Name = "termsOfServiceAgreed", EmitDefaultValue = false)]
				public bool? TermsOfServiceAgreed { get; set; }

				[DataMember(Name = "onlyReturnExisting", EmitDefaultValue = false)]
				public bool? OnlyReturnExisting  { get; set; }

				//[DataMember(Name = "externalAccountBinding", EmitDefaultValue = false)]
				//public object ExternalAccountBinding { get; set; }
			}

			[Test]
			public void Deserialize_Test()
			{
				var jsonSerializer = new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer();
				var jwkBuilderFactory = new ISI.Extensions.JsonJwt.JwkBuilders.JwkBuilderFactory(jsonSerializer);
				var jwtEncoder = new ISI.Extensions.JsonJwt.JwtEncoder(new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper(), jsonSerializer, jwkBuilderFactory);

				var signedJwt = @"eyJhbGciOiJFUzI1NiIsImtpZCI6Imh0dHBzOi8vbG9jYWxob3N0OjE1NjMzL2FjbWUtYWNjb3VudHMvOWRkOTdkMmItODY4My00YzQ0LWI5OGUtM2JkYjE3NzI5ZWVjIiwibm9uY2UiOiJNNzlTTzJKNDJEWUk3TEhPSUNCVE83IiwidXJsIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6MTU2MzMvYWNtZS1vcmRlcnMvY3JlYXRlIn0.eyJpZGVudGlmaWVycyI6W3sidHlwZSI6ImRucyIsInZhbHVlIjoiKi55b3VyLmRvbWFpbi5uYW1lIn1dfQ.pnUwQYhNNW7vIA_FWapKfY0WHrhwpUJ5dDQG7RSO6arO2_48gNkJJwrYFvY2YsE7XC5RmgUwvNCoD03XSqE_Pg";

				//var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

				//if (jwtSecurityTokenHandler.CanReadToken(signedJwt))
				//{
				//	var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(signedJwt);

				//	var request = jwtSecurityToken.Payload.Deserialize<NewOrderRequest>(jsonSerializer);
				//}

				signedJwt = @"eyJhbGciOiJFUzI1NiIsImp3ayI6eyJjcnYiOiJQLTI1NiIsImt0eSI6IkVDIiwieCI6IkdyeDViVnd1aDZyV2xoaWlxQ2lPOVEzcUhvOVN5cUoyanFFSmlfVVhDRm8iLCJ5IjoiX0E1UGhzdkVNWTA4RWVWcHdlN0lXWU1mcWpOWEhycEN2aEdjUHI1b0RMVSJ9LCJub25jZSI6IlZDODI3M1dIVTg3T1RPR0dPMjUxQkciLCJ1cmwiOiJodHRwczovL2xvY2FsaG9zdDoxNTYzMy9hY21lLWFjY291bnRzL2NyZWF0ZSJ9.eyJjb250YWN0IjpbImFkbWluQGV4YW1wbGUuY29tIl0sInRlcm1zT2ZTZXJ2aWNlQWdyZWVkIjp0cnVlfQ.s84kdQ5EjOmkgmZh0LHoXjk8YcBhJc01hPcxoYAfC4SgJ7SSGQTY_8Zf-mv1k1J9x31maN0AE4GbqGyWr2qI1w";


				if (jwtEncoder.TryDecodeSignedJwt(signedJwt, kid => null, out var jwt))
				{
					var request = jwt.DeserializePayload<NewAccountRequest>(jsonSerializer);
				}
			}
		}
	}
}
