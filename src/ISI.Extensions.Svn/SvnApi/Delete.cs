using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Svn.DataTransferObjects.SvnApi;
using SourceControlClientApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Svn
{
	public partial class SvnApi
	{
		public DTOs.DeleteResponse DeletePaths(DTOs.DeleteRequest request)
		{
			var response = new DTOs.DeleteResponse()
			{
				Success = true,
			};

			var fullNames = request.FullNames.ToNullCheckedArray(System.IO.Path.GetFullPath, NullCheckCollectionResult.Empty);

			foreach (var fullName in fullNames)
			{
				var arguments = new List<string>();

				arguments.Add("delete");
				arguments.Add(string.Format("\"{0}\"", fullName));

				ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = new NullLogger(),
					ProcessExeFullName = "svn",
					Arguments = arguments.ToArray(),
				});
			}

			return response;
		}
	}
}