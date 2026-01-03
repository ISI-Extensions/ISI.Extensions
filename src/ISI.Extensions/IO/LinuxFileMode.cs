#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public partial class IO
	{
		//http://minnie.tuhs.org/cgi-bin/utree.pl?file=4.4BSD/usr/include/sys/stat.h
		
		[Flags]
		public enum LinuxFileMode : uint
		{
			None = 0,

			UserCanRead = 0x0000400,
			UserCanWrite = 0x0000200,
			UserCanExecute = 0x0000100,

			GroupCanRead = 0x0000040,
			GroupCanWrite = 0x0000020,
			GroupCanExecute = 0x0000010,

			OthersCanRead = 0x0000004,
			OthersCanWrite = 0x0000002,
			OthersCanExecute = 0x0000001,

			LinuxPermissionsMask = 0x0000FFF,

			FIFO = 0x0010000,
			CharSpecial = 0x0020000,
			Directory = 0x0040000,
			BlockSpecial = 0x0060000,
			File = 0x0100000,
			SymbolicLink = 0x0120000,
			Socket = 0x0140000,

			LinuxFileTypeMask = 0x0170000,
		}
	}
}