#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/AspNetCoreLocalizer/bin");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/AspNetCoreLocalizer.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/AspNetCoreLocalizer.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./src/AspNetCoreLocalizer.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// PUSH NUGET PACKAGE
//////////////////////////////////////////////////////////////////////

Task("Package")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() => {

		var buildNo = BuildSystem.AppVeyor.Environment.Build.Number;
		var buildVersion = BuildSystem.AppVeyor.Environment.Build.Version;
		var buildId = BuildSystem.AppVeyor.Environment.Build.Id;

		NuGetPack("./AspNetCoreLocalizer.nuspec", 
					new NuGetPackSettings() {
                                     Version = buildVersion,
                                     Files = new [] { new NuSpecContent {Source = "./src/AspNetCoreLocalizer/bin/Release/netcoreapp2.0/AspNetCoreLocalizer.dll", Target = "lib/netcoreapp2.0"}, },
											});
				});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);