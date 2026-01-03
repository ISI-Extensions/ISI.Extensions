#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
	public class LinuxServiceConfigurator : IServiceConfigurator
	{
		private string GetServiceConfigFullName(string serviceName) => $"/etc/systemd/system/{serviceName}.service";

		public DTOs.StartServiceResponse StartService(DTOs.StartServiceRequest request)
		{
			var response = new DTOs.StartServiceResponse();

			System.Console.WriteLine("Starting Service");

			ISI.Extensions.Process.WaitForProcessResponse("systemctl", "start", $"{request.ServiceName}.service");

			System.Console.WriteLine("Started Service");

			return response;
		}

		public DTOs.StopServiceResponse StopService(DTOs.StopServiceRequest request)
		{
			var response = new DTOs.StopServiceResponse();

			System.Console.WriteLine("Stoping Service");

			ISI.Extensions.Process.WaitForProcessResponse("systemctl", "stop", $"{request.ServiceName}.service");

			System.Console.WriteLine("Stopped Service");

			return response;
		}

		public DTOs.InstallServiceResponse InstallService(DTOs.InstallServiceRequest request)
		{
			var response = new DTOs.InstallServiceResponse();

			System.Console.WriteLine($"ServiceName => {request.ServiceName}");

			var serviceConfig = @$"[Unit]
Description={request.ServiceName}

[Service]
Type=notify
ExecStart={request.Executable} {ServiceApplicationContext.RunningAsServiceOption}

[Install]
WantedBy=multi-user.target
";

			System.Console.WriteLine("Install Service");
			
			System.Console.WriteLine($"Create => {GetServiceConfigFullName(request.ServiceName)}");

			System.IO.File.WriteAllText(GetServiceConfigFullName(request.ServiceName), serviceConfig);

			ISI.Extensions.Process.WaitForProcessResponse("systemctl", "daemon-reload");

			ISI.Extensions.Process.WaitForProcessResponse("systemctl", "enable", $"{request.ServiceName}.service");

			System.Console.WriteLine("Installed Service");
			
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
			
			System.Console.WriteLine($"Delete => {GetServiceConfigFullName(request.ServiceName)}");

			System.IO.File.Delete(GetServiceConfigFullName(request.ServiceName));
			
			System.Console.WriteLine("disable Service");

			ISI.Extensions.Process.WaitForProcessResponse("systemctl", "disable", $"{request.ServiceName}.service");
			
			System.Console.WriteLine("daemon-reload");

			ISI.Extensions.Process.WaitForProcessResponse("systemctl", "daemon-reload");
			
			System.Console.WriteLine("Uninstalled Service");

			return response;
		}
	}
}
