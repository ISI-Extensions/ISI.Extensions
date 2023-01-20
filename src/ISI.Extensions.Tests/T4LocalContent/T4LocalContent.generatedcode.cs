#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 

// <auto-generated />
// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#region ISI.Extensions.Tests.T4Files
namespace ISI.Extensions.Tests
{
	[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
	public static partial class T4Files
	{
		public static string __root = null;
		public static string _root()
		{
			return (__root ??= ISI.Extensions.VirtualFileVolumesFileProvider.GetPathPrefix(typeof(T4Files)));
		}
	
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class EmbeddedFileProvider
		{
			public static string _root()
			{
				return string.Format("{0}EmbeddedFileProvider/", T4Files._root());
			}
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Documents
			{
				public static string _root()
				{
					return string.Format("{0}Documents/", T4Files.EmbeddedFileProvider._root());
				}
			
				public static readonly string TextFile1_txt = _root() + "TextFile1.txt";
			}
		}
	}
}
#endregion


#region ISI.Extensions.Tests.T4Embedded
namespace ISI.Extensions.Tests
{
	[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
	public static partial class T4Embedded
	{
		public static string _root()
		{
			return "pack://application:,,,/ISI.Extensions.Tests;component/";
		}
	
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class EmbeddedFileProvider
		{
			public static string _root()
			{
				return string.Format("{0}EmbeddedFileProvider/", T4Embedded._root());
			}
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Documents
			{
				public static string _root()
				{
					return string.Format("{0}Documents/", T4Embedded.EmbeddedFileProvider._root());
				}
			
				public static readonly string TextFile1_txt = _root() + "TextFile1.txt";
			}
		}
	}
}
#endregion

#region ISI.Extensions.Tests.T4Resources
namespace ISI.Extensions.Tests
{
	[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
	public static partial class T4Resources
	{
		public static string _root()
		{
			return "ISI.Extensions.Tests.";
		}
	
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class EmbeddedFileProvider
		{
			public static string _root()
			{
				return string.Format("{0}EmbeddedFileProvider.", T4Resources._root());
			}
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Documents
			{
				public static string _root()
				{
					return string.Format("{0}Documents.", T4Resources.EmbeddedFileProvider._root());
				}
			
				public static readonly string TextFile1_txt = _root() + "TextFile1.txt";
			}
		}
	}
}
#endregion



