using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Tailscale.DataTransferObjects.TailscaleApi
{
	public class CreateAuthKeyRequest : IRequest
	{
		public string TailscaleApiUrl { get; set; }
		public string TailscaleApiToken { get; set; }

		public string Tailnet { get; set; }

		public string Description { get; set; }
		public TimeSpan Expiry { get; set; } = TimeSpan.FromDays(90);
		public bool Reusable { get; set; }
		public bool Ephemeral { get; set; }
		public bool Preauthorized { get; set; }
		public string[] Tags { get; set; }
	}
}