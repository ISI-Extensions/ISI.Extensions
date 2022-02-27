using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.AspNetCore.Tests.Models
{
	public abstract class BaseModel : ISI.Extensions.AspNetCore.IHasTitleModel
	{
		public Microsoft.AspNetCore.Html.IHtmlContent Title { get; set; }
	}
}