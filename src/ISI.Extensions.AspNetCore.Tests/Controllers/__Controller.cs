using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.AspNetCore.Tests.Controllers
{
	public abstract partial class Controller : Microsoft.AspNetCore.Mvc.Controller
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		protected Controller(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}
	}
}