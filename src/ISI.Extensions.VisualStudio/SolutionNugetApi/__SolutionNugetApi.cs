using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionNugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionNugetApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public SolutionNugetApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}
	}
}