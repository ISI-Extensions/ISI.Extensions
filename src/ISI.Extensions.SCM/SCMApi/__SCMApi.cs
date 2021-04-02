using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SCMApi;

namespace ISI.Extensions.Scm
{
	public partial class SCMApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public SCMApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}
	}
}