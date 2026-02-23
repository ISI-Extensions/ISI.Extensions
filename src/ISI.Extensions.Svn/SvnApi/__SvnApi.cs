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
using Microsoft.Extensions.DependencyInjection;
using DTOs = ISI.Extensions.Svn.DataTransferObjects.SvnApi;
using SourceControlClientApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Svn
{
	[SourceControlClientApi]
	public partial class SvnApi : ISI.Extensions.Scm.ISourceControlClientApi
	{
		public const string SourceControlTypeUuid = "6f5ddcfd-8678-441a-977e-a4f58415de5f";

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.Serialization.ISerialization Serializer { get; }

		public SvnApi()
			: this(null, null)
		{
		}

		public SvnApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.Serialization.ISerialization serializer)
		{
			Logger = logger ?? new ConsoleLogger();
			Serializer = serializer ?? ISI.Extensions.ServiceLocator.Current?.GetService<ISI.Extensions.Serialization.ISerialization>();
		}

		protected static bool? _svnIsInstalled { get; set; } = null;
		protected bool SvnIsInstalled => _svnIsInstalled ??= ISI.Extensions.IO.Path.IsInEnvironmentPath("svn");

		protected static bool? _tortoiseProcIsInstalled { get; set; } = null;
		protected bool TortoiseProcIsInstalled => _tortoiseProcIsInstalled ??= ISI.Extensions.IO.Path.IsInEnvironmentPath("TortoiseProc");

		private const string SccDirectoryName = ".svn";
		Guid ISI.Extensions.Scm.ISourceControlClientApi.SourceControlTypeUuid => SourceControlTypeUuid.ToGuid();
		bool ISI.Extensions.Scm.ISourceControlClientApi.IsSccDirectory(string directoryName) => string.Equals(System.IO.Path.GetFileName(directoryName), SccDirectoryName, StringComparison.InvariantCultureIgnoreCase);
		bool ISI.Extensions.Scm.ISourceControlClientApi.UsesScc(string path)
		{
			var usesSvn = false;

			path = System.IO.Path.GetFullPath(path);

			while (!string.IsNullOrEmpty(path) && !System.IO.Directory.Exists(path))
			{
				path = System.IO.Path.GetDirectoryName(path);
			}

			while (!string.IsNullOrEmpty(path) && !usesSvn)
			{
				usesSvn = System.IO.Directory.Exists(System.IO.Path.Combine(path, SccDirectoryName));

				path = System.IO.Path.GetDirectoryName(path);
			}

			return usesSvn;
		}

		SourceControlClientApiDTOs.GetRootDirectoryResponse ISI.Extensions.Scm.ISourceControlClientApi.GetRootDirectory(SourceControlClientApiDTOs.GetRootDirectoryRequest request)
		{
			var response = new SourceControlClientApiDTOs.GetRootDirectoryResponse();

			var svnResponse = GetWorkingCopyInfos(new()
			{
				Source = request.FullName,
				Depth = Depth.Empty,
			}).Infos.NullCheckedFirstOrDefault();

			response.FullName = svnResponse?.WorkingCopyRootPath ?? string.Empty;
			response.SolutionUrl = svnResponse?.Url;
			response.SourceControlUrl = svnResponse?.Url;

			return response;
		}

		SourceControlClientApiDTOs.ListResponse ISI.Extensions.Scm.ISourceControlClientApi.List(SourceControlClientApiDTOs.ListRequest request)
		{
			var response = new SourceControlClientApiDTOs.ListResponse();

			var apiResponse = List(new()
			{
				SourceUrl = request.SourceUrl,
				AddToLog = request.AddToLog,
			});

			response.FileNames = apiResponse.FileNames;

			return response;
		}

		SourceControlClientApiDTOs.CheckOutResponse ISI.Extensions.Scm.ISourceControlClientApi.CheckOut(SourceControlClientApiDTOs.CheckOutRequest request)
		{
			var response = new SourceControlClientApiDTOs.CheckOutResponse();

			var apiResponse = CheckOut(new()
			{
				SourceUrl = request.SourceUrl,
				TargetFullName = request.TargetFullName,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.CheckOutSingleFileResponse ISI.Extensions.Scm.ISourceControlClientApi.CheckOutSingleFile(SourceControlClientApiDTOs.CheckOutSingleFileRequest request)
		{
			var response = new SourceControlClientApiDTOs.CheckOutSingleFileResponse();

			var apiResponse = CheckOutSingleFile(new()
			{
				SourceUrl = request.SourceUrl,
				TargetFullName = request.TargetFullName,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.UpdateWorkingCopyResponse ISI.Extensions.Scm.ISourceControlClientApi.UpdateWorkingCopy(SourceControlClientApiDTOs.UpdateWorkingCopyRequest request)
		{
			var response = new SourceControlClientApiDTOs.UpdateWorkingCopyResponse();

			var apiResponse = UpdateWorkingCopy(new()
			{
				FullName = request.FullName,
				IncludeExternals = request.IncludeExternals,
				UseTortoiseSvn = request.UseWindowsClient,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.AddResponse ISI.Extensions.Scm.ISourceControlClientApi.Add(SourceControlClientApiDTOs.AddRequest request)
		{
			var response = new SourceControlClientApiDTOs.AddResponse();

			var apiResponse = Add(new()
			{
				FullNames = request.FullNames,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.DeleteResponse ISI.Extensions.Scm.ISourceControlClientApi.Delete(SourceControlClientApiDTOs.DeleteRequest request)
		{
			var response = new SourceControlClientApiDTOs.DeleteResponse();

			var apiResponse = DeletePaths(new()
			{
				FullNames = request.FullNames,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.CommitWorkingCopyResponse ISI.Extensions.Scm.ISourceControlClientApi.CommitWorkingCopy(SourceControlClientApiDTOs.CommitWorkingCopyRequest request)
		{
			var response = new SourceControlClientApiDTOs.CommitWorkingCopyResponse();

			var apiResponse = CommitWorkingCopy(new()
			{
				FullName = request.FullName,
				LogMessage = request.LogMessage,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.CommitResponse ISI.Extensions.Scm.ISourceControlClientApi.Commit(SourceControlClientApiDTOs.CommitRequest request)
		{
			var response = new SourceControlClientApiDTOs.CommitResponse();

			var apiResponse = Commit(new()
			{
				FullNames = request.FullNames,
				LogMessage = request.LogMessage,
				AddToLog = request.AddToLog,
			});

			response.Success = apiResponse.Success;

			return response;
		}

		SourceControlClientApiDTOs.GetWorkingCopyCommitInformationResponse ISI.Extensions.Scm.ISourceControlClientApi.GetWorkingCopyCommitInformation(SourceControlClientApiDTOs.GetWorkingCopyCommitInformationRequest request)
		{
			var response = new SourceControlClientApiDTOs.GetWorkingCopyCommitInformationResponse();

			var apiResponse = GetWorkingCopyCommitInformation(new()
			{
				FullName = request.FullName,
			});

			response.WorkingCopyCommitInformation = apiResponse.WorkingCopyCommitInformation;

			return response;
		}
	}
}