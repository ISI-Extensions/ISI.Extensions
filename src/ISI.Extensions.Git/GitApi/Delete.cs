using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Git.DataTransferObjects.GitApi;
using SourceControlClientApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Git
{
	public partial class GitApi
	{
		public DTOs.DeleteResponse Delete(DTOs.DeleteRequest request)
		{
			var response = new DTOs.DeleteResponse()
			{
				Success = true,
			};

			var fullNames = request.FullNames.ToNullCheckedArray(System.IO.Path.GetFullPath, NullCheckCollectionResult.Empty);

			foreach (var fullName in fullNames)
			{
				if (System.IO.File.Exists(fullName))
				{
					System.IO.File.Delete(fullName);
				}
				else if (System.IO.Directory.Exists(fullName))
				{
					System.IO.Directory.Delete(fullName, true);
				}
			}

			return response;
		}
	}
}