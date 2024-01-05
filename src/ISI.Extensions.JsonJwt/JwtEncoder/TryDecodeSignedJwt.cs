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
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.TypeLocator.Extensions;
using SerializableEntitiesDTOs = ISI.Extensions.JsonJwt.SerializableEntities;

namespace ISI.Extensions.JsonJwt
{
	public partial class JwtEncoder
	{
		public bool TryDecodeSignedJwt(string signedJwt, out Jwt jwt) => TryDecodeSignedJwt(new SerializableEntitiesDTOs.SignedJwt(signedJwt), null, out jwt);

		public bool TryDecodeSignedJwt(SerializableEntitiesDTOs.SignedJwt signedJwt, out Jwt jwt) => TryDecodeSignedJwt(signedJwt, null, out jwt);

		public bool TryDecodeSignedJwt(string signedJwt, string secretKey, out Jwt jwt) => TryDecodeSignedJwt(new SerializableEntitiesDTOs.SignedJwt(signedJwt), secretKey, out jwt);

		public bool TryDecodeSignedJwt(SerializableEntitiesDTOs.SignedJwt signedJwt, string secretKey, out Jwt jwt)
		{
			jwt = new Jwt()
			{
				Signature = signedJwt.Signature,
			};

			var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

			if (jwtSecurityTokenHandler.CanReadToken($"{signedJwt.ProtectedHeader}.{signedJwt.Payload}.{signedJwt.Signature}"))
			{
				var jwtToken = jwtSecurityTokenHandler.ReadJwtToken($"{signedJwt.ProtectedHeader}.{signedJwt.Payload}.{signedJwt.Signature}");

				foreach (var header in jwtToken.Header)
				{
					jwt.Header.Add(header.Key, header.Value);
				}

				foreach (var item in jwtToken.Payload)
				{
					jwt.Payload.Add(item.Key, item.Value);
				}

				return true;
			}

			if (CanReadSignedJwt(signedJwt))
			{
				jwt.Header = JsonSerializer.Deserialize<Dictionary<string, object>>(Base64Decode(signedJwt.ProtectedHeader));

				if (!string.IsNullOrWhiteSpace(signedJwt.Payload))
				{
					jwt.Payload.Add("payload", Base64Decode(signedJwt.Payload));
				}

				return true;

				//return ValidateJwt(jwt, secretKey);
			}

			return false;
		}

		private string Base64Decode(string value)
		{
			value = value.Replace("-", "+").Replace("_", "/");

			switch (value.Length % 4)
			{
				case 0:
					break;
				case 2:
					value += "==";
					break;
				case 3:
					value += "=";
					break;
				default:
					throw new Exception("Not Valid");
			}

			return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value));
		}
	}
}