using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.ServicesManagerApi;

namespace ISI.Extensions.Scm
{
	public partial class ServicesManagerApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public ServicesManagerApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}
	}
}