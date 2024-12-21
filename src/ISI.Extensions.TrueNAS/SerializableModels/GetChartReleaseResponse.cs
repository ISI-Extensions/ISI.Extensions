#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.TrueNAS.SerializableModels
{
	[DataContract]
	public class GetChartReleaseResponse
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		//[DataMember(Name = "info", EmitDefaultValue = false)]
		//public Info Info { get; set; }

		//[DataMember(Name = "config", EmitDefaultValue = false)]
		//public Config Config { get; set; }

		//[DataMember(Name = "version", EmitDefaultValue = false)]
		//public int Version { get; set; }

		//[DataMember(Name = "_namespace", EmitDefaultValue = false)]
		//public string Namespace { get; set; }

		//[DataMember(Name = "chart_metadata", EmitDefaultValue = false)]
		//public Chart_Metadata ChartMetadata { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		//[DataMember(Name = "catalog", EmitDefaultValue = false)]
		//public string Catalog { get; set; }

		//[DataMember(Name = "catalog_train", EmitDefaultValue = false)]
		//public string CatalogTrain { get; set; }

		//[DataMember(Name = "path", EmitDefaultValue = false)]
		//public string Path { get; set; }

		//[DataMember(Name = "dataset", EmitDefaultValue = false)]
		//public string Dataset { get; set; }

		//[DataMember(Name = "status", EmitDefaultValue = false)]
		//public string Status { get; set; }

		//[DataMember(Name = "used_ports", EmitDefaultValue = false)]
		//public Used_Ports[] UsedPorts { get; set; }

		//[DataMember(Name = "pod_status", EmitDefaultValue = false)]
		//public Pod_Status PodStatus { get; set; }

		//[DataMember(Name = "update_available", EmitDefaultValue = false)]
		//public bool UpdateAvailable { get; set; }

		//[DataMember(Name = "human_version", EmitDefaultValue = false)]
		//public string HumanVersion { get; set; }

		//[DataMember(Name = "human_latest_version", EmitDefaultValue = false)]
		//public string HumanLatestVersion { get; set; }

		//[DataMember(Name = "container_images_update_available", EmitDefaultValue = false)]
		//public bool ContainerImagesUpdateAvailable { get; set; }

		//[DataMember(Name = "portals", EmitDefaultValue = false)]
		//public Portals Portals { get; set; }
	}

	public class Info
	{
		public string first_deployed { get; set; }
		public string last_deployed { get; set; }
		public string deleted { get; set; }
		public string description { get; set; }
		public string status { get; set; }
		public string notes { get; set; }
	}

	public class Config
	{
		public Global global { get; set; }
		public Image image { get; set; }
		public Ixcertificateauthorities ixCertificateAuthorities { get; set; }
		public Ixcertificates ixCertificates { get; set; }
		public Ixchartcontext1 ixChartContext { get; set; }
		public object[] ixExternalInterfacesConfiguration { get; set; }
		public object[] ixExternalInterfacesConfigurationNames { get; set; }
		public Ixvolume[] ixVolumes { get; set; }
		public Logpostgresimage logPostgresImage { get; set; }
		public Logsearchimage logSearchImage { get; set; }
		public Minioconfig minioConfig { get; set; }
		public Minionetwork minioNetwork { get; set; }
		public Miniostorage minioStorage { get; set; }
		public Notes notes { get; set; }
		public Podoptions podOptions { get; set; }
		public string release_name { get; set; }
		public Resources resources { get; set; }
	}

	public class Global
	{
		public Ixchartcontext ixChartContext { get; set; }
	}

	public class Ixchartcontext
	{
		public bool addNvidiaRuntimeClass { get; set; }
		public bool hasNFSCSI { get; set; }
		public bool hasSMBCSI { get; set; }
		public bool isInstall { get; set; }
		public bool isStopped { get; set; }
		public bool isUpdate { get; set; }
		public bool isUpgrade { get; set; }
		public Kubernetes_Config kubernetes_config { get; set; }
		public string nfsProvisioner { get; set; }
		public string nvidiaRuntimeClassName { get; set; }
		public string operation { get; set; }
		public string smbProvisioner { get; set; }
		public string storageClassName { get; set; }
		public Upgrademetadata upgradeMetadata { get; set; }
	}

	public class Kubernetes_Config
	{
		public string cluster_cidr { get; set; }
		public string cluster_dns_ip { get; set; }
		public string service_cidr { get; set; }
	}

	public class Upgrademetadata
	{
	}

	public class Image
	{
		public string pullPolicy { get; set; }
		public string repository { get; set; }
		public string tag { get; set; }
	}

	public class Ixcertificateauthorities
	{
	}

	public class Ixcertificates
	{
		public _2 _2 { get; set; }
	}

	public class _2
	{
		public bool CA_type_existing { get; set; }
		public bool CA_type_intermediate { get; set; }
		public bool CA_type_internal { get; set; }
		public object CSR { get; set; }
		public string DN { get; set; }
		public bool can_be_revoked { get; set; }
		public string cert_type { get; set; }
		public bool cert_type_CSR { get; set; }
		public bool cert_type_existing { get; set; }
		public bool cert_type_internal { get; set; }
		public string certificate { get; set; }
		public string certificate_path { get; set; }
		public bool chain { get; set; }
		public string[] chain_list { get; set; }
		public object city { get; set; }
		public string common { get; set; }
		public object country { get; set; }
		public string csr_path { get; set; }
		public string digest_algorithm { get; set; }
		public object email { get; set; }
		public bool expired { get; set; }
		public Extensions extensions { get; set; }
		public string fingerprint { get; set; }
		public string from { get; set; }
		public int id { get; set; }
		public string _internal { get; set; }
		public string issuer { get; set; }
		public int key_length { get; set; }
		public string key_type { get; set; }
		public int lifetime { get; set; }
		public string name { get; set; }
		public object organization { get; set; }
		public object organizational_unit { get; set; }
		public bool parsed { get; set; }
		public string privatekey { get; set; }
		public string privatekey_path { get; set; }
		public bool revoked { get; set; }
		public object revoked_date { get; set; }
		public string root_path { get; set; }
		public string[] san { get; set; }
		public float serial { get; set; }
		public object signedby { get; set; }
		public object state { get; set; }
		public long subject_name_hash { get; set; }
		public int type { get; set; }
		public string until { get; set; }
	}

	public class Extensions
	{
		public string AuthorityInfoAccess { get; set; }
		public string AuthorityKeyIdentifier { get; set; }
		public string BasicConstraints { get; set; }
		public string CertificatePolicies { get; set; }
		public string Ct_precert_scts { get; set; }
		public string ExtendedKeyUsage { get; set; }
		public string KeyUsage { get; set; }
		public string SubjectAltName { get; set; }
		public string SubjectKeyIdentifier { get; set; }
	}

	public class Ixchartcontext1
	{
		public bool addNvidiaRuntimeClass { get; set; }
		public bool hasNFSCSI { get; set; }
		public bool hasSMBCSI { get; set; }
		public bool isInstall { get; set; }
		public bool isStopped { get; set; }
		public bool isUpdate { get; set; }
		public bool isUpgrade { get; set; }
		public Kubernetes_Config1 kubernetes_config { get; set; }
		public string nfsProvisioner { get; set; }
		public string nvidiaRuntimeClassName { get; set; }
		public string operation { get; set; }
		public string smbProvisioner { get; set; }
		public string storageClassName { get; set; }
		public Upgrademetadata1 upgradeMetadata { get; set; }
	}

	public class Kubernetes_Config1
	{
		public string cluster_cidr { get; set; }
		public string cluster_dns_ip { get; set; }
		public string service_cidr { get; set; }
	}

	public class Upgrademetadata1
	{
	}

	public class Logpostgresimage
	{
		public string pullPolicy { get; set; }
		public string repository { get; set; }
		public string tag { get; set; }
	}

	public class Logsearchimage
	{
		public string pullPolicy { get; set; }
		public string repository { get; set; }
		public string tag { get; set; }
	}

	public class Minioconfig
	{
		public object[] additionalEnvs { get; set; }
		public string domain { get; set; }
		public object[] extraArgs { get; set; }
		public string rootPassword { get; set; }
		public string rootUser { get; set; }
	}

	public class Minionetwork
	{
		public int apiPort { get; set; }
		public int certificateID { get; set; }
		public int consolePort { get; set; }
	}

	public class Miniostorage
	{
		public object[] additionalStorages { get; set; }
		public object[] distributedIps { get; set; }
		public bool distributedMode { get; set; }
		public Export export { get; set; }
		public bool logSearchApi { get; set; }
		public int logSearchDiskCapacityGB { get; set; }
		public Pgbackup pgBackup { get; set; }
		public Pgdata pgData { get; set; }
	}

	public class Export
	{
		public Ixvolumeconfig ixVolumeConfig { get; set; }
		public string mountPath { get; set; }
		public string type { get; set; }
	}

	public class Ixvolumeconfig
	{
		public bool aclEnable { get; set; }
		public string datasetName { get; set; }
	}

	public class Pgbackup
	{
		public Ixvolumeconfig1 ixVolumeConfig { get; set; }
		public string type { get; set; }
	}

	public class Ixvolumeconfig1
	{
		public string datasetName { get; set; }
	}

	public class Pgdata
	{
		public Ixvolumeconfig2 ixVolumeConfig { get; set; }
		public string type { get; set; }
	}

	public class Ixvolumeconfig2
	{
		public string datasetName { get; set; }
	}

	public class Notes
	{
		public string custom { get; set; }
	}

	public class Podoptions
	{
		public Dnsconfig dnsConfig { get; set; }
	}

	public class Dnsconfig
	{
		public object[] options { get; set; }
	}

	public class Resources
	{
		public Limits limits { get; set; }
	}

	public class Limits
	{
		public string cpu { get; set; }
		public string memory { get; set; }
	}

	public class Ixvolume
	{
		public string hostPath { get; set; }
	}

	public class Chart_Metadata
	{
		public string name { get; set; }
		public string home { get; set; }
		public string[] sources { get; set; }
		public string version { get; set; }
		public string description { get; set; }
		public string[] keywords { get; set; }
		public Maintainer[] maintainers { get; set; }
		public string icon { get; set; }
		public string apiVersion { get; set; }
		public string appVersion { get; set; }
		public Annotations annotations { get; set; }
		public string kubeVersion { get; set; }
		public Dependency[] dependencies { get; set; }
		public string type { get; set; }
		public string latest_chart_version { get; set; }
	}

	public class Annotations
	{
		public string title { get; set; }
	}

	public class Maintainer
	{
		public string name { get; set; }
		public string email { get; set; }
		public string url { get; set; }
	}

	public class Dependency
	{
		public string name { get; set; }
		public string version { get; set; }
		public string repository { get; set; }
		public bool enabled { get; set; }
	}

	public class Pod_Status
	{
		public int desired { get; set; }
		public int available { get; set; }
	}

	public class Portals
	{
		public string[] web_portal { get; set; }
	}

	public class Used_Ports
	{
		public int port { get; set; }
		public string protocol { get; set; }
	}
}
