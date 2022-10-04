#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using ISI.Extensions.Scm.ServiceReferences.ServicesManager;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.DeploymentManagerApi;


namespace ISI.Extensions.Scm
{
	public partial class DeploymentManagerApi
	{
		public DTOs.DeployArtifactResponse DeployArtifact(DTOs.DeployArtifactRequest request)
		{
			Logger.LogInformation(string.Format("DeployArtifact, ServicesManagerUrl: {0}", request.ServicesManagerUrl));
			Logger.LogInformation(string.Format("DeployArtifact, ArtifactName: {0}", request.ArtifactName));
			Logger.LogInformation(string.Format("DeployArtifact, ArtifactDateTimeStampVersionUrl: {0}", request.ArtifactDateTimeStampVersionUrl));
			Logger.LogInformation(string.Format("DeployArtifact, ArtifactDownloadUrl: {0}", request.ArtifactDownloadUrl));
			Logger.LogInformation(string.Format("DeployArtifact, FromEnvironment: {0}", request.FromEnvironment));
			Logger.LogInformation(string.Format("DeployArtifact, ToEnvironment: {0}", request.ToEnvironment));
			Logger.LogInformation(string.Format("DeployArtifact, ToDateTimeStamp: {0}", request.ToDateTimeStamp));
			Logger.LogInformation(string.Format("DeployArtifact, ConfigurationKey: {0}", request.ConfigurationKey));

			var endPointVersion = GetEndpointVersion(request.ServicesManagerUrl);

			var response = (endPointVersion >= 3 ? DeployArtifactV3(request) : DeployArtifactV1(request));

			foreach (var componentResponse in response.DeployComponentResponses)
			{
				switch (componentResponse)
				{
					case DTOs.DeployConsoleApplicationResponse deployConsoleApplicationResponse:
						Logger.LogInformation(string.Format("  PackageFolder '{0}'.", deployConsoleApplicationResponse.PackageFolder));
						if (!string.IsNullOrWhiteSpace(deployConsoleApplicationResponse.Log))
						{
							Logger.LogInformation(string.Format("  Log '{0}'.", deployConsoleApplicationResponse.Log));
						}
						Logger.LogInformation(string.Format("  SameVersion '{0}'.", deployConsoleApplicationResponse.SameVersion.TrueFalse()));
						Logger.LogInformation(string.Format("  InUse '{0}'.", deployConsoleApplicationResponse.InUse.TrueFalse()));
						Logger.LogInformation(string.Format("  NewInstall '{0}'.", deployConsoleApplicationResponse.NewInstall.TrueFalse()));
						Logger.LogInformation(string.Format("  Installed '{0}'.", deployConsoleApplicationResponse.Installed.TrueFalse()));
						Logger.Log((deployConsoleApplicationResponse.Success ? LogLevel.Information : LogLevel.Error), string.Format("  Success '{0}'.", deployConsoleApplicationResponse.Success.TrueFalse()));
						break;

					case DTOs.DeployWebSiteResponse deployWebSiteResponse:
						Logger.LogInformation(string.Format("  PackageFolder '{0}'.", deployWebSiteResponse.PackageFolder));
						if (!string.IsNullOrWhiteSpace(deployWebSiteResponse.Log))
						{
							Logger.LogInformation(string.Format("  Log '{0}'.", deployWebSiteResponse.Log));
						}
						Logger.LogInformation(string.Format("  SameVersion '{0}'.", deployWebSiteResponse.SameVersion.TrueFalse()));
						Logger.LogInformation(string.Format("  InUse '{0}'.", deployWebSiteResponse.InUse.TrueFalse()));
						Logger.LogInformation(string.Format("  NewInstall '{0}'.", deployWebSiteResponse.NewInstall.TrueFalse()));
						Logger.LogInformation(string.Format("  Installed '{0}'.", deployWebSiteResponse.Installed.TrueFalse()));
						Logger.Log((deployWebSiteResponse.Success ? LogLevel.Information : LogLevel.Error), string.Format("  Success '{0}'.", deployWebSiteResponse.Success.TrueFalse()));
						break;

					case DTOs.DeployWindowsServiceResponse deployWindowsServiceResponse:
						Logger.LogInformation(string.Format("  PackageFolder '{0}'.", deployWindowsServiceResponse.PackageFolder));
						if (!string.IsNullOrWhiteSpace(deployWindowsServiceResponse.Log))
						{
							Logger.LogInformation(string.Format("  Log '{0}'.", deployWindowsServiceResponse.Log));
						}
						Logger.LogInformation(string.Format("  SameVersion '{0}'.", deployWindowsServiceResponse.SameVersion.TrueFalse()));
						Logger.LogInformation(string.Format("  InUse '{0}'.", deployWindowsServiceResponse.InUse.TrueFalse()));
						Logger.LogInformation(string.Format("  NewInstall '{0}'.", deployWindowsServiceResponse.NewInstall.TrueFalse()));
						Logger.LogInformation(string.Format("  Installed '{0}'.", deployWindowsServiceResponse.Installed.TrueFalse()));
						Logger.Log((deployWindowsServiceResponse.Success ? LogLevel.Information : LogLevel.Error), string.Format("  Success '{0}'.", deployWindowsServiceResponse.Success.TrueFalse()));
						break;
				}
			}

			return response;
		}

