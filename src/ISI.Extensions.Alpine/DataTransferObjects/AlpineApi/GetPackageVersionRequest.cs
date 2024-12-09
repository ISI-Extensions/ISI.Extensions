using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Alpine.DataTransferObjects.AlpineApi
{
	public class GetPackageVersionRequest
	{
		public string Branch { get; set; } = "edge";
		public string Repository { get; set; }
		public string Architecture { get; set; } = "x86_64";
		public string Package { get; set; }
	}
}