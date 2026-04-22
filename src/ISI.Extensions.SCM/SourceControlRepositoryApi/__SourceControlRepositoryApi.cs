using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi;

namespace ISI.Extensions.Scm
{
	public partial class SourceControlRepositoryApi : ISourceControlRepositoryApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public SourceControlRepositoryApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}

		public Guid SourceControlRepositoryTypeUuid => throw new NotImplementedException();
		public string Description => throw new NotImplementedException();
		public string RepositoryType => throw new NotImplementedException();
		public bool UseApiUrl => throw new NotImplementedException();
		public string ApiUrlDescription => throw new NotImplementedException();
		public bool UseApiToken => throw new NotImplementedException();
		public string ApiTokenDescription => throw new NotImplementedException();
		public bool UseRepositoryNamespace => throw new NotImplementedException();
		public string RepositoryNamespaceDescription => throw new NotImplementedException();
	}
}