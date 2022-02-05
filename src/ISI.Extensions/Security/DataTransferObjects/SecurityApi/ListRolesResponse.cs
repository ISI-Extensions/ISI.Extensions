using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Security.DataTransferObjects.SecurityApi
{
	public partial class ListRolesResponse
	{
		public IEnumerable<string> Roles { get; set; }	
	}
}