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
using ISI.Extensions.Extensions;
using NUnit.Framework;

namespace ISI.Extensions.Tests.EmbeddedFileProvider
{
	[TestFixture]
	public class EmbeddedFileProvider_Tests
	{
		[Test]
		public void List_Directory_Test()
		{
			var fileProvider = new Microsoft.Extensions.FileProviders.EmbeddedFileProvider(this.GetType().Assembly);

			foreach (var content in fileProvider.GetDirectoryContents(""))
			{
				Console.WriteLine(content.Name);
				Console.WriteLine(content.Exists);
				Console.WriteLine();
			}
		}

		[Test]
		public void EmbeddedVolumesFileProvider_Test()
		{
			ISI.Extensions.StartUp.Start();
			ISI.Extensions.VirtualFileVolumesFileProvider.RegisterEmbeddedVolume(typeof(EmbeddedFileProvider_Tests));

			var fileProvider = new ISI.Extensions.VirtualFileVolumesFileProvider();

			var fileInfo = fileProvider.GetFileInfo(T4Files.EmbeddedFileProvider.Documents.TextFile1_txt);

			Console.WriteLine(fileInfo.Name);
			Console.WriteLine(fileInfo.Exists);
			Console.WriteLine(fileInfo.Length);
			Console.WriteLine(fileInfo.LastModified.Formatted(DateTimeExtensions.DateTimeFormat.DateTime));
			Console.WriteLine(fileInfo.CreateReadStream().TextReadToEnd());
			Console.WriteLine();
		}
	}
}
