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
using System.Text;
using System.Runtime.Serialization;
using ISI.Extensions.JsonSerialization.Extensions;
using SerializableEntitiesDTOs = ISI.Extensions.JsonJwt.SerializableEntities;

namespace ISI.Extensions.JsonJwt.JwkBuilders
{
	public class ESJwkBuilder : IJwkBuilder
	{
		public string AlgorithmKey => $"ES{HashSize}";

		private const int DefaultHasSize = 256;

		private int _hashSize;
		public int HashSize
		{
			get => _hashSize;
			private set
			{
				_hashSize = value;

				switch (value)
				{
					case 256:
						HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName.SHA256;
						break;

					case 384:
						HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName.SHA384;
						break;

					case 512:
						HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName.SHA512;
						break;
				}
			}
		}

		protected System.Security.Cryptography.HashAlgorithmName HashAlgorithmName { get; private set; }

		protected System.Security.Cryptography.ECCurve ECCurve
		{
			get
			{
				switch (HashSize)
				{
					case 256:
						return System.Security.Cryptography.ECCurve.NamedCurves.nistP256;

					case 384:
						return System.Security.Cryptography.ECCurve.NamedCurves.nistP384;

					case 512:
						return System.Security.Cryptography.ECCurve.NamedCurves.nistP521;
				}

				throw new NotImplementedException();
			}
		}

		private System.Security.Cryptography.ECDsa _ecDsa = null;
		protected System.Security.Cryptography.ECDsa ECDsa => _ecDsa ??= System.Security.Cryptography.ECDsa.Create(ECCurve);

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public ESJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string serializedJwk,
			int? hashSize = null)
		{
			JsonSerializer = jsonSerializer;

			if (string.IsNullOrWhiteSpace(serializedJwk))
			{
				HashSize = hashSize ?? DefaultHasSize;
			}
			else
			{
				var jwkDetails = JsonSerializer.Deserialize<SerializableEntitiesDTOs.ESJwkDetails>(serializedJwk);
				HashSize = jwkDetails.HashSize ?? hashSize ?? DefaultHasSize;

				var ecParameters = ECDsa.ExportParameters(true);

				if (!string.IsNullOrWhiteSpace(jwkDetails.D))
				{
					ecParameters.D = JwtEncoder.Base64DecodeToBytes(jwkDetails.D);
				}

				ecParameters.Q.X = JwtEncoder.Base64DecodeToBytes(jwkDetails.X);
				ecParameters.Q.Y = JwtEncoder.Base64DecodeToBytes(jwkDetails.Y);

				ECDsa.ImportParameters(ecParameters);
			}
		}
		
		public string GetSerializedJwk()
		{
			var ecParameters = ECDsa.ExportParameters(false);

			return JsonSerializer.Serialize(new SerializableEntitiesDTOs.ESJwk()
			{
				CurveName = $"P-{HashSize}",
				X = JwtEncoder.UrlEncode(ecParameters.Q.X),
				Y = JwtEncoder.UrlEncode(ecParameters.Q.Y),
			});
		}
		
		public string GetSignature(string headerDotPayload)
		{
			return JwtEncoder.UrlEncode(ECDsa.SignData(Encoding.ASCII.GetBytes(headerDotPayload), HashAlgorithmName));
		}

		public bool VerifySignature(string headerDotPayload, string signature)
		{
			return ECDsa.VerifyData(Encoding.ASCII.GetBytes(headerDotPayload), JwtEncoder.Base64DecodeToBytes(signature), HashAlgorithmName);
		}

		public void Dispose()
		{
			_ecDsa?.Dispose();
			_ecDsa = null;
		}
	}
}
