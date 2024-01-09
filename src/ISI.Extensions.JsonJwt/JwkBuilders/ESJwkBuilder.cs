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
	public class ESJwkBuilder : AbstractJwkBuilder, IJwkBuilder
	{
		public override JwkAlgorithmKey JwkAlgorithmKey
		{
			get
			{
				switch (HashSize)
				{
					case 256: return JwkAlgorithmKey.ES256;
					case 384: return JwkAlgorithmKey.ES384;
					case 512: return JwkAlgorithmKey.ES512;
					default: throw new NotImplementedException();
				}
			}
		}

		private const int DefaultHashSize = 256;

		protected string CurveName
		{
			get
			{
				switch (HashSize)
				{
					case 256: return "P-256";
					case 384: return "P-384";
					case 512: return "P-512";
					default: throw new NotImplementedException();
				}
			}
		}

		protected Org.BouncyCastle.Asn1.DerObjectIdentifier CurveIdentifier
		{
			get
			{
				switch (HashSize)
				{
					case 256: return Org.BouncyCastle.Asn1.Sec.SecObjectIdentifiers.SecP256r1;
					case 384: return Org.BouncyCastle.Asn1.Sec.SecObjectIdentifiers.SecP384r1;
					case 512: return Org.BouncyCastle.Asn1.Sec.SecObjectIdentifiers.SecP521r1;
					default: throw new NotImplementedException();
				}
			}
		}

		protected override string HashAlgorithm
		{
			get
			{
				switch (HashSize)
				{
					case 256: return "SHA256";
					case 384: return "SHA384";
					case 512: return "SHA512";
					default: throw new NotImplementedException();
				}
			}
		}

		protected override string SigningAlgorithm
		{
			get
			{
				switch (HashSize)
				{
					case 256: return "SHA-256withECDSA";
					case 384: return "SHA-384withECDSA";
					case 512: return "SHA-512withECDSA";
					default: throw new NotImplementedException();
				}
			}
		}

		private Org.BouncyCastle.Crypto.AsymmetricKeyParameter _publicKey = null;
		protected override Org.BouncyCastle.Crypto.AsymmetricKeyParameter PublicKey => _publicKey ?? base.PublicKey;

		protected int FieldSize { get; }

		public ESJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			int hashSize = DefaultHashSize)
		: base(jsonSerializer, hashSize)
		{
			var keyPairGenerator = Org.BouncyCastle.Security.GeneratorUtilities.GetKeyPairGenerator("ECDSA");

			var ecKeyGenerationParameters = new Org.BouncyCastle.Crypto.Parameters.ECKeyGenerationParameters(Org.BouncyCastle.Crypto.EC.CustomNamedCurves.GetOid(CurveName), new Org.BouncyCastle.Security.SecureRandom());

			keyPairGenerator.Init(ecKeyGenerationParameters);

			AsymmetricCipherKeyPair = keyPairGenerator.GenerateKeyPair();

			FieldSize = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)AsymmetricCipherKeyPair.Private).Parameters.Curve.FieldSize / 8;
		}

		public ESJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair asymmetricCipherKeyPair,
			int hashSize)
		: base(jsonSerializer, hashSize)
		{
			AsymmetricCipherKeyPair = asymmetricCipherKeyPair;

			FieldSize = ((Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters)AsymmetricCipherKeyPair.Private).Parameters.Curve.FieldSize / 8;
		}

		public ESJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string serializedJwk,
			int hashSize)
		: base(jsonSerializer, hashSize)
		{
			var jwkDetails = JsonSerializer.Deserialize<SerializableEntitiesDTOs.ESJwkDetails>(serializedJwk);

			var curve = Org.BouncyCastle.Asn1.Nist.NistNamedCurves.GetByName(CurveName).Curve;

			var ecPoint = curve.CreatePoint(new Org.BouncyCastle.Math.BigInteger(JwtEncoder.Base64DecodeToBytes(jwkDetails.X)), new Org.BouncyCastle.Math.BigInteger(JwtEncoder.Base64DecodeToBytes(jwkDetails.Y)));

			_publicKey = new Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters("ECDSA", ecPoint, CurveIdentifier);
		}

		public override string GetSerializedJwk()
		{
			var ecPublicKeyParameters = (Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters)AsymmetricCipherKeyPair.Public;

			var jwt = new SerializableEntitiesDTOs.ESJwk()
			{
				CurveName = CurveName,
				X = JwtEncoder.UrlEncode(ecPublicKeyParameters.Q.AffineXCoord.ToBigInteger().ToByteArrayUnsigned()),
				Y = JwtEncoder.UrlEncode(ecPublicKeyParameters.Q.AffineYCoord.ToBigInteger().ToByteArrayUnsigned()),
			};

			return JsonSerializer.Serialize(jwt, false);
		}

		public override string GetSignature(string headerDotPayload)
		{
			var headerDotPayloadBytes = Encoding.ASCII.GetBytes(headerDotPayload);

			var signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner(SigningAlgorithm);
			signer.Init(true, AsymmetricCipherKeyPair.Private);
			signer.BlockUpdate(headerDotPayloadBytes, 0, headerDotPayloadBytes.Length);

			var signature = signer.GenerateSignature();
			var sequence = (Org.BouncyCastle.Asn1.Asn1Sequence)Org.BouncyCastle.Asn1.Asn1Object.FromByteArray(signature);

			var nums = sequence
				.OfType<Org.BouncyCastle.Asn1.DerInteger>()
				.Select(i => i.Value.ToByteArrayUnsigned())
				.ToArray();

			var signatureBytes = new byte[FieldSize * nums.Length];

			for (var i = 0; i < nums.Length; ++i)
			{
				Array.Copy(nums[i], 0, signatureBytes, FieldSize * (i + 1) - nums[i].Length, nums[i].Length);
			}

			return JwtEncoder.UrlEncode(signatureBytes);
		}
	}
}