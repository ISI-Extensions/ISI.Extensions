// <auto-generated />
// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#region ISI.Extensions.Jenkins.T4Files
namespace ISI.Extensions.Jenkins
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
		public static partial class Artwork
		{
			public static string _root()
			{
				return string.Format("{0}Artwork/", T4Files._root());
			}
		
			public static readonly string Jenkins_ico = _root() + "Jenkins.ico";
			public static System.IO.Stream GetJenkins_icoStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(Jenkins_ico);
			}
			public static readonly string jenkinsConfig_16x16_png = _root() + "jenkinsConfig-16x16.png";
			public static System.IO.Stream GetjenkinsConfig_16x16_pngStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(jenkinsConfig_16x16_png);
			}
			public static readonly string jenkinsConfig_32x32_png = _root() + "jenkinsConfig-32x32.png";
			public static System.IO.Stream GetjenkinsConfig_32x32_pngStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(jenkinsConfig_32x32_png);
			}
			public static readonly string JenkinsOverlay_ico = _root() + "JenkinsOverlay.ico";
			public static System.IO.Stream GetJenkinsOverlay_icoStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(JenkinsOverlay_ico);
			}
		}
	}
}
#endregion

#region ISI.Extensions.Jenkins.T4Resources
namespace ISI.Extensions.Jenkins
{
	[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
	public static partial class T4Resources
	{
		public static string _root()
		{
			return "ISI.Extensions.Jenkins.";
		}
	
		[GeneratedCode("T4LocalContent", "1.0"), DebuggerNonUserCode]
		public static partial class Artwork
		{
			public static string _root()
			{
				return string.Format("{0}Artwork.", T4Resources._root());
			}
		
			public static readonly string Jenkins_ico = _root() + "Jenkins.ico";
			public static System.IO.Stream GetJenkins_icoStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(Jenkins_ico);
			}
			public static readonly string jenkinsConfig_16x16_png = _root() + "jenkinsConfig-16x16.png";
			public static System.IO.Stream GetjenkinsConfig_16x16_pngStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(jenkinsConfig_16x16_png);
			}
			public static readonly string jenkinsConfig_32x32_png = _root() + "jenkinsConfig-32x32.png";
			public static System.IO.Stream GetjenkinsConfig_32x32_pngStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(jenkinsConfig_32x32_png);
			}
			public static readonly string JenkinsOverlay_ico = _root() + "JenkinsOverlay.ico";
			public static System.IO.Stream GetJenkinsOverlay_icoStream()
			{
				return typeof(ISI.Extensions.Jenkins.T4Resources).Assembly.GetManifestResourceStream(JenkinsOverlay_ico);
			}
		}
	}
}
#endregion



