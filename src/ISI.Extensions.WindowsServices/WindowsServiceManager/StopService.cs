using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.WindowsServices
{
	public partial class WindowsServiceManager
	{
		public void StopService()
		{
			using (var serviceController = new System.ServiceProcess.ServiceController(ServiceName))
			{
				switch ( serviceController.Status)
				{
					case System.ServiceProcess.ServiceControllerStatus.Running:
					case System.ServiceProcess.ServiceControllerStatus.Paused:
						serviceController.Stop();
						serviceController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, StopTimeOut);
						break;
					case System.ServiceProcess.ServiceControllerStatus.Stopped:
						break;
					case System.ServiceProcess.ServiceControllerStatus.StartPending:
						break;
					case System.ServiceProcess.ServiceControllerStatus.StopPending:
						break;
					case System.ServiceProcess.ServiceControllerStatus.ContinuePending:
						break;
					case System.ServiceProcess.ServiceControllerStatus.PausePending:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}