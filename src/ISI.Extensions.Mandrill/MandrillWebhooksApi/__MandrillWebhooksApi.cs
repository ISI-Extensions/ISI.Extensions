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
	public partial class MandrillWebhooksApi : IMandrillWebhooksApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		
		protected IMandrillProfilesApi MandrillProfilesApi { get; }

		public MandrillWebhooksApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			IMandrillProfilesApi mandrillProfilesApi)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			MandrillProfilesApi = mandrillProfilesApi;
		}
	}
}