#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Platforms.ServiceApplication.DataTransferObjects.ServiceConfigurator;

namespace ISI.Platforms.ServiceApplication
{
	public class WindowsServiceConfigurator : IServiceConfigurator
	{
		public const string LocalServiceUserName = "NT AUTHORITY\\LocalService";
		public const string NetworkServiceUserName = "NT AUTHORITY\\NetworkService";


		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenSCManager(string machineName, string databaseName, int access);

		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreateService(IntPtr databaseHandle, string serviceName, string displayName, int access, int serviceType, int startType, int errorControl, string binaryPath, string loadOrderGroup, IntPtr pTagId, string dependencies, string servicesStartName, string password);

		[System.Runtime.InteropServices.DllImport("advapi32.dll")]
		public static extern void CloseServiceHandle(IntPtr SCHANDLE);

		[System.Runtime.InteropServices.DllImport("advapi32.dll")]
		public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);

		[System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
		public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);

		[System.Runtime.InteropServices.DllImport("advapi32.dll")]
		public static extern int DeleteService(IntPtr SVHANDLE);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern int GetLastError();

		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
		public struct SERVICE_DELAYED_AUTOSTART_INFO
		{
			public bool fDelayedAutostart;
		}

		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
		public struct SERVICE_DESCRIPTION
		{
			public IntPtr description;
		}

		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
		public static extern bool ChangeServiceConfig2(IntPtr serviceHandle, uint infoLevel, ref SERVICE_DESCRIPTION serviceDesc);

		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
		public static extern bool ChangeServiceConfig2(IntPtr serviceHandle, uint infoLevel, ref SERVICE_DELAYED_AUTOSTART_INFO serviceDesc);


		static int SC_MANAGER_CREATE_SERVICE = 0x0002;

