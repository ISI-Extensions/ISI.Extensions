using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public class CreateNewAccountRequest : IRequest
	{
		public HostContext HostContext { get; set; }

		public string AccountName { get; set; }

		public string[] Contacts { get; set; }
		public bool? TermsOfServiceAgreed { get; set; }
		public bool? OnlyReturnExisting { get; set; }
	}
}