using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi;

namespace ISI.Extensions.Scm
{
	public partial class SourceControlRepositoryApi
	{
		private static ISourceControlRepositoryApi[] _sourceControlRepositoryApis = null;
		protected ISourceControlRepositoryApi[] SourceControlRepositoryApis => _sourceControlRepositoryApis ??= GetSourceControlRepositoryApis();

		private ISourceControlRepositoryApi[] GetSourceControlRepositoryApis()
		{
			var sourceControlRepositoryApiTypes = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes(typeof(ISourceControlRepositoryApi));

			return sourceControlRepositoryApiTypes.ToNullCheckedArray(sourceControlRepositoryApiType => Activator.CreateInstance(sourceControlRepositoryApiType, new object[] { Logger }) as ISourceControlRepositoryApi, NullCheckCollectionResult.Empty);
		}

		private ISourceControlRepositoryApi GetSourceControlRepositoryApi(Guid sourceControlRepositoryTypeUuid)
		{
			foreach (var sourceControlRepositoryApi in SourceControlRepositoryApis)
			{
				if (sourceControlRepositoryApi.SourceControlRepositoryTypeUuid == sourceControlRepositoryTypeUuid)
				{
					return sourceControlRepositoryApi;
				}
			}

			return null;
		}
	}
}