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
	public class DirectoryIcon
	{
		[System.Runtime.InteropServices.DllImport("shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
		private static extern void SHChangeNotify(int wEventId, int uFlags, IntPtr dwItem1, IntPtr dwItem2);

		public static void SetDirectoryIcon(string directoryFullName, string iconFullName, int iconIndex = 0)
		{
			var iconFileName = System.IO.Path.GetFileName(iconFullName);

			var desktopIniFullName = System.IO.Path.Combine(directoryFullName, "desktop.ini");

			using (var streamWriter = new System.IO.StreamWriter(desktopIniFullName, false, Encoding.Unicode))
			{
				streamWriter.WriteLine("[.ShellClassInfo]");
				streamWriter.WriteLine($"IconFile={iconFileName}");
				streamWriter.WriteLine($"IconIndex={iconIndex}");
				streamWriter.WriteLine("ConfirmFileOp=0");
				streamWriter.WriteLine($"IconResource={iconFileName},{iconIndex}");

				streamWriter.Flush();
				streamWriter.Close();
			}

			System.IO.File.SetAttributes(desktopIniFullName, System.IO.File.GetAttributes(desktopIniFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.Archive | System.IO.FileAttributes.System);
			System.IO.File.SetAttributes(directoryFullName, System.IO.File.GetAttributes(directoryFullName) | System.IO.FileAttributes.System);






			//System.IO.File.WriteAllText(desktopIniFullName, string.Format("[.ShellClassInfo]\nIconResource={0},{1}\n[ViewState]\nMode=\nVid=\nFolderType=Generic", iconFileName, iconIndex));
			//System.IO.File.WriteAllText(desktopIniFullName, string.Format("[.ShellClassInfo]\nIconFile={0}\nIconIndex={1}\nConfirmFileOp=0\nIconResource={0},{1}\n[ViewState]\nMode=\nVid=\nFolderType=Generic", iconFileName, iconIndex));
			//System.IO.File.SetAttributes(desktopIniFullName, System.IO.File.GetAttributes(desktopIniFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.Archive | System.IO.FileAttributes.System);
			//System.IO.File.SetAttributes(desktopIniFullName, System.IO.File.GetAttributes(desktopIniFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.ReadOnly);

			var iconDirectoryIconFullName = System.IO.Path.Combine(directoryFullName, iconFileName);
			if (string.IsNullOrWhiteSpace(System.IO.Path.GetDirectoryName(iconFullName)))
			{
				iconFullName = iconDirectoryIconFullName;
			}
			if (!string.Equals(directoryFullName.TrimEnd(System.IO.Path.DirectorySeparatorChar), System.IO.Path.GetDirectoryName(iconFullName).TrimEnd(System.IO.Path.DirectorySeparatorChar), StringComparison.InvariantCultureIgnoreCase))
			{
				System.IO.File.Copy(iconFullName, iconDirectoryIconFullName);
			}
			//System.IO.File.SetAttributes(iconDirectoryIconFullName, System.IO.File.GetAttributes(iconDirectoryIconFullName) | System.IO.FileAttributes.Hidden);
			//System.IO.File.SetAttributes(iconDirectoryIconFullName, System.IO.File.GetAttributes(iconDirectoryIconFullName) | System.IO.FileAttributes.Hidden | System.IO.FileAttributes.ReadOnly);

			SHChangeNotify(0x08000000, 0x0000, (IntPtr)null, (IntPtr)null);
		}
	}
}
