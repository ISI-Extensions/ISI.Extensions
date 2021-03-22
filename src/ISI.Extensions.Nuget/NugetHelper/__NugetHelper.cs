using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetHelper;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetHelper
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public NugetHelper(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}
	}
}