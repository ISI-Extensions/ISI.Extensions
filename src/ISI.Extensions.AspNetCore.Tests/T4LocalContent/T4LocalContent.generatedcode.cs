// <auto-generated />
// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#region ISI.Extensions.AspNetCore.Tests.T4Files
namespace ISI.Extensions.AspNetCore.Tests
{
	[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
	public static partial class T4Files
	{
		public static string __root = "~/";
		public static string _root()
		{
			return (__root ??= ISI.Extensions.VirtualFileVolumesFileProvider.GetPathPrefix(typeof(T4Files)));
		}
	
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class JavaScripts
		{
			public static string _root()
			{
				return string.Format("{0}JavaScripts/", T4Files._root());
			}
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class _Shared
			{
				public static string _root()
				{
					return string.Format("{0}_Shared/", T4Files.JavaScripts._root());
				}
			
				public static readonly string _Layout_csjs = _root() + "_Layout.csjs";
			}
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Public
			{
				public static string _root()
				{
					return string.Format("{0}Public/", T4Files.JavaScripts._root());
				}
			
				public static readonly string _Layout_csjs = _root() + "_Layout.csjs";
				public static readonly string Index_csjs = _root() + "Index.csjs";
			}
		}
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class StyleSheets
		{
			public static string _root()
			{
				return string.Format("{0}StyleSheets/", T4Files._root());
			}
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class _Shared
			{
				public static string _root()
				{
					return string.Format("{0}_Shared/", T4Files.StyleSheets._root());
				}
			
				public static readonly string _Layout_csless = _root() + "_Layout.csless";
			}
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Public
			{
				public static string _root()
				{
					return string.Format("{0}Public/", T4Files.StyleSheets._root());
				}
			
				public static readonly string _Layout_csless = _root() + "_Layout.csless";
				public static readonly string Index_csless = _root() + "Index.csless";
			}
		}
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class Views
		{
			public static string _root()
			{
				return string.Format("{0}Views/", T4Files._root());
			}
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class _Shared
			{
				public static string _root()
				{
					return string.Format("{0}_Shared/", T4Files.Views._root());
				}
			
				public static readonly string _Layout_cshtml = _root() + "_Layout.cshtml";
			}
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Public
			{
				public static string _root()
				{
					return string.Format("{0}Public/", T4Files.Views._root());
				}
			
				public static readonly string _Layout_cshtml = _root() + "_Layout.cshtml";
				public static readonly string Index_cshtml = _root() + "Index.cshtml";
			}
		}
	}
}
#endregion

#region ISI.Extensions.AspNetCore.Tests.T4Links
namespace ISI.Extensions.AspNetCore.Tests
{
	[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
	public static partial class T4Links
	{
		private static Func<string, bool, global::ISI.Extensions.AspNetCore.IContentUrl> _getContentUrl = null;
		public static Func<string, bool, global::ISI.Extensions.AspNetCore.IContentUrl> GetContentUrl 
		{
			get => _getContentUrl ??= (url, isOriginalUrl) => new global::ISI.Extensions.AspNetCore.ContentUrl(url, isOriginalUrl);
			set => _getContentUrl = value;
		}
	
		[ISI.Extensions.AspNetCore.ContentUrlGeneratorSetter]
		public class ContentUrlGeneratorSetter : global::ISI.Extensions.AspNetCore.IContentUrlGeneratorSetter
		{
			public void SetContentUrlGenerator(Func<string, bool, global::ISI.Extensions.AspNetCore.IContentUrl> setter)
			{
				GetContentUrl = setter;
			}
		}
	
		public static string __root = "~/";
		public static string _root()
		{
			return (__root ??= ISI.Extensions.VirtualFileVolumesFileProvider.GetPathPrefix(typeof(T4Links)));
		}
	
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class JavaScripts
		{
			public static string _root()
			{
				return string.Format("{0}JavaScripts/", T4Links._root());
			}
		
			private static global::ISI.Extensions.AspNetCore.IContentUrl _directory = null;
			public static global::ISI.Extensions.AspNetCore.IContentUrl _Directory => _directory ??= GetContentUrl(T4Links._root() + "JavaScripts", true);
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class _Shared
			{
				public static string _root()
				{
					return string.Format("{0}_Shared/", T4Links.JavaScripts._root());
				}
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl _directory = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Directory => _directory ??= GetContentUrl(T4Links.JavaScripts._root() + "_Shared", true);
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl __Layout_csjs = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Layout_csjs => __Layout_csjs ??= GetContentUrl(_root() + "_Layout.csjs", true);
			}
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Public
			{
				public static string _root()
				{
					return string.Format("{0}Public/", T4Links.JavaScripts._root());
				}
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl _directory = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Directory => _directory ??= GetContentUrl(T4Links.JavaScripts._root() + "Public", true);
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl __Layout_csjs = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Layout_csjs => __Layout_csjs ??= GetContentUrl(_root() + "_Layout.csjs", true);
				private static global::ISI.Extensions.AspNetCore.IContentUrl _Index_csjs = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl Index_csjs => _Index_csjs ??= GetContentUrl(_root() + "Index.csjs", true);
			}
		}
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class StyleSheets
		{
			public static string _root()
			{
				return string.Format("{0}StyleSheets/", T4Links._root());
			}
		
			private static global::ISI.Extensions.AspNetCore.IContentUrl _directory = null;
			public static global::ISI.Extensions.AspNetCore.IContentUrl _Directory => _directory ??= GetContentUrl(T4Links._root() + "StyleSheets", true);
		
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class _Shared
			{
				public static string _root()
				{
					return string.Format("{0}_Shared/", T4Links.StyleSheets._root());
				}
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl _directory = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Directory => _directory ??= GetContentUrl(T4Links.StyleSheets._root() + "_Shared", true);
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl __Layout_csless = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Layout_csless => __Layout_csless ??= GetContentUrl(_root() + "_Layout.csless", true);
			}
			[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
			public static partial class Public
			{
				public static string _root()
				{
					return string.Format("{0}Public/", T4Links.StyleSheets._root());
				}
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl _directory = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Directory => _directory ??= GetContentUrl(T4Links.StyleSheets._root() + "Public", true);
			
				private static global::ISI.Extensions.AspNetCore.IContentUrl __Layout_csless = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl _Layout_csless => __Layout_csless ??= GetContentUrl(_root() + "_Layout.csless", true);
				private static global::ISI.Extensions.AspNetCore.IContentUrl _Index_csless = null;
				public static global::ISI.Extensions.AspNetCore.IContentUrl Index_csless => _Index_csless ??= GetContentUrl(_root() + "Index.csless", true);
			}
		}
	}
}
#endregion
