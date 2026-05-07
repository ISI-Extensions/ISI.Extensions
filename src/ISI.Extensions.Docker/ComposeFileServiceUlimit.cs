using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileServiceUlimit
	{
		public int? Soft { get; set; }
		public int? Hard { get; set; }
	}
}
