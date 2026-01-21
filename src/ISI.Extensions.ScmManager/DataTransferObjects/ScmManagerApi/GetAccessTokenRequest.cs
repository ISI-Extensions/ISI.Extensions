using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi
{
	public class GetAccessTokenRequest : IRequestWithScmManagerApiUrl
	{
		public string ScmManagerApiUrl { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
	}
}