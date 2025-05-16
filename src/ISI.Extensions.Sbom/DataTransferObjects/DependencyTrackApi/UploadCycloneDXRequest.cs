using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Sbom.DataTransferObjects.DependencyTrackApi
{
	public interface IUploadCycloneDXRequest : IRequest
	{

	}
	public interface IUploadCycloneDXWithProjectUuidRequest : IUploadCycloneDXRequest
	{
		Guid ProjectUuid { get; }
	}
	public interface IUploadCycloneDXWithParentProjectUuidRequest : IUploadCycloneDXRequest
	{
		Guid? ParentProjectUuid { get; }
	}
	public interface IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest : IUploadCycloneDXRequest
	{
		Version ProjectVersion { get; }
		bool IsLatestProjectVersion { get; }
		Version ParentProjectVersion { get; }
	}
	public interface IUploadCycloneDXWithParentProjectNameRequest : IUploadCycloneDXRequest
	{
		string ParentProjectName { get; }
	}
	public interface IUploadCycloneDXWithProjectNameRequest : IUploadCycloneDXRequest
	{
		string ProjectName { get; }
		IEnumerable<string> ProjectTags { get; }
		bool AutoCreate { get; }
	}
	public interface IUploadCycloneDXWithCycloneDXRequest : IUploadCycloneDXRequest
	{
		string CycloneDX { get; }
	}
	public interface IUploadCycloneDXWithCycloneDXFullNameRequest : IUploadCycloneDXRequest
	{
		string CycloneDXFullName { get; }
	}
	public interface IUploadCycloneDXWithCycloneDXStreamRequest : IUploadCycloneDXRequest
	{
		System.IO.Stream CycloneDXStream { get; }
	}

	public class UploadCycloneDXWithProjectUuidRequest : IUploadCycloneDXWithProjectUuidRequest, IUploadCycloneDXWithParentProjectUuidRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public Guid ProjectUuid { get; set; }
		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public Guid? ParentProjectUuid { get; set; }
		public Version ParentProjectVersion { get; set; }
		public string CycloneDX { get; set; }
	}

	public class UploadCycloneDXWithProjectUuidAndCycloneDXFullNameRequest : IUploadCycloneDXWithProjectUuidRequest, IUploadCycloneDXWithParentProjectUuidRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXFullNameRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public Guid ProjectUuid { get; set; }
		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public Guid? ParentProjectUuid { get; set; }
		public Version ParentProjectVersion { get; set; }

		public string CycloneDXFullName { get; set; }
	}

	public class UploadCycloneDXWithProjectUuidAndCycloneDXStreamRequest : IUploadCycloneDXWithProjectUuidRequest, IUploadCycloneDXWithParentProjectUuidRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXStreamRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public Guid ProjectUuid { get; set; }
		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public Guid? ParentProjectUuid { get; set; }
		public Version ParentProjectVersion { get; set; }

		public System.IO.Stream CycloneDXStream { get; set; }
	}

	public class UploadCycloneDXProjectNameAndParentProjectNameRequest : IUploadCycloneDXWithProjectNameRequest, IUploadCycloneDXWithParentProjectNameRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public string ProjectName { get; set; }
		public IEnumerable<string> ProjectTags { get; set; }
		public bool AutoCreate { get; set; }

		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public string ParentProjectName { get; set; }
		public Version ParentProjectVersion { get; set; }

		public string CycloneDX { get; set; }
	}

	public class UploadCycloneDXWithProjectNameAndParentProjectNameAndCycloneDXFullNameRequest : IUploadCycloneDXWithProjectNameRequest, IUploadCycloneDXWithParentProjectNameRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXFullNameRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public string ProjectName { get; set; }
		public IEnumerable<string> ProjectTags { get; set; }
		public bool AutoCreate { get; set; }

		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public string ParentProjectName { get; set; }
		public Version ParentProjectVersion { get; set; }

		public string CycloneDXFullName { get; set; }
	}

	public class UploadCycloneDXWithProjectNameAndParentProjectNameAndCycloneDXStreamRequest : IUploadCycloneDXWithProjectNameRequest, IUploadCycloneDXWithParentProjectNameRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXStreamRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public string ProjectName { get; set; }
		public IEnumerable<string> ProjectTags { get; set; }
		public bool AutoCreate { get; set; }

		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public string ParentProjectName { get; set; }
		public Version ParentProjectVersion { get; set; }

		public System.IO.Stream CycloneDXStream { get; set; }
	}

	public class UploadCycloneDXProjectNameAndParentProjectUuidRequest : IUploadCycloneDXWithProjectNameRequest, IUploadCycloneDXWithParentProjectUuidRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public string ProjectName { get; set; }
		public IEnumerable<string> ProjectTags { get; set; }
		public bool AutoCreate { get; set; }

		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public Guid? ParentProjectUuid { get; set; }
		public Version ParentProjectVersion { get; set; }

		public string CycloneDX { get; set; }
	}

	public class UploadCycloneDXWithProjectNameAndParentProjectUuidAndCycloneDXFullNameRequest : IUploadCycloneDXWithProjectNameRequest, IUploadCycloneDXWithParentProjectUuidRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXFullNameRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public string ProjectName { get; set; }
		public IEnumerable<string> ProjectTags { get; set; }
		public bool AutoCreate { get; set; }

		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public Guid? ParentProjectUuid { get; set; }
		public Version ParentProjectVersion { get; set; }

		public string CycloneDXFullName { get; set; }
	}

	public class UploadCycloneDXWithProjectNameAndParentProjectUuidAndCycloneDXStreamRequest : IUploadCycloneDXWithProjectNameRequest, IUploadCycloneDXWithParentProjectUuidRequest, IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest, IUploadCycloneDXWithCycloneDXStreamRequest
	{
		public string DependencyTrackApiUrl { get; set; }
		public string DependencyTrackApiKey { get; set; }

		public string ProjectName { get; set; }
		public IEnumerable<string> ProjectTags { get; set; }
		public bool AutoCreate { get; set; }

		public Version ProjectVersion { get; set; }
		public bool IsLatestProjectVersion { get; set; }

		public Guid? ParentProjectUuid { get; set; }
		public Version ParentProjectVersion { get; set; }

		public System.IO.Stream CycloneDXStream { get; set; }
	}
}