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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class Pbkdf2SaltedHashGenerator_Tests
	{
		[Test]
		public void Generate_Test()
		{
			var goodPassword = "6a3300f6-8179-4f60-99b7-bdcc3be3f6bbcd16f3e6-ddd9-4faa-95b2-c31aed9d0feb";
			var badPassword = "a47d4b7f-3da4-44a0-b1cf-1bc0c72e3da645ebae6b-ab8e-4587-80be-9854fe2f592cc7c4a6b3-2bf6-4d5f-81b4-b46ef526974b";

			var saltedHashGenerator = new ISI.Extensions.Crypto.Pbkdf2SaltedHashGenerator(null);

			var passwordSalt = saltedHashGenerator.GenerateNewCryptoSalt();

			var hashedPassword = saltedHashGenerator.GeneratedHashedValue(goodPassword, passwordSalt);

			Assert.That(saltedHashGenerator.ValidateValue(goodPassword, passwordSalt, hashedPassword));

			Assert.That(!saltedHashGenerator.ValidateValue(badPassword, passwordSalt, hashedPassword));
		}
	}
}
