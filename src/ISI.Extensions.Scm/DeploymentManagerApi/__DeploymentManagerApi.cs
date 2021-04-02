using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi;

namespace ISI.Extensions.Scm
{
	public partial class DeploymentManagerApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public DeploymentManagerApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}
	}
}