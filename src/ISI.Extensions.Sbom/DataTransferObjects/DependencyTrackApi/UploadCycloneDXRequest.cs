#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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