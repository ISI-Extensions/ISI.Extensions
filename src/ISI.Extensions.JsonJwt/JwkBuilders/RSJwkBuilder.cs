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
	public class RSJwkBuilder : IJwkBuilder
	{
		public string JwkAlgorithmKey => $"ES{HashSize}";

		private const int DefaultHasSize = 256;
		private const int DefaultKeySize = 2048;

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
						HashAlgorithm = System.Security.Cryptography.SHA256.Create();
						break;

					case 384:
						HashAlgorithm = System.Security.Cryptography.SHA384.Create();
						break;

					case 512:
						HashAlgorithm = System.Security.Cryptography.SHA512.Create();
						break;

					default:
						throw new System.InvalidOperationException("illegal SHA2 hash size");
				}
			}
		}

		private int _keySize;
		public int KeySize
		{
			get => _keySize;
			private set
			{
				_keySize = value;

				if ((KeySize < 2048) || (KeySize > 4096))
				{
					throw new InvalidOperationException("illegal RSA key bit length");
				}
			}
		}

		protected System.Security.Cryptography.HashAlgorithm HashAlgorithm { get; private set; }

		private System.Security.Cryptography.RSACryptoServiceProvider _rsaCryptoServiceProvider = null;
		protected System.Security.Cryptography.RSACryptoServiceProvider RSACryptoServiceProvider => _rsaCryptoServiceProvider ??= new System.Security.Cryptography.RSACryptoServiceProvider(KeySize);


		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public RSJwkBuilder(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string serializedJwk,
			int hashSize = DefaultHasSize,
			int keySize = DefaultKeySize)
		{
			JsonSerializer = jsonSerializer;

			HashSize = hashSize;

			KeySize = keySize;

			if (!string.IsNullOrWhiteSpace(serializedJwk))
			{
				try
				{
					RSACryptoServiceProvider.FromXmlString(serializedJwk);
				}
				catch (PlatformNotSupportedException)
				{
					FromXmlString(serializedJwk);
				}
			}
		}

		public string GetSerializedJwk()
		{
			var keyParams = RSACryptoServiceProvider.ExportParameters(false);

			return JsonSerializer.Serialize(new SerializableEntitiesDTOs.RSJwk()
			{
				Exponent = JwtEncoder.UrlEncode(keyParams.Exponent),
				Modulus = JwtEncoder.UrlEncode(keyParams.Modulus),
			});
		}


		public string GetSignature(string headerDotPayload)
		{
			return JwtEncoder.UrlEncode(RSACryptoServiceProvider.SignData(Encoding.ASCII.GetBytes(headerDotPayload), HashAlgorithm));
		}

		public bool VerifySignature(string headerDotPayload, string signature)
		{
			return RSACryptoServiceProvider.VerifyData(Encoding.ASCII.GetBytes(headerDotPayload), HashAlgorithm, JwtEncoder.Base64DecodeToBytes(signature));
		}

		//  https://github.com/dotnet/corefx/issues/23686#issuecomment-383245291
		private void FromXmlString(string jwk)
		{
			var parameters = new System.Security.Cryptography.RSAParameters();

			var jwkXml = System.Xml.Linq.XElement.Parse(jwk);

			var rsaKeyValueElement = jwkXml.GetElementByLocalName("RSAKeyValue");
			if (rsaKeyValueElement != null)
			{
				byte[] getNodeValue(string nodeName)
				{
					var element = rsaKeyValueElement.GetElementByLocalName(nodeName);

					if (element != null)
					{
						return (string.IsNullOrEmpty(element.Value) ? null : Convert.FromBase64String(element.Value));
					}

					return null;
				}

				parameters.Modulus = getNodeValue(nameof(parameters.Modulus));
				parameters.Exponent = getNodeValue(nameof(parameters.Exponent));
				parameters.P = getNodeValue(nameof(parameters.P));
				parameters.Q = getNodeValue(nameof(parameters.Q));
				parameters.DP = getNodeValue(nameof(parameters.DP));
				parameters.DQ = getNodeValue(nameof(parameters.DQ));
				parameters.InverseQ = getNodeValue(nameof(parameters.InverseQ));
				parameters.D = getNodeValue(nameof(parameters.D));
			}

			RSACryptoServiceProvider.ImportParameters(parameters);
		}

		public void Dispose()
		{
			_rsaCryptoServiceProvider?.Dispose();
			_rsaCryptoServiceProvider = null;
			HashAlgorithm?.Dispose();
			HashAlgorithm = null;
		}
	}
}