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
using DTOs = ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi;
using SerializableDTOs = ISI.Extensions.BitBucket.SerializableModels;
using SourceControlRepositoryApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi;

namespace ISI.Extensions.BitBucket
{
	public partial class BitBucketManagerApi
	{
		protected Configuration Configuration { get; }

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public BitBucketManagerApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Configuration = configuration ?? new();
			Logger = logger ?? new ConsoleLogger();
			DateTimeStamper = dateTimeStamper ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper();
		}
	}

	[SourceControlRepositoryApi]
	public class GitBitBucketManagerApi : BitBucketManagerApi, ISI.Extensions.Scm.ISourceControlRepositoryApi
	{
		public const string SourceControlRepositoryTypeUuid = "49ebec63-389d-4fea-9167-2e3a5873d82c";
		public const string Description = "BitBucket (Git)";
		public const string RepositoryType = "git";

		public GitBitBucketManagerApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		: base(configuration, logger, dateTimeStamper)
		{
		}

		public GitBitBucketManagerApi(
			Microsoft.Extensions.Logging.ILogger logger)
		: this(null, logger, null)
		{
		}

		Guid ISI.Extensions.Scm.ISourceControlRepositoryApi.SourceControlRepositoryTypeUuid => SourceControlRepositoryTypeUuid.ToGuid();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.Description => Description;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryType => RepositoryType;
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiUrl => false;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiUrlDescription => "ApiUrl";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiToken => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiTokenDescription => "ApiToken";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseRepositoryNamespace => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryNamespaceDescription => "Workspace";

		SourceControlRepositoryApiDTOs.CreateRepositoryResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.CreateRepository(SourceControlRepositoryApiDTOs.CreateRepositoryRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.CreateRepositoryResponse();

			var apiResponse = CreateRepository(new()
			{
				BitBucketApiToken = request.ApiToken,
				Workspace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
				Scm = "git",
				IsPrivate = request.IsPrivate,
				ProjectKey = "NET",
			});

			response.Url = apiResponse.Url;

			return response;
		}

		SourceControlRepositoryApiDTOs.ListRepositoriesResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.ListRepositories(SourceControlRepositoryApiDTOs.ListRepositoriesRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.ListRepositoriesResponse();

			var apiResponse = ListRepositories(new()
			{
				BitBucketApiToken = request.ApiToken,
			});

			response.Repositories = apiResponse.Repositories
				.NullCheckedWhere(repository => string.IsNullOrWhiteSpace(request.RepositoryNamespace) || string.Equals(request.RepositoryNamespace, repository.Workspace, StringComparison.InvariantCultureIgnoreCase))
				.ToNullCheckedArray(repository => new ISI.Extensions.Scm.Repository()
				{
					RepositoryNamespace = repository.Workspace,
					RepositoryKey = repository.Name,
					Description = repository.Description,
					SourceUrl = repository.SourceUrl,
					Contact = repository.Contact,
					CreationDate = repository.CreationDate,
					Type = repository.Type,
					LastModified = repository.LastModified,
				});

			return response;
		}

		SourceControlRepositoryApiDTOs.DeleteRepositoryResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.DeleteRepository(SourceControlRepositoryApiDTOs.DeleteRepositoryRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.DeleteRepositoryResponse();

			var apiResponse = DeleteRepository(new()
			{
				BitBucketApiToken = request.ApiToken,
				Workspace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
			});

			response.Success = apiResponse.Success;

			return response;
		}
	}

	[SourceControlRepositoryApi]
	public class SvnBitBucketManagerApi : BitBucketManagerApi, ISI.Extensions.Scm.ISourceControlRepositoryApi
	{
		public const string SourceControlRepositoryTypeUuid = "5c8bbe81-770f-4e07-8065-7d2b53d08e88";
		public const string Description = "BitBucket (Svn)";
		public const string RepositoryType = "svn";

		public SvnBitBucketManagerApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		: base(configuration, logger, dateTimeStamper)
		{
		}

		public SvnBitBucketManagerApi(
			Microsoft.Extensions.Logging.ILogger logger)
		: this(null, logger, null)
		{
		}

		Guid ISI.Extensions.Scm.ISourceControlRepositoryApi.SourceControlRepositoryTypeUuid => SourceControlRepositoryTypeUuid.ToGuid();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.Description => Description;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryType => RepositoryType;
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiUrl => false;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiUrlDescription => "ApiUrl";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiToken => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiTokenDescription => "ApiToken";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseRepositoryNamespace => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryNamespaceDescription => "Workspace";

		SourceControlRepositoryApiDTOs.CreateRepositoryResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.CreateRepository(SourceControlRepositoryApiDTOs.CreateRepositoryRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.CreateRepositoryResponse();

			var apiResponse = CreateRepository(new()
			{
				BitBucketApiToken = request.ApiToken,
				Workspace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
				Scm = "git",
				IsPrivate = request.IsPrivate,
				ProjectKey = "NET",
			});

			response.Url = apiResponse.Url;

			return response;
		}

		SourceControlRepositoryApiDTOs.ListRepositoriesResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.ListRepositories(SourceControlRepositoryApiDTOs.ListRepositoriesRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.ListRepositoriesResponse();

			var apiResponse = ListRepositories(new()
			{
				BitBucketApiToken = request.ApiToken,
			});

			response.Repositories = apiResponse.Repositories
				.NullCheckedWhere(repository => string.IsNullOrWhiteSpace(request.RepositoryNamespace) || string.Equals(request.RepositoryNamespace, repository.Workspace, StringComparison.InvariantCultureIgnoreCase))
				.ToNullCheckedArray(repository => new ISI.Extensions.Scm.Repository()
				{
					RepositoryNamespace = repository.Workspace,
					RepositoryKey = repository.Name,
					Description = repository.Description,
					SourceUrl = repository.SourceUrl,
					Contact = repository.Contact,
					CreationDate = repository.CreationDate,
					Type = repository.Type,
					LastModified = repository.LastModified,
				});

			return response;
		}

		SourceControlRepositoryApiDTOs.DeleteRepositoryResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.DeleteRepository(SourceControlRepositoryApiDTOs.DeleteRepositoryRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.DeleteRepositoryResponse();

			var apiResponse = DeleteRepository(new()
			{
				BitBucketApiToken = request.ApiToken,
				Workspace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
			});

			response.Success = apiResponse.Success;

			return response;
		}
	}
}
