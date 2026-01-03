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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions
{
	public partial class IO
	{
		//https://github.com/arcdev/UnblockFiles
		/*
		Copyright (c) 2017 arcdev
	
		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:
	
		The above copyright notice and this permission notice shall be included in all
		copies or substantial portions of the Software.
	
		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
		SOFTWARE.	 
		*/

		public partial class FileZone
		{
			[System.Runtime.InteropServices.ComImport]
			[System.Runtime.InteropServices.Guid("0968e258-16c7-4dba-aa86-462dd61e31a3")]
			public class PersistentZoneIdentifier
			{
			}

			/// <a href="https://msdn.microsoft.com/en-us/library/ms537032%28v=vs.85%29.aspx?f=255&amp;MSPPError=-2147217396">source</a>
			[System.Runtime.InteropServices.ComImport]
			[System.Runtime.InteropServices.Guid("cd45f185-1b21-48e2-967b-ead743a8914e")]
			[System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
			public interface IZoneIdentifier
			{
				int GetId(out URLZONE pdwZone);
				int SetId(URLZONE dwZone);
				int Remove();
			}


			/// <a href="https://msdn.microsoft.com/en-us/library/ms537175(v=vs.85).aspx">source</a>
			public enum URLZONE
			{
				INVALID = -1,
				PREDEFINED_MIN = 0,
				LOCAL_MACHINE = 0,
				INTRANET = 1,
				TRUSTED = 2,
				INTERNET = 3,
				UNTRUSTED = 4,
				PREDEFINED_MAX = 999,
				USER_MIN = 1000,
				USER_MAX = 10000
			}

			/// <a href="https://msdn.microsoft.com/en-us/library/aa380337(v=vs.85).aspx">source</a>
			[Flags]
			public enum STGM : long
			{
				//Access	
				READ = 0x00000000L,
				WRITE = 0x00000001L,
				READWRITE = 0x00000002L,

				//Sharing
				SHARE_DENY_NONE = 0x00000040L,
				SHARE_DENY_READ = 0x00000030L,
				SHARE_DENY_WRITE = 0x00000020L,
				SHARE_EXCLUSIVE = 0x00000010L,
				PRIORITY = 0x00040000L,

				//Creation
				CREATE = 0x00001000L,
				CONVERT = 0x00020000L,
				FAILIFTHERE = 0x00000000L,

				//Transactioning
				DIRECT = 0x00000000L,
				TRANSACTED = 0x00010000L,

				//Transactioning Performance
				NOSCRATCH = 0x00100000L,
				NOSNAPSHOT = 0x00200000L,

				//Direct SWMR and Simple
				SIMPLE = 0x08000000L,
				DIRECT_SWMR = 0x00400000L,

				//Delete On Release
				DELETEONRELEASE = 0x04000000L
			}

			public static string GetZone(string filename)
			{
				System.Runtime.InteropServices.ComTypes.IPersistFile persistFile = null;
				IZoneIdentifier zoneIdentifier = null;

				try
				{
					persistFile = new PersistentZoneIdentifier() as System.Runtime.InteropServices.ComTypes.IPersistFile;

					const int mode = (int)STGM.READ;

					try
					{
						persistFile.Load(filename, mode);
					}
					catch (System.IO.FileNotFoundException)
					{
						return "(none)";
					}
					catch (UnauthorizedAccessException)
					{
						return "(access denied)";
					}

					zoneIdentifier = (IZoneIdentifier)persistFile;

					zoneIdentifier.GetId(out var zone);

					return zone.ToString();
				}
				finally
				{
					if (persistFile != null)
					{
						System.Runtime.InteropServices.Marshal.ReleaseComObject(persistFile);
					}

					if (zoneIdentifier != null)
					{
						System.Runtime.InteropServices.Marshal.ReleaseComObject(zoneIdentifier);
					}
				}
			}

			public static void RemoveZone(string filename)
			{
				System.Runtime.InteropServices.ComTypes.IPersistFile persistFile = null;
				IZoneIdentifier zoneIdentifier = null;

				try
				{
					persistFile = new PersistentZoneIdentifier() as System.Runtime.InteropServices.ComTypes.IPersistFile;
					const int mode = (int)(STGM.READWRITE | STGM.SHARE_EXCLUSIVE);

					URLZONE zone;

					try
					{
						persistFile.Load(filename, mode);

						zoneIdentifier = persistFile as IZoneIdentifier;

						zoneIdentifier.GetId(out zone);
					}
					catch (System.IO.FileNotFoundException)
					{
						zone = URLZONE.LOCAL_MACHINE;
					}
					catch (UnauthorizedAccessException)
					{
						zone = URLZONE.INVALID;
					}

					if (zone is URLZONE.LOCAL_MACHINE or URLZONE.INVALID)
					{
						Console.WriteLine($"Nothing to remove on '{filename}'");
						return;
					}

					zoneIdentifier.Remove();

					persistFile.Save(filename, true);
				}
				finally
				{
					if (persistFile != null)
					{
						System.Runtime.InteropServices.Marshal.ReleaseComObject(persistFile);
					}

					if (zoneIdentifier != null)
					{
						System.Runtime.InteropServices.Marshal.ReleaseComObject(zoneIdentifier);
					}
				}
			}
		}
	}
}