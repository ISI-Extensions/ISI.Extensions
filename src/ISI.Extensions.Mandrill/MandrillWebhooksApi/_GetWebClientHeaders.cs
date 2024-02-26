using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Mandrill.DataTransferObjects.MandrillWebhooksApi;
using SerializableDTOs = ISI.Extensions.Mandrill.SerializableModels.MandrillWebhooksApi;

namespace ISI.Extensions.Mandrill
{
	public partial class MandrillWebhooksApi
	{
		#region UserAgent
		private static string _userAgent = null;
		public string UserAgent
		{
			get
			{
				if (_userAgent == null)
				{
					var assembly = this.GetType().Assembly;

					_userAgent = string.Format("{0}/{1}", assembly.GetName().Name, ISI.Extensions.SystemInformation.GetAssemblyVersion(assembly));
				}

				return _userAgent;
			}
		}
		#endregion

		private ISI.Extensions.WebClient.HeaderCollection GetWebClientHeaders(MandrillProfile mandrillProfile)
		{
			var headers = new ISI.Extensions.WebClient.HeaderCollection();

			headers.Add("User-Agent", UserAgent);

			return headers;
		}
	}
}