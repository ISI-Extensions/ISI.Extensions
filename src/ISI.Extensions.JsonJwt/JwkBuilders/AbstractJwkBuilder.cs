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
	public abstract class AbstractJwkBuilder
	{
		public abstract JwkAlgorithmKey JwkAlgorithmKey { get; }

		public int HashSize { get; }

		protected abstract string HashAlgorithm { get; }
		protected abstract string SigningAlgorithm { get; }

		protected Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair AsymmetricCipherKeyPair { get; set; }
		protected virtual Org.BouncyCastle.Crypto.AsymmetricKeyParameter PublicKey => AsymmetricCipherKeyPair.Public;

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }


		public AbstractJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			int hashSize)
		{
			JsonSerializer = jsonSerializer;

			HashSize = hashSize;
		}

		public abstract string GetSerializedJwk();

		public abstract string GetSignature(string headerDotPayload);

		public virtual bool VerifySignature(string headerDotPayload, string signature)
		{
			var headerDotPayloadBytes = Encoding.ASCII.GetBytes(headerDotPayload);
			var signatureBytes = JwtEncoder.Base64DecodeToBytes(signature);

			var signer = Org.BouncyCastle.Security.SignerUtilities.GetSigner(SigningAlgorithm);
			signer.Init(false, PublicKey);
			signer.BlockUpdate(headerDotPayloadBytes, 0, headerDotPayloadBytes.Length);

			return signer.VerifySignature(signatureBytes);
		}

		public byte[] GetDer()
		{
			return Org.BouncyCastle.Pkcs.PrivateKeyInfoFactory.CreatePrivateKeyInfo(AsymmetricCipherKeyPair.Private).GetDerEncoded();
		}

		public string GetPublicPem()
		{
			using (var stringWriter = new System.IO.StringWriter())
			{
				var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter);

				pemWriter.WriteObject(AsymmetricCipherKeyPair.Public);

				return stringWriter.ToString();
			}
		}

		public string GetPrivatePem()
		{
			using (var stringWriter = new System.IO.StringWriter())
			{
				var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(stringWriter);

				pemWriter.WriteObject(AsymmetricCipherKeyPair);

				return stringWriter.ToString();
			}
		}
	}
}