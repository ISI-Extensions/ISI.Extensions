using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.NameCheap.DataTransferObjects.SslApi
{
	public enum ListCertificatesRequestListType
	{
		[ISI.Extensions.Enum("All", "ALL")] All,
		[ISI.Extensions.Enum("Processing", "Processing")] Processing,
		[ISI.Extensions.Enum("Email Sent", "EmailSent")] EmailSent,
		[ISI.Extensions.Enum("Technical Problem", "TechnicalProblem")] TechnicalProblem,
		[ISI.Extensions.Enum("In Progress", "InProgress")] InProgress,
		[ISI.Extensions.Enum("Completed", "Completed")] Completed,
		[ISI.Extensions.Enum("Deactivated", "Deactivated")] Deactivated,
		[ISI.Extensions.Enum("Active", "Active")] Active,
		[ISI.Extensions.Enum("Cancelled", "Cancelled")] Cancelled,
		[ISI.Extensions.Enum("New Purchase", "NewPurchase")] NewPurchase,
		[ISI.Extensions.Enum("New Renewal", "NewRenewal")] NewRenewal,
	}

	public class ListCertificatesRequest : IRequest
	{
		public string Url { get; set; }
		public string ApiUser { get; set; }
		public string ApiKey { get; set; }

		public ListCertificatesRequestListType ListType { get; set; }
		public string SearchTerm { get; set; }
	}
}