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
using ISI.Extensions.JsonSerialization.Extensions;
using SerializableEntitiesDTOs = ISI.Extensions.JsonJwt.SerializableEntities;

namespace ISI.Extensions.JsonJwt.JwkBuilders
{
	public class JwkBuilderFactory
	{
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public JwkBuilderFactory(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			JsonSerializer = jsonSerializer;
		}

		public IJwkBuilder GetJwkBuilder(JwkAlgorithmKey jwkAlgorithmKey)
		{
			switch (jwkAlgorithmKey)
			{
				case JwkAlgorithmKey.ES256:
					return new ESJwkBuilder(JsonSerializer, 256);

				case JwkAlgorithmKey.ES384:
					return new ESJwkBuilder(JsonSerializer, 384);

				case JwkAlgorithmKey.ES512:
					return new ESJwkBuilder(JsonSerializer, 512);

				case JwkAlgorithmKey.RS256:
					return new RSJwkBuilder(JsonSerializer);

				default:
					throw new ArgumentOutOfRangeException(nameof(jwkAlgorithmKey), jwkAlgorithmKey, null);
			}
		}

		public IJwkBuilder GetJwkBuilder(string pem)
		{
			IJwkBuilder GetJwkBuilderFromKey(Org.BouncyCastle.Crypto.AsymmetricKeyParameter asymmetricKeyParameter)
			{
				var jwkAlgorithmKey = (string)null;
				var asymmetricCipherKeyPair = (Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair)null;

				if (asymmetricKeyParameter is Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters)
				{
					var publicKey = new Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters(false, rsaPrivateCrtKeyParameters.Modulus, rsaPrivateCrtKeyParameters.PublicExponent);

					asymmetricCipherKeyPair = new Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair(publicKey, asymmetricKeyParameter);

					return new RSJwkBuilder(JsonSerializer, asymmetricCipherKeyPair);
				}

				if (asymmetricKeyParameter is Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters privateKey)
				{
					var ecPoint = privateKey.Parameters.G.Multiply(privateKey.D);

					Org.BouncyCastle.Asn1.DerObjectIdentifier derObjectIdentifier = null;

					switch (privateKey.Parameters.Curve.FieldSize)
					{
						case 256:
							derObjectIdentifier = Org.BouncyCastle.Asn1.Sec.SecObjectIdentifiers.SecP256r1;
							break;

						case 384:
							derObjectIdentifier = Org.BouncyCastle.Asn1.Sec.SecObjectIdentifiers.SecP384r1;
							break;

						case 521:
							derObjectIdentifier = Org.BouncyCastle.Asn1.Sec.SecObjectIdentifiers.SecP521r1;
							break;

						default:
							throw new NotSupportedException();
					}

					var publicKey = new Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters("EC", ecPoint, derObjectIdentifier);

					asymmetricCipherKeyPair = new Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair(publicKey, asymmetricKeyParameter);

					return new ESJwkBuilder(JsonSerializer, asymmetricCipherKeyPair, privateKey.Parameters.Curve.FieldSize);
				}

				throw new NotSupportedException();
			};


			using (var reader = new System.IO.StringReader(pem))
			{
				var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader);

				switch (pemReader.ReadObject())
				{
					case Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair asymmetricCipherKeyPair:
						return GetJwkBuilderFromKey(asymmetricCipherKeyPair.Private);

					case Org.BouncyCastle.Crypto.AsymmetricKeyParameter asymmetricKeyParameter:
						return GetJwkBuilderFromKey(asymmetricKeyParameter);

					default:
						throw new NotSupportedException();
				}
			}
		}

		public IJwkBuilder GetJwkBuilder(JwkAlgorithmKey jwkAlgorithmKey, string serializedJwk)
		{
			switch (jwkAlgorithmKey)
			{
				case JwkAlgorithmKey.ES256:
				case JwkAlgorithmKey.ES384:
				case JwkAlgorithmKey.ES512:
					return new ESJwkBuilder(JsonSerializer, serializedJwk, jwkAlgorithmKey.GetKey().Substring(2).ToInt());

				case JwkAlgorithmKey.RS256:
					return new RSJwkBuilder(JsonSerializer, serializedJwk);
				
				default:
					throw new ArgumentOutOfRangeException(nameof(jwkAlgorithmKey), jwkAlgorithmKey, null);
			}
		}
	}
}