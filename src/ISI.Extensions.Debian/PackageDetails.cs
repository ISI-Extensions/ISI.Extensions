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

namespace ISI.Extensions.Debian
{
	public class PackageDetails
	{
		public string Distribution { get; set; }
		public string Branch { get; set; }
		public string Architecture { get; set; }

		
		public string Package { get; set; }
		public string Replaces { get; set; }
		public string Source { get; set; }
		public string Version { get; set; }
		public string MultiArch { get; set; }
		public string Depends { get; set; }
		public string PreDepends { get; set; }
		public string Recommends { get; set; }
		public string Suggests { get; set; }
		public string Enhances { get; set; }
		public string Breaks { get; set; }
		public string Conflicts { get; set; }
		public long? InstalledSize { get; set; }
		public string Maintainer { get; set; }
		public string OriginalMaintainer { get; set; }
		public string Homepage { get; set; }
		public string Description { get; set; }
		public string DescriptionMd5sum { get; set; }
		public string Priority { get; set; }
		public string Section { get; set; }
		public string Origin { get; set; }
		public string Bugs { get; set; }
		public string Tasks { get; set; }

		public string FileName { get; set; }

		public long? FileSize { get; set; }
		public string Md5sum { get; set; }
		public string SHA1Hash { get; set; }
		public string SHA256Hash { get; set; }
		public string SHA512Hash { get; set; }
	}
}
