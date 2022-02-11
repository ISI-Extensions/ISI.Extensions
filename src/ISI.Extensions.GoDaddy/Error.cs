using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.GoDaddy
{
	public class Error
	{
		public string Code { get; set; }
		public ErrorField[] Fields { get; set; }
		public string Message { get; set; }
	}

	public class ErrorField
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public string Path { get; set; }
		public string PathRelated { get; set; }
	}
}
