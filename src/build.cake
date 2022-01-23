//dotnet tool install Cake.Tool -g
#addin nuget:?package=Cake.FileHelpers&version=4.0.1
#addin nuget:?package=ISI.Cake.AddIn&loaddependencies=true

//mklink /D Secrets S:\
var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
var settings = GetSettings(settingsFullName);

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionPath = File("./ISI.Extensions.sln");
var solution = ParseSolution(solutionPath);

var assemblyVersionFile = File("./ISI.Extensions.Version.cs");

var buildDateTime = DateTime.UtcNow;
var buildDateTimeStamp = GetDateTimeStamp(buildDateTime);
var buildRevision = GetBuildRevision(buildDateTime);
var assemblyVersion = GetAssemblyVersion(ParseAssemblyInfo(assemblyVersionFile).AssemblyVersion, buildRevision);
Information("AssemblyVersion: {0}", assemblyVersion);

var nugetPackOutputDirectory = Argument("NugetPackOutputDirectory", "../Nuget");

Task("Clean")
	.Does(() =>
	{
		foreach(var projectPath in solution.Projects.Select(p => p.Path.GetDirectory()))
		{
			Information("Cleaning {0}", projectPath);
			CleanDirectories(projectPath + "/**/bin/" + configuration);
			CleanDirectories(projectPath + "/**/obj/" + configuration);
		}

		CleanDirectories(nugetPackOutputDirectory);

		Information("Cleaning Projects ...");
	});

Task("NugetPackageRestore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		Information("Restoring Nuget Packages ...");
		NuGetRestore(solutionPath);
	});

Task("Build")
	.IsDependentOn("NugetPackageRestore")
	.Does(() => 
	{
		CreateAssemblyInfo(assemblyVersionFile, new AssemblyInfoSettings()
		{
			Version = assemblyVersion,
		});

		MSBuild(solutionPath, configurator => configurator
			.SetConfiguration(configuration)
			.SetVerbosity(Verbosity.Quiet)
			.SetMSBuildPlatform(MSBuildPlatform.Automatic)
			.SetPlatformTarget(PlatformTarget.MSIL)
			.WithTarget("Build"));

		CreateAssemblyInfo(assemblyVersionFile, new AssemblyInfoSettings()
		{
			Version = GetAssemblyVersion(assemblyVersion, "*"),
		});
	});

Task("Sign")
	.IsDependentOn("Build")
	.Does(() =>
	{
		if (settings.CodeSigning.DoCodeSigning && configuration.Equals("Release"))
		{
			var files = GetFiles("./**/bin/" + configuration + "/**/ISI.*.dll");

			if(files.Any())
			{
				using(var tempDirectory = GetNewTempDirectory())
				{
					foreach(var file in files)
					{
						var tempFile = File(tempDirectory.FullName + "/" + file.GetFilename());

						if(System.IO.File.Exists(tempFile.Path.FullPath))
						{
							DeleteFile(tempFile);
						}

						CopyFile(file, tempFile);
					}

					var tempFiles = GetFiles(tempDirectory.FullName + "/ISI.*.dll");

					SignAssemblies(new ISI.Cake.Addin.CodeSigning.SignAssembliesRequest()
					{
						AssemblyPaths = tempFiles,
						RemoteCodeSigningServiceUri = GetNullableUri(settings.CodeSigning.RemoteCodeSigningServiceUrl),
						RemoteCodeSigningServicePassword = settings.CodeSigning.RemoteCodeSigningServicePassword,
						TimeStampUri = GetNullableUri(settings.CodeSigning.TimeStampUrl),
						TimeStampDigestAlgorithm = SignToolDigestAlgorithm.Sha256,
						CertificatePath = GetNullableFile(settings.CodeSigning.CertificateFileName),
						CertificatePassword = settings.CodeSigning.CertificatePassword,
						CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
						DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
					});

					foreach(var file in files)
					{
						var tempFile = File(tempDirectory.FullName + "/" + file.GetFilename());

						DeleteFile(file);

						CopyFile(tempFile, file);
					}
				}
			}
		}
	});

