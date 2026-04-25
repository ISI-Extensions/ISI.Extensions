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
using DTOs = ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi;
using SerializableDTOs = ISI.Extensions.ScmManager.SerializableModels;
using SourceControlRepositoryApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi;

namespace ISI.Extensions.ScmManager
{
	public partial class ScmManagerApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public ScmManagerApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger ?? new ConsoleLogger();
			DateTimeStamper = dateTimeStamper ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper();
		}
	}

	[SourceControlRepositoryApi]
	public class GitScmManagerApi : ScmManagerApi, ISI.Extensions.Scm.ISourceControlRepositoryApi
	{
		public const string SourceControlRepositoryTypeUuid = "d5d64897-2016-4eee-bd05-fab204733ea4";
		public const string Description = "ScmManager (Git)";
		public const string RepositoryType = "git";

		public GitScmManagerApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		: base(logger, dateTimeStamper)
		{
		}

		public GitScmManagerApi(
			Microsoft.Extensions.Logging.ILogger logger)
		: this(logger, null)
		{
		}

		Guid ISI.Extensions.Scm.ISourceControlRepositoryApi.SourceControlRepositoryTypeUuid => SourceControlRepositoryTypeUuid.ToGuid();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.Description => Description;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryType => RepositoryType;
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiUrl => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiUrlDescription => "ApiUrl";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiToken => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiTokenDescription => "ApiToken";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseRepositoryNamespace => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryNamespaceDescription => "Namespace";

		SourceControlRepositoryApiDTOs.CreateRepositoryResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.CreateRepository(SourceControlRepositoryApiDTOs.CreateRepositoryRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.CreateRepositoryResponse();

			var apiResponse = CreateRepository(new()
			{
				ScmManagerApiUrl = request.ApiUrl,
				ScmManagerApiToken = request.ApiToken,
				Namespace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
				Type = RepositoryType,
				Initialize = true,
			});

			response.Url = apiResponse.Url;

			return response;
		}

		SourceControlRepositoryApiDTOs.ListRepositoriesResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.ListRepositories(SourceControlRepositoryApiDTOs.ListRepositoriesRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.ListRepositoriesResponse();

			var apiResponse = ListRepositories(new()
			{
				ScmManagerApiUrl = request.ApiUrl,
				ScmManagerApiToken = request.ApiToken,
			});

			response.Repositories = apiResponse.Repositories
				.NullCheckedWhere(repository => string.Equals(repository.Type, RepositoryType, StringComparison.InvariantCultureIgnoreCase) && (string.IsNullOrWhiteSpace(request.RepositoryNamespace) || string.Equals(request.RepositoryNamespace, repository.Namespace, StringComparison.InvariantCultureIgnoreCase)))
				.ToNullCheckedArray(repository => new ISI.Extensions.Scm.Repository()
				{
					RepositoryNamespace = repository.Namespace,
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
				ScmManagerApiUrl = request.ApiUrl,
				ScmManagerApiToken = request.ApiToken,
				Namespace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
			});

			response.Success = apiResponse.Success;

			return response;
		}
	}

	[SourceControlRepositoryApi]
	public class SvnScmManagerApi : ScmManagerApi, ISI.Extensions.Scm.ISourceControlRepositoryApi
	{
		public const string SourceControlRepositoryTypeUuid = "5f5cd390-3508-421b-bf38-5175cb2418fb";
		public const string Description = "ScmManager (Svn)";
		public const string RepositoryType = "svn";

		public SvnScmManagerApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		: base(logger, dateTimeStamper)
		{
		}

		public SvnScmManagerApi(
			Microsoft.Extensions.Logging.ILogger logger)
		: this(logger, null)
		{
		}

		Guid ISI.Extensions.Scm.ISourceControlRepositoryApi.SourceControlRepositoryTypeUuid => SourceControlRepositoryTypeUuid.ToGuid();
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.Description => Description;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryType => RepositoryType;
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiUrl => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiUrlDescription => "ApiUrl";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseApiToken => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.ApiTokenDescription => "ApiToken";
		bool ISI.Extensions.Scm.ISourceControlRepositoryApi.UseRepositoryNamespace => true;
		string ISI.Extensions.Scm.ISourceControlRepositoryApi.RepositoryNamespaceDescription => "Namespace";

		SourceControlRepositoryApiDTOs.CreateRepositoryResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.CreateRepository(SourceControlRepositoryApiDTOs.CreateRepositoryRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.CreateRepositoryResponse();

			var apiResponse = CreateRepository(new()
			{
				ScmManagerApiUrl = request.ApiUrl,
				ScmManagerApiToken = request.ApiToken,
				Namespace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
				Type = RepositoryType,
				Initialize = true,
			});

			response.Url = apiResponse.Url;

			return response;
		}

		SourceControlRepositoryApiDTOs.ListRepositoriesResponse ISI.Extensions.Scm.ISourceControlRepositoryApi.ListRepositories(SourceControlRepositoryApiDTOs.ListRepositoriesRequest request)
		{
			var response = new SourceControlRepositoryApiDTOs.ListRepositoriesResponse();

			var apiResponse = ListRepositories(new()
			{
				ScmManagerApiUrl = request.ApiUrl,
				ScmManagerApiToken = request.ApiToken,
			});

			response.Repositories = apiResponse.Repositories
				.NullCheckedWhere(repository => string.Equals(repository.Type, RepositoryType, StringComparison.InvariantCultureIgnoreCase) && (string.IsNullOrWhiteSpace(request.RepositoryNamespace) || string.Equals(request.RepositoryNamespace, repository.Namespace, StringComparison.InvariantCultureIgnoreCase)))
				.ToNullCheckedArray(repository => new ISI.Extensions.Scm.Repository()
				{
					RepositoryNamespace = repository.Namespace,
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
				ScmManagerApiUrl = request.ApiUrl,
				ScmManagerApiToken = request.ApiToken,
				Namespace = request.RepositoryNamespace,
				Name = request.RepositoryKey,
			});

			response.Success = apiResponse.Success;

			return response;
		}
	}
}