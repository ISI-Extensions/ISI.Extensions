using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi
{
	public class DeleteRepositoryRequest : IRequest
	{
		public string ScmManagerApiUrl { get; set; }
		public string ScmManagerApiToken { get; set; }

		public string Namespace { get; set; }
		public string Name { get; set; }
	}
}