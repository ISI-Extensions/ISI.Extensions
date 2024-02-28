using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Mandrill.DataTransferObjects.MandrillWebHooksApi;
using SerializableDTOs = ISI.Extensions.Mandrill.SerializableModels.MandrillWebHooksApi;

namespace ISI.Extensions.Mandrill
{
	public partial class MandrillWebHooksApi
	{
		private UriBuilder GetMessageApiUri(MandrillProfile mandrillProfile)
		{
			var uri = new UriBuilder(mandrillProfile.ApiUrl);

			return uri;
		}
	}
}