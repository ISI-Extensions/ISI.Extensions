using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Security.Ldap.DataTransferObjects.LdapApi;

namespace ISI.Extensions.Security.Ldap
{
	public partial class LdapApi : ILdapApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected Microsoft.Extensions.Caching.Memory.IMemoryCache MemoryCache { get; }

		public LdapApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			MemoryCache = memoryCache;
		}
	}
}