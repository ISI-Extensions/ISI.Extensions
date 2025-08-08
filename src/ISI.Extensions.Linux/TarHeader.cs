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

namespace ISI.Extensions.Linux
{
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public unsafe struct TarHeader : IArchiveHeader
	{
		public const string UsTarMagic = "ustar";

		private static readonly string Empty8 = new string('\0', 8);

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 100)]
		private byte[] name;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] mode;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] uid;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] gid;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 12)]
		private byte[] size;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 12)]
		private byte[] mtime;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] chksum;

		private byte typeflag;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 100)]
		private byte[] linkname;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 6)]
		private byte[] magic;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 2)]
		private byte[] version;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 32)]
		private byte[] uname;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 32)]
		private byte[] gname;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] devmajor;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 8)]
		private byte[] devminor;

		[System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1, SizeConst = 155)]
		private byte[] prefix;

		public string FileName
		{
			get => GetString(name, 100);
			set => name = CreateString(value, 100);
		}

		public ISI.Extensions.IO.LinuxFileMode LinuxFileMode
		{
			get => (ISI.Extensions.IO.LinuxFileMode)Convert.ToUInt32(GetString(mode, 8), 8);
			set => mode = GetUIntTo8((uint)value);
		}

		public uint UserId
		{
			get => Convert.ToUInt32(GetString(uid, 8), 8);
			set => uid = GetUIntTo8(value);
		}

		public uint GroupId
		{
			get => Convert.ToUInt32(GetString(gid, 8), 8);
			set => gid = GetUIntTo8(value);
		}

		public uint FileSize
		{
			get => Convert.ToUInt32(GetString(size, 12), 8);
			set => size = GetUIntTo8(value, 12);
		}

		public DateTimeOffset LastModified
		{
			get => DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToUInt64(GetString(mtime, 12), 8));
			set => mtime = GetUIntTo8((uint)value.ToUnixTimeSeconds(), 12);
		}

		public uint Checksum
		{
			get => Convert.ToUInt32(GetString(chksum, 8), 8);
			set
			{
				var s = GetUIntTo8(value, 7);
				var buffer = new byte[8];
				Array.Copy(s, buffer, 7);
				buffer[7] = 32;
				chksum = buffer;
			}
		}

		public TarType TypeFlag
		{
			get => (TarType)typeflag;
			set => typeflag = (byte)value;
		}

		public string LinkName
		{
			get => GetString(linkname, 100);
			set => linkname = CreateString(value, 100);
		}

		public string Magic
		{
			get => GetString(magic, 6)?.Trim();
			set => magic = CreateString(value.PadRight(6), 6);
		}

		public uint? Version
		{
			get
			{
				var v = GetString(version, 2);
				if (uint.TryParse(v, out uint rv))
				{
					return rv;
				}

				return null;
			}

			set => version = value == null ? " \0"u8.ToArray() : GetUIntTo8(value, 2);
		}

		public string UserName
		{
			get => GetString(uname, 32);
			set => uname = CreateString(value, 32);
		}

		public string GroupName
		{
			get => GetString(gname, 32);
			set => gname = CreateString(value, 32);
		}

		public uint? DevMajor
		{
			get => devmajor[0] == 0 ? (uint?)null : Convert.ToUInt32(GetString(devmajor, 8), 8);
			set => devmajor = GetUIntTo8(value);
		}

		public uint? DevMinor
		{
			get => devminor[0] == 0 ? (uint?)null : Convert.ToUInt32(GetString(devminor, 8), 8);
			set => devminor = GetUIntTo8(value);
		}

		public string Prefix
		{
			get => GetString(prefix, 155);
			set => prefix = CreateString(value, 155);
		}

		public uint ComputeChecksum()
		{
			var other = this;
			other.chksum = new byte[8];
			for (var c = 0; c < 8; c++)
			{
				other.chksum[c] = 32;
			}

			var data = new byte[System.Runtime.InteropServices.Marshal.SizeOf<TarHeader>()];
			fixed (byte* ptr = data)
			{
				System.Runtime.InteropServices.Marshal.StructureToPtr(other, new IntPtr(ptr), true);
			}

			uint sum = 0;
			foreach (var b in data)
			{
				sum += b;
			}

			return sum;
		}

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

		private byte[] GetUIntTo8(uint? data, int len = 8)
		{
			if (data == null)
			{
				return new byte[len];
			}

			return CreateString(Convert.ToString(data.Value, 8).PadLeft(len - 1, '0'), len);
		}

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
				target[c] = (c < buffer.Length) ? buffer[c] : (byte)0;
			}

			return target;
		}
	}
}