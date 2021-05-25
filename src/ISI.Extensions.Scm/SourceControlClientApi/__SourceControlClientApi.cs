using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Scm
{
	public partial class SourceControlClientApi : ISourceControlClientApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public SourceControlClientApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}

		public Guid SourceControlTypeUuid => throw new NotImplementedException();
	}
}