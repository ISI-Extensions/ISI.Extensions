using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Ant.DataTransferObjects.AntApi;

namespace ISI.Extensions.Ant
{
	[BuildScriptApi]
	public partial class AntApi : ISI.Extensions.Scm.IBuildScriptApi
	{
		public const string BuildScriptTypeUuid = "62773a5f-62ec-40c0-8523-4890f76b1d8f";
		public const string BuildScriptFileName = "build.ant";
		public const string AntFileNameExtension = ".ant";

		private static HashSet<string> AntFileNameExtensions = new([".ant", ".xml"], StringComparer.InvariantCultureIgnoreCase);

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public AntApi(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}

		Guid ISI.Extensions.Scm.IBuildScriptApi.BuildScriptTypeUuid => BuildScriptTypeUuid.ToGuid();

		bool ISI.Extensions.Scm.IBuildScriptApi.TryGetBuildScript(string solutionDirectory, out string buildScriptFullName)
		{
			try
			{
				if (string.Equals(System.IO.Path.GetFileName(solutionDirectory), BuildScriptFileName, StringComparison.CurrentCultureIgnoreCase))
				{
					buildScriptFullName = solutionDirectory;

					return true;
				}

				if (System.IO.File.Exists(solutionDirectory))
				{
					solutionDirectory = System.IO.Path.GetDirectoryName(solutionDirectory);
				}

				var possibleBuildScriptFullNames = System.IO.Directory.GetFiles(solutionDirectory, BuildScriptFileName, System.IO.SearchOption.AllDirectories).OrderBy(fileName => fileName.Length);

				buildScriptFullName = possibleBuildScriptFullNames.NullCheckedFirstOrDefault();

				return !string.IsNullOrWhiteSpace(buildScriptFullName);
			}
			catch
			{
				buildScriptFullName = null;

				return false;
			}
		}

		ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.IsBuildScriptFileResponse ISI.Extensions.Scm.IBuildScriptApi.IsBuildScriptFile(ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.IsBuildScriptFileRequest request)
		{
			var response = new ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.IsBuildScriptFileResponse();

			response.IsBuildFile = IsBuildScriptFile(new()
			{
				BuildScriptFullName = request.BuildScriptFullName,
			}).IsBuildFile;

			return response;
		}

		ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.GetTargetKeysFromBuildScriptResponse ISI.Extensions.Scm.IBuildScriptApi.GetTargetKeysFromBuildScript(ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.GetTargetKeysFromBuildScriptRequest request)
		{
			var response = new ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.GetTargetKeysFromBuildScriptResponse();

			response.Targets = GetTargetKeysFromBuildScript(new()
			{
				BuildScriptFullName = request.BuildScriptFullName,
			}).Targets;

			return response;
		}

		ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.ExecuteBuildTargetResponse ISI.Extensions.Scm.IBuildScriptApi.ExecuteBuildTarget(ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.ExecuteBuildTargetRequest request)
		{
			var response = new ISI.Extensions.Scm.DataTransferObjects.BuildScriptApi.ExecuteBuildTargetResponse();

			var executeBuildTargetResponse = ExecuteBuildTarget(new()
			{
				BuildScriptFullName = request.BuildScriptFullName,
				Target = request.Target,
				Parameters = request.Parameters,
				UseShell = request.UseShell,
				AddToLog = request.AddToLog,
			});

			response.ExecutionOutputLog = executeBuildTargetResponse?.ExecutionOutputLog;
			response.Success = executeBuildTargetResponse?.Success ?? false;

			return response;
		}
	}
}