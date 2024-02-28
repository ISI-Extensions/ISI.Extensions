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
	public partial class MandrillWebHooksApi : IMandrillWebHooksApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		
		protected IMandrillProfilesApi MandrillProfilesApi { get; }

		public MandrillWebHooksApi(
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