using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nginx.DataTransferObjects.NginxApi
{
	public delegate bool UpdateNginxSettingsDelegate(NginxSettings nginxSettings);

	public class UpdateNginxSettingsRequest
	{
		public UpdateNginxSettingsDelegate UpdateSettings { get; set; }
	}
}