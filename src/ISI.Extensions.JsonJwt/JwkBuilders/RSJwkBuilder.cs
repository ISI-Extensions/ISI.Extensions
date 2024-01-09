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
	public class RSJwkBuilder : AbstractJwkBuilder, IJwkBuilder
	{
		public override JwkAlgorithmKey JwkAlgorithmKey => JwkAlgorithmKey.RS256;

		private const int DefaultHashSize = 256;
		private const int DefaultKeySize = 2048;

		protected override string HashAlgorithm => $"SHA{HashSize}";
		protected override string SigningAlgorithm => $"SHA-{HashSize}withRSA";

		public int KeySize { get; }

		public RSJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			int hashSize = DefaultHashSize,
			int keySize = DefaultKeySize)
			: base(jsonSerializer, hashSize)
		{
			KeySize = keySize;

			var generator = Org.BouncyCastle.Security.GeneratorUtilities.GetKeyPairGenerator("RSA");
			var generatorParams = new Org.BouncyCastle.Crypto.Parameters.RsaKeyGenerationParameters(Org.BouncyCastle.Math.BigInteger.ValueOf(0x10001), new Org.BouncyCastle.Security.SecureRandom(), (int)keySize, 128);
			generator.Init(generatorParams);

			AsymmetricCipherKeyPair = generator.GenerateKeyPair();
		}

		public RSJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair asymmetricCipherKeyPair)
			: base(jsonSerializer, DefaultHashSize)
		{
			KeySize = DefaultKeySize;

			AsymmetricCipherKeyPair = asymmetricCipherKeyPair;
		}

		public RSJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string serializedJwk)
			: base(jsonSerializer, DefaultHashSize)
		{
			throw new NotImplementedException();
		}
		
		public override string GetSerializedJwk()
		{
			var rsaKeyParameters = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)AsymmetricCipherKeyPair.Public;

			var jwt = new SerializableEntitiesDTOs.RSJwk()
			{
				Exponent = JwtEncoder.UrlEncode(rsaKeyParameters.Exponent.ToByteArrayUnsigned()),
				Modulus = JwtEncoder.UrlEncode(rsaKeyParameters.Modulus.ToByteArrayUnsigned()),
			};

			return JsonSerializer.Serialize(jwt, false);
		}

		public override string GetSignature(string headerDotPayload)
		{
			var headerDotPayloadBytes = Encoding.ASCII.GetBytes(headerDotPayload);

			var signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner(SigningAlgorithm);
			signer.Init(true, AsymmetricCipherKeyPair.Private);
			signer.BlockUpdate(headerDotPayloadBytes, 0, headerDotPayloadBytes.Length);

			var signatureBytes = signer.GenerateSignature();

			return JwtEncoder.UrlEncode(signatureBytes);
		}
	}
}

