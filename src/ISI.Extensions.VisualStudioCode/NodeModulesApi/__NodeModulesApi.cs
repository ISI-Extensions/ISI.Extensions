using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.VisualStudioCode.DataTransferObjects.NodeModulesApi;

namespace ISI.Extensions.VisualStudioCode
{
	public partial class NodeModulesApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected SolutionApi SolutionApi { get; }

		public NodeModulesApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			SolutionApi solutionApi)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			SolutionApi = solutionApi;
		}
	}
}