using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.GoDaddy.DataTransferObjects
{
	public abstract class AbstractRequest
	{
		public string Url { get; set; }
		public string ApiKey { get; set; }
		public string ApiSecret { get; set; }
	}
}
