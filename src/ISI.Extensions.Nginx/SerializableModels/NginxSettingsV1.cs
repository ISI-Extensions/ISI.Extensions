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
	[ISI.Extensions.Serialization.SerializerContractUuid("116ae30a-8bc6-4e8d-8e6f-547dddf4ff9c")]
	public class NginxSettingsV1 : INginxSettings
	{
		public static INginxSettings ToSerializable(LOCALENTITIES.NginxSettings source)
		{
			return new NginxSettingsV1()
			{
				NginxManagerServers = source.NginxManagerServers.ToNullCheckedArray(NginxManagerServerV1.ToSerializable),
				FormLocationAndSizes = source.FormLocationAndSizes.ToNullCheckedArray(FormLocationAndSizeV1.ToSerializable),
				MaxCheckDirectoryDepth = source.MaxCheckDirectoryDepth,
			};
		}

		public LOCALENTITIES.NginxSettings Export()
		{
			return new LOCALENTITIES.NginxSettings()
			{
				NginxManagerServers = NginxManagerServers.ToNullCheckedArray(nginxManagerServer => nginxManagerServer.Export()),
				FormLocationAndSizes = FormLocationAndSizes.ToNullCheckedArray(formLocationAndSize => formLocationAndSize.Export()),
				MaxCheckDirectoryDepth = MaxCheckDirectoryDepth,
			};
		}

		[DataMember(Name = "nginxManagerServers", EmitDefaultValue = false)]
		public INginxManagerServer[] NginxManagerServers { get; set; }

		[DataMember(Name = "formLocationAndSizes", EmitDefaultValue = false)]
		public IFormLocationAndSize[] FormLocationAndSizes { get; set; }

		[DataMember(Name = "maxCheckDirectoryDepth", EmitDefaultValue = false)]
		public int MaxCheckDirectoryDepth { get; set; } = 5;
	}
}