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
using System.Text;

namespace ISI.Extensions.Crypto
{
	public partial class Rijndael
	{
		public partial class String
		{
			public static string Encrypt(string cryptoKey, string unEncryptedValue)
			{
				string result = null;

				var clearBytes = System.Text.Encoding.Unicode.GetBytes(unEncryptedValue);
				var pdb = new System.Security.Cryptography.PasswordDeriveBytes(cryptoKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

				using (var ms = new System.IO.MemoryStream())
				{
					var alg = System.Security.Cryptography.Rijndael.Create();
					alg.Key = pdb.GetBytes(32);
					alg.IV = pdb.GetBytes(16);
					using (var cryptoStream = new System.Security.Cryptography.CryptoStream(ms, alg.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
					{
						cryptoStream.Write(clearBytes, 0, clearBytes.Length);
					}

					result = Convert.ToBase64String(ms.ToArray());
				}

				return result;
			}
		}
	}
}
