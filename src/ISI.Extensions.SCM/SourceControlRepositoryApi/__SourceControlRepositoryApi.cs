using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi;

namespace ISI.Extensions.Scm
{
	public partial class SourceControlRepositoryApi : ISI.Extensions.Scm.ISourceControlRepositoryApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public SourceControlRepositoryApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}

		Guid ISI.Extensions.Scm.ISourceControlRepositoryApi.SourceControlRepositoryTypeUuid => throw new NotImplementedException();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.Description => throw new NotImplementedException();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryType => throw new NotImplementedException();
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiUrl => throw new NotImplementedException();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiUrlDescription => throw new NotImplementedException();
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiToken => throw new NotImplementedException();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiTokenDescription => throw new NotImplementedException();
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseRepositoryNamespace => throw new NotImplementedException();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryNamespaceDescription => throw new NotImplementedException();
	}
}