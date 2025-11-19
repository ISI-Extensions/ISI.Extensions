//dotnet tool install Cake.Tool -g
//dotnet tool update Cake.Tool -g
#addin nuget:?package=Cake.FileHelpers
#tool nuget:?package=7-Zip.CommandLine
#addin nuget:?package=Cake.7zip
#addin nuget:?package=ISI.Cake.AddIn&loaddependencies=true
//#addin nuget:?package=Cake.Git

//mklink /D Secrets S:\
var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
var settings = GetSettings(settingsFullName);

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionFile = File("./src/ISI.Extensions.slnx");
var solutionDetails = GetSolutionDetails(solutionFile);
var rootAssemblyVersionKey = "ISI.Extensions";

var buildDateTime = DateTime.UtcNow;
var buildDateTimeStamp = GetDateTimeStamp(buildDateTime);
var buildRevision = GetBuildRevision(buildDateTime);

var assemblyVersions = GetAssemblyVersionFiles(rootAssemblyVersionKey, buildRevision);
var assemblyVersion = assemblyVersions[rootAssemblyVersionKey].AssemblyVersion;

var buildDateTimeStampVersion = new ISI.Extensions.Scm.DateTimeStampVersion(buildDateTimeStamp, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion);

Information("BuildDateTimeStampVersion: {0}", buildDateTimeStampVersion);

var nugetPackOutputDirectory = Argument("NugetPackOutputDirectory", System.IO.Path.GetFullPath("./Nuget"));

Task("Clean")
	.Does(() =>
	{
		Information("Cleaning Projects ...");

		foreach(var projectPath in new HashSet<string>(solutionDetails.ProjectDetailsSet.Select(project => project.ProjectDirectory)))
		{
			Information("Cleaning {0}", projectPath);
			CleanDirectories(projectPath + "/**/bin/" + configuration);
			CleanDirectories(projectPath + "/**/obj/" + configuration);
		}

		Information("Cleaning {0}", nugetPackOutputDirectory);
		CleanDirectories(nugetPackOutputDirectory);
	});

Task("NugetPackageRestore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		Information("Restoring Nuget Packages ...");
		using(GetNugetLock())
		{
			RestoreNugetPackages(solutionFile);
		}
	});

Task("Build")
	.IsDependentOn("NugetPackageRestore")
	.Does(() => 
	{
		using(SetAssemblyVersionFiles(assemblyVersions))
		{
			BuildSolution(solutionFile, new ISI.Cake.Addin.VisualStudio.BuildSolutionRequest()
			{
				Configuration = configuration,
				Target = "Rebuild",
				UseMSBuild = true,
				Verbosity = ISI.Extensions.VisualStudio.MSBuildVerbosity.Quiet,
			});
		}
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

					SignAssemblies(new ISI.Cake.Addin.CodeSigning.SignAssembliesUsingSettingsRequest()
					{
						AssemblyPaths = tempFiles,
						Settings = settings,
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
		var sourceControlUrl = GetSolutionSourceControlUrl();
		var nupkgFiles = new FilePathCollection();

		foreach(var project in solutionDetails.ProjectDetailsSet.Where(project => project.ProjectFullName.EndsWith(".csproj") && 
																	!project.ProjectFullName.EndsWith(".Test") && 
																	!project.ProjectFullName.EndsWith(".Tests") && 
																	!project.ProjectFullName.EndsWith(".T4LocalContent")).OrderBy(project => project.ProjectName, StringComparer.InvariantCultureIgnoreCase))
		{
			Information(project.ProjectName);

			var nuspec = GenerateNuspecFromProject(new ISI.Cake.Addin.Nuget.GenerateNuspecFromProjectRequest()
			{
				ProjectFullName =  System.IO.Path.GetFullPath(project.ProjectFullName),
				TryGetPackageVersion = (string package, out string version) =>
				{
					if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
					{
						version =  assemblyVersion;
						return true;
					}

					version = string.Empty;
					return false;
				},
				IncludeSBom = false,
				Settings = settings,
			}).Nuspec;
			nuspec.Version = assemblyVersion;
			nuspec.ProjectUri = GetNullableUri(sourceControlUrl);
			nuspec.Title = project.ProjectName;
			nuspec.Description = project.ProjectName;

			var nuspecFile = File(project.ProjectDirectory + "/" + project.ProjectName + ".nuspec");

			CreateNuspecFile(new ISI.Cake.Addin.Nuget.CreateNuspecFileRequest()
			{
				Nuspec = nuspec,
				NuspecFullName = nuspecFile.Path.FullPath,
			});

			NupkgPack(new ISI.Cake.Addin.Nuget.NupkgPackRequest()
			{
				NuspecFullName = System.IO.Path.GetFullPath(nuspecFile.Path.FullPath),
				CsProjFullName = System.IO.Path.GetFullPath(project.ProjectFullName),
				OutputDirectory = nugetPackOutputDirectory,
			});

			DeleteFile(nuspecFile);

			nupkgFiles.Add(File(nugetPackOutputDirectory + "/" + project.ProjectName + "." + assemblyVersion + ".nupkg"));
		}

		if(settings.CodeSigning.DoCodeSigning)
		{
			SignNupkgs(new ISI.Cake.Addin.CodeSigning.SignNupkgsUsingSettingsRequest()
			{
				NupkgPaths = nupkgFiles,
				Settings = settings,
			});
		}
	});

Task("Nuget-Publish")
	.IsDependentOn("Nuget")
	.Does(() =>
	{
		var nupkgFiles = GetFiles(MakeAbsolute(Directory(nugetPackOutputDirectory)) + "/*.nupkg");

		NupkgPush(new ISI.Cake.Addin.Nuget.NupkgPushUsingSettingsActiveDirectoryRequest()
		{
			NupkgPaths = nupkgFiles,
			Settings = settings,
		});
	});

Task("Default")
	.IsDependentOn("Nuget-Publish")
	.Does(() => 
	{
		Information("No target provided. Starting default task");
	});

using(GetSolutionLock())
{
	RunTarget(target);
}
