#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
//http://crackstation.net/hashing-security.htm

	public class RNGSaltedHash
	{
		public int SaltByteSize { get; }
		public int HashByteSize { get; }


		public RNGSaltedHash(int saltByteSize = 24, int hashByteSize = 24)
		{
			SaltByteSize = saltByteSize;
			HashByteSize = hashByteSize;
		}

		public string GenerateNewCryptoSalt(int interations = 100)
		{
			// Generate a random salt
			var csprng = new System.Security.Cryptography.RNGCryptoServiceProvider();

			byte[] salt = new byte[SaltByteSize];

			csprng.GetBytes(salt);

			return string.Format("{0}:{1}", interations, Convert.ToBase64String(salt));
		}


		public string GeneratedHashedValue(string value, string valueSalt)
		{
			var saltParts = valueSalt.Split(new[] { ':' }, StringSplitOptions.None);

			var iterations = saltParts[0].ToInt();

			var salt = Convert.FromBase64String(saltParts[1]);

			var hash = PBKDF2(value, salt, iterations, HashByteSize);

			return Convert.ToBase64String(hash);
		}

		public bool ValidateValue(string value, string valueSalt, string hashedValue)
		{
			var saltParts = valueSalt.Split(new[] {':'}, StringSplitOptions.None);

			var iterations = saltParts[0].ToInt();

			var salt = Convert.FromBase64String(saltParts[1]);

			var hash = Convert.FromBase64String(hashedValue);

			var testHash = PBKDF2(value, salt, iterations, hash.Length);

			return SlowEquals(hash, testHash);
		}

		/// <summary>
		/// Compares two byte arrays in length-constant time. This comparison
		/// method is used so that password hashes cannot be extracted from
		/// on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte array.</param>
		/// <param name="b">The second byte array.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private bool SlowEquals(byte[] a, byte[] b)
		{
			var diff = (uint) a.Length ^ (uint) b.Length;
			
			for (var i = 0; i < a.Length && i < b.Length; i++)
			{
				diff |= (uint) (a[i] ^ b[i]);
			}
			
			return diff == 0;
		}

		/// <summary>
		/// Computes the PBKDF2-SHA1 hash of a password.
		/// </summary>
		/// <param name="value">The password to hash.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The PBKDF2 iteration count.</param>
		/// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
		/// <returns>A hash of the password.</returns>
		private byte[] PBKDF2(string value, byte[] salt, int iterations, int outputBytes)
		{
			var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(value, salt)
			{
				IterationCount = iterations
			};

			return pbkdf2.GetBytes(outputBytes);
		}
	}
}