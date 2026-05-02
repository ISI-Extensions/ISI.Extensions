using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nginx.DataTransferObjects.NginxApi
{
	public class FindNginxConfigFileNamesRequest
	{
		public IEnumerable<string> Paths { get; set; }
	}
}