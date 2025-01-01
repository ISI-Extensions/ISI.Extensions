#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Crypto
{
	[ISI.Extensions.Crypto.SaltedHashGenerator]
	public class Pbkdf2SaltedHashGenerator : ISaltedHashGenerator
	{
		public const string SaltedHashGeneratorTypeUuid = "ada16a99-d528-4904-9ceb-b1506a2aed1d";

		protected Configuration Configuration { get; }

		public Pbkdf2SaltedHashGenerator(Configuration configuration)
		{
			Configuration = configuration ?? new();
		}

		Guid ISaltedHashGenerator.SaltedHashGeneratorTypeUuid => SaltedHashGeneratorTypeUuid.ToGuid();

		public string GenerateNewCryptoSalt()
		{
			using (var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create())
			{
				var salt = new byte[Configuration.Pbkdf2SaltedHashGenerator.KeySize];

				randomNumberGenerator.GetBytes(salt);

				return string.Format("{0}:{1}", Configuration.Pbkdf2SaltedHashGenerator.Interations, Convert.ToBase64String(salt));
			}
		}


		public string GeneratedHashedValue(string value, string cryptoSalt)
		{
			var cryptoSaltParts = cryptoSalt.Split([':'], StringSplitOptions.None);

			var iterations = cryptoSaltParts[0].ToInt();

			var salt = Convert.FromBase64String(cryptoSaltParts[1]);

			var hash = GeneratePbkdf2(value, salt, iterations, Configuration.Pbkdf2SaltedHashGenerator.KeySize);

			return Convert.ToBase64String(hash);
		}

		public bool ValidateValue(string value, string cryptoSalt, string hashedValue)
		{
			var cryptoSaltParts = cryptoSalt.Split([':'], StringSplitOptions.None);

			var iterations = cryptoSaltParts[0].ToInt();

			var salt = Convert.FromBase64String(cryptoSaltParts[1]);

			var hash = Convert.FromBase64String(hashedValue);

			var pbkdf2 = GeneratePbkdf2(value, salt, iterations, hash.Length);

			return SlowEquals(hash, pbkdf2);
		}

		private bool SlowEquals(byte[] a, byte[] b)
		{
			var diff = (uint)a.Length ^ (uint)b.Length;

			for (var i = 0; i < a.Length && i < b.Length; i++)
			{
				diff |= (uint)(a[i] ^ b[i]);
			}

			return diff == 0;
		}

		private byte[] GeneratePbkdf2(string value, byte[] salt, int iterations, int keySize)
		{
			using (var rfc2898DeriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(value, salt, iterations))
			{
				return rfc2898DeriveBytes.GetBytes(keySize);
			}
		}
	}
}