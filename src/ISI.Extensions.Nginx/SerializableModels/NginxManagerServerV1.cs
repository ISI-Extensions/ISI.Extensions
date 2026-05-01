using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using LOCALENTITIES = ISI.Extensions.Nginx;

namespace ISI.Extensions.Nginx.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("3fcddb8e-8f47-442f-972f-08d0ffe59f11")]
	public class NginxManagerServerV1 : INginxManagerServer
	{
		public static INginxManagerServer ToSerializable(LOCALENTITIES.NginxManagerServer source)
		{
			return new NginxManagerServerV1()
			{
				NginxManagerServerUuid = source.NginxManagerServerUuid,
				Description = source.Description,
				NginxManagerApiUrl = source.NginxManagerApiUrl,
				NginxManagerApiKey = source.NginxManagerApiKey,
				Directories = source.Directories.ToNullCheckedArray(),
			};
		}

		public LOCALENTITIES.NginxManagerServer Export()
		{
			return new LOCALENTITIES.NginxManagerServer()
			{
				NginxManagerServerUuid = NginxManagerServerUuid,
				Description = Description,
				NginxManagerApiUrl = NginxManagerApiUrl,
				NginxManagerApiKey = NginxManagerApiKey,
				Directories = Directories.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "nginxManagerServerUuid", EmitDefaultValue = false)]
		public Guid NginxManagerServerUuid { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "nginxManagerApiUrl", EmitDefaultValue = false)]
		public string NginxManagerApiUrl { get; set; }

		[DataMember(Name = "nginxManagerApiKey", EmitDefaultValue = false)]
		public string NginxManagerApiKey { get; set; }

		[DataMember(Name = "directories", EmitDefaultValue = false)]
		public string[] Directories { get; set; }
	}
}