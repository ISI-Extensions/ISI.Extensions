using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Mandrill.DataTransferObjects.MandrillProfilesApi
{
	public class ListMandrillProfilesResponse
	{
		public IEnumerable<MandrillProfile> MandrillProfiles { get; set; }
	}
}