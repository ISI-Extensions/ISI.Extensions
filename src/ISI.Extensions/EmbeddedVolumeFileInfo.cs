#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public class EmbeddedVolumeFileInfo : Microsoft.Extensions.FileProviders.IFileInfo
	{
		public System.Reflection.Assembly ResourceAssembly { get; }
		public string ResourceName { get; }
		public string LocalFileName { get; }

		public EmbeddedVolumeFileInfo(
			System.Reflection.Assembly resourceAssembly,
			string resourceName,
			string localFileName)
		{
			ResourceAssembly = resourceAssembly;
			ResourceName = resourceName;
			LocalFileName = localFileName;
		}

		public string Name => ResourceName;

		public bool IsDirectory => false;

		public string PhysicalPath
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(LocalFileName) && System.IO.File.Exists(LocalFileName))
				{
					return LocalFileName;
				}

				return null;
			}
		}

		public bool Exists => true;

		public long Length
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(LocalFileName) && System.IO.File.Exists(LocalFileName))
				{
					return (new System.IO.FileInfo(LocalFileName))?.Length ?? 0;
				}

				if (!string.IsNullOrWhiteSpace(ResourceName))
				{
					using (var stream = ResourceAssembly.GetManifestResourceStream(ResourceName))
					{
						return stream.Length;
					}
				}

				return 0;
			}
		}

		public DateTimeOffset LastModified
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(LocalFileName) && System.IO.File.Exists(LocalFileName))
				{
					return System.IO.File.GetLastWriteTimeUtc(LocalFileName);
				}

				if (!string.IsNullOrWhiteSpace(ResourceName))
				{
					var version = ResourceAssembly.GetName().Version;

					return new DateTime(2000, 2, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
				}

				return DateTime.UtcNow;
			}
		}
		
		public System.IO.Stream CreateReadStream()
		{
			if (!string.IsNullOrWhiteSpace(LocalFileName) && System.IO.File.Exists(LocalFileName))
			{
				var stream = new System.IO.MemoryStream();

				using (var fileStream = System.IO.File.OpenRead(LocalFileName))
				{
					fileStream.CopyTo(stream);
				}

				stream.Rewind();

				return stream;
			}
				
			if (!string.IsNullOrWhiteSpace(ResourceName))
			{
				return ResourceAssembly.GetManifestResourceStream(ResourceName);
			}

			return null;
		}
	}
}
