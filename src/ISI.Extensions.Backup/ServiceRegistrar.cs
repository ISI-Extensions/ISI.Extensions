using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Backup
{
	[ISI.Extensions.DependencyInjection.ServiceRegistrar]
	public class ServiceRegistrar : ISI.Extensions.DependencyInjection.IServiceRegistrar
	{
		public void ServiceRegister(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
		{
			services.AddSingleton<BackupAgent>();
		}
	}
}