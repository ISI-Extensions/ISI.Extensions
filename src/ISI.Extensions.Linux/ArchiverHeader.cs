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

namespace ISI.Extensions.Linux
{
	public struct ArchiverHeader : IArchiveHeader
	{
		public const string ArchiverMagic = "!<arch>\n";

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 16)]
		private byte[] fileName;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 12)]
		private byte[] lastModified;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 6)]
		private byte[] ownerId;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 6)]
		private byte[] groupId;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] fileMode;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 10)]
		private byte[] fileSize;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 2)]
		private byte[] endChar;

		/// <summary>
		/// Gets or sets the name of the current file.
		/// </summary>
		public string FileName
		{
			get => GetString(fileName, 16).Trim();
			set => fileName = CreateString(value, 16);
		}

		/// <summary>
		/// Gets or sets the date at which the current file was last modified.
		/// </summary>
		public DateTimeOffset LastModified
		{
			get => DateTimeOffset.FromUnixTimeSeconds(GetString(lastModified, 12).ToInt());
			set => lastModified = CreateString(value.ToUnixTimeSeconds().ToString(), 12);
		}

		/// <summary>
		/// Gets or sets the user ID of the owner of the file.
		/// </summary>
		public uint OwnerId
		{
			get => ReadUInt(ownerId);
			set => ownerId = CreateString(value.ToString(), 6);
		}

		/// <summary>
		/// Gets or sets group ID of the owner of the file.
		/// </summary>
		public uint GroupId
		{
			get => ReadUInt(groupId);
			set => groupId = CreateString(value.ToString(), 6);
		}

		/// <inheritdoc/>
		public ISI.Extensions.IO.LinuxFileMode LinuxFileMode
		{
			get => (ISI.Extensions.IO.LinuxFileMode)Convert.ToUInt32(GetString(fileMode, 8).Trim(), 8);
			set => fileMode = CreateString(Convert.ToString((uint)value, 8), 8);
		}

		/// <inheritdoc/>
		public uint FileSize
		{
			get => ReadUInt(fileSize);
			set => fileSize = CreateString(value.ToString(), 10);
		}

		public string EndChar
		{
			get => GetString(endChar, 2);
			set => endChar = CreateString(value, 2);
		}

		/// <inheritdoc/>
		public override string ToString() => FileName;

		private string GetString(byte[] data, int maxLen)
		{
			int len;
			for (len = 0; len < maxLen; len++)
			{
				if (data[len] == 0)
				{
					break;
				}
			}

			if (len == 0)
			{
				return null;
			}

			return Encoding.UTF8.GetString(data, 0, len);
		}

		private uint ReadUInt(byte[] data) => Convert.ToUInt32(GetString(data, data.Length).Trim());

		private byte[] CreateString(string s, int len)
		{
			var target = new byte[len];
			if (s == null)
			{
				return target;
			}

			var buffer = Encoding.UTF8.GetBytes(s);
			if (buffer.Length > len)
			{
				throw new Exception($"String {s} exceeds the limit of {len}");
			}

			for (var c = 0; c < len; c++)
			{
				target[c] = (c < buffer.Length) ? buffer[c] : (byte)0x20;
			}

			return target;
		}
	}
}
