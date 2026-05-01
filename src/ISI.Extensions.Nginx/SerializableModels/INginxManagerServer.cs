using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;
using LOCALENTITIES = ISI.Extensions.Nginx;

namespace ISI.Extensions.Nginx.SerializableModels
{
	public interface INginxManagerServer : ISI.Extensions.Converters.IExportTo<LOCALENTITIES.NginxManagerServer>
	{
	}
}