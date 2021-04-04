#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
	public class DirectoryIcon
	{
		[System.Runtime.InteropServices.DllImportAttribute("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		static extern IntPtr SendMessageTimeout(int windowHandle, int msg, int wParam, string lParam, SendMessageTimeoutFlags flags, int timeout, out int result);

		[Flags]
		enum SendMessageTimeoutFlags : uint
		{
			SMTO_NORMAL = 0x0,
			SMTO_BLOCK = 0x1,
			SMTO_ABORTIFHUNG = 0x2,
			SMTO_NOTIMEOUTIFNOTHUNG = 0x8
		}

		static void RefreshIconCache()
		{
			SendMessageTimeout(0xffff, 0x001A, 0, "", SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 5000, out var res);

			System.Threading.Thread.Sleep(15000);
		}
		public static void SetDirectoryIcon(string directoryFullName, string iconFullName, int iconIndex = 0)
		{
			var iconFileName = System.IO.Path.GetFileName(iconFullName);

			var desktopIniFullName = System.IO.Path.Combine(directoryFullName, "desktop.ini");
			System.IO.File.WriteAllText(desktopIniFullName, string.Format("[.ShellClassInfo]\nIconFile={0}\nIconIndex={1}\nConfirmFileOp=0\nIconResource={0},{1}\n[ViewState]\nMode=\nVid=\nFolderType=Generic", iconFileName, iconIndex));
			System.IO.File.SetAttributes(desktopIniFullName, System.IO.File.GetAttributes(desktopIniFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.Archive | System.IO.FileAttributes.System);
			//System.IO.File.SetAttributes(desktopIniFullName, System.IO.File.GetAttributes(desktopIniFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.ReadOnly);

			var inconDirectoryIconFullName = System.IO.Path.Combine(directoryFullName, iconFileName);
			if (string.IsNullOrWhiteSpace(System.IO.Path.GetDirectoryName(iconFullName)))
			{
				iconFullName = inconDirectoryIconFullName;
			}
			if (!string.Equals(directoryFullName.TrimEnd(System.IO.Path.DirectorySeparatorChar), System.IO.Path.GetDirectoryName(iconFullName).TrimEnd(System.IO.Path.DirectorySeparatorChar), StringComparison.InvariantCultureIgnoreCase))
			{
				System.IO.File.Copy(iconFullName, inconDirectoryIconFullName);
			}
			//System.IO.File.SetAttributes(inconDirectoryIconFullName, System.IO.File.GetAttributes(inconDirectoryIconFullName) | System.IO.FileAttributes.Hidden);
			//System.IO.File.SetAttributes(inDirectoryIconFullName, System.IO.File.GetAttributes(inDirectoryIconFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.ReadOnly);


			RefreshIconCache();
		}
	}
}
