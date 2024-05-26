using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.VisualStudioCode.DataTransferObjects.NodeModulesApi
{
	public class CleanSolutionRequest
	{
		public string Solution { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}