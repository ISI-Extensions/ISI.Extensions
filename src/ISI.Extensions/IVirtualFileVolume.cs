using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions
{
	public interface IVirtualFileVolume
	{
		string PathPrefix { get; }
		IVirtualFileVolumeFileInfo GetFileInfo(string path);
	}
}
