using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.VisualStudioCode.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudioCode
{
	public partial class SolutionApi 
	{
		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }
		protected ISI.Extensions.Scm.BuildScriptApi BuildScriptApi { get; }
		protected ISI.Extensions.Scm.SourceControlClientApi SourceControlClientApi { get; }

		public SolutionApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			ISI.Extensions.Scm.BuildScriptApi buildScriptApi,
			ISI.Extensions.Scm.SourceControlClientApi sourceControlClientApi)
		{
			Configuration = configuration;
			Logger = logger;
			JsonSerializer = jsonSerializer;
			BuildScriptApi = buildScriptApi;
			SourceControlClientApi = sourceControlClientApi;
		}
	}
}