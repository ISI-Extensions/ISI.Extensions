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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Crypto
{
	public partial class Aes
	{
		public partial class String
		{
			public static (byte[] Key, byte[] EncryptedValue) Encrypt(string unEncryptedValue, IEnumerable<byte> key = null)
			{
				using (var memoryStream = new System.IO.MemoryStream())
				{
					using (var aes = System.Security.Cryptography.Aes.Create())
					{
						if (key.NullCheckedAny())
						{
							aes.Key = key.ToArray();
						}
						else
						{
							aes.GenerateKey();
							key = aes.Key;
						}

						var iv = aes.IV;
						memoryStream.Write(iv, 0, iv.Length);

						using (var cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, aes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
						{
							using (var encryptWriter = new System.IO.StreamWriter(cryptoStream))
							{
								encryptWriter.Write(unEncryptedValue);

								encryptWriter.Flush();

								cryptoStream.FlushFinalBlock();

								memoryStream.Flush();

								memoryStream.Rewind();

								return (Key: key.ToArray(), EncryptedValue: memoryStream.ToArray());
							}
						}
					}
				}
			}
		}
	}
}
