using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.DocumentStorage.DataTransferObjects
{
	public class GetDocumentResponse
	{
		public ISI.Extensions.DocumentStorage.IDocument Document { get; set; }
	}
}