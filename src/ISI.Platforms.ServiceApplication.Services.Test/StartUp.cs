using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Platforms.ServiceApplication.Services.Test
{
	[ISI.Extensions.StartUp]
	public class StartUp : ISI.Extensions.IStartUp
	{
		private static bool _isInitialized = false;
		public void Start()
		{
			if (!_isInitialized)
			{
				_isInitialized = true;

				ISI.Extensions.VirtualFileVolumesFileProvider.RegisterEmbeddedVolume(typeof(StartUp));
			}
		}
	}
}