		private DTOs.DeployArtifactResponse DeployArtifactV1(DTOs.DeployArtifactRequest request)
		{
			var response = new DTOs.DeployArtifactResponse();

			var buildArtifactManagementUri = new UriBuilder(request.BuildArtifactManagementUrl);
			buildArtifactManagementUri.Path = "remote-management/";

			var deployComponents = new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployComponentCollection();

			ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployComponentExcludeFileCollection getDeployComponentExcludeFileCollection(IEnumerable<string> fileNames)
			{
				var deployComponentExcludeFiles = new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployComponentExcludeFileCollection();

				foreach (var fileName in fileNames.ToNullCheckedArray(NullCheckCollectionResult.Empty))
				{
					deployComponentExcludeFiles.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployComponentExcludeFile()
					{
						ExcludeFile = fileName,
					});
				}

				return deployComponentExcludeFiles;
			}

			foreach (var component in request.Components.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				switch (component)
				{
					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponent deployComponent:
						if (string.Equals(deployComponent.ComponentType, "ConsoleApplication", StringComparison.InvariantCultureIgnoreCase))
						{
							deployComponents.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployConsoleApplication()
							{
								PackageFolder = deployComponent.PackageFolder,
								DeployToSubfolder = deployComponent.DeployToSubfolder,
								ConsoleApplicationExe = deployComponent.ApplicationExe,
								ExcludeFiles = getDeployComponentExcludeFileCollection(deployComponent.ExcludeFiles),
							});
							Logger.LogInformation(string.Format("  deployComponent.PackageFolder: {0}", deployComponent.PackageFolder));
							Logger.LogInformation(string.Format("  deployComponent.DeployToSubfolder: {0}", deployComponent.DeployToSubfolder));
							Logger.LogInformation(string.Format("  deployComponent.ConsoleApplicationExe: {0}", deployComponent.ApplicationExe));
						}
						else if (string.Equals(deployComponent.ComponentType, "WebSite", StringComparison.InvariantCultureIgnoreCase))
						{
							deployComponents.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployWebSite()
							{
								PackageFolder = deployComponent.PackageFolder,
								DeployToSubfolder = deployComponent.DeployToSubfolder,
								ExcludeFiles = getDeployComponentExcludeFileCollection(deployComponent.ExcludeFiles),
							});
							Logger.LogInformation(string.Format("  deployComponent.PackageFolder: {0}", deployComponent.PackageFolder));
							Logger.LogInformation(string.Format("  deployComponent.DeployToSubfolder: {0}", deployComponent.DeployToSubfolder));
						}
						else if (string.Equals(deployComponent.ComponentType, "WindowsService", StringComparison.InvariantCultureIgnoreCase))
						{
							deployComponents.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployWindowsService()
							{
								PackageFolder = deployComponent.PackageFolder,
								DeployToSubfolder = deployComponent.DeployToSubfolder,
								WindowsServiceExe = deployComponent.ApplicationExe,
								ExcludeFiles = getDeployComponentExcludeFileCollection(deployComponent.ExcludeFiles),
							});
							Logger.LogInformation(string.Format("  deployComponent.PackageFolder: {0}", deployComponent.PackageFolder));
							Logger.LogInformation(string.Format("  deployComponent.DeployToSubfolder: {0}", deployComponent.DeployToSubfolder));
							Logger.LogInformation(string.Format("  deployComponent.WindowsServiceExe: {0}", deployComponent.ApplicationExe));
						}
						break;

					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentConsoleApplication deployComponentConsoleApplication:
						deployComponents.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployConsoleApplication()
						{
							PackageFolder = deployComponentConsoleApplication.PackageFolder,
							DeployToSubfolder = deployComponentConsoleApplication.DeployToSubfolder,
							ConsoleApplicationExe = deployComponentConsoleApplication.ConsoleApplicationExe,
							ExcludeFiles = getDeployComponentExcludeFileCollection(deployComponentConsoleApplication.ExcludeFiles),
							ExecuteConsoleApplicationAfterInstall = deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstall,
							ExecuteConsoleApplicationAfterInstallArguments = deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstallArguments,
						});
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.PackageFolder: {0}", deployComponentConsoleApplication.PackageFolder));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.DeployToSubfolder: {0}", deployComponentConsoleApplication.DeployToSubfolder));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.ConsoleApplicationExe: {0}", deployComponentConsoleApplication.ConsoleApplicationExe));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstall: {0}", deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstall.TrueFalse()));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstallArguments: {0}", deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstallArguments));
						break;

					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWebSite deployComponentWebSite:
						deployComponents.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployWebSite()
						{
							PackageFolder = deployComponentWebSite.PackageFolder,
							DeployToSubfolder = deployComponentWebSite.DeployToSubfolder,
							ExcludeFiles = getDeployComponentExcludeFileCollection(deployComponentWebSite.ExcludeFiles),
						});
						Logger.LogInformation(string.Format("  deployComponentWebSite.PackageFolder: {0}", deployComponentWebSite.PackageFolder));
						Logger.LogInformation(string.Format("  deployComponentWebSite.DeployToSubfolder: {0}", deployComponentWebSite.DeployToSubfolder));
						break;

					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWindowsService deployComponentWindowsService:
						deployComponents.Add(new ISI.Extensions.Scm.ServiceReferences.ServicesManager.DeployWindowsService()
						{
							PackageFolder = deployComponentWindowsService.PackageFolder,
							DeployToSubfolder = deployComponentWindowsService.DeployToSubfolder,
							WindowsServiceExe = deployComponentWindowsService.WindowsServiceExe,
							ExcludeFiles = getDeployComponentExcludeFileCollection(deployComponentWindowsService.ExcludeFiles),
							UninstallIfInstalled = deployComponentWindowsService.UninstallIfInstalled,
						});
						Logger.LogInformation(string.Format("  deployComponentWindowsService.PackageFolder: {0}", deployComponentWindowsService.PackageFolder));
						Logger.LogInformation(string.Format("  deployComponentWindowsService.DeployToSubfolder: {0}", deployComponentWindowsService.DeployToSubfolder));
						Logger.LogInformation(string.Format("  deployComponentWindowsService.WindowsServiceExe: {0}", deployComponentWindowsService.WindowsServiceExe));
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(component));
				}
				Logger.LogInformation(string.Empty);
			}

			var statusTrackerKey = string.Empty;

			using (var managerClient = ISI.Extensions.Scm.ServiceReferences.ServicesManager.ManagerClient.GetClient(request.ServicesManagerUrl))
			{
				managerClient.Endpoint.Binding.SendTimeout = TimeSpan.FromMinutes(15);
				managerClient.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMinutes(15);

				var deployArtifactResponse = managerClient.DeployArtifact(request.Password, buildArtifactManagementUri.Uri.ToString(), request.AuthenticationToken, request.ArtifactName, request.ArtifactDateTimeStampVersionUrl, request.ArtifactDownloadUrl, request.ToDateTimeStamp, request.FromEnvironment, request.ToEnvironment, request.ConfigurationKey, deployComponents, request.SetDeployedVersion, request.RunAsync);

				statusTrackerKey = deployArtifactResponse.StatusTrackerKey;

				Logger.LogInformation(string.Format("  statusTrackerKey: {0}", statusTrackerKey));

				if (!request.RunAsync)
				{
					response.DeployComponentResponses = deployArtifactResponse.DeployComponentResponses.ToNullCheckedArray(deployComponentResponse => ((ISI.Extensions.Scm.ServiceReferences.ServicesManager.IDeployComponentResponse)deployComponentResponse).Export());
				}
			}

			if (request.RunAsync)
			{
				response.Success = WatchV1(request.ServicesManagerUrl, request.Password, statusTrackerKey);
			}

			return response;
		}

		private DTOs.DeployArtifactResponse DeployArtifactV3(DTOs.DeployArtifactRequest request)
		{
			var response = new DTOs.DeployArtifactResponse();

			var buildArtifactManagementUri = new UriBuilder(request.BuildArtifactManagementUrl);
			buildArtifactManagementUri.Path = "remote-management/";

			var deployComponents = new List<SerializableDTOs.IDeployComponent>();

			SerializableDTOs.DeployComponentExcludeFile[] getDeployComponentExcludeFiles(IEnumerable<string> fileNames)
			{
				var deployComponentExcludeFiles = new List<SerializableDTOs.DeployComponentExcludeFile>();

				foreach (var fileName in fileNames.ToNullCheckedArray(NullCheckCollectionResult.Empty))
				{
					deployComponentExcludeFiles.Add(new SerializableDTOs.DeployComponentExcludeFile()
					{
						ExcludeFile = fileName,
					});
				}

				return deployComponentExcludeFiles.ToArray();
			}

			foreach (var component in request.Components.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				switch (component)
				{
					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponent deployComponent:
						if (string.Equals(deployComponent.ComponentType, "ConsoleApplication", StringComparison.InvariantCultureIgnoreCase))
						{
							deployComponents.Add(new SerializableDTOs.DeployConsoleApplication()
							{
								PackageFolder = deployComponent.PackageFolder,
								DeployToSubfolder = deployComponent.DeployToSubfolder,
								ConsoleApplicationExe = deployComponent.ApplicationExe,
								ExcludeFiles = getDeployComponentExcludeFiles(deployComponent.ExcludeFiles),
							});
							Logger.LogInformation(string.Format("  deployComponent.PackageFolder: {0}", deployComponent.PackageFolder));
							Logger.LogInformation(string.Format("  deployComponent.DeployToSubfolder: {0}", deployComponent.DeployToSubfolder));
							Logger.LogInformation(string.Format("  deployComponent.ConsoleApplicationExe: {0}", deployComponent.ApplicationExe));
						}
						else if (string.Equals(deployComponent.ComponentType, "WebSite", StringComparison.InvariantCultureIgnoreCase))
						{
							deployComponents.Add(new SerializableDTOs.DeployWebSite()
							{
								PackageFolder = deployComponent.PackageFolder,
								DeployToSubfolder = deployComponent.DeployToSubfolder,
								ExcludeFiles = getDeployComponentExcludeFiles(deployComponent.ExcludeFiles),
							});
							Logger.LogInformation(string.Format("  deployComponent.PackageFolder: {0}", deployComponent.PackageFolder));
							Logger.LogInformation(string.Format("  deployComponent.DeployToSubfolder: {0}", deployComponent.DeployToSubfolder));
						}
						else if (string.Equals(deployComponent.ComponentType, "WindowsService", StringComparison.InvariantCultureIgnoreCase))
						{
							deployComponents.Add(new SerializableDTOs.DeployWindowsService()
							{
								PackageFolder = deployComponent.PackageFolder,
								DeployToSubfolder = deployComponent.DeployToSubfolder,
								WindowsServiceExe = deployComponent.ApplicationExe,
								ExcludeFiles = getDeployComponentExcludeFiles(deployComponent.ExcludeFiles),
							});
							Logger.LogInformation(string.Format("  deployComponent.PackageFolder: {0}", deployComponent.PackageFolder));
							Logger.LogInformation(string.Format("  deployComponent.DeployToSubfolder: {0}", deployComponent.DeployToSubfolder));
							Logger.LogInformation(string.Format("  deployComponent.WindowsServiceExe: {0}", deployComponent.ApplicationExe));
						}
						break;

					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentConsoleApplication deployComponentConsoleApplication:
						deployComponents.Add(new SerializableDTOs.DeployConsoleApplication()
						{
							PackageFolder = deployComponentConsoleApplication.PackageFolder,
							DeployToSubfolder = deployComponentConsoleApplication.DeployToSubfolder,
							ConsoleApplicationExe = deployComponentConsoleApplication.ConsoleApplicationExe,
							ExcludeFiles = getDeployComponentExcludeFiles(deployComponentConsoleApplication.ExcludeFiles),
							ExecuteConsoleApplicationAfterInstall = deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstall,
							ExecuteConsoleApplicationAfterInstallArguments = deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstallArguments,
						});
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.PackageFolder: {0}", deployComponentConsoleApplication.PackageFolder));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.DeployToSubfolder: {0}", deployComponentConsoleApplication.DeployToSubfolder));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.ConsoleApplicationExe: {0}", deployComponentConsoleApplication.ConsoleApplicationExe));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstall: {0}", deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstall.TrueFalse()));
						Logger.LogInformation(string.Format("  deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstallArguments: {0}", deployComponentConsoleApplication.ExecuteConsoleApplicationAfterInstallArguments));
						break;

					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWebSite deployComponentWebSite:
						deployComponents.Add(new SerializableDTOs.DeployWebSite()
						{
							PackageFolder = deployComponentWebSite.PackageFolder,
							DeployToSubfolder = deployComponentWebSite.DeployToSubfolder,
							ExcludeFiles = getDeployComponentExcludeFiles(deployComponentWebSite.ExcludeFiles),
						});
						Logger.LogInformation(string.Format("  deployComponentWebSite.PackageFolder: {0}", deployComponentWebSite.PackageFolder));
						Logger.LogInformation(string.Format("  deployComponentWebSite.DeployToSubfolder: {0}", deployComponentWebSite.DeployToSubfolder));
						break;

					case ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWindowsService deployComponentWindowsService:
						deployComponents.Add(new SerializableDTOs.DeployWindowsService()
						{
							PackageFolder = deployComponentWindowsService.PackageFolder,
							DeployToSubfolder = deployComponentWindowsService.DeployToSubfolder,
							WindowsServiceExe = deployComponentWindowsService.WindowsServiceExe,
							ExcludeFiles = getDeployComponentExcludeFiles(deployComponentWindowsService.ExcludeFiles),
							UninstallIfInstalled = deployComponentWindowsService.UninstallIfInstalled,
						});
						Logger.LogInformation(string.Format("  deployComponentWindowsService.PackageFolder: {0}", deployComponentWindowsService.PackageFolder));
						Logger.LogInformation(string.Format("  deployComponentWindowsService.DeployToSubfolder: {0}", deployComponentWindowsService.DeployToSubfolder));
						Logger.LogInformation(string.Format("  deployComponentWindowsService.WindowsServiceExe: {0}", deployComponentWindowsService.WindowsServiceExe));
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(component));
				}
				Logger.LogInformation(string.Empty);
			}

			var uri = new UriBuilder(request.ServicesManagerUrl);
			uri.SetPathAndQueryString("rest/manager/v3/deploy-artifact");

			var restRequest = new SerializableDTOs.DeployArtifactRequest()
			{
				RemoteManagementUrl = buildArtifactManagementUri.Uri.ToString(),
				AuthenticationToken = request.AuthenticationToken,
				ArtifactName = request.ArtifactName,
				ArtifactDateTimeStampVersionUrl = request.ArtifactDateTimeStampVersionUrl,
				ArtifactDownloadUrl = request.ArtifactDownloadUrl,
				ToDateTimeStamp = request.ToDateTimeStamp,
				FromEnvironment = request.FromEnvironment,
				ToEnvironment = request.ToEnvironment,
				ConfigurationKey = request.ConfigurationKey,
				WaitForFileLocksMaxTimeOutInSeconds = (request.WaitForFileLocksMaxTimeOut.HasValue ? (long)request.WaitForFileLocksMaxTimeOut.Value.TotalSeconds : null),
				DeployComponents = deployComponents.ToArray(),
				SetDeployedVersion = request.SetDeployedVersion,
				RunAsync = request.RunAsync,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.DeployArtifactRequest, SerializableDTOs.DeployArtifactResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request.Password), restRequest, false);

			var statusTrackerKey = restResponse?.Response?.StatusTrackerKey;

			Logger.LogInformation(string.Format("  statusTrackerKey: {0}", statusTrackerKey));

			if (!request.RunAsync)
			{
				response.DeployComponentResponses = restResponse?.Response?.DeployComponentResponses.ToNullCheckedArray(deployComponentResponse => deployComponentResponse.Export());
			}

			if (request.RunAsync)
			{
				response.DeployComponentResponses = WatchV3(request.ServicesManagerUrl, request.Password, statusTrackerKey);
			}

			return response;
		}
	}
}