using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.GoDaddy.DataTransferObjects
{
	public abstract class AbstractResponse
	{
		public Error Error { get; set; }
	}
}