Task("Nuget")
	.IsDependentOn("Sign")
	.Does(() =>
	{
		var nupgkFiles = new FilePathCollection();

		foreach(var project in solution.Projects.Where(project => project.Path.FullPath.EndsWith(".csproj") && !project.Name.EndsWith(".Tests") && !project.Name.EndsWith(".T4LocalContent")).OrderBy(project => project.Name, StringComparer.InvariantCultureIgnoreCase))
		{
			Information(project.Name);

			var nuspec = GenerateNuspecFromProject(new ISI.Cake.Addin.Nuget.GenerateNuspecFromProjectRequest()
			{
				ProjectFullName = project.Path.FullPath,
				TryGetPackageVersion = (string package, out string version) =>
				{
					if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
					{
						version =  assemblyVersion;
						return true;
					}

					version = string.Empty;
					return false;
				}
			}).Nuspec;
			nuspec.Version = assemblyVersion;
			nuspec.IconUri = GetNullableUri(@"https://nuget.isi-net.com/Images/Lantern.png");
			nuspec.ProjectUri = GetNullableUri(@"https://github.com/ISI-Extensions/ISI.Extensions");
			nuspec.Title = project.Name;
			nuspec.Description = project.Name;
			nuspec.Copyright = string.Format("Copyright (c) {0}, Integrated Solutions, Inc.", DateTime.Now.Year);
			nuspec.Authors = new [] { "Integrated Solutions, Inc." };
			nuspec.Owners = new [] { "Integrated Solutions, Inc." };

			var nuspecFile = File(project.Path.GetDirectory() + "/" + project.Name + ".nuspec");

			CreateNuspecFile(new ISI.Cake.Addin.Nuget.CreateNuspecFileRequest()
			{
				Nuspec = nuspec,
				NuspecFullName = nuspecFile.Path.FullPath,
			});

			NuGetPack(project.Path.FullPath, new NuGetPackSettings()
			{
				Id = project.Name,
				Version = assemblyVersion, 
				Verbosity = NuGetVerbosity.Detailed,
				Properties = new Dictionary<string, string>
				{
					{ "Configuration", configuration }
				},
				NoPackageAnalysis = false,
				Symbols = false,
				OutputDirectory = nugetPackOutputDirectory,
			});

			DeleteFile(nuspecFile);

			nupgkFiles.Add(File(nugetPackOutputDirectory + "/" + project.Name + "." + assemblyVersion + ".nupkg"));
		}

		//ISI.Extensions.T4LocalContent.*
		foreach(var t4LocalContentVersion in new [] { "", "Embedded", "Resources", "VirtualFiles", "WebApplication", "WebPortableArea" })
		{
			var project = solution.Projects.First(project => project.Name.EndsWith("ISI.Extensions.T4LocalContent"));

			var projectName = "ISI.Extensions.T4LocalContent" + (string.IsNullOrWhiteSpace(t4LocalContentVersion) ? string.Empty : "." + t4LocalContentVersion);
			Information(projectName);
			Information(project.Path.GetDirectory());
			
			var assemblyGroupDirectory = System.IO.Path.GetDirectoryName(project.Path.GetDirectory().FullPath);
			var assemblyGroupName = System.IO.Path.GetFileName(assemblyGroupDirectory);

			Information(string.Format("  {0} => {1}", assemblyGroupName, assemblyGroupDirectory));

			var nuspec = new ISI.Extensions.Nuget.Nuspec()
			{
				Package = projectName,
				Version = assemblyVersion,
				IconUri = GetNullableUri(@"https://nuget.isi-net.com/Images/Lantern.png"),
				ProjectUri = GetNullableUri(@"https://svn.isi-net.com/ISI"),
				Title = projectName,
				Description = projectName,
				Copyright = string.Format("Copyright (c) {0}, Integrated Solutions, Inc.", DateTime.Now.Year),
				Authors = new [] { "Integrated Solutions, Inc." },
				Owners = new [] { "Integrated Solutions, Inc." },
				Files = new []
				{
					new ISI.Extensions.Nuget.NuspecFile()
					{
						Target = "Content/T4LocalContent",
						SourcePattern = (string.IsNullOrWhiteSpace(t4LocalContentVersion) ? string.Empty : t4LocalContentVersion + "/") + "T4LocalContent.settings.t4.pp",
					},
					new ISI.Extensions.Nuget.NuspecFile()
					{
						Target = "Content/T4LocalContent",
						SourcePattern = "T4LocalContent.Generator.t4",
					},
					new ISI.Extensions.Nuget.NuspecFile()
					{
						Target = "Content/T4LocalContent",
						SourcePattern = "T4LocalContent.tt.pp",
					},
					new ISI.Extensions.Nuget.NuspecFile()
					{
						Target = "tools",
						SourcePattern = "Install.ps1",
					},
				},
			};

			var nuspecFile = File(project.Path.GetDirectory() + "/" + projectName + ".nuspec");
			
			Information(nuspecFile.Path.FullPath);

			CreateNuspecFile(new ISI.Cake.Addin.Nuget.CreateNuspecFileRequest()
			{
				Nuspec = nuspec,
				NuspecFullName = nuspecFile.Path.FullPath,
			});

			NupkgPack(new ISI.Cake.Addin.Nuget.NupkgPackRequest()
			{
				NuspecFullName = nuspecFile.Path.FullPath,
				OutputDirectory = nugetPackOutputDirectory,
			});

			DeleteFile(nuspecFile);

			nupgkFiles.Add(File(nugetPackOutputDirectory + "/" + project.Name + "." + assemblyVersion + ".nupkg"));
		}

		if(settings.CodeSigning.DoCodeSigning)
		{
			SignNupkgs(new ISI.Cake.Addin.CodeSigning.SignNupkgsRequest()
			{
				NupkgPaths = nupgkFiles,
				RemoteCodeSigningServiceUri = GetNullableUri(settings.CodeSigning.RemoteCodeSigningServiceUrl),
				RemoteCodeSigningServicePassword = settings.CodeSigning.RemoteCodeSigningServicePassword,
				TimeStampUri = GetNullableUri(settings.CodeSigning.TimeStampUrl),
				TimeStampDigestAlgorithm = SignToolDigestAlgorithm.Sha256,
				CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
				CertificatePath = GetNullableFile(settings.CodeSigning.CertificateFileName),
				CertificatePassword = settings.CodeSigning.CertificatePassword,
				DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
			});
		}

		NupkgPush(new ISI.Cake.Addin.Nuget.NupkgPushRequest()
		{
			NupkgPaths = nupgkFiles,
			ApiKey = settings.Nuget.ApiKey,
			RepositoryName = settings.Nuget.RepositoryName,
			RepositoryUri = GetNullableUri(settings.Nuget.RepositoryUrl),
			PackageChunksRepositoryUri = GetNullableUri(settings.Nuget.PackageChunksRepositoryUrl),
		});
	});

Task("Default")
	.IsDependentOn("Nuget")
	.Does(() => 
	{
		Information("No target provided. Starting default task");
	});

using(GetNugetLock())
{
	using(GetSolutionLock())
	{
		RunTarget(target);
	}
}