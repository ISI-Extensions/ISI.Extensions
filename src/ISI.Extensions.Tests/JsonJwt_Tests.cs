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

			//signedJwt = @"eyJhbGciOiJFUzI1NiIsImp3ayI6eyJjcnYiOiJQLTI1NiIsImt0eSI6IkVDIiwieCI6IkdyeDViVnd1aDZyV2xoaWlxQ2lPOVEzcUhvOVN5cUoyanFFSmlfVVhDRm8iLCJ5IjoiX0E1UGhzdkVNWTA4RWVWcHdlN0lXWU1mcWpOWEhycEN2aEdjUHI1b0RMVSJ9LCJub25jZSI6IlZDODI3M1dIVTg3T1RPR0dPMjUxQkciLCJ1cmwiOiJodHRwczovL2xvY2FsaG9zdDoxNTYzMy9hY21lLWFjY291bnRzL2NyZWF0ZSJ9.eyJjb250YWN0IjpbImFkbWluQGV4YW1wbGUuY29tIl0sInRlcm1zT2ZTZXJ2aWNlQWdyZWVkIjp0cnVlfQ.s84kdQ5EjOmkgmZh0LHoXjk8YcBhJc01hPcxoYAfC4SgJ7SSGQTY_8Zf-mv1k1J9x31maN0AE4GbqGyWr2qI1w";
			//signedJwt = @"eyJhbGciOiJFUzI1NiIsImp3ayI6eyJjcnYiOiJQLTI1NiIsImt0eSI6IkVDIiwieCI6IkxSVFFUWU5Rck5ONUFrTkpoSUFtSE9KOXcxd0ZWdWI5V0hwMFZEamFSRFUiLCJ5IjoiVGZWNnQ4UHM5R0gwaUxNYURiSHZLd3JGYW50MUFNMmRuLWNoVF9TdmVBdyJ9LCJub25jZSI6Ik5WSjY1MklTUzNSSUI0QlJKSURPTUUiLCJ1cmwiOiJodHRwczovL2xvY2FsaG9zdDoxNTYzMy9hY21lLWFjY291bnRzL2NyZWF0ZSJ9.eyJjb250YWN0IjpbImFkbWluQGV4YW1wbGUuY29tIl0sInRlcm1zT2ZTZXJ2aWNlQWdyZWVkIjp0cnVlfQ.WECUzP_70e_shya9pbTV8eWE-_Ib-oUbNKhQL4dcWRk2-1gIn11XcjeYNcwl9VI97VIr6DXomnmRmtyOdiELaQ";


			//if (jwtEncoder.TryDecodeSignedJwt(signedJwt, kid => null, out var jwt))
			//{
			//	var request = jwt.DeserializePayload<NewAccountRequest>(jsonSerializer);
			//}

			signedJwt = @"eyJhbGciOiJFUzI1NiIsImtpZCI6Imh0dHBzOi8vbG9jYWxob3N0OjE1NjMzL2FjbWUtYWNjb3VudHMvODVlMmQ2M2ItNTEzMC00Y2YyLWFlZTYtNTJhYmU1MjkwMmE0Iiwibm9uY2UiOiJDVFpISE8yRjY0ODkxSjE1VkFRSUUxIiwidXJsIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6MTU2MzMvYWNtZS1vcmRlcnMvY3JlYXRlIn0.eyJpZGVudGlmaWVycyI6W3sidHlwZSI6ImRucyIsInZhbHVlIjoiKi55b3VyLmRvbWFpbi5uYW1lIn1dfQ.AxuaucT5ZYGFmNtwBzXZPU3mFKqiPY-1VAUbv3fyrJdPpBrG95dmH8S3DgHOrZGko3_rE6Q5k9IP9-bSzyNHoQ";

			signedJwt = @"eyJOb25jZSI6IllNV0FWUkRWWks3NTVKNzFFTlFaUFMiLCJhbGciOiJFUzI1NiIsImp3ayI6IntcImNydlwiOlwiUC0yNTZcIixcImt0eVwiOlwiRUNcIixcInhcIjpcIkVSRWY3NmxBb2lySmlETjFDWjVYRC0zYnJ6THdvR2lkX3lqV01pdF81Y29cIixcInlcIjpcIm52ODQ3N3N2bkw0cFJMVW5Ocm5laTJTbWZRZ09fcjBSTUN1eGE0Y2pQYjRcIn0ifQ.eyJhY2NvdW50TmFtZSI6ImxvY2FsaG9zdCIsImNvbnRhY3QiOiJbXCJtZUBoZXJlLmNvbVwiXSIsInRlcm1zT2ZTZXJ2aWNlQWdyZWVkIjoidHJ1ZSJ9.eDQSakk3kIAofD9M56wnSGRx-mNqKi2MaLEDTxpy7VO1a5IJWDyHK1TGO8JdBZEbhMXE7GRLyWVExVfnrX-GTg";

			if (jwtEncoder.TryDecodeSignedJwt(signedJwt, kid => @"{
	""crv"": ""P-256"",
	""kty"": ""EC"",
	""x"": ""gAUZA2edykp3CrJ87vMJbpOQg1vjo0UZgv0sjmbQuCM"",
	""y"": ""hMUtT4ORvQopm1QfR5Q-9KvM-39j0HowlINU78Lw8xs""
}", out var jwt))
			{
				var request = jwt.DeserializePayload<ISI.Extensions.Acme.SerializableModels.AcmeAccounts.CreateNewAccountRequest>(jsonSerializer);
			}
		}
	}
}