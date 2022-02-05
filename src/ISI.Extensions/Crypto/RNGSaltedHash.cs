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