		static int STANDARD_RIGHTS_REQUIRED = 0xF0000;
		static int SERVICE_QUERY_CONFIG = 0x0001;
		static int SERVICE_CHANGE_CONFIG = 0x0002;
		static int SERVICE_QUERY_STATUS = 0x0004;
		static int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
		static int SERVICE_START = 0x0010;
		static int SERVICE_STOP = 0x0020;
		static int SERVICE_PAUSE_CONTINUE = 0x0040;
		static int SERVICE_INTERROGATE = 0x0080;
		static int SERVICE_USER_DEFINED_CONTROL = 0x0100;
		static int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG | SERVICE_QUERY_STATUS | SERVICE_ENUMERATE_DEPENDENTS | SERVICE_START | SERVICE_STOP | SERVICE_PAUSE_CONTINUE | SERVICE_INTERROGATE | SERVICE_USER_DEFINED_CONTROL);

		static int SERVICE_ADAPTER = 0x00000004;
		static int SERVICE_FILE_SYSTEM_DRIVER = 0x00000002;
		static int SERVICE_KERNEL_DRIVER = 0x00000001;
		static int SERVICE_RECOGNIZER_DRIVER = 0x00000008;
		static int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
		static int SERVICE_WIN32_SHARE_PROCESS = 0x00000020;

		static int SERVICE_ERROR_CRITICAL = 0x00000003;
		static int SERVICE_ERROR_IGNORE = 0x00000000;
		static int SERVICE_ERROR_NORMAL = 0x00000001;
		static int SERVICE_ERROR_SEVERE = 0x00000002;

		static int SERVICE_AUTO_START = 0x00000002;
		static int SERVICE_BOOT_START = 0x00000000;
		static int SERVICE_DEMAND_START = 0x00000003;
		static int SERVICE_DISABLED = 0x00000004;
		static int SERVICE_SYSTEM_START = 0x00000001;


		private System.ServiceProcess.ServiceController GetServiceController(string serviceName)
		{
			return System.ServiceProcess.ServiceController.GetServices().FirstOrDefault(serviceController => string.Equals(serviceController.ServiceName, serviceName)) ?? System.ServiceProcess.ServiceController.GetServices().FirstOrDefault(s => string.Equals(s.DisplayName, serviceName));
		}

		public DTOs.StartServiceResponse StartService(DTOs.StartServiceRequest request)
		{
			var response = new DTOs.StartServiceResponse();

			var service = GetServiceController(request.ServiceName);

			response.ServiceFound = (service != null);
			if (response.ServiceFound)
			{
				if (service.Status is System.ServiceProcess.ServiceControllerStatus.Stopped or System.ServiceProcess.ServiceControllerStatus.Paused)
				{
					System.Console.WriteLine("Starting Service");

					service.Start();
					service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running, request.WaitForDuration);
				}

				service = GetServiceController(request.ServiceName);
				response.Success = (service.Status == System.ServiceProcess.ServiceControllerStatus.Running);

				if (response.Success)
				{
					System.Console.WriteLine("Started Service");
				}
			}
			else
			{
				System.Console.WriteLine("Service Not found while trying to start");
			}

			return response;
		}

		public DTOs.StopServiceResponse StopService(DTOs.StopServiceRequest request)
		{
			var response = new DTOs.StopServiceResponse();

			var service = GetServiceController(request.ServiceName);
			response.ServiceFound = (service != null);
			if (response.ServiceFound)
			{
				if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
				{
					System.Console.WriteLine("Stoping Service");

					service.Stop();
					service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, request.WaitForDuration);
				}

				service = GetServiceController(request.ServiceName);
				response.Success = (service.Status == System.ServiceProcess.ServiceControllerStatus.Stopped);

				if (response.Success)
				{
					System.Console.WriteLine("Stopped Service");
				}
			}
			else
			{
				System.Console.WriteLine("Service Not found while trying to stop");
			}

			return response;
		}

		public DTOs.InstallServiceResponse InstallService(DTOs.InstallServiceRequest request)
		{
			var response = new DTOs.InstallServiceResponse();

			System.Console.WriteLine($"ServiceName => {request.ServiceName}");

			System.Console.WriteLine("Install Service");
			
			var scmManagerPtr = IntPtr.Zero;
			var createServicePtr = IntPtr.Zero;

			try
			{
				scmManagerPtr = OpenSCManager(null, null, 983103);
				if (scmManagerPtr == IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot Open SCManager", new System.ComponentModel.Win32Exception());
				}

				var userName = string.IsNullOrWhiteSpace(request.UserName) ? LocalServiceUserName : request.UserName;

				createServicePtr = CreateService(scmManagerPtr, request.ServiceName, request.DisplayName, SERVICE_ALL_ACCESS, SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START, SERVICE_ERROR_NORMAL, $"\"{request.Executable}\"", null, IntPtr.Zero, string.Empty, userName, request.Password);
				if (createServicePtr == IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot Create Service", new System.ComponentModel.Win32Exception());
				}

				if (!string.IsNullOrWhiteSpace(request.DisplayName))
				{
					var serviceDescription = default(SERVICE_DESCRIPTION);
					serviceDescription.description = System.Runtime.InteropServices.Marshal.StringToHGlobalUni(request.DisplayName);
					var changeServiceDescription = ChangeServiceConfig2(createServicePtr, 1u, ref serviceDescription);
					System.Runtime.InteropServices.Marshal.FreeHGlobal(serviceDescription.description);
					if (!changeServiceDescription)
					{
						throw new System.ComponentModel.Win32Exception();
					}
				}
				if (request.DelayStart)
				{
					var setDelayedStart = default(SERVICE_DELAYED_AUTOSTART_INFO);
					setDelayedStart.fDelayedAutostart = true;
					if (!ChangeServiceConfig2(createServicePtr, 3u, ref setDelayedStart))
					{
						throw new System.ComponentModel.Win32Exception();
					}
				}

				System.Console.WriteLine("Installed Service");
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				if (createServicePtr != IntPtr.Zero)
				{
					CloseServiceHandle(createServicePtr);
				}
				if (scmManagerPtr != IntPtr.Zero)
				{
					CloseServiceHandle(scmManagerPtr);
				}
			}

			return response;
		}

		public DTOs.UnInstallServiceResponse UnInstallService(DTOs.UnInstallServiceRequest request)
		{
			var response = new DTOs.UnInstallServiceResponse();

			System.Console.WriteLine($"ServiceName => {request.ServiceName}");

			StopService(new()
			{
				ServiceName = request.ServiceName,
			});

			System.Console.WriteLine("Uninstall Service");
			
			var scmManagerPtr = IntPtr.Zero;
			var openServicePtr = IntPtr.Zero;

			try
			{
				scmManagerPtr = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
				if (scmManagerPtr == IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot Open SCManager", new System.ComponentModel.Win32Exception());
				}

				openServicePtr = OpenService(scmManagerPtr, request.ServiceName, 65536);
				if (openServicePtr == IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot Open Service", new System.ComponentModel.Win32Exception());
				}

				DeleteService(openServicePtr);

				System.Console.WriteLine("Uninstalled Service");
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				if (openServicePtr != IntPtr.Zero)
				{
					CloseServiceHandle(openServicePtr);
				}
				if (scmManagerPtr != IntPtr.Zero)
				{
					CloseServiceHandle(scmManagerPtr);
				}
			}

			return response;
		}
	}
}
