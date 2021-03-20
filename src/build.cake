#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=ISI.Cake.AddIn&loaddependencies=true

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionPath = File("./ISI.Extensions.sln");
var solution = ParseSolution(solutionPath);

var assemblyVersionFile = File("./ISI.Extensions.Version.cs");

var revisionBuild = GetBuildRevision();
var assemblyVersion = ParseAssemblyInfo(assemblyVersionFile).AssemblyVersion;
var assemblyVersionPieces = assemblyVersion.Split(new [] { '.' }, StringSplitOptions.RemoveEmptyEntries);
assemblyVersion = string.Format("{0}.{1}.*", assemblyVersionPieces[0], assemblyVersionPieces[1]);
Information("AssemblyVersion: {0}", assemblyVersion);
var buildVersion = assemblyVersion.Replace("*", revisionBuild);
Information("BuildVersion: {0}", buildVersion);

var nugetDirectory = "../Nuget";

Task("Clean")
	.Does(() =>
	{
		foreach(var projectPath in solution.Projects.Select(p => p.Path.GetDirectory()))
		{
			Information("Cleaning {0}", projectPath);
			CleanDirectories(projectPath + "/**/bin/" + configuration);
			CleanDirectories(projectPath + "/**/obj/" + configuration);
		}

		CleanDirectories(nugetDirectory);

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
			Version = buildVersion,
		});

		MSBuild(solutionPath, configurator => configurator
			.SetConfiguration(configuration)
			.SetVerbosity(Verbosity.Quiet)
			.SetMSBuildPlatform(MSBuildPlatform.Automatic)
			.SetPlatformTarget(PlatformTarget.MSIL)
			.WithTarget("Build"));

		CreateAssemblyInfo(assemblyVersionFile, new AssemblyInfoSettings()
		{
			Version = assemblyVersion,
		});
	});

Task("Sign")
	.IsDependentOn("Build")
	.Does(() =>
	{
		if (configuration.Equals("Release"))
		{
			var files = GetFiles("./**/bin/" + configuration + "/**/ISI.*.dll");
			Sign(files, new SignToolSignSettings {
						TimeStampUri = new Uri("http://timestamp.digicert.com"),
						CertPath = "S:/ISI.CodeSign.pfx",
						Password = System.IO.File.ReadAllText("S:\\ISI.CodeSign.pwd")
			});
		}
	});

Task("Nuget")
	.IsDependentOn("Sign")
	.Does(() =>
	{
		foreach(var project in solution.Projects.Where(project => project.Path.FullPath.EndsWith(".csproj") && (project.Name.IndexOf(".Tests") < 0))
		{
			Information(project.Name);

			var nuspec = GenerateNuspecFromProject(project.Path, package =>
			{
				if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
				{
					return buildVersion;
				}

				return string.Empty;
			});
			nuspec.Version = buildVersion;
			nuspec.IconUri = new Uri(@"https://github.com/ISI-Extensions/ISI.Extensions/Lantern.png");
			nuspec.ProjectUri = new Uri(@"https://github.com/ISI-Extensions/ISI.Extensions");
			nuspec.Title = project.Name;
			nuspec.Description = project.Name;
			nuspec.Copyright = string.Format("Copyright (c) {0}, Integrated Solutions, Inc.", DateTime.Now.Year);
			nuspec.Authors = new [] { "Integrated Solutions, Inc." };
			nuspec.Owners = new [] { "Integrated Solutions, Inc." };

			var nuspecFile = File(project.Path.GetDirectory() + "/" + project.Name + ".nuspec");

			CreateNuspec(nuspec, nuspecFile);

			NuGetPack(project.Path.FullPath, new NuGetPackSettings()
			{
				Id = project.Name,
				Version = buildVersion, 
				Verbosity = NuGetVerbosity.Detailed,
				Properties = new Dictionary<string, string>
				{
					{ "Configuration", configuration }
				},
				NoPackageAnalysis = false,
				Symbols = false,
				OutputDirectory = nugetDirectory,
			});

			DeleteFile(nuspecFile);

			var nupgkFile = File(nugetDirectory + "/" + project.Name + "." + buildVersion + ".nupkg");
			NupkgSign(nupgkFile, new ISI.Cake.Addin.NupkgSignToolSettings()
			{
				CertificatePath = File("S:/ISI.CodeSign.pfx"),
				CertificatePassword = System.IO.File.ReadAllText("S:\\ISI.CodeSign.pwd"),
			});

			NupkgPush(nupgkFile, new ISI.Cake.Addin.NupkgPushToolSettings()
			{
				UseNugetPush = false,
				ApiKey = System.IO.File.ReadAllText("S:\\ISI.NugetApiKey.txt"),
			});
		}
	});

Task("Default")
	.IsDependentOn("Nuget")
	.Does(() => 
	{
		Information("No target provided. Starting default task");
	});

RunTarget(target);