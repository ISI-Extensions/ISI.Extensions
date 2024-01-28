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
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using ISI.Extensions.JsonSerialization.Extensions;
using SerializableEntitiesDTOs = ISI.Extensions.JsonJwt.SerializableEntities;

namespace ISI.Extensions.JsonJwt.JwkBuilders
{
	public class ESJwkBuilder : IJwkBuilder
	{
		public JwkAlgorithmKey JwkAlgorithmKey
		{
			get
			{
				switch (KeySize)
				{
					case 256: return JwkAlgorithmKey.ES256;
					case 384: return JwkAlgorithmKey.ES384;
					case 521: return JwkAlgorithmKey.ES512;
					default: throw new NotImplementedException();
				}
			}
		}

		private const int DefaultKeySize = 256;

		public int KeySize { get; set; }

		protected System.Security.Cryptography.HashAlgorithmName HashAlgorithmName
		{
			get
			{
				switch (KeySize)
				{
					case 256: return System.Security.Cryptography.HashAlgorithmName.SHA256;
					case 384: return System.Security.Cryptography.HashAlgorithmName.SHA384;
					case 521: return System.Security.Cryptography.HashAlgorithmName.SHA512;
					default: throw new NotImplementedException();
				}
			}
		}

		protected string CurveName
		{
			get
			{
				switch (KeySize)
				{
					case 256: return "P-256";
					case 384: return "P-384";
					case 521: return "P-512";
					default: throw new NotImplementedException();
				}
			}
			set
			{
				switch (value)
				{
					case "P-256":
						KeySize = 256;
						break;

					case  "P-384":
						KeySize = 384;
						break;

					case "P-512":
						KeySize = 521;
						break;

					default: throw new NotImplementedException();
				}
			}
		}

		protected System.Security.Cryptography.ECCurve Curve
		{
			get
			{
				switch (KeySize)
				{
					case 256: return System.Security.Cryptography.ECCurve.NamedCurves.nistP256;
					case 384: return System.Security.Cryptography.ECCurve.NamedCurves.nistP384;
					case 521: return System.Security.Cryptography.ECCurve.NamedCurves.nistP521;
					default: throw new NotImplementedException();
				}
			}
		}

		protected System.Security.Cryptography.ECDsa ECDsa = null;

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public ESJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			int keySize = DefaultKeySize)
		{
			JsonSerializer = jsonSerializer;

			KeySize = keySize;

			ECDsa = System.Security.Cryptography.ECDsa.Create();
			ECDsa.KeySize = KeySize;
		}

		public ESJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string serializedJwkOrPem)
		{
			JsonSerializer = jsonSerializer;

			if (serializedJwkOrPem.IndexOf("PRIVATE KEY", StringComparison.InvariantCultureIgnoreCase) >= 0)
			{
				ECDsa = System.Security.Cryptography.ECDsa.Create();
				ECDsa.ImportFromPem(serializedJwkOrPem);
			}
			else
			{
				var jwk = JsonSerializer.Deserialize<SerializableEntitiesDTOs.Jwk>(serializedJwkOrPem);

				var ecParameters = new System.Security.Cryptography.ECParameters();

				ecParameters.Q = new System.Security.Cryptography.ECPoint();
				ecParameters.Q.X = JwtEncoder.Base64DecodeToBytes(jwk.X);
				ecParameters.Q.Y = JwtEncoder.Base64DecodeToBytes(jwk.Y);

				CurveName = jwk.CurveName;

				ecParameters.Curve = Curve;

				//PrivateKey
				if (!string.IsNullOrWhiteSpace(jwk.D))
				{
					ecParameters.D = JwtEncoder.Base64DecodeToBytes(jwk.D);
				}

				ECDsa = System.Security.Cryptography.ECDsa.Create();
				ECDsa.ImportParameters(ecParameters);
			}
			
			KeySize = ECDsa.KeySize;
		}
		
		public string GetSerializedJwk()
		{
			var ecParamaters = ECDsa.ExportParameters(true);

			var jwk = new SerializableEntitiesDTOs.Jwk()
			{
				JwtKeyType = JwtKeyType.EllipticCurve,
				CurveName = CurveName,
				X = JwtEncoder.UrlEncode(ecParamaters.Q.X),
				Y = JwtEncoder.UrlEncode(ecParamaters.Q.Y),
			};

			return JsonSerializer.Serialize(jwk, false);
		}

		public string GetSignature(string headerDotPayload)
		{
			var headerDotPayloadBytes = Encoding.ASCII.GetBytes(headerDotPayload);

			var signatureBytes = ECDsa.SignData(headerDotPayloadBytes, HashAlgorithmName);

			return JwtEncoder.UrlEncode(signatureBytes);
		}

		public virtual bool VerifySignature(string headerDotPayload, string signature)
		{
			var headerDotPayloadBytes = Encoding.ASCII.GetBytes(headerDotPayload);
			var signatureBytes = JwtEncoder.Base64DecodeToBytes(signature);

			return ECDsa.VerifyData(headerDotPayloadBytes, signatureBytes, HashAlgorithmName);
		}

		public string GetPrivatePem() => ECDsa.ExportECPrivateKeyPem();

		public void Dispose()
		{
			ECDsa?.Dispose();
			ECDsa = null;
		}
	}
}