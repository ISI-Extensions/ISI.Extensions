using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.WindowsServices
{
	public partial class WindowsServiceManager
	{
		public string ServiceName { get; }
		public TimeSpan StartTimeOut { get; }
		public TimeSpan StopTimeOut { get; }

		public WindowsServiceManager(
			string serviceName,
			TimeSpan? startTimeOut = null,
			TimeSpan? stopTimeOut = null)
		{
			ServiceName = serviceName;
			StartTimeOut = startTimeOut ?? new TimeSpan(30000);
			StopTimeOut = stopTimeOut ?? new TimeSpan(30000);
		}
	}
}