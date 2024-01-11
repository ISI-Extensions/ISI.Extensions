using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Platforms.ServiceApplication.Test.Controllers
{
	public partial class ApiController : Controller
	{
		public ApiController(
			Microsoft.Extensions.Logging.ILogger logger)
			: base(logger)
		{
		}
	}
}