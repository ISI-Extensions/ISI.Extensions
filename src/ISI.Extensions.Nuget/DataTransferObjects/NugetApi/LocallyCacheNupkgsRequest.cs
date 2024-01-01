using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.DataTransferObjects.NugetApi
{
	public class LocallyCacheNupkgsRequest
	{
		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }

		public IEnumerable<string> NupkgFullNames { get; set; }
	}
}