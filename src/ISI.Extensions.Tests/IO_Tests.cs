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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class IO_Tests
	{
		[Test]
		public void GetShortPathName_Test()
		{
			var xxx = ISI.Extensions.IO.Path.GetShortPathName(@"F:\ISI\Internal Projects\ISI.FileExplorer.Extensions\tools\Addins\ISI.Extensions.VisualStudio.10.0.9037.37282\lib\netstandard2.0\signtool.exe");
		}

		[Test]
		public void Open_Test()
		{
			var sourceStream = ISI.Extensions.FileSystem.OpenRead(@"\\10.105.10.1\backup$\ISI.20241118.181054988.bak.lz");
		}

		[Test]
		public void Open2_Test()
		{
			var sourceStream = ISI.Extensions.FileSystem.OpenRead(@"E:\ISI.20241118.181054988.bak.lz");
		}

		[Test]
		public void Drives_Test()
		{
			var mosPsks = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");

			// Loop through each object (disk) retrieved by WMI

			foreach (System.Management.ManagementObject moDisk in mosPsks.Get())
			{
					foreach (var moDiskProperty in moDisk.Properties)
					{
						Console.WriteLine($"  {moDiskProperty.Name} => {moDiskProperty.Value}");
					}

					Console.WriteLine();
			}

			var mosDisks = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

			// Loop through each object (disk) retrieved by WMI

			foreach (System.Management.ManagementObject moDisk in mosDisks.Get())
			{
				if (string.Equals(moDisk["DeviceID"], @"\\.\PHYSICALDRIVE10"))
				{
					Console.WriteLine(moDisk["Model"]);
					foreach (var moDiskProperty in moDisk.Properties)
					{
						Console.WriteLine($"  {moDiskProperty.Name} => {moDiskProperty.Value}");
					}

					Console.WriteLine();
				}
			}

			foreach (var drive in System.IO.DriveInfo.GetDrives().Where(drive => drive.IsReady))
			{
				Console.WriteLine(drive.VolumeLabel);
			}

			var searcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskPartition");

			foreach (var queryObj in searcher.Get())
			{
				if ((uint)queryObj["DiskIndex"] == 10)
				{




					Console.WriteLine("-----------------------------------");
					Console.WriteLine("Win32_DiskPartition instance");

					foreach (var queryObjProperty in queryObj.Properties)
					{
						Console.WriteLine($"  {queryObjProperty.Name} => {queryObjProperty.Value}");
					}
				}
			}
		}

		[Test]
		public void GetDirectoryName_Test()
		{
			var xxx = System.IO.Path.GetDirectoryName(@"C:\Temp\Pizza");
			var cccc1c = System.IO.Path.GetFileName(@"C:\Temp\Pizza\");
			var ccccc = System.IO.Path.GetFileName(@"C:\Temp\Pizza");
		}

		[Test]
		public void xxxx_Test()
		{
			var tartFullName = @"E:\Temp\test-34\55555\baby.tab";

			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(tartFullName));

			System.IO.File.Copy(@"E:\Temp\baby.tab", tartFullName);
		}

		[Test]
		public void GetRelativePath_Test()
		{
			{
				var relativeTo = @"F:\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests";
				var path = @"F:\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\EmbeddedFiles\Views\Test.cshtml";

				Console.WriteLine(System.IO.Path.GetRelativePath(relativeTo, path));

				Console.WriteLine(ISI.Extensions.IO.Path.GetRelativePath(relativeTo, path));
			}

			{
				var relativeTo = @"F:\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\Pizza";
				var path = @"F:\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\EmbeddedFiles\Views\Test.cshtml";

				Console.WriteLine(System.IO.Path.GetRelativePath(relativeTo, path));

				Console.WriteLine(ISI.Extensions.IO.Path.GetRelativePath(relativeTo, path));
			}

			{
				var relativeTo = @"E:\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\Pizza";
				var path = @"F:\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\EmbeddedFiles\Views\Test.cshtml";

				Console.WriteLine(System.IO.Path.GetRelativePath(relativeTo, path));

				Console.WriteLine(ISI.Extensions.IO.Path.GetRelativePath(relativeTo, path));
			}

			{
				var relativeTo = @"\\Server\E$\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\Pizza";
				var path = @"\\Server\E$\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\EmbeddedFiles\Views\Test.cshtml";

				Console.WriteLine(System.IO.Path.GetRelativePath(relativeTo, path));

				Console.WriteLine(ISI.Extensions.IO.Path.GetRelativePath(relativeTo, path));
			}

			{
				var relativeTo = @"\\Server\E$\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\Pizza";
				var path = @"\\Server1\E$\ISI\ISI.FrameWork\src\ISI.Extensions\ISI.Extensions.UnitTests\EmbeddedFiles\Views\Test.cshtml";

				Console.WriteLine(System.IO.Path.GetRelativePath(relativeTo, path));

				Console.WriteLine(ISI.Extensions.IO.Path.GetRelativePath(relativeTo, path));
			}
		}

	}
}
