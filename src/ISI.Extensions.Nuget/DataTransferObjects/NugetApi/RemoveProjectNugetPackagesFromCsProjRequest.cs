using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.DataTransferObjects.NugetApi
{
	public partial class RemoveProjectNugetPackagesFromCsProjRequest
	{
		public string CsProjFullName { get; set; }
		public string[] NugetPackageNames { get; set; }
	}
}