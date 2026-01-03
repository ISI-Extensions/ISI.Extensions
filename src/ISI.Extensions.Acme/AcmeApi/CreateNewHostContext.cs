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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Acme.DataTransferObjects.AcmeApi;

namespace ISI.Extensions.Acme
{
	public partial class AcmeApi
	{
		public DTOs.CreateNewHostContextResponse CreateNewHostContext(DTOs.CreateNewHostContextRequest request)
		{
			var response = new DTOs.CreateNewHostContextResponse();

			using (var privateKey = System.Security.Cryptography.ECDsa.Create(System.Security.Cryptography.ECCurve.NamedCurves.nistP256))
			using (var publicKey = System.Security.Cryptography.ECDsa.Create(privateKey.ExportParameters(false)))
			{
				var jsonWebKey = Microsoft.IdentityModel.Tokens.JsonWebKeyConverter.ConvertFromECDsaSecurityKey(new Microsoft.IdentityModel.Tokens.ECDsaSecurityKey(publicKey)).NullCheckedConvert(source => new ISI.Extensions.JsonJwt.SerializableEntities.JsonWebKey()
				{
					Alg = source.Alg,
					Crv = source.Crv,
					D = source.D,
					DP = source.DP,
					DQ = source.DQ,
					E = source.E,
					K = source.K,
					KeyOps = source.KeyOps.ToNullCheckedArray(),
					Kid = source.Kid,
					Kty = source.Kty,
					N = source.N,
					Oth = source.Oth.ToNullCheckedArray(),
					P = source.P,
					Q = source.Q,
					QI = source.QI,
					Use = source.Use,
					X = source.X,
					X5c = source.X5c.ToNullCheckedArray(),
					X5t = source.X5t,
					X5tS256 = source.X5tS256,
					X5u = source.X5u,
					Y = source.Y,
				});

				response.HostContext = GetHostContext(new()
				{
					HostDirectoryUri = request.HostDirectoryUri,
					SerializedJsonWebKey = JsonSerializer.Serialize(jsonWebKey, false),
					Pem = privateKey.ExportECPrivateKeyPem(),
				}).HostContext;
			}

			return response;
		}
	}
}