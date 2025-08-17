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

namespace ISI.Extensions.Extensions
{
	public static class LinuxFileModeExtensions
	{
		public static string Formatted(this ISI.Extensions.IO.LinuxFileMode linuxFileMode)
		{
			var response = new StringBuilder();

			char hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode fileMode, char trueValue, char falseValue = '-')
			{
				return ((linuxFileMode & fileMode) == fileMode ? trueValue : falseValue);
			}

			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.Directory, 'd', hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.SymbolicLink, 'l')));

			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.UserCanRead, 'r'));
			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.UserCanWrite, 'w'));
			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.UserCanExecute, 'x'));

			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.GroupCanRead, 'r'));
			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.GroupCanWrite, 'w'));
			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.GroupCanExecute, 'x'));

			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.OthersCanRead, 'r'));
			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.OthersCanWrite, 'w'));
			response.Append(hasLinuxFileMode(ISI.Extensions.IO.LinuxFileMode.OthersCanExecute, 'x'));

			return response.ToString();
		}
	}
}
