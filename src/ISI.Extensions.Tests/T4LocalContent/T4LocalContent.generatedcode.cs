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
			return (__root ??= ISI.Extensions.FileProviders.EmbeddedVolumesFileProvider.GetPathPrefix(typeof(T4Files)));
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



