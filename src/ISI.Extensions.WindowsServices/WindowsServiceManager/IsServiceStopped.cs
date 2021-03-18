using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.WindowsServices
{
	public partial class WindowsServiceManager
	{
		public bool IsServiceStopped
		{
			get
			{
				using (var serviceController = new System.ServiceProcess.ServiceController(ServiceName))
				{
					return serviceController.Status == System.ServiceProcess.ServiceControllerStatus.Stopped;
				}
			}
		}
	}
}