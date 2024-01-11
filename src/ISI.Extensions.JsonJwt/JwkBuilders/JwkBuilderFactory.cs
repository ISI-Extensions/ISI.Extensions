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

				case JwkAlgorithmKey.ES521:
					return new ESJwkBuilder(JsonSerializer, 521);

				case JwkAlgorithmKey.RS256:
					return new RSJwkBuilder(JsonSerializer, 256);

				case JwkAlgorithmKey.RS384:
					return new RSJwkBuilder(JsonSerializer, 384);

				case JwkAlgorithmKey.RS512:
					return new RSJwkBuilder(JsonSerializer, 512);

				default:
					throw new ArgumentOutOfRangeException(nameof(jwkAlgorithmKey), jwkAlgorithmKey, null);
			}
		}


		public IJwkBuilder GetJwkBuilder(JwkAlgorithmKey jwkAlgorithmKey, string serializedJwkOrPem)
		{
			switch (jwkAlgorithmKey)
			{
				case JwkAlgorithmKey.ES256:
				case JwkAlgorithmKey.ES384:
				case JwkAlgorithmKey.ES521:
					return new ESJwkBuilder(JsonSerializer, serializedJwkOrPem);

				case JwkAlgorithmKey.RS256:
				case JwkAlgorithmKey.RS384:
				case JwkAlgorithmKey.RS512:
					return new RSJwkBuilder(JsonSerializer, serializedJwkOrPem);
				
				default:
					throw new ArgumentOutOfRangeException(nameof(jwkAlgorithmKey), jwkAlgorithmKey, null);
			}
		}
	}